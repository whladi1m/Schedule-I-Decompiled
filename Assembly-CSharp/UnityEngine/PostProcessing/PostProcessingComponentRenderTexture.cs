using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D3 RID: 211
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000366 RID: 870 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Prepare(Material material)
		{
		}
	}
}
