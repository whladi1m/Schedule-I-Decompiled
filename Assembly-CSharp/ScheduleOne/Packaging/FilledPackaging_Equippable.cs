using System;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;

namespace ScheduleOne.Packaging
{
	// Token: 0x0200086F RID: 2159
	public class FilledPackaging_Equippable : Equippable_Viewmodel
	{
		// Token: 0x06003A8D RID: 14989 RVA: 0x000F660E File Offset: 0x000F480E
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			(item as ProductItemInstance).SetupPackagingVisuals(this.Visuals);
		}

		// Token: 0x04002A67 RID: 10855
		public FilledPackagingVisuals Visuals;
	}
}
