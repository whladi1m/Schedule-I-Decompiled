using System;

namespace Funly.SkyStudio
{
	// Token: 0x02000195 RID: 405
	public class LightningBlender : FeatureBlender
	{
		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00026110 File Offset: 0x00024310
		protected override string featureKey
		{
			get
			{
				return "LightningFeature";
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00026118 File Offset: 0x00024318
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor("LightningTintColorKey");
			helper.BlendNumber("ThunderSoundVolumeKey");
			helper.BlendNumber("ThunderSoundDelayKey");
			helper.BlendNumber("LightningProbabilityKey");
			helper.BlendNumber("LightningStrikeCoolDown");
			helper.BlendNumber("LightningIntensityKey");
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00026167 File Offset: 0x00024367
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn("ThunderSoundVolumeKey", 0f);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00026179 File Offset: 0x00024379
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut("ThunderSoundVolumeKey", 0f);
		}
	}
}
