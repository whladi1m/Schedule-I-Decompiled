using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000BD RID: 189
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00013097 File Offset: 0x00011297
		// (set) Token: 0x06000329 RID: 809 RVA: 0x0001309F File Offset: 0x0001129F
		public FogModel.Settings settings
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

		// Token: 0x0600032A RID: 810 RVA: 0x000130A8 File Offset: 0x000112A8
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x04000404 RID: 1028
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x020000BE RID: 190
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700006C RID: 108
			// (get) Token: 0x0600032C RID: 812 RVA: 0x000130C8 File Offset: 0x000112C8
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x04000405 RID: 1029
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
