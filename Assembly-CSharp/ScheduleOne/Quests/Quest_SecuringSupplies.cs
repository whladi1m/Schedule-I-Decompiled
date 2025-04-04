using System;
using FishNet;
using ScheduleOne.Economy;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F2 RID: 754
	public class Quest_SecuringSupplies : Quest
	{
		// Token: 0x060010C3 RID: 4291 RVA: 0x0004AFFD File Offset: 0x000491FD
		protected override void MinPass()
		{
			base.MinPass();
			if (InstanceFinder.IsServer)
			{
				EQuestState questState = base.QuestState;
			}
		}

		// Token: 0x040010F1 RID: 4337
		public Supplier Supplier;
	}
}
