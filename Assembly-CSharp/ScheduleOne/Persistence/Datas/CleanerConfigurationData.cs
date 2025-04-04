using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003EF RID: 1007
	[Serializable]
	public class CleanerConfigurationData : SaveData
	{
		// Token: 0x0600154E RID: 5454 RVA: 0x0005F18B File Offset: 0x0005D38B
		public CleanerConfigurationData(ObjectFieldData bed, ObjectListFieldData bins)
		{
			this.Bed = bed;
			this.Bins = bins;
		}

		// Token: 0x040013AA RID: 5034
		public ObjectFieldData Bed;

		// Token: 0x040013AB RID: 5035
		public ObjectListFieldData Bins;
	}
}
