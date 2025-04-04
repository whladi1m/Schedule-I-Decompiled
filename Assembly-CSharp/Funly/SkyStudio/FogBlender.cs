using System;

namespace Funly.SkyStudio
{
	// Token: 0x02000194 RID: 404
	public class FogBlender : FeatureBlender
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x000260C2 File Offset: 0x000242C2
		protected override string featureKey
		{
			get
			{
				return "FogFeature";
			}
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x000260C9 File Offset: 0x000242C9
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumber("FogDensityKey");
			helper.BlendNumber("FogLengthKey");
			helper.BlendColor("FogColorKey");
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x000260EC File Offset: 0x000242EC
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("FogDensityKey", 0f);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x000260FE File Offset: 0x000242FE
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("FogDensityKey", 0f);
		}
	}
}
