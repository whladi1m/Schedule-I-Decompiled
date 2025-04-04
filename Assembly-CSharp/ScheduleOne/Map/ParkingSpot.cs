using System;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Map
{
	// Token: 0x02000BFF RID: 3071
	public class ParkingSpot : MonoBehaviour
	{
		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060055E0 RID: 21984 RVA: 0x00168D41 File Offset: 0x00166F41
		// (set) Token: 0x060055E1 RID: 21985 RVA: 0x00168D49 File Offset: 0x00166F49
		public LandVehicle OccupantVehicle { get; protected set; }

		// Token: 0x060055E2 RID: 21986 RVA: 0x00168D52 File Offset: 0x00166F52
		private void Awake()
		{
			this.Init();
			if (this.ParentLot == null)
			{
				Debug.LogError("ParkingSpot has not parent ParkingLot!");
			}
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x00168D74 File Offset: 0x00166F74
		private void Init()
		{
			if (this.ParentLot == null)
			{
				this.ParentLot = base.GetComponentInParent<ParkingLot>();
			}
			if (this.ParentLot == null)
			{
				Debug.LogError("ParkingSpot has not parent ParkingLot!");
			}
			this.ParentLot.ParkingSpots.Add(this);
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x00168DC4 File Offset: 0x00166FC4
		public void SetOccupant(LandVehicle vehicle)
		{
			this.OccupantVehicle = vehicle;
			this.OccupantVehicle_Readonly = this.OccupantVehicle;
		}

		// Token: 0x04003FC9 RID: 16329
		private ParkingLot ParentLot;

		// Token: 0x04003FCA RID: 16330
		public Transform AlignmentPoint;

		// Token: 0x04003FCB RID: 16331
		public EParkingAlignment Alignment;

		// Token: 0x04003FCC RID: 16332
		[SerializeField]
		private LandVehicle OccupantVehicle_Readonly;
	}
}
