using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003FA RID: 1018
	[Serializable]
	public class PotConfigurationData : SaveData
	{
		// Token: 0x06001559 RID: 5465 RVA: 0x0005F253 File Offset: 0x0005D453
		public PotConfigurationData(ItemFieldData seed, ItemFieldData additive1, ItemFieldData additive2, ItemFieldData additive3, ObjectFieldData destination)
		{
			this.Seed = seed;
			this.Additive1 = additive1;
			this.Additive2 = additive2;
			this.Additive3 = additive3;
			this.Destination = destination;
		}

		// Token: 0x040013BA RID: 5050
		public ItemFieldData Seed;

		// Token: 0x040013BB RID: 5051
		public ItemFieldData Additive1;

		// Token: 0x040013BC RID: 5052
		public ItemFieldData Additive2;

		// Token: 0x040013BD RID: 5053
		public ItemFieldData Additive3;

		// Token: 0x040013BE RID: 5054
		public ObjectFieldData Destination;
	}
}
