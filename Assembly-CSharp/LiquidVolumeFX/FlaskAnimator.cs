using System;
using UnityEngine;

namespace LiquidVolumeFX
{
	// Token: 0x02000170 RID: 368
	public class FlaskAnimator : MonoBehaviour
	{
		// Token: 0x060006F9 RID: 1785 RVA: 0x0001FE53 File Offset: 0x0001E053
		private void Awake()
		{
			this.liquid = base.GetComponent<LiquidVolume>();
			this.level = this.liquid.level;
			this.liquid.alpha = 0f;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001FE84 File Offset: 0x0001E084
		private void Update()
		{
			float num = (this.duration > 0f) ? ((Time.time - this.delay) / this.duration) : 1f;
			if (num >= 1f)
			{
				this.level += this.direction * this.speed;
				if (this.level < this.minRange || this.level > this.maxRange)
				{
					this.direction *= -1f;
				}
				this.direction += Mathf.Sign(0.5f - this.level) * this.acceleration;
				this.level = Mathf.Clamp(this.level, this.minRange, this.maxRange);
				this.liquid.level = this.level;
				num = ((this.alphaDuration > 0f) ? Mathf.Clamp01((Time.time - this.duration - this.delay) / this.alphaDuration) : 1f);
				this.liquid.alpha = num;
				this.liquid.blurIntensity = num * this.finalRefractionBlur;
			}
			else if (this.initialPosition != this.finalPosition)
			{
				base.transform.position = Vector3.Lerp(this.initialPosition, this.finalPosition, num);
			}
			base.transform.Rotate(Vector3.up * Time.deltaTime * this.rotationSpeed * 57.29578f, Space.Self);
		}

		// Token: 0x040007E2 RID: 2018
		public float speed = 0.01f;

		// Token: 0x040007E3 RID: 2019
		public Vector3 initialPosition = Vector3.down * 4f;

		// Token: 0x040007E4 RID: 2020
		public Vector3 finalPosition = Vector3.zero;

		// Token: 0x040007E5 RID: 2021
		public float duration = 5f;

		// Token: 0x040007E6 RID: 2022
		public float delay = 6f;

		// Token: 0x040007E7 RID: 2023
		[Range(0f, 1f)]
		public float level;

		// Token: 0x040007E8 RID: 2024
		[Range(0f, 1f)]
		public float minRange = 0.05f;

		// Token: 0x040007E9 RID: 2025
		[Range(0f, 1f)]
		public float maxRange = 0.95f;

		// Token: 0x040007EA RID: 2026
		[Range(0f, 1f)]
		public float acceleration = 0.04f;

		// Token: 0x040007EB RID: 2027
		[Range(0f, 1f)]
		public float rotationSpeed = 0.25f;

		// Token: 0x040007EC RID: 2028
		[Range(0f, 2f)]
		public float alphaDuration = 2f;

		// Token: 0x040007ED RID: 2029
		[Range(0f, 1f)]
		public float finalRefractionBlur = 0.75f;

		// Token: 0x040007EE RID: 2030
		private LiquidVolume liquid;

		// Token: 0x040007EF RID: 2031
		private float direction = 1f;
	}
}
