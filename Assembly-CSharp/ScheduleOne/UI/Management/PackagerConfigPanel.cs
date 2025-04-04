using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD8 RID: 2776
	public class PackagerConfigPanel : ConfigPanel
	{
		// Token: 0x06004A52 RID: 19026 RVA: 0x00137840 File Offset: 0x00135A40
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<ObjectListField> list2 = new List<ObjectListField>();
			List<RouteListField> list3 = new List<RouteListField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				PackagerConfiguration packagerConfiguration = (PackagerConfiguration)entityConfiguration;
				if (packagerConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to PackagerConfiguration", null);
					return;
				}
				list.Add(packagerConfiguration.Bed);
				list2.Add(packagerConfiguration.Stations);
				list3.Add(packagerConfiguration.Routes);
			}
			this.BedUI.Bind(list);
			this.StationsUI.Bind(list2);
			this.RoutesUI.Bind(list3);
		}

		// Token: 0x040037E6 RID: 14310
		[Header("References")]
		public ObjectFieldUI BedUI;

		// Token: 0x040037E7 RID: 14311
		public ObjectListFieldUI StationsUI;

		// Token: 0x040037E8 RID: 14312
		public RouteListFieldUI RoutesUI;
	}
}
