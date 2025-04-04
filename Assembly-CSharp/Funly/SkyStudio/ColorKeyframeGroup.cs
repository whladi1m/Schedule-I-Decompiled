using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A2 RID: 418
	[Serializable]
	public class ColorKeyframeGroup : KeyframeGroup<ColorKeyframe>
	{
		// Token: 0x06000873 RID: 2163 RVA: 0x00026B2B File Offset: 0x00024D2B
		public ColorKeyframeGroup(string name) : base(name)
		{
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00026B34 File Offset: 0x00024D34
		public ColorKeyframeGroup(string name, ColorKeyframe frame) : base(name)
		{
			base.AddKeyFrame(frame);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00026B44 File Offset: 0x00024D44
		public Color ColorForTime(float time)
		{
			time -= (float)((int)time);
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Can't return color since there aren't any keyframes.");
				return Color.white;
			}
			if (this.keyframes.Count == 1)
			{
				return base.GetKeyframe(0).color;
			}
			int index;
			int index2;
			base.GetSurroundingKeyFrames(time, out index, out index2);
			ColorKeyframe keyframe = base.GetKeyframe(index);
			ColorKeyframe keyframe2 = base.GetKeyframe(index2);
			float t = KeyframeGroup<ColorKeyframe>.ProgressBetweenSurroundingKeyframes(time, keyframe, keyframe2);
			float t2 = base.CurveAdjustedBlendingTime(keyframe.interpolationCurve, t);
			return Color.Lerp(keyframe.color, keyframe2.color, t2);
		}
	}
}
