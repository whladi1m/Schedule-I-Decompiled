using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200007F RID: 127
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000FE37 File Offset: 0x0000E037
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000FE6A File Offset: 0x0000E06A
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000022C9 File Offset: 0x000004C9
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000FE71 File Offset: 0x0000E071
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterImageEffectsOpaque;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000FE78 File Offset: 0x0000E078
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			Color value = GraphicsUtils.isLinearColorSpace ? RenderSettings.fogColor.linear : RenderSettings.fogColor;
			material.SetColor(FogComponent.Uniforms._FogColor, value);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			switch (RenderSettings.fogMode)
			{
			case FogMode.Linear:
				material.EnableKeyword("FOG_LINEAR");
				break;
			case FogMode.Exponential:
				material.EnableKeyword("FOG_EXP");
				break;
			case FogMode.ExponentialSquared:
				material.EnableKeyword("FOG_EXP2");
				break;
			}
			RenderTextureFormat format = this.context.isHdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, FilterMode.Bilinear, format);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, settings.excludeSkybox ? 1 : 0);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x040002E5 RID: 741
		private const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x02000080 RID: 128
		private static class Uniforms
		{
			// Token: 0x040002E6 RID: 742
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x040002E7 RID: 743
			internal static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x040002E8 RID: 744
			internal static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x040002E9 RID: 745
			internal static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x040002EA RID: 746
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}
