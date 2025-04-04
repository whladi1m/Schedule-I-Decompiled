using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Product;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008A2 RID: 2210
	public class CocaineStationItem : StationItem
	{
		// Token: 0x06003C4C RID: 15436 RVA: 0x000FDC50 File Offset: 0x000FBE50
		public override void Initialize(StorableItemDefinition itemDefinition)
		{
			base.Initialize(itemDefinition);
			CocaineInstance cocaineInstance = ((CocaineDefinition)itemDefinition).GetDefaultInstance(1) as CocaineInstance;
			foreach (FilledPackagingVisuals visuals2 in this.Visuals)
			{
				cocaineInstance.SetupPackagingVisuals(visuals2);
			}
		}

		// Token: 0x04002B72 RID: 11122
		public FilledPackagingVisuals[] Visuals;
	}
}
