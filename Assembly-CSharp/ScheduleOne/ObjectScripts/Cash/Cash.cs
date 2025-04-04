using System;
using UnityEngine;

namespace ScheduleOne.ObjectScripts.Cash
{
	// Token: 0x02000BD3 RID: 3027
	public class Cash : MonoBehaviour
	{
		// Token: 0x060054FC RID: 21756 RVA: 0x001659F5 File Offset: 0x00163BF5
		public static int GetBillStacksToDisplay(float amount)
		{
			return Mathf.Clamp((int)(amount / 5f), 1, 50);
		}

		// Token: 0x04003EFC RID: 16124
		public static float stackSize = 250f;

		// Token: 0x04003EFD RID: 16125
		public static int[] amounts = new int[]
		{
			5,
			50,
			(int)Cash.stackSize
		};
	}
}
