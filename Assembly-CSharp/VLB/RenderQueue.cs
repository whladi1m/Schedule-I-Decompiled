using System;

namespace VLB
{
	// Token: 0x02000104 RID: 260
	public enum RenderQueue
	{
		// Token: 0x040005A2 RID: 1442
		Custom,
		// Token: 0x040005A3 RID: 1443
		Background = 1000,
		// Token: 0x040005A4 RID: 1444
		Geometry = 2000,
		// Token: 0x040005A5 RID: 1445
		AlphaTest = 2450,
		// Token: 0x040005A6 RID: 1446
		GeometryLast = 2500,
		// Token: 0x040005A7 RID: 1447
		Transparent = 3000,
		// Token: 0x040005A8 RID: 1448
		Overlay = 4000
	}
}
