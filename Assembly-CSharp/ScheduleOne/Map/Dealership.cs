using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BE9 RID: 3049
	public class Dealership : MonoBehaviour
	{
		// Token: 0x0600557E RID: 21886 RVA: 0x00167A80 File Offset: 0x00165C80
		public void SpawnVehicle(string vehicleCode)
		{
			Transform transform = this.SpawnPoints[UnityEngine.Random.Range(0, this.SpawnPoints.Length)];
			NetworkSingleton<VehicleManager>.Instance.SpawnVehicle(vehicleCode, transform.position, transform.rotation, true);
		}

		// Token: 0x04003F6F RID: 16239
		public Transform[] SpawnPoints;
	}
}
