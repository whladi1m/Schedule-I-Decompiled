using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD9 RID: 2777
	public class PackagingStationConfigPanel : ConfigPanel
	{
		// Token: 0x06004A54 RID: 19028 RVA: 0x001378FC File Offset: 0x00135AFC
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				PackagingStationConfiguration packagingStationConfiguration = (PackagingStationConfiguration)entityConfiguration;
				if (packagingStationConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to PackagingStationConfiguration", null);
					return;
				}
				list.Add(packagingStationConfiguration.Destination);
			}
			this.DestinationUI.Bind(list);
		}

		// Token: 0x040037E9 RID: 14313
		[Header("References")]
		public ObjectFieldUI DestinationUI;
	}
}
