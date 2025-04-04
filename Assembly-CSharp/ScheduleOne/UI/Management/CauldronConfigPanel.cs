using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD1 RID: 2769
	public class CauldronConfigPanel : ConfigPanel
	{
		// Token: 0x06004A44 RID: 19012 RVA: 0x00137450 File Offset: 0x00135650
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				CauldronConfiguration cauldronConfiguration = (CauldronConfiguration)entityConfiguration;
				if (cauldronConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to CauldronConfiguration", null);
					return;
				}
				list.Add(cauldronConfiguration.Destination);
			}
			this.DestinationUI.Bind(list);
		}

		// Token: 0x040037DA RID: 14298
		[Header("References")]
		public ObjectFieldUI DestinationUI;
	}
}
