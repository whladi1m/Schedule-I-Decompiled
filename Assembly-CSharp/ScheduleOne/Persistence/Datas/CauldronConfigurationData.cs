using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003EC RID: 1004
	[Serializable]
	public class CauldronConfigurationData : SaveData
	{
		// Token: 0x0600154B RID: 5451 RVA: 0x0005F150 File Offset: 0x0005D350
		public CauldronConfigurationData(ObjectFieldData destination)
		{
			this.Destination = destination;
		}

		// Token: 0x040013A5 RID: 5029
		public ObjectFieldData Destination;
	}
}
