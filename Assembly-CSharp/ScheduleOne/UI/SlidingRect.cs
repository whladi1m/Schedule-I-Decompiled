using System;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A2D RID: 2605
	public class SlidingRect : MonoBehaviour
	{
		// Token: 0x06004642 RID: 17986 RVA: 0x00126130 File Offset: 0x00124330
		public void Update()
		{
			this._time += Time.deltaTime * this.SpeedMultiplier;
			if (this._time > this.Duration)
			{
				this._time -= this.Duration;
			}
			float t = this._time / this.Duration;
			this.Rect.anchoredPosition = Vector2.Lerp(this.Start, this.End, t);
		}

		// Token: 0x040033F8 RID: 13304
		public RectTransform Rect;

		// Token: 0x040033F9 RID: 13305
		public Vector2 Start;

		// Token: 0x040033FA RID: 13306
		public Vector2 End;

		// Token: 0x040033FB RID: 13307
		public float Duration = 1f;

		// Token: 0x040033FC RID: 13308
		public float SpeedMultiplier = 1f;

		// Token: 0x040033FD RID: 13309
		private float _time;
	}
}
