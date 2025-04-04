using System;
using UnityEngine;

namespace ScheduleOne.TV
{
	// Token: 0x0200029A RID: 666
	public class PongPaddle : MonoBehaviour
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x0003DBE4 File Offset: 0x0003BDE4
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x0003DBEC File Offset: 0x0003BDEC
		public float TargetY { get; set; }

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0003DBF5 File Offset: 0x0003BDF5
		public void SetTargetY(float y)
		{
			this.TargetY = y;
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0003DBFE File Offset: 0x0003BDFE
		private void Update()
		{
			this.UpdateMove();
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0003DC08 File Offset: 0x0003BE08
		private void UpdateMove()
		{
			float num = this.Rect.anchoredPosition.y;
			num = Mathf.Lerp(num, this.TargetY, 20f * Time.deltaTime * this.SpeedMultiplier);
			num = Mathf.Clamp(num, -160f, 160f);
			this.Rect.anchoredPosition = new Vector3(this.Rect.anchoredPosition.x, num);
		}

		// Token: 0x04000E70 RID: 3696
		public const float BOUND_Y = 160f;

		// Token: 0x04000E71 RID: 3697
		public const float MOVE_SPEED = 20f;

		// Token: 0x04000E72 RID: 3698
		public float SpeedMultiplier = 1f;

		// Token: 0x04000E74 RID: 3700
		public RectTransform Rect;
	}
}
