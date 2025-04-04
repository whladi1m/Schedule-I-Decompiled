using System;
using System.Collections.Generic;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003FC RID: 1020
	[Serializable]
	public class RouteListData
	{
		// Token: 0x0600155B RID: 5467 RVA: 0x0005F28F File Offset: 0x0005D48F
		public RouteListData(List<AdvancedTransitRouteData> routes)
		{
			this.Routes = routes;
		}

		// Token: 0x040013C0 RID: 5056
		public List<AdvancedTransitRouteData> Routes;
	}
}
