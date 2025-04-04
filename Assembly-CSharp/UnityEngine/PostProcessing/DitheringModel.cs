using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000B8 RID: 184
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00012F92 File Offset: 0x00011192
		// (set) Token: 0x0600031F RID: 799 RVA: 0x00012F9A File Offset: 0x0001119A
		public DitheringModel.Settings settings
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

		// Token: 0x06000320 RID: 800 RVA: 0x00012FA3 File Offset: 0x000111A3
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x040003F4 RID: 1012
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x020000B9 RID: 185
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000068 RID: 104
			// (get) Token: 0x06000322 RID: 802 RVA: 0x00012FC4 File Offset: 0x000111C4
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
