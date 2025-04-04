using System;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008B0 RID: 2224
	public class LiquidLevelVisuals : MonoBehaviour
	{
		// Token: 0x06003C87 RID: 15495 RVA: 0x000FE488 File Offset: 0x000FC688
		private void Update()
		{
			if (this.Container == null)
			{
				return;
			}
			float num = this.Container.CurrentLiquidLevel / this.Container.MaxLevel;
			this.LiquidSurface.localPosition = Vector3.Lerp(this.LiquidSurface_Min.localPosition, this.LiquidSurface_Max.localPosition, num);
			this.LiquidSurface.localScale = new Vector3(this.LiquidSurface.localScale.x, num, this.LiquidSurface.localScale.z);
			this.LiquidSurface.gameObject.SetActive(this.Container.CurrentLiquidLevel > 0f);
		}

		// Token: 0x04002BA5 RID: 11173
		public LiquidContainer Container;

		// Token: 0x04002BA6 RID: 11174
		public Transform LiquidSurface;

		// Token: 0x04002BA7 RID: 11175
		public Transform LiquidSurface_Min;

		// Token: 0x04002BA8 RID: 11176
		public Transform LiquidSurface_Max;
	}
}
