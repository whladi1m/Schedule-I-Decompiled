using System;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x02000A2C RID: 2604
	public class DestroyUIAtBounds : MonoBehaviour
	{
		// Token: 0x06004640 RID: 17984 RVA: 0x00126070 File Offset: 0x00124270
		public void Update()
		{
			if (this.Rect.anchoredPosition.x < this.MinBounds.x || this.Rect.anchoredPosition.x > this.MaxBounds.x || this.Rect.anchoredPosition.y < this.MinBounds.y || this.Rect.anchoredPosition.y > this.MaxBounds.y)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040033F5 RID: 13301
		public RectTransform Rect;

		// Token: 0x040033F6 RID: 13302
		public Vector2 MinBounds = new Vector2(-1000f, -1000f);

		// Token: 0x040033F7 RID: 13303
		public Vector2 MaxBounds = new Vector2(1000f, 1000f);
	}
}
