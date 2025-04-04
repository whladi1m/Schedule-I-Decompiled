using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200014A RID: 330
	public static class ShaderProperties
	{
		// Token: 0x0400070C RID: 1804
		public static readonly int ConeRadius = Shader.PropertyToID("_ConeRadius");

		// Token: 0x0400070D RID: 1805
		public static readonly int ConeGeomProps = Shader.PropertyToID("_ConeGeomProps");

		// Token: 0x0400070E RID: 1806
		public static readonly int ColorFlat = Shader.PropertyToID("_ColorFlat");

		// Token: 0x0400070F RID: 1807
		public static readonly int DistanceFallOff = Shader.PropertyToID("_DistanceFallOff");

		// Token: 0x04000710 RID: 1808
		public static readonly int NoiseVelocityAndScale = Shader.PropertyToID("_NoiseVelocityAndScale");

		// Token: 0x04000711 RID: 1809
		public static readonly int NoiseParam = Shader.PropertyToID("_NoiseParam");

		// Token: 0x04000712 RID: 1810
		public static readonly int ColorGradientMatrix = Shader.PropertyToID("_ColorGradientMatrix");

		// Token: 0x04000713 RID: 1811
		public static readonly int LocalToWorldMatrix = Shader.PropertyToID("_LocalToWorldMatrix");

		// Token: 0x04000714 RID: 1812
		public static readonly int WorldToLocalMatrix = Shader.PropertyToID("_WorldToLocalMatrix");

		// Token: 0x04000715 RID: 1813
		public static readonly int BlendSrcFactor = Shader.PropertyToID("_BlendSrcFactor");

		// Token: 0x04000716 RID: 1814
		public static readonly int BlendDstFactor = Shader.PropertyToID("_BlendDstFactor");

		// Token: 0x04000717 RID: 1815
		public static readonly int ZTest = Shader.PropertyToID("_ZTest");

		// Token: 0x04000718 RID: 1816
		public static readonly int ParticlesTintColor = Shader.PropertyToID("_TintColor");

		// Token: 0x04000719 RID: 1817
		public static readonly int HDRPExposureWeight = Shader.PropertyToID("_HDRPExposureWeight");

		// Token: 0x0400071A RID: 1818
		public static readonly int GlobalUsesReversedZBuffer = Shader.PropertyToID("_VLB_UsesReversedZBuffer");

		// Token: 0x0400071B RID: 1819
		public static readonly int GlobalNoiseTex3D = Shader.PropertyToID("_VLB_NoiseTex3D");

		// Token: 0x0400071C RID: 1820
		public static readonly int GlobalNoiseCustomTime = Shader.PropertyToID("_VLB_NoiseCustomTime");

		// Token: 0x0400071D RID: 1821
		public static readonly int GlobalDitheringFactor = Shader.PropertyToID("_VLB_DitheringFactor");

		// Token: 0x0400071E RID: 1822
		public static readonly int GlobalDitheringNoiseTex = Shader.PropertyToID("_VLB_DitheringNoiseTex");

		// Token: 0x0200014B RID: 331
		public static class SD
		{
			// Token: 0x0400071F RID: 1823
			public static readonly int FadeOutFactor = Shader.PropertyToID("_FadeOutFactor");

			// Token: 0x04000720 RID: 1824
			public static readonly int ConeSlopeCosSin = Shader.PropertyToID("_ConeSlopeCosSin");

			// Token: 0x04000721 RID: 1825
			public static readonly int AlphaInside = Shader.PropertyToID("_AlphaInside");

			// Token: 0x04000722 RID: 1826
			public static readonly int AlphaOutside = Shader.PropertyToID("_AlphaOutside");

			// Token: 0x04000723 RID: 1827
			public static readonly int AttenuationLerpLinearQuad = Shader.PropertyToID("_AttenuationLerpLinearQuad");

			// Token: 0x04000724 RID: 1828
			public static readonly int DistanceCamClipping = Shader.PropertyToID("_DistanceCamClipping");

			// Token: 0x04000725 RID: 1829
			public static readonly int FresnelPow = Shader.PropertyToID("_FresnelPow");

			// Token: 0x04000726 RID: 1830
			public static readonly int GlareBehind = Shader.PropertyToID("_GlareBehind");

			// Token: 0x04000727 RID: 1831
			public static readonly int GlareFrontal = Shader.PropertyToID("_GlareFrontal");

			// Token: 0x04000728 RID: 1832
			public static readonly int DrawCap = Shader.PropertyToID("_DrawCap");

			// Token: 0x04000729 RID: 1833
			public static readonly int DepthBlendDistance = Shader.PropertyToID("_DepthBlendDistance");

			// Token: 0x0400072A RID: 1834
			public static readonly int CameraParams = Shader.PropertyToID("_CameraParams");

			// Token: 0x0400072B RID: 1835
			public static readonly int DynamicOcclusionClippingPlaneWS = Shader.PropertyToID("_DynamicOcclusionClippingPlaneWS");

			// Token: 0x0400072C RID: 1836
			public static readonly int DynamicOcclusionClippingPlaneProps = Shader.PropertyToID("_DynamicOcclusionClippingPlaneProps");

			// Token: 0x0400072D RID: 1837
			public static readonly int DynamicOcclusionDepthTexture = Shader.PropertyToID("_DynamicOcclusionDepthTexture");

			// Token: 0x0400072E RID: 1838
			public static readonly int DynamicOcclusionDepthProps = Shader.PropertyToID("_DynamicOcclusionDepthProps");

			// Token: 0x0400072F RID: 1839
			public static readonly int LocalForwardDirection = Shader.PropertyToID("_LocalForwardDirection");

			// Token: 0x04000730 RID: 1840
			public static readonly int TiltVector = Shader.PropertyToID("_TiltVector");

			// Token: 0x04000731 RID: 1841
			public static readonly int AdditionalClippingPlaneWS = Shader.PropertyToID("_AdditionalClippingPlaneWS");
		}

		// Token: 0x0200014C RID: 332
		public static class HD
		{
			// Token: 0x04000732 RID: 1842
			public static readonly int Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x04000733 RID: 1843
			public static readonly int SideSoftness = Shader.PropertyToID("_SideSoftness");

			// Token: 0x04000734 RID: 1844
			public static readonly int CameraForwardOS = Shader.PropertyToID("_CameraForwardOS");

			// Token: 0x04000735 RID: 1845
			public static readonly int CameraForwardWS = Shader.PropertyToID("_CameraForwardWS");

			// Token: 0x04000736 RID: 1846
			public static readonly int TransformScale = Shader.PropertyToID("_TransformScale");

			// Token: 0x04000737 RID: 1847
			public static readonly int ShadowDepthTexture = Shader.PropertyToID("_ShadowDepthTexture");

			// Token: 0x04000738 RID: 1848
			public static readonly int ShadowProps = Shader.PropertyToID("_ShadowProps");

			// Token: 0x04000739 RID: 1849
			public static readonly int Jittering = Shader.PropertyToID("_Jittering");

			// Token: 0x0400073A RID: 1850
			public static readonly int CookieTexture = Shader.PropertyToID("_CookieTexture");

			// Token: 0x0400073B RID: 1851
			public static readonly int CookieProperties = Shader.PropertyToID("_CookieProperties");

			// Token: 0x0400073C RID: 1852
			public static readonly int CookiePosAndScale = Shader.PropertyToID("_CookiePosAndScale");

			// Token: 0x0400073D RID: 1853
			public static readonly int GlobalCameraBlendingDistance = Shader.PropertyToID("_VLB_CameraBlendingDistance");

			// Token: 0x0400073E RID: 1854
			public static readonly int GlobalJitteringNoiseTex = Shader.PropertyToID("_VLB_JitteringNoiseTex");
		}
	}
}
