using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScheduleOne.AvatarFramework.Animation
{
	// Token: 0x0200097F RID: 2431
	public class AvatarSeatSet : MonoBehaviour
	{
		// Token: 0x06004208 RID: 16904 RVA: 0x001153E8 File Offset: 0x001135E8
		public AvatarSeat GetFirstFreeSeat()
		{
			for (int i = 0; i < this.Seats.Length; i++)
			{
				if (!this.Seats[i].IsOccupied)
				{
					return this.Seats[i];
				}
			}
			Console.LogWarning("Failed to find a free seat! Returning the first seat.", null);
			return this.Seats[0];
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x00115434 File Offset: 0x00113634
		public AvatarSeat GetRandomFreeSeat()
		{
			List<AvatarSeat> list = (from x in this.Seats
			where !x.IsOccupied
			select x).ToList<AvatarSeat>();
			if (list.Count == 0)
			{
				Console.LogWarning("Failed to find a free seat! Returning the first seat.", null);
				return this.Seats[0];
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		// Token: 0x04003010 RID: 12304
		public AvatarSeat[] Seats;
	}
}
