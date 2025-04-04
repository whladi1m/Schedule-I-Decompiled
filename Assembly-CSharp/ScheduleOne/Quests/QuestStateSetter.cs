using System;
using ScheduleOne.DevUtilities;

namespace ScheduleOne.Quests
{
	// Token: 0x020002FC RID: 764
	[Serializable]
	public class QuestStateSetter
	{
		// Token: 0x060010FB RID: 4347 RVA: 0x0004BC30 File Offset: 0x00049E30
		public void Execute()
		{
			Quest quest = Quest.GetQuest(this.QuestName);
			if (quest == null)
			{
				Console.LogWarning("Failed to find quest with name: " + this.QuestName, null);
				return;
			}
			if (this.SetQuestState)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestAction(quest.GUID.ToString(), this.QuestState);
			}
			if (this.SetQuestEntryState)
			{
				NetworkSingleton<QuestManager>.Instance.SendQuestEntryState(quest.GUID.ToString(), this.QuestEntryIndex, this.QuestEntryState);
			}
		}

		// Token: 0x0400111B RID: 4379
		public string QuestName;

		// Token: 0x0400111C RID: 4380
		public bool SetQuestState;

		// Token: 0x0400111D RID: 4381
		public QuestManager.EQuestAction QuestState;

		// Token: 0x0400111E RID: 4382
		public bool SetQuestEntryState;

		// Token: 0x0400111F RID: 4383
		public int QuestEntryIndex;

		// Token: 0x04001120 RID: 4384
		public EQuestState QuestEntryState;
	}
}
