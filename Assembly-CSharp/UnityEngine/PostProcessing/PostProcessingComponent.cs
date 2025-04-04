using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D1 RID: 209
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00014005 File Offset: 0x00012205
		// (set) Token: 0x0600035E RID: 862 RVA: 0x0001400D File Offset: 0x0001220D
		public T model { get; internal set; }

		// Token: 0x0600035F RID: 863 RVA: 0x00014016 File Offset: 0x00012216
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00014026 File Offset: 0x00012226
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
