using System;
using UnityEngine;

namespace StylizedGrassDemo
{
	// Token: 0x02000164 RID: 356
	public class MoveInCircle : MonoBehaviour
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x0001EFFA File Offset: 0x0001D1FA
		private void Update()
		{
			this.Move();
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001F004 File Offset: 0x0001D204
		private void Move()
		{
			float x = Mathf.Sin(Time.realtimeSinceStartup * this.speed) * this.radius + this.offset.x;
			float y = base.transform.position.y + this.offset.y;
			float z = Mathf.Cos(Time.realtimeSinceStartup * this.speed) * this.radius + this.offset.z;
			base.transform.localPosition = new Vector3(x, y, z);
		}

		// Token: 0x040007A7 RID: 1959
		public float radius = 1f;

		// Token: 0x040007A8 RID: 1960
		public float speed = 1f;

		// Token: 0x040007A9 RID: 1961
		public Vector3 offset;
	}
}
