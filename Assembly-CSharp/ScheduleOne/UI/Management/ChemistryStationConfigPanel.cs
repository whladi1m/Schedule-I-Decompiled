using System;
using System.Collections.Generic;
using ScheduleOne.Management;
using ScheduleOne.Management.UI;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000AD3 RID: 2771
	public class ChemistryStationConfigPanel : ConfigPanel
	{
		// Token: 0x06004A48 RID: 19016 RVA: 0x00137564 File Offset: 0x00135764
		public override void Bind(List<EntityConfiguration> configs)
		{
			List<StationRecipeField> list = new List<StationRecipeField>();
			List<ObjectField> list2 = new List<ObjectField>();
			foreach (EntityConfiguration entityConfiguration in configs)
			{
				ChemistryStationConfiguration chemistryStationConfiguration = (ChemistryStationConfiguration)entityConfiguration;
				if (chemistryStationConfiguration == null)
				{
					Console.LogError("Failed to cast EntityConfiguration to ChemistryStationConfiguration", null);
					return;
				}
				list2.Add(chemistryStationConfiguration.Destination);
				list.Add(chemistryStationConfiguration.Recipe);
			}
			this.RecipeUI.Bind(list);
			this.DestinationUI.Bind(list2);
		}

		// Token: 0x040037DD RID: 14301
		[Header("References")]
		public StationRecipeFieldUI RecipeUI;

		// Token: 0x040037DE RID: 14302
		public ObjectFieldUI DestinationUI;
	}
}
