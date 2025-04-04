using System;
using System.Collections.Generic;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000922 RID: 2338
	public class ItemFilter_Category : ItemFilter
	{
		// Token: 0x06003F77 RID: 16247 RVA: 0x0010B6C2 File Offset: 0x001098C2
		public ItemFilter_Category(List<EItemCategory> acceptedCategories)
		{
			this.AcceptedCategories = acceptedCategories;
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x0010B6DC File Offset: 0x001098DC
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			return this.AcceptedCategories.Contains(instance.Category) && base.DoesItemMatchFilter(instance);
		}

		// Token: 0x04002DC2 RID: 11714
		public List<EItemCategory> AcceptedCategories = new List<EItemCategory>();
	}
}
