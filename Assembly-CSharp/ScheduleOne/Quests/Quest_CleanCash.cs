using System;
using FishNet;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Property;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E5 RID: 741
	public class Quest_CleanCash : Quest
	{
		// Token: 0x0600109E RID: 4254 RVA: 0x0004A844 File Offset: 0x00048A44
		protected override void MinPass()
		{
			base.MinPass();
			if (base.QuestState == EQuestState.Inactive && InstanceFinder.IsServer && ATM.WeeklyDepositSum >= 10000f)
			{
				this.Begin(true);
			}
			if (base.QuestState == EQuestState.Completed)
			{
				return;
			}
			if (InstanceFinder.IsServer && this.BuyBusinessEntry.State == EQuestState.Active && Business.OwnedBusinesses.Count > 0)
			{
				this.BuyBusinessEntry.Complete();
			}
			if (this.GoToBusinessEntry.State == EQuestState.Active)
			{
				if (Business.OwnedBusinesses.Count > 0)
				{
					this.GoToBusinessEntry.transform.position = Business.OwnedBusinesses[0].PoI.transform.position;
				}
				if (Player.Local.CurrentBusiness != null)
				{
					this.GoToBusinessEntry.Complete();
				}
			}
		}

		// Token: 0x040010DA RID: 4314
		public QuestEntry BuyBusinessEntry;

		// Token: 0x040010DB RID: 4315
		public QuestEntry GoToBusinessEntry;
	}
}
