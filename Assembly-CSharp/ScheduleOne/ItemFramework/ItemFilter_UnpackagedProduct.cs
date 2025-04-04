using System;
using System.Collections.Generic;
using ScheduleOne.Product;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000928 RID: 2344
	public class ItemFilter_UnpackagedProduct : ItemFilter_Category
	{
		// Token: 0x06003F86 RID: 16262 RVA: 0x0010B869 File Offset: 0x00109A69
		public ItemFilter_UnpackagedProduct() : base(new List<EItemCategory>
		{
			EItemCategory.Product
		})
		{
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x0010B8B0 File Offset: 0x00109AB0
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			ProductItemInstance productItemInstance = instance as ProductItemInstance;
			return productItemInstance != null && !(productItemInstance.AppliedPackaging != null) && base.DoesItemMatchFilter(instance);
		}
	}
}
