using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD0 RID: 2768
	public class BrickPressConfigPanel : ConfigPanel
	{
		// Token: 0x06004A42 RID: 19010 RVA: 0x001373D4 File Offset: 0x001355D4
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<ObjectField> list = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				BrickPressConfiguration brickPressConfiguration = (BrickPressConfiguration)entityConfiguration;
				if (brickPressConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to BrickPressConfiguration", null);
					return;
				}
				list.Add(brickPressConfiguration.Destination);
			}
			this.DestinationUI.Bind(list);
		}

		// Token: 0x040037D9 RID: 14297
		[Header("References")]
		public ObjectFieldUI DestinationUI;
	}
}
