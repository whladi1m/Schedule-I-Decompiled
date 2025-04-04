using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001D9 RID: 473
	public class LightningController : MonoBehaviour, ISkyModule
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x0002E592 File Offset: 0x0002C792
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogWarning("Can't render lightning since GPU instancing is not supported on this platform.");
				base.enabled = false;
				return;
			}
			this.ClearLightningRenderers();
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0002E5B3 File Offset: 0x0002C7B3
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0002E5C4 File Offset: 0x0002C7C4
		public void Update()
		{
			if (this.m_SkyProfile == null || !this.m_SkyProfile.IsFeatureEnabled("LightningFeature", true))
			{
				this.ClearLightningRenderers();
				return;
			}
			if (this.m_SkyProfile.lightningArtSet == null || this.m_SkyProfile.lightningArtSet.lightingStyleItems == null || this.m_SkyProfile.lightningArtSet.lightingStyleItems.Count == 0)
			{
				return;
			}
			if (this.m_SkyProfile.lightningArtSet.lightingStyleItems.Count != this.m_LightningRenderers.Count)
			{
				this.ClearLightningRenderers();
				this.CreateLightningRenderers();
			}
			for (int i = 0; i < this.m_SkyProfile.lightningArtSet.lightingStyleItems.Count; i++)
			{
				LightningArtItem artItem = this.m_SkyProfile.lightningArtSet.lightingStyleItems[i];
				this.m_LightningRenderers[i].UpdateForTimeOfDay(this.m_SkyProfile, this.m_TimeOfDay, artItem);
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0002E6BC File Offset: 0x0002C8BC
		public void ClearLightningRenderers()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
			}
			this.m_LightningRenderers.Clear();
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0002E700 File Offset: 0x0002C900
		public void CreateLightningRenderers()
		{
			for (int i = 0; i < this.m_SkyProfile.lightningArtSet.lightingStyleItems.Count; i++)
			{
				LightningRenderer lightningRenderer = new GameObject("Lightning Renderer").AddComponent<LightningRenderer>();
				lightningRenderer.transform.parent = base.transform;
				this.m_LightningRenderers.Add(lightningRenderer);
			}
		}

		// Token: 0x04000B6A RID: 2922
		private SkyProfile m_SkyProfile;

		// Token: 0x04000B6B RID: 2923
		private float m_TimeOfDay;

		// Token: 0x04000B6C RID: 2924
		private List<LightningRenderer> m_LightningRenderers = new List<LightningRenderer>();
	}
}
