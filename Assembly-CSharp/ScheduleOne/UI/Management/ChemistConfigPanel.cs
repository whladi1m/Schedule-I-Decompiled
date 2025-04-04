using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD2 RID: 2770
	public class ChemistConfigPanel : ConfigPanel
	{
		// Token: 0x06004A46 RID: 19014 RVA: 0x001374CC File Offset: 0x001356CC
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			List<ObjectListField> list2 = new List<ObjectListField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				ChemistConfiguration chemistConfiguration = (ChemistConfiguration)entityConfiguration;
				if (chemistConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to BotanistConfiguration", null);
					return;
				}
				list.Add(chemistConfiguration.Bed);
				list2.Add(chemistConfiguration.Stations);
			}
			this.BedUI.Bind(list);
			this.StationsUI.Bind(list2);
		}

		// Token: 0x040037DB RID: 14299
		[Header("References")]
		public ObjectFieldUI BedUI;

		// Token: 0x040037DC RID: 14300
		public ObjectListFieldUI StationsUI;
	}
}
