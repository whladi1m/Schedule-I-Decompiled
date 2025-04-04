using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003ED RID: 1005
	[Serializable]
	public class ChemistConfigurationData : SaveData
	{
		// Token: 0x0600154C RID: 5452 RVA: 0x0005F15F File Offset: 0x0005D35F
		public ChemistConfigurationData(ObjectFieldData bed, ObjectListFieldData stations)
		{
			this.Bed = bed;
			this.Stations = stations;
		}

		// Token: 0x040013A6 RID: 5030
		public ObjectFieldData Bed;

		// Token: 0x040013A7 RID: 5031
		public ObjectListFieldData Stations;
	}
}
