using System;
using System.Collections.Generic;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A4 RID: 420
	[Serializable]
	public class KeyframeGroup<T> : IKeyframeGroup where T : IBaseKeyframe
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00026BD9 File Offset: 0x00024DD9
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x00026BE1 File Offset: 0x00024DE1
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00026BEA File Offset: 0x00024DEA
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x00026BF2 File Offset: 0x00024DF2
		public string id
		{
			get
			{
				return this.m_Id;
			}
			set
			{
				this.m_Id = value;
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00026BFC File Offset: 0x00024DFC
		public KeyframeGroup(string name)
		{
			this.name = name;
			this.id = Guid.NewGuid().ToString();
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00026C3A File Offset: 0x00024E3A
		public void AddKeyFrame(T keyFrame)
		{
			this.keyframes.Add(keyFrame);
			this.SortKeyframes();
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00026C4E File Offset: 0x00024E4E
		public void RemoveKeyFrame(T keyFrame)
		{
			if (this.keyframes.Count == 1)
			{
				Debug.LogError("You must have at least 1 keyframe in every group.");
				return;
			}
			this.keyframes.Remove(keyFrame);
			this.SortKeyframes();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00026C7C File Offset: 0x00024E7C
		public void RemoveKeyFrame(IBaseKeyframe keyframe)
		{
			this.RemoveKeyFrame((T)((object)keyframe));
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00026C8A File Offset: 0x00024E8A
		public int GetKeyFrameCount()
		{
			return this.keyframes.Count;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00026C97 File Offset: 0x00024E97
		public T GetKeyframe(int index)
		{
			return this.keyframes[index];
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00026CA5 File Offset: 0x00024EA5
		public void SortKeyframes()
		{
			this.keyframes.Sort();
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00026CB2 File Offset: 0x00024EB2
		public float CurveAdjustedBlendingTime(InterpolationCurve curve, float t)
		{
			if (curve == InterpolationCurve.Linear)
			{
				return t;
			}
			if (curve == InterpolationCurve.EaseInEaseOut)
			{
				return Mathf.Clamp01((t < 0.5f) ? (2f * t * t) : (-1f + (4f - 2f * t) * t));
			}
			return t;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00026CEC File Offset: 0x00024EEC
		public T GetPreviousKeyFrame(float time)
		{
			T result;
			T t;
			if (!this.GetSurroundingKeyFrames(time, out result, out t))
			{
				return default(T);
			}
			return result;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00026D14 File Offset: 0x00024F14
		public bool GetSurroundingKeyFrames(float time, out T beforeKeyframe, out T afterKeyframe)
		{
			beforeKeyframe = default(T);
			afterKeyframe = default(T);
			int index;
			int index2;
			if (this.GetSurroundingKeyFrames(time, out index, out index2))
			{
				beforeKeyframe = this.GetKeyframe(index);
				afterKeyframe = this.GetKeyframe(index2);
				return true;
			}
			return false;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00026D5C File Offset: 0x00024F5C
		public bool GetSurroundingKeyFrames(float time, out int beforeIndex, out int afterIndex)
		{
			beforeIndex = 0;
			afterIndex = 0;
			if (this.keyframes.Count == 0)
			{
				Debug.LogError("Can't return nearby keyframes since it's empty.");
				return false;
			}
			if (this.keyframes.Count == 1)
			{
				return true;
			}
			T t = this.keyframes[0];
			if (time < t.time)
			{
				beforeIndex = this.keyframes.Count - 1;
				afterIndex = 0;
				return true;
			}
			int num = 0;
			for (int i = 0; i < this.keyframes.Count; i++)
			{
				t = this.keyframes[i];
				if (t.time >= time)
				{
					break;
				}
				num = i;
			}
			int num2 = (num + 1) % this.keyframes.Count;
			beforeIndex = num;
			afterIndex = num2;
			return true;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00026E17 File Offset: 0x00025017
		public static float ProgressBetweenSurroundingKeyframes(float time, BaseKeyframe beforeKey, BaseKeyframe afterKey)
		{
			return KeyframeGroup<T>.ProgressBetweenSurroundingKeyframes(time, beforeKey.time, afterKey.time);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00026E2C File Offset: 0x0002502C
		public static float ProgressBetweenSurroundingKeyframes(float time, float beforeKeyTime, float afterKeyTime)
		{
			if (afterKeyTime > beforeKeyTime && time <= beforeKeyTime)
			{
				return 0f;
			}
			float num = KeyframeGroup<T>.WidthBetweenCircularValues(beforeKeyTime, afterKeyTime);
			return Mathf.Clamp01(KeyframeGroup<T>.WidthBetweenCircularValues(beforeKeyTime, time) / num);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00026E5D File Offset: 0x0002505D
		public static float WidthBetweenCircularValues(float begin, float end)
		{
			if (begin <= end)
			{
				return end - begin;
			}
			return 1f - begin + end;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00026E70 File Offset: 0x00025070
		public void TrimToSingleKeyframe()
		{
			if (this.keyframes.Count == 1)
			{
				return;
			}
			this.keyframes.RemoveRange(1, this.keyframes.Count - 1);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00026E9C File Offset: 0x0002509C
		public InterpolationDirection GetShortestInterpolationDirection(float previousKeyValue, float nextKeyValue, float minValue, float maxValue)
		{
			float num;
			float num2;
			this.CalculateCircularDistances(previousKeyValue, nextKeyValue, minValue, maxValue, out num, out num2);
			if (num2 > num)
			{
				return InterpolationDirection.Reverse;
			}
			return InterpolationDirection.Foward;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00026EBF File Offset: 0x000250BF
		public void CalculateCircularDistances(float previousKeyValue, float nextKeyValue, float minValue, float maxValue, out float forwardDistance, out float reverseDistance)
		{
			if (nextKeyValue < previousKeyValue)
			{
				forwardDistance = maxValue - previousKeyValue + (nextKeyValue - minValue);
			}
			else
			{
				forwardDistance = nextKeyValue - previousKeyValue;
			}
			reverseDistance = minValue + maxValue - forwardDistance;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00026EE4 File Offset: 0x000250E4
		public float InterpolateFloat(InterpolationCurve curve, InterpolationDirection direction, float time, float beforeTime, float nextTime, float previousKeyValue, float nextKeyValue, float minValue, float maxValue)
		{
			float t = KeyframeGroup<T>.ProgressBetweenSurroundingKeyframes(time, beforeTime, nextTime);
			float num = this.CurveAdjustedBlendingTime(curve, t);
			if (direction == InterpolationDirection.Auto)
			{
				return this.AutoInterpolation(num, previousKeyValue, nextKeyValue);
			}
			InterpolationDirection interpolationDirection = direction;
			float num2;
			float num3;
			this.CalculateCircularDistances(previousKeyValue, nextKeyValue, minValue, maxValue, out num2, out num3);
			if (interpolationDirection == InterpolationDirection.ShortestPath)
			{
				if (num3 > num2)
				{
					interpolationDirection = InterpolationDirection.Foward;
				}
				else
				{
					interpolationDirection = InterpolationDirection.Reverse;
				}
			}
			if (interpolationDirection == InterpolationDirection.Foward)
			{
				return this.ForwardInterpolation(num, previousKeyValue, nextKeyValue, minValue, maxValue, num2);
			}
			if (interpolationDirection == InterpolationDirection.Reverse)
			{
				return this.ReverseInterpolation(num, previousKeyValue, nextKeyValue, minValue, maxValue, num3);
			}
			Debug.LogError("Unhandled interpolation direction: " + interpolationDirection.ToString() + ", returning min value.");
			return minValue;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00026F85 File Offset: 0x00025185
		public float AutoInterpolation(float curvedTime, float previousValue, float nextValue)
		{
			return Mathf.Lerp(previousValue, nextValue, curvedTime);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00026F90 File Offset: 0x00025190
		public float ForwardInterpolation(float time, float previousKeyValue, float nextKeyValue, float minValue, float maxValue, float distance)
		{
			if (previousKeyValue <= nextKeyValue)
			{
				return Mathf.Lerp(previousKeyValue, nextKeyValue, time);
			}
			float num = time * distance;
			float num2 = maxValue - previousKeyValue;
			if (num <= num2)
			{
				return previousKeyValue + num;
			}
			return minValue + (num - num2);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00026FC4 File Offset: 0x000251C4
		public float ReverseInterpolation(float time, float previousKeyValue, float nextKeyValue, float minValue, float maxValue, float distance)
		{
			if (nextKeyValue <= previousKeyValue)
			{
				return Mathf.Lerp(previousKeyValue, nextKeyValue, time);
			}
			float num = time * distance;
			float num2 = previousKeyValue - minValue;
			if (num <= num2)
			{
				return previousKeyValue - num;
			}
			return maxValue - (num - num2);
		}

		// Token: 0x04000952 RID: 2386
		public List<T> keyframes = new List<T>();

		// Token: 0x04000953 RID: 2387
		[SerializeField]
		private string m_Name;

		// Token: 0x04000954 RID: 2388
		[SerializeField]
		private string m_Id;
	}
}
