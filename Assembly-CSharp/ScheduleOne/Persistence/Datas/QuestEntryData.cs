using System;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000426 RID: 1062
	[Serializable]
	public class QuestEntryData : SaveData
	{
		// Token: 0x06001587 RID: 5511 RVA: 0x0005FA48 File Offset: 0x0005DC48
		public QuestEntryData(string name, EQuestState state)
		{
			this.Name = name;
			this.State = state;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0005ECD4 File Offset: 0x0005CED4
		public QuestEntryData()
		{
		}

		// Token: 0x0400144D RID: 5197
		public string Name;

		// Token: 0x0400144E RID: 5198
		public EQuestState State;
	}
}
