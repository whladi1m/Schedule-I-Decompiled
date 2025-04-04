using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;

namespace ScheduleOne.Clothing
{
	// Token: 0x0200072F RID: 1839
	[Serializable]
	public class ClothingInstance : StorableItemInstance
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060031F1 RID: 12785 RVA: 0x000CFC1B File Offset: 0x000CDE1B
		public override string Name
		{
			get
			{
				return base.Name + ((this.Color != EClothingColor.White) ? (" (" + this.Color.GetLabel() + ")") : string.Empty);
			}
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x000CFC51 File Offset: 0x000CDE51
		public ClothingInstance()
		{
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000CFC59 File Offset: 0x000CDE59
		public ClothingInstance(ItemDefinition definition, int quantity, EClothingColor color) : base(definition, quantity)
		{
			this.Color = color;
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x000CFC6C File Offset: 0x000CDE6C
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			return new ClothingInstance(base.Definition, quantity, this.Color);
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x000CFC98 File Offset: 0x000CDE98
		public override ItemData GetItemData()
		{
			return new ClothingData(this.ID, this.Quantity, this.Color);
		}

		// Token: 0x040023BB RID: 9147
		public EClothingColor Color;
	}
}
