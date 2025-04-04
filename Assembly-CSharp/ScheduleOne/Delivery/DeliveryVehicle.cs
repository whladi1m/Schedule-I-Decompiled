using System;
using ScheduleOne.Map;
using ScheduleOne.Storage;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Delivery
{
	// Token: 0x02000708 RID: 1800
	[RequireComponent(typeof(LandVehicle))]
	public class DeliveryVehicle : MonoBehaviour
	{
		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x000CA9EC File Offset: 0x000C8BEC
		// (set) Token: 0x060030B3 RID: 12467 RVA: 0x000CA9F4 File Offset: 0x000C8BF4
		public LandVehicle Vehicle { get; private set; }

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060030B4 RID: 12468 RVA: 0x000CA9FD File Offset: 0x000C8BFD
		// (set) Token: 0x060030B5 RID: 12469 RVA: 0x000CAA05 File Offset: 0x000C8C05
		public DeliveryInstance ActiveDelivery { get; private set; }

		// Token: 0x060030B6 RID: 12470 RVA: 0x000CAA0E File Offset: 0x000C8C0E
		private void Awake()
		{
			this.Vehicle = base.GetComponent<LandVehicle>();
			this.Vehicle.SetGUID(new Guid(this.GUID));
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x000CAA34 File Offset: 0x000C8C34
		public void Activate(DeliveryInstance instance)
		{
			Console.Log("Activating delivery vehicle for delivery instance " + instance.DeliveryID, null);
			this.ActiveDelivery = instance;
			ParkingLot parking = instance.LoadingDock.Parking;
			instance.LoadingDock.SetStaticOccupant(this.Vehicle);
			this.Vehicle.Park(null, new ParkData
			{
				lotGUID = parking.GUID,
				spotIndex = 0,
				alignment = parking.ParkingSpots[0].Alignment
			}, false);
			this.Vehicle.SetVisible(true);
			this.Vehicle.Storage.AccessSettings = StorageEntity.EAccessSettings.Full;
			this.Vehicle.GetComponentInChildren<StorageDoorAnimation>().OverrideState(true);
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x000CAAE8 File Offset: 0x000C8CE8
		public void Deactivate()
		{
			Console.Log("Deactivating delivery vehicle", null);
			if (this.Vehicle != null)
			{
				this.Vehicle.ExitPark(false);
				this.Vehicle.SetIsStatic(true);
				this.Vehicle.SetVisible(false);
				this.Vehicle.SetTransform(new Vector3(0f, -100f, 0f), Quaternion.identity);
			}
			if (this.ActiveDelivery != null)
			{
				this.ActiveDelivery.LoadingDock.SetStaticOccupant(null);
				this.ActiveDelivery.LoadingDock.VehicleDetector.Clear();
			}
		}

		// Token: 0x040022DC RID: 8924
		public string GUID = string.Empty;
	}
}
