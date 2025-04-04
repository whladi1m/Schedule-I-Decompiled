using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Money
{
	// Token: 0x02000B60 RID: 2912
	public class CashSlot : HotbarSlot
	{
		// Token: 0x06004D7A RID: 19834 RVA: 0x0014704B File Offset: 0x0014524B
		public override void ClearStoredInstance(bool _internal = false)
		{
			(base.ItemInstance as CashInstance).SetBalance(0f, true);
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x000022C9 File Offset: 0x000004C9
		public override bool CanSlotAcceptCash()
		{
			return true;
		}

		// Token: 0x04003AAE RID: 15022
		public const float MAX_CASH_PER_SLOT = 1000f;
	}
}
