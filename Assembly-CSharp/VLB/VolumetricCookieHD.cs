using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000111 RID: 273
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VolumetricLightBeamHD))]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-cookie-hd/")]
	public class VolumetricCookieHD : MonoBehaviour
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00016E75 File Offset: 0x00015075
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x00016E7D File Offset: 0x0001507D
		public float contribution
		{
			get
			{
				return this.m_Contribution;
			}
			set
			{
				if (this.m_Contribution != value)
				{
					this.m_Contribution = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00016E95 File Offset: 0x00015095
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x00016E9D File Offset: 0x0001509D
		public Texture cookieTexture
		{
			get
			{
				return this.m_CookieTexture;
			}
			set
			{
				if (this.m_CookieTexture != value)
				{
					this.m_CookieTexture = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00016EBA File Offset: 0x000150BA
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00016EC2 File Offset: 0x000150C2
		public CookieChannel channel
		{
			get
			{
				return this.m_Channel;
			}
			set
			{
				if (this.m_Channel != value)
				{
					this.m_Channel = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00016EDA File Offset: 0x000150DA
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00016EE2 File Offset: 0x000150E2
		public bool negative
		{
			get
			{
				return this.m_Negative;
			}
			set
			{
				if (this.m_Negative != value)
				{
					this.m_Negative = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00016EFA File Offset: 0x000150FA
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x00016F02 File Offset: 0x00015102
		public Vector2 translation
		{
			get
			{
				return this.m_Translation;
			}
			set
			{
				if (this.m_Translation != value)
				{
					this.m_Translation = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x00016F1F File Offset: 0x0001511F
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x00016F27 File Offset: 0x00015127
		public float rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				if (this.m_Rotation != value)
				{
					this.m_Rotation = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00016F3F File Offset: 0x0001513F
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x00016F47 File Offset: 0x00015147
		public Vector2 scale
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				if (this.m_Scale != value)
				{
					this.m_Scale = value;
					this.SetDirty();
				}
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00016F64 File Offset: 0x00015164
		private void SetDirty()
		{
			if (this.m_Master)
			{
				this.m_Master.SetPropertyDirty(DirtyProps.CookieProps);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00016F84 File Offset: 0x00015184
		public static void ApplyMaterialProperties(VolumetricCookieHD instance, BeamGeometryHD geom)
		{
			if (instance && instance.enabled && instance.cookieTexture != null)
			{
				geom.SetMaterialProp(ShaderProperties.HD.CookieTexture, instance.cookieTexture);
				geom.SetMaterialProp(ShaderProperties.HD.CookieProperties, new Vector4(instance.negative ? instance.contribution : (-instance.contribution), (float)instance.channel, Mathf.Cos(instance.rotation * 0.017453292f), Mathf.Sin(instance.rotation * 0.017453292f)));
				geom.SetMaterialProp(ShaderProperties.HD.CookiePosAndScale, new Vector4(instance.translation.x, instance.translation.y, instance.scale.x, instance.scale.y));
				return;
			}
			geom.SetMaterialProp(ShaderProperties.HD.CookieTexture, BeamGeometryHD.InvalidTexture.Null);
			geom.SetMaterialProp(ShaderProperties.HD.CookieProperties, Vector4.zero);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00017072 File Offset: 0x00015272
		private void Awake()
		{
			this.m_Master = base.GetComponent<VolumetricLightBeamHD>();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00017080 File Offset: 0x00015280
		private void OnEnable()
		{
			this.SetDirty();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00017080 File Offset: 0x00015280
		private void OnDisable()
		{
			this.SetDirty();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00017080 File Offset: 0x00015280
		private void OnDidApplyAnimationProperties()
		{
			this.SetDirty();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00017088 File Offset: 0x00015288
		private void Start()
		{
			if (Application.isPlaying)
			{
				this.SetDirty();
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00017088 File Offset: 0x00015288
		private void OnDestroy()
		{
			if (Application.isPlaying)
			{
				this.SetDirty();
			}
		}

		// Token: 0x040005EA RID: 1514
		public const string ClassName = "VolumetricCookieHD";

		// Token: 0x040005EB RID: 1515
		[SerializeField]
		private float m_Contribution = 1f;

		// Token: 0x040005EC RID: 1516
		[SerializeField]
		private Texture m_CookieTexture;

		// Token: 0x040005ED RID: 1517
		[SerializeField]
		private CookieChannel m_Channel = CookieChannel.Alpha;

		// Token: 0x040005EE RID: 1518
		[SerializeField]
		private bool m_Negative;

		// Token: 0x040005EF RID: 1519
		[SerializeField]
		private Vector2 m_Translation = Consts.Cookie.TranslationDefault;

		// Token: 0x040005F0 RID: 1520
		[SerializeField]
		private float m_Rotation;

		// Token: 0x040005F1 RID: 1521
		[SerializeField]
		private Vector2 m_Scale = Consts.Cookie.ScaleDefault;

		// Token: 0x040005F2 RID: 1522
		private VolumetricLightBeamHD m_Master;
	}
}
