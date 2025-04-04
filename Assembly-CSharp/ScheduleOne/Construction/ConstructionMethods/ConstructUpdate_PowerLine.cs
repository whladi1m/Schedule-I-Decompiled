using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property.Utilities.Power;
using ScheduleOne.UI;
using ScheduleOne.UI.Construction;
using UnityEngine;

namespace ScheduleOne.Construction.ConstructionMethods
{
	// Token: 0x02000720 RID: 1824
	public class ConstructUpdate_PowerLine : ConstructUpdate_Base
	{
		// Token: 0x06003160 RID: 12640 RVA: 0x000CCAF0 File Offset: 0x000CACF0
		protected virtual void Start()
		{
			this.tempPowerLineContainer = new GameObject("TempPowerLine").transform;
			this.tempPowerLineContainer.SetParent(base.transform);
			for (int i = 0; i < PowerLine.powerLine_MaxSegments; i++)
			{
				Transform transform = UnityEngine.Object.Instantiate<GameObject>(Singleton<PowerManager>.Instance.powerLineSegmentPrefab, this.tempPowerLineContainer).transform;
				transform.Find("Model").GetComponent<MeshRenderer>().material = this.ghostPowerLine_Material;
				transform.gameObject.SetActive(false);
				this.tempSegments.Add(transform);
			}
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 5);
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x000CCB93 File Offset: 0x000CAD93
		public override void ConstructionStop()
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
			Singleton<HUD>.Instance.HideTopScreenText();
			base.ConstructionStop();
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x000CCBB6 File Offset: 0x000CADB6
		public void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (this.startNode != null)
			{
				exit.used = true;
				this.StopCreatingPowerLine();
			}
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x000CCBDC File Offset: 0x000CADDC
		protected override void Update()
		{
			base.Update();
			this.cosmeticPowerNode.SetActive(false);
			this.hoveredPowerNode = this.GetHoveredPowerNode();
			if (this.startNode == null)
			{
				Singleton<HUD>.Instance.ShowTopScreenText("Choose start point");
				if (this.hoveredPowerNode != null)
				{
					this.cosmeticPowerNode.transform.position = this.hoveredPowerNode.transform.position;
					this.cosmeticPowerNode.transform.rotation = this.hoveredPowerNode.transform.rotation;
					this.cosmeticPowerNode.gameObject.SetActive(true);
					if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
					{
						this.startNode = this.hoveredPowerNode;
						this.powerLineInitialDistance = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, this.startNode.pConnectionPoint.transform.position);
						return;
					}
				}
			}
			else
			{
				Singleton<HUD>.Instance.ShowTopScreenText("Choose end point");
				if (this.hoveredPowerNode != null && PowerLine.CanNodesBeConnected(this.startNode, this.hoveredPowerNode))
				{
					this.cosmeticPowerNode.transform.position = this.hoveredPowerNode.transform.position;
					this.cosmeticPowerNode.transform.rotation = this.hoveredPowerNode.transform.rotation;
					this.cosmeticPowerNode.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x000CCD54 File Offset: 0x000CAF54
		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (this.startNode != null)
			{
				Vector3 position = this.startNode.pConnectionPoint.transform.position;
				Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.powerLineInitialDistance));
				RaycastHit raycastHit;
				if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(this.powerLineInitialDistance, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f))
				{
					vector = raycastHit.point;
				}
				Vector3 vector2 = vector - position;
				vector2 = Vector3.ClampMagnitude(vector2, PowerLine.maxLineLength);
				vector = position + vector2;
				PowerNode powerNode = this.GetHoveredPowerNode();
				if (powerNode != null && PowerLine.CanNodesBeConnected(this.startNode, powerNode))
				{
					vector = this.GetHoveredPowerNode().pConnectionPoint.transform.position;
				}
				int segmentCount = PowerLine.GetSegmentCount(position, vector);
				List<Transform> list = new List<Transform>();
				for (int i = 0; i < this.tempSegments.Count; i++)
				{
					if (i < segmentCount)
					{
						this.tempSegments[i].gameObject.SetActive(true);
						list.Add(this.tempSegments[i]);
					}
					else
					{
						this.tempSegments[i].gameObject.SetActive(false);
					}
				}
				PowerLine.DrawPowerLine(position, vector, list, 1.002f);
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && powerNode != null && PowerLine.CanNodesBeConnected(this.startNode, powerNode) && NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= Singleton<ConstructionMenu>.Instance.GetListingPrice("Utilities/PowerLine/PowerLine"))
				{
					this.CompletePowerLine(powerNode);
				}
			}
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x000CCF10 File Offset: 0x000CB110
		protected PowerNode GetHoveredPowerNode()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.MouseRaycast(200f, out raycastHit, 1 << LayerMask.NameToLayer("Default"), true, 0f) && raycastHit.collider.GetComponentInParent<PowerNodeTag>())
			{
				return raycastHit.collider.GetComponentInParent<PowerNodeTag>().powerNode;
			}
			return null;
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x000CCF70 File Offset: 0x000CB170
		private void CompletePowerLine(PowerNode target)
		{
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Power Line", -Singleton<ConstructionMenu>.Instance.GetListingPrice("Utilities/PowerLine/PowerLine"), 1f, string.Empty);
			PowerLine c = Singleton<PowerManager>.Instance.CreatePowerLine(this.startNode, target, Singleton<ConstructionManager>.Instance.currentProperty);
			if (Singleton<ConstructionManager>.Instance.onNewConstructableBuilt != null)
			{
				Singleton<ConstructionManager>.Instance.onNewConstructableBuilt(c);
			}
			this.StopCreatingPowerLine();
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.startNode = target;
				if (this.startNode != null)
				{
					this.powerLineInitialDistance = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, target.pConnectionPoint.transform.position);
				}
			}
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x000CD030 File Offset: 0x000CB230
		private void StopCreatingPowerLine()
		{
			this.startNode = null;
			for (int i = 0; i < this.tempSegments.Count; i++)
			{
				this.tempSegments[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x04002343 RID: 9027
		[Header("Settings")]
		[SerializeField]
		protected Material ghostPowerLine_Material;

		// Token: 0x04002344 RID: 9028
		[Header("References")]
		[SerializeField]
		protected GameObject cosmeticPowerNode;

		// Token: 0x04002345 RID: 9029
		protected Transform tempPowerLineContainer;

		// Token: 0x04002346 RID: 9030
		protected PowerNode hoveredPowerNode;

		// Token: 0x04002347 RID: 9031
		protected List<Transform> tempSegments = new List<Transform>();

		// Token: 0x04002348 RID: 9032
		protected PowerNode startNode;

		// Token: 0x04002349 RID: 9033
		protected float powerLineInitialDistance;
	}
}
