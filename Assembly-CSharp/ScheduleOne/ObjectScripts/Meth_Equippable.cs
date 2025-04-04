using System;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B85 RID: 2949
	public class Meth_Equippable : Equippable_Viewmodel
	{
		// Token: 0x06004F7E RID: 20350 RVA: 0x0014F208 File Offset: 0x0014D408
		public override void Equip(ItemInstance item)
		{
			base.Equip(item);
			MethInstance methInstance = item as MethInstance;
			if (methInstance != null)
			{
				this.Visuals.Setup(methInstance.Definition as MethDefinition);
			}
		}

		// Token: 0x04003BFB RID: 15355
		public MethVisuals Visuals;
	}
}
