using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LiquidVolumeFX
{
	// Token: 0x0200017F RID: 383
	public class LiquidVolumeDepthPrePassRenderFeature : ScriptableRendererFeature
	{
		// Token: 0x060007E3 RID: 2019 RVA: 0x00024ED8 File Offset: 0x000230D8
		public static void AddLiquidToBackRenderers(LiquidVolume lv)
		{
			if (lv == null || lv.topology != TOPOLOGY.Irregular || LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Contains(lv))
			{
				return;
			}
			LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Add(lv);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00024F06 File Offset: 0x00023106
		public static void RemoveLiquidFromBackRenderers(LiquidVolume lv)
		{
			if (lv == null || !LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Contains(lv))
			{
				return;
			}
			LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Remove(lv);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00024F2B File Offset: 0x0002312B
		public static void AddLiquidToFrontRenderers(LiquidVolume lv)
		{
			if (lv == null || lv.topology != TOPOLOGY.Irregular || LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Contains(lv))
			{
				return;
			}
			LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Add(lv);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00024F59 File Offset: 0x00023159
		public static void RemoveLiquidFromFrontRenderers(LiquidVolume lv)
		{
			if (lv == null || !LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Contains(lv))
			{
				return;
			}
			LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Remove(lv);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00024F80 File Offset: 0x00023180
		private void OnDestroy()
		{
			Shader.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.ForcedInvisible, 0f);
			CoreUtils.Destroy(this.mat);
			if (this.backPass != null)
			{
				this.backPass.CleanUp();
			}
			if (this.frontPass != null)
			{
				this.frontPass.CleanUp();
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00024FD0 File Offset: 0x000231D0
		public override void Create()
		{
			base.name = "Liquid Volume Depth PrePass";
			this.shader = Shader.Find("LiquidVolume/DepthPrePass");
			if (this.shader == null)
			{
				return;
			}
			this.mat = CoreUtils.CreateEngineMaterial(this.shader);
			this.backPass = new LiquidVolumeDepthPrePassRenderFeature.DepthPass(this.mat, LiquidVolumeDepthPrePassRenderFeature.Pass.BackBuffer, this.renderPassEvent);
			this.frontPass = new LiquidVolumeDepthPrePassRenderFeature.DepthPass(this.mat, LiquidVolumeDepthPrePassRenderFeature.Pass.FrontBuffer, this.renderPassEvent);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00025048 File Offset: 0x00023248
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			LiquidVolumeDepthPrePassRenderFeature.installed = true;
			if (this.backPass != null && LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers.Count > 0)
			{
				this.backPass.Setup(this, renderer);
				renderer.EnqueuePass(this.backPass);
			}
			if (this.frontPass != null && LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers.Count > 0)
			{
				this.frontPass.Setup(this, renderer);
				this.frontPass.renderer = renderer;
				renderer.EnqueuePass(this.frontPass);
			}
		}

		// Token: 0x040008EB RID: 2283
		public static readonly List<LiquidVolume> lvBackRenderers = new List<LiquidVolume>();

		// Token: 0x040008EC RID: 2284
		public static readonly List<LiquidVolume> lvFrontRenderers = new List<LiquidVolume>();

		// Token: 0x040008ED RID: 2285
		[SerializeField]
		[HideInInspector]
		private Shader shader;

		// Token: 0x040008EE RID: 2286
		public static bool installed;

		// Token: 0x040008EF RID: 2287
		private Material mat;

		// Token: 0x040008F0 RID: 2288
		private LiquidVolumeDepthPrePassRenderFeature.DepthPass backPass;

		// Token: 0x040008F1 RID: 2289
		private LiquidVolumeDepthPrePassRenderFeature.DepthPass frontPass;

		// Token: 0x040008F2 RID: 2290
		[Tooltip("Renders each irregular liquid volume completely before rendering the next one.")]
		public bool interleavedRendering;

		// Token: 0x040008F3 RID: 2291
		public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;

		// Token: 0x02000180 RID: 384
		private static class ShaderParams
		{
			// Token: 0x040008F4 RID: 2292
			public const string RTBackBufferName = "_VLBackBufferTexture";

			// Token: 0x040008F5 RID: 2293
			public static int RTBackBuffer = Shader.PropertyToID("_VLBackBufferTexture");

			// Token: 0x040008F6 RID: 2294
			public const string RTFrontBufferName = "_VLFrontBufferTexture";

			// Token: 0x040008F7 RID: 2295
			public static int RTFrontBuffer = Shader.PropertyToID("_VLFrontBufferTexture");

			// Token: 0x040008F8 RID: 2296
			public static int FlaskThickness = Shader.PropertyToID("_FlaskThickness");

			// Token: 0x040008F9 RID: 2297
			public static int ForcedInvisible = Shader.PropertyToID("_LVForcedInvisible");

			// Token: 0x040008FA RID: 2298
			public const string SKW_FP_RENDER_TEXTURE = "LIQUID_VOLUME_FP_RENDER_TEXTURES";
		}

		// Token: 0x02000181 RID: 385
		private enum Pass
		{
			// Token: 0x040008FC RID: 2300
			BackBuffer,
			// Token: 0x040008FD RID: 2301
			FrontBuffer
		}

		// Token: 0x02000182 RID: 386
		private class DepthPass : ScriptableRenderPass
		{
			// Token: 0x060007ED RID: 2029 RVA: 0x0002512C File Offset: 0x0002332C
			public DepthPass(Material mat, LiquidVolumeDepthPrePassRenderFeature.Pass pass, RenderPassEvent renderPassEvent)
			{
				base.renderPassEvent = renderPassEvent;
				this.mat = mat;
				this.passData.depthPass = this;
				if (pass == LiquidVolumeDepthPrePassRenderFeature.Pass.BackBuffer)
				{
					this.targetNameId = LiquidVolumeDepthPrePassRenderFeature.ShaderParams.RTBackBuffer;
					RenderTargetIdentifier tex = new RenderTargetIdentifier(this.targetNameId, 0, CubemapFace.Unknown, -1);
					this.targetRT = RTHandles.Alloc(tex, "_VLBackBufferTexture");
					this.passId = 0;
					this.lvRenderers = LiquidVolumeDepthPrePassRenderFeature.lvBackRenderers;
					return;
				}
				if (pass != LiquidVolumeDepthPrePassRenderFeature.Pass.FrontBuffer)
				{
					return;
				}
				this.targetNameId = LiquidVolumeDepthPrePassRenderFeature.ShaderParams.RTFrontBuffer;
				RenderTargetIdentifier tex2 = new RenderTargetIdentifier(this.targetNameId, 0, CubemapFace.Unknown, -1);
				this.targetRT = RTHandles.Alloc(tex2, "_VLFrontBufferTexture");
				this.passId = 1;
				this.lvRenderers = LiquidVolumeDepthPrePassRenderFeature.lvFrontRenderers;
			}

			// Token: 0x060007EE RID: 2030 RVA: 0x000251E9 File Offset: 0x000233E9
			public void Setup(LiquidVolumeDepthPrePassRenderFeature feature, ScriptableRenderer renderer)
			{
				this.renderer = renderer;
				this.interleavedRendering = feature.interleavedRendering;
			}

			// Token: 0x060007EF RID: 2031 RVA: 0x00025200 File Offset: 0x00023400
			private int SortByDistanceToCamera(LiquidVolume lv1, LiquidVolume lv2)
			{
				bool flag = lv1 == null;
				bool flag2 = lv2 == null;
				if (flag && flag2)
				{
					return 0;
				}
				if (flag2)
				{
					return 1;
				}
				if (flag)
				{
					return -1;
				}
				float num = Vector3.Distance(lv1.transform.position, LiquidVolumeDepthPrePassRenderFeature.DepthPass.currentCameraPosition);
				float num2 = Vector3.Distance(lv2.transform.position, LiquidVolumeDepthPrePassRenderFeature.DepthPass.currentCameraPosition);
				if (num < num2)
				{
					return 1;
				}
				if (num > num2)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x060007F0 RID: 2032 RVA: 0x00025268 File Offset: 0x00023468
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
				cameraTextureDescriptor.colorFormat = (LiquidVolume.useFPRenderTextures ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32);
				cameraTextureDescriptor.sRGB = false;
				cameraTextureDescriptor.depthBufferBits = 16;
				cameraTextureDescriptor.msaaSamples = 1;
				cmd.GetTemporaryRT(this.targetNameId, cameraTextureDescriptor);
				if (!this.interleavedRendering)
				{
					base.ConfigureTarget(this.targetRT);
				}
				base.ConfigureInput(ScriptableRenderPassInput.Depth);
			}

			// Token: 0x060007F1 RID: 2033 RVA: 0x000252CC File Offset: 0x000234CC
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				if (this.lvRenderers == null)
				{
					return;
				}
				CommandBuffer commandBuffer = CommandBufferPool.Get("LiquidVolumeDepthPrePass");
				commandBuffer.Clear();
				this.passData.cam = renderingData.cameraData.camera;
				this.passData.cmd = commandBuffer;
				this.passData.mat = this.mat;
				this.passData.cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				this.passData.source = this.renderer.cameraColorTargetHandle;
				this.passData.depth = this.renderer.cameraDepthTargetHandle;
				LiquidVolumeDepthPrePassRenderFeature.DepthPass.ExecutePass(this.passData);
				context.ExecuteCommandBuffer(commandBuffer);
				CommandBufferPool.Release(commandBuffer);
			}

			// Token: 0x060007F2 RID: 2034 RVA: 0x00025384 File Offset: 0x00023584
			private static void ExecutePass(LiquidVolumeDepthPrePassRenderFeature.DepthPass.PassData passData)
			{
				CommandBuffer cmd = passData.cmd;
				cmd.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.ForcedInvisible, 0f);
				Camera cam = passData.cam;
				LiquidVolumeDepthPrePassRenderFeature.DepthPass depthPass = passData.depthPass;
				RenderTextureDescriptor cameraTargetDescriptor = passData.cameraTargetDescriptor;
				cameraTargetDescriptor.colorFormat = (LiquidVolume.useFPRenderTextures ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32);
				cameraTargetDescriptor.sRGB = false;
				cameraTargetDescriptor.depthBufferBits = 16;
				cameraTargetDescriptor.msaaSamples = 1;
				cmd.GetTemporaryRT(depthPass.targetNameId, cameraTargetDescriptor);
				int count = depthPass.lvRenderers.Count;
				if (depthPass.interleavedRendering)
				{
					RenderTargetIdentifier rt = new RenderTargetIdentifier(depthPass.targetNameId, 0, CubemapFace.Unknown, -1);
					LiquidVolumeDepthPrePassRenderFeature.DepthPass.currentCameraPosition = cam.transform.position;
					depthPass.lvRenderers.Sort(new Comparison<LiquidVolume>(depthPass.SortByDistanceToCamera));
					for (int i = 0; i < count; i++)
					{
						LiquidVolume liquidVolume = depthPass.lvRenderers[i];
						if (liquidVolume != null && liquidVolume.isActiveAndEnabled)
						{
							if (liquidVolume.topology == TOPOLOGY.Irregular)
							{
								cmd.SetRenderTarget(rt, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
								if (LiquidVolume.useFPRenderTextures)
								{
									cmd.ClearRenderTarget(true, true, new Color(cam.farClipPlane, 0f, 0f, 0f), 1f);
									cmd.EnableShaderKeyword("LIQUID_VOLUME_FP_RENDER_TEXTURES");
								}
								else
								{
									cmd.ClearRenderTarget(true, true, new Color(0.9882353f, 0.4470558f, 0.75f, 0f), 1f);
									cmd.DisableShaderKeyword("LIQUID_VOLUME_FP_RENDER_TEXTURES");
								}
								cmd.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.FlaskThickness, 1f - liquidVolume.flaskThickness);
								cmd.DrawRenderer(liquidVolume.mr, passData.mat, (liquidVolume.subMeshIndex >= 0) ? liquidVolume.subMeshIndex : 0, depthPass.passId);
							}
							RenderTargetIdentifier color = new RenderTargetIdentifier(passData.source, 0, CubemapFace.Unknown, -1);
							RenderTargetIdentifier depth = new RenderTargetIdentifier(passData.depth, 0, CubemapFace.Unknown, -1);
							cmd.SetRenderTarget(color, depth);
							cmd.DrawRenderer(liquidVolume.mr, liquidVolume.liqMat, (liquidVolume.subMeshIndex >= 0) ? liquidVolume.subMeshIndex : 0, 1);
						}
					}
					cmd.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.ForcedInvisible, 1f);
					return;
				}
				RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(depthPass.targetNameId, 0, CubemapFace.Unknown, -1);
				cmd.SetRenderTarget(renderTargetIdentifier);
				cmd.SetGlobalTexture(depthPass.targetNameId, renderTargetIdentifier);
				if (LiquidVolume.useFPRenderTextures)
				{
					cmd.ClearRenderTarget(true, true, new Color(cam.farClipPlane, 0f, 0f, 0f), 1f);
					cmd.EnableShaderKeyword("LIQUID_VOLUME_FP_RENDER_TEXTURES");
				}
				else
				{
					cmd.ClearRenderTarget(true, true, new Color(0.9882353f, 0.4470558f, 0.75f, 0f), 1f);
					cmd.DisableShaderKeyword("LIQUID_VOLUME_FP_RENDER_TEXTURES");
				}
				for (int j = 0; j < count; j++)
				{
					LiquidVolume liquidVolume2 = depthPass.lvRenderers[j];
					if (liquidVolume2 != null && liquidVolume2.isActiveAndEnabled)
					{
						cmd.SetGlobalFloat(LiquidVolumeDepthPrePassRenderFeature.ShaderParams.FlaskThickness, 1f - liquidVolume2.flaskThickness);
						cmd.DrawRenderer(liquidVolume2.mr, passData.mat, (liquidVolume2.subMeshIndex >= 0) ? liquidVolume2.subMeshIndex : 0, depthPass.passId);
					}
				}
			}

			// Token: 0x060007F3 RID: 2035 RVA: 0x000256C5 File Offset: 0x000238C5
			public void CleanUp()
			{
				RTHandles.Release(this.targetRT);
			}

			// Token: 0x040008FE RID: 2302
			private const string profilerTag = "LiquidVolumeDepthPrePass";

			// Token: 0x040008FF RID: 2303
			private Material mat;

			// Token: 0x04000900 RID: 2304
			private int targetNameId;

			// Token: 0x04000901 RID: 2305
			private RTHandle targetRT;

			// Token: 0x04000902 RID: 2306
			private int passId;

			// Token: 0x04000903 RID: 2307
			private List<LiquidVolume> lvRenderers;

			// Token: 0x04000904 RID: 2308
			public ScriptableRenderer renderer;

			// Token: 0x04000905 RID: 2309
			public bool interleavedRendering;

			// Token: 0x04000906 RID: 2310
			private static Vector3 currentCameraPosition;

			// Token: 0x04000907 RID: 2311
			private readonly LiquidVolumeDepthPrePassRenderFeature.DepthPass.PassData passData = new LiquidVolumeDepthPrePassRenderFeature.DepthPass.PassData();

			// Token: 0x02000183 RID: 387
			private class PassData
			{
				// Token: 0x04000908 RID: 2312
				public Camera cam;

				// Token: 0x04000909 RID: 2313
				public CommandBuffer cmd;

				// Token: 0x0400090A RID: 2314
				public LiquidVolumeDepthPrePassRenderFeature.DepthPass depthPass;

				// Token: 0x0400090B RID: 2315
				public Material mat;

				// Token: 0x0400090C RID: 2316
				public RTHandle source;

				// Token: 0x0400090D RID: 2317
				public RTHandle depth;

				// Token: 0x0400090E RID: 2318
				public RenderTextureDescriptor cameraTargetDescriptor;
			}
		}
	}
}
