using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001DA RID: 474
	[RequireComponent(typeof(AudioSource))]
	public class LightningRenderer : BaseSpriteInstancedRenderer
	{
		// Token: 0x06000A7F RID: 2687 RVA: 0x0002E76D File Offset: 0x0002C96D
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogError("Can't render lightning since GPU instancing is not supported on this platform.");
				base.enabled = false;
				return;
			}
			this.m_AudioSource = base.GetComponent<AudioSource>();
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0002E794 File Offset: 0x0002C994
		protected override Bounds CalculateMeshBounds()
		{
			return new Bounds(Vector3.zero, new Vector3(500f, 500f, 500f));
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0002E7B4 File Offset: 0x0002C9B4
		protected override BaseSpriteItemData CreateSpriteItemData()
		{
			return new BaseSpriteItemData();
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0002E7BB File Offset: 0x0002C9BB
		protected override bool IsRenderingEnabled()
		{
			return !(this.m_SkyProfile == null) && this.m_SkyProfile.IsFeatureEnabled("LightningFeature", true) && LightningRenderer.m_SpawnAreas.Count != 0;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0002E7F0 File Offset: 0x0002C9F0
		protected override void CalculateSpriteTRS(BaseSpriteItemData data, out Vector3 spritePosition, out Quaternion spriteRotation, out Vector3 spriteScale)
		{
			LightningSpawnArea randomLightningSpawnArea = this.GetRandomLightningSpawnArea();
			float num = this.CalculateLightningBoltScaleForArea(randomLightningSpawnArea);
			spriteScale = new Vector3(num, num, num);
			spritePosition = this.GetRandomWorldPositionInsideSpawnArea(randomLightningSpawnArea);
			if (Camera.main == null)
			{
				Debug.LogError("Can't billboard lightning to viewer since there is no main camera tagged.");
				spriteRotation = randomLightningSpawnArea.transform.rotation;
				return;
			}
			spriteRotation = Quaternion.LookRotation(spritePosition - Camera.main.transform.position, Vector3.up);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0002E87B File Offset: 0x0002CA7B
		protected override void ConfigureSpriteItemData(BaseSpriteItemData data)
		{
			if (this.m_SkyProfile.IsFeatureEnabled("ThunderFeature", true))
			{
				base.Invoke("PlayThunderBoltSound", this.m_ThunderSoundDelay);
			}
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000045B1 File Offset: 0x000027B1
		protected override void PrepareDataArraysForRendering(int instanceId, BaseSpriteItemData data)
		{
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0002E8A1 File Offset: 0x0002CAA1
		protected override void PopulatePropertyBlockForRendering(ref MaterialPropertyBlock propertyBlock)
		{
			propertyBlock.SetFloat("_Intensity", this.m_LightningIntensity);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0002E8B8 File Offset: 0x0002CAB8
		protected override int GetNextSpawnCount()
		{
			if (this.m_NextSpawnTime > Time.time)
			{
				return 0;
			}
			this.m_NextSpawnTime = Time.time + 0.5f;
			if (UnityEngine.Random.value < this.m_LightningProbability)
			{
				this.m_NextSpawnTime += this.m_SpawnCoolDown;
				return 1;
			}
			return 0;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0002E908 File Offset: 0x0002CB08
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay, LightningArtItem artItem)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
			this.m_Style = artItem;
			if (this.m_SkyProfile == null)
			{
				Debug.LogError("Assigned null sky profile!");
				return;
			}
			if (this.m_Style == null)
			{
				Debug.LogError("Can't render lightning without an art item");
				return;
			}
			this.SyncDataFromSkyProfile();
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0002E964 File Offset: 0x0002CB64
		private void SyncDataFromSkyProfile()
		{
			this.m_LightningProbability = this.m_SkyProfile.GetNumberPropertyValue("LightningProbabilityKey", this.m_TimeOfDay);
			this.m_LightningIntensity = this.m_SkyProfile.GetNumberPropertyValue("LightningIntensityKey", this.m_TimeOfDay);
			this.m_SpawnCoolDown = this.m_SkyProfile.GetNumberPropertyValue("LightningStrikeCoolDown", this.m_TimeOfDay);
			this.m_ThunderSoundDelay = this.m_SkyProfile.GetNumberPropertyValue("ThunderSoundDelayKey", this.m_TimeOfDay);
			this.m_LightningProbability *= this.m_Style.strikeProbability;
			this.m_LightningIntensity *= this.m_Style.intensity;
			this.m_SpriteSheetLayout.columns = this.m_Style.columns;
			this.m_SpriteSheetLayout.rows = this.m_Style.rows;
			this.m_SpriteSheetLayout.frameCount = this.m_Style.totalFrames;
			this.m_SpriteSheetLayout.frameRate = this.m_Style.animateSpeed;
			this.m_TintColor = this.m_Style.tintColor * this.m_SkyProfile.GetColorPropertyValue("LightningTintColorKey", this.m_TimeOfDay);
			this.renderMaterial = this.m_Style.material;
			this.modelMesh = this.m_Style.mesh;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0002EAB8 File Offset: 0x0002CCB8
		private LightningSpawnArea GetRandomLightningSpawnArea()
		{
			if (LightningRenderer.m_SpawnAreas.Count == 0)
			{
				return null;
			}
			int index = Mathf.RoundToInt((float)UnityEngine.Random.Range(0, LightningRenderer.m_SpawnAreas.Count)) % LightningRenderer.m_SpawnAreas.Count;
			return LightningRenderer.m_SpawnAreas[index];
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0002EB00 File Offset: 0x0002CD00
		private void PlayThunderBoltSound()
		{
			if (this.m_Style.thunderSound != null)
			{
				this.m_AudioSource.volume = this.m_SkyProfile.GetNumberPropertyValue("ThunderSoundVolumeKey", this.m_TimeOfDay);
				this.m_AudioSource.PlayOneShot(this.m_Style.thunderSound);
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0002EB57 File Offset: 0x0002CD57
		public static void AddSpawnArea(LightningSpawnArea area)
		{
			if (!LightningRenderer.m_SpawnAreas.Contains(area))
			{
				LightningRenderer.m_SpawnAreas.Add(area);
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0002EB71 File Offset: 0x0002CD71
		public static void RemoveSpawnArea(LightningSpawnArea area)
		{
			if (LightningRenderer.m_SpawnAreas.Contains(area))
			{
				LightningRenderer.m_SpawnAreas.Remove(area);
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0002EB8C File Offset: 0x0002CD8C
		private Vector3 GetRandomWorldPositionInsideSpawnArea(LightningSpawnArea area)
		{
			float x = UnityEngine.Random.Range(-area.lightningArea.x, area.lightningArea.x) / 2f;
			float z = UnityEngine.Random.Range(-area.lightningArea.z, area.lightningArea.z) / 2f;
			float y = 0f;
			if (this.m_Style.alignment == LightningArtItem.Alignment.TopAlign)
			{
				y = area.lightningArea.y / 2f - this.m_Style.size / 2f;
			}
			return area.transform.TransformPoint(new Vector3(x, y, z));
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0002EC2A File Offset: 0x0002CE2A
		private float CalculateLightningBoltScaleForArea(LightningSpawnArea area)
		{
			if (this.m_Style.alignment == LightningArtItem.Alignment.ScaleToFit)
			{
				return area.lightningArea.y / 2f;
			}
			return this.m_Style.size;
		}

		// Token: 0x04000B6D RID: 2925
		private static List<LightningSpawnArea> m_SpawnAreas = new List<LightningSpawnArea>();

		// Token: 0x04000B6E RID: 2926
		private float m_LightningProbability;

		// Token: 0x04000B6F RID: 2927
		private float m_NextSpawnTime;

		// Token: 0x04000B70 RID: 2928
		private SkyProfile m_SkyProfile;

		// Token: 0x04000B71 RID: 2929
		private LightningArtItem m_Style;

		// Token: 0x04000B72 RID: 2930
		private float m_TimeOfDay;

		// Token: 0x04000B73 RID: 2931
		private AudioSource m_AudioSource;

		// Token: 0x04000B74 RID: 2932
		private float m_LightningIntensity;

		// Token: 0x04000B75 RID: 2933
		private float m_ThunderSoundDelay;

		// Token: 0x04000B76 RID: 2934
		private float m_SpawnCoolDown;

		// Token: 0x04000B77 RID: 2935
		private const float k_ProbabiltyCheckInterval = 0.5f;
	}
}
