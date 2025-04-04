using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000131 RID: 305
	[Serializable]
	public struct MinMaxRangeFloat : IEquatable<MinMaxRangeFloat>
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x0001935F File Offset: 0x0001755F
		public float minValue
		{
			get
			{
				return this.m_MinValue;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00019367 File Offset: 0x00017567
		public float maxValue
		{
			get
			{
				return this.m_MaxValue;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0001936F File Offset: 0x0001756F
		public float randomValue
		{
			get
			{
				return UnityEngine.Random.Range(this.minValue, this.maxValue);
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00019382 File Offset: 0x00017582
		public Vector2 asVector2
		{
			get
			{
				return new Vector2(this.minValue, this.maxValue);
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00019395 File Offset: 0x00017595
		public float GetLerpedValue(float lerp01)
		{
			return Mathf.Lerp(this.minValue, this.maxValue, lerp01);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000193A9 File Offset: 0x000175A9
		public MinMaxRangeFloat(float min, float max)
		{
			this.m_MinValue = min;
			this.m_MaxValue = max;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000193BC File Offset: 0x000175BC
		public override bool Equals(object obj)
		{
			if (obj is MinMaxRangeFloat)
			{
				MinMaxRangeFloat other = (MinMaxRangeFloat)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000193E1 File Offset: 0x000175E1
		public bool Equals(MinMaxRangeFloat other)
		{
			return this.m_MinValue == other.m_MinValue && this.m_MaxValue == other.m_MaxValue;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00019404 File Offset: 0x00017604
		public override int GetHashCode()
		{
			return new ValueTuple<float, float>(this.m_MinValue, this.m_MaxValue).GetHashCode();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00019430 File Offset: 0x00017630
		public static bool operator ==(MinMaxRangeFloat lhs, MinMaxRangeFloat rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001943A File Offset: 0x0001763A
		public static bool operator !=(MinMaxRangeFloat lhs, MinMaxRangeFloat rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04000679 RID: 1657
		[SerializeField]
		private float m_MinValue;

		// Token: 0x0400067A RID: 1658
		[SerializeField]
		private float m_MaxValue;
	}
}
