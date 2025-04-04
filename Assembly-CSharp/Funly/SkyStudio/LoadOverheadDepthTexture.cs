using System;
using UnityEngine;
using UnityEngine.UI;

namespace Funly.SkyStudio
{
	// Token: 0x020001CE RID: 462
	[RequireComponent(typeof(RawImage))]
	public class LoadOverheadDepthTexture : MonoBehaviour
	{
		// Token: 0x06000A26 RID: 2598 RVA: 0x0002D4A7 File Offset: 0x0002B6A7
		private void Start()
		{
			this.m_RainCamera = UnityEngine.Object.FindObjectOfType<WeatherDepthCamera>();
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Update()
		{
		}

		// Token: 0x04000B31 RID: 2865
		private WeatherDepthCamera m_RainCamera;
	}
}
