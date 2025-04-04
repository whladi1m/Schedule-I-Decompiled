using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009A9 RID: 2473
	public class CanvasDistanceFade : MonoBehaviour
	{
		// Token: 0x060042CF RID: 17103 RVA: 0x0011800C File Offset: 0x0011620C
		public void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			float num = Vector3.Distance(PlayerSingleton<PlayerCamera>.Instance.transform.position, base.transform.position);
			if (num < this.MinDistance)
			{
				this.CanvasGroup.alpha = 1f;
				return;
			}
			if (num > this.MaxDistance)
			{
				this.CanvasGroup.alpha = 0f;
				return;
			}
			this.CanvasGroup.alpha = 1f - (num - this.MinDistance) / (this.MaxDistance - this.MinDistance);
		}

		// Token: 0x040030C4 RID: 12484
		public CanvasGroup CanvasGroup;

		// Token: 0x040030C5 RID: 12485
		public float MinDistance = 5f;

		// Token: 0x040030C6 RID: 12486
		public float MaxDistance = 10f;
	}
}
