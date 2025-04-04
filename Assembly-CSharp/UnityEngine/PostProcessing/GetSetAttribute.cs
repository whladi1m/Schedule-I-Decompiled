using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000068 RID: 104
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000D12A File Offset: 0x0000B32A
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x04000271 RID: 625
		public readonly string name;

		// Token: 0x04000272 RID: 626
		public bool dirty;
	}
}
