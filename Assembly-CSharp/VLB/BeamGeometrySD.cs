using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x02000137 RID: 311
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-sd/")]
	public class BeamGeometrySD : BeamGeometryAbstractBase, MaterialModifier.Interface
	{
		// Token: 0x06000549 RID: 1353 RVA: 0x000197B5 File Offset: 0x000179B5
		protected override VolumetricLightBeamAbstractBase GetMaster()
		{
			return this.m_Master;
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x000197BD File Offset: 0x000179BD
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x000197CA File Offset: 0x000179CA
		private bool visible
		{
			get
			{
				return base.meshRenderer.enabled;
			}
			set
			{
				base.meshRenderer.enabled = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x000197D8 File Offset: 0x000179D8
		// (set) Token: 0x0600054D RID: 1357 RVA: 0x000197E5 File Offset: 0x000179E5
		public int sortingLayerID
		{
			get
			{
				return base.meshRenderer.sortingLayerID;
			}
			set
			{
				base.meshRenderer.sortingLayerID = value;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x000197F3 File Offset: 0x000179F3
		// (set) Token: 0x0600054F RID: 1359 RVA: 0x00019800 File Offset: 0x00017A00
		public int sortingOrder
		{
			get
			{
				return base.meshRenderer.sortingOrder;
			}
			set
			{
				base.meshRenderer.sortingOrder = value;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001980E File Offset: 0x00017A0E
		public bool _INTERNAL_IsFadeOutCoroutineRunning
		{
			get
			{
				return this.m_CoFadeOut != null;
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001981C File Offset: 0x00017A1C
		private float ComputeFadeOutFactor(Transform camTransform)
		{
			if (this.m_Master.isFadeOutEnabled)
			{
				float value = Vector3.SqrMagnitude(base.meshRenderer.bounds.center - camTransform.position);
				return Mathf.InverseLerp(this.m_Master.fadeOutEnd * this.m_Master.fadeOutEnd, this.m_Master.fadeOutBegin * this.m_Master.fadeOutBegin, value);
			}
			return 1f;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00019894 File Offset: 0x00017A94
		private IEnumerator CoUpdateFadeOut()
		{
			while (this.m_Master.isFadeOutEnabled)
			{
				this.ComputeFadeOutFactor();
				yield return null;
			}
			this.SetFadeOutFactorProp(1f);
			this.m_CoFadeOut = null;
			yield break;
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000198A4 File Offset: 0x00017AA4
		private void ComputeFadeOutFactor()
		{
			Transform fadeOutCameraTransform = Config.Instance.fadeOutCameraTransform;
			if (fadeOutCameraTransform)
			{
				float fadeOutFactorProp = this.ComputeFadeOutFactor(fadeOutCameraTransform);
				this.SetFadeOutFactorProp(fadeOutFactorProp);
				return;
			}
			this.SetFadeOutFactorProp(1f);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000198DF File Offset: 0x00017ADF
		private void SetFadeOutFactorProp(float value)
		{
			if (value > 0f)
			{
				base.meshRenderer.enabled = true;
				this.MaterialChangeStart();
				this.SetMaterialProp(ShaderProperties.SD.FadeOutFactor, value);
				this.MaterialChangeStop();
				return;
			}
			base.meshRenderer.enabled = false;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001991A File Offset: 0x00017B1A
		private void StopFadeOutCoroutine()
		{
			if (this.m_CoFadeOut != null)
			{
				base.StopCoroutine(this.m_CoFadeOut);
				this.m_CoFadeOut = null;
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00019937 File Offset: 0x00017B37
		public void RestartFadeOutCoroutine()
		{
			this.StopFadeOutCoroutine();
			if (this.m_Master && this.m_Master.isFadeOutEnabled)
			{
				this.m_CoFadeOut = base.StartCoroutine(this.CoUpdateFadeOut());
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001996B File Offset: 0x00017B6B
		public void OnMasterEnable()
		{
			this.visible = true;
			this.RestartFadeOutCoroutine();
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001997A File Offset: 0x00017B7A
		public void OnMasterDisable()
		{
			this.StopFadeOutCoroutine();
			this.visible = false;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00019989 File Offset: 0x00017B89
		private void OnDisable()
		{
			SRPHelper.UnregisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
			this.m_CurrentCameraRenderingSRP = null;
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000022C9 File Offset: 0x000004C9
		public static bool isCustomRenderPipelineSupported
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x000199A3 File Offset: 0x00017BA3
		private bool shouldUseGPUInstancedMaterial
		{
			get
			{
				return this.m_Master._INTERNAL_DynamicOcclusionMode != MaterialManager.SD.DynamicOcclusion.DepthTexture && Config.Instance.GetActualRenderingMode(ShaderMode.SD) == RenderingMode.GPUInstancing;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000199C3 File Offset: 0x00017BC3
		private void OnEnable()
		{
			this.RestartFadeOutCoroutine();
			SRPHelper.RegisterOnBeginCameraRendering(new Action<ScriptableRenderContext, Camera>(this.OnBeginCameraRenderingSRP));
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000199DC File Offset: 0x00017BDC
		public void Initialize(VolumetricLightBeamSD master)
		{
			HideFlags proceduralObjectsHideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			this.m_Master = master;
			base.transform.SetParent(master.transform, false);
			base.meshRenderer = base.gameObject.GetOrAddComponent<MeshRenderer>();
			base.meshRenderer.hideFlags = proceduralObjectsHideFlags;
			base.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			base.meshRenderer.receiveShadows = false;
			base.meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			base.meshRenderer.lightProbeUsage = LightProbeUsage.Off;
			if (!this.shouldUseGPUInstancedMaterial)
			{
				this.m_CustomMaterial = Config.Instance.NewMaterialTransient(ShaderMode.SD, false);
				this.ApplyMaterial();
			}
			if (SortingLayer.IsValid(this.m_Master.sortingLayerID))
			{
				this.sortingLayerID = this.m_Master.sortingLayerID;
			}
			else
			{
				Debug.LogError(string.Format("Beam '{0}' has an invalid sortingLayerID ({1}). Please fix it by setting a valid layer.", Utils.GetPath(this.m_Master.transform), this.m_Master.sortingLayerID));
			}
			this.sortingOrder = this.m_Master.sortingOrder;
			base.meshFilter = base.gameObject.GetOrAddComponent<MeshFilter>();
			base.meshFilter.hideFlags = proceduralObjectsHideFlags;
			base.gameObject.hideFlags = proceduralObjectsHideFlags;
			this.RestartFadeOutCoroutine();
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00019B0C File Offset: 0x00017D0C
		public void RegenerateMesh(bool masterEnabled)
		{
			if (Config.Instance.geometryOverrideLayer)
			{
				base.gameObject.layer = Config.Instance.geometryLayerID;
			}
			else
			{
				base.gameObject.layer = this.m_Master.gameObject.layer;
			}
			base.gameObject.tag = Config.Instance.geometryTag;
			if (base.coneMesh && this.m_CurrentMeshType == MeshType.Custom)
			{
				UnityEngine.Object.DestroyImmediate(base.coneMesh);
			}
			this.m_CurrentMeshType = this.m_Master.geomMeshType;
			MeshType geomMeshType = this.m_Master.geomMeshType;
			if (geomMeshType != MeshType.Shared)
			{
				if (geomMeshType == MeshType.Custom)
				{
					base.coneMesh = MeshGenerator.GenerateConeZ_Radii(1f, 1f, 1f, this.m_Master.geomCustomSides, this.m_Master.geomCustomSegments, this.m_Master.geomCap, Config.Instance.SD_requiresDoubleSidedMesh);
					base.coneMesh.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
					base.meshFilter.mesh = base.coneMesh;
				}
				else
				{
					Debug.LogError("Unsupported MeshType");
				}
			}
			else
			{
				base.coneMesh = GlobalMeshSD.Get();
				base.meshFilter.sharedMesh = base.coneMesh;
			}
			this.UpdateMaterialAndBounds();
			this.visible = masterEnabled;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00019C54 File Offset: 0x00017E54
		private Vector3 ComputeLocalMatrix()
		{
			float num = Mathf.Max(this.m_Master.coneRadiusStart, this.m_Master.coneRadiusEnd);
			base.transform.localScale = new Vector3(num, num, this.m_Master.maxGeometryDistance);
			base.transform.localRotation = this.m_Master.beamInternalLocalRotation;
			return base.transform.localScale;
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00019CBB File Offset: 0x00017EBB
		private bool isNoiseEnabled
		{
			get
			{
				return this.m_Master.isNoiseEnabled && this.m_Master.noiseIntensity > 0f && Noise3D.isSupported;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00019CE3 File Offset: 0x00017EE3
		private bool isDepthBlendEnabled
		{
			get
			{
				return BatchingHelper.forceEnableDepthBlend || this.m_Master.depthBlendDistance > 0f;
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00019D00 File Offset: 0x00017F00
		private MaterialManager.StaticPropertiesSD ComputeMaterialStaticProperties()
		{
			MaterialManager.ColorGradient colorGradient = MaterialManager.ColorGradient.Off;
			if (this.m_Master.colorMode == ColorMode.Gradient)
			{
				colorGradient = ((Utils.GetFloatPackingPrecision() == Utils.FloatPackingPrecision.High) ? MaterialManager.ColorGradient.MatrixHigh : MaterialManager.ColorGradient.MatrixLow);
			}
			return new MaterialManager.StaticPropertiesSD
			{
				blendingMode = (MaterialManager.BlendingMode)this.m_Master.blendingMode,
				noise3D = (this.isNoiseEnabled ? MaterialManager.Noise3D.On : MaterialManager.Noise3D.Off),
				depthBlend = (this.isDepthBlendEnabled ? MaterialManager.SD.DepthBlend.On : MaterialManager.SD.DepthBlend.Off),
				colorGradient = colorGradient,
				dynamicOcclusion = this.m_Master._INTERNAL_DynamicOcclusionMode_Runtime,
				meshSkewing = (this.m_Master.hasMeshSkewing ? MaterialManager.SD.MeshSkewing.On : MaterialManager.SD.MeshSkewing.Off),
				shaderAccuracy = ((this.m_Master.shaderAccuracy == ShaderAccuracy.Fast) ? MaterialManager.SD.ShaderAccuracy.Fast : MaterialManager.SD.ShaderAccuracy.High)
			};
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00019DB8 File Offset: 0x00017FB8
		private bool ApplyMaterial()
		{
			MaterialManager.StaticPropertiesSD staticPropertiesSD = this.ComputeMaterialStaticProperties();
			Material material;
			if (!this.shouldUseGPUInstancedMaterial)
			{
				material = this.m_CustomMaterial;
				if (material)
				{
					staticPropertiesSD.ApplyToMaterial(material);
				}
			}
			else
			{
				material = MaterialManager.GetInstancedMaterial(this.m_Master._INTERNAL_InstancedMaterialGroupID, ref staticPropertiesSD);
			}
			base.meshRenderer.material = material;
			return material != null;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00016611 File Offset: 0x00014811
		public void SetMaterialProp(int nameID, float value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetFloat(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetFloat(nameID, value);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001663A File Offset: 0x0001483A
		public void SetMaterialProp(int nameID, Vector4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetVector(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetVector(nameID, value);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00016663 File Offset: 0x00014863
		public void SetMaterialProp(int nameID, Color value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetColor(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetColor(nameID, value);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001668C File Offset: 0x0001488C
		public void SetMaterialProp(int nameID, Matrix4x4 value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetMatrix(nameID, value);
				return;
			}
			MaterialManager.materialPropertyBlock.SetMatrix(nameID, value);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00019E15 File Offset: 0x00018015
		public void SetMaterialProp(int nameID, Texture value)
		{
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetTexture(nameID, value);
				return;
			}
			Debug.LogError("Setting a Texture property to a GPU instanced material is not supported");
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00016715 File Offset: 0x00014915
		private void MaterialChangeStart()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.GetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00016735 File Offset: 0x00014935
		private void MaterialChangeStop()
		{
			if (this.m_CustomMaterial == null)
			{
				base.meshRenderer.SetPropertyBlock(MaterialManager.materialPropertyBlock);
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00019E3C File Offset: 0x0001803C
		public void SetDynamicOcclusionCallback(string shaderKeyword, MaterialModifier.Callback cb)
		{
			this.m_MaterialModifierCallback = cb;
			if (this.m_CustomMaterial)
			{
				this.m_CustomMaterial.SetKeywordEnabled(shaderKeyword, cb != null);
				if (cb != null)
				{
					cb(this);
					return;
				}
			}
			else
			{
				this.UpdateMaterialAndBounds();
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00019E74 File Offset: 0x00018074
		public void UpdateMaterialAndBounds()
		{
			if (!this.ApplyMaterial())
			{
				return;
			}
			this.MaterialChangeStart();
			if (this.m_CustomMaterial == null && this.m_MaterialModifierCallback != null)
			{
				this.m_MaterialModifierCallback(this);
			}
			float f = this.m_Master.coneAngle * 0.017453292f / 2f;
			this.SetMaterialProp(ShaderProperties.SD.ConeSlopeCosSin, new Vector2(Mathf.Cos(f), Mathf.Sin(f)));
			Vector2 v = new Vector2(Mathf.Max(this.m_Master.coneRadiusStart, 0.0001f), Mathf.Max(this.m_Master.coneRadiusEnd, 0.0001f));
			this.SetMaterialProp(ShaderProperties.ConeRadius, v);
			float x = Mathf.Sign(this.m_Master.coneApexOffsetZ) * Mathf.Max(Mathf.Abs(this.m_Master.coneApexOffsetZ), 0.0001f);
			this.SetMaterialProp(ShaderProperties.ConeGeomProps, new Vector2(x, (float)this.m_Master.geomSides));
			if (this.m_Master.usedColorMode == ColorMode.Flat)
			{
				this.SetMaterialProp(ShaderProperties.ColorFlat, this.m_Master.color);
			}
			else
			{
				Utils.FloatPackingPrecision floatPackingPrecision = Utils.GetFloatPackingPrecision();
				this.m_ColorGradientMatrix = this.m_Master.colorGradient.SampleInMatrix((int)floatPackingPrecision);
			}
			float value;
			float value2;
			this.m_Master.GetInsideAndOutsideIntensity(out value, out value2);
			this.SetMaterialProp(ShaderProperties.SD.AlphaInside, value);
			this.SetMaterialProp(ShaderProperties.SD.AlphaOutside, value2);
			this.SetMaterialProp(ShaderProperties.SD.AttenuationLerpLinearQuad, this.m_Master.attenuationLerpLinearQuad);
			this.SetMaterialProp(ShaderProperties.DistanceFallOff, new Vector3(this.m_Master.fallOffStart, this.m_Master.fallOffEnd, this.m_Master.maxGeometryDistance));
			this.SetMaterialProp(ShaderProperties.SD.DistanceCamClipping, this.m_Master.cameraClippingDistance);
			this.SetMaterialProp(ShaderProperties.SD.FresnelPow, Mathf.Max(0.001f, this.m_Master.fresnelPow));
			this.SetMaterialProp(ShaderProperties.SD.GlareBehind, this.m_Master.glareBehind);
			this.SetMaterialProp(ShaderProperties.SD.GlareFrontal, this.m_Master.glareFrontal);
			this.SetMaterialProp(ShaderProperties.SD.DrawCap, (float)(this.m_Master.geomCap ? 1 : 0));
			this.SetMaterialProp(ShaderProperties.SD.TiltVector, this.m_Master.tiltFactor);
			this.SetMaterialProp(ShaderProperties.SD.AdditionalClippingPlaneWS, this.m_Master.additionalClippingPlane);
			if (Config.Instance.isHDRPExposureWeightSupported)
			{
				this.SetMaterialProp(ShaderProperties.HDRPExposureWeight, this.m_Master.hdrpExposureWeight);
			}
			if (this.isDepthBlendEnabled)
			{
				this.SetMaterialProp(ShaderProperties.SD.DepthBlendDistance, this.m_Master.depthBlendDistance);
			}
			if (this.isNoiseEnabled)
			{
				Noise3D.LoadIfNeeded();
				Vector3 vector = this.m_Master.noiseVelocityUseGlobal ? Config.Instance.globalNoiseVelocity : this.m_Master.noiseVelocityLocal;
				float w = this.m_Master.noiseScaleUseGlobal ? Config.Instance.globalNoiseScale : this.m_Master.noiseScaleLocal;
				this.SetMaterialProp(ShaderProperties.NoiseVelocityAndScale, new Vector4(vector.x, vector.y, vector.z, w));
				this.SetMaterialProp(ShaderProperties.NoiseParam, new Vector2(this.m_Master.noiseIntensity, (this.m_Master.noiseMode == NoiseMode.WorldSpace) ? 0f : 1f));
			}
			Vector3 vector2 = this.ComputeLocalMatrix();
			if (this.m_Master.hasMeshSkewing)
			{
				Vector3 skewingLocalForwardDirectionNormalized = this.m_Master.skewingLocalForwardDirectionNormalized;
				this.SetMaterialProp(ShaderProperties.SD.LocalForwardDirection, skewingLocalForwardDirectionNormalized);
				if (base.coneMesh != null)
				{
					Vector3 vector3 = skewingLocalForwardDirectionNormalized;
					vector3 /= vector3.z;
					vector3 *= this.m_Master.fallOffEnd;
					vector3.x /= vector2.x;
					vector3.y /= vector2.y;
					Bounds bounds = MeshGenerator.ComputeBounds(1f, 1f, 1f);
					Vector3 min = bounds.min;
					Vector3 max = bounds.max;
					if (vector3.x > 0f)
					{
						max.x += vector3.x;
					}
					else
					{
						min.x += vector3.x;
					}
					if (vector3.y > 0f)
					{
						max.y += vector3.y;
					}
					else
					{
						min.y += vector3.y;
					}
					bounds.min = min;
					bounds.max = max;
					base.coneMesh.bounds = bounds;
				}
			}
			this.UpdateMatricesPropertiesForGPUInstancingSRP();
			this.MaterialChangeStop();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001A330 File Offset: 0x00018530
		private void UpdateMatricesPropertiesForGPUInstancingSRP()
		{
			if (SRPHelper.IsUsingCustomRenderPipeline() && Config.Instance.GetActualRenderingMode(ShaderMode.SD) == RenderingMode.GPUInstancing)
			{
				this.SetMaterialProp(ShaderProperties.LocalToWorldMatrix, base.transform.localToWorldMatrix);
				this.SetMaterialProp(ShaderProperties.WorldToLocalMatrix, base.transform.worldToLocalMatrix);
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001A37E File Offset: 0x0001857E
		private void OnBeginCameraRenderingSRP(ScriptableRenderContext context, Camera cam)
		{
			this.m_CurrentCameraRenderingSRP = cam;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0001A388 File Offset: 0x00018588
		private void OnWillRenderObject()
		{
			Camera cam;
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				cam = this.m_CurrentCameraRenderingSRP;
			}
			else
			{
				cam = Camera.current;
			}
			this.OnWillCameraRenderThisBeam(cam);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0001A3B4 File Offset: 0x000185B4
		private void OnWillCameraRenderThisBeam(Camera cam)
		{
			if (this.m_Master && cam && cam.enabled)
			{
				this.UpdateCameraRelatedProperties(cam);
				this.m_Master._INTERNAL_OnWillCameraRenderThisBeam(cam);
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001A3E8 File Offset: 0x000185E8
		private void UpdateCameraRelatedProperties(Camera cam)
		{
			if (cam && this.m_Master)
			{
				this.MaterialChangeStart();
				Vector3 posOS = this.m_Master.transform.InverseTransformPoint(cam.transform.position);
				Vector3 normalized = base.transform.InverseTransformDirection(cam.transform.forward).normalized;
				float w = cam.orthographic ? -1f : this.m_Master.GetInsideBeamFactorFromObjectSpacePos(posOS);
				this.SetMaterialProp(ShaderProperties.SD.CameraParams, new Vector4(normalized.x, normalized.y, normalized.z, w));
				this.UpdateMatricesPropertiesForGPUInstancingSRP();
				if (this.m_Master.usedColorMode == ColorMode.Gradient)
				{
					this.SetMaterialProp(ShaderProperties.ColorGradientMatrix, this.m_ColorGradientMatrix);
				}
				this.MaterialChangeStop();
				if (this.m_Master.depthBlendDistance > 0f)
				{
					cam.depthTextureMode |= DepthTextureMode.Depth;
				}
			}
		}

		// Token: 0x04000683 RID: 1667
		private VolumetricLightBeamSD m_Master;

		// Token: 0x04000684 RID: 1668
		private MeshType m_CurrentMeshType;

		// Token: 0x04000685 RID: 1669
		private MaterialModifier.Callback m_MaterialModifierCallback;

		// Token: 0x04000686 RID: 1670
		private Coroutine m_CoFadeOut;

		// Token: 0x04000687 RID: 1671
		private Camera m_CurrentCameraRenderingSRP;
	}
}
