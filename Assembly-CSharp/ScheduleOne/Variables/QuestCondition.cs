using System;
using ScheduleOne.Quests;

namespace ScheduleOne.Variables
{
	// Token: 0x0200028F RID: 655
	[Serializable]
	public class QuestCondition
	{
		// Token: 0x06000D8B RID: 3467 RVA: 0x0003C574 File Offset: 0x0003A774
		public bool Evaluate()
		{
			Quest quest = Quest.GetQuest(this.QuestName);
			if (quest == null)
			{
				Console.LogError("Quest " + this.QuestName + " not found", null);
				return false;
			}
			if (this.CheckQuestState && quest.QuestState != this.QuestState)
			{
				return false;
			}
			if (this.CheckQuestEntryState)
			{
				if (quest.Entries.Count <= this.QuestEntryIndex)
				{
					Console.LogError("Quest " + this.QuestName + " does not have entry " + this.QuestEntryIndex.ToString(), null);
					return false;
				}
				if (quest.Entries[this.QuestEntryIndex].State != this.QuestEntryState)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000E29 RID: 3625
		public bool CheckQuestState = true;

		// Token: 0x04000E2A RID: 3626
		public string QuestName = "Quest name";

		// Token: 0x04000E2B RID: 3627
		public EQuestState QuestState = EQuestState.Active;

		// Token: 0x04000E2C RID: 3628
		public bool CheckQuestEntryState;

		// Token: 0x04000E2D RID: 3629
		public int QuestEntryIndex;

		// Token: 0x04000E2E RID: 3630
		public EQuestState QuestEntryState = EQuestState.Active;
	}
}
