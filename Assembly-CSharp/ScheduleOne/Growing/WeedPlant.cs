using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Growing
{
	// Token: 0x0200086E RID: 2158
	public class WeedPlant : Plant
	{
		// Token: 0x06003A8B RID: 14987 RVA: 0x000F65D8 File Offset: 0x000F47D8
		public override ItemInstance GetHarvestedProduct(int quantity = 1)
		{
			EQuality quality = ItemQuality.GetQuality(this.QualityLevel);
			QualityItemInstance qualityItemInstance = this.BranchPrefab.Product.GetDefaultInstance(quantity) as QualityItemInstance;
			qualityItemInstance.Quality = quality;
			return qualityItemInstance;
		}

		// Token: 0x04002A66 RID: 10854
		public PlantHarvestable BranchPrefab;
	}
}
