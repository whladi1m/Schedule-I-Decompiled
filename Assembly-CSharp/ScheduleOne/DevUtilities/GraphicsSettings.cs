using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006F3 RID: 1779
	[Serializable]
	public class GraphicsSettings
	{
		// Token: 0x0400227A RID: 8826
		public GraphicsSettings.EGraphicsQuality GraphicsQuality;

		// Token: 0x0400227B RID: 8827
		public GraphicsSettings.EAntiAliasingMode AntiAliasingMode;

		// Token: 0x0400227C RID: 8828
		public float FOV;

		// Token: 0x0400227D RID: 8829
		public bool SSAO;

		// Token: 0x0400227E RID: 8830
		public bool GodRays;

		// Token: 0x020006F4 RID: 1780
		public enum EAntiAliasingMode
		{
			// Token: 0x04002280 RID: 8832
			Off,
			// Token: 0x04002281 RID: 8833
			FXAA,
			// Token: 0x04002282 RID: 8834
			SMAA
		}

		// Token: 0x020006F5 RID: 1781
		public enum EGraphicsQuality
		{
			// Token: 0x04002284 RID: 8836
			Low,
			// Token: 0x04002285 RID: 8837
			Medium,
			// Token: 0x04002286 RID: 8838
			High,
			// Token: 0x04002287 RID: 8839
			Ultra
		}
	}
}
