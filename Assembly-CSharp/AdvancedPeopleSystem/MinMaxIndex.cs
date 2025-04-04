using System;
using UnityEngine;

namespace AdvancedPeopleSystem
{
	// Token: 0x0200020A RID: 522
	[Serializable]
	public class MinMaxIndex
	{
		// Token: 0x06000B6E RID: 2926 RVA: 0x000354EB File Offset: 0x000336EB
		public int GetRandom(int max)
		{
			return Mathf.Clamp(UnityEngine.Random.Range(this.Min, this.Max), -1, max);
		}

		// Token: 0x04000C5F RID: 3167
		public int Min;

		// Token: 0x04000C60 RID: 3168
		public int Max;
	}
}
