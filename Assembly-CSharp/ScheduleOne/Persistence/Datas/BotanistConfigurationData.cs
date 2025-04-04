using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003EA RID: 1002
	[Serializable]
	public class BotanistConfigurationData : SaveData
	{
		// Token: 0x06001549 RID: 5449 RVA: 0x0005F124 File Offset: 0x0005D324
		public BotanistConfigurationData(ObjectFieldData bed, ObjectFieldData supplies, ObjectListFieldData pots)
		{
			this.Bed = bed;
			this.Supplies = supplies;
			this.Pots = pots;
		}

		// Token: 0x040013A1 RID: 5025
		public ObjectFieldData Bed;

		// Token: 0x040013A2 RID: 5026
		public ObjectFieldData Supplies;

		// Token: 0x040013A3 RID: 5027
		public ObjectListFieldData Pots;
	}
}
