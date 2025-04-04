using System;
using UnityEngine;

namespace Funly.SkyStudio
{
	// Token: 0x020001A8 RID: 424
	[Serializable]
	public class BaseKeyframe : IComparable, IBaseKeyframe
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00027275 File Offset: 0x00025475
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x0002727D File Offset: 0x0002547D
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

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00027286 File Offset: 0x00025486
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x0002728E File Offset: 0x0002548E
		public float time
		{
			get
			{
				return this.m_Time;
			}
			set
			{
				this.m_Time = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00027297 File Offset: 0x00025497
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x0002729F File Offset: 0x0002549F
		public InterpolationCurve interpolationCurve
		{
			get
			{
				return this.m_InterpolationCurve;
			}
			set
			{
				this.m_InterpolationCurve = value;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x000272A8 File Offset: 0x000254A8
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x000272B0 File Offset: 0x000254B0
		public InterpolationDirection interpolationDirection
		{
			get
			{
				return this.m_InterpolationDirection;
			}
			set
			{
				this.m_InterpolationDirection = value;
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x000272BC File Offset: 0x000254BC
		public BaseKeyframe(float time)
		{
			this.id = Guid.NewGuid().ToString();
			this.time = time;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000272F0 File Offset: 0x000254F0
		public int CompareTo(object other)
		{
			BaseKeyframe baseKeyframe = other as BaseKeyframe;
			return this.time.CompareTo(baseKeyframe.time);
		}

		// Token: 0x0400095B RID: 2395
		[SerializeField]
		public string m_Id;

		// Token: 0x0400095C RID: 2396
		[SerializeField]
		private float m_Time;

		// Token: 0x0400095D RID: 2397
		[SerializeField]
		private InterpolationCurve m_InterpolationCurve;

		// Token: 0x0400095E RID: 2398
		[SerializeField]
		private InterpolationDirection m_InterpolationDirection;
	}
}
