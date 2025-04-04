using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD7 RID: 2775
	public class MixingStationConfigPanel : ConfigPanel
	{
		// Token: 0x06004A50 RID: 19024 RVA: 0x001377A8 File Offset: 0x001359A8
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<NumberField> list2 = new List<NumberField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				MixingStationConfiguration mixingStationConfiguration = (MixingStationConfiguration)entityConfiguration;
				if (mixingStationConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to MixingStationConfiguration", null);
					return;
				}
				list.Add(mixingStationConfiguration.Destination);
				list2.Add(mixingStationConfiguration.StartThrehold);
			}
			this.DestinationUI.Bind(list);
			this.StartThresholdUI.Bind(list2);
		}

		// Token: 0x040037E4 RID: 14308
		[Header("References")]
		public ObjectFieldUI DestinationUI;

		// Token: 0x040037E5 RID: 14309
		public NumberFieldUI StartThresholdUI;
	}
}
