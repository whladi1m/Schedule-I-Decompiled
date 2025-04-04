using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000069 RID: 105
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0000D139 File Offset: 0x0000B339
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04000273 RID: 627
		public readonly float min;
	}
}
