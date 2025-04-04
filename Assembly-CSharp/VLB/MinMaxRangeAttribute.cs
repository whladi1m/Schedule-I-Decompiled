using System;

namespace VLB
{
	// Token: 0x02000132 RID: 306
	public class MinMaxRangeAttribute : Attribute
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x00019446 File Offset: 0x00017646
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x0001944E File Offset: 0x0001764E
		public float minValue { get; private set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00019457 File Offset: 0x00017657
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x0001945F File Offset: 0x0001765F
		public float maxValue { get; private set; }

		// Token: 0x06000536 RID: 1334 RVA: 0x00019468 File Offset: 0x00017668
		public MinMaxRangeAttribute(float min, float max)
		{
			this.minValue = min;
			this.maxValue = max;
		}
	}
}
