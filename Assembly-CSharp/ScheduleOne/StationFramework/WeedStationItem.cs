using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Product;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008BD RID: 2237
	public class WeedStationItem : StationItem
	{
		// Token: 0x06003CBE RID: 15550 RVA: 0x000FF068 File Offset: 0x000FD268
		public override void Initialize(StorableItemDefinition itemDefinition)
		{
			base.Initialize(itemDefinition);
			WeedInstance weedInstance = ((WeedDefinition)itemDefinition).GetDefaultInstance(1) as WeedInstance;
			foreach (FilledPackagingVisuals visuals2 in this.Visuals)
			{
				weedInstance.SetupPackagingVisuals(visuals2);
			}
		}

		// Token: 0x04002BE0 RID: 11232
		public FilledPackagingVisuals[] Visuals;
	}
}
