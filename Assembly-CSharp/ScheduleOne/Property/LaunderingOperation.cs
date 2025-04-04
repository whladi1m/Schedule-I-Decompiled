using System;

namespace ScheduleOne.Property
{
	// Token: 0x020007F8 RID: 2040
	public class LaunderingOperation
	{
		// Token: 0x0600377D RID: 14205 RVA: 0x000EB417 File Offset: 0x000E9617
		public LaunderingOperation(Business _business, float _amount, int _minutesSinceStarted)
		{
			this.business = _business;
			this.amount = _amount;
			this.minutesSinceStarted = _minutesSinceStarted;
		}

		// Token: 0x04002886 RID: 10374
		public Business business;

		// Token: 0x04002887 RID: 10375
		public float amount;

		// Token: 0x04002888 RID: 10376
		public int minutesSinceStarted;

		// Token: 0x04002889 RID: 10377
		public int completionTime_Minutes = 1440;
	}
}
