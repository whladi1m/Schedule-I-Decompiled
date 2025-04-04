using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000094 RID: 148
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0001239C File Offset: 0x0001059C
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x000123A4 File Offset: 0x000105A4
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x060002E5 RID: 741 RVA: 0x000123AD File Offset: 0x000105AD
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x04000368 RID: 872
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x02000095 RID: 149
		public enum SampleCount
		{
			// Token: 0x0400036A RID: 874
			Lowest = 3,
			// Token: 0x0400036B RID: 875
			Low = 6,
			// Token: 0x0400036C RID: 876
			Medium = 10,
			// Token: 0x0400036D RID: 877
			High = 16
		}

		// Token: 0x02000096 RID: 150
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000049 RID: 73
			// (get) Token: 0x060002E7 RID: 743 RVA: 0x000123D0 File Offset: 0x000105D0
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x0400036E RID: 878
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x0400036F RID: 879
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x04000370 RID: 880
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x04000371 RID: 881
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x04000372 RID: 882
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x04000373 RID: 883
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x04000374 RID: 884
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}
