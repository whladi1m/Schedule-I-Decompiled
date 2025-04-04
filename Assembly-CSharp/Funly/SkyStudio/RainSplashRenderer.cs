using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001DF RID: 479
	public class RainSplashRenderer : BaseSpriteInstancedRenderer
	{
		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002F29C File Offset: 0x0002D49C
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogError("Can't render rain splashes since GPU instancing is not supported on this platform.");
				base.enabled = false;
				return;
			}
			WeatherDepthCamera weatherDepthCamera = UnityEngine.Object.FindObjectOfType<WeatherDepthCamera>();
			if (weatherDepthCamera == null)
			{
				Debug.LogError("Can't generate splashes without a RainDepthCamera in the scene");
				base.enabled = false;
				return;
			}
			this.m_DepthCamera = weatherDepthCamera.GetComponent<Camera>();
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0002F2EF File Offset: 0x0002D4EF
		protected override Bounds CalculateMeshBounds()
		{
			return this.m_Bounds;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0002F2F7 File Offset: 0x0002D4F7
		protected override BaseSpriteItemData CreateSpriteItemData()
		{
			return new RainSplashData();
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002F300 File Offset: 0x0002D500
		protected override bool IsRenderingEnabled()
		{
			if (this.m_SkyProfile == null)
			{
				return false;
			}
			if (!this.m_SkyProfile.IsFeatureEnabled("RainSplashFeature", true))
			{
				return false;
			}
			if (base.m_ViewerCamera == null)
			{
				Debug.LogError("Can't render ground raindrops since no active camera has the MainCamera tag applied.");
				return false;
			}
			return true;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002F350 File Offset: 0x0002D550
		protected override int GetNextSpawnCount()
		{
			int num = base.maxSprites - this.m_Active.Count;
			if (num <= 0)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0002F378 File Offset: 0x0002D578
		protected override void CalculateSpriteTRS(BaseSpriteItemData data, out Vector3 spritePosition, out Quaternion spriteRotation, out Vector3 spriteScale)
		{
			float num = UnityEngine.Random.Range(this.m_SplashScale * (1f - this.m_SplashScaleVarience), this.m_SplashScale);
			spritePosition = data.spritePosition;
			spriteRotation = Quaternion.identity;
			spriteScale = new Vector3(num, num, num);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x0002F3CA File Offset: 0x0002D5CA
		protected override void ConfigureSpriteItemData(BaseSpriteItemData data)
		{
			data.spritePosition = this.CreateWorldSplashPoint();
			data.delay = UnityEngine.Random.Range(0f, 0.5f);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0002F3F0 File Offset: 0x0002D5F0
		protected override void PrepareDataArraysForRendering(int instanceId, BaseSpriteItemData data)
		{
			RainSplashData rainSplashData = data as RainSplashData;
			Vector3 vector = this.m_DepthCamera.WorldToScreenPoint(rainSplashData.spritePosition);
			Vector2 depthTextureUV = new Vector2(vector.x / (float)this.m_DepthCamera.pixelWidth, vector.y / (float)this.m_DepthCamera.pixelHeight);
			rainSplashData.depthTextureUV = depthTextureUV;
			this.m_StartSplashYPositions[instanceId] = rainSplashData.spritePosition.y;
			this.m_DepthUs[instanceId] = rainSplashData.depthTextureUV.x;
			this.m_DepthVs[instanceId] = rainSplashData.depthTextureUV.y;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0002F484 File Offset: 0x0002D684
		protected override void PopulatePropertyBlockForRendering(ref MaterialPropertyBlock propertyBlock)
		{
			propertyBlock.SetFloat("_Intensity", this.m_SplashItensity);
			propertyBlock.SetFloatArray("_OverheadDepthU", this.m_DepthUs);
			propertyBlock.SetFloatArray("_OverheadDepthV", this.m_DepthVs);
			propertyBlock.SetFloatArray("_SplashStartYPosition", this.m_StartSplashYPositions);
			propertyBlock.SetFloat("_SplashGroundOffset", this.m_SplashSurfaceOffset);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0002F4EB File Offset: 0x0002D6EB
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay, RainSplashArtItem style)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
			this.m_Style = style;
			if (this.m_SkyProfile == null)
			{
				return;
			}
			this.SyncDataFromSkyProfile();
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002F518 File Offset: 0x0002D718
		private void SyncDataFromSkyProfile()
		{
			base.maxSprites = (int)this.m_SkyProfile.GetNumberPropertyValue("RainSplashMaxConcurrentKey", this.m_TimeOfDay);
			this.m_SplashAreaStart = this.m_SkyProfile.GetNumberPropertyValue("RainSplashAreaStartKey", this.m_TimeOfDay);
			this.m_SplashAreaLength = this.m_SkyProfile.GetNumberPropertyValue("RainSplashAreaLengthKey", this.m_TimeOfDay);
			this.m_SplashScale = this.m_SkyProfile.GetNumberPropertyValue("RainSplashScaleKey", this.m_TimeOfDay);
			this.m_SplashScaleVarience = this.m_SkyProfile.GetNumberPropertyValue("RainSplashScaleVarienceKey", this.m_TimeOfDay);
			this.m_SplashItensity = this.m_SkyProfile.GetNumberPropertyValue("RainSplashIntensityKey", this.m_TimeOfDay);
			this.m_SplashSurfaceOffset = this.m_SkyProfile.GetNumberPropertyValue("RainSplashSurfaceOffsetKey", this.m_TimeOfDay);
			this.m_SplashScale *= this.m_Style.scaleMultiplier;
			this.m_SplashItensity *= this.m_Style.intensityMultiplier;
			this.m_SpriteSheetLayout.columns = this.m_Style.columns;
			this.m_SpriteSheetLayout.rows = this.m_Style.rows;
			this.m_SpriteSheetLayout.frameCount = this.m_Style.totalFrames;
			this.m_SpriteSheetLayout.frameRate = this.m_Style.animateSpeed;
			this.m_TintColor = this.m_Style.tintColor * this.m_SkyProfile.GetColorPropertyValue("RainSplashTintColorKey", this.m_TimeOfDay);
			this.modelMesh = this.m_Style.mesh;
			this.renderMaterial = this.m_Style.material;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002F6C0 File Offset: 0x0002D8C0
		private Vector3 CreateWorldSplashPoint()
		{
			float y = UnityEngine.Random.Range(0f, -170f);
			Vector3 vector = Quaternion.Euler(new Vector3(0f, y, 0f)) * Vector3.right;
			float d = UnityEngine.Random.Range(this.m_SplashAreaStart, this.m_SplashAreaStart + this.m_SplashAreaLength);
			Vector3 position = vector.normalized * d;
			return base.m_ViewerCamera.transform.TransformPoint(position);
		}

		// Token: 0x04000B83 RID: 2947
		private Camera m_DepthCamera;

		// Token: 0x04000B84 RID: 2948
		private float[] m_StartSplashYPositions = new float[1000];

		// Token: 0x04000B85 RID: 2949
		private float[] m_DepthUs = new float[1000];

		// Token: 0x04000B86 RID: 2950
		private float[] m_DepthVs = new float[1000];

		// Token: 0x04000B87 RID: 2951
		private float m_SplashAreaStart;

		// Token: 0x04000B88 RID: 2952
		private float m_SplashAreaLength;

		// Token: 0x04000B89 RID: 2953
		private float m_SplashScale;

		// Token: 0x04000B8A RID: 2954
		private float m_SplashScaleVarience;

		// Token: 0x04000B8B RID: 2955
		private float m_SplashItensity;

		// Token: 0x04000B8C RID: 2956
		private float m_SplashSurfaceOffset;

		// Token: 0x04000B8D RID: 2957
		private SkyProfile m_SkyProfile;

		// Token: 0x04000B8E RID: 2958
		private float m_TimeOfDay;

		// Token: 0x04000B8F RID: 2959
		private RainSplashArtItem m_Style;

		// Token: 0x04000B90 RID: 2960
		private Bounds m_Bounds = new Bounds(Vector3.zero, new Vector3(100f, 100f, 100f));
	}
}
