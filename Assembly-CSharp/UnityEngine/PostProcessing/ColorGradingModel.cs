using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000AA RID: 170
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000308 RID: 776 RVA: 0x000129EA File Offset: 0x00010BEA
		// (set) Token: 0x06000309 RID: 777 RVA: 0x000129F2 File Offset: 0x00010BF2
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00012A01 File Offset: 0x00010C01
		// (set) Token: 0x0600030B RID: 779 RVA: 0x00012A09 File Offset: 0x00010C09
		public bool isDirty { get; internal set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00012A12 File Offset: 0x00010C12
		// (set) Token: 0x0600030D RID: 781 RVA: 0x00012A1A File Offset: 0x00010C1A
		public RenderTexture bakedLut { get; internal set; }

		// Token: 0x0600030E RID: 782 RVA: 0x00012A23 File Offset: 0x00010C23
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00012A36 File Offset: 0x00010C36
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x040003B3 RID: 947
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x020000AB RID: 171
		public enum Tonemapper
		{
			// Token: 0x040003B7 RID: 951
			None,
			// Token: 0x040003B8 RID: 952
			ACES,
			// Token: 0x040003B9 RID: 953
			Neutral
		}

		// Token: 0x020000AC RID: 172
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000311 RID: 785 RVA: 0x00012A54 File Offset: 0x00010C54
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x040003BA RID: 954
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x040003BB RID: 955
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x040003BC RID: 956
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x040003BD RID: 957
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x040003BE RID: 958
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x040003BF RID: 959
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x040003C0 RID: 960
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x020000AD RID: 173
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x1700005E RID: 94
			// (get) Token: 0x06000312 RID: 786 RVA: 0x00012ABC File Offset: 0x00010CBC
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x040003C1 RID: 961
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x040003C2 RID: 962
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x040003C3 RID: 963
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x040003C4 RID: 964
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x040003C5 RID: 965
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x040003C6 RID: 966
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x020000AE RID: 174
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x1700005F RID: 95
			// (get) Token: 0x06000313 RID: 787 RVA: 0x00012B1C File Offset: 0x00010D1C
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x040003C7 RID: 967
			public Vector3 red;

			// Token: 0x040003C8 RID: 968
			public Vector3 green;

			// Token: 0x040003C9 RID: 969
			public Vector3 blue;

			// Token: 0x040003CA RID: 970
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x020000AF RID: 175
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x17000060 RID: 96
			// (get) Token: 0x06000314 RID: 788 RVA: 0x00012B8C File Offset: 0x00010D8C
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x040003CB RID: 971
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x040003CC RID: 972
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x040003CD RID: 973
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x020000B0 RID: 176
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x17000061 RID: 97
			// (get) Token: 0x06000315 RID: 789 RVA: 0x00012BC8 File Offset: 0x00010DC8
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x040003CE RID: 974
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x040003CF RID: 975
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x040003D0 RID: 976
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x020000B1 RID: 177
		public enum ColorWheelMode
		{
			// Token: 0x040003D2 RID: 978
			Linear,
			// Token: 0x040003D3 RID: 979
			Log
		}

		// Token: 0x020000B2 RID: 178
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x17000062 RID: 98
			// (get) Token: 0x06000316 RID: 790 RVA: 0x00012C04 File Offset: 0x00010E04
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x040003D4 RID: 980
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x040003D5 RID: 981
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x040003D6 RID: 982
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x020000B3 RID: 179
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x17000063 RID: 99
			// (get) Token: 0x06000317 RID: 791 RVA: 0x00012C3C File Offset: 0x00010E3C
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x040003D7 RID: 983
			public ColorGradingCurve master;

			// Token: 0x040003D8 RID: 984
			public ColorGradingCurve red;

			// Token: 0x040003D9 RID: 985
			public ColorGradingCurve green;

			// Token: 0x040003DA RID: 986
			public ColorGradingCurve blue;

			// Token: 0x040003DB RID: 987
			public ColorGradingCurve hueVShue;

			// Token: 0x040003DC RID: 988
			public ColorGradingCurve hueVSsat;

			// Token: 0x040003DD RID: 989
			public ColorGradingCurve satVSsat;

			// Token: 0x040003DE RID: 990
			public ColorGradingCurve lumVSsat;

			// Token: 0x040003DF RID: 991
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x040003E0 RID: 992
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x040003E1 RID: 993
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x040003E2 RID: 994
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x040003E3 RID: 995
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x020000B4 RID: 180
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000064 RID: 100
			// (get) Token: 0x06000318 RID: 792 RVA: 0x00012EC4 File Offset: 0x000110C4
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x040003E4 RID: 996
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x040003E5 RID: 997
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x040003E6 RID: 998
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x040003E7 RID: 999
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x040003E8 RID: 1000
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
