using System;
using ScheduleOne.Product;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000924 RID: 2340
	public class ItemFilter_Dryable : ItemFilter
	{
		// Token: 0x06003F7E RID: 16254 RVA: 0x0010B769 File Offset: 0x00109969
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			return ItemFilter_Dryable.IsItemDryable(instance) && base.DoesItemMatchFilter(instance);
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x0010B77C File Offset: 0x0010997C
		public static bool IsItemDryable(ItemInstance instance)
		{
			if (instance == null)
			{
				return false;
			}
			ProductItemInstance productItemInstance = instance as ProductItemInstance;
			return (productItemInstance != null && (productItemInstance.Definition as ProductDefinition).DrugType == EDrugType.Marijuana && productItemInstance.AppliedPackaging == null && productItemInstance.Quality < EQuality.Heavenly) || (instance.ID == "cocaleaf" && (instance as QualityItemInstance).Quality < EQuality.Heavenly);
		}
	}
}
