using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x0200019C RID: 412
	public class BlendSkyProfiles : MonoBehaviour
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x00026615 File Offset: 0x00024815
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x0002661D File Offset: 0x0002481D
		public SkyProfile fromProfile { get; private set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x00026626 File Offset: 0x00024826
		// (set) Token: 0x0600085D RID: 2141 RVA: 0x0002662E File Offset: 0x0002482E
		public SkyProfile toProfile { get; private set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00026637 File Offset: 0x00024837
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x0002663F File Offset: 0x0002483F
		public SkyProfile blendedProfile { get; private set; }

		// Token: 0x06000860 RID: 2144 RVA: 0x00026648 File Offset: 0x00024848
		public SkyProfile StartBlending(TimeOfDayController controller, SkyProfile fromProfile, SkyProfile toProfile, float duration)
		{
			if (controller == null)
			{
				Debug.LogWarning("Can't transition with null TimeOfDayController");
				return null;
			}
			if (fromProfile == null)
			{
				Debug.LogWarning("Can't transition to null 'from' sky profile.");
				return null;
			}
			if (toProfile == null)
			{
				Debug.LogWarning("Can't transition to null 'to' sky profile");
				return null;
			}
			if (!fromProfile.IsFeatureEnabled("GradientSkyFeature", true) || !toProfile.IsFeatureEnabled("GradientSkyFeature", true))
			{
				Debug.LogWarning("Sky Studio doesn't currently support automatic transition blending with cubemap backgrounds.");
			}
			this.m_TimeOfDayController = controller;
			this.fromProfile = fromProfile;
			this.toProfile = toProfile;
			this.m_StartTime = Time.time;
			this.m_EndTime = this.m_StartTime + duration;
			this.blendedProfile = UnityEngine.Object.Instantiate<SkyProfile>(fromProfile);
			this.blendedProfile.skyboxMaterial = fromProfile.skyboxMaterial;
			this.m_TimeOfDayController.skyProfile = this.blendedProfile;
			this.m_State = new ProfileBlendingState(this.blendedProfile, fromProfile, toProfile, 0f, 0f, 0f, this.m_TimeOfDayController.timeOfDay);
			this.blendingHelper = new BlendingHelper(this.m_State);
			this.UpdateBlendedProfile();
			return this.blendedProfile;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00026761 File Offset: 0x00024961
		public void CancelBlending()
		{
			this.TearDownBlending();
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00026769 File Offset: 0x00024969
		public void TearDownBlending()
		{
			if (this.m_TimeOfDayController == null)
			{
				return;
			}
			this.m_TimeOfDayController = null;
			this.blendedProfile = null;
			base.enabled = false;
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0002679A File Offset: 0x0002499A
		private void Update()
		{
			if (this.blendedProfile == null)
			{
				return;
			}
			this.UpdateBlendedProfile();
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x000267B4 File Offset: 0x000249B4
		private void UpdateBlendedProfile()
		{
			if (this.m_TimeOfDayController == null)
			{
				return;
			}
			float num = this.m_EndTime - this.m_StartTime;
			float num2 = Time.time - this.m_StartTime;
			this.m_State.progress = num2 / num;
			this.m_State.inProgress = this.PercentForMode(ProfileFeatureBlendingMode.FadeFeatureIn, this.m_State.progress);
			this.m_State.outProgress = this.PercentForMode(ProfileFeatureBlendingMode.FadeFeatureOut, this.m_State.progress);
			this.blendingHelper.UpdateState(this.m_State);
			if (this.m_State.progress > 0.5f && this.m_IsBlendingFirstHalf)
			{
				this.m_IsBlendingFirstHalf = false;
				this.blendedProfile = UnityEngine.Object.Instantiate<SkyProfile>(this.toProfile);
				this.m_State.blendedProfile = this.blendedProfile;
				this.m_TimeOfDayController.skyProfile = this.blendedProfile;
			}
			this.blendingHelper.UpdateState(this.m_State);
			foreach (FeatureBlender featureBlender in new FeatureBlender[]
			{
				this.skyBlender,
				this.sunBlender,
				this.moonBlender,
				this.cloudBlender,
				this.starLayer1Blender,
				this.starLayer2Blender,
				this.starLayer3Blender,
				this.rainBlender,
				this.rainSplashBlender,
				this.lightningBlender,
				this.fogBlender
			})
			{
				if (!(featureBlender == null))
				{
					featureBlender.Blend(this.m_State, this.blendingHelper);
				}
			}
			this.m_TimeOfDayController.skyProfile = this.blendedProfile;
			if (this.m_State.progress >= 1f)
			{
				this.onBlendComplete(this);
				this.TearDownBlending();
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0002697B File Offset: 0x00024B7B
		private float PercentForMode(ProfileFeatureBlendingMode mode, float percent)
		{
			if (mode == ProfileFeatureBlendingMode.FadeFeatureOut)
			{
				return Mathf.Clamp01(percent * 2f);
			}
			if (mode == ProfileFeatureBlendingMode.FadeFeatureIn)
			{
				return Mathf.Clamp01((percent - 0.5f) * 2f);
			}
			return percent;
		}

		// Token: 0x04000934 RID: 2356
		[Tooltip("Called when blending finishes.")]
		public Action<BlendSkyProfiles> onBlendComplete;

		// Token: 0x04000935 RID: 2357
		[HideInInspector]
		private float m_StartTime = -1f;

		// Token: 0x04000936 RID: 2358
		[HideInInspector]
		private float m_EndTime = -1f;

		// Token: 0x04000937 RID: 2359
		[Tooltip("Blender used for basic sky background properties.")]
		public FeatureBlender skyBlender;

		// Token: 0x04000938 RID: 2360
		[Tooltip("Blender used for the sun properties.")]
		public FeatureBlender sunBlender;

		// Token: 0x04000939 RID: 2361
		[Tooltip("Blender used moon properties.")]
		public FeatureBlender moonBlender;

		// Token: 0x0400093A RID: 2362
		[Tooltip("Blender used cloud properties.")]
		public FeatureBlender cloudBlender;

		// Token: 0x0400093B RID: 2363
		[Tooltip("Blender used star layer 1 properties.")]
		public FeatureBlender starLayer1Blender;

		// Token: 0x0400093C RID: 2364
		[Tooltip("Blender used star layer 2 properties.")]
		public FeatureBlender starLayer2Blender;

		// Token: 0x0400093D RID: 2365
		[Tooltip("Blender used star layer 3 properties.")]
		public FeatureBlender starLayer3Blender;

		// Token: 0x0400093E RID: 2366
		[Tooltip("Blender used by the rain downfall feature.")]
		public FeatureBlender rainBlender;

		// Token: 0x0400093F RID: 2367
		[Tooltip("Blender used by the rain splash feature.")]
		public FeatureBlender rainSplashBlender;

		// Token: 0x04000940 RID: 2368
		[Tooltip("Blender used for lightning feature properties.")]
		public FeatureBlender lightningBlender;

		// Token: 0x04000941 RID: 2369
		[Tooltip("Blender used for fog properties.")]
		public FeatureBlender fogBlender;

		// Token: 0x04000942 RID: 2370
		private bool m_IsBlendingFirstHalf = true;

		// Token: 0x04000943 RID: 2371
		private ProfileBlendingState m_State;

		// Token: 0x04000944 RID: 2372
		private TimeOfDayController m_TimeOfDayController;

		// Token: 0x04000945 RID: 2373
		private BlendingHelper blendingHelper;
	}
}
