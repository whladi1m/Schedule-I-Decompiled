using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x020000F2 RID: 242
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-flicker/")]
	public class EffectFlicker : EffectAbstractBase
	{
		// Token: 0x060003E5 RID: 997 RVA: 0x00015C7C File Offset: 0x00013E7C
		public override void InitFrom(EffectAbstractBase source)
		{
			base.InitFrom(source);
			EffectFlicker effectFlicker = source as EffectFlicker;
			if (effectFlicker)
			{
				this.frequency = effectFlicker.frequency;
				this.performPauses = effectFlicker.performPauses;
				this.flickeringDuration = effectFlicker.flickeringDuration;
				this.pauseDuration = effectFlicker.pauseDuration;
				this.restoreIntensityOnPause = effectFlicker.restoreIntensityOnPause;
				this.intensityAmplitude = effectFlicker.intensityAmplitude;
				this.smoothing = effectFlicker.smoothing;
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00015CF3 File Offset: 0x00013EF3
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CoUpdate());
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00015D08 File Offset: 0x00013F08
		private IEnumerator CoUpdate()
		{
			for (;;)
			{
				yield return this.CoFlicker();
				if (this.performPauses)
				{
					yield return this.CoChangeIntensity(this.pauseDuration.randomValue, this.restoreIntensityOnPause ? 0f : this.m_CurrentAdditiveIntensity);
				}
			}
			yield break;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00015D17 File Offset: 0x00013F17
		private IEnumerator CoFlicker()
		{
			float remainingDuration = this.flickeringDuration.randomValue;
			float deltaTime = Time.deltaTime;
			while (!this.performPauses || remainingDuration > 0f)
			{
				float freqDuration = 1f / this.frequency;
				yield return this.CoChangeIntensity(freqDuration, this.intensityAmplitude.randomValue);
				remainingDuration -= freqDuration;
			}
			yield break;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00015D26 File Offset: 0x00013F26
		private IEnumerator CoChangeIntensity(float expectedDuration, float nextIntensity)
		{
			float velocity = 0f;
			float t = 0f;
			while (t < expectedDuration)
			{
				this.m_CurrentAdditiveIntensity = Mathf.SmoothDamp(this.m_CurrentAdditiveIntensity, nextIntensity, ref velocity, this.smoothing);
				base.SetAdditiveIntensity(this.m_CurrentAdditiveIntensity);
				t += Time.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x04000557 RID: 1367
		public new const string ClassName = "EffectFlicker";

		// Token: 0x04000558 RID: 1368
		[Range(1f, 60f)]
		public float frequency = 10f;

		// Token: 0x04000559 RID: 1369
		public bool performPauses;

		// Token: 0x0400055A RID: 1370
		[MinMaxRange(0f, 10f)]
		public MinMaxRangeFloat flickeringDuration = Consts.Effects.FlickeringDurationDefault;

		// Token: 0x0400055B RID: 1371
		[MinMaxRange(0f, 10f)]
		public MinMaxRangeFloat pauseDuration = Consts.Effects.PauseDurationDefault;

		// Token: 0x0400055C RID: 1372
		public bool restoreIntensityOnPause;

		// Token: 0x0400055D RID: 1373
		[MinMaxRange(-5f, 5f)]
		public MinMaxRangeFloat intensityAmplitude = Consts.Effects.IntensityAmplitudeDefault;

		// Token: 0x0400055E RID: 1374
		[Range(0f, 0.25f)]
		public float smoothing = 0.05f;

		// Token: 0x0400055F RID: 1375
		private float m_CurrentAdditiveIntensity;
	}
}
