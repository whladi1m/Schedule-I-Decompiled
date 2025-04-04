using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000CA RID: 202
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000132C6 File Offset: 0x000114C6
		// (set) Token: 0x0600033D RID: 829 RVA: 0x000132CE File Offset: 0x000114CE
		public UserLutModel.Settings settings
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

		// Token: 0x0600033E RID: 830 RVA: 0x000132D7 File Offset: 0x000114D7
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x04000426 RID: 1062
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x020000CB RID: 203
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000074 RID: 116
			// (get) Token: 0x06000340 RID: 832 RVA: 0x000132F8 File Offset: 0x000114F8
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x04000427 RID: 1063
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x04000428 RID: 1064
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
