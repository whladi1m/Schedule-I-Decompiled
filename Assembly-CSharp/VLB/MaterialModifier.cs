using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200012A RID: 298
	public static class MaterialModifier
	{
		// Token: 0x0200012B RID: 299
		public interface Interface
		{
			// Token: 0x0600050A RID: 1290
			void SetMaterialProp(int nameID, float value);

			// Token: 0x0600050B RID: 1291
			void SetMaterialProp(int nameID, Vector4 value);

			// Token: 0x0600050C RID: 1292
			void SetMaterialProp(int nameID, Color value);

			// Token: 0x0600050D RID: 1293
			void SetMaterialProp(int nameID, Matrix4x4 value);

			// Token: 0x0600050E RID: 1294
			void SetMaterialProp(int nameID, Texture value);
		}

		// Token: 0x0200012C RID: 300
		// (Invoke) Token: 0x06000510 RID: 1296
		public delegate void Callback(MaterialModifier.Interface owner);
	}
}
