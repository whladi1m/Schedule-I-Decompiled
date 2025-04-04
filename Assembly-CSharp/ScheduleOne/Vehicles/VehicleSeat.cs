using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007C5 RID: 1989
	public class VehicleSeat : MonoBehaviour
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06003676 RID: 13942 RVA: 0x000E524C File Offset: 0x000E344C
		public bool isOccupied
		{
			get
			{
				return this.Occupant != null;
			}
		}

		// Token: 0x0400273A RID: 10042
		public bool isDriverSeat;

		// Token: 0x0400273B RID: 10043
		public Player Occupant;
	}
}
