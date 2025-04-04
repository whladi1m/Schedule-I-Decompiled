using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200019A RID: 410
	public class StarBlender : FeatureBlender
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x00026470 File Offset: 0x00024670
		protected override string featureKey
		{
			get
			{
				return "StarLayer" + this.starLayer.ToString() + "Feature";
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0002648C File Offset: 0x0002468C
		protected override void BlendBoth(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendColor(this.PropertyKeyForLayer("Star1ColorKey"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1SizeKey"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1RotationSpeed"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1TwinkleAmountKey"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1TwinkleSpeedKey"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1EdgeFeathering"));
			helper.BlendNumber(this.PropertyKeyForLayer("Star1ColorIntensityKey"));
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00026510 File Offset: 0x00024710
		protected override void BlendIn(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberIn(this.PropertyKeyForLayer("Star1SizeKey"), 0f);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00026528 File Offset: 0x00024728
		protected override void BlendOut(ProfileBlendingState state, BlendingHelper helper)
		{
			helper.BlendNumberOut(this.PropertyKeyForLayer("Star1SizeKey"), 0f);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00026540 File Offset: 0x00024740
		private string PropertyKeyForLayer(string key)
		{
			return key.Replace("Star1", "Star" + this.starLayer.ToString());
		}

		// Token: 0x04000930 RID: 2352
		[Range(1f, 3f)]
		public int starLayer;
	}
}
