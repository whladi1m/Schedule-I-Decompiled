using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008A0 RID: 2208
	public class LiquidMeth_StationItem : StationItem
	{
		// Token: 0x06003C39 RID: 15417 RVA: 0x000FD654 File Offset: 0x000FB854
		public override void Initialize(StorableItemDefinition itemDefinition)
		{
			base.Initialize(itemDefinition);
			LiquidMethDefinition liquidMethDefinition = itemDefinition as LiquidMethDefinition;
			if (this.Visuals != null)
			{
				this.Visuals.Setup(liquidMethDefinition);
			}
			base.GetModule<CookableModule>().LiquidColor = liquidMethDefinition.CookableLiquidColor;
			base.GetModule<CookableModule>().SolidColor = liquidMethDefinition.CookableSolidColor;
			base.GetModule<PourableModule>().LiquidColor = liquidMethDefinition.LiquidVolumeColor;
			base.GetModule<PourableModule>().PourParticlesColor = liquidMethDefinition.PourParticlesColor;
		}

		// Token: 0x04002B5E RID: 11102
		public LiquidMethVisuals Visuals;
	}
}
