using System;

namespace Funly.SkyStudio
{
	// Token: 0x02000196 RID: 406
	public class MoonBlender : FeatureBlender
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x0002618B File Offset: 0x0002438B
		protected override string featureKey
		{
			get
			{
				return "MoonFeature";
			}
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00026194 File Offset: 0x00024394
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("MoonColorKey");
			helper.BlendNumber("MoonSizeKey");
			helper.BlendNumber("MoonEdgeFeatheringKey");
			helper.BlendNumber("MoonColorIntensityKey");
			helper.BlendNumber("MoonAlphaKey");
			helper.BlendColor("MoonLightColorKey");
			helper.BlendNumber("MoonLightIntensityKey");
			helper.BlendSpherePoint("MoonPositionKey");
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000261F9 File Offset: 0x000243F9
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("MoonAlphaKey", 0f);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002620B File Offset: 0x0002440B
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("MoonAlphaKey", 0f);
		}
	}
}
