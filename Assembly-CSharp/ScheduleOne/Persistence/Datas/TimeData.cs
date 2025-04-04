using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042B RID: 1067
	[Serializable]
	public class TimeData : SaveData
	{
		// Token: 0x06001592 RID: 5522 RVA: 0x0005FB94 File Offset: 0x0005DD94
		public TimeData(int timeOfDay, int elapsedDays, int playtime)
		{
			this.TimeOfDay = timeOfDay;
			this.ElapsedDays = elapsedDays;
			this.Playtime = playtime;
		}

		// Token: 0x0400145C RID: 5212
		public int TimeOfDay;

		// Token: 0x0400145D RID: 5213
		public int ElapsedDays;

		// Token: 0x0400145E RID: 5214
		public int Playtime;
	}
}
