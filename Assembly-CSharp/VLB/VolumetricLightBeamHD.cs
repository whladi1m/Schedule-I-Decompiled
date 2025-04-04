using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000112 RID: 274
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[SelectionBase]
	[HelpURL("http://saladgamer.com/vlb-doc/comp-lightbeam-hd/")]
	public class VolumetricLightBeamHD : VolumetricLightBeamAbstractBase
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x000170C7 File Offset: 0x000152C7
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x000170CF File Offset: 0x000152CF
		public bool colorFromLight
		{
			get
			{
				return this.m_ColorFromLight;
			}
			set
			{
				if (this.m_ColorFromLight != value)
				{
					this.m_ColorFromLight = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x000170E7 File Offset: 0x000152E7
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x000170FD File Offset: 0x000152FD
		public ColorMode colorMode
		{
			get
			{
				if (Config.Instance.featureEnabledColorGradient == FeatureEnabledColorGradient.Off)
				{
					return ColorMode.Flat;
				}
				return this.m_ColorMode;
			}
			set
			{
				if (this.m_ColorMode != value)
				{
					this.m_ColorMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.ColorMode);
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0001711C File Offset: 0x0001531C
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00017124 File Offset: 0x00015324
		public Color colorFlat
		{
			get
			{
				return this.m_ColorFlat;
			}
			set
			{
				if (this.m_ColorFlat != value)
				{
					this.m_ColorFlat = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Color);
				}
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x00017149 File Offset: 0x00015349
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x00017151 File Offset: 0x00015351
		public Gradient colorGradient
		{
			get
			{
				return this.m_ColorGradient;
			}
			set
			{
				if (this.m_ColorGradient != value)
				{
					this.m_ColorGradient = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Color);
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x00017171 File Offset: 0x00015371
		private bool useColorFromAttachedLightSpot
		{
			get
			{
				return this.colorFromLight && base.lightSpotAttached != null;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00017189 File Offset: 0x00015389
		private bool useColorTemperatureFromAttachedLightSpot
		{
			get
			{
				return this.useColorFromAttachedLightSpot && base.lightSpotAttached.useColorTemperature && Config.Instance.useLightColorTemperature;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x000171AC File Offset: 0x000153AC
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x000171B4 File Offset: 0x000153B4
		public float intensity
		{
			get
			{
				return this.m_Intensity;
			}
			set
			{
				if (this.m_Intensity != value)
				{
					this.m_Intensity = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Intensity);
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x000171D3 File Offset: 0x000153D3
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x000171DB File Offset: 0x000153DB
		public float intensityMultiplier
		{
			get
			{
				return this.m_IntensityMultiplier;
			}
			set
			{
				if (this.m_IntensityMultiplier != value)
				{
					this.m_IntensityMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x000171F3 File Offset: 0x000153F3
		public bool useIntensityFromAttachedLightSpot
		{
			get
			{
				return this.intensityMultiplier >= 0f && base.lightSpotAttached != null;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00017210 File Offset: 0x00015410
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x00017218 File Offset: 0x00015418
		public float hdrpExposureWeight
		{
			get
			{
				return this.m_HDRPExposureWeight;
			}
			set
			{
				if (this.m_HDRPExposureWeight != value)
				{
					this.m_HDRPExposureWeight = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.HDRPExposureWeight);
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00017237 File Offset: 0x00015437
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0001723F File Offset: 0x0001543F
		public BlendingMode blendingMode
		{
			get
			{
				return this.m_BlendingMode;
			}
			set
			{
				if (this.m_BlendingMode != value)
				{
					this.m_BlendingMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.BlendingMode);
				}
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0001725F File Offset: 0x0001545F
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x00017267 File Offset: 0x00015467
		public float spotAngle
		{
			get
			{
				return this.m_SpotAngle;
			}
			set
			{
				if (this.m_SpotAngle != value)
				{
					this.m_SpotAngle = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x00017287 File Offset: 0x00015487
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x0001728F File Offset: 0x0001548F
		public float spotAngleMultiplier
		{
			get
			{
				return this.m_SpotAngleMultiplier;
			}
			set
			{
				if (this.m_SpotAngleMultiplier != value)
				{
					this.m_SpotAngleMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x000172A7 File Offset: 0x000154A7
		public bool useSpotAngleFromAttachedLightSpot
		{
			get
			{
				return this.spotAngleMultiplier >= 0f && base.lightSpotAttached != null;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x000172C4 File Offset: 0x000154C4
		public float coneAngle
		{
			get
			{
				return Mathf.Atan2(this.coneRadiusEnd - this.coneRadiusStart, this.maxGeometryDistance) * 57.29578f * 2f;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x000172EA File Offset: 0x000154EA
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x000172F2 File Offset: 0x000154F2
		public float coneRadiusStart
		{
			get
			{
				return this.m_ConeRadiusStart;
			}
			set
			{
				if (this.m_ConeRadiusStart != value)
				{
					this.m_ConeRadiusStart = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00017312 File Offset: 0x00015512
		// (set) Token: 0x0600046C RID: 1132 RVA: 0x00017325 File Offset: 0x00015525
		public float coneRadiusEnd
		{
			get
			{
				return Utils.ComputeConeRadiusEnd(this.maxGeometryDistance, this.spotAngle);
			}
			set
			{
				this.spotAngle = Utils.ComputeSpotAngle(this.maxGeometryDistance, value);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0001733C File Offset: 0x0001553C
		public float coneVolume
		{
			get
			{
				float coneRadiusStart = this.coneRadiusStart;
				float coneRadiusEnd = this.coneRadiusEnd;
				return 1.0471976f * (coneRadiusStart * coneRadiusStart + coneRadiusStart * coneRadiusEnd + coneRadiusEnd * coneRadiusEnd) * this.fallOffEnd;
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00017370 File Offset: 0x00015570
		public float GetConeApexOffsetZ(bool counterApplyScaleForUnscalableBeam)
		{
			float num = this.coneRadiusStart / this.coneRadiusEnd;
			if (num == 1f)
			{
				return float.MaxValue;
			}
			float num2 = this.maxGeometryDistance * num / (1f - num);
			if (counterApplyScaleForUnscalableBeam && !this.scalable)
			{
				num2 /= this.GetLossyScale().z;
			}
			return num2;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x000173C4 File Offset: 0x000155C4
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x000173CC File Offset: 0x000155CC
		public bool scalable
		{
			get
			{
				return this.m_Scalable;
			}
			set
			{
				if (this.m_Scalable != value)
				{
					this.m_Scalable = value;
					this.SetPropertyDirty(DirtyProps.Attenuation);
				}
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000173E9 File Offset: 0x000155E9
		public override bool IsScalable()
		{
			return this.scalable;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x000173F1 File Offset: 0x000155F1
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x000173F9 File Offset: 0x000155F9
		public AttenuationEquationHD attenuationEquation
		{
			get
			{
				return this.m_AttenuationEquation;
			}
			set
			{
				if (this.m_AttenuationEquation != value)
				{
					this.m_AttenuationEquation = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Attenuation);
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0001741C File Offset: 0x0001561C
		// (set) Token: 0x06000475 RID: 1141 RVA: 0x00017424 File Offset: 0x00015624
		public float fallOffStart
		{
			get
			{
				return this.m_FallOffStart;
			}
			set
			{
				if (this.m_FallOffStart != value)
				{
					this.m_FallOffStart = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00017444 File Offset: 0x00015644
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x0001744C File Offset: 0x0001564C
		public float fallOffEnd
		{
			get
			{
				return this.m_FallOffEnd;
			}
			set
			{
				if (this.m_FallOffEnd != value)
				{
					this.m_FallOffEnd = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Cone);
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001746C File Offset: 0x0001566C
		public float maxGeometryDistance
		{
			get
			{
				return this.fallOffEnd;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x00017474 File Offset: 0x00015674
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x0001747C File Offset: 0x0001567C
		public float fallOffEndMultiplier
		{
			get
			{
				return this.m_FallOffEndMultiplier;
			}
			set
			{
				if (this.m_FallOffEndMultiplier != value)
				{
					this.m_FallOffEndMultiplier = value;
					this.ValidateProperties();
				}
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00017494 File Offset: 0x00015694
		public bool useFallOffEndFromAttachedLightSpot
		{
			get
			{
				return this.fallOffEndMultiplier >= 0f && base.lightSpotAttached != null;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x000174B1 File Offset: 0x000156B1
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x000174B9 File Offset: 0x000156B9
		public float sideSoftness
		{
			get
			{
				return this.m_SideSoftness;
			}
			set
			{
				if (this.m_SideSoftness != value)
				{
					this.m_SideSoftness = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.SideSoftness);
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x000174DC File Offset: 0x000156DC
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x000174E4 File Offset: 0x000156E4
		public float jitteringFactor
		{
			get
			{
				return this.m_JitteringFactor;
			}
			set
			{
				if (this.m_JitteringFactor != value)
				{
					this.m_JitteringFactor = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00017507 File Offset: 0x00015707
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x0001750F File Offset: 0x0001570F
		public int jitteringFrameRate
		{
			get
			{
				return this.m_JitteringFrameRate;
			}
			set
			{
				if (this.m_JitteringFrameRate != value)
				{
					this.m_JitteringFrameRate = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00017532 File Offset: 0x00015732
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x0001753A File Offset: 0x0001573A
		public MinMaxRangeFloat jitteringLerpRange
		{
			get
			{
				return this.m_JitteringLerpRange;
			}
			set
			{
				if (this.m_JitteringLerpRange != value)
				{
					this.m_JitteringLerpRange = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.Jittering);
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00017562 File Offset: 0x00015762
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x0001756A File Offset: 0x0001576A
		public NoiseMode noiseMode
		{
			get
			{
				return this.m_NoiseMode;
			}
			set
			{
				if (this.m_NoiseMode != value)
				{
					this.m_NoiseMode = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseMode);
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0001758D File Offset: 0x0001578D
		public bool isNoiseEnabled
		{
			get
			{
				return this.noiseMode > NoiseMode.Disabled;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00017598 File Offset: 0x00015798
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x000175A0 File Offset: 0x000157A0
		public float noiseIntensity
		{
			get
			{
				return this.m_NoiseIntensity;
			}
			set
			{
				if (this.m_NoiseIntensity != value)
				{
					this.m_NoiseIntensity = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseIntensity);
				}
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x000175C3 File Offset: 0x000157C3
		// (set) Token: 0x0600048A RID: 1162 RVA: 0x000175CB File Offset: 0x000157CB
		public bool noiseScaleUseGlobal
		{
			get
			{
				return this.m_NoiseScaleUseGlobal;
			}
			set
			{
				if (this.m_NoiseScaleUseGlobal != value)
				{
					this.m_NoiseScaleUseGlobal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x000175EE File Offset: 0x000157EE
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x000175F6 File Offset: 0x000157F6
		public float noiseScaleLocal
		{
			get
			{
				return this.m_NoiseScaleLocal;
			}
			set
			{
				if (this.m_NoiseScaleLocal != value)
				{
					this.m_NoiseScaleLocal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00017619 File Offset: 0x00015819
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x00017621 File Offset: 0x00015821
		public bool noiseVelocityUseGlobal
		{
			get
			{
				return this.m_NoiseVelocityUseGlobal;
			}
			set
			{
				if (this.m_NoiseVelocityUseGlobal != value)
				{
					this.m_NoiseVelocityUseGlobal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00017644 File Offset: 0x00015844
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x0001764C File Offset: 0x0001584C
		public Vector3 noiseVelocityLocal
		{
			get
			{
				return this.m_NoiseVelocityLocal;
			}
			set
			{
				if (this.m_NoiseVelocityLocal != value)
				{
					this.m_NoiseVelocityLocal = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.NoiseVelocityAndScale);
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00017674 File Offset: 0x00015874
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x0001767C File Offset: 0x0001587C
		public int raymarchingQualityID
		{
			get
			{
				return this.m_RaymarchingQualityID;
			}
			set
			{
				if (this.m_RaymarchingQualityID != value)
				{
					this.m_RaymarchingQualityID = value;
					this.ValidateProperties();
					this.SetPropertyDirty(DirtyProps.RaymarchingQuality);
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001769F File Offset: 0x0001589F
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x000176B1 File Offset: 0x000158B1
		public int raymarchingQualityIndex
		{
			get
			{
				return Config.Instance.GetRaymarchingQualityIndexForUniqueID(this.raymarchingQualityID);
			}
			set
			{
				this.raymarchingQualityID = Config.Instance.GetRaymarchingQualityForIndex(this.raymarchingQualityIndex).uniqueID;
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000176CE File Offset: 0x000158CE
		public override BeamGeometryAbstractBase GetBeamGeometry()
		{
			return this.m_BeamGeom;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000176D6 File Offset: 0x000158D6
		protected override void SetBeamGeometryNull()
		{
			this.m_BeamGeom = null;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x000176DF File Offset: 0x000158DF
		public int blendingModeAsInt
		{
			get
			{
				return Mathf.Clamp((int)this.blendingMode, 0, Enum.GetValues(typeof(BlendingMode)).Length);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00017701 File Offset: 0x00015901
		public Quaternion beamInternalLocalRotation
		{
			get
			{
				if (this.GetDimensions() != Dimensions.Dim3D)
				{
					return Quaternion.LookRotation(Vector3.right, Vector3.up);
				}
				return Quaternion.identity;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00017720 File Offset: 0x00015920
		public Vector3 beamLocalForward
		{
			get
			{
				if (this.GetDimensions() != Dimensions.Dim3D)
				{
					return Vector3.right;
				}
				return Vector3.forward;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x00017735 File Offset: 0x00015935
		public Vector3 beamGlobalForward
		{
			get
			{
				return base.transform.TransformDirection(this.beamLocalForward);
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00017748 File Offset: 0x00015948
		public override Vector3 GetLossyScale()
		{
			if (this.GetDimensions() != Dimensions.Dim3D)
			{
				return new Vector3(base.transform.lossyScale.z, base.transform.lossyScale.y, base.transform.lossyScale.x);
			}
			return base.transform.lossyScale;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001779E File Offset: 0x0001599E
		public VolumetricCookieHD GetAdditionalComponentCookie()
		{
			return base.GetComponent<VolumetricCookieHD>();
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x000177A6 File Offset: 0x000159A6
		public VolumetricShadowHD GetAdditionalComponentShadow()
		{
			return base.GetComponent<VolumetricShadowHD>();
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000177AE File Offset: 0x000159AE
		public void SetPropertyDirty(DirtyProps flags)
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.SetPropertyDirty(flags);
			}
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00014002 File Offset: 0x00012202
		public virtual Dimensions GetDimensions()
		{
			return Dimensions.Dim3D;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00014002 File Offset: 0x00012202
		public virtual bool DoesSupportSorting2D()
		{
			return false;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00014002 File Offset: 0x00012202
		public virtual int GetSortingLayerID()
		{
			return 0;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00014002 File Offset: 0x00012202
		public virtual int GetSortingOrder()
		{
			return 0;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x000177C9 File Offset: 0x000159C9
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x000177D1 File Offset: 0x000159D1
		public uint _INTERNAL_InstancedMaterialGroupID { get; protected set; }

		// Token: 0x060004A5 RID: 1189 RVA: 0x000177DA File Offset: 0x000159DA
		public float GetInsideBeamFactor(Vector3 posWS)
		{
			return this.GetInsideBeamFactorFromObjectSpacePos(base.transform.InverseTransformPoint(posWS));
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000177F0 File Offset: 0x000159F0
		public float GetInsideBeamFactorFromObjectSpacePos(Vector3 posOS)
		{
			if (this.GetDimensions() == Dimensions.Dim2D)
			{
				posOS = new Vector3(posOS.z, posOS.y, posOS.x);
			}
			if (posOS.z < 0f)
			{
				return -1f;
			}
			Vector2 normalized = new Vector2(posOS.xy().magnitude, posOS.z + this.GetConeApexOffsetZ(true)).normalized;
			return Mathf.Clamp((Mathf.Abs(Mathf.Sin(this.coneAngle * 0.017453292f / 2f)) - Mathf.Abs(normalized.x)) / 0.1f, -1f, 1f);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001789C File Offset: 0x00015A9C
		public virtual void GenerateGeometry()
		{
			if (this.pluginVersion == -1)
			{
				this.raymarchingQualityID = Config.Instance.defaultRaymarchingQualityUniqueID;
			}
			if (!Config.Instance.IsRaymarchingQualityUniqueIDValid(this.raymarchingQualityID))
			{
				Debug.LogErrorFormat(base.gameObject, "HD Beam '{0}': fallback to default quality '{1}'", new object[]
				{
					base.name,
					Config.Instance.GetRaymarchingQualityForUniqueID(Config.Instance.defaultRaymarchingQualityUniqueID).name
				});
				this.raymarchingQualityID = Config.Instance.defaultRaymarchingQualityUniqueID;
				Utils.MarkCurrentSceneDirty();
			}
			this.HandleBackwardCompatibility(this.pluginVersion, 20100);
			this.pluginVersion = 20100;
			this.ValidateProperties();
			if (this.m_BeamGeom == null)
			{
				this.m_BeamGeom = Utils.NewWithComponent<BeamGeometryHD>("Beam Geometry");
				this.m_BeamGeom.Initialize(this);
			}
			this.m_BeamGeom.RegenerateMesh();
			this.m_BeamGeom.visible = base.enabled;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001798C File Offset: 0x00015B8C
		public virtual void UpdateAfterManualPropertyChange()
		{
			this.ValidateProperties();
			this.SetPropertyDirty(DirtyProps.All);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001799F File Offset: 0x00015B9F
		private void Start()
		{
			base.InitLightSpotAttachedCached();
			this.GenerateGeometry();
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000179AD File Offset: 0x00015BAD
		private void OnEnable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.visible = true;
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000179C8 File Offset: 0x00015BC8
		private void OnDisable()
		{
			if (this.m_BeamGeom)
			{
				this.m_BeamGeom.visible = false;
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x000179E3 File Offset: 0x00015BE3
		private void OnDidApplyAnimationProperties()
		{
			this.AssignPropertiesFromAttachedSpotLight();
			this.UpdateAfterManualPropertyChange();
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000179F4 File Offset: 0x00015BF4
		public void AssignPropertiesFromAttachedSpotLight()
		{
			Light lightSpotAttached = base.lightSpotAttached;
			if (lightSpotAttached)
			{
				if (this.useIntensityFromAttachedLightSpot)
				{
					this.intensity = SpotLightHelper.GetIntensity(lightSpotAttached) * this.intensityMultiplier;
				}
				if (this.useFallOffEndFromAttachedLightSpot)
				{
					this.fallOffEnd = SpotLightHelper.GetFallOffEnd(lightSpotAttached) * this.fallOffEndMultiplier;
				}
				if (this.useSpotAngleFromAttachedLightSpot)
				{
					this.spotAngle = Mathf.Clamp(SpotLightHelper.GetSpotAngle(lightSpotAttached) * this.spotAngleMultiplier, 0.1f, 179.9f);
				}
				if (this.m_ColorFromLight)
				{
					this.colorMode = ColorMode.Flat;
					if (this.useColorTemperatureFromAttachedLightSpot)
					{
						Color b = Mathf.CorrelatedColorTemperatureToRGB(lightSpotAttached.colorTemperature);
						this.colorFlat = (lightSpotAttached.color.linear * b).gamma;
						return;
					}
					this.colorFlat = lightSpotAttached.color;
				}
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00017AC8 File Offset: 0x00015CC8
		private void ClampProperties()
		{
			this.m_Intensity = Mathf.Max(this.m_Intensity, 0f);
			this.m_FallOffEnd = Mathf.Max(0.01f, this.m_FallOffEnd);
			this.m_FallOffStart = Mathf.Clamp(this.m_FallOffStart, 0f, this.m_FallOffEnd - 0.01f);
			this.m_SpotAngle = Mathf.Clamp(this.m_SpotAngle, 0.1f, 179.9f);
			this.m_ConeRadiusStart = Mathf.Max(this.m_ConeRadiusStart, 0f);
			this.m_SideSoftness = Mathf.Clamp(this.m_SideSoftness, 0.0001f, 10f);
			this.m_JitteringFactor = Mathf.Max(this.m_JitteringFactor, 0f);
			this.m_JitteringFrameRate = Mathf.Clamp(this.m_JitteringFrameRate, 0, 120);
			this.m_NoiseIntensity = Mathf.Clamp(this.m_NoiseIntensity, 0f, 1f);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00017BB4 File Offset: 0x00015DB4
		private void ValidateProperties()
		{
			this.AssignPropertiesFromAttachedSpotLight();
			this.ClampProperties();
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00017BC2 File Offset: 0x00015DC2
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
			Utils.MarkCurrentSceneDirty();
		}

		// Token: 0x040005F3 RID: 1523
		public new const string ClassName = "VolumetricLightBeamHD";

		// Token: 0x040005F4 RID: 1524
		[SerializeField]
		private bool m_ColorFromLight = true;

		// Token: 0x040005F5 RID: 1525
		[SerializeField]
		private ColorMode m_ColorMode;

		// Token: 0x040005F6 RID: 1526
		[SerializeField]
		private Color m_ColorFlat = Consts.Beam.FlatColor;

		// Token: 0x040005F7 RID: 1527
		[SerializeField]
		private Gradient m_ColorGradient;

		// Token: 0x040005F8 RID: 1528
		[SerializeField]
		private BlendingMode m_BlendingMode;

		// Token: 0x040005F9 RID: 1529
		[SerializeField]
		private float m_Intensity = 1f;

		// Token: 0x040005FA RID: 1530
		[SerializeField]
		private float m_IntensityMultiplier = 1f;

		// Token: 0x040005FB RID: 1531
		[SerializeField]
		private float m_HDRPExposureWeight;

		// Token: 0x040005FC RID: 1532
		[SerializeField]
		private float m_SpotAngle = 35f;

		// Token: 0x040005FD RID: 1533
		[SerializeField]
		private float m_SpotAngleMultiplier = 1f;

		// Token: 0x040005FE RID: 1534
		[SerializeField]
		private float m_ConeRadiusStart = 0.1f;

		// Token: 0x040005FF RID: 1535
		[SerializeField]
		private bool m_Scalable = true;

		// Token: 0x04000600 RID: 1536
		[SerializeField]
		private float m_FallOffStart;

		// Token: 0x04000601 RID: 1537
		[SerializeField]
		private float m_FallOffEnd = 3f;

		// Token: 0x04000602 RID: 1538
		[SerializeField]
		private float m_FallOffEndMultiplier = 1f;

		// Token: 0x04000603 RID: 1539
		[SerializeField]
		private AttenuationEquationHD m_AttenuationEquation = AttenuationEquationHD.Quadratic;

		// Token: 0x04000604 RID: 1540
		[SerializeField]
		private float m_SideSoftness = 1f;

		// Token: 0x04000605 RID: 1541
		[SerializeField]
		private int m_RaymarchingQualityID = -1;

		// Token: 0x04000606 RID: 1542
		[SerializeField]
		private float m_JitteringFactor;

		// Token: 0x04000607 RID: 1543
		[SerializeField]
		private int m_JitteringFrameRate = 60;

		// Token: 0x04000608 RID: 1544
		[MinMaxRange(0f, 1f)]
		[SerializeField]
		private MinMaxRangeFloat m_JitteringLerpRange = Consts.Beam.HD.JitteringLerpRange;

		// Token: 0x04000609 RID: 1545
		[SerializeField]
		private NoiseMode m_NoiseMode;

		// Token: 0x0400060A RID: 1546
		[SerializeField]
		private float m_NoiseIntensity = 0.5f;

		// Token: 0x0400060B RID: 1547
		[SerializeField]
		private bool m_NoiseScaleUseGlobal = true;

		// Token: 0x0400060C RID: 1548
		[SerializeField]
		private float m_NoiseScaleLocal = 0.5f;

		// Token: 0x0400060D RID: 1549
		[SerializeField]
		private bool m_NoiseVelocityUseGlobal = true;

		// Token: 0x0400060E RID: 1550
		[SerializeField]
		private Vector3 m_NoiseVelocityLocal = Consts.Beam.NoiseVelocityDefault;

		// Token: 0x04000610 RID: 1552
		protected BeamGeometryHD m_BeamGeom;
	}
}
