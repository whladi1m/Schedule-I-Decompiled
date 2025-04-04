using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Funly.SkyStudio
{
	// Token: 0x020001E8 RID: 488
	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(UniversalAdditionalCameraData))]
	public class URPWeatherDepth : MonoBehaviour
	{
		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002FE0F File Offset: 0x0002E00F
		private void Start()
		{
			this.m_Camera = base.GetComponent<Camera>();
			this.m_CameraData = base.GetComponent<UniversalAdditionalCameraData>();
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0002FE2C File Offset: 0x0002E02C
		private void Update()
		{
			this.m_CameraData.SetRenderer(1);
			Shader.SetGlobalTexture("_OverheadDepthTex", this.renderTexture);
			Shader.SetGlobalVector("_OverheadDepthPosition", this.m_Camera.transform.position);
			Shader.SetGlobalFloat("_OverheadDepthNearClip", this.m_Camera.nearClipPlane);
			Shader.SetGlobalFloat("_OverheadDepthFarClip", this.m_Camera.farClipPlane);
		}

		// Token: 0x04000BB2 RID: 2994
		public RenderTexture renderTexture;

		// Token: 0x04000BB3 RID: 2995
		private Camera m_Camera;

		// Token: 0x04000BB4 RID: 2996
		private UniversalAdditionalCameraData m_CameraData;
	}
}
