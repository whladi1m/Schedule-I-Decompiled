using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003CC RID: 972
	[Serializable]
	public class DateTimeData : SaveData
	{
		// Token: 0x0600151B RID: 5403 RVA: 0x0005ECDC File Offset: 0x0005CEDC
		public DateTimeData(DateTime date)
		{
			this.Year = date.Year;
			this.Month = date.Month;
			this.Day = date.Day;
			this.Hour = date.Hour;
			this.Minute = date.Minute;
			this.Second = date.Second;
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0005ED3D File Offset: 0x0005CF3D
		public DateTime GetDateTime()
		{
			return new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second);
		}

		// Token: 0x0400136E RID: 4974
		public int Year;

		// Token: 0x0400136F RID: 4975
		public int Month;

		// Token: 0x04001370 RID: 4976
		public int Day;

		// Token: 0x04001371 RID: 4977
		public int Hour;

		// Token: 0x04001372 RID: 4978
		public int Minute;

		// Token: 0x04001373 RID: 4979
		public int Second;
	}
}
