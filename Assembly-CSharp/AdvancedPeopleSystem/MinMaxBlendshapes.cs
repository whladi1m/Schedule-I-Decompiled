using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200020C RID: 524
	[Serializable]
	public class MinMaxBlendshapes
	{
		// Token: 0x06000B72 RID: 2930 RVA: 0x00035571 File Offset: 0x00033771
		public float GetRandom()
		{
			return UnityEngine.Random.Range(this.Min, this.Max);
		}

		// Token: 0x04000C63 RID: 3171
		[Range(-100f, 100f)]
		public float Min;

		// Token: 0x04000C64 RID: 3172
		[Range(-100f, 100f)]
		public float Max;
	}
}
