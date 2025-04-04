using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D0 RID: 208
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x06000357 RID: 855 RVA: 0x00014002 File Offset: 0x00012202
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000358 RID: 856
		public abstract bool active { get; }

		// Token: 0x06000359 RID: 857 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void OnEnable()
		{
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void OnDisable()
		{
		}

		// Token: 0x0600035B RID: 859
		public abstract PostProcessingModel GetModel();

		// Token: 0x04000453 RID: 1107
		public PostProcessingContext context;
	}
}
