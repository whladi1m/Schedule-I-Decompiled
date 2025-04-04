using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Product;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008B2 RID: 2226
	public class MethStationItem : StationItem
	{
		// Token: 0x06003C8B RID: 15499 RVA: 0x000FE554 File Offset: 0x000FC754
		public override void Initialize(StorableItemDefinition itemDefinition)
		{
			base.Initialize(itemDefinition);
			MethInstance methInstance = ((MethDefinition)itemDefinition).GetDefaultInstance(1) as MethInstance;
			foreach (FilledPackagingVisuals visuals2 in this.Visuals)
			{
				methInstance.SetupPackagingVisuals(visuals2);
			}
		}

		// Token: 0x04002BAA RID: 11178
		public FilledPackagingVisuals[] Visuals;
	}
}
