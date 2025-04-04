using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x0200067C RID: 1660
	public class SensorRollerDoors : RollerDoor
	{
		// Token: 0x06002E10 RID: 11792 RVA: 0x000C1480 File Offset: 0x000BF680
		protected virtual void Update()
		{
			if (!this.CanOpen())
			{
				if (base.IsOpen)
				{
					base.Close();
				}
				return;
			}
			if (this.Detector.vehicles.Count <= 0)
			{
				base.Close();
				return;
			}
			if (!this.DetectPlayerOccupiedVehiclesOnly || this.ClipDetector.vehicles.Count > 0)
			{
				base.Open();
				return;
			}
			for (int i = 0; i < this.Detector.vehicles.Count; i++)
			{
				if (this.Detector.vehicles[i].DriverPlayer != null)
				{
					base.Open();
					return;
				}
			}
			base.Close();
		}

		// Token: 0x040020D4 RID: 8404
		[Header("References")]
		public VehicleDetector Detector;

		// Token: 0x040020D5 RID: 8405
		public VehicleDetector ClipDetector;

		// Token: 0x040020D6 RID: 8406
		[Header("Settings")]
		public bool DetectPlayerOccupiedVehiclesOnly = true;
	}
}
