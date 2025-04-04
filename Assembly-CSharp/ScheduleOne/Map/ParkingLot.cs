using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BFD RID: 3069
	public class ParkingLot : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x060055D5 RID: 21973 RVA: 0x00168B7C File Offset: 0x00166D7C
		// (set) Token: 0x060055D6 RID: 21974 RVA: 0x00168B84 File Offset: 0x00166D84
		public Guid GUID { get; protected set; }

		// Token: 0x060055D7 RID: 21975 RVA: 0x00168B90 File Offset: 0x00166D90
		private void Awake()
		{
			if (this.ExitPoint != null && this.ExitPointVehicleDetector == null)
			{
				Console.LogWarning("ExitPoint specified but no ExitPointVehicleDetector!", null);
			}
			if (!GUIDManager.IsGUIDValid(this.BakedGUID))
			{
				Console.LogError(base.gameObject.name + "'s baked GUID is not valid! Bad.", null);
			}
			this.GUID = new Guid(this.BakedGUID);
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060055D8 RID: 21976 RVA: 0x00168C03 File Offset: 0x00166E03
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x060055D9 RID: 21977 RVA: 0x00168C14 File Offset: 0x00166E14
		public ParkingSpot GetRandomFreeSpot()
		{
			List<ParkingSpot> freeParkingSpots = this.GetFreeParkingSpots();
			if (freeParkingSpots.Count == 0)
			{
				Console.Log("No free parking spots in " + base.gameObject.name + "!", null);
				return null;
			}
			return freeParkingSpots[UnityEngine.Random.Range(0, freeParkingSpots.Count)];
		}

		// Token: 0x060055DA RID: 21978 RVA: 0x00168C64 File Offset: 0x00166E64
		public int GetRandomFreeSpotIndex()
		{
			List<ParkingSpot> freeParkingSpots = this.GetFreeParkingSpots();
			if (freeParkingSpots.Count == 0)
			{
				return -1;
			}
			return this.ParkingSpots.IndexOf(freeParkingSpots[UnityEngine.Random.Range(0, freeParkingSpots.Count)]);
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x00168CA0 File Offset: 0x00166EA0
		public List<ParkingSpot> GetFreeParkingSpots()
		{
			if (this.ParkingSpots == null || this.ParkingSpots.Count == 0)
			{
				return new List<ParkingSpot>();
			}
			return (from x in this.ParkingSpots
			where x != null && x.OccupantVehicle == null
			select x).ToList<ParkingSpot>();
		}

		// Token: 0x04003FBE RID: 16318
		[SerializeField]
		protected string BakedGUID = string.Empty;

		// Token: 0x04003FC0 RID: 16320
		[Header("READONLY")]
		public List<ParkingSpot> ParkingSpots = new List<ParkingSpot>();

		// Token: 0x04003FC1 RID: 16321
		[Header("Entry")]
		public Transform EntryPoint;

		// Token: 0x04003FC2 RID: 16322
		public Transform HiddenVehicleAccessPoint;

		// Token: 0x04003FC3 RID: 16323
		[Header("Exit")]
		public bool UseExitPoint;

		// Token: 0x04003FC4 RID: 16324
		public EParkingAlignment ExitAlignment = EParkingAlignment.RearToKerb;

		// Token: 0x04003FC5 RID: 16325
		public Transform ExitPoint;

		// Token: 0x04003FC6 RID: 16326
		public VehicleDetector ExitPointVehicleDetector;
	}
}
