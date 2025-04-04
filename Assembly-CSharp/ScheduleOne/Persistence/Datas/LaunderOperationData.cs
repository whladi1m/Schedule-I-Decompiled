using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E7 RID: 999
	[Serializable]
	public class LaunderOperationData : SaveData
	{
		// Token: 0x06001545 RID: 5445 RVA: 0x0005F0DA File Offset: 0x0005D2DA
		public LaunderOperationData(float amount, int minutesSinceStarted)
		{
			this.Amount = amount;
			this.MinutesSinceStarted = minutesSinceStarted;
		}

		// Token: 0x0400139A RID: 5018
		public float Amount;

		// Token: 0x0400139B RID: 5019
		public int MinutesSinceStarted;
	}
}
