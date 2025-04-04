using System;

namespace LiquidVolumeFX
{
	// Token: 0x0200017A RID: 378
	public static class DetailExtensions
	{
		// Token: 0x0600070E RID: 1806 RVA: 0x00020954 File Offset: 0x0001EB54
		public static bool allowsRefraction(this DETAIL detail)
		{
			return detail != DETAIL.DefaultNoFlask;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002095E File Offset: 0x0001EB5E
		public static bool usesFlask(this DETAIL detail)
		{
			return detail == DETAIL.Simple || detail == DETAIL.Default;
		}
	}
}
