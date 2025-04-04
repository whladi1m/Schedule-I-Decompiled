using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.Management;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AF0 RID: 2800
	public class ObjectSelector : MonoBehaviour
	{
		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x0013A6A3 File Offset: 0x001388A3
		// (set) Token: 0x06004AE9 RID: 19177 RVA: 0x0013A6AB File Offset: 0x001388AB
		public bool IsOpen { get; protected set; }

		// Token: 0x06004AEA RID: 19178 RVA: 0x0013A6B4 File Offset: 0x001388B4
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 12);
			Singleton<ManagementClipboard>.Instance.onClipboardUnequipped.AddListener(new UnityAction(this.ClipboardClosed));
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x0013A6E4 File Offset: 0x001388E4
		public virtual void Open(string _selectionTitle, string instruction, int _maxSelectedObjects, List<BuildableItem> _selectedObjects, List<Type> _typeRequirements, Property property, ObjectSelector.ObjectFilter _objectFilter, Action<List<BuildableItem>> _callback, List<Transform> transitLineSources = null)
		{
			this.IsOpen = true;
			this.changesMade = false;
			this.targetProperty = property;
			this.selectionTitle = _selectionTitle;
			if (instruction != string.Empty)
			{
				Singleton<HUD>.Instance.ShowTopScreenText(instruction);
			}
			this.maxSelectedObjects = _maxSelectedObjects;
			this.selectedObjects = new List<BuildableItem>();
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

		// Token: 0x06004AEC RID: 19180 RVA: 0x0013A834 File Offset: 0x00138A34
		private void UpdateTransitLines()
		{
			float num = 1.5f;
			Vector3 destinationPosition = PlayerSingleton<PlayerCamera>.Instance.transform.position + PlayerSingleton<PlayerCamera>.Instance.transform.forward * num;
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(num, out raycastHit, this.DetectionMask, false, 0f))
			{
				destinationPosition = raycastHit.point;
			}
			for (int i = 0; i < this.transitSources.Count; i++)
			{
				this.transitLines[i].SetSourcePosition(this.transitSources[i].position);
				this.transitLines[i].SetDestinationPosition(destinationPosition);
			}
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x0013A8E0 File Offset: 0x00138AE0
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
			if (pushChanges)
			{
				this.callback(this.selectedObjects);
			}
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x0013A9E4 File Offset: 0x00138BE4
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

		// Token: 0x06004AEF RID: 19183 RVA: 0x0013AB81 File Offset: 0x00138D81
		private void LateUpdate()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.UpdateTransitLines();
		}

		// Token: 0x06004AF0 RID: 19184 RVA: 0x0013AB94 File Offset: 0x00138D94
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

		// Token: 0x06004AF1 RID: 19185 RVA: 0x0013AC08 File Offset: 0x00138E08
		private BuildableItem GetHoveredObject()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(5f, out raycastHit, this.DetectionMask, false, 0.1f))
			{
				return raycastHit.collider.GetComponentInParent<BuildableItem>();
			}
			return null;
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x0013AC44 File Offset: 0x00138E44
		public bool IsObjectTypeValid(BuildableItem obj, out string reason)
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
			if (this.targetProperty != null && obj.ParentProperty != this.targetProperty)
			{
				reason = "Wrong property";
				return false;
			}
			string text;
			if (this.objectFilter != null && !this.objectFilter(obj, out text))
			{
				reason = text;
				return false;
			}
			return true;
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x0013AD04 File Offset: 0x00138F04
		public void ObjectClicked(BuildableItem obj)
		{
			string text;
			if (!this.IsObjectTypeValid(obj, out text))
			{
				return;
			}
			this.changesMade = true;
			if (!this.selectedObjects.Contains(obj))
			{
				if (this.maxSelectedObjects == 1 && this.selectedObjects.Count == 1)
				{
					BuildableItem buildableItem = this.selectedObjects[0];
					this.selectedObjects.Remove(buildableItem);
					this.SetSelectionOutline(buildableItem, false);
				}
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

		// Token: 0x06004AF4 RID: 19188 RVA: 0x0013ADCD File Offset: 0x00138FCD
		private void SetSelectionOutline(BuildableItem obj, bool on)
		{
			if (obj.IsDestroyed)
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

		// Token: 0x06004AF5 RID: 19189 RVA: 0x0013ADEE File Offset: 0x00138FEE
		private void ClipboardClosed()
		{
			this.Close(false, false);
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0013ADF8 File Offset: 0x00138FF8
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

		// Token: 0x0400385E RID: 14430
		public const float SELECTION_RANGE = 5f;

		// Token: 0x04003860 RID: 14432
		[Header("Settings")]
		public LayerMask DetectionMask;

		// Token: 0x04003861 RID: 14433
		public Color HoverOutlineColor;

		// Token: 0x04003862 RID: 14434
		public Color SelectOutlineColor;

		// Token: 0x04003863 RID: 14435
		private int maxSelectedObjects;

		// Token: 0x04003864 RID: 14436
		private List<BuildableItem> selectedObjects = new List<BuildableItem>();

		// Token: 0x04003865 RID: 14437
		private List<Type> typeRequirements = new List<Type>();

		// Token: 0x04003866 RID: 14438
		private ObjectSelector.ObjectFilter objectFilter;

		// Token: 0x04003867 RID: 14439
		private Action<List<BuildableItem>> callback;

		// Token: 0x04003868 RID: 14440
		private BuildableItem hoveredObj;

		// Token: 0x04003869 RID: 14441
		private BuildableItem highlightedObj;

		// Token: 0x0400386A RID: 14442
		private string selectionTitle = "";

		// Token: 0x0400386B RID: 14443
		private bool changesMade;

		// Token: 0x0400386C RID: 14444
		private List<Transform> transitSources = new List<Transform>();

		// Token: 0x0400386D RID: 14445
		private List<TransitLineVisuals> transitLines = new List<TransitLineVisuals>();

		// Token: 0x0400386E RID: 14446
		private Property targetProperty;

		// Token: 0x02000AF1 RID: 2801
		// (Invoke) Token: 0x06004AF9 RID: 19193
		public delegate bool ObjectFilter(BuildableItem obj, out string reason);
	}
}
