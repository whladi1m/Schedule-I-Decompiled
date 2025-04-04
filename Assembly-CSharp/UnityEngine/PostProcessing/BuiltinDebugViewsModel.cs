using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000A3 RID: 163
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00012862 File Offset: 0x00010A62
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0001286A File Offset: 0x00010A6A
		public BuiltinDebugViewsModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00012873 File Offset: 0x00010A73
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000128A6 File Offset: 0x00010AA6
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000128B3 File Offset: 0x00010AB3
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x0400039A RID: 922
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x020000A4 RID: 164
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000300 RID: 768 RVA: 0x000128D8 File Offset: 0x00010AD8
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x0400039B RID: 923
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x020000A5 RID: 165
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000301 RID: 769 RVA: 0x000128FC File Offset: 0x00010AFC
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x0400039C RID: 924
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x0400039D RID: 925
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x0400039E RID: 926
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x0400039F RID: 927
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x040003A0 RID: 928
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x040003A1 RID: 929
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x020000A6 RID: 166
		public enum Mode
		{
			// Token: 0x040003A3 RID: 931
			None,
			// Token: 0x040003A4 RID: 932
			Depth,
			// Token: 0x040003A5 RID: 933
			Normals,
			// Token: 0x040003A6 RID: 934
			MotionVectors,
			// Token: 0x040003A7 RID: 935
			AmbientOcclusion,
			// Token: 0x040003A8 RID: 936
			EyeAdaptation,
			// Token: 0x040003A9 RID: 937
			FocusPlane,
			// Token: 0x040003AA RID: 938
			PreGradingLog,
			// Token: 0x040003AB RID: 939
			LogLut,
			// Token: 0x040003AC RID: 940
			UserLut
		}

		// Token: 0x020000A7 RID: 167
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000057 RID: 87
			// (get) Token: 0x06000302 RID: 770 RVA: 0x00012958 File Offset: 0x00010B58
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x040003AD RID: 941
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x040003AE RID: 942
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x040003AF RID: 943
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
