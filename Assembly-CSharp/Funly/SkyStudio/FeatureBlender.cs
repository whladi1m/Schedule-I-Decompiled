using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200019D RID: 413
	public abstract class FeatureBlender : MonoBehaviour, IFeatureBlender
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000867 RID: 2151
		protected abstract string featureKey { get; }

		// Token: 0x06000868 RID: 2152
		protected abstract void BlendBoth(ProfileBlendingState state, BlendingHelper helper);

		// Token: 0x06000869 RID: 2153
		protected abstract void BlendIn(ProfileBlendingState state, BlendingHelper helper);

		// Token: 0x0600086A RID: 2154
		protected abstract void BlendOut(ProfileBlendingState state, BlendingHelper helper);

		// Token: 0x0600086B RID: 2155 RVA: 0x000269CB File Offset: 0x00024BCB
		protected virtual ProfileFeatureBlendingMode BlendingMode(ProfileBlendingState state, BlendingHelper helper)
		{
			return helper.GetFeatureAnimationMode(this.featureKey);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000269DC File Offset: 0x00024BDC
		public virtual void Blend(ProfileBlendingState state, BlendingHelper helper)
		{
			switch (this.BlendingMode(state, helper))
			{
			case ProfileFeatureBlendingMode.Normal:
				this.BlendBoth(state, helper);
				return;
			case ProfileFeatureBlendingMode.FadeFeatureOut:
				this.BlendOut(state, helper);
				return;
			case ProfileFeatureBlendingMode.FadeFeatureIn:
				this.BlendIn(state, helper);
				return;
			default:
				return;
			}
		}
	}
}
