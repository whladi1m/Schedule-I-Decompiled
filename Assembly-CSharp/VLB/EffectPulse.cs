using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x020000F7 RID: 247
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-pulse/")]
	public class EffectPulse : EffectAbstractBase
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x00016084 File Offset: 0x00014284
		public override void InitFrom(EffectAbstractBase source)
		{
			base.InitFrom(source);
			EffectPulse effectPulse = source as EffectPulse;
			if (effectPulse)
			{
				this.frequency = effectPulse.frequency;
				this.intensityAmplitude = effectPulse.intensityAmplitude;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000160BF File Offset: 0x000142BF
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CoUpdate());
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000160D4 File Offset: 0x000142D4
		private IEnumerator CoUpdate()
		{
			float t = 0f;
			for (;;)
			{
				float num = Mathf.Sin(this.frequency * t);
				float lerpedValue = this.intensityAmplitude.GetLerpedValue(num * 0.5f + 0.5f);
				base.SetAdditiveIntensity(lerpedValue);
				yield return null;
				t += Time.deltaTime;
			}
			yield break;
		}

		// Token: 0x04000572 RID: 1394
		public new const string ClassName = "EffectPulse";

		// Token: 0x04000573 RID: 1395
		[Range(0.1f, 60f)]
		public float frequency = 10f;

		// Token: 0x04000574 RID: 1396
		[MinMaxRange(-5f, 5f)]
		public MinMaxRangeFloat intensityAmplitude = Consts.Effects.IntensityAmplitudeDefault;
	}
}
