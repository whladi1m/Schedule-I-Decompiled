using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003FF RID: 1023
	[Serializable]
	public class MoneyData : SaveData
	{
		// Token: 0x0600155E RID: 5470 RVA: 0x0005F2DA File Offset: 0x0005D4DA
		public MoneyData(float onlineBalance, float netWorth, float lifetimeEarnings, float weeklyDepositSum)
		{
			this.OnlineBalance = onlineBalance;
			this.Networth = netWorth;
			this.LifetimeEarnings = lifetimeEarnings;
			this.WeeklyDepositSum = weeklyDepositSum;
		}

		// Token: 0x040013C7 RID: 5063
		public float OnlineBalance;

		// Token: 0x040013C8 RID: 5064
		public float Networth;

		// Token: 0x040013C9 RID: 5065
		public float LifetimeEarnings;

		// Token: 0x040013CA RID: 5066
		public float WeeklyDepositSum;
	}
}
