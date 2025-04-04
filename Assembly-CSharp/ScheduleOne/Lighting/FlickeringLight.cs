using System;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x02000598 RID: 1432
	[RequireComponent(typeof(Light))]
	public class FlickeringLight : MonoBehaviour
	{
		// Token: 0x060023AE RID: 9134 RVA: 0x00090EC2 File Offset: 0x0008F0C2
		private void Start()
		{
			this.lightSource = base.GetComponent<Light>();
			this.UpdateTargetValues();
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x00090ED8 File Offset: 0x0008F0D8
		private void Update()
		{
			this.lightSource.intensity = Mathf.Lerp(this.lightSource.intensity, this.targetIntensity, this.flickerSpeed * Time.deltaTime);
			if (this.enableColorShift)
			{
				this.lightSource.color = Color.Lerp(this.lightSource.color, this.targetColor, this.flickerSpeed * Time.deltaTime);
			}
			if (Mathf.Abs(this.lightSource.intensity - this.targetIntensity) < 0.05f)
			{
				this.UpdateTargetValues();
			}
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x00090F6B File Offset: 0x0008F16B
		private void UpdateTargetValues()
		{
			this.targetIntensity = UnityEngine.Random.Range(this.minIntensity, this.maxIntensity);
			if (this.enableColorShift)
			{
				this.targetColor = Color.Lerp(this.minColor, this.maxColor, UnityEngine.Random.value);
			}
		}

		// Token: 0x04001A99 RID: 6809
		[Header("Intensity Settings")]
		[Tooltip("The minimum light intensity.")]
		public float minIntensity = 0.8f;

		// Token: 0x04001A9A RID: 6810
		[Tooltip("The maximum light intensity.")]
		public float maxIntensity = 1.2f;

		// Token: 0x04001A9B RID: 6811
		[Header("Color Settings")]
		[Tooltip("Enable slight color shifts to simulate a warm flame.")]
		public bool enableColorShift = true;

		// Token: 0x04001A9C RID: 6812
		public Color minColor = new Color(1f, 0.8f, 0.6f);

		// Token: 0x04001A9D RID: 6813
		public Color maxColor = new Color(1f, 0.9f, 0.7f);

		// Token: 0x04001A9E RID: 6814
		[Header("Flicker Speed")]
		[Tooltip("How quickly the light flickers (lower is faster).")]
		public float flickerSpeed = 0.1f;

		// Token: 0x04001A9F RID: 6815
		private Light lightSource;

		// Token: 0x04001AA0 RID: 6816
		private float targetIntensity;

		// Token: 0x04001AA1 RID: 6817
		private Color targetColor;
	}
}
