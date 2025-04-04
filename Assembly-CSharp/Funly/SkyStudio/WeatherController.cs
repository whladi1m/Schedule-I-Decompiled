using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E1 RID: 481
	public class WeatherController : MonoBehaviour
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0002F974 File Offset: 0x0002DB74
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x0002F97C File Offset: 0x0002DB7C
		public RainDownfallController rainDownfallController { get; protected set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0002F985 File Offset: 0x0002DB85
		// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x0002F98D File Offset: 0x0002DB8D
		public RainSplashController rainSplashController { get; protected set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0002F996 File Offset: 0x0002DB96
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x0002F99E File Offset: 0x0002DB9E
		public LightningController lightningController { get; protected set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0002F9A7 File Offset: 0x0002DBA7
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x0002F9AF File Offset: 0x0002DBAF
		public WeatherDepthCamera weatherDepthCamera { get; protected set; }

		// Token: 0x06000ABB RID: 2747 RVA: 0x0002F9B8 File Offset: 0x0002DBB8
		private void Awake()
		{
			this.DiscoverWeatherControllers();
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0002F9B8 File Offset: 0x0002DBB8
		private void Start()
		{
			this.DiscoverWeatherControllers();
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0002F9C0 File Offset: 0x0002DBC0
		private void OnEnable()
		{
			this.DiscoverWeatherControllers();
			if (this.detector == null)
			{
				Debug.LogError("Can't register for enclosure callbacks since there's no WeatherEnclosureDetector on any children");
				return;
			}
			WeatherEnclosureDetector weatherEnclosureDetector = this.detector;
			weatherEnclosureDetector.enclosureChangedCallback = (Action<WeatherEnclosure>)Delegate.Combine(weatherEnclosureDetector.enclosureChangedCallback, new Action<WeatherEnclosure>(this.OnEnclosureDidChange));
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002FA13 File Offset: 0x0002DC13
		private void DiscoverWeatherControllers()
		{
			this.rainDownfallController = base.GetComponentInChildren<RainDownfallController>();
			this.rainSplashController = base.GetComponentInChildren<RainSplashController>();
			this.lightningController = base.GetComponentInChildren<LightningController>();
			this.weatherDepthCamera = base.GetComponentInChildren<WeatherDepthCamera>();
			this.detector = base.GetComponentInChildren<WeatherEnclosureDetector>();
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0002FA51 File Offset: 0x0002DC51
		private void OnDisable()
		{
			if (this.detector == null)
			{
				return;
			}
			WeatherEnclosureDetector weatherEnclosureDetector = this.detector;
			weatherEnclosureDetector.enclosureChangedCallback = (Action<WeatherEnclosure>)Delegate.Remove(weatherEnclosureDetector.enclosureChangedCallback, new Action<WeatherEnclosure>(this.OnEnclosureDidChange));
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0002FA8C File Offset: 0x0002DC8C
		public void UpdateForTimeOfDay(SkyProfile skyProfile, float timeOfDay)
		{
			if (!skyProfile)
			{
				return;
			}
			this.m_Profile = skyProfile;
			this.m_TimeOfDay = timeOfDay;
			if (this.weatherDepthCamera != null)
			{
				this.weatherDepthCamera.enabled = skyProfile.IsFeatureEnabled("RainSplashFeature", true);
			}
			if (this.rainDownfallController != null)
			{
				this.rainDownfallController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
			if (this.rainSplashController != null)
			{
				this.rainSplashController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
			if (this.lightningController != null)
			{
				this.lightningController.UpdateForTimeOfDay(skyProfile, timeOfDay);
			}
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0002FB28 File Offset: 0x0002DD28
		private void LateUpdate()
		{
			if (this.m_Profile == null)
			{
				return;
			}
			if (this.m_EnclosureMeshRenderer && this.rainDownfallController && this.m_Profile.IsFeatureEnabled("RainFeature", true))
			{
				this.m_EnclosureMeshRenderer.enabled = true;
				return;
			}
			this.m_EnclosureMeshRenderer.enabled = false;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0002FB8C File Offset: 0x0002DD8C
		private void OnEnclosureDidChange(WeatherEnclosure enclosure)
		{
			this.m_Enclosure = enclosure;
			if (this.m_Enclosure != null)
			{
				this.m_EnclosureMeshRenderer = this.m_Enclosure.GetComponentInChildren<MeshRenderer>();
			}
			this.rainDownfallController.SetWeatherEnclosure(this.m_Enclosure);
			this.UpdateForTimeOfDay(this.m_Profile, this.m_TimeOfDay);
		}

		// Token: 0x04000B9A RID: 2970
		private WeatherEnclosure m_Enclosure;

		// Token: 0x04000B9B RID: 2971
		private MeshRenderer m_EnclosureMeshRenderer;

		// Token: 0x04000B9C RID: 2972
		private WeatherEnclosureDetector detector;

		// Token: 0x04000B9D RID: 2973
		private SkyProfile m_Profile;

		// Token: 0x04000B9E RID: 2974
		private float m_TimeOfDay;
	}
}
