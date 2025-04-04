using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Trash;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.PlayerTasks.Tasks
{
	// Token: 0x02000358 RID: 856
	public class PourIntoPotTask : Task
	{
		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001352 RID: 4946 RVA: 0x00055F64 File Offset: 0x00054164
		// (set) Token: 0x06001353 RID: 4947 RVA: 0x00055F6C File Offset: 0x0005416C
		public override string TaskName { get; protected set; } = "Pour";

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001354 RID: 4948 RVA: 0x00055F75 File Offset: 0x00054175
		protected virtual bool UseCoverage { get; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x00055F7D File Offset: 0x0005417D
		protected virtual bool FailOnEmpty { get; } = 1;

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x00055F85 File Offset: 0x00054185
		protected virtual Pot.ECameraPosition CameraPosition { get; } = 1;

		// Token: 0x06001357 RID: 4951 RVA: 0x00055F90 File Offset: 0x00054190
		public PourIntoPotTask(Pot _pot, ItemInstance _itemInstance, Pourable _pourablePrefab)
		{
			if (_pot == null)
			{
				Console.LogWarning("PourIntoPotTask: pot null", null);
				this.StopTask();
				return;
			}
			if (_pourablePrefab == null)
			{
				Console.LogWarning("PourIntoPotTask: pourablePrefab null", null);
				this.StopTask();
				return;
			}
			this.ClickDetectionEnabled = true;
			this.item = _itemInstance;
			this.pot = _pot;
			if (this.pot.Plant != null)
			{
				this.pot.Plant.SetVisible(false);
			}
			this.pot.SetPlayerUser(Player.Local.NetworkObject);
			this.pot.PositionCameraContainer();
			Transform cameraPosition = this.pot.GetCameraPosition(this.CameraPosition);
			PlayerSingleton<PlayerCamera>.Instance.OverrideTransform(cameraPosition.position, cameraPosition.rotation, 0.25f, false);
			PlayerSingleton<PlayerCamera>.Instance.OverrideFOV(70f, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.pourable = UnityEngine.Object.Instantiate<GameObject>(_pourablePrefab.gameObject, NetworkSingleton<GameManager>.Instance.Temp).GetComponent<Pourable>();
			this.pourable.transform.position = this.pot.PourableStartPoint.position;
			this.pourable.Rb.position = this.pot.PourableStartPoint.position;
			this.pourable.Origin = this.pot.PourableStartPoint.position;
			this.pourable.MaxDistanceFromOrigin = 0.5f;
			this.pourable.LocationRestrictionEnabled = true;
			this.pourable.TargetPot = _pot;
			Pourable pourable = this.pourable;
			pourable.onInitialPour = (Action)Delegate.Combine(pourable.onInitialPour, new Action(this.OnInitialPour));
			Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.transform.position - this.pourable.transform.position;
			this.pourable.transform.rotation = Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z), Vector3.up);
			this.pourable.Rb.rotation = Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z), Vector3.up);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("pourable");
			if (this.UseCoverage)
			{
				this.pot.SoilCover.Reset();
				this.pot.SoilCover.gameObject.SetActive(true);
				this.pot.SoilCover.onSufficientCoverage.AddListener(new UnityAction(this.FullyCovered));
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0005625B File Offset: 0x0005445B
		public override void Update()
		{
			base.Update();
			if (this.FailOnEmpty && this.pourable.currentQuantity <= 0f)
			{
				this.Fail();
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00056284 File Offset: 0x00054484
		public override void StopTask()
		{
			base.StopTask();
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0.15f, true, true);
			PlayerSingleton<PlayerCamera>.Instance.StopFOVOverride(0.15f);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			UnityEngine.Object.Destroy(this.pourable.gameObject);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			if (this.UseCoverage)
			{
				this.pot.SoilCover.onSufficientCoverage.RemoveListener(new UnityAction(this.FullyCovered));
				this.pot.SoilCover.gameObject.SetActive(false);
			}
			if (this.pot.Plant != null)
			{
				this.pot.Plant.SetVisible(true);
			}
			this.pot.SetPlayerUser(null);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00056361 File Offset: 0x00054561
		private void OnInitialPour()
		{
			if (this.removeItemAfterInitialPour)
			{
				this.RemoveItem();
			}
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00056374 File Offset: 0x00054574
		protected void RemoveItem()
		{
			PlayerSingleton<PlayerInventory>.Instance.RemoveAmountOfItem(this.item.ID, 1U);
			if (this.pourable.TrashItem != null)
			{
				NetworkSingleton<TrashManager>.Instance.CreateTrashItem(this.pourable.TrashItem.ID, Player.Local.Avatar.transform.position + Vector3.up * 0.3f, UnityEngine.Random.rotation, default(Vector3), "", false);
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void FullyCovered()
		{
		}

		// Token: 0x0400128B RID: 4747
		protected Pot pot;

		// Token: 0x0400128C RID: 4748
		protected ItemInstance item;

		// Token: 0x0400128D RID: 4749
		protected Pourable pourable;

		// Token: 0x04001291 RID: 4753
		protected bool removeItemAfterInitialPour;
	}
}
