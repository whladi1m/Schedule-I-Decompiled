using System;
using FishNet;
using ScheduleOne.Economy;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FB RID: 763
	public class Quest_WeNeedToCook : Quest
	{
		// Token: 0x060010F9 RID: 4345 RVA: 0x0004BBD0 File Offset: 0x00049DD0
		protected override void MinPass()
		{
			base.MinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.QuestState == EQuestState.Inactive)
			{
				if (!this.MethSupplier.RelationData.Unlocked)
				{
					return;
				}
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

		// Token: 0x04001119 RID: 4377
		public Quest[] PrerequisiteQuests;

		// Token: 0x0400111A RID: 4378
		public Supplier MethSupplier;
	}
}
