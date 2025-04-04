using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD6 RID: 2774
	public class LabOvenConfigPanel : ConfigPanel
	{
		// Token: 0x06004A4E RID: 19022 RVA: 0x0013772C File Offset: 0x0013592C
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				LabOvenConfiguration labOvenConfiguration = (LabOvenConfiguration)entityConfiguration;
				if (labOvenConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to LabOvenConfiguration", null);
					return;
				}
				list.Add(labOvenConfiguration.Destination);
			}
			this.DestinationUI.Bind(list);
		}

		// Token: 0x040037E3 RID: 14307
		[Header("References")]
		public ObjectFieldUI DestinationUI;
	}
}
