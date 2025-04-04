using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000C1 RID: 193
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0001315A File Offset: 0x0001135A
		// (set) Token: 0x06000333 RID: 819 RVA: 0x00013162 File Offset: 0x00011362
		public MotionBlurModel.Settings settings
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

		// Token: 0x06000334 RID: 820 RVA: 0x0001316B File Offset: 0x0001136B
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x0400040B RID: 1035
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x020000C2 RID: 194
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000070 RID: 112
			// (get) Token: 0x06000336 RID: 822 RVA: 0x0001318C File Offset: 0x0001138C
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x0400040C RID: 1036
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x0400040D RID: 1037
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x0400040E RID: 1038
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
