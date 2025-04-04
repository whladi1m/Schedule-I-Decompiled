using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall.Utilities
{
	// Token: 0x0200022B RID: 555
	[Serializable]
	public class MaterialOverrides
	{
		// Token: 0x06000BC9 RID: 3017 RVA: 0x00036D90 File Offset: 0x00034F90
		public void OverrideMaterial(Material material)
		{
			foreach (MaterialOverrides.TextureProperty textureProperty in this.textureOverrides)
			{
				material.SetTexture(textureProperty.propertyName, textureProperty.propertyValue);
			}
			foreach (MaterialOverrides.FloatProperty floatProperty in this.floatOverrides)
			{
				material.SetFloat(floatProperty.propertyName, floatProperty.propertyValue);
			}
			foreach (MaterialOverrides.IntProperty intProperty in this.intOverrides)
			{
				material.SetInt(intProperty.propertyName, intProperty.propertyValue);
			}
			foreach (MaterialOverrides.VectorProperty vectorProperty in this.vectorOverrides)
			{
				material.SetVector(vectorProperty.propertyName, vectorProperty.propertyValue);
			}
			foreach (MaterialOverrides.ColorProperty colorProperty in this.colorOverrides)
			{
				material.SetColor(colorProperty.propertyName, colorProperty.propertyValue);
			}
		}

		// Token: 0x04000D36 RID: 3382
		public List<MaterialOverrides.TextureProperty> textureOverrides;

		// Token: 0x04000D37 RID: 3383
		public List<MaterialOverrides.FloatProperty> floatOverrides;

		// Token: 0x04000D38 RID: 3384
		public List<MaterialOverrides.IntProperty> intOverrides;

		// Token: 0x04000D39 RID: 3385
		public List<MaterialOverrides.VectorProperty> vectorOverrides;

		// Token: 0x04000D3A RID: 3386
		public List<MaterialOverrides.ColorProperty> colorOverrides;

		// Token: 0x0200022C RID: 556
		[Serializable]
		public class TextureProperty
		{
			// Token: 0x04000D3B RID: 3387
			public string propertyName;

			// Token: 0x04000D3C RID: 3388
			public Texture2D propertyValue;
		}

		// Token: 0x0200022D RID: 557
		[Serializable]
		public class FloatProperty
		{
			// Token: 0x04000D3D RID: 3389
			public string propertyName;

			// Token: 0x04000D3E RID: 3390
			public float propertyValue;
		}

		// Token: 0x0200022E RID: 558
		[Serializable]
		public class IntProperty
		{
			// Token: 0x04000D3F RID: 3391
			public string propertyName;

			// Token: 0x04000D40 RID: 3392
			public int propertyValue;
		}

		// Token: 0x0200022F RID: 559
		[Serializable]
		public class VectorProperty
		{
			// Token: 0x04000D41 RID: 3393
			public string propertyName;

			// Token: 0x04000D42 RID: 3394
			public Vector4 propertyValue;
		}

		// Token: 0x02000230 RID: 560
		[Serializable]
		public class ColorProperty
		{
			// Token: 0x04000D43 RID: 3395
			public string propertyName;

			// Token: 0x04000D44 RID: 3396
			public Color propertyValue;
		}
	}
}
