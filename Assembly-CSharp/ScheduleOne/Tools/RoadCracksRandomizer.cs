using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000857 RID: 2135
	public class RoadCracksRandomizer : MonoBehaviour
	{
		// Token: 0x06003A34 RID: 14900 RVA: 0x000F5040 File Offset: 0x000F3240
		[Button]
		private void Randomize()
		{
			List<Transform> list = new List<Transform>(this.Cracks);
			for (int i = 0; i < list.Count; i++)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				Transform value = list[i];
				list[i] = list[index];
				list[index] = value;
			}
			int num = UnityEngine.Random.Range(this.MinCount, this.MaxCount + 1);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].gameObject.SetActive(j < num);
			}
		}

		// Token: 0x04002A02 RID: 10754
		public Transform[] Cracks;

		// Token: 0x04002A03 RID: 10755
		public int MinCount;

		// Token: 0x04002A04 RID: 10756
		public int MaxCount = 4;
	}
}
