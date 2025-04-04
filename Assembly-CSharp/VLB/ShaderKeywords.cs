using System;

namespace VLB
{
	// Token: 0x02000147 RID: 327
	public static class ShaderKeywords
	{
		// Token: 0x040006FD RID: 1789
		public const string AlphaAsBlack = "VLB_ALPHA_AS_BLACK";

		// Token: 0x040006FE RID: 1790
		public const string ColorGradientMatrixLow = "VLB_COLOR_GRADIENT_MATRIX_LOW";

		// Token: 0x040006FF RID: 1791
		public const string ColorGradientMatrixHigh = "VLB_COLOR_GRADIENT_MATRIX_HIGH";

		// Token: 0x04000700 RID: 1792
		public const string Noise3D = "VLB_NOISE_3D";

		// Token: 0x02000148 RID: 328
		public static class SD
		{
			// Token: 0x04000701 RID: 1793
			public const string DepthBlend = "VLB_DEPTH_BLEND";

			// Token: 0x04000702 RID: 1794
			public const string OcclusionClippingPlane = "VLB_OCCLUSION_CLIPPING_PLANE";

			// Token: 0x04000703 RID: 1795
			public const string OcclusionDepthTexture = "VLB_OCCLUSION_DEPTH_TEXTURE";

			// Token: 0x04000704 RID: 1796
			public const string MeshSkewing = "VLB_MESH_SKEWING";

			// Token: 0x04000705 RID: 1797
			public const string ShaderAccuracyHigh = "VLB_SHADER_ACCURACY_HIGH";
		}

		// Token: 0x02000149 RID: 329
		public static class HD
		{
			// Token: 0x06000643 RID: 1603 RVA: 0x0001C60E File Offset: 0x0001A80E
			public static string GetRaymarchingQuality(int id)
			{
				return "VLB_RAYMARCHING_QUALITY_" + id.ToString();
			}

			// Token: 0x04000706 RID: 1798
			public const string AttenuationLinear = "VLB_ATTENUATION_LINEAR";

			// Token: 0x04000707 RID: 1799
			public const string AttenuationQuad = "VLB_ATTENUATION_QUAD";

			// Token: 0x04000708 RID: 1800
			public const string Shadow = "VLB_SHADOW";

			// Token: 0x04000709 RID: 1801
			public const string CookieSingleChannel = "VLB_COOKIE_1CHANNEL";

			// Token: 0x0400070A RID: 1802
			public const string CookieRGBA = "VLB_COOKIE_RGBA";

			// Token: 0x0400070B RID: 1803
			public const string RaymarchingStepCount = "VLB_RAYMARCHING_STEP_COUNT";
		}
	}
}
