using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000926 RID: 2342
	public class ItemFilter_MixingIngredient : ItemFilter
	{
		// Token: 0x06003F83 RID: 16259 RVA: 0x0010B824 File Offset: 0x00109A24
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			if (instance == null)
			{
				return false;
			}
			ItemDefinition definition = instance.Definition;
			if (!(definition is PropertyItemDefinition))
			{
				return false;
			}
			PropertyItemDefinition item = definition as PropertyItemDefinition;
			return NetworkSingleton<ProductManager>.Instance.ValidMixIngredients.Contains(item) && base.DoesItemMatchFilter(instance);
		}
	}
}
