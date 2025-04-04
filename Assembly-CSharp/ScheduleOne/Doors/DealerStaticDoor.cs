using System;
using ScheduleOne.Economy;
using UnityEngine;

namespace ScheduleOne.Doors
{
	// Token: 0x02000674 RID: 1652
	public class DealerStaticDoor : StaticDoor
	{
		// Token: 0x06002DD0 RID: 11728 RVA: 0x000C05B4 File Offset: 0x000BE7B4
		protected override bool IsKnockValid(out string message)
		{
			if (this.Building.OccupantCount == 0 && Vector3.Distance(base.transform.position, this.Dealer.transform.position) > 2f)
			{
				message = this.Dealer.FirstName + " is out dealing";
				return false;
			}
			return base.IsKnockValid(out message);
		}

		// Token: 0x0400209C RID: 8348
		public Dealer Dealer;
	}
}
