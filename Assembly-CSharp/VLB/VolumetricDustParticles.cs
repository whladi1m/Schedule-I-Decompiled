using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000157 RID: 343
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamAbstractBase))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-dustparticles/")]
	public class VolumetricDustParticles : MonoBehaviour
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0001D8E3 File Offset: 0x0001BAE3
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0001D8EB File Offset: 0x0001BAEB
		public bool isCulled { get; private set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0001D8F4 File Offset: 0x0001BAF4
		// (set) Token: 0x06000696 RID: 1686 RVA: 0x0001D8FC File Offset: 0x0001BAFC
		public float alphaAdditionalRuntime
		{
			get
			{
				return this.m_AlphaAdditionalRuntime;
			}
			set
			{
				if (this.m_AlphaAdditionalRuntime != value)
				{
					this.m_AlphaAdditionalRuntime = value;
					this.m_RuntimePropertiesDirty = true;
				}
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000697 RID: 1687 RVA: 0x0001D915 File Offset: 0x0001BB15
		public bool particlesAreInstantiated
		{
			get
			{
				return this.m_Particles;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001D922 File Offset: 0x0001BB22
		public int particlesCurrentCount
		{
			get
			{
				if (!this.m_Particles)
				{
					return 0;
				}
				return this.m_Particles.particleCount;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001D940 File Offset: 0x0001BB40
		public int particlesMaxCount
		{
			get
			{
				if (!this.m_Particles)
				{
					return 0;
				}
				return this.m_Particles.main.maxParticles;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001D970 File Offset: 0x0001BB70
		public Camera mainCamera
		{
			get
			{
				if (!VolumetricDustParticles.ms_MainCamera)
				{
					VolumetricDustParticles.ms_MainCamera = Camera.main;
					if (!VolumetricDustParticles.ms_MainCamera && !VolumetricDustParticles.ms_NoMainCameraLogged)
					{
						Debug.LogErrorFormat(base.gameObject, "In order to use 'VolumetricDustParticles' culling, you must have a MainCamera defined in your scene.", Array.Empty<object>());
						VolumetricDustParticles.ms_NoMainCameraLogged = true;
					}
				}
				return VolumetricDustParticles.ms_MainCamera;
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001D9C6 File Offset: 0x0001BBC6
		private void Start()
		{
			this.isCulled = false;
			this.m_Master = base.GetComponent<VolumetricLightBeamAbstractBase>();
			this.HandleBackwardCompatibility(this.m_Master._INTERNAL_pluginVersion, 20100);
			this.InstantiateParticleSystem();
			this.SetActiveAndPlay();
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001DA00 File Offset: 0x0001BC00
		private void InstantiateParticleSystem()
		{
			base.gameObject.ForeachComponentsInDirectChildrenOnly(delegate(ParticleSystem ps)
			{
				UnityEngine.Object.DestroyImmediate(ps.gameObject);
			}, true);
			this.m_Particles = Config.Instance.NewVolumetricDustParticles();
			if (this.m_Particles)
			{
				this.m_Particles.transform.SetParent(base.transform, false);
				this.m_Renderer = this.m_Particles.GetComponent<ParticleSystemRenderer>();
				this.m_Material = new Material(this.m_Renderer.sharedMaterial);
				this.m_Renderer.material = this.m_Material;
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001DAA4 File Offset: 0x0001BCA4
		private void OnEnable()
		{
			this.SetActiveAndPlay();
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001DAAC File Offset: 0x0001BCAC
		private void SetActive(bool active)
		{
			if (this.m_Particles)
			{
				this.m_Particles.gameObject.SetActive(active);
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001DACC File Offset: 0x0001BCCC
		private void SetActiveAndPlay()
		{
			this.SetActive(true);
			this.Play();
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001DADB File Offset: 0x0001BCDB
		private void Play()
		{
			if (this.m_Particles)
			{
				this.SetParticleProperties();
				this.m_Particles.Simulate(0f);
				this.m_Particles.Play(true);
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001DB0C File Offset: 0x0001BD0C
		private void OnDisable()
		{
			this.SetActive(false);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001DB18 File Offset: 0x0001BD18
		private void OnDestroy()
		{
			if (this.m_Particles)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Particles.gameObject);
				this.m_Particles = null;
			}
			if (this.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material);
				this.m_Material = null;
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001DB68 File Offset: 0x0001BD68
		private void Update()
		{
			this.UpdateCulling();
			if (UtilsBeamProps.CanChangeDuringPlaytime(this.m_Master))
			{
				this.SetParticleProperties();
			}
			if (this.m_RuntimePropertiesDirty && this.m_Material != null)
			{
				this.m_Material.SetColor(ShaderProperties.ParticlesTintColor, new Color(1f, 1f, 1f, this.alphaAdditionalRuntime));
				this.m_RuntimePropertiesDirty = false;
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001DBD8 File Offset: 0x0001BDD8
		private void SetParticleProperties()
		{
			if (this.m_Particles && this.m_Particles.gameObject.activeSelf)
			{
				this.m_Particles.transform.localRotation = UtilsBeamProps.GetInternalLocalRotation(this.m_Master);
				this.m_Particles.transform.localScale = (this.m_Master.IsScalable() ? Vector3.one : Vector3.one.Divide(this.m_Master.GetLossyScale()));
				float num = UtilsBeamProps.GetFallOffEnd(this.m_Master) * (this.spawnDistanceRange.maxValue - this.spawnDistanceRange.minValue);
				float num2 = num * this.density;
				int maxParticles = (int)(num2 * 4f);
				ParticleSystem.MainModule main = this.m_Particles.main;
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				startLifetime.mode = ParticleSystemCurveMode.TwoConstants;
				startLifetime.constantMin = 4f;
				startLifetime.constantMax = 6f;
				main.startLifetime = startLifetime;
				ParticleSystem.MinMaxCurve startSize = main.startSize;
				startSize.mode = ParticleSystemCurveMode.TwoConstants;
				startSize.constantMin = this.size * 0.9f;
				startSize.constantMax = this.size * 1.1f;
				main.startSize = startSize;
				ParticleSystem.MinMaxGradient startColor = main.startColor;
				if (UtilsBeamProps.GetColorMode(this.m_Master) == ColorMode.Flat)
				{
					startColor.mode = ParticleSystemGradientMode.Color;
					Color colorFlat = UtilsBeamProps.GetColorFlat(this.m_Master);
					colorFlat.a *= this.alpha;
					startColor.color = colorFlat;
				}
				else
				{
					startColor.mode = ParticleSystemGradientMode.Gradient;
					Gradient colorGradient = UtilsBeamProps.GetColorGradient(this.m_Master);
					GradientColorKey[] colorKeys = colorGradient.colorKeys;
					GradientAlphaKey[] alphaKeys = colorGradient.alphaKeys;
					for (int i = 0; i < alphaKeys.Length; i++)
					{
						GradientAlphaKey[] array = alphaKeys;
						int num3 = i;
						array[num3].alpha = array[num3].alpha * this.alpha;
					}
					this.m_GradientCached.SetKeys(colorKeys, alphaKeys);
					startColor.gradient = this.m_GradientCached;
				}
				main.startColor = startColor;
				ParticleSystem.MinMaxCurve startSpeed = main.startSpeed;
				startSpeed.constant = ((this.direction == ParticlesDirection.Random) ? Mathf.Abs(this.velocity.z) : 0f);
				main.startSpeed = startSpeed;
				ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.m_Particles.velocityOverLifetime;
				velocityOverLifetime.enabled = (this.direction > ParticlesDirection.Random);
				velocityOverLifetime.space = ((this.direction == ParticlesDirection.LocalSpace) ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World);
				velocityOverLifetime.xMultiplier = this.velocity.x;
				velocityOverLifetime.yMultiplier = this.velocity.y;
				velocityOverLifetime.zMultiplier = this.velocity.z;
				main.maxParticles = maxParticles;
				float thickness = UtilsBeamProps.GetThickness(this.m_Master);
				float fallOffEnd = UtilsBeamProps.GetFallOffEnd(this.m_Master);
				ParticleSystem.ShapeModule shape = this.m_Particles.shape;
				shape.shapeType = ParticleSystemShapeType.ConeVolume;
				float num4 = UtilsBeamProps.GetConeAngle(this.m_Master) * Mathf.Lerp(0.7f, 1f, thickness);
				shape.angle = num4 * 0.5f;
				float a = UtilsBeamProps.GetConeRadiusStart(this.m_Master) * Mathf.Lerp(0.3f, 1f, thickness);
				float b = Utils.ComputeConeRadiusEnd(fallOffEnd, num4);
				shape.radius = Mathf.Lerp(a, b, this.spawnDistanceRange.minValue);
				shape.length = num;
				float z = fallOffEnd * this.spawnDistanceRange.minValue;
				shape.position = new Vector3(0f, 0f, z);
				shape.arc = 360f;
				shape.randomDirectionAmount = ((this.direction == ParticlesDirection.Random) ? 1f : 0f);
				ParticleSystem.EmissionModule emission = this.m_Particles.emission;
				ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
				rateOverTime.constant = num2;
				emission.rateOverTime = rateOverTime;
				if (this.m_Renderer)
				{
					this.m_Renderer.sortingLayerID = UtilsBeamProps.GetSortingLayerID(this.m_Master);
					this.m_Renderer.sortingOrder = UtilsBeamProps.GetSortingOrder(this.m_Master);
				}
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001DFC8 File Offset: 0x0001C1C8
		private void HandleBackwardCompatibility(int serializedVersion, int newVersion)
		{
			if (serializedVersion == -1)
			{
				return;
			}
			if (serializedVersion == newVersion)
			{
				return;
			}
			if (serializedVersion < 1880)
			{
				if (this.direction == ParticlesDirection.Random)
				{
					this.direction = ParticlesDirection.LocalSpace;
				}
				else
				{
					this.direction = ParticlesDirection.Random;
				}
				this.velocity = new Vector3(0f, 0f, this.speed);
			}
			if (serializedVersion < 1940)
			{
				this.spawnDistanceRange = new MinMaxRangeFloat(this.spawnMinDistance, this.spawnMaxDistance);
			}
			Utils.MarkCurrentSceneDirty();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001E040 File Offset: 0x0001C240
		private void UpdateCulling()
		{
			if (this.m_Particles)
			{
				bool flag = true;
				bool fadeOutEnabled = UtilsBeamProps.GetFadeOutEnabled(this.m_Master);
				if ((this.cullingEnabled || fadeOutEnabled) && this.m_Master.hasGeometry)
				{
					if (this.mainCamera)
					{
						float num = this.cullingMaxDistance;
						if (fadeOutEnabled)
						{
							num = Mathf.Min(num, UtilsBeamProps.GetFadeOutEnd(this.m_Master));
						}
						float num2 = num * num;
						flag = (this.m_Master.bounds.SqrDistance(this.mainCamera.transform.position) <= num2);
					}
					else
					{
						this.cullingEnabled = false;
					}
				}
				if (this.m_Particles.gameObject.activeSelf != flag)
				{
					this.SetActive(flag);
					this.isCulled = !flag;
				}
				if (flag && !this.m_Particles.isPlaying)
				{
					this.m_Particles.Play();
				}
			}
		}

		// Token: 0x04000756 RID: 1878
		public const string ClassName = "VolumetricDustParticles";

		// Token: 0x04000757 RID: 1879
		[Range(0f, 1f)]
		public float alpha = 0.5f;

		// Token: 0x04000758 RID: 1880
		[Range(0.0001f, 0.1f)]
		public float size = 0.01f;

		// Token: 0x04000759 RID: 1881
		public ParticlesDirection direction;

		// Token: 0x0400075A RID: 1882
		public Vector3 velocity = Consts.DustParticles.VelocityDefault;

		// Token: 0x0400075B RID: 1883
		[Obsolete("Use 'velocity' instead")]
		public float speed = 0.03f;

		// Token: 0x0400075C RID: 1884
		public float density = 5f;

		// Token: 0x0400075D RID: 1885
		[MinMaxRange(0f, 1f)]
		public MinMaxRangeFloat spawnDistanceRange = Consts.DustParticles.SpawnDistanceRangeDefault;

		// Token: 0x0400075E RID: 1886
		[Obsolete("Use 'spawnDistanceRange' instead")]
		public float spawnMinDistance;

		// Token: 0x0400075F RID: 1887
		[Obsolete("Use 'spawnDistanceRange' instead")]
		public float spawnMaxDistance = 0.7f;

		// Token: 0x04000760 RID: 1888
		public bool cullingEnabled;

		// Token: 0x04000761 RID: 1889
		public float cullingMaxDistance = 10f;

		// Token: 0x04000763 RID: 1891
		[SerializeField]
		private float m_AlphaAdditionalRuntime = 1f;

		// Token: 0x04000764 RID: 1892
		private ParticleSystem m_Particles;

		// Token: 0x04000765 RID: 1893
		private ParticleSystemRenderer m_Renderer;

		// Token: 0x04000766 RID: 1894
		private Material m_Material;

		// Token: 0x04000767 RID: 1895
		private Gradient m_GradientCached = new Gradient();

		// Token: 0x04000768 RID: 1896
		private bool m_RuntimePropertiesDirty = true;

		// Token: 0x04000769 RID: 1897
		private static bool ms_NoMainCameraLogged;

		// Token: 0x0400076A RID: 1898
		private static Camera ms_MainCamera;

		// Token: 0x0400076B RID: 1899
		private VolumetricLightBeamAbstractBase m_Master;
	}
}
