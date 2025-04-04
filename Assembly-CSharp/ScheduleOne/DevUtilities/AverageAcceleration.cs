using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006C3 RID: 1731
	public class AverageAcceleration : MonoBehaviour
	{
		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002F31 RID: 12081 RVA: 0x000C4CC0 File Offset: 0x000C2EC0
		// (set) Token: 0x06002F32 RID: 12082 RVA: 0x000C4CC8 File Offset: 0x000C2EC8
		public Vector3 Acceleration { get; private set; } = Vector3.zero;

		// Token: 0x06002F33 RID: 12083 RVA: 0x000C4CD4 File Offset: 0x000C2ED4
		private void Start()
		{
			if (this.Rb == null)
			{
				this.Rb = base.GetComponent<Rigidbody>();
			}
			this.accelerations = new Vector3[Mathf.CeilToInt(this.TimeWindow / Time.fixedDeltaTime)];
			for (int i = 0; i < this.accelerations.Length; i++)
			{
				this.accelerations[i] = Vector3.zero;
			}
			this.prevVelocity = this.Rb.velocity;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000C4D4C File Offset: 0x000C2F4C
		private void FixedUpdate()
		{
			this.timer += Time.fixedDeltaTime;
			if (this.timer >= this.TimeWindow)
			{
				this.timer -= Time.fixedDeltaTime;
				this.accelerations[this.currentIndex] = Vector3.zero;
				this.currentIndex = (this.currentIndex + 1) % this.accelerations.Length;
			}
			Vector3 vector = (this.Rb.velocity - this.prevVelocity) / Time.fixedDeltaTime;
			this.accelerations[this.currentIndex] = vector;
			this.prevVelocity = this.Rb.velocity;
			Vector3 a = Vector3.zero;
			for (int i = 0; i < this.accelerations.Length; i++)
			{
				a += this.accelerations[i];
			}
			this.Acceleration = a / (float)this.accelerations.Length;
		}

		// Token: 0x04002199 RID: 8601
		public Rigidbody Rb;

		// Token: 0x0400219A RID: 8602
		public float TimeWindow = 0.5f;

		// Token: 0x0400219B RID: 8603
		private Vector3[] accelerations;

		// Token: 0x0400219C RID: 8604
		private int currentIndex;

		// Token: 0x0400219D RID: 8605
		private float timer;

		// Token: 0x0400219E RID: 8606
		private Vector3 prevVelocity;
	}
}
