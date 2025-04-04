using System;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A2E RID: 2606
	public class UIMover : MonoBehaviour
	{
		// Token: 0x06004644 RID: 17988 RVA: 0x001261C0 File Offset: 0x001243C0
		private void Start()
		{
			this.speed = new Vector2(UnityEngine.Random.Range(this.MinSpeed.x, this.MaxSpeed.x), UnityEngine.Random.Range(this.MinSpeed.y, this.MaxSpeed.y));
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x00126210 File Offset: 0x00124410
		public void Update()
		{
			Vector2 b = this.speed * this.SpeedMultiplier * Time.deltaTime;
			this.Rect.anchoredPosition += b;
		}

		// Token: 0x040033FE RID: 13310
		public RectTransform Rect;

		// Token: 0x040033FF RID: 13311
		public Vector2 MinSpeed = Vector2.one;

		// Token: 0x04003400 RID: 13312
		public Vector2 MaxSpeed = Vector2.one;

		// Token: 0x04003401 RID: 13313
		public float SpeedMultiplier = 1f;

		// Token: 0x04003402 RID: 13314
		private Vector2 speed = Vector2.zero;
	}
}
