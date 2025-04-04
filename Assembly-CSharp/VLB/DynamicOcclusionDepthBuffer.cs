using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200013B RID: 315
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dynocclusion-sd-depthbuffer/")]
	public class DynamicOcclusionDepthBuffer : DynamicOcclusionAbstractBase
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x0001A875 File Offset: 0x00018A75
		protected override string GetShaderKeyword()
		{
			return "VLB_OCCLUSION_DEPTH_TEXTURE";
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001A87C File Offset: 0x00018A7C
		protected override MaterialManager.SD.DynamicOcclusion GetDynamicOcclusionMode()
		{
			return MaterialManager.SD.DynamicOcclusion.DepthTexture;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001A87F File Offset: 0x00018A7F
		private void ProcessOcclusionInternal()
		{
			this.UpdateDepthCameraPropertiesAccordingToBeam();
			this.m_DepthCamera.Render();
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001A892 File Offset: 0x00018A92
		protected override bool OnProcessOcclusion(DynamicOcclusionAbstractBase.ProcessOcclusionSource source)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				this.m_NeedToUpdateOcclusionNextFrame = true;
			}
			else
			{
				this.ProcessOcclusionInternal();
			}
			return true;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001A8AB File Offset: 0x00018AAB
		private void Update()
		{
			if (this.m_NeedToUpdateOcclusionNextFrame && this.m_Master && this.m_DepthCamera && Time.frameCount > 1)
			{
				this.ProcessOcclusionInternal();
				this.m_NeedToUpdateOcclusionNextFrame = false;
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001A8E4 File Offset: 0x00018AE4
		private void UpdateDepthCameraPropertiesAccordingToBeam()
		{
			Utils.SetupDepthCamera(this.m_DepthCamera, this.m_Master.coneApexOffsetZ, this.m_Master.maxGeometryDistance, this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd, this.m_Master.beamLocalForward, this.m_Master.GetLossyScale(), this.m_Master.IsScalable(), this.m_Master.beamInternalLocalRotation, true);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001A958 File Offset: 0x00018B58
		public bool HasLayerMaskIssues()
		{
			if (Config.Instance.geometryOverrideLayer)
			{
				int num = 1 << Config.Instance.geometryLayerID;
				return (this.layerMask.value & num) == num;
			}
			return false;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001A993 File Offset: 0x00018B93
		protected override void OnValidateProperties()
		{
			base.OnValidateProperties();
			this.depthMapResolution = Mathf.Clamp(Mathf.NextPowerOfTwo(this.depthMapResolution), 8, 2048);
			this.fadeDistanceToSurface = Mathf.Max(this.fadeDistanceToSurface, 0f);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001A9D0 File Offset: 0x00018BD0
		private void InstantiateOrActivateDepthCamera()
		{
			if (this.m_DepthCamera != null)
			{
				this.m_DepthCamera.gameObject.SetActive(true);
				return;
			}
			base.gameObject.ForeachComponentsInDirectChildrenOnly(delegate(Camera cam)
			{
				UnityEngine.Object.DestroyImmediate(cam.gameObject);
			}, true);
			this.m_DepthCamera = Utils.NewWithComponent<Camera>("Depth Camera");
			if (this.m_DepthCamera && this.m_Master)
			{
				this.m_DepthCamera.enabled = false;
				this.m_DepthCamera.cullingMask = this.layerMask;
				this.m_DepthCamera.clearFlags = CameraClearFlags.Depth;
				this.m_DepthCamera.depthTextureMode = DepthTextureMode.Depth;
				this.m_DepthCamera.renderingPath = RenderingPath.VertexLit;
				this.m_DepthCamera.useOcclusionCulling = this.useOcclusionCulling;
				this.m_DepthCamera.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				this.m_DepthCamera.transform.SetParent(base.transform, false);
				Config.Instance.SetURPScriptableRendererIndexToDepthCamera(this.m_DepthCamera);
				RenderTexture targetTexture = new RenderTexture(this.depthMapResolution, this.depthMapResolution, 16, RenderTextureFormat.Depth);
				this.m_DepthCamera.targetTexture = targetTexture;
				this.UpdateDepthCameraPropertiesAccordingToBeam();
			}
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001AB12 File Offset: 0x00018D12
		protected override void OnEnablePostValidate()
		{
			this.InstantiateOrActivateDepthCamera();
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001AB1A File Offset: 0x00018D1A
		protected override void OnDisable()
		{
			base.OnDisable();
			if (this.m_DepthCamera)
			{
				this.m_DepthCamera.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001AB40 File Offset: 0x00018D40
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001AB48 File Offset: 0x00018D48
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.DestroyDepthCamera();
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001AB58 File Offset: 0x00018D58
		private void DestroyDepthCamera()
		{
			if (this.m_DepthCamera)
			{
				if (this.m_DepthCamera.targetTexture)
				{
					this.m_DepthCamera.targetTexture.Release();
					UnityEngine.Object.DestroyImmediate(this.m_DepthCamera.targetTexture);
					this.m_DepthCamera.targetTexture = null;
				}
				UnityEngine.Object.DestroyImmediate(this.m_DepthCamera.gameObject);
				this.m_DepthCamera = null;
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001ABC8 File Offset: 0x00018DC8
		protected override void OnModifyMaterialCallback(MaterialModifier.Interface owner)
		{
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionDepthTexture, this.m_DepthCamera.targetTexture);
			Vector3 lossyScale = this.m_Master.GetLossyScale();
			owner.SetMaterialProp(ShaderProperties.SD.DynamicOcclusionDepthProps, new Vector4(Mathf.Sign(lossyScale.x) * Mathf.Sign(lossyScale.z), Mathf.Sign(lossyScale.y), this.fadeDistanceToSurface, this.m_DepthCamera.orthographic ? 0f : 1f));
		}

		// Token: 0x04000699 RID: 1689
		public new const string ClassName = "DynamicOcclusionDepthBuffer";

		// Token: 0x0400069A RID: 1690
		public LayerMask layerMask = Consts.DynOcclusion.LayerMaskDefault;

		// Token: 0x0400069B RID: 1691
		public bool useOcclusionCulling = true;

		// Token: 0x0400069C RID: 1692
		public int depthMapResolution = 128;

		// Token: 0x0400069D RID: 1693
		public float fadeDistanceToSurface;

		// Token: 0x0400069E RID: 1694
		private Camera m_DepthCamera;

		// Token: 0x0400069F RID: 1695
		private bool m_NeedToUpdateOcclusionNextFrame;
	}
}
