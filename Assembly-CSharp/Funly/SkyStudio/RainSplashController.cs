using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001DD RID: 477
	public class RainSplashController : MonoBehaviour, ISkyModule
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x0002F0B1 File Offset: 0x0002D2B1
		private void Start()
		{
			if (!SystemInfo.supportsInstancing)
			{
				Debug.LogWarning("Can't render rain splashes since GPU instancing is not supported on this platform.");
				base.enabled = false;
				return;
			}
			this.ClearSplashRenderers();
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002F0D2 File Offset: 0x0002D2D2
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
		{
			this.m_SkyProfile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002F0E4 File Offset: 0x0002D2E4
		private void Update()
		{
			if (this.m_SkyProfile == null || !this.m_SkyProfile.IsFeatureEnabled("RainSplashFeature", true))
			{
				this.ClearSplashRenderers();
				return;
			}
			if (this.m_SkyProfile.rainSplashArtSet == null || this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems == null || this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count == 0)
			{
				this.ClearSplashRenderers();
				return;
			}
			if (this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count != this.m_SplashRenderers.Count)
			{
				this.ClearSplashRenderers();
				this.CreateSplashRenderers();
			}
			for (int i = 0; i < this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count; i++)
			{
				RainSplashArtItem style = this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems[i];
				this.m_SplashRenderers[i].UpdateForTimeOfDay(this.m_SkyProfile, this.m_TimeOfDay, style);
			}
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0002F1E0 File Offset: 0x0002D3E0
		public void ClearSplashRenderers()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
			}
			this.m_SplashRenderers.Clear();
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002F224 File Offset: 0x0002D424
		public void CreateSplashRenderers()
		{
			for (int i = 0; i < this.m_SkyProfile.rainSplashArtSet.rainSplashArtItems.Count; i++)
			{
				RainSplashRenderer rainSplashRenderer = new GameObject("Rain Splash Renderer").AddComponent<RainSplashRenderer>();
				rainSplashRenderer.transform.parent = base.transform;
				this.m_SplashRenderers.Add(rainSplashRenderer);
			}
		}

		// Token: 0x04000B7F RID: 2943
		private SkyProfile m_SkyProfile;

		// Token: 0x04000B80 RID: 2944
		private float m_TimeOfDay;

		// Token: 0x04000B81 RID: 2945
		private List<RainSplashRenderer> m_SplashRenderers = new List<RainSplashRenderer>();
	}
}
