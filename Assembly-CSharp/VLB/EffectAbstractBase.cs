using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x020000F0 RID: 240
	[AddComponentMenu("")]
	public class EffectAbstractBase : MonoBehaviour
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00015A2B File Offset: 0x00013C2B
		// (set) Token: 0x060003DA RID: 986 RVA: 0x00015A33 File Offset: 0x00013C33
		[Obsolete("Use 'restoreIntensityOnDisable' instead")]
		public bool restoreBaseIntensity
		{
			get
			{
				return this.restoreIntensityOnDisable;
			}
			set
			{
				this.restoreIntensityOnDisable = value;
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00015A3C File Offset: 0x00013C3C
		public virtual void InitFrom(EffectAbstractBase Source)
		{
			if (Source)
			{
				this.componentsToChange = Source.componentsToChange;
				this.restoreIntensityOnDisable = Source.restoreIntensityOnDisable;
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00015A5E File Offset: 0x00013C5E
		private void GetIntensity(VolumetricLightBeamSD beam)
		{
			if (beam)
			{
				this.m_BaseIntensityBeamInside = beam.intensityInside;
				this.m_BaseIntensityBeamOutside = beam.intensityOutside;
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00015A80 File Offset: 0x00013C80
		private void GetIntensity(VolumetricLightBeamHD beam)
		{
			if (beam)
			{
				this.m_BaseIntensityBeamOutside = beam.intensity;
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00015A96 File Offset: 0x00013C96
		private void SetIntensity(VolumetricLightBeamSD beam, float additive)
		{
			if (beam)
			{
				beam.intensityInside = Mathf.Max(0f, this.m_BaseIntensityBeamInside + additive);
				beam.intensityOutside = Mathf.Max(0f, this.m_BaseIntensityBeamOutside + additive);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00015AD0 File Offset: 0x00013CD0
		private void SetIntensity(VolumetricLightBeamHD beam, float additive)
		{
			if (beam)
			{
				beam.intensity = Mathf.Max(0f, this.m_BaseIntensityBeamOutside + additive);
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00015AF4 File Offset: 0x00013CF4
		protected void SetAdditiveIntensity(float additive)
		{
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.VolumetricLightBeam) && this.m_Beam)
			{
				this.SetIntensity(this.m_Beam as VolumetricLightBeamSD, additive);
				this.SetIntensity(this.m_Beam as VolumetricLightBeamHD, additive);
			}
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.UnityLight) && this.m_Light)
			{
				this.m_Light.intensity = Mathf.Max(0f, this.m_BaseIntensityLight + additive);
			}
			if (this.componentsToChange.HasFlag(EffectAbstractBase.ComponentsToChange.VolumetricDustParticles) && this.m_Particles)
			{
				this.m_Particles.alphaAdditionalRuntime = 1f + additive;
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00015BC4 File Offset: 0x00013DC4
		private void Awake()
		{
			this.m_Beam = base.GetComponent<VolumetricLightBeamAbstractBase>();
			this.m_Light = base.GetComponent<Light>();
			this.m_Particles = base.GetComponent<VolumetricDustParticles>();
			this.GetIntensity(this.m_Beam as VolumetricLightBeamSD);
			this.GetIntensity(this.m_Beam as VolumetricLightBeamHD);
			this.m_BaseIntensityLight = (this.m_Light ? this.m_Light.intensity : 0f);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00015C3C File Offset: 0x00013E3C
		protected virtual void OnEnable()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00015C44 File Offset: 0x00013E44
		private void OnDisable()
		{
			base.StopAllCoroutines();
			if (this.restoreIntensityOnDisable)
			{
				this.SetAdditiveIntensity(0f);
			}
		}

		// Token: 0x0400054A RID: 1354
		public const string ClassName = "EffectAbstractBase";

		// Token: 0x0400054B RID: 1355
		public EffectAbstractBase.ComponentsToChange componentsToChange = (EffectAbstractBase.ComponentsToChange)2147483647;

		// Token: 0x0400054C RID: 1356
		[FormerlySerializedAs("restoreBaseIntensity")]
		public bool restoreIntensityOnDisable = true;

		// Token: 0x0400054D RID: 1357
		protected VolumetricLightBeamAbstractBase m_Beam;

		// Token: 0x0400054E RID: 1358
		protected Light m_Light;

		// Token: 0x0400054F RID: 1359
		protected VolumetricDustParticles m_Particles;

		// Token: 0x04000550 RID: 1360
		protected float m_BaseIntensityBeamInside;

		// Token: 0x04000551 RID: 1361
		protected float m_BaseIntensityBeamOutside;

		// Token: 0x04000552 RID: 1362
		protected float m_BaseIntensityLight;

		// Token: 0x020000F1 RID: 241
		[Flags]
		public enum ComponentsToChange
		{
			// Token: 0x04000554 RID: 1364
			UnityLight = 1,
			// Token: 0x04000555 RID: 1365
			VolumetricLightBeam = 2,
			// Token: 0x04000556 RID: 1366
			VolumetricDustParticles = 4
		}
	}
}
