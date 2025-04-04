using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A6 RID: 422
	[Serializable]
	public class SpherePointKeyframeGroup : KeyframeGroup<SpherePointKeyframe>
	{
		// Token: 0x0600089D RID: 2205 RVA: 0x0002713B File Offset: 0x0002533B
		public SpherePointKeyframeGroup(string name) : base(name)
		{
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00027144 File Offset: 0x00025344
		public SpherePointKeyframeGroup(string name, SpherePointKeyframe keyframe) : base(name)
		{
			base.AddKeyFrame(keyframe);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00027154 File Offset: 0x00025354
		public SpherePoint SpherePointForTime(float time)
		{
			if (this.keyframes.Count == 1)
			{
				return this.keyframes[0].spherePoint;
			}
			int index;
			int index2;
			if (!base.GetSurroundingKeyFrames(time, out index, out index2))
			{
				Debug.LogError("Failed to get surrounding sphere point for time: " + time.ToString());
				return null;
			}
			time -= (float)((int)time);
			SpherePointKeyframe keyframe = base.GetKeyframe(index);
			SpherePointKeyframe keyframe2 = base.GetKeyframe(index2);
			float t = KeyframeGroup<SpherePointKeyframe>.ProgressBetweenSurroundingKeyframes(time, keyframe.time, keyframe2.time);
			float t2 = base.CurveAdjustedBlendingTime(keyframe.interpolationCurve, t);
			return new SpherePoint(Vector3.Slerp(keyframe.spherePoint.GetWorldDirection(), keyframe2.spherePoint.GetWorldDirection(), t2).normalized);
		}

		// Token: 0x04000957 RID: 2391
		public const float MinHorizontalRotation = -3.1415927f;

		// Token: 0x04000958 RID: 2392
		public const float MaxHorizontalRotation = 3.1415927f;

		// Token: 0x04000959 RID: 2393
		public const float MinVerticalRotation = -1.5707964f;

		// Token: 0x0400095A RID: 2394
		public const float MaxVerticalRotation = 1.5707964f;
	}
}
