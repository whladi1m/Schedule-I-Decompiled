using System;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008B1 RID: 2225
	public class LiquidVolumeCollider : MonoBehaviour
	{
		// Token: 0x06003C89 RID: 15497 RVA: 0x000FE536 File Offset: 0x000FC736
		private void Awake()
		{
			if (this.LiquidContainer == null)
			{
				this.LiquidContainer = base.GetComponentInParent<LiquidContainer>();
			}
		}

		// Token: 0x04002BA9 RID: 11177
		public LiquidContainer LiquidContainer;
	}
}
