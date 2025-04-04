using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.EntityFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D7 RID: 1751
	public class ObjectSelector : Singleton<ObjectSelector>
	{
		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002FB5 RID: 12213 RVA: 0x000C68C4 File Offset: 0x000C4AC4
		// (set) Token: 0x06002FB6 RID: 12214 RVA: 0x000C68CC File Offset: 0x000C4ACC
		public bool isSelecting { get; protected set; }

		// Token: 0x06002FB7 RID: 12215 RVA: 0x000C68D5 File Offset: 0x000C4AD5
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x000C68F0 File Offset: 0x000C4AF0
		protected virtual void Update()
		{
			if (this.isSelecting)
			{
				this.hoveredBuildable = this.GetHoveredBuildable();
				this.hoveredConstructable = this.GetHoveredConstructable();
				if (this.hoveredBuildable != null || this.hoveredConstructable != null)
				{
					Singleton<HUD>.Instance.ShowRadialIndicator(1f);
				}
				RaycastHit raycastHit;
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, true, 0f))
				{
					if (raycastHit.collider.GetComponentInParent<BuildableItem>())
					{
						BuildableItem componentInParent = raycastHit.collider.GetComponentInParent<BuildableItem>();
						if (this.allowedTypes.Contains(componentInParent.GetType()))
						{
							if (this.selectedObjects.Contains(componentInParent))
							{
								Console.Log("Deselected: " + componentInParent.ItemInstance.Name, null);
								this.selectedObjects.Remove(componentInParent);
								return;
							}
							if (this.selectedObjects.Count + this.selectedConstructables.Count < this.selectionLimit)
							{
								Console.Log("Selected: " + componentInParent.ItemInstance.Name, null);
								this.selectedObjects.Add(componentInParent);
								if (this.selectedObjects.Count + this.selectedConstructables.Count >= this.selectionLimit && this.exitOnSelectionLimit)
								{
									this.StopSelecting();
									return;
								}
							}
						}
					}
					else if (raycastHit.collider.GetComponentInParent<Constructable>())
					{
						Constructable componentInParent2 = raycastHit.collider.GetComponentInParent<Constructable>();
						if (this.allowedTypes.Contains(componentInParent2.GetType()))
						{
							if (this.selectedConstructables.Contains(componentInParent2))
							{
								Console.Log("Deselected: " + componentInParent2.ConstructableName, null);
								this.selectedConstructables.Remove(componentInParent2);
								return;
							}
							if (this.selectedObjects.Count + this.selectedConstructables.Count < this.selectionLimit)
							{
								Console.Log("Selected: " + componentInParent2.ConstructableName, null);
								this.selectedConstructables.Add(componentInParent2);
								if (this.selectedObjects.Count + this.selectedConstructables.Count >= this.selectionLimit && this.exitOnSelectionLimit)
								{
									this.StopSelecting();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x000C6B40 File Offset: 0x000C4D40
		protected virtual void LateUpdate()
		{
			if (this.isSelecting)
			{
				for (int i = 0; i < this.outlinedObjects.Count; i++)
				{
					this.outlinedObjects[i].HideOutline();
				}
				for (int j = 0; j < this.outlinedConstructables.Count; j++)
				{
					this.outlinedConstructables[j].HideOutline();
				}
				this.outlinedObjects.Clear();
				this.outlinedConstructables.Clear();
				for (int k = 0; k < this.selectedConstructables.Count; k++)
				{
					this.selectedConstructables[k].ShowOutline(BuildableItem.EOutlineColor.Blue);
					this.outlinedConstructables.Add(this.selectedConstructables[k]);
				}
				for (int l = 0; l < this.selectedObjects.Count; l++)
				{
					this.selectedObjects[l].ShowOutline(BuildableItem.EOutlineColor.Blue);
					this.outlinedObjects.Add(this.selectedObjects[l]);
				}
				if (this.hoveredBuildable != null)
				{
					if (this.selectedObjects.Contains(this.hoveredBuildable))
					{
						this.hoveredBuildable.ShowOutline(BuildableItem.EOutlineColor.LightBlue);
					}
					else
					{
						this.hoveredBuildable.ShowOutline(BuildableItem.EOutlineColor.White);
						this.outlinedObjects.Add(this.hoveredBuildable);
					}
				}
				if (this.hoveredConstructable != null)
				{
					if (this.selectedConstructables.Contains(this.hoveredConstructable))
					{
						this.hoveredConstructable.ShowOutline(BuildableItem.EOutlineColor.LightBlue);
						return;
					}
					this.hoveredConstructable.ShowOutline(BuildableItem.EOutlineColor.White);
					this.outlinedConstructables.Add(this.hoveredConstructable);
				}
			}
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000C6CD4 File Offset: 0x000C4ED4
		private BuildableItem GetHoveredBuildable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, false, 0.1f))
			{
				BuildableItem componentInParent = raycastHit.collider.GetComponentInParent<BuildableItem>();
				if (componentInParent != null && this.allowedTypes.Contains(componentInParent.GetType()))
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000C6D30 File Offset: 0x000C4F30
		private Constructable GetHoveredConstructable()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, false, 0.1f))
			{
				Constructable componentInParent = raycastHit.collider.GetComponentInParent<Constructable>();
				if (componentInParent != null && this.allowedTypes.Contains(componentInParent.GetType()))
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000C6D89 File Offset: 0x000C4F89
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (action.exitType == ExitType.Escape && this.isSelecting)
			{
				action.used = true;
				this.StopSelecting();
			}
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x000C6DB4 File Offset: 0x000C4FB4
		public void StartSelecting(string selectionTitle, List<Type> _typeRestriction, ref List<BuildableItem> initialSelection_Objects, ref List<Constructable> initalSelection_Constructables, int _selectionLimit, bool _exitOnSelectionLimit)
		{
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.allowedTypes = _typeRestriction;
			this.selectedObjects = initialSelection_Objects;
			this.selectedConstructables = initalSelection_Constructables;
			for (int i = 0; i < this.selectedObjects.Count; i++)
			{
				this.selectedObjects[i].ShowOutline(BuildableItem.EOutlineColor.White);
				this.outlinedObjects.Add(this.selectedObjects[i]);
			}
			for (int j = 0; j < this.selectedConstructables.Count; j++)
			{
				this.selectedConstructables[j].ShowOutline(BuildableItem.EOutlineColor.White);
				this.outlinedConstructables.Add(this.selectedConstructables[j]);
			}
			this.selectionLimit = _selectionLimit;
			Singleton<HUD>.Instance.ShowTopScreenText(selectionTitle);
			this.isSelecting = true;
			this.exitOnSelectionLimit = _exitOnSelectionLimit;
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000C6E8C File Offset: 0x000C508C
		public void StopSelecting()
		{
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.allowedTypes = null;
			for (int i = 0; i < this.outlinedObjects.Count; i++)
			{
				this.outlinedObjects[i].HideOutline();
			}
			for (int j = 0; j < this.outlinedConstructables.Count; j++)
			{
				this.outlinedConstructables[j].HideOutline();
			}
			this.outlinedObjects.Clear();
			this.outlinedConstructables.Clear();
			if (this.onClose != null)
			{
				this.onClose();
			}
			Singleton<HUD>.Instance.HideTopScreenText();
			this.isSelecting = false;
		}

		// Token: 0x04002202 RID: 8706
		[Header("Settings")]
		[SerializeField]
		protected float detectionRange = 5f;

		// Token: 0x04002203 RID: 8707
		[SerializeField]
		protected LayerMask detectionMask;

		// Token: 0x04002205 RID: 8709
		private List<Type> allowedTypes;

		// Token: 0x04002206 RID: 8710
		private List<BuildableItem> selectedObjects = new List<BuildableItem>();

		// Token: 0x04002207 RID: 8711
		private List<Constructable> selectedConstructables = new List<Constructable>();

		// Token: 0x04002208 RID: 8712
		public Action onClose;

		// Token: 0x04002209 RID: 8713
		private int selectionLimit;

		// Token: 0x0400220A RID: 8714
		private bool exitOnSelectionLimit;

		// Token: 0x0400220B RID: 8715
		private BuildableItem hoveredBuildable;

		// Token: 0x0400220C RID: 8716
		private Constructable hoveredConstructable;

		// Token: 0x0400220D RID: 8717
		private List<BuildableItem> outlinedObjects = new List<BuildableItem>();

		// Token: 0x0400220E RID: 8718
		private List<Constructable> outlinedConstructables = new List<Constructable>();
	}
}
