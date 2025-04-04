using System;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x02000C19 RID: 3097
	[Serializable]
	public struct RenderLayer
	{
		// Token: 0x060056AB RID: 22187 RVA: 0x0016BAF0 File Offset: 0x00169CF0
		public RenderLayer(LayerMask mask, Color color)
		{
			this.mask = mask;
			this.color = color;
		}

		// Token: 0x0400406A RID: 16490
		public LayerMask mask;

		// Token: 0x0400406B RID: 16491
		public Color color;
	}
}
