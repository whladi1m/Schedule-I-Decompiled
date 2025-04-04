using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace VLB
{
	// Token: 0x0200014E RID: 334
	public static class SRPHelper
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0001C991 File Offset: 0x0001AB91
		public static string renderPipelineScriptingDefineSymbolAsString
		{
			get
			{
				return "VLB_URP";
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001C998 File Offset: 0x0001AB98
		public static RenderPipeline projectRenderPipeline
		{
			get
			{
				if (!SRPHelper.m_IsRenderPipelineCached)
				{
					SRPHelper.m_RenderPipelineCached = SRPHelper.ComputeRenderPipeline();
					SRPHelper.m_IsRenderPipelineCached = true;
				}
				return SRPHelper.m_RenderPipelineCached;
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001C9B8 File Offset: 0x0001ABB8
		private static RenderPipeline ComputeRenderPipeline()
		{
			RenderPipelineAsset renderPipelineAsset = GraphicsSettings.renderPipelineAsset;
			if (renderPipelineAsset)
			{
				string text = renderPipelineAsset.GetType().ToString();
				if (text.Contains("Universal"))
				{
					return RenderPipeline.URP;
				}
				if (text.Contains("Lightweight"))
				{
					return RenderPipeline.URP;
				}
				if (text.Contains("HD"))
				{
					return RenderPipeline.HDRP;
				}
			}
			return RenderPipeline.BuiltIn;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001CA0D File Offset: 0x0001AC0D
		public static bool IsUsingCustomRenderPipeline()
		{
			return RenderPipelineManager.currentPipeline != null || GraphicsSettings.renderPipelineAsset != null;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001CA23 File Offset: 0x0001AC23
		public static void RegisterOnBeginCameraRendering(Action<ScriptableRenderContext, Camera> cb)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				RenderPipelineManager.beginCameraRendering -= cb;
				RenderPipelineManager.beginCameraRendering += cb;
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001CA38 File Offset: 0x0001AC38
		public static void UnregisterOnBeginCameraRendering(Action<ScriptableRenderContext, Camera> cb)
		{
			if (SRPHelper.IsUsingCustomRenderPipeline())
			{
				RenderPipelineManager.beginCameraRendering -= cb;
			}
		}

		// Token: 0x0400073F RID: 1855
		private static bool m_IsRenderPipelineCached;

		// Token: 0x04000740 RID: 1856
		private static RenderPipeline m_RenderPipelineCached;
	}
}
