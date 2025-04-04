using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x02000210 RID: 528
	[Serializable]
	public class MinMaxFacialBlendshapes
	{
		// Token: 0x06000B76 RID: 2934 RVA: 0x00035597 File Offset: 0x00033797
		public float GetRandom()
		{
			return UnityEngine.Random.Range(this.Min, this.Max);
		}

		// Token: 0x04000C72 RID: 3186
		public string name;

		// Token: 0x04000C73 RID: 3187
		[Range(-100f, 100f)]
		public float Min;

		// Token: 0x04000C74 RID: 3188
		[Range(-100f, 100f)]
		public float Max;
	}
}
