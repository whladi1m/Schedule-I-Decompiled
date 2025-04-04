using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD5 RID: 2773
	public class DryingRackConfigPanel : ConfigPanel
	{
		// Token: 0x06004A4C RID: 19020 RVA: 0x00137694 File Offset: 0x00135894
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<QualityField> list = new List<QualityField>();
			List<ObjectField> list2 = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				DryingRackConfiguration dryingRackConfiguration = (DryingRackConfiguration)entityConfiguration;
				if (dryingRackConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to DryingRackConfiguration", null);
					return;
				}
				list.Add(dryingRackConfiguration.TargetQuality);
				list2.Add(dryingRackConfiguration.Destination);
			}
			this.QualityUI.Bind(list);
			this.DestinationUI.Bind(list2);
		}

		// Token: 0x040037E1 RID: 14305
		[Header("References")]
		public QualityFieldUI QualityUI;

		// Token: 0x040037E2 RID: 14306
		public ObjectFieldUI DestinationUI;
	}
}
