using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006EC RID: 1772
	public class VehicleSelector : Singleton<VehicleSelector>
	{
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x000C878F File Offset: 0x000C698F
		// (set) Token: 0x06003027 RID: 12327 RVA: 0x000C8797 File Offset: 0x000C6997
		public bool isSelecting { get; protected set; }

		// Token: 0x06003028 RID: 12328 RVA: 0x000C87A0 File Offset: 0x000C69A0
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 8);
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000C87BC File Offset: 0x000C69BC
		protected virtual void Update()
		{
			if (this.isSelecting)
			{
				this.hoveredVehicle = this.GetHoveredVehicle();
				if (this.hoveredVehicle != null)
				{
					Singleton<HUD>.Instance.ShowRadialIndicator(1f);
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) && this.hoveredVehicle != null && (this.vehicleFilter == null || this.vehicleFilter(this.hoveredVehicle)))
				{
					if (this.selectedVehicles.Contains(this.hoveredVehicle))
					{
						Console.Log("Deselected: " + this.hoveredVehicle.VehicleName, null);
						this.selectedVehicles.Remove(this.hoveredVehicle);
						return;
					}
					if (this.selectedVehicles.Count < this.selectionLimit)
					{
						this.selectedVehicles.Add(this.hoveredVehicle);
						if (this.selectedVehicles.Count >= this.selectionLimit && this.exitOnSelectionLimit)
						{
							this.StopSelecting();
						}
					}
				}
			}
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x000C88C0 File Offset: 0x000C6AC0
		protected virtual void LateUpdate()
		{
			if (this.isSelecting)
			{
				for (int i = 0; i < this.outlinedVehicles.Count; i++)
				{
					this.outlinedVehicles[i].HideOutline();
				}
				this.outlinedVehicles.Clear();
				for (int j = 0; j < this.selectedVehicles.Count; j++)
				{
					this.selectedVehicles[j].ShowOutline(BuildableItem.EOutlineColor.Blue);
					this.outlinedVehicles.Add(this.selectedVehicles[j]);
				}
				if (this.hoveredVehicle != null)
				{
					if (this.selectedVehicles.Contains(this.hoveredVehicle))
					{
						this.hoveredVehicle.ShowOutline(BuildableItem.EOutlineColor.LightBlue);
						return;
					}
					this.hoveredVehicle.ShowOutline(BuildableItem.EOutlineColor.White);
					this.outlinedVehicles.Add(this.hoveredVehicle);
				}
			}
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x000C8994 File Offset: 0x000C6B94
		private LandVehicle GetHoveredVehicle()
		{
			RaycastHit raycastHit;
			if (PlayerSingleton<PlayerCamera>.Instance.LookRaycast(this.detectionRange, out raycastHit, this.detectionMask, false, 0.1f))
			{
				LandVehicle componentInParent = raycastHit.collider.GetComponentInParent<LandVehicle>();
				if (componentInParent != null && (this.vehicleFilter == null || this.vehicleFilter(componentInParent)))
				{
					return componentInParent;
				}
			}
			return null;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000C89F0 File Offset: 0x000C6BF0
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

		// Token: 0x0600302D RID: 12333 RVA: 0x000C8A1C File Offset: 0x000C6C1C
		public void StartSelecting(string selectionTitle, ref List<LandVehicle> initialSelection, int _selectionLimit, bool _exitOnSelectionLimit, Func<LandVehicle, bool> filter = null)
		{
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			this.selectedVehicles = initialSelection;
			for (int i = 0; i < this.selectedVehicles.Count; i++)
			{
				this.selectedVehicles[i].ShowOutline(BuildableItem.EOutlineColor.White);
				this.outlinedVehicles.Add(this.selectedVehicles[i]);
			}
			this.selectionLimit = _selectionLimit;
			this.vehicleFilter = filter;
			Singleton<HUD>.Instance.ShowTopScreenText(selectionTitle);
			this.isSelecting = true;
			this.exitOnSelectionLimit = _exitOnSelectionLimit;
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000C8AAC File Offset: 0x000C6CAC
		public void StopSelecting()
		{
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			this.vehicleFilter = null;
			for (int i = 0; i < this.outlinedVehicles.Count; i++)
			{
				this.outlinedVehicles[i].HideOutline();
			}
			this.outlinedVehicles.Clear();
			if (this.onClose != null)
			{
				this.onClose();
			}
			Singleton<HUD>.Instance.HideTopScreenText();
			this.isSelecting = false;
		}

		// Token: 0x0400225A RID: 8794
		[Header("Settings")]
		[SerializeField]
		protected float detectionRange = 5f;

		// Token: 0x0400225B RID: 8795
		[SerializeField]
		protected LayerMask detectionMask;

		// Token: 0x0400225D RID: 8797
		private List<LandVehicle> selectedVehicles = new List<LandVehicle>();

		// Token: 0x0400225E RID: 8798
		public Action onClose;

		// Token: 0x0400225F RID: 8799
		private int selectionLimit;

		// Token: 0x04002260 RID: 8800
		private bool exitOnSelectionLimit;

		// Token: 0x04002261 RID: 8801
		private LandVehicle hoveredVehicle;

		// Token: 0x04002262 RID: 8802
		private List<LandVehicle> outlinedVehicles = new List<LandVehicle>();

		// Token: 0x04002263 RID: 8803
		private Func<LandVehicle, bool> vehicleFilter;
	}
}
