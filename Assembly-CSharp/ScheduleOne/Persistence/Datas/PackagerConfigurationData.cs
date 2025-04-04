using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F8 RID: 1016
	[Serializable]
	public class PackagerConfigurationData : SaveData
	{
		// Token: 0x06001557 RID: 5463 RVA: 0x0005F227 File Offset: 0x0005D427
		public PackagerConfigurationData(ObjectFieldData bed, ObjectListFieldData stations, RouteListData routes)
		{
			this.Bed = bed;
			this.Stations = stations;
			this.Routes = routes;
		}

		// Token: 0x040013B6 RID: 5046
		public ObjectFieldData Bed;

		// Token: 0x040013B7 RID: 5047
		public ObjectListFieldData Stations;

		// Token: 0x040013B8 RID: 5048
		public RouteListData Routes;
	}
}
