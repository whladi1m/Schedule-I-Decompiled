using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D7 RID: 215
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x06000378 RID: 888 RVA: 0x0001419C File Offset: 0x0001239C
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000141C8 File Offset: 0x000123C8
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe key = this.curve[length - 1];
			key.time -= this.m_Range;
			Keyframe key2 = this.curve[0];
			key2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(key);
			this.m_InternalLoopingCurve.AddKey(key2);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00014278 File Offset: 0x00012478
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x0400046A RID: 1130
		public AnimationCurve curve;

		// Token: 0x0400046B RID: 1131
		[SerializeField]
		private bool m_Loop;

		// Token: 0x0400046C RID: 1132
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x0400046D RID: 1133
		[SerializeField]
		private float m_Range;

		// Token: 0x0400046E RID: 1134
		private AnimationCurve m_InternalLoopingCurve;
	}
}
