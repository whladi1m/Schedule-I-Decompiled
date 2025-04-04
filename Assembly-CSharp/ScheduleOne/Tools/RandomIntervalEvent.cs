using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Tools
{
	// Token: 0x02000854 RID: 2132
	public class RandomIntervalEvent : MonoBehaviour
	{
		// Token: 0x06003A2C RID: 14892 RVA: 0x000F4F6B File Offset: 0x000F316B
		private void OnEnable()
		{
			if (this.ExecuteOnEnable)
			{
				this.Execute();
			}
			this.nextInterval = Time.time + UnityEngine.Random.Range(this.MinInterval, this.MaxInterval);
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x000F4F98 File Offset: 0x000F3198
		private void Update()
		{
			if (Time.time >= this.nextInterval)
			{
				this.Execute();
			}
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x000F4FAD File Offset: 0x000F31AD
		private void Execute()
		{
			if (this.OnInterval != null)
			{
				this.OnInterval.Invoke();
			}
			this.nextInterval = Time.time + UnityEngine.Random.Range(this.MinInterval, this.MaxInterval);
		}

		// Token: 0x040029FC RID: 10748
		public float MinInterval = 5f;

		// Token: 0x040029FD RID: 10749
		public float MaxInterval = 10f;

		// Token: 0x040029FE RID: 10750
		public bool ExecuteOnEnable;

		// Token: 0x040029FF RID: 10751
		public UnityEvent OnInterval;

		// Token: 0x04002A00 RID: 10752
		private float nextInterval;
	}
}
