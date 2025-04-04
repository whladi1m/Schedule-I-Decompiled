using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E8 RID: 1000
	public class LawData : SaveData
	{
		// Token: 0x06001546 RID: 5446 RVA: 0x0005F0F0 File Offset: 0x0005D2F0
		public LawData(float internalLawIntensity)
		{
			this.InternalLawIntensity = internalLawIntensity;
		}

		// Token: 0x0400139C RID: 5020
		public float InternalLawIntensity;
	}
}
