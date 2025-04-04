using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Quests
{
	// Token: 0x020002EA RID: 746
	public class Quest_ExpandingOperations : Quest
	{
		// Token: 0x060010AE RID: 4270 RVA: 0x0004ABCC File Offset: 0x00048DCC
		protected override void MinPass()
		{
			base.MinPass();
			if (base.QuestState == EQuestState.Active)
			{
				int num = Mathf.Clamp(Mathf.RoundToInt(NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Sweatshop_Pots")) - 2, 0, 2);
				this.SetUpGrowTentsEntry.SetEntryTitle("Set up 2 more grow tents (" + num.ToString() + "/2)");
				if (num >= 2 && this.SetUpGrowTentsEntry.State != EQuestState.Completed)
				{
					this.SetUpGrowTentsEntry.Complete();
				}
				int count = Customer.UnlockedCustomers.Count;
				this.ReachCustomersEntry.SetEntryTitle("Reach 10 customers (" + count.ToString() + "/10)");
				if (count >= 10 && this.ReachCustomersEntry.State != EQuestState.Completed)
				{
					this.ReachCustomersEntry.Complete();
				}
			}
		}

		// Token: 0x040010E0 RID: 4320
		public QuestEntry SetUpGrowTentsEntry;

		// Token: 0x040010E1 RID: 4321
		public QuestEntry ReachCustomersEntry;
	}
}
