using System;

namespace Funly.SkyStudio
{
	// Token: 0x0200019B RID: 411
	public class SunBlender : FeatureBlender
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x00026562 File Offset: 0x00024762
		protected override string featureKey
		{
			get
			{
				return "SunFeature";
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0002656C File Offset: 0x0002476C
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("SunColorKey");
			helper.BlendNumber("SunSizeKey");
			helper.BlendNumber("SunEdgeFeatheringKey");
			helper.BlendNumber("SunColorIntensityKey");
			helper.BlendNumber("SunAlphaKey");
			helper.BlendColor("SunLightColorKey");
			helper.BlendNumber("SunLightIntensityKey");
			helper.BlendSpherePoint("SunPositionKey");
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x000265D1 File Offset: 0x000247D1
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("SunAlphaKey", 0f);
			helper.BlendNumberIn("SunLightIntensityKey", 0f);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x000265F3 File Offset: 0x000247F3
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("SunAlphaKey", 0f);
			helper.BlendNumberOut("SunLightIntensityKey", 0f);
		}
	}
}
