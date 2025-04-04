using System;

namespace Funly.SkyStudio
{
	// Token: 0x02000197 RID: 407
	public class RainBlender : FeatureBlender
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0002621D File Offset: 0x0002441D
		protected override string featureKey
		{
			get
			{
				return "RainFeature";
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00026224 File Offset: 0x00024424
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumber("RainSoundVolume");
			helper.BlendNumber("RainNearIntensityKey");
			helper.BlendNumber("RainNearSpeedKey");
			helper.BlendNumber("RainNearTextureTiling");
			helper.BlendNumber("RainFarIntensityKey");
			helper.BlendNumber("RainFarSpeedKey");
			helper.BlendNumber("RainFarTextureTiling");
			helper.BlendColor("RainTintColorKey");
			helper.BlendNumber("RainWindTurbulenceKey");
			helper.BlendNumber("RainWindTurbulenceSpeedKey");
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0002629F File Offset: 0x0002449F
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("RainSoundVolume", 0f);
			helper.BlendNumberIn("RainNearIntensityKey", 0f);
			helper.BlendNumberIn("RainFarIntensityKey", 0f);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000262D1 File Offset: 0x000244D1
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("RainSoundVolume", 0f);
			helper.BlendNumberOut("RainNearIntensityKey", 0f);
			helper.BlendNumberOut("RainFarIntensityKey", 0f);
		}
	}
}
