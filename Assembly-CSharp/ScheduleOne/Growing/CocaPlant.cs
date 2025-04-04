using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Growing
{
	// Token: 0x02000866 RID: 2150
	public class CocaPlant : Plant
	{
		// Token: 0x06003A62 RID: 14946 RVA: 0x000F5C7C File Offset: 0x000F3E7C
		public override ItemInstance GetHarvestedProduct(int quantity = 1)
		{
			EQuality quality = ItemQuality.GetQuality(this.QualityLevel);
			QualityItemInstance qualityItemInstance = this.Harvestable.Product.GetDefaultInstance(quantity) as QualityItemInstance;
			qualityItemInstance.Quality = quality;
			return qualityItemInstance;
		}

		// Token: 0x04002A3F RID: 10815
		public PlantHarvestable Harvestable;
	}
}
