using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x0200084A RID: 2122
	public class IntervalEvent : MonoBehaviour
	{
		// Token: 0x060039FD RID: 14845 RVA: 0x000F4710 File Offset: 0x000F2910
		public void Start()
		{
			base.InvokeRepeating("Execute", this.Interval, this.Interval);
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x000F4729 File Offset: 0x000F2929
		private void Execute()
		{
			if (this.Event != null)
			{
				this.Event.Invoke();
			}
		}

		// Token: 0x040029DE RID: 10718
		public float Interval = 1f;

		// Token: 0x040029DF RID: 10719
		public UnityEvent Event;
	}
}
