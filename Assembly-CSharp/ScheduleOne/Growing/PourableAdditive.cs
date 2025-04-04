using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerTasks;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x0200086A RID: 2154
	public class PourableAdditive : Pourable
	{
		// Token: 0x06003A7A RID: 14970 RVA: 0x000F63B4 File Offset: 0x000F45B4
		protected override void PourAmount(float amount)
		{
			base.PourAmount(amount);
		}

		// Token: 0x04002A55 RID: 10837
		public const float NormalizedAmountForSuccess = 0.8f;

		// Token: 0x04002A56 RID: 10838
		public AdditiveDefinition AdditiveDefinition;

		// Token: 0x04002A57 RID: 10839
		public Color LiquidColor;

		// Token: 0x04002A58 RID: 10840
		private float pouredAmount;
	}
}
