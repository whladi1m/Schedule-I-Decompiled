using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D2 RID: 210
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000362 RID: 866
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x06000363 RID: 867
		public abstract string GetName();

		// Token: 0x06000364 RID: 868
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
