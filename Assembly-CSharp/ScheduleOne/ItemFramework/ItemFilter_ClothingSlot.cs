using System;
using ScheduleOne.Clothing;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000923 RID: 2339
	public class ItemFilter_ClothingSlot : ItemFilter
	{
		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06003F79 RID: 16249 RVA: 0x0010B6FA File Offset: 0x001098FA
		// (set) Token: 0x06003F7A RID: 16250 RVA: 0x0010B702 File Offset: 0x00109902
		public EClothingSlot SlotType { get; private set; }

		// Token: 0x06003F7B RID: 16251 RVA: 0x0010B70B File Offset: 0x0010990B
		public ItemFilter_ClothingSlot(EClothingSlot slot)
		{
			this.SlotType = slot;
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x0010B71C File Offset: 0x0010991C
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			ClothingInstance clothingInstance = instance as ClothingInstance;
			if (clothingInstance == null)
			{
				return false;
			}
			ClothingDefinition clothingDefinition = clothingInstance.Definition as ClothingDefinition;
			return !(clothingDefinition == null) && clothingDefinition.Slot == this.SlotType && base.DoesItemMatchFilter(instance);
		}
	}
}
