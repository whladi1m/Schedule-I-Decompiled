using System;

namespace VLB
{
	// Token: 0x02000109 RID: 265
	[Flags]
	public enum ShadowUpdateRate
	{
		// Token: 0x040005BA RID: 1466
		Never = 1,
		// Token: 0x040005BB RID: 1467
		OnEnable = 2,
		// Token: 0x040005BC RID: 1468
		OnBeamMove = 4,
		// Token: 0x040005BD RID: 1469
		EveryXFrames = 8,
		// Token: 0x040005BE RID: 1470
		OnBeamMoveAndEveryXFrames = 12
	}
}
