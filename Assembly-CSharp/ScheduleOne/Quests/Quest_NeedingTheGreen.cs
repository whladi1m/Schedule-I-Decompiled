using System;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;

namespace ScheduleOne.Quests
{
	// Token: 0x020002EF RID: 751
	public class Quest_NeedingTheGreen : Quest
	{
		// Token: 0x060010BC RID: 4284 RVA: 0x0004AE54 File Offset: 0x00049054
		protected override void MinPass()
		{
			base.MinPass();
			string text = MoneyManager.FormatAmount(this.LifetimeEarningsRequirement, false, false);
			this.EarnEntry.SetEntryTitle(string.Concat(new string[]
			{
				"Earn ",
				text,
				" (",
				MoneyManager.FormatAmount(NetworkSingleton<MoneyManager>.Instance.LifetimeEarnings, false, false),
				" / ",
				text,
				")"
			}));
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.QuestState == EQuestState.Inactive)
			{
				Quest[] prerequisiteQuests = this.PrerequisiteQuests;
				for (int i = 0; i < prerequisiteQuests.Length; i++)
				{
					if (prerequisiteQuests[i].QuestState != EQuestState.Completed)
					{
						return;
					}
				}
				this.Begin(true);
			}
		}

		// Token: 0x040010EC RID: 4332
		public Quest[] PrerequisiteQuests;

		// Token: 0x040010ED RID: 4333
		public QuestEntry EarnEntry;

		// Token: 0x040010EE RID: 4334
		public float LifetimeEarningsRequirement = 10000f;
	}
}
