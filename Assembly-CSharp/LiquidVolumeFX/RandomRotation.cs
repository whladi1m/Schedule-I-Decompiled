using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x0200016C RID: 364
	public class RandomRotation : MonoBehaviour
	{
		// Token: 0x060006EF RID: 1775 RVA: 0x0001F9BA File Offset: 0x0001DBBA
		private void Start()
		{
			this.randomization = UnityEngine.Random.value;
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001F9C8 File Offset: 0x0001DBC8
		private void Update()
		{
			if (Time.time > this.lastTime)
			{
				this.lastTime = Time.time + this.randomChangeInterval + this.randomization;
				this.v = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
			}
			base.transform.Rotate(this.v * Time.deltaTime * this.speed);
		}

		// Token: 0x040007CE RID: 1998
		[Range(1f, 50f)]
		public float speed = 10f;

		// Token: 0x040007CF RID: 1999
		[Range(1f, 30f)]
		public float randomChangeInterval = 10f;

		// Token: 0x040007D0 RID: 2000
		private float lastTime;

		// Token: 0x040007D1 RID: 2001
		private Vector3 v;

		// Token: 0x040007D2 RID: 2002
		private float randomization;
	}
}
