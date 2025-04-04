using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A5 RID: 421
	[Serializable]
	public class NumberKeyframeGroup : KeyframeGroup<NumberKeyframe>
	{
		// Token: 0x06000896 RID: 2198 RVA: 0x00026FF6 File Offset: 0x000251F6
		public NumberKeyframeGroup(string name, float min, float max) : base(name)
		{
			this.minValue = min;
			this.maxValue = max;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002700D File Offset: 0x0002520D
		public NumberKeyframeGroup(string name, float min, float max, NumberKeyframe frame) : base(name)
		{
			this.minValue = min;
			this.maxValue = max;
			base.AddKeyFrame(frame);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002702C File Offset: 0x0002522C
		public float GetFirstValue()
		{
			return base.GetKeyframe(0).value;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0002703A File Offset: 0x0002523A
		public float ValueToPercent(float value)
		{
			return Mathf.Abs((value - this.minValue) / (this.maxValue - this.minValue));
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00027057 File Offset: 0x00025257
		public float ValuePercentAtTime(float time)
		{
			return this.ValueToPercent(this.NumericValueAtTime(time));
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00027066 File Offset: 0x00025266
		public float PercentToValue(float percent)
		{
			return Mathf.Clamp(this.minValue + (this.maxValue - this.minValue) * percent, this.minValue, this.maxValue);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00027090 File Offset: 0x00025290
		public float NumericValueAtTime(float time)
		{
			time -= (float)((int)time);
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Keyframe group has no keyframes: " + base.name);
				return this.minValue;
			}
			if (this.keyframes.Count == 1)
			{
				return base.GetKeyframe(0).value;
			}
			int index;
			int index2;
			base.GetSurroundingKeyFrames(time, out index, out index2);
			NumberKeyframe keyframe = base.GetKeyframe(index);
			NumberKeyframe keyframe2 = base.GetKeyframe(index2);
			return base.InterpolateFloat(keyframe.interpolationCurve, keyframe.interpolationDirection, time, keyframe.time, keyframe2.time, keyframe.value, keyframe2.value, this.minValue, this.maxValue);
		}

		// Token: 0x04000955 RID: 2389
		public float minValue;

		// Token: 0x04000956 RID: 2390
		public float maxValue;
	}
}
