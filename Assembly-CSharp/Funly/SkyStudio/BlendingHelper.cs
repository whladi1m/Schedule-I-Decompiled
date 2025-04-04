using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x02000191 RID: 401
	public class BlendingHelper
	{
		// Token: 0x0600081A RID: 2074 RVA: 0x00025CBB File Offset: 0x00023EBB
		public BlendingHelper(ProfileBlendingState state)
		{
			this.m_State = state;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00025CCA File Offset: 0x00023ECA
		public void UpdateState(ProfileBlendingState state)
		{
			this.m_State = state;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00025CD4 File Offset: 0x00023ED4
		public Color ProfileColorForKey(SkyProfile profile, string key)
		{
			float time = (profile == this.m_State.toProfile) ? 0f : this.m_State.timeOfDay;
			return profile.GetGroup<ColorKeyframeGroup>(key).ColorForTime(time);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00025D14 File Offset: 0x00023F14
		public float ProfileNumberForKey(SkyProfile profile, string key)
		{
			float time = (profile == this.m_State.toProfile) ? 0f : this.m_State.timeOfDay;
			return profile.GetGroup<NumberKeyframeGroup>(key).NumericValueAtTime(time);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00025D54 File Offset: 0x00023F54
		public SpherePoint ProfileSpherePointForKey(SkyProfile profile, string key)
		{
			float time = (profile == this.m_State.toProfile) ? 0f : this.m_State.timeOfDay;
			return profile.GetGroup<SpherePointKeyframeGroup>(key).SpherePointForTime(time);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00025D94 File Offset: 0x00023F94
		public void BlendColor(string key)
		{
			this.BlendColor(key, this.ProfileColorForKey(this.m_State.fromProfile, key), this.ProfileColorForKey(this.m_State.toProfile, key), this.m_State.progress);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00025DCC File Offset: 0x00023FCC
		public void BlendColorOut(string key)
		{
			this.BlendColor(key, this.ProfileColorForKey(this.m_State.fromProfile, key), this.ProfileColorForKey(this.m_State.fromProfile, key).Clear(), this.m_State.outProgress);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00025E09 File Offset: 0x00024009
		public void BlendColorIn(string key)
		{
			this.BlendColor(key, this.ProfileColorForKey(this.m_State.toProfile, key).Clear(), this.ProfileColorForKey(this.m_State.toProfile, key), this.m_State.inProgress);
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00025E46 File Offset: 0x00024046
		public void BlendColor(string key, Color from, Color to, float progress)
		{
			this.m_State.blendedProfile.GetGroup<ColorKeyframeGroup>(key).keyframes[0].color = Color.LerpUnclamped(from, to, progress);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00025E72 File Offset: 0x00024072
		public void BlendNumber(string key)
		{
			this.BlendNumber(key, this.ProfileNumberForKey(this.m_State.fromProfile, key), this.ProfileNumberForKey(this.m_State.toProfile, key), this.m_State.progress);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00025EAA File Offset: 0x000240AA
		public void BlendNumberOut(string key, float toValue = 0f)
		{
			this.BlendNumber(key, this.ProfileNumberForKey(this.m_State.fromProfile, key), toValue, this.m_State.outProgress);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00025ED1 File Offset: 0x000240D1
		public void BlendNumberIn(string key, float fromValue = 0f)
		{
			this.BlendNumber(key, fromValue, this.ProfileNumberForKey(this.m_State.toProfile, key), this.m_State.inProgress);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00025EF8 File Offset: 0x000240F8
		public void BlendNumber(string key, float from, float to, float progress)
		{
			this.m_State.blendedProfile.GetGroup<NumberKeyframeGroup>(key).keyframes[0].value = Mathf.Lerp(from, to, progress);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00025F24 File Offset: 0x00024124
		public void BlendSpherePoint(string key)
		{
			this.BlendSpherePoint(key, this.ProfileSpherePointForKey(this.m_State.fromProfile, "MoonPositionKey"), this.ProfileSpherePointForKey(this.m_State.toProfile, "MoonPositionKey"), this.m_State.progress);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00025F64 File Offset: 0x00024164
		public void BlendSpherePoint(string key, SpherePoint from, SpherePoint to, float progress)
		{
			Vector3 vector = Vector3.Slerp(from.GetWorldDirection(), to.GetWorldDirection(), progress);
			this.m_State.blendedProfile.GetGroup<SpherePointKeyframeGroup>(key).keyframes[0].spherePoint = new SpherePoint(vector.normalized);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00025FB4 File Offset: 0x000241B4
		public ProfileFeatureBlendingMode GetFeatureAnimationMode(string featureKey)
		{
			bool flag = this.m_State.fromProfile.IsFeatureEnabled(featureKey, true);
			bool flag2 = this.m_State.toProfile.IsFeatureEnabled(featureKey, true);
			if (flag && flag2)
			{
				return ProfileFeatureBlendingMode.Normal;
			}
			if (flag && !flag2)
			{
				return ProfileFeatureBlendingMode.FadeFeatureOut;
			}
			if (!flag && flag2)
			{
				return ProfileFeatureBlendingMode.FadeFeatureIn;
			}
			return ProfileFeatureBlendingMode.None;
		}

		// Token: 0x0400092F RID: 2351
		private ProfileBlendingState m_State;
	}
}
