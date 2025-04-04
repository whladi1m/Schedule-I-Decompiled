using System;
using Beautify.Universal;
using CorgiGodRays;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ScheduleOne.FX
{
	// Token: 0x02000617 RID: 1559
	public class PostProcessingManager : Singleton<PostProcessingManager>
	{
		// Token: 0x06002905 RID: 10501 RVA: 0x000A9370 File Offset: 0x000A7570
		protected override void Awake()
		{
			base.Awake();
			this.GlobalVolume.enabled = true;
			this.GlobalVolume.sharedProfile.TryGet<Vignette>(out this.vig);
			this.ResetVignette();
			this.GlobalVolume.sharedProfile.TryGet<DepthOfField>(out this.DoF);
			this.DoF.active = false;
			this.GlobalVolume.sharedProfile.TryGet<GodRaysVolume>(out this.GodRays);
			this.GlobalVolume.sharedProfile.TryGet<ColorAdjustments>(out this.ColorAdjustments);
			this.GlobalVolume.sharedProfile.TryGet<Beautify>(out this.beautifySettings);
			this.GlobalVolume.sharedProfile.TryGet<Bloom>(out this.bloom);
			this.GlobalVolume.sharedProfile.TryGet<ChromaticAberration>(out this.chromaticAberration);
			this.GlobalVolume.sharedProfile.TryGet<ColorAdjustments>(out this.colorAdjustments);
			this.ChromaticAberrationController.Initialize();
			this.SaturationController.Initialize();
			this.BloomController.Initialize();
			this.ColorFilterController.Initialize();
			this.SetBlur(0f);
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000A9490 File Offset: 0x000A7690
		public void Update()
		{
			this.UpdateEffects();
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x000A9498 File Offset: 0x000A7698
		private void UpdateEffects()
		{
			float num = Mathf.Lerp(1f, 12f, PlayerSingleton<PlayerCamera>.InstanceExists ? PlayerSingleton<PlayerCamera>.Instance.FovJitter : 0f);
			this.chromaticAberration.intensity.value = this.ChromaticAberrationController.CurrentValue * num;
			this.ColorAdjustments.saturation.value = this.SaturationController.CurrentValue;
			this.ColorAdjustments.postExposure.value = 0.1f * num;
			this.bloom.intensity.value = this.BloomController.CurrentValue * num;
			this.colorAdjustments.colorFilter.value = this.ColorFilterController.CurrentValue;
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000A9554 File Offset: 0x000A7754
		public void OverrideVignette(float intensity, float smoothness)
		{
			this.vig.intensity.value = intensity;
			this.vig.smoothness.value = smoothness;
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000A9578 File Offset: 0x000A7778
		public void ResetVignette()
		{
			this.vig.intensity.value = this.Vig_DefaultIntensity;
			this.vig.smoothness.value = this.Vig_DefaultSmoothness;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000A95A6 File Offset: 0x000A77A6
		public void SetGodRayIntensity(float intensity)
		{
			this.GodRays.MainLightIntensity.value = intensity;
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000A95B9 File Offset: 0x000A77B9
		public void SetContrast(float value)
		{
			this.ColorAdjustments.contrast.value = value;
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000A95CC File Offset: 0x000A77CC
		public void SetSaturation(float value)
		{
			this.SaturationController.SetDefault(value);
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000A95DA File Offset: 0x000A77DA
		public void SetBloomThreshold(float threshold)
		{
			this.bloom.threshold.value = threshold;
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000A95ED File Offset: 0x000A77ED
		public void SetBlur(float blurLevel)
		{
			this.beautifySettings.blurIntensity.value = Mathf.Lerp(this.MinBlur, this.MaxBlur, blurLevel);
		}

		// Token: 0x04001E49 RID: 7753
		[Header("References")]
		public Volume GlobalVolume;

		// Token: 0x04001E4A RID: 7754
		[Header("Vignette")]
		public float Vig_DefaultIntensity = 0.25f;

		// Token: 0x04001E4B RID: 7755
		public float Vig_DefaultSmoothness = 0.3f;

		// Token: 0x04001E4C RID: 7756
		[Header("Blur")]
		public float MinBlur;

		// Token: 0x04001E4D RID: 7757
		public float MaxBlur = 1f;

		// Token: 0x04001E4E RID: 7758
		[Header("Smoothers")]
		public FloatSmoother ChromaticAberrationController;

		// Token: 0x04001E4F RID: 7759
		public FloatSmoother SaturationController;

		// Token: 0x04001E50 RID: 7760
		public FloatSmoother BloomController;

		// Token: 0x04001E51 RID: 7761
		public HDRColorSmoother ColorFilterController;

		// Token: 0x04001E52 RID: 7762
		private Vignette vig;

		// Token: 0x04001E53 RID: 7763
		private DepthOfField DoF;

		// Token: 0x04001E54 RID: 7764
		private GodRaysVolume GodRays;

		// Token: 0x04001E55 RID: 7765
		private ColorAdjustments ColorAdjustments;

		// Token: 0x04001E56 RID: 7766
		private Beautify beautifySettings;

		// Token: 0x04001E57 RID: 7767
		private Bloom bloom;

		// Token: 0x04001E58 RID: 7768
		private ChromaticAberration chromaticAberration;

		// Token: 0x04001E59 RID: 7769
		private ColorAdjustments colorAdjustments;
	}
}
