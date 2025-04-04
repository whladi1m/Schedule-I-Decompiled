using System;
using FishNet.Object;
using ScheduleOne.Map;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007BE RID: 1982
	[RequireComponent(typeof(LandVehicle))]
	public class VehicleInitializer : NetworkBehaviour
	{
		// Token: 0x06003638 RID: 13880 RVA: 0x000E4538 File Offset: 0x000E2738
		public override void OnStartServer()
		{
			base.OnStartServer();
			if (this.InitialParkingLot != null && !base.GetComponent<LandVehicle>().isParked)
			{
				int randomFreeSpotIndex = this.InitialParkingLot.GetRandomFreeSpotIndex();
				if (randomFreeSpotIndex != -1)
				{
					EParkingAlignment alignment = this.InitialParkingLot.ParkingSpots[randomFreeSpotIndex].Alignment;
				}
			}
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000E458D File Offset: 0x000E278D
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleInitializerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleInitializerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000E45A0 File Offset: 0x000E27A0
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleInitializerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleInitializerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000E45B3 File Offset: 0x000E27B3
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000E45B3 File Offset: 0x000E27B3
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002708 RID: 9992
		public ParkingLot InitialParkingLot;

		// Token: 0x04002709 RID: 9993
		private bool dll_Excuted;

		// Token: 0x0400270A RID: 9994
		private bool dll_Excuted;
	}
}
