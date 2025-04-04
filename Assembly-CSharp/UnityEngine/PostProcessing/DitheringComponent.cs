using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200007B RID: 123
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000F736 File Offset: 0x0000D936
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000F755 File Offset: 0x0000D955
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000F760 File Offset: 0x0000D960
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i.ToString());
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
		public override void Prepare(Material uberMaterial)
		{
			int num = this.textureIndex + 1;
			this.textureIndex = num;
			if (num >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x040002CF RID: 719
		private Texture2D[] noiseTextures;

		// Token: 0x040002D0 RID: 720
		private int textureIndex;

		// Token: 0x040002D1 RID: 721
		private const int k_TextureCount = 64;

		// Token: 0x0200007C RID: 124
		private static class Uniforms
		{
			// Token: 0x040002D2 RID: 722
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x040002D3 RID: 723
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}
