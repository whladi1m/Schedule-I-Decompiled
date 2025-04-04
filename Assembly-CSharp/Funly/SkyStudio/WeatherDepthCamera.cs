using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001E0 RID: 480
	[RequireComponent(typeof(Camera))]
	public class WeatherDepthCamera : MonoBehaviour
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x0002F79F File Offset: 0x0002D99F
		private void Start()
		{
			this.m_DepthCamera = base.GetComponent<Camera>();
			this.m_DepthCamera.enabled = false;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0002F7B9 File Offset: 0x0002D9B9
		private void Update()
		{
			if (this.m_DepthCamera.enabled)
			{
				this.m_DepthCamera.enabled = false;
			}
			if (Time.frameCount % this.renderFrameInterval != 0)
			{
				return;
			}
			this.RenderOverheadCamera();
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002F7EC File Offset: 0x0002D9EC
		private void RenderOverheadCamera()
		{
			this.PrepareRenderTexture();
			if (this.depthShader == null)
			{
				Debug.LogError("Can't render depth since depth shader is missing.");
				return;
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = this.overheadDepthTexture;
			GL.Clear(true, true, Color.black);
			this.m_DepthCamera.RenderWithShader(this.depthShader, "RenderType");
			RenderTexture.active = active;
			Shader.SetGlobalTexture("_OverheadDepthTex", this.overheadDepthTexture);
			Shader.SetGlobalVector("_OverheadDepthPosition", this.m_DepthCamera.transform.position);
			Shader.SetGlobalFloat("_OverheadDepthNearClip", this.m_DepthCamera.nearClipPlane);
			Shader.SetGlobalFloat("_OverheadDepthFarClip", this.m_DepthCamera.farClipPlane);
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002F8A8 File Offset: 0x0002DAA8
		private void PrepareRenderTexture()
		{
			if (this.overheadDepthTexture == null)
			{
				int num = Mathf.ClosestPowerOfTwo(Mathf.FloorToInt((float)this.textureResolution));
				RenderTextureFormat format = RenderTextureFormat.ARGB32;
				this.overheadDepthTexture = new RenderTexture(num, num, 24, format, RenderTextureReadWrite.Linear);
				this.overheadDepthTexture.useMipMap = false;
				this.overheadDepthTexture.autoGenerateMips = false;
				this.overheadDepthTexture.filterMode = FilterMode.Point;
				this.overheadDepthTexture.antiAliasing = 2;
			}
			if (!this.overheadDepthTexture.IsCreated())
			{
				this.overheadDepthTexture.Create();
			}
			if (this.m_DepthCamera.targetTexture != this.overheadDepthTexture)
			{
				this.m_DepthCamera.targetTexture = this.overheadDepthTexture;
			}
		}

		// Token: 0x04000B91 RID: 2961
		private Camera m_DepthCamera;

		// Token: 0x04000B92 RID: 2962
		[Tooltip("Shader used to render out depth + normal texture. This should be the sky studio depth shader.")]
		public Shader depthShader;

		// Token: 0x04000B93 RID: 2963
		[HideInInspector]
		public RenderTexture overheadDepthTexture;

		// Token: 0x04000B94 RID: 2964
		[Tooltip("You can help increase performance by only rendering periodically some number of frames.")]
		[Range(1f, 60f)]
		public int renderFrameInterval = 5;

		// Token: 0x04000B95 RID: 2965
		[Tooltip("The resolution of the texture. Higher resolution uses more rendering time but makes more precise weather along edges.")]
		[Range(128f, 8192f)]
		public int textureResolution = 1024;
	}
}
