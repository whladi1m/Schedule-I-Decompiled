using System;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200082C RID: 2092
	public class CashPile : MonoBehaviour
	{
		// Token: 0x06003991 RID: 14737 RVA: 0x000F3984 File Offset: 0x000F1B84
		private void Awake()
		{
			this.CashInstances = new Transform[this.Container.childCount];
			for (int i = 0; i < this.CashInstances.Length; i++)
			{
				this.CashInstances[i] = this.Container.GetChild(i);
				this.CashInstances[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x000F39E4 File Offset: 0x000F1BE4
		public void SetDisplayedAmount(float amount)
		{
			int num = Mathf.FloorToInt(amount / 100000f * (float)this.CashInstances.Length);
			for (int i = 0; i < this.CashInstances.Length; i++)
			{
				this.CashInstances[i].gameObject.SetActive(i < num);
			}
		}

		// Token: 0x04002996 RID: 10646
		public const float MAX_AMOUNT = 100000f;

		// Token: 0x04002997 RID: 10647
		public Transform Container;

		// Token: 0x04002998 RID: 10648
		private Transform[] CashInstances;
	}
}
