using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AF6 RID: 2806
	public class TransitEntitySelector : MonoBehaviour
	{
		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06004B0B RID: 19211 RVA: 0x0013B25B File Offset: 0x0013945B
		// (set) Token: 0x06004B0C RID: 19212 RVA: 0x0013B263 File Offset: 0x00139463
		public bool IsOpen { get; protected set; }

		// Token: 0x06004B0D RID: 19213 RVA: 0x0013B26C File Offset: 0x0013946C
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 12);
			Singleton<ManagementClipboard>.Instance.onClipboardUnequipped.AddListener(new UnityAction(this.ClipboardClosed));
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x0013B29C File Offset: 0x0013949C
		public virtual void Open(string _selectionTitle, string instruction, int _maxSelectedObjects, List<ITransitEntity> _selectedObjects, List<Type> _typeRequirements, TransitEntitySelector.ObjectFilter _objectFilter, Action<List<ITransitEntity>> _callback, List<Transform> transitLineSources = null, bool selectingDestination = true)
		{
			this.IsOpen = true;
			this.changesMade = false;
			this.selectDestination = selectingDestination;
			this.selectionTitle = _selectionTitle;
			if (instruction != string.Empty)
			{
				Singleton<HUD>.Instance.ShowTopScreenText(instruction);
			}
			this.maxSelectedObjects = _maxSelectedObjects;
			this.selectedObjects = new List<ITransitEntity>();
			this.selectedObjects.AddRange(_selectedObjects);
			for (int i = 0; i < this.selectedObjects.Count; i++)
			{
				this.SetSelectionOutline(this.selectedObjects[i], true);
			}
			this.objectFilter = _objectFilter;
			this.typeRequirements = _typeRequirements;
			this.callback = _callback;
			this.UpdateInstructions();
			Singleton<ManagementInterface>.Instance.EquippedClipboard.OverrideClipboardText(this.selectionTitle);
			Singleton<ManagementClipboard>.Instance.Close(true);
			if (this.maxSelectedObjects == 1)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("objectselector");
			}
			else
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("objectselector_multi");
			}
			if (transitLineSources != null)
			{
				this.transitSources.Clear();
				this.transitSources.AddRange(transitLineSources);
				for (int j = 0; j < this.transitSources.Count; j++)
				{
					TransitLineVisuals item = UnityEngine.Object.Instantiate<TransitLineVisuals>(Singleton<ManagementWorldspaceCanvas>.Instance.TransitRouteVisualsPrefab, NetworkSingleton<GameManager>.Instance.Temp);
					this.transitLines.Add(item);
				}
				this.UpdateTransitLines();
			}
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x0013B3EC File Offset: 0x001395EC
		private void UpdateTransitLines()
		{
			float num = 1.5f;
			Vector3 vector = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * num;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(num, out raycastHit, this.DetectionMask, false, 0f))
			{
				vector = raycastHit.point;
			}
			for (int i = 0; i < this.transitSources.Count; i++)
			{
				if (this.selectDestination)
				{
					this.transitLines[i].SetSourcePosition(this.transitSources[i].position);
					this.transitLines[i].SetDestinationPosition(vector);
				}
				else
				{
					this.transitLines[i].SetSourcePosition(vector);
					this.transitLines[i].SetDestinationPosition(this.transitSources[i].position);
				}
			}
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x0013B4D8 File Offset: 0x001396D8
		public virtual void Close(bool returnToClipboard, bool pushChanges)
		{
			this.IsOpen = false;
			if (Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "npcselector" || Singleton<InputPromptsCanvas>.Instance.currentModuleLabel == "objectselector_multi")
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			for (int i = 0; i < this.selectedObjects.Count; i++)
			{
				this.SetSelectionOutline(this.selectedObjects[i], false);
			}
			Singleton<HUD>.Instance.HideTopScreenText();
			if (returnToClipboard)
			{
				Singleton<ManagementInterface>.Instance.EquippedClipboard.EndOverride();
				Singleton<ManagementClipboard>.Instance.Open(Singleton<ManagementInterface>.Instance.Configurables, Singleton<ManagementInterface>.Instance.EquippedClipboard);
			}
			for (int j = 0; j < this.transitLines.Count; j++)
			{
				UnityEngine.Object.Destroy(this.transitLines[j].gameObject);
			}
			this.transitLines.Clear();
			this.transitSources.Clear();
			this.objectFilter = null;
			if (pushChanges)
			{
				this.callback(this.selectedObjects);
			}
		}

		// Token: 0x06004B11 RID: 19217 RVA: 0x0013B5E4 File Offset: 0x001397E4
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.hoveredObj = this.GetHoveredObject();
			string empty = string.Empty;
			if (this.hoveredObj != null && this.IsObjectTypeValid(this.hoveredObj, out empty))
			{
				if (this.hoveredObj != this.highlightedObj && !this.selectedObjects.Contains(this.hoveredObj))
				{
					if (this.highlightedObj != null)
					{
						if (this.selectedObjects.Contains(this.highlightedObj))
						{
							this.highlightedObj.ShowOutline(this.SelectOutlineColor);
						}
						else
						{
							this.highlightedObj.HideOutline();
						}
						this.highlightedObj = null;
					}
					this.highlightedObj = this.hoveredObj;
					this.hoveredObj.ShowOutline(this.HoverOutlineColor);
				}
			}
			else
			{
				Singleton<HUD>.Instance.CrosshairText.Show(empty, new Color32(byte.MaxValue, 125, 125, byte.MaxValue));
				if (this.highlightedObj != null)
				{
					if (this.selectedObjects.Contains(this.highlightedObj))
					{
						this.highlightedObj.ShowOutline(this.SelectOutlineColor);
					}
					else
					{
						this.highlightedObj.HideOutline();
					}
					this.highlightedObj = null;
				}
			}
			this.UpdateInstructions();
			if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.hoveredObj != null && this.IsObjectTypeValid(this.hoveredObj, out empty))
			{
				this.ObjectClicked(this.hoveredObj);
			}
			if (GameInput.GetButtonDown(GameInput.ButtonCode.Submit) && this.maxSelectedObjects > 1)
			{
				this.Close(true, true);
			}
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x0013B764 File Offset: 0x00139964
		private void LateUpdate()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.UpdateTransitLines();
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x0013B778 File Offset: 0x00139978
		private void UpdateInstructions()
		{
			string text = this.selectionTitle;
			if (this.maxSelectedObjects > 1)
			{
				text = string.Concat(new string[]
				{
					text,
					" (",
					this.selectedObjects.Count.ToString(),
					"/",
					this.maxSelectedObjects.ToString(),
					")"
				});
			}
			Singleton<HUD>.Instance.ShowTopScreenText(text);
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x0013B7EC File Offset: 0x001399EC
		private ITransitEntity GetHoveredObject()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, this.DetectionMask, false, 0.1f))
			{
				return raycastHit.collider.GetComponentInParent<ITransitEntity>();
			}
			return null;
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x0013B828 File Offset: 0x00139A28
		public bool IsObjectTypeValid(ITransitEntity obj, out string reason)
		{
			reason = string.Empty;
			if (this.typeRequirements.Count > 0 && !this.typeRequirements.Contains(obj.GetType()))
			{
				bool flag = false;
				for (int i = 0; i < this.typeRequirements.Count; i++)
				{
					if (obj.GetType().IsAssignableFrom(this.typeRequirements[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					reason = "Does not match type requirement";
					return false;
				}
			}
			string text;
			if (this.objectFilter != null && !this.objectFilter(obj, out text))
			{
				reason = text;
				return false;
			}
			return true;
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x0013B8BC File Offset: 0x00139ABC
		public void ObjectClicked(ITransitEntity obj)
		{
			string text;
			if (!this.IsObjectTypeValid(obj, out text))
			{
				return;
			}
			this.changesMade = true;
			if (!this.selectedObjects.Contains(obj))
			{
				if (this.selectedObjects.Count < this.maxSelectedObjects)
				{
					this.selectedObjects.Add(obj);
					this.SetSelectionOutline(obj, true);
				}
			}
			else if (this.maxSelectedObjects > 1)
			{
				this.selectedObjects.Remove(obj);
				this.SetSelectionOutline(obj, false);
			}
			if (this.maxSelectedObjects == 1 || !GameInput.GetButton(GameInput.ButtonCode.Sprint))
			{
				this.Close(true, true);
				return;
			}
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x0013B94C File Offset: 0x00139B4C
		private void SetSelectionOutline(ITransitEntity obj, bool on)
		{
			if (obj == null)
			{
				return;
			}
			if (on)
			{
				obj.ShowOutline(this.SelectOutlineColor);
				return;
			}
			obj.HideOutline();
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x0013B968 File Offset: 0x00139B68
		private void ClipboardClosed()
		{
			this.Close(false, false);
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x0013B972 File Offset: 0x00139B72
		private void Exit(ExitAction exitAction)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (exitAction.used)
			{
				return;
			}
			if (exitAction.exitType == ExitType.Escape)
			{
				exitAction.used = true;
				this.Close(true, this.changesMade);
			}
		}

		// Token: 0x04003881 RID: 14465
		public const float SELECTION_RANGE = 5f;

		// Token: 0x04003883 RID: 14467
		[Header("Settings")]
		public LayerMask DetectionMask;

		// Token: 0x04003884 RID: 14468
		public Color HoverOutlineColor;

		// Token: 0x04003885 RID: 14469
		public Color SelectOutlineColor;

		// Token: 0x04003886 RID: 14470
		private int maxSelectedObjects;

		// Token: 0x04003887 RID: 14471
		private List<ITransitEntity> selectedObjects = new List<ITransitEntity>();

		// Token: 0x04003888 RID: 14472
		private List<Type> typeRequirements = new List<Type>();

		// Token: 0x04003889 RID: 14473
		private TransitEntitySelector.ObjectFilter objectFilter;

		// Token: 0x0400388A RID: 14474
		private Action<List<ITransitEntity>> callback;

		// Token: 0x0400388B RID: 14475
		private ITransitEntity hoveredObj;

		// Token: 0x0400388C RID: 14476
		private ITransitEntity highlightedObj;

		// Token: 0x0400388D RID: 14477
		private string selectionTitle = "";

		// Token: 0x0400388E RID: 14478
		private bool changesMade;

		// Token: 0x0400388F RID: 14479
		private List<Transform> transitSources = new List<Transform>();

		// Token: 0x04003890 RID: 14480
		private List<TransitLineVisuals> transitLines = new List<TransitLineVisuals>();

		// Token: 0x04003891 RID: 14481
		private bool selectDestination = true;

		// Token: 0x02000AF7 RID: 2807
		// (Invoke) Token: 0x06004B1C RID: 19228
		public delegate bool ObjectFilter(ITransitEntity obj, out string reason);
	}
}
