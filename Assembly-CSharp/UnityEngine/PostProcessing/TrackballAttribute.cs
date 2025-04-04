using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200006A RID: 106
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x0600023F RID: 575 RVA: 0x0000D148 File Offset: 0x0000B348
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x04000274 RID: 628
		public readonly string method;
	}
}
