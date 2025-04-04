using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D4 RID: 212
	public class PostProcessingContext
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00014043 File Offset: 0x00012243
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0001404B File Offset: 0x0001224B
		public bool interrupted { get; private set; }

		// Token: 0x0600036A RID: 874 RVA: 0x00014054 File Offset: 0x00012254
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001405D File Offset: 0x0001225D
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00014083 File Offset: 0x00012283
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == RenderingPath.DeferredShading;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00014093 File Offset: 0x00012293
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600036E RID: 878 RVA: 0x000140A0 File Offset: 0x000122A0
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600036F RID: 879 RVA: 0x000140AD File Offset: 0x000122AD
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000370 RID: 880 RVA: 0x000140BA File Offset: 0x000122BA
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x04000455 RID: 1109
		public PostProcessingProfile profile;

		// Token: 0x04000456 RID: 1110
		public Camera camera;

		// Token: 0x04000457 RID: 1111
		public MaterialFactory materialFactory;

		// Token: 0x04000458 RID: 1112
		public RenderTextureFactory renderTextureFactory;
	}
}
