using System;

namespace Funly.SkyStudio
{
	// Token: 0x02000198 RID: 408
	public class RainSplashBlender : FeatureBlender
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x00026303 File Offset: 0x00024503
		protected override string featureKey
		{
			get
			{
				return "RainSplashFeature";
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002630C File Offset: 0x0002450C
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumber("RainSplashMaxConcurrentKey");
			helper.BlendNumber("RainSplashAreaStartKey");
			helper.BlendNumber("RainSplashAreaLengthKey");
			helper.BlendNumber("RainSplashScaleKey");
			helper.BlendNumber("RainSplashScaleVarienceKey");
			helper.BlendNumber("RainSplashIntensityKey");
			helper.BlendNumber("RainSplashSurfaceOffsetKey");
			helper.BlendColor("RainSplashTintColorKey");
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00026371 File Offset: 0x00024571
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("RainSplashIntensityKey", 0f);
			helper.BlendNumberIn("RainSplashMaxConcurrentKey", 0f);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00026393 File Offset: 0x00024593
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("RainSplashIntensityKey", 0f);
			helper.BlendNumberOut("RainSplashMaxConcurrentKey", 0f);
		}
	}
}
