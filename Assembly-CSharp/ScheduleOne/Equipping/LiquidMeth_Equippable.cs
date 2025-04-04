using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;

namespace ScheduleOne.Equipping
{
	// Token: 0x020008FC RID: 2300
	public class LiquidMeth_Equippable : Equippable_Viewmodel
	{
		// Token: 0x06003E5B RID: 15963 RVA: 0x001073F8 File Offset: 0x001055F8
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			LiquidMethDefinition def = item.Definition as LiquidMethDefinition;
			if (this.Visuals != null)
			{
				this.Visuals.Setup(def);
			}
		}

		// Token: 0x04002CE1 RID: 11489
		public LiquidMethVisuals Visuals;
	}
}
