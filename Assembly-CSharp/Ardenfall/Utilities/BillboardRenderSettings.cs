using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall.Utilities
{
	// Token: 0x02000228 RID: 552
	[CreateAssetMenu(menuName = "Ardenfall/Foliage/Billboard Render Settings")]
	public class BillboardRenderSettings : ScriptableObject
	{
		// Token: 0x04000D2A RID: 3370
		public List<BillboardRenderSettings.BillboardTexture> billboardTextures;

		// Token: 0x04000D2B RID: 3371
		public Shader billboardShader;

		// Token: 0x02000229 RID: 553
		[Serializable]
		public class BillboardTexture
		{
			// Token: 0x06000BC6 RID: 3014 RVA: 0x00036C28 File Offset: 0x00034E28
			public TextureFormat GetFormat()
			{
				Vector4 vector = default(Vector4);
				foreach (BillboardRenderSettings.BakePass bakePass in this.bakePasses)
				{
					if (bakePass.r)
					{
						vector.x += 1f;
					}
					if (bakePass.g)
					{
						vector.y += 1f;
					}
					if (bakePass.b)
					{
						vector.z += 1f;
					}
					if (bakePass.a)
					{
						vector.w += 1f;
					}
				}
				if (vector.x > 1f || vector.y > 1f || vector.z > 1f || vector.w > 1f)
				{
					Debug.LogError("Multiple bake passes in the same texture channel detected");
				}
				if (vector.w >= 1f)
				{
					return TextureFormat.RGBA32;
				}
				if (vector.z >= 1f)
				{
					return TextureFormat.RGB24;
				}
				if (vector.y >= 1f)
				{
					return TextureFormat.RG16;
				}
				return TextureFormat.R8;
			}

			// Token: 0x04000D2C RID: 3372
			public string textureId = "_MainTex";

			// Token: 0x04000D2D RID: 3373
			public bool powerOfTwo = true;

			// Token: 0x04000D2E RID: 3374
			public bool alphaIsTransparency = true;

			// Token: 0x04000D2F RID: 3375
			public List<BillboardRenderSettings.BakePass> bakePasses;
		}

		// Token: 0x0200022A RID: 554
		[Serializable]
		public class BakePass
		{
			// Token: 0x04000D30 RID: 3376
			public Shader customShader;

			// Token: 0x04000D31 RID: 3377
			public MaterialOverrides materialOverrides;

			// Token: 0x04000D32 RID: 3378
			public bool r = true;

			// Token: 0x04000D33 RID: 3379
			public bool g = true;

			// Token: 0x04000D34 RID: 3380
			public bool b = true;

			// Token: 0x04000D35 RID: 3381
			public bool a = true;
		}
	}
}
