using System;
using ScheduleOne.Economy;

namespace ScheduleOne.Quests
{
	// Token: 0x020002EE RID: 750
	public class Quest_MovingUp : Quest
	{
		// Token: 0x060010BA RID: 4282 RVA: 0x0004ADE8 File Offset: 0x00048FE8
		protected override void MinPass()
		{
			base.MinPass();
			if (this.ReachCustomersEntry.State == EQuestState.Active)
			{
				int count = Customer.UnlockedCustomers.Count;
				this.ReachCustomersEntry.SetEntryTitle("Unlock 10 customers (" + count.ToString() + "/10)");
				if (count >= 10 && this.ReachCustomersEntry.State != EQuestState.Completed)
				{
					this.ReachCustomersEntry.Complete();
				}
			}
		}

		// Token: 0x040010EB RID: 4331
		public QuestEntry ReachCustomersEntry;
	}
}
