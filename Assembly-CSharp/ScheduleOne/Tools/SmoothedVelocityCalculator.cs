using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x0200085C RID: 2140
	public class SmoothedVelocityCalculator : MonoBehaviour
	{
		// Token: 0x06003A41 RID: 14913 RVA: 0x000F5687 File Offset: 0x000F3887
		private void Start()
		{
			this.lastFramePosition = base.transform.position;
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x000F569C File Offset: 0x000F389C
		protected virtual void FixedUpdate()
		{
			if (this.zeroOut)
			{
				this.Velocity = Vector3.zero;
				return;
			}
			Vector3 item = (base.transform.position - this.lastFramePosition) / Time.fixedDeltaTime;
			if (item.magnitude <= this.MaxReasonableVelocity)
			{
				this.VelocityHistory.Add(new Tuple<Vector3, float>(item, Time.timeSinceLevelLoad));
			}
			if (this.VelocityHistory.Count > this.maxSamples)
			{
				this.VelocityHistory.RemoveAt(0);
			}
			this.Velocity = this.GetAverageVelocity();
			this.lastFramePosition = base.transform.position;
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x000F5740 File Offset: 0x000F3940
		private Vector3 GetAverageVelocity()
		{
			Vector3 a = Vector3.zero;
			int num = 0;
			int num2 = this.VelocityHistory.Count - 1;
			while (num2 >= 0 && Time.timeSinceLevelLoad - this.VelocityHistory[num2].Item2 <= this.SampleLength)
			{
				a += this.VelocityHistory[num2].Item1;
				num++;
				num2--;
			}
			if (num == 0)
			{
				return Vector3.zero;
			}
			return a / (float)num;
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x000F57BB File Offset: 0x000F39BB
		public void FlushBuffer()
		{
			this.VelocityHistory.Clear();
			this.Velocity = Vector3.zero;
			this.lastFramePosition = base.transform.position;
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x000F57E4 File Offset: 0x000F39E4
		public void ZeroOut(float duration)
		{
			SmoothedVelocityCalculator.<>c__DisplayClass11_0 CS$<>8__locals1 = new SmoothedVelocityCalculator.<>c__DisplayClass11_0();
			CS$<>8__locals1.duration = duration;
			CS$<>8__locals1.<>4__this = this;
			this.zeroOut = true;
			base.StartCoroutine(CS$<>8__locals1.<ZeroOut>g__Routine|0());
		}

		// Token: 0x04002A13 RID: 10771
		public Vector3 Velocity = Vector3.zero;

		// Token: 0x04002A14 RID: 10772
		[Header("Settings")]
		public float SampleLength = 0.2f;

		// Token: 0x04002A15 RID: 10773
		public float MaxReasonableVelocity = 25f;

		// Token: 0x04002A16 RID: 10774
		private List<Tuple<Vector3, float>> VelocityHistory = new List<Tuple<Vector3, float>>();

		// Token: 0x04002A17 RID: 10775
		private int maxSamples = 20;

		// Token: 0x04002A18 RID: 10776
		private Vector3 lastFramePosition = Vector3.zero;

		// Token: 0x04002A19 RID: 10777
		private bool zeroOut;
	}
}
