using System;
using ScheduleOne.GameTime;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003D0 RID: 976
	[Serializable]
	public class GameDateTimeData : SaveData
	{
		// Token: 0x06001521 RID: 5409 RVA: 0x0005EDCB File Offset: 0x0005CFCB
		public GameDateTimeData(int _elapsedDays, int _time)
		{
			this.ElapsedDays = _elapsedDays;
			this.Time = _time;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0005EDE1 File Offset: 0x0005CFE1
		public GameDateTimeData(GameDateTime gameDateTime)
		{
			this.ElapsedDays = gameDateTime.elapsedDays;
			this.Time = gameDateTime.time;
		}

		// Token: 0x0400137B RID: 4987
		public int ElapsedDays;

		// Token: 0x0400137C RID: 4988
		public int Time;
	}
}
