using System;
using System.Collections.Generic;
using ScheduleOne.Product;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000927 RID: 2343
	public class ItemFilter_PackagedProduct : ItemFilter_Category
	{
		// Token: 0x06003F84 RID: 16260 RVA: 0x0010B869 File Offset: 0x00109A69
		public ItemFilter_PackagedProduct() : base(new List<EItemCategory>
		{
			EItemCategory.Product
		})
		{
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x0010B880 File Offset: 0x00109A80
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			ProductItemInstance productItemInstance = instance as ProductItemInstance;
			return productItemInstance != null && !(productItemInstance.AppliedPackaging == null) && base.DoesItemMatchFilter(instance);
		}
	}
}
