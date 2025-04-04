using System;

namespace Funly.SkyStudio
{
	// Token: 0x02000192 RID: 402
	public class CloudBlender : FeatureBlender
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00026001 File Offset: 0x00024201
		protected override string featureKey
		{
			get
			{
				return "CloudFeature";
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00026008 File Offset: 0x00024208
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumber("CloudDensityKey");
			helper.BlendNumber("CloudTextureTiling");
			helper.BlendNumber("CloudSpeedKey");
			helper.BlendNumber("CloudDirectionKey");
			helper.BlendNumber("CloudFadeAmountKey");
			helper.BlendNumber("CloudFadePositionKey");
			helper.BlendNumber("CloudAlphaKey");
			helper.BlendColor("CloudColor1Key");
			helper.BlendColor("CloudColor2Key");
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00026078 File Offset: 0x00024278
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("CloudAlphaKey", 0f);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0002608A File Offset: 0x0002428A
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("CloudAlphaKey", 0f);
		}
	}
}
