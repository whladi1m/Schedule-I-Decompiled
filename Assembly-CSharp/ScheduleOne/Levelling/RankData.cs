using System;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Levelling
{
	// Token: 0x020005A6 RID: 1446
	public class RankData : SaveData
	{
		// Token: 0x0600241E RID: 9246 RVA: 0x00092789 File Offset: 0x00090989
		public RankData(int rank, int tier, int xp, int totalXP)
		{
			this.Rank = rank;
			this.Tier = tier;
			this.XP = xp;
			this.TotalXP = totalXP;
		}

		// Token: 0x04001AEC RID: 6892
		public int Rank;

		// Token: 0x04001AED RID: 6893
		public int Tier;

		// Token: 0x04001AEE RID: 6894
		public int XP;

		// Token: 0x04001AEF RID: 6895
		public int TotalXP;
	}
}
