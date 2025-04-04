using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003EE RID: 1006
	[Serializable]
	public class ChemistryStationConfigurationData : SaveData
	{
		// Token: 0x0600154D RID: 5453 RVA: 0x0005F175 File Offset: 0x0005D375
		public ChemistryStationConfigurationData(StationRecipeFieldData recipe, ObjectFieldData destination)
		{
			this.Recipe = recipe;
			this.Destination = destination;
		}

		// Token: 0x040013A8 RID: 5032
		public StationRecipeFieldData Recipe;

		// Token: 0x040013A9 RID: 5033
		public ObjectFieldData Destination;
	}
}
