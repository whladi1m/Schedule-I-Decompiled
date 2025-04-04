using System;
using AmplifyColor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

// Token: 0x02000004 RID: 4
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(AmplifyColorEffect))]
[AddComponentMenu("Image Effects/Amplify Color Render Mask")]
public class AmplifyColorRenderMask : MonoBehaviour
{
	// Token: 0x0600002D RID: 45 RVA: 0x00003948 File Offset: 0x00001B48
	private void OnEnable()
	{
		if (this.maskCamera == null)
		{
			GameObject gameObject = new GameObject("Mask Camera", new Type[]
			{
				typeof(Camera)
			})
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			gameObject.transform.parent = base.gameObject.transform;
			this.maskCamera = gameObject.GetComponent<Camera>();
		}
		this.referenceCamera = base.GetComponent<Camera>();
		this.colorEffect = base.GetComponent<AmplifyColorEffect>();
		this.colorMaskShader = Shader.Find("Hidden/RenderMask");
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000039D3 File Offset: 0x00001BD3
	private void OnDisable()
	{
		this.DestroyCamera();
		this.DestroyRenderTextures();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000039E1 File Offset: 0x00001BE1
	private void DestroyCamera()
	{
		if (this.maskCamera != null)
		{
			UnityEngine.Object.DestroyImmediate(this.maskCamera.gameObject);
			this.maskCamera = null;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003A08 File Offset: 0x00001C08
	private void DestroyRenderTextures()
	{
		if (this.maskTexture != null)
		{
			RenderTexture.active = null;
			UnityEngine.Object.DestroyImmediate(this.maskTexture);
			this.maskTexture = null;
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003A30 File Offset: 0x00001C30
	private void UpdateRenderTextures(bool singlePassStereo)
	{
		int num = this.referenceCamera.pixelWidth;
		int num2 = this.referenceCamera.pixelHeight;
		if (this.maskTexture == null || this.width != num || this.height != num2 || !this.maskTexture.IsCreated() || this.singlePassStereo != singlePassStereo)
		{
			this.width = num;
			this.height = num2;
			this.DestroyRenderTextures();
			if (XRSettings.enabled)
			{
				num = XRSettings.eyeTextureWidth * (singlePassStereo ? 2 : 1);
				num2 = XRSettings.eyeTextureHeight;
			}
			if (this.maskTexture == null)
			{
				this.maskTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB)
				{
					hideFlags = HideFlags.HideAndDontSave,
					name = "MaskTexture"
				};
				this.maskTexture.name = "AmplifyColorMaskTexture";
				bool allowMSAA = this.maskCamera.allowMSAA;
				this.maskTexture.antiAliasing = ((allowMSAA && QualitySettings.antiAliasing > 0) ? QualitySettings.antiAliasing : 1);
			}
			this.maskTexture.Create();
			this.singlePassStereo = singlePassStereo;
		}
		if (this.colorEffect != null)
		{
			this.colorEffect.MaskTexture = this.maskTexture;
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003B5C File Offset: 0x00001D5C
	private void UpdateCameraProperties()
	{
		this.maskCamera.CopyFrom(this.referenceCamera);
		this.maskCamera.targetTexture = this.maskTexture;
		this.maskCamera.clearFlags = CameraClearFlags.Nothing;
		this.maskCamera.renderingPath = RenderingPath.VertexLit;
		this.maskCamera.pixelRect = new Rect(0f, 0f, (float)this.width, (float)this.height);
		this.maskCamera.depthTextureMode = DepthTextureMode.None;
		this.maskCamera.allowHDR = false;
		this.maskCamera.enabled = false;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003BF0 File Offset: 0x00001DF0
	private void OnPreRender()
	{
		if (this.maskCamera != null)
		{
			RenderBuffer activeColorBuffer = Graphics.activeColorBuffer;
			RenderBuffer activeDepthBuffer = Graphics.activeDepthBuffer;
			bool flag = false;
			if (this.referenceCamera.stereoEnabled)
			{
				flag = (XRSettings.eyeTextureDesc.vrUsage == VRTextureUsage.TwoEyes);
				this.maskCamera.SetStereoViewMatrix(Camera.StereoscopicEye.Left, this.referenceCamera.GetStereoViewMatrix(Camera.StereoscopicEye.Left));
				this.maskCamera.SetStereoViewMatrix(Camera.StereoscopicEye.Right, this.referenceCamera.GetStereoViewMatrix(Camera.StereoscopicEye.Right));
				this.maskCamera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, this.referenceCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left));
				this.maskCamera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, this.referenceCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right));
			}
			this.UpdateRenderTextures(flag);
			this.UpdateCameraProperties();
			Graphics.SetRenderTarget(this.maskTexture);
			GL.Clear(true, true, this.ClearColor);
			if (flag)
			{
				this.maskCamera.worldToCameraMatrix = this.referenceCamera.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
				this.maskCamera.projectionMatrix = this.referenceCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
				this.maskCamera.rect = new Rect(0f, 0f, 0.5f, 1f);
			}
			foreach (RenderLayer renderLayer in this.RenderLayers)
			{
				Shader.SetGlobalColor("_COLORMASK_Color", renderLayer.color);
				this.maskCamera.cullingMask = renderLayer.mask;
				this.maskCamera.RenderWithShader(this.colorMaskShader, "RenderType");
			}
			if (flag)
			{
				this.maskCamera.worldToCameraMatrix = this.referenceCamera.GetStereoViewMatrix(Camera.StereoscopicEye.Right);
				this.maskCamera.projectionMatrix = this.referenceCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
				this.maskCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
				foreach (RenderLayer renderLayer2 in this.RenderLayers)
				{
					Shader.SetGlobalColor("_COLORMASK_Color", renderLayer2.color);
					this.maskCamera.cullingMask = renderLayer2.mask;
					this.maskCamera.RenderWithShader(this.colorMaskShader, "RenderType");
				}
			}
			Graphics.SetRenderTarget(activeColorBuffer, activeDepthBuffer);
		}
	}

	// Token: 0x04000047 RID: 71
	[FormerlySerializedAs("clearColor")]
	public Color ClearColor = Color.black;

	// Token: 0x04000048 RID: 72
	[FormerlySerializedAs("renderLayers")]
	public RenderLayer[] RenderLayers = new RenderLayer[0];

	// Token: 0x04000049 RID: 73
	[FormerlySerializedAs("debug")]
	public bool DebugMask;

	// Token: 0x0400004A RID: 74
	private Camera referenceCamera;

	// Token: 0x0400004B RID: 75
	private Camera maskCamera;

	// Token: 0x0400004C RID: 76
	private AmplifyColorEffect colorEffect;

	// Token: 0x0400004D RID: 77
	private int width;

	// Token: 0x0400004E RID: 78
	private int height;

	// Token: 0x0400004F RID: 79
	private RenderTexture maskTexture;

	// Token: 0x04000050 RID: 80
	private Shader colorMaskShader;

	// Token: 0x04000051 RID: 81
	private bool singlePassStereo;
}
