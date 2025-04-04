using System;

namespace VLB
{
	// Token: 0x02000107 RID: 263
	[Flags]
	public enum DynamicOcclusionUpdateRate
	{
		// Token: 0x040005B0 RID: 1456
		Never = 1,
		// Token: 0x040005B1 RID: 1457
		OnEnable = 2,
		// Token: 0x040005B2 RID: 1458
		OnBeamMove = 4,
		// Token: 0x040005B3 RID: 1459
		EveryXFrames = 8,
		// Token: 0x040005B4 RID: 1460
		OnBeamMoveAndEveryXFrames = 12
	}
}
