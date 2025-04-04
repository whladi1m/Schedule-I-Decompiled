using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000A8 RID: 168
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0001298E File Offset: 0x00010B8E
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00012996 File Offset: 0x00010B96
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x06000305 RID: 773 RVA: 0x0001299F File Offset: 0x00010B9F
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x040003B0 RID: 944
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x020000A9 RID: 169
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000059 RID: 89
			// (get) Token: 0x06000307 RID: 775 RVA: 0x000129C0 File Offset: 0x00010BC0
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x040003B1 RID: 945
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x040003B2 RID: 946
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
