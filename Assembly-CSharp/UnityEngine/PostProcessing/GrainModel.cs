using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000BF RID: 191
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600032D RID: 813 RVA: 0x000130E6 File Offset: 0x000112E6
		// (set) Token: 0x0600032E RID: 814 RVA: 0x000130EE File Offset: 0x000112EE
		public GrainModel.Settings settings
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

		// Token: 0x0600032F RID: 815 RVA: 0x000130F7 File Offset: 0x000112F7
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x04000406 RID: 1030
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x020000C0 RID: 192
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700006E RID: 110
			// (get) Token: 0x06000331 RID: 817 RVA: 0x00013118 File Offset: 0x00011318
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x04000407 RID: 1031
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x04000408 RID: 1032
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x04000409 RID: 1033
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x0400040A RID: 1034
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
