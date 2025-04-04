using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200009F RID: 159
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0001276A File Offset: 0x0001096A
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x00012772 File Offset: 0x00010972
		public BloomModel.Settings settings
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

		// Token: 0x060002F3 RID: 755 RVA: 0x0001277B File Offset: 0x0001097B
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x04000390 RID: 912
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x020000A0 RID: 160
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060002F6 RID: 758 RVA: 0x000127A9 File Offset: 0x000109A9
			// (set) Token: 0x060002F5 RID: 757 RVA: 0x0001279B File Offset: 0x0001099B
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060002F7 RID: 759 RVA: 0x000127B8 File Offset: 0x000109B8
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x04000391 RID: 913
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x04000392 RID: 914
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x04000393 RID: 915
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x04000394 RID: 916
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x04000395 RID: 917
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x020000A1 RID: 161
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x17000051 RID: 81
			// (get) Token: 0x060002F8 RID: 760 RVA: 0x00012808 File Offset: 0x00010A08
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x04000396 RID: 918
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x04000397 RID: 919
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x020000A2 RID: 162
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000052 RID: 82
			// (get) Token: 0x060002F9 RID: 761 RVA: 0x00012834 File Offset: 0x00010A34
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000398 RID: 920
			public BloomModel.BloomSettings bloom;

			// Token: 0x04000399 RID: 921
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
