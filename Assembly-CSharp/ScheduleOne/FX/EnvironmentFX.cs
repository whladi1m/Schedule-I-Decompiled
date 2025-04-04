using System;
using AtmosphericHeightFog;
using Funly.SkyStudio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Tools;
using UnityEngine;
using VolumetricFogAndMist2;

namespace ScheduleOne.FX
{
	// Token: 0x02000611 RID: 1553
	[ExecuteInEditMode]
	public class EnvironmentFX : Singleton<EnvironmentFX>
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000A8A4E File Offset: 0x000A6C4E
		public float normalizedEnvironmentalBrightness
		{
			get
			{
				return this.environmentalBrightnessCurve.Evaluate(((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal + NetworkSingleton<TimeManager>.Instance.TimeOnCurrentMinute / 1f) / 1440f);
			}
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000A8A80 File Offset: 0x000A6C80
		protected override void Start()
		{
			base.Start();
			this.UpdateVisuals();
			this.FogEndDistanceController = new FloatSmoother();
			this.FogEndDistanceController.Initialize();
			this.FogEndDistanceController.SetSmoothingSpeed(0.2f);
			this.FogEndDistanceController.SetDefault(1f);
			if (Application.isPlaying && !this.started)
			{
				this.started = true;
				base.InvokeRepeating("UpdateVisuals", 0f, 0.1f);
			}
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000A8AFC File Offset: 0x000A6CFC
		private void Update()
		{
			if (Application.isEditor)
			{
				byte b = (byte)this.distanceTreeColorCurve.Evaluate(this.timeOfDayController.skyTime);
				this.distanceTreeMat.SetColor("_TintColor", new Color32(b, b, b, byte.MaxValue));
				this.grassMat.color = this.grassColorGradient.Evaluate(this.timeOfDayController.skyTime);
			}
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000A8B6C File Offset: 0x000A6D6C
		private void UpdateVisuals()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			float num = (float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal + NetworkSingleton<TimeManager>.Instance.TimeOnCurrentMinute / 1f;
			this.timeOfDayController.skyTime = num / 1440f;
			RenderSettings.fogColor = this.fogColorGradient.Evaluate(this.timeOfDayController.skyTime);
			RenderSettings.fogEndDistance = this.fogEndDistanceCurve.Evaluate(this.timeOfDayController.skyTime) * this.fogEndDistanceMultiplier * this.FogEndDistanceController.CurrentValue;
			this.HeightFog.fogColorStart = this.HeightFogColor.Evaluate(this.timeOfDayController.skyTime);
			this.HeightFog.fogColorEnd = this.HeightFogColor.Evaluate(this.timeOfDayController.skyTime);
			this.HeightFog.fogIntensity = this.HeightFogIntensityCurve.Evaluate(this.timeOfDayController.skyTime);
			this.HeightFog.directionalIntensity = this.HeightFogDirectionalIntensityCurve.Evaluate(this.timeOfDayController.skyTime);
			Color albedo = this.VolumetricFogColor.Evaluate(this.timeOfDayController.skyTime);
			albedo.a = this.VolumetricFogIntensityCurve.Evaluate(this.timeOfDayController.skyTime) * this.VolumetricFogIntensityMultiplier;
			this.VolumetricFog.profile.albedo = albedo;
			byte b = (byte)this.distanceTreeColorCurve.Evaluate(num / 1440f);
			this.distanceTreeMat.SetColor("_TintColor", new Color32(b, b, b, byte.MaxValue));
			this.grassMat.color = this.grassColorGradient.Evaluate(this.timeOfDayController.skyTime);
			Singleton<PostProcessingManager>.Instance.SetGodRayIntensity(this.godRayIntensityCurve.Evaluate(this.timeOfDayController.skyTime));
			Singleton<PostProcessingManager>.Instance.SetContrast(this.contrastCurve.Evaluate(this.timeOfDayController.skyTime) * this.contractMultiplier);
			Singleton<PostProcessingManager>.Instance.SetSaturation(this.saturationCurve.Evaluate(this.timeOfDayController.skyTime) * this.saturationMultiplier);
			Singleton<PostProcessingManager>.Instance.SetBloomThreshold(this.bloomThreshholdCurve.Evaluate(this.timeOfDayController.skyTime));
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000A8DAE File Offset: 0x000A6FAE
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x04001E14 RID: 7700
		[Header("References")]
		[SerializeField]
		protected WindZone windZone;

		// Token: 0x04001E15 RID: 7701
		[SerializeField]
		protected TimeOfDayController timeOfDayController;

		// Token: 0x04001E16 RID: 7702
		public HeightFogGlobal HeightFog;

		// Token: 0x04001E17 RID: 7703
		public VolumetricFog VolumetricFog;

		// Token: 0x04001E18 RID: 7704
		public Light SunLight;

		// Token: 0x04001E19 RID: 7705
		public Light MoonLight;

		// Token: 0x04001E1A RID: 7706
		[Header("Fog")]
		[SerializeField]
		protected Gradient fogColorGradient;

		// Token: 0x04001E1B RID: 7707
		[SerializeField]
		protected AnimationCurve fogEndDistanceCurve;

		// Token: 0x04001E1C RID: 7708
		[SerializeField]
		protected float fogEndDistanceMultiplier = 0.01f;

		// Token: 0x04001E1D RID: 7709
		[Header("Height Fog")]
		[SerializeField]
		protected Gradient HeightFogColor;

		// Token: 0x04001E1E RID: 7710
		[SerializeField]
		protected AnimationCurve HeightFogIntensityCurve;

		// Token: 0x04001E1F RID: 7711
		[SerializeField]
		protected float HeightFogIntensityMultiplier = 0.5f;

		// Token: 0x04001E20 RID: 7712
		[SerializeField]
		protected AnimationCurve HeightFogDirectionalIntensityCurve;

		// Token: 0x04001E21 RID: 7713
		[Header("Volumetric Fog")]
		[SerializeField]
		protected Gradient VolumetricFogColor;

		// Token: 0x04001E22 RID: 7714
		[SerializeField]
		protected AnimationCurve VolumetricFogIntensityCurve;

		// Token: 0x04001E23 RID: 7715
		[SerializeField]
		protected float VolumetricFogIntensityMultiplier = 0.5f;

		// Token: 0x04001E24 RID: 7716
		[Header("God rays")]
		[SerializeField]
		protected AnimationCurve godRayIntensityCurve;

		// Token: 0x04001E25 RID: 7717
		[Header("Contrast")]
		[SerializeField]
		protected AnimationCurve contrastCurve;

		// Token: 0x04001E26 RID: 7718
		[SerializeField]
		protected float contractMultiplier = 1f;

		// Token: 0x04001E27 RID: 7719
		[Header("Saturation")]
		[SerializeField]
		protected AnimationCurve saturationCurve;

		// Token: 0x04001E28 RID: 7720
		[SerializeField]
		protected float saturationMultiplier = 1f;

		// Token: 0x04001E29 RID: 7721
		[Header("Grass")]
		[SerializeField]
		protected Material grassMat;

		// Token: 0x04001E2A RID: 7722
		[SerializeField]
		protected Gradient grassColorGradient;

		// Token: 0x04001E2B RID: 7723
		[Header("Trees")]
		public Material distanceTreeMat;

		// Token: 0x04001E2C RID: 7724
		public AnimationCurve distanceTreeColorCurve;

		// Token: 0x04001E2D RID: 7725
		[Header("Stealth settings")]
		public AnimationCurve environmentalBrightnessCurve;

		// Token: 0x04001E2E RID: 7726
		[Header("Bloom")]
		public AnimationCurve bloomThreshholdCurve;

		// Token: 0x04001E2F RID: 7727
		private bool started;

		// Token: 0x04001E30 RID: 7728
		public FloatSmoother FogEndDistanceController;
	}
}
