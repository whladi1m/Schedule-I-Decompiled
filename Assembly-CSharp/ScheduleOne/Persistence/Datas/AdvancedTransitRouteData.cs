using System;
using System.Collections.Generic;
using ScheduleOne.Management;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E9 RID: 1001
	[Serializable]
	public class AdvancedTransitRouteData
	{
		// Token: 0x06001547 RID: 5447 RVA: 0x0005F0FF File Offset: 0x0005D2FF
		public AdvancedTransitRouteData(string sourceGUID, string destinationGUID, ManagementItemFilter.EMode filtermode, List<string> filterGUIDs)
		{
			this.SourceGUID = sourceGUID;
			this.DestinationGUID = destinationGUID;
			this.FilterMode = filtermode;
			this.FilterItemIDs = filterGUIDs;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0000494F File Offset: 0x00002B4F
		public AdvancedTransitRouteData()
		{
		}

		// Token: 0x0400139D RID: 5021
		public string SourceGUID;

		// Token: 0x0400139E RID: 5022
		public string DestinationGUID;

		// Token: 0x0400139F RID: 5023
		public ManagementItemFilter.EMode FilterMode;

		// Token: 0x040013A0 RID: 5024
		public List<string> FilterItemIDs;
	}
}
