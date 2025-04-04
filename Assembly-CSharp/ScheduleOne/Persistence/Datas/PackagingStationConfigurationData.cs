using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F9 RID: 1017
	[Serializable]
	public class PackagingStationConfigurationData : SaveData
	{
		// Token: 0x06001558 RID: 5464 RVA: 0x0005F244 File Offset: 0x0005D444
		public PackagingStationConfigurationData(ObjectFieldData destination)
		{
			this.Destination = destination;
		}

		// Token: 0x040013B9 RID: 5049
		public ObjectFieldData Destination;
	}
}
