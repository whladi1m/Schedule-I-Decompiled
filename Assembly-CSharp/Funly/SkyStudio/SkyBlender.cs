using System;

namespace Funly.SkyStudio
{
	// Token: 0x02000199 RID: 409
	public class SkyBlender : FeatureBlender
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x000263B5 File Offset: 0x000245B5
		protected override string featureKey
		{
			get
			{
				return "";
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000022C9 File Offset: 0x000004C9
		protected override ProfileFeatureBlendingMode BlendingMode(ProfileBlendingState state, BlendingHelper helper)
		{
			return ProfileFeatureBlendingMode.Normal;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x000263BC File Offset: 0x000245BC
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("SkyLowerColorKey");
			helper.BlendColor("SkyMiddleColorKey");
			helper.BlendColor("SkyUpperColorKey");
			helper.BlendNumber("SkyMiddleColorPosition");
			helper.BlendNumber("HorizonTransitionStartKey");
			helper.BlendNumber("HorizonTransitionLengthKey");
			helper.BlendNumber("StarTransitionStartKey");
			helper.BlendNumber("StarTransitionLengthKey");
			helper.BlendNumber("HorizonStarScaleKey");
			helper.BlendColor("AmbientLightSkyColorKey");
			helper.BlendColor("AmbientLightEquatorColorKey");
			helper.BlendColor("AmbientLightGroundColorKey");
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0002644D File Offset: 0x0002464D
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("AmbientLightSkyColorKey");
			helper.BlendColor("AmbientLightEquatorColorKey");
			helper.BlendColor("AmbientLightGroundColorKey");
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0002644D File Offset: 0x0002464D
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("AmbientLightSkyColorKey");
			helper.BlendColor("AmbientLightEquatorColorKey");
			helper.BlendColor("AmbientLightGroundColorKey");
		}
	}
}
