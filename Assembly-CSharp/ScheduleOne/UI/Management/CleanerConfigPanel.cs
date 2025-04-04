using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD4 RID: 2772
	public class CleanerConfigPanel : ConfigPanel
	{
		// Token: 0x06004A4A RID: 19018 RVA: 0x001375FC File Offset: 0x001357FC
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<ObjectListField> list2 = new List<ObjectListField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				CleanerConfiguration cleanerConfiguration = (CleanerConfiguration)entityConfiguration;
				if (cleanerConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to CleanerConfiguration", null);
					return;
				}
				list.Add(cleanerConfiguration.Bed);
				list2.Add(cleanerConfiguration.Bins);
			}
			this.BedUI.Bind(list);
			this.BinsUI.Bind(list2);
		}

		// Token: 0x040037DF RID: 14303
		[Header("References")]
		public ObjectFieldUI BedUI;

		// Token: 0x040037E0 RID: 14304
		public ObjectListFieldUI BinsUI;
	}
}
