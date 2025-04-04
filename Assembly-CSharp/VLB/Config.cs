using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace VLB
{
	// Token: 0x020000DF RID: 223
	[HelpURL("http://saladgamer.com/vlb-doc/config/")]
	public class Config : ScriptableObject
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0001527B File Offset: 0x0001347B
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00015283 File Offset: 0x00013483
		public RenderPipeline renderPipeline
		{
			get
			{
				return this.m_RenderPipeline;
			}
			set
			{
				Debug.LogError("Modifying the RenderPipeline in standalone builds is not permitted");
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0001528F File Offset: 0x0001348F
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00015297 File Offset: 0x00013497
		public RenderingMode renderingMode
		{
			get
			{
				return this.m_RenderingMode;
			}
			set
			{
				Debug.LogError("Modifying the RenderingMode in standalone builds is not permitted");
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000152A4 File Offset: 0x000134A4
		public bool IsSRPBatcherSupported()
		{
			if (this.renderPipeline == RenderPipeline.BuiltIn)
			{
				return false;
			}
			RenderPipeline projectRenderPipeline = SRPHelper.projectRenderPipeline;
			return projectRenderPipeline == RenderPipeline.URP || projectRenderPipeline == RenderPipeline.HDRP;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x000152CB File Offset: 0x000134CB
		public RenderingMode GetActualRenderingMode(ShaderMode shaderMode)
		{
			if (this.renderingMode == RenderingMode.SRPBatcher && !this.IsSRPBatcherSupported())
			{
				return RenderingMode.Default;
			}
			if (this.renderPipeline != RenderPipeline.BuiltIn && this.renderingMode == RenderingMode.MultiPass)
			{
				return RenderingMode.Default;
			}
			if (shaderMode == ShaderMode.HD && this.renderingMode == RenderingMode.MultiPass)
			{
				return RenderingMode.Default;
			}
			return this.renderingMode;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00015306 File Offset: 0x00013506
		public bool SD_useSinglePassShader
		{
			get
			{
				return this.GetActualRenderingMode(ShaderMode.SD) > RenderingMode.MultiPass;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00015312 File Offset: 0x00013512
		public bool SD_requiresDoubleSidedMesh
		{
			get
			{
				return this.SD_useSinglePassShader;
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001531A File Offset: 0x0001351A
		public unsafe Shader GetBeamShader(ShaderMode mode)
		{
			return *this.GetBeamShaderInternal(mode);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00015324 File Offset: 0x00013524
		private ref Shader GetBeamShaderInternal(ShaderMode mode)
		{
			if (mode == ShaderMode.SD)
			{
				return ref this._BeamShader;
			}
			return ref this._BeamShaderHD;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00015336 File Offset: 0x00013536
		private int GetRenderQueueInternal(ShaderMode mode)
		{
			if (mode == ShaderMode.SD)
			{
				return this.geometryRenderQueue;
			}
			return this.geometryRenderQueueHD;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00015348 File Offset: 0x00013548
		public Material NewMaterialTransient(ShaderMode mode, bool gpuInstanced)
		{
			Material material = MaterialManager.NewMaterialPersistent(this.GetBeamShader(mode), gpuInstanced);
			if (material)
			{
				material.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
				material.renderQueue = this.GetRenderQueueInternal(mode);
			}
			return material;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00015384 File Offset: 0x00013584
		public void SetURPScriptableRendererIndexToDepthCamera(Camera camera)
		{
			if (this.urpDepthCameraScriptableRendererIndex < 0)
			{
				return;
			}
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.GetUniversalAdditionalCameraData();
			if (universalAdditionalCameraData)
			{
				universalAdditionalCameraData.SetRenderer(this.urpDepthCameraScriptableRendererIndex);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x000153B6 File Offset: 0x000135B6
		public Transform fadeOutCameraTransform
		{
			get
			{
				if (this.m_CachedFadeOutCamera == null)
				{
					this.ForceUpdateFadeOutCamera();
				}
				return this.m_CachedFadeOutCamera;
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000153D4 File Offset: 0x000135D4
		public void ForceUpdateFadeOutCamera()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.fadeOutCameraTag);
			if (gameObject)
			{
				this.m_CachedFadeOutCamera = gameObject.transform;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00015401 File Offset: 0x00013601
		public int defaultRaymarchingQualityUniqueID
		{
			get
			{
				return this.m_DefaultRaymarchingQualityUniqueID;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00015409 File Offset: 0x00013609
		public RaymarchingQuality GetRaymarchingQualityForIndex(int index)
		{
			return this.m_RaymarchingQualities[index];
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00015414 File Offset: 0x00013614
		public RaymarchingQuality GetRaymarchingQualityForUniqueID(int id)
		{
			int raymarchingQualityIndexForUniqueID = this.GetRaymarchingQualityIndexForUniqueID(id);
			if (raymarchingQualityIndexForUniqueID >= 0)
			{
				return this.GetRaymarchingQualityForIndex(raymarchingQualityIndexForUniqueID);
			}
			return null;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00015438 File Offset: 0x00013638
		public int GetRaymarchingQualityIndexForUniqueID(int id)
		{
			for (int i = 0; i < this.m_RaymarchingQualities.Length; i++)
			{
				RaymarchingQuality raymarchingQuality = this.m_RaymarchingQualities[i];
				if (raymarchingQuality != null && raymarchingQuality.uniqueID == id)
				{
					return i;
				}
			}
			Debug.LogErrorFormat("Failed to find RaymarchingQualityIndex for Unique ID {0}", new object[]
			{
				id
			});
			return -1;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00015489 File Offset: 0x00013689
		public bool IsRaymarchingQualityUniqueIDValid(int id)
		{
			return this.GetRaymarchingQualityIndexForUniqueID(id) >= 0;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00015498 File Offset: 0x00013698
		public int raymarchingQualitiesCount
		{
			get
			{
				return Mathf.Max(1, (this.m_RaymarchingQualities != null) ? this.m_RaymarchingQualities.Length : 1);
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000154B4 File Offset: 0x000136B4
		private void CreateDefaultRaymarchingQualityPreset(bool onlyIfNeeded)
		{
			if (this.m_RaymarchingQualities == null || this.m_RaymarchingQualities.Length == 0 || !onlyIfNeeded)
			{
				this.m_RaymarchingQualities = new RaymarchingQuality[3];
				this.m_RaymarchingQualities[0] = RaymarchingQuality.New("Fast", 1, 5);
				this.m_RaymarchingQualities[1] = RaymarchingQuality.New("Balanced", 2, 10);
				this.m_RaymarchingQualities[2] = RaymarchingQuality.New("High", 3, 20);
				this.m_DefaultRaymarchingQualityUniqueID = this.m_RaymarchingQualities[1].uniqueID;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00015532 File Offset: 0x00013732
		public bool isHDRPExposureWeightSupported
		{
			get
			{
				return this.renderPipeline == RenderPipeline.HDRP;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0001553D File Offset: 0x0001373D
		public bool hasRenderPipelineMismatch
		{
			get
			{
				return SRPHelper.projectRenderPipeline == RenderPipeline.BuiltIn != (this.m_RenderPipeline == RenderPipeline.BuiltIn);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00015555 File Offset: 0x00013755
		[RuntimeInitializeOnLoadMethod]
		private static void OnStartup()
		{
			Config.Instance.m_CachedFadeOutCamera = null;
			Config.Instance.RefreshGlobalShaderProperties();
			if (Config.Instance.hasRenderPipelineMismatch)
			{
				Debug.LogError("It looks like the 'Render Pipeline' is not correctly set in the config. Please make sure to select the proper value depending on your pipeline in use.", Config.Instance);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00015588 File Offset: 0x00013788
		public void Reset()
		{
			this.geometryOverrideLayer = true;
			this.geometryLayerID = 1;
			this.geometryTag = "Untagged";
			this.geometryRenderQueue = 3000;
			this.geometryRenderQueueHD = 3100;
			this.sharedMeshSides = 24;
			this.sharedMeshSegments = 5;
			this.globalNoiseScale = 0.5f;
			this.globalNoiseVelocity = Consts.Beam.NoiseVelocityDefault;
			this.renderPipeline = RenderPipeline.BuiltIn;
			this.renderingMode = RenderingMode.Default;
			this.ditheringFactor = 0f;
			this.useLightColorTemperature = true;
			this.fadeOutCameraTag = "MainCamera";
			this.featureEnabledColorGradient = FeatureEnabledColorGradient.HighOnly;
			this.featureEnabledDepthBlend = true;
			this.featureEnabledNoise3D = true;
			this.featureEnabledDynamicOcclusion = true;
			this.featureEnabledMeshSkewing = true;
			this.featureEnabledShaderAccuracyHigh = true;
			this.hdBeamsCameraBlendingDistance = 0.5f;
			this.urpDepthCameraScriptableRendererIndex = -1;
			this.CreateDefaultRaymarchingQualityPreset(false);
			this.ResetInternalData();
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00015660 File Offset: 0x00013860
		private void RefreshGlobalShaderProperties()
		{
			Shader.SetGlobalFloat(ShaderProperties.GlobalUsesReversedZBuffer, SystemInfo.usesReversedZBuffer ? 1f : 0f);
			Shader.SetGlobalFloat(ShaderProperties.GlobalDitheringFactor, this.ditheringFactor);
			Shader.SetGlobalTexture(ShaderProperties.GlobalDitheringNoiseTex, this.ditheringNoiseTexture);
			Shader.SetGlobalFloat(ShaderProperties.HD.GlobalCameraBlendingDistance, this.hdBeamsCameraBlendingDistance);
			Shader.SetGlobalTexture(ShaderProperties.HD.GlobalJitteringNoiseTex, this.jitteringNoiseTexture);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000156CC File Offset: 0x000138CC
		public void ResetInternalData()
		{
			this.noiseTexture3D = (Resources.Load("Noise3D_64x64x64") as Texture3D);
			this.dustParticlesPrefab = (Resources.Load("DustParticles", typeof(ParticleSystem)) as ParticleSystem);
			this.ditheringNoiseTexture = (Resources.Load("VLBDitheringNoise", typeof(Texture2D)) as Texture2D);
			this.jitteringNoiseTexture = (Resources.Load("VLBBlueNoise", typeof(Texture2D)) as Texture2D);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001574C File Offset: 0x0001394C
		public ParticleSystem NewVolumetricDustParticles()
		{
			if (!this.dustParticlesPrefab)
			{
				if (Application.isPlaying)
				{
					Debug.LogError("Failed to instantiate VolumetricDustParticles prefab.");
				}
				return null;
			}
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.dustParticlesPrefab);
			particleSystem.useAutoRandomSeed = false;
			particleSystem.name = "Dust Particles";
			particleSystem.gameObject.hideFlags = Consts.Internal.ProceduralObjectsHideFlags;
			particleSystem.gameObject.SetActive(true);
			return particleSystem;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000157B2 File Offset: 0x000139B2
		private void OnEnable()
		{
			this.CreateDefaultRaymarchingQualityPreset(true);
			this.HandleBackwardCompatibility(this.pluginVersion, 20100);
			this.pluginVersion = 20100;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000045B1 File Offset: 0x000027B1
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003CA RID: 970 RVA: 0x000157D7 File Offset: 0x000139D7
		public static Config Instance
		{
			get
			{
				return Config.GetInstance(true);
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000157DF File Offset: 0x000139DF
		private static Config LoadAssetInternal(string assetName)
		{
			return Resources.Load<Config>(assetName);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000157E8 File Offset: 0x000139E8
		private static Config GetInstance(bool assertIfNotFound)
		{
			if (Config.ms_Instance == null)
			{
				Config x = Config.LoadAssetInternal("VLBConfigOverride" + PlatformHelper.GetCurrentPlatformSuffix());
				if (x == null)
				{
					x = Config.LoadAssetInternal("VLBConfigOverride");
				}
				Config.ms_Instance = x;
				Config.ms_Instance == null;
			}
			return Config.ms_Instance;
		}

		// Token: 0x04000485 RID: 1157
		public const string ClassName = "Config";

		// Token: 0x04000486 RID: 1158
		public const string kAssetName = "VLBConfigOverride";

		// Token: 0x04000487 RID: 1159
		public const string kAssetNameExt = ".asset";

		// Token: 0x04000488 RID: 1160
		public bool geometryOverrideLayer = true;

		// Token: 0x04000489 RID: 1161
		public int geometryLayerID = 1;

		// Token: 0x0400048A RID: 1162
		public string geometryTag = "Untagged";

		// Token: 0x0400048B RID: 1163
		public int geometryRenderQueue = 3000;

		// Token: 0x0400048C RID: 1164
		public int geometryRenderQueueHD = 3100;

		// Token: 0x0400048D RID: 1165
		[FormerlySerializedAs("renderPipeline")]
		[FormerlySerializedAs("_RenderPipeline")]
		[SerializeField]
		private RenderPipeline m_RenderPipeline;

		// Token: 0x0400048E RID: 1166
		[FormerlySerializedAs("renderingMode")]
		[FormerlySerializedAs("_RenderingMode")]
		[SerializeField]
		private RenderingMode m_RenderingMode = RenderingMode.Default;

		// Token: 0x0400048F RID: 1167
		public float ditheringFactor;

		// Token: 0x04000490 RID: 1168
		public bool useLightColorTemperature = true;

		// Token: 0x04000491 RID: 1169
		public int sharedMeshSides = 24;

		// Token: 0x04000492 RID: 1170
		public int sharedMeshSegments = 5;

		// Token: 0x04000493 RID: 1171
		public float hdBeamsCameraBlendingDistance = 0.5f;

		// Token: 0x04000494 RID: 1172
		public int urpDepthCameraScriptableRendererIndex = -1;

		// Token: 0x04000495 RID: 1173
		[Range(0.01f, 2f)]
		public float globalNoiseScale = 0.5f;

		// Token: 0x04000496 RID: 1174
		public Vector3 globalNoiseVelocity = Consts.Beam.NoiseVelocityDefault;

		// Token: 0x04000497 RID: 1175
		public string fadeOutCameraTag = "MainCamera";

		// Token: 0x04000498 RID: 1176
		[HighlightNull]
		public Texture3D noiseTexture3D;

		// Token: 0x04000499 RID: 1177
		[HighlightNull]
		public ParticleSystem dustParticlesPrefab;

		// Token: 0x0400049A RID: 1178
		[HighlightNull]
		public Texture2D ditheringNoiseTexture;

		// Token: 0x0400049B RID: 1179
		[HighlightNull]
		public Texture2D jitteringNoiseTexture;

		// Token: 0x0400049C RID: 1180
		public FeatureEnabledColorGradient featureEnabledColorGradient = FeatureEnabledColorGradient.HighOnly;

		// Token: 0x0400049D RID: 1181
		public bool featureEnabledDepthBlend = true;

		// Token: 0x0400049E RID: 1182
		public bool featureEnabledNoise3D = true;

		// Token: 0x0400049F RID: 1183
		public bool featureEnabledDynamicOcclusion = true;

		// Token: 0x040004A0 RID: 1184
		public bool featureEnabledMeshSkewing = true;

		// Token: 0x040004A1 RID: 1185
		public bool featureEnabledShaderAccuracyHigh = true;

		// Token: 0x040004A2 RID: 1186
		public bool featureEnabledShadow = true;

		// Token: 0x040004A3 RID: 1187
		public bool featureEnabledCookie = true;

		// Token: 0x040004A4 RID: 1188
		[SerializeField]
		private RaymarchingQuality[] m_RaymarchingQualities;

		// Token: 0x040004A5 RID: 1189
		[SerializeField]
		private int m_DefaultRaymarchingQualityUniqueID;

		// Token: 0x040004A6 RID: 1190
		[SerializeField]
		private int pluginVersion = -1;

		// Token: 0x040004A7 RID: 1191
		[SerializeField]
		private Material _DummyMaterial;

		// Token: 0x040004A8 RID: 1192
		[SerializeField]
		private Material _DummyMaterialHD;

		// Token: 0x040004A9 RID: 1193
		[SerializeField]
		private Shader _BeamShader;

		// Token: 0x040004AA RID: 1194
		[SerializeField]
		private Shader _BeamShaderHD;

		// Token: 0x040004AB RID: 1195
		private Transform m_CachedFadeOutCamera;

		// Token: 0x040004AC RID: 1196
		private static Config ms_Instance;
	}
}
