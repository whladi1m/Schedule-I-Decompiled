using System;
using System.Collections;
using System.Runtime.CompilerServices;
using EasyButtons;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000835 RID: 2101
	public class DelayedUnityEvent : MonoBehaviour
	{
		// Token: 0x060039B5 RID: 14773 RVA: 0x000F3FA0 File Offset: 0x000F21A0
		[Button]
		public void Execute()
		{
			base.StartCoroutine(this.<Execute>g__Wait|3_0());
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x000F3FC2 File Offset: 0x000F21C2
		[CompilerGenerated]
		private IEnumerator <Execute>g__Wait|3_0()
		{
			if (this.onDelayStart != null)
			{
				this.onDelayStart.Invoke();
			}
			yield return new WaitForSeconds(this.Delay);
			if (this.onDelayedExecute != null)
			{
				this.onDelayedExecute.Invoke();
			}
			yield break;
		}

		// Token: 0x040029B3 RID: 10675
		public float Delay = 1f;

		// Token: 0x040029B4 RID: 10676
		public UnityEvent onDelayStart;

		// Token: 0x040029B5 RID: 10677
		public UnityEvent onDelayedExecute;
	}
}
