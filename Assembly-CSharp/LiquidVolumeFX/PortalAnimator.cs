using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000171 RID: 369
	public class PortalAnimator : MonoBehaviour
	{
		// Token: 0x060006FC RID: 1788 RVA: 0x000200B5 File Offset: 0x0001E2B5
		private void Start()
		{
			this.scale = base.transform.localScale;
			base.transform.localScale = Vector3.zero;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000200D8 File Offset: 0x0001E2D8
		private void Update()
		{
			if (Time.time < this.delay)
			{
				return;
			}
			float value;
			if (Time.time > this.delayFadeOut)
			{
				value = 1f - (Time.time - this.delayFadeOut) / this.duration;
			}
			else
			{
				value = (Time.time - this.delay) / this.duration;
			}
			base.transform.localScale = Mathf.Clamp01(value) * this.scale;
		}

		// Token: 0x040007F0 RID: 2032
		public float delay = 2f;

		// Token: 0x040007F1 RID: 2033
		public float duration = 1f;

		// Token: 0x040007F2 RID: 2034
		public float delayFadeOut = 4f;

		// Token: 0x040007F3 RID: 2035
		private Vector3 scale;
	}
}
