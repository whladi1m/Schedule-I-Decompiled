using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A31 RID: 2609
	public class VehicleCanvas : Singleton<VehicleCanvas>
	{
		// Token: 0x0600464F RID: 17999 RVA: 0x001264D8 File Offset: 0x001246D8
		protected override void Start()
		{
			base.Start();
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.Subscribe));
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x00126500 File Offset: 0x00124700
		private void Subscribe()
		{
			Player local = Player.Local;
			local.onEnterVehicle = (Player.VehicleEvent)Delegate.Combine(local.onEnterVehicle, new Player.VehicleEvent(this.VehicleEntered));
			Player local2 = Player.Local;
			local2.onExitVehicle = (Player.VehicleTransformEvent)Delegate.Combine(local2.onExitVehicle, new Player.VehicleTransformEvent(this.VehicleExited));
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x00126559 File Offset: 0x00124759
		private void Update()
		{
			if (Player.Local == null)
			{
				return;
			}
			if (Player.Local.CurrentVehicle != null)
			{
				this.Canvas.enabled = !Singleton<GameplayMenu>.Instance.IsOpen;
			}
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x00126593 File Offset: 0x00124793
		private void LateUpdate()
		{
			if (this.currentVehicle != null)
			{
				this.UpdateSpeedText();
			}
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x001265A9 File Offset: 0x001247A9
		private void VehicleEntered(LandVehicle veh)
		{
			this.currentVehicle = veh;
			this.UpdateSpeedText();
			this.Canvas.enabled = true;
			this.DriverPromptsContainer.SetActive(this.currentVehicle.localPlayerIsDriver);
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x001265DA File Offset: 0x001247DA
		private void VehicleExited(LandVehicle veh, Transform exitPoint)
		{
			this.Canvas.enabled = false;
			this.currentVehicle = null;
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x001265F0 File Offset: 0x001247F0
		private void UpdateSpeedText()
		{
			if (this.SpeedText == null)
			{
				return;
			}
			if (Singleton<Settings>.Instance.unitType == Settings.UnitType.Metric)
			{
				this.SpeedText.text = Mathf.Abs(this.currentVehicle.VelocityCalculator.Velocity.magnitude * 3.6f * 1.4f).ToString("0") + " km/h";
				return;
			}
			this.SpeedText.text = Mathf.Abs(this.currentVehicle.VelocityCalculator.Velocity.magnitude * 2.23694f * 1.4f).ToString("0") + " mph";
		}

		// Token: 0x04003410 RID: 13328
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003411 RID: 13329
		public TextMeshProUGUI SpeedText;

		// Token: 0x04003412 RID: 13330
		public GameObject DriverPromptsContainer;

		// Token: 0x04003413 RID: 13331
		private LandVehicle currentVehicle;
	}
}
