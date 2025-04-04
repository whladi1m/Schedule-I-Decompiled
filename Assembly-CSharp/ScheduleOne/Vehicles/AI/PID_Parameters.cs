using System;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007E6 RID: 2022
	[Serializable]
	public struct PID_Parameters
	{
		// Token: 0x060036F5 RID: 14069 RVA: 0x000E7AFB File Offset: 0x000E5CFB
		public PID_Parameters(float P, float I, float D)
		{
			this.P = P;
			this.I = I;
			this.D = D;
		}

		// Token: 0x040027F0 RID: 10224
		public float P;

		// Token: 0x040027F1 RID: 10225
		public float I;

		// Token: 0x040027F2 RID: 10226
		public float D;
	}
}
