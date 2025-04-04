using System;

namespace AdvancedPeopleSystem
{
	// Token: 0x020001F9 RID: 505
	public enum CombinerState : byte
	{
		// Token: 0x04000BFD RID: 3069
		NotCombined,
		// Token: 0x04000BFE RID: 3070
		InProgressCombineMesh,
		// Token: 0x04000BFF RID: 3071
		InProgressBlendshapeTransfer,
		// Token: 0x04000C00 RID: 3072
		InProgressClear,
		// Token: 0x04000C01 RID: 3073
		Combined,
		// Token: 0x04000C02 RID: 3074
		UsedPreBuitMeshes
	}
}
