using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000173 RID: 371
	public class VerticalBounce : MonoBehaviour
	{
		// Token: 0x06000702 RID: 1794 RVA: 0x000202F0 File Offset: 0x0001E4F0
		private void Update()
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.y, base.transform.localPosition.z);
			this.y += this.speed;
			this.direction = ((this.y < 0f) ? 1f : -1f);
			this.speed += Time.deltaTime * this.direction * this.acceleration;
		}

		// Token: 0x04000800 RID: 2048
		[Range(0f, 0.1f)]
		public float acceleration = 0.1f;

		// Token: 0x04000801 RID: 2049
		private float direction = 1f;

		// Token: 0x04000802 RID: 2050
		private float y;

		// Token: 0x04000803 RID: 2051
		private float speed = 0.01f;
	}
}
