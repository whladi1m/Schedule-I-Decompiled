using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E0 RID: 1760
	[Serializable]
	public class PID
	{
		// Token: 0x06002FE9 RID: 12265 RVA: 0x000C7C99 File Offset: 0x000C5E99
		public PID(float pFactor, float iFactor, float dFactor)
		{
			this.pFactor = pFactor;
			this.iFactor = iFactor;
			this.dFactor = dFactor;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x000C7CB8 File Offset: 0x000C5EB8
		public float Update(float setpoint, float actual, float timeFrame)
		{
			float num = setpoint - actual;
			this.integral += num * timeFrame;
			float num2 = (num - this.lastError) / timeFrame;
			this.lastError = num;
			return num * this.pFactor + this.integral * this.iFactor + num2 * this.dFactor;
		}

		// Token: 0x04002230 RID: 8752
		public float pFactor;

		// Token: 0x04002231 RID: 8753
		public float iFactor;

		// Token: 0x04002232 RID: 8754
		public float dFactor;

		// Token: 0x04002233 RID: 8755
		private float integral;

		// Token: 0x04002234 RID: 8756
		private float lastError;
	}
}
