using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000CC RID: 204
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00013322 File Offset: 0x00011522
		// (set) Token: 0x06000342 RID: 834 RVA: 0x0001332A File Offset: 0x0001152A
		public VignetteModel.Settings settings
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

		// Token: 0x06000343 RID: 835 RVA: 0x00013333 File Offset: 0x00011533
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x04000429 RID: 1065
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x020000CD RID: 205
		public enum Mode
		{
			// Token: 0x0400042B RID: 1067
			Classic,
			// Token: 0x0400042C RID: 1068
			Masked
		}

		// Token: 0x020000CE RID: 206
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000076 RID: 118
			// (get) Token: 0x06000345 RID: 837 RVA: 0x00013354 File Offset: 0x00011554
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x0400042D RID: 1069
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x0400042E RID: 1070
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x0400042F RID: 1071
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x04000430 RID: 1072
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x04000431 RID: 1073
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x04000432 RID: 1074
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x04000433 RID: 1075
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x04000434 RID: 1076
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x04000435 RID: 1077
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
