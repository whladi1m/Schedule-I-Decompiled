using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Funly.SkyStudio
{
	// Token: 0x020001C8 RID: 456
	[ExecuteInEditMode]
	public class TimeOfDayController : MonoBehaviour
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0002B980 File Offset: 0x00029B80
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0002B987 File Offset: 0x00029B87
		public static TimeOfDayController instance { get; private set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0002B98F File Offset: 0x00029B8F
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x0002B997 File Offset: 0x00029B97
		public SkyProfile skyProfile
		{
			get
			{
				return this.m_SkyProfile;
			}
			set
			{
				if (value != null && this.copySkyProfile)
				{
					this.m_SkyProfile = UnityEngine.Object.Instantiate<SkyProfile>(value);
				}
				else
				{
					this.m_SkyProfile = value;
				}
				this.m_SkyMaterialController = null;
				this.UpdateSkyForCurrentTime();
				this.SynchronizeAllShaderKeywords();
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0002B9D2 File Offset: 0x00029BD2
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x0002B9DA File Offset: 0x00029BDA
		public float skyTime
		{
			get
			{
				return this.m_SkyTime;
			}
			set
			{
				this.m_SkyTime = Mathf.Abs(value);
				this.UpdateSkyForCurrentTime();
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0002B9EE File Offset: 0x00029BEE
		public SkyMaterialController SkyMaterial
		{
			get
			{
				return this.m_SkyMaterialController;
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000A08 RID: 2568 RVA: 0x0002B9F8 File Offset: 0x00029BF8
		// (remove) Token: 0x06000A09 RID: 2569 RVA: 0x0002BA30 File Offset: 0x00029C30
		public event TimeOfDayController.TimeOfDayDidChange timeChangedCallback;

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0002BA65 File Offset: 0x00029C65
		public float timeOfDay
		{
			get
			{
				return this.m_SkyTime - (float)((int)this.m_SkyTime);
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0002BA76 File Offset: 0x00029C76
		public int daysElapsed
		{
			get
			{
				return (int)this.m_SkyTime;
			}
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0002BA7F File Offset: 0x00029C7F
		private void Awake()
		{
			TimeOfDayController.instance = this;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0002BA87 File Offset: 0x00029C87
		private void OnEnabled()
		{
			this.skyTime = this.m_SkyTime;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0002BA95 File Offset: 0x00029C95
		private void OnValidate()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.skyTime = this.m_SkyTime;
			this.skyProfile = this.m_SkyProfile;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0002BABD File Offset: 0x00029CBD
		private void WarnInvalidSkySetup()
		{
			Debug.LogError("Your SkySystemController has an old or invalid prefab layout! Please run the upgrade tool in 'Windows -> Sky Studio -> Upgrade Sky System Controller'. Do not rename or modify any of the children in the SkySystemController hierarchy.");
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0002BACC File Offset: 0x00029CCC
		private void Update()
		{
			if (!this.skyProfile)
			{
				return;
			}
			if (this.automaticTimeIncrement && Application.isPlaying)
			{
				this.skyTime += this.automaticIncrementSpeed * Time.deltaTime;
			}
			if (this.sunOrbit == null || this.moonOrbit == null || this.sunOrbit.rotateBody == null || this.moonOrbit.rotateBody == null || this.sunOrbit.positionTransform == null || this.moonOrbit.positionTransform == null)
			{
				this.WarnInvalidSkySetup();
				return;
			}
			if (!this.m_DidInitialUpdate)
			{
				this.UpdateSkyForCurrentTime();
				this.m_DidInitialUpdate = true;
			}
			if (this.weatherController != null)
			{
				this.weatherController.UpdateForTimeOfDay(this.skyProfile, this.timeOfDay);
			}
			if (this.skyProfile.IsFeatureEnabled("SunFeature", true))
			{
				if (this.sunOrbit.positionTransform)
				{
					this.m_SkyMaterialController.SunWorldToLocalMatrix = this.sunOrbit.positionTransform.worldToLocalMatrix;
				}
				if (this.skyProfile.IsFeatureEnabled("SunCustomTextureFeature", true))
				{
					if (this.skyProfile.IsFeatureEnabled("SunRotationFeature", true))
					{
						this.sunOrbit.rotateBody.AllowSpinning = true;
						this.sunOrbit.rotateBody.SpinSpeed = this.skyProfile.GetNumberPropertyValue("SunRotationSpeedKey", this.timeOfDay);
					}
					else
					{
						this.sunOrbit.rotateBody.AllowSpinning = false;
					}
				}
			}
			if (this.skyProfile.IsFeatureEnabled("MoonFeature", true))
			{
				if (this.moonOrbit.positionTransform)
				{
					this.m_SkyMaterialController.MoonWorldToLocalMatrix = this.moonOrbit.positionTransform.worldToLocalMatrix;
				}
				if (this.skyProfile.IsFeatureEnabled("MoonCustomTextureFeature", true))
				{
					if (this.skyProfile.IsFeatureEnabled("MoonRotationFeature", true))
					{
						this.moonOrbit.rotateBody.AllowSpinning = true;
						this.moonOrbit.rotateBody.SpinSpeed = this.skyProfile.GetNumberPropertyValue("MoonRotationSpeedKey", this.timeOfDay);
						return;
					}
					this.moonOrbit.rotateBody.AllowSpinning = false;
				}
			}
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0002BD1D File Offset: 0x00029F1D
		public void UpdateGlobalIllumination()
		{
			DynamicGI.UpdateEnvironment();
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0002BD24 File Offset: 0x00029F24
		private void SynchronizeAllShaderKeywords()
		{
			if (this.m_SkyProfile == null)
			{
				return;
			}
			ProfileFeatureSection[] features = this.m_SkyProfile.profileDefinition.features;
			for (int i = 0; i < features.Length; i++)
			{
				foreach (ProfileFeatureDefinition profileFeatureDefinition in features[i].featureDefinitions)
				{
					if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeyword)
					{
						this.SynchronizedShaderKeyword(profileFeatureDefinition.featureKey, profileFeatureDefinition.shaderKeyword);
					}
					else if (profileFeatureDefinition.featureType == ProfileFeatureDefinition.FeatureType.ShaderKeywordDropdown)
					{
						for (int k = 0; k < profileFeatureDefinition.featureKeys.Length; k++)
						{
							this.SynchronizedShaderKeyword(profileFeatureDefinition.featureKeys[k], profileFeatureDefinition.shaderKeywords[k]);
						}
					}
				}
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0002BDDC File Offset: 0x00029FDC
		private void SynchronizedShaderKeyword(string featureKey, string shaderKeyword)
		{
			if (this.skyProfile == null || this.skyProfile.skyboxMaterial == null)
			{
				return;
			}
			if (this.skyProfile.IsFeatureEnabled(featureKey, true))
			{
				if (!this.skyProfile.skyboxMaterial.IsKeywordEnabled(shaderKeyword))
				{
					this.skyProfile.skyboxMaterial.EnableKeyword(shaderKeyword);
					return;
				}
			}
			else if (this.skyProfile.skyboxMaterial.IsKeywordEnabled(shaderKeyword))
			{
				this.skyProfile.skyboxMaterial.DisableKeyword(shaderKeyword);
			}
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0002BE64 File Offset: 0x0002A064
		private Vector3 GetPrimaryLightDirection()
		{
			Vector3 bodyGlobalDirection;
			if (this.skyProfile.IsFeatureEnabled("SunFeature", true) && this.sunOrbit)
			{
				bodyGlobalDirection = this.sunOrbit.BodyGlobalDirection;
			}
			else if (this.skyProfile.IsFeatureEnabled("MoonFeature", true) && this.moonOrbit)
			{
				bodyGlobalDirection = this.moonOrbit.BodyGlobalDirection;
			}
			else
			{
				bodyGlobalDirection = new Vector3(0f, 1f, 0f);
			}
			return bodyGlobalDirection;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0002BEE4 File Offset: 0x0002A0E4
		public bool StartSkyProfileTransition(SkyProfile toProfile, float duration = 1f)
		{
			this.CancelSkyProfileTransition();
			if (this.skyProfileTransitionPrefab == null)
			{
				Debug.LogWarning("Can't transition since the skyProfileTransitionPrefab is null");
				return false;
			}
			if (toProfile == null)
			{
				Debug.LogWarning("Can't transition to null profile");
				return false;
			}
			if (this.skyProfile == null)
			{
				Debug.LogWarning("Can't transition to a SkyProfile without a current profile to start from.");
				this.skyProfile = toProfile;
				return false;
			}
			BlendSkyProfiles blendSkyProfiles = UnityEngine.Object.Instantiate<BlendSkyProfiles>(this.skyProfileTransitionPrefab);
			blendSkyProfiles.onBlendComplete = (Action<BlendSkyProfiles>)Delegate.Combine(blendSkyProfiles.onBlendComplete, new Action<BlendSkyProfiles>(this.OnBlendComplete));
			SkyProfile skyProfile = blendSkyProfiles.StartBlending(this, this.skyProfile, toProfile, duration);
			if (skyProfile == null)
			{
				Debug.LogWarning("Failed to create blending profile, check your from/to args.");
				return false;
			}
			this.skyProfile = skyProfile;
			return true;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0002BFA0 File Offset: 0x0002A1A0
		public void CancelSkyProfileTransition()
		{
			BlendSkyProfiles component = base.GetComponent<BlendSkyProfiles>();
			if (component == null)
			{
				return;
			}
			component.CancelBlending();
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0002BFC4 File Offset: 0x0002A1C4
		public void OnBlendComplete(BlendSkyProfiles blender)
		{
			this.skyProfile = blender.toProfile;
			this.skyTime = 0f;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0002BFDD File Offset: 0x0002A1DD
		public bool IsBlendingInProgress()
		{
			return base.GetComponent<BlendSkyProfiles>();
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
		public void UpdateSkyForCurrentTime()
		{
			if (this.skyProfile == null)
			{
				return;
			}
			if (this.skyProfile.skyboxMaterial == null)
			{
				Debug.LogError("Your sky profile is missing a reference to the skybox material.");
				return;
			}
			if (this.m_SkyMaterialController == null)
			{
				this.m_SkyMaterialController = new SkyMaterialController();
			}
			this.m_SkyMaterialController.SkyboxMaterial = this.skyProfile.skyboxMaterial;
			if (RenderSettings.skybox == null || RenderSettings.skybox.GetInstanceID() != this.skyProfile.skyboxMaterial.GetInstanceID())
			{
				RenderSettings.skybox = this.skyProfile.skyboxMaterial;
			}
			this.SynchronizeAllShaderKeywords();
			this.m_SkyMaterialController.BackgroundCubemap = (this.skyProfile.GetTexturePropertyValue("SkyCubemapKey", this.timeOfDay) as Cubemap);
			this.m_SkyMaterialController.SkyColor = this.skyProfile.GetColorPropertyValue("SkyUpperColorKey", this.timeOfDay);
			this.m_SkyMaterialController.SkyMiddleColor = this.skyProfile.GetColorPropertyValue("SkyMiddleColorKey", this.timeOfDay);
			this.m_SkyMaterialController.HorizonColor = this.skyProfile.GetColorPropertyValue("SkyLowerColorKey", this.timeOfDay);
			this.m_SkyMaterialController.GradientFadeBegin = this.skyProfile.GetNumberPropertyValue("HorizonTransitionStartKey", this.timeOfDay);
			this.m_SkyMaterialController.GradientFadeLength = this.skyProfile.GetNumberPropertyValue("HorizonTransitionLengthKey", this.timeOfDay);
			this.m_SkyMaterialController.SkyMiddlePosition = this.skyProfile.GetNumberPropertyValue("SkyMiddleColorPosition", this.timeOfDay);
			this.m_SkyMaterialController.StarFadeBegin = this.skyProfile.GetNumberPropertyValue("StarTransitionStartKey", this.timeOfDay);
			this.m_SkyMaterialController.StarFadeLength = this.skyProfile.GetNumberPropertyValue("StarTransitionLengthKey", this.timeOfDay);
			this.m_SkyMaterialController.HorizonDistanceScale = this.skyProfile.GetNumberPropertyValue("HorizonStarScaleKey", this.timeOfDay);
			if (this.skyProfile.IsFeatureEnabled("AmbientLightGradient", true))
			{
				if (RenderSettings.ambientMode != AmbientMode.Trilight)
				{
					Debug.Log("Sky Profile is using Ambient Light feature, however Unity scene isn't configured for environment gradient. Changing environment to trilight gradient...");
					RenderSettings.ambientMode = AmbientMode.Trilight;
				}
				RenderSettings.ambientSkyColor = this.skyProfile.GetColorPropertyValue("AmbientLightSkyColorKey", this.timeOfDay);
				RenderSettings.ambientEquatorColor = this.skyProfile.GetColorPropertyValue("AmbientLightEquatorColorKey", this.timeOfDay);
				RenderSettings.ambientGroundColor = this.skyProfile.GetColorPropertyValue("AmbientLightGroundColorKey", this.timeOfDay);
			}
			if (this.skyProfile.IsFeatureEnabled("CloudFeature", true))
			{
				this.m_SkyMaterialController.CloudAlpha = this.skyProfile.GetNumberPropertyValue("CloudAlphaKey", this.timeOfDay);
				if (this.skyProfile.IsFeatureEnabled("NoiseCloudFeature", true))
				{
					this.m_SkyMaterialController.CloudTexture = this.skyProfile.GetTexturePropertyValue("CloudNoiseTextureKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudTextureTiling = this.skyProfile.GetNumberPropertyValue("CloudTextureTiling", this.timeOfDay);
					this.m_SkyMaterialController.CloudDensity = this.skyProfile.GetNumberPropertyValue("CloudDensityKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudSpeed = this.skyProfile.GetNumberPropertyValue("CloudSpeedKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudDirection = this.skyProfile.GetNumberPropertyValue("CloudDirectionKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudHeight = this.skyProfile.GetNumberPropertyValue("CloudHeightKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudColor1 = this.skyProfile.GetColorPropertyValue("CloudColor1Key", this.timeOfDay);
					this.m_SkyMaterialController.CloudColor2 = this.skyProfile.GetColorPropertyValue("CloudColor2Key", this.timeOfDay);
					this.m_SkyMaterialController.CloudFadePosition = this.skyProfile.GetNumberPropertyValue("CloudFadePositionKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudFadeAmount = this.skyProfile.GetNumberPropertyValue("CloudFadeAmountKey", this.timeOfDay);
				}
				else if (this.skyProfile.IsFeatureEnabled("CubemapCloudFeature", true))
				{
					this.m_SkyMaterialController.CloudCubemap = this.skyProfile.GetTexturePropertyValue("CloudCubemapTextureKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudCubemapRotationSpeed = this.skyProfile.GetNumberPropertyValue("CloudCubemapRotationSpeedKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudCubemapTintColor = this.skyProfile.GetColorPropertyValue("CloudCubemapTintColorKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudCubemapHeight = this.skyProfile.GetNumberPropertyValue("CloudCubemapHeightKey", this.timeOfDay);
					if (this.skyProfile.IsFeatureEnabled("CubemapCloudDoubleLayerFeature", true))
					{
						this.m_SkyMaterialController.CloudCubemapDoubleLayerHeight = this.skyProfile.GetNumberPropertyValue("CloudCubemapDoubleLayerHeightKey", this.timeOfDay);
						this.m_SkyMaterialController.CloudCubemapDoubleLayerRotationSpeed = this.skyProfile.GetNumberPropertyValue("CloudCubemapDoubleLayerRotationSpeedKey", this.timeOfDay);
						this.m_SkyMaterialController.CloudCubemapDoubleLayerTintColor = this.skyProfile.GetColorPropertyValue("CloudCubemapDoubleLayerTintColorKey", this.timeOfDay);
						if (this.skyProfile.IsFeatureEnabled("CubemapCloudDoubleLayerCubemap", true))
						{
							this.m_SkyMaterialController.CloudCubemapDoubleLayerCustomTexture = this.skyProfile.GetTexturePropertyValue("CloudCubemapDoubleLayerCustomTextureKey", this.timeOfDay);
						}
					}
				}
				else if (this.skyProfile.IsFeatureEnabled("CubemapNormalCloudFeature", true))
				{
					this.m_SkyMaterialController.CloudCubemapNormalLightDirection = this.GetPrimaryLightDirection();
					this.m_SkyMaterialController.CloudCubemapNormalTexture = this.skyProfile.GetTexturePropertyValue("CloudCubemapNormalTextureKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudCubemapNormalLitColor = this.skyProfile.GetColorPropertyValue("CloudCubemapNormalLitColorKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudCubemapNormalShadowColor = this.skyProfile.GetColorPropertyValue("CloudCubemapNormalShadowColorKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudCubemapNormalAmbientIntensity = this.skyProfile.GetNumberPropertyValue("CloudCubemapNormalAmbientIntensityKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudCubemapNormalHeight = this.skyProfile.GetNumberPropertyValue("CloudCubemapNormalHeightKey", this.timeOfDay);
					this.m_SkyMaterialController.CloudCubemapNormalRotationSpeed = this.skyProfile.GetNumberPropertyValue("CloudCubemapNormalRotationSpeedKey", this.timeOfDay);
					if (this.skyProfile.IsFeatureEnabled("CubemapNormalCloudDoubleLayerFeature", true))
					{
						this.m_SkyMaterialController.CloudCubemapNormalDoubleLayerHeight = this.skyProfile.GetNumberPropertyValue("CloudCubemapNormalDoubleLayerHeightKey", this.timeOfDay);
						this.m_SkyMaterialController.CloudCubemapNormalDoubleLayerRotationSpeed = this.skyProfile.GetNumberPropertyValue("CloudCubemapNormalDoubleLayerRotationSpeedKey", this.timeOfDay);
						this.m_SkyMaterialController.CloudCubemapNormalDoubleLayerLitColor = this.skyProfile.GetColorPropertyValue("CloudCubemapNormalDoubleLayerLitColorKey", this.timeOfDay);
						this.m_SkyMaterialController.CloudCubemapNormalDoubleLayerShadowColor = this.skyProfile.GetColorPropertyValue("CloudCubemapNormalDoubleLayerShadowKey", this.timeOfDay);
						if (this.skyProfile.IsFeatureEnabled("CubemapNormalCloudDoubleLayerCubemap", true))
						{
							this.m_SkyMaterialController.CloudCubemapNormalDoubleLayerCustomTexture = this.skyProfile.GetTexturePropertyValue("CloudCubemapNormalDoubleLayerCustomTextureKey", this.timeOfDay);
						}
					}
				}
			}
			if (this.skyProfile.IsFeatureEnabled("FogFeature", true))
			{
				Color colorPropertyValue = this.skyProfile.GetColorPropertyValue("FogColorKey", this.timeOfDay);
				this.m_SkyMaterialController.FogColor = colorPropertyValue;
				this.m_SkyMaterialController.FogDensity = this.skyProfile.GetNumberPropertyValue("FogDensityKey", this.timeOfDay);
				this.m_SkyMaterialController.FogHeight = this.skyProfile.GetNumberPropertyValue("FogLengthKey", this.timeOfDay);
				if (this.skyProfile.GetBoolPropertyValue("FogSyncWithGlobal", this.timeOfDay))
				{
					RenderSettings.fogColor = colorPropertyValue;
				}
			}
			if (this.skyProfile.IsFeatureEnabled("SunFeature", true) && this.sunOrbit)
			{
				this.sunOrbit.Point = this.skyProfile.GetSpherePointPropertyValue("SunPositionKey", this.timeOfDay);
				this.m_SkyMaterialController.SunDirection = this.sunOrbit.BodyGlobalDirection;
				this.m_SkyMaterialController.SunColor = this.skyProfile.GetColorPropertyValue("SunColorKey", this.timeOfDay);
				this.m_SkyMaterialController.SunSize = this.skyProfile.GetNumberPropertyValue("SunSizeKey", this.timeOfDay);
				this.m_SkyMaterialController.SunEdgeFeathering = this.skyProfile.GetNumberPropertyValue("SunEdgeFeatheringKey", this.timeOfDay);
				this.m_SkyMaterialController.SunBloomFilterBoost = this.skyProfile.GetNumberPropertyValue("SunColorIntensityKey", this.timeOfDay);
				this.m_SkyMaterialController.SunAlpha = this.skyProfile.GetNumberPropertyValue("SunAlphaKey", this.timeOfDay);
				if (this.skyProfile.IsFeatureEnabled("SunCustomTextureFeature", true))
				{
					this.m_SkyMaterialController.SunWorldToLocalMatrix = this.sunOrbit.positionTransform.worldToLocalMatrix;
					this.m_SkyMaterialController.SunTexture = this.skyProfile.GetTexturePropertyValue("SunTextureKey", this.timeOfDay);
					if (this.skyProfile.IsFeatureEnabled("SunRotationFeature", true))
					{
						this.sunOrbit.rotateBody.SpinSpeed = this.skyProfile.GetNumberPropertyValue("SunRotationSpeedKey", this.timeOfDay);
					}
				}
				if (this.skyProfile.IsFeatureEnabled("SunSpriteSheetFeature", true))
				{
					this.m_SkyMaterialController.SetSunSpriteDimensions((int)this.skyProfile.GetNumberPropertyValue("SunSpriteColumnCountKey", this.timeOfDay), (int)this.skyProfile.GetNumberPropertyValue("SunSpriteRowCountKey", this.timeOfDay));
					this.m_SkyMaterialController.SunSpriteItemCount = (int)this.skyProfile.GetNumberPropertyValue("SunSpriteItemCount", this.timeOfDay);
					this.m_SkyMaterialController.SunSpriteAnimationSpeed = this.skyProfile.GetNumberPropertyValue("SunSpriteAnimationSpeed", this.timeOfDay);
				}
				if (this.sunOrbit.BodyLight)
				{
					if (!this.sunOrbit.BodyLight.enabled)
					{
						this.sunOrbit.BodyLight.enabled = true;
					}
					RenderSettings.sun = this.sunOrbit.BodyLight;
					this.sunOrbit.BodyLight.color = this.skyProfile.GetColorPropertyValue("SunLightColorKey", this.timeOfDay);
					this.sunOrbit.BodyLight.intensity = this.skyProfile.GetNumberPropertyValue("SunLightIntensityKey", this.timeOfDay);
				}
			}
			else if (this.sunOrbit && this.sunOrbit.BodyLight)
			{
				this.sunOrbit.BodyLight.enabled = false;
			}
			if (this.skyProfile.IsFeatureEnabled("MoonFeature", true) && this.moonOrbit)
			{
				this.moonOrbit.Point = this.skyProfile.GetSpherePointPropertyValue("MoonPositionKey", this.timeOfDay);
				this.m_SkyMaterialController.MoonDirection = this.moonOrbit.BodyGlobalDirection;
				this.m_SkyMaterialController.MoonColor = this.skyProfile.GetColorPropertyValue("MoonColorKey", this.timeOfDay);
				this.m_SkyMaterialController.MoonSize = this.skyProfile.GetNumberPropertyValue("MoonSizeKey", this.timeOfDay);
				this.m_SkyMaterialController.MoonEdgeFeathering = this.skyProfile.GetNumberPropertyValue("MoonEdgeFeatheringKey", this.timeOfDay);
				this.m_SkyMaterialController.MoonBloomFilterBoost = this.skyProfile.GetNumberPropertyValue("MoonColorIntensityKey", this.timeOfDay);
				this.m_SkyMaterialController.MoonAlpha = this.skyProfile.GetNumberPropertyValue("MoonAlphaKey", this.timeOfDay);
				if (this.skyProfile.IsFeatureEnabled("MoonCustomTextureFeature", true))
				{
					this.m_SkyMaterialController.MoonTexture = this.skyProfile.GetTexturePropertyValue("MoonTextureKey", this.timeOfDay);
					this.m_SkyMaterialController.MoonWorldToLocalMatrix = this.moonOrbit.positionTransform.worldToLocalMatrix;
					if (this.skyProfile.IsFeatureEnabled("MoonRotationFeature", true))
					{
						this.moonOrbit.rotateBody.SpinSpeed = this.skyProfile.GetNumberPropertyValue("MoonRotationSpeedKey", this.timeOfDay);
					}
				}
				if (this.skyProfile.IsFeatureEnabled("MoonSpriteSheetFeature", true))
				{
					this.m_SkyMaterialController.SetMoonSpriteDimensions((int)this.skyProfile.GetNumberPropertyValue("MoonSpriteColumnCountKey", this.timeOfDay), (int)this.skyProfile.GetNumberPropertyValue("MoonSpriteRowCountKey", this.timeOfDay));
					this.m_SkyMaterialController.MoonSpriteItemCount = (int)this.skyProfile.GetNumberPropertyValue("MoonSpriteItemCount", this.timeOfDay);
					this.m_SkyMaterialController.MoonSpriteAnimationSpeed = this.skyProfile.GetNumberPropertyValue("MoonSpriteAnimationSpeed", this.timeOfDay);
				}
				if (this.moonOrbit.BodyLight)
				{
					if (!this.moonOrbit.BodyLight.enabled)
					{
						this.moonOrbit.BodyLight.enabled = true;
					}
					this.moonOrbit.BodyLight.color = this.skyProfile.GetColorPropertyValue("MoonLightColorKey", this.timeOfDay);
					this.moonOrbit.BodyLight.intensity = this.skyProfile.GetNumberPropertyValue("MoonLightIntensityKey", this.timeOfDay);
				}
			}
			else if (this.moonOrbit && this.moonOrbit.BodyLight)
			{
				this.moonOrbit.BodyLight.enabled = false;
			}
			if (this.skyProfile.IsFeatureEnabled("StarBasicFeature", true))
			{
				this.m_SkyMaterialController.StarBasicCubemap = this.skyProfile.GetTexturePropertyValue("StarBasicCubemapKey", this.timeOfDay);
				this.m_SkyMaterialController.StarBasicTwinkleSpeed = this.skyProfile.GetNumberPropertyValue("StarBasicTwinkleSpeedKey", this.timeOfDay);
				this.m_SkyMaterialController.StarBasicTwinkleAmount = this.skyProfile.GetNumberPropertyValue("StarBasicTwinkleAmountKey", this.timeOfDay);
				this.m_SkyMaterialController.StarBasicOpacity = this.skyProfile.GetNumberPropertyValue("StarBasicOpacityKey", this.timeOfDay);
				this.m_SkyMaterialController.StarBasicTintColor = this.skyProfile.GetColorPropertyValue("StarBasicTintColorKey", this.timeOfDay);
				this.m_SkyMaterialController.StarBasicExponent = this.skyProfile.GetNumberPropertyValue("StarBasicExponentKey", this.timeOfDay);
				this.m_SkyMaterialController.StarBasicIntensity = this.skyProfile.GetNumberPropertyValue("StarBasicIntensityKey", this.timeOfDay);
			}
			else
			{
				if (this.skyProfile.IsFeatureEnabled("StarLayer1Feature", true))
				{
					this.m_SkyMaterialController.StarLayer1DataTexture = this.skyProfile.starLayer1DataTexture;
					this.m_SkyMaterialController.StarLayer1Color = this.skyProfile.GetColorPropertyValue("Star1ColorKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer1MaxRadius = this.skyProfile.GetNumberPropertyValue("Star1SizeKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer1Texture = this.skyProfile.GetTexturePropertyValue("Star1TextureKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer1TwinkleAmount = this.skyProfile.GetNumberPropertyValue("Star1TwinkleAmountKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer1TwinkleSpeed = this.skyProfile.GetNumberPropertyValue("Star1TwinkleSpeedKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer1RotationSpeed = this.skyProfile.GetNumberPropertyValue("Star1RotationSpeed", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer1EdgeFeathering = this.skyProfile.GetNumberPropertyValue("Star1EdgeFeathering", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer1BloomFilterBoost = this.skyProfile.GetNumberPropertyValue("Star1ColorIntensityKey", this.timeOfDay);
					if (this.skyProfile.IsFeatureEnabled("StarLayer1SpriteSheetFeature", true))
					{
						this.m_SkyMaterialController.StarLayer1SpriteItemCount = (int)this.skyProfile.GetNumberPropertyValue("Star1SpriteItemCount", this.timeOfDay);
						this.m_SkyMaterialController.StarLayer1SpriteAnimationSpeed = (float)((int)this.skyProfile.GetNumberPropertyValue("Star1SpriteAnimationSpeed", this.timeOfDay));
						this.m_SkyMaterialController.SetStarLayer1SpriteDimensions((int)this.skyProfile.GetNumberPropertyValue("Star1SpriteColumnCountKey", this.timeOfDay), (int)this.skyProfile.GetNumberPropertyValue("Star1SpriteRowCountKey", this.timeOfDay));
					}
				}
				if (this.skyProfile.IsFeatureEnabled("StarLayer2Feature", true))
				{
					this.m_SkyMaterialController.StarLayer2DataTexture = this.skyProfile.starLayer2DataTexture;
					this.m_SkyMaterialController.StarLayer2Color = this.skyProfile.GetColorPropertyValue("Star2ColorKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer2MaxRadius = this.skyProfile.GetNumberPropertyValue("Star2SizeKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer2Texture = this.skyProfile.GetTexturePropertyValue("Star2TextureKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer2TwinkleAmount = this.skyProfile.GetNumberPropertyValue("Star2TwinkleAmountKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer2TwinkleSpeed = this.skyProfile.GetNumberPropertyValue("Star2TwinkleSpeedKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer2RotationSpeed = this.skyProfile.GetNumberPropertyValue("Star2RotationSpeed", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer2EdgeFeathering = this.skyProfile.GetNumberPropertyValue("Star2EdgeFeathering", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer2BloomFilterBoost = this.skyProfile.GetNumberPropertyValue("Star2ColorIntensityKey", this.timeOfDay);
					if (this.skyProfile.IsFeatureEnabled("StarLayer2SpriteSheetFeature", true))
					{
						this.m_SkyMaterialController.StarLayer2SpriteItemCount = (int)this.skyProfile.GetNumberPropertyValue("Star2SpriteItemCount", this.timeOfDay);
						this.m_SkyMaterialController.StarLayer2SpriteAnimationSpeed = (float)((int)this.skyProfile.GetNumberPropertyValue("Star2SpriteAnimationSpeed", this.timeOfDay));
						this.m_SkyMaterialController.SetStarLayer2SpriteDimensions((int)this.skyProfile.GetNumberPropertyValue("Star2SpriteColumnCountKey", this.timeOfDay), (int)this.skyProfile.GetNumberPropertyValue("Star2SpriteRowCountKey", this.timeOfDay));
					}
				}
				if (this.skyProfile.IsFeatureEnabled("StarLayer3Feature", true))
				{
					this.m_SkyMaterialController.StarLayer3DataTexture = this.skyProfile.starLayer3DataTexture;
					this.m_SkyMaterialController.StarLayer3Color = this.skyProfile.GetColorPropertyValue("Star3ColorKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer3MaxRadius = this.skyProfile.GetNumberPropertyValue("Star3SizeKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer3Texture = this.skyProfile.GetTexturePropertyValue("Star3TextureKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer3TwinkleAmount = this.skyProfile.GetNumberPropertyValue("Star3TwinkleAmountKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer3TwinkleSpeed = this.skyProfile.GetNumberPropertyValue("Star3TwinkleSpeedKey", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer3RotationSpeed = this.skyProfile.GetNumberPropertyValue("Star3RotationSpeed", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer3EdgeFeathering = this.skyProfile.GetNumberPropertyValue("Star3EdgeFeathering", this.timeOfDay);
					this.m_SkyMaterialController.StarLayer3BloomFilterBoost = this.skyProfile.GetNumberPropertyValue("Star3ColorIntensityKey", this.timeOfDay);
					if (this.skyProfile.IsFeatureEnabled("StarLayer3SpriteSheetFeature", true))
					{
						this.m_SkyMaterialController.StarLayer3SpriteItemCount = (int)this.skyProfile.GetNumberPropertyValue("Star3SpriteItemCount", this.timeOfDay);
						this.m_SkyMaterialController.StarLayer3SpriteAnimationSpeed = (float)((int)this.skyProfile.GetNumberPropertyValue("Star3SpriteAnimationSpeed", this.timeOfDay));
						this.m_SkyMaterialController.SetStarLayer3SpriteDimensions((int)this.skyProfile.GetNumberPropertyValue("Star3SpriteColumnCountKey", this.timeOfDay), (int)this.skyProfile.GetNumberPropertyValue("Star3SpriteRowCountKey", this.timeOfDay));
					}
				}
			}
			if (this.updateGlobalIllumination)
			{
				this.UpdateGlobalIllumination();
			}
			if (this.timeChangedCallback != null)
			{
				this.timeChangedCallback(this, this.timeOfDay);
			}
		}

		// Token: 0x04000B22 RID: 2850
		[Tooltip("Sky profile defines the skyColors configuration for times of day. This script will animate between those skyColors values based on the time of day.")]
		[SerializeField]
		private SkyProfile m_SkyProfile;

		// Token: 0x04000B23 RID: 2851
		[Tooltip("Time is expressed in a fractional number of days that have completed.")]
		[SerializeField]
		private float m_SkyTime;

		// Token: 0x04000B24 RID: 2852
		[Tooltip("Automatically advance time at fixed speed.")]
		public bool automaticTimeIncrement;

		// Token: 0x04000B25 RID: 2853
		[Tooltip("Create a copy of the sky profile at runtime, so modifications don't affect the original Sky Profile in your project.")]
		public bool copySkyProfile;

		// Token: 0x04000B26 RID: 2854
		private SkyMaterialController m_SkyMaterialController;

		// Token: 0x04000B27 RID: 2855
		[Tooltip("Speed at which to advance time by if in automatic increment is enabled.")]
		[Range(0f, 1f)]
		public float automaticIncrementSpeed = 0.01f;

		// Token: 0x04000B28 RID: 2856
		[Tooltip("Sun orbit.")]
		public OrbitingBody sunOrbit;

		// Token: 0x04000B29 RID: 2857
		[Tooltip("Moon orbit.")]
		public OrbitingBody moonOrbit;

		// Token: 0x04000B2A RID: 2858
		[Tooltip("Controller for managing weather effects")]
		public WeatherController weatherController;

		// Token: 0x04000B2B RID: 2859
		[Tooltip("If true we'll invoke DynamicGI.UpdateEnvironment() when skybox changes. This is an expensive operation.")]
		public bool updateGlobalIllumination;

		// Token: 0x04000B2C RID: 2860
		[Tooltip("Configurable prefab that determines how to animate between 2 sky profiles. You can override individual feature animations, ex: 'skyBlender', to create a custom sky blending effect.")]
		public BlendSkyProfiles skyProfileTransitionPrefab;

		// Token: 0x04000B2E RID: 2862
		private bool m_DidInitialUpdate;

		// Token: 0x020001C9 RID: 457
		// (Invoke) Token: 0x06000A1C RID: 2588
		public delegate void TimeOfDayDidChange(TimeOfDayController tc, float timeOfDay);
	}
}
