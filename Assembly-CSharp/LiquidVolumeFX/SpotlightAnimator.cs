using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000172 RID: 370
	public class SpotlightAnimator : MonoBehaviour
	{
		// Token: 0x060006FF RID: 1791 RVA: 0x00020175 File Offset: 0x0001E375
		private void Awake()
		{
			this.spotLight = base.GetComponent<Light>();
			this.spotLight.intensity = 0f;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00020194 File Offset: 0x0001E394
		private void Update()
		{
			if (Time.time < this.lightOnDelay)
			{
				return;
			}
			float num = (Time.time - this.lightOnDelay) / this.duration;
			this.spotLight.intensity = Mathf.Lerp(this.initialIntensity, this.targetIntensity, num);
			if (Time.time - this.lastColorChange > this.nextColorInterval)
			{
				if (this.changingColor)
				{
					num = (Time.time - this.colorChangeStarted) / this.colorChangeDuration;
					if (num >= 1f)
					{
						this.changingColor = false;
						this.lastColorChange = Time.time;
					}
					this.spotLight.color = Color.Lerp(this.currentColor, this.nextColor, num);
					return;
				}
				this.currentColor = this.spotLight.color;
				this.nextColor = new Color(Mathf.Clamp01(UnityEngine.Random.value + 0.25f), Mathf.Clamp01(UnityEngine.Random.value + 0.25f), Mathf.Clamp01(UnityEngine.Random.value + 0.25f), 1f);
				this.changingColor = true;
				this.colorChangeStarted = Time.time;
			}
		}

		// Token: 0x040007F4 RID: 2036
		public float lightOnDelay = 2f;

		// Token: 0x040007F5 RID: 2037
		public float targetIntensity = 3.5f;

		// Token: 0x040007F6 RID: 2038
		public float initialIntensity;

		// Token: 0x040007F7 RID: 2039
		public float duration = 3f;

		// Token: 0x040007F8 RID: 2040
		public float nextColorInterval = 2f;

		// Token: 0x040007F9 RID: 2041
		public float colorChangeDuration = 2f;

		// Token: 0x040007FA RID: 2042
		private Light spotLight;

		// Token: 0x040007FB RID: 2043
		private float lastColorChange;

		// Token: 0x040007FC RID: 2044
		private float colorChangeStarted;

		// Token: 0x040007FD RID: 2045
		private Color currentColor;

		// Token: 0x040007FE RID: 2046
		private Color nextColor;

		// Token: 0x040007FF RID: 2047
		private bool changingColor;
	}
}
