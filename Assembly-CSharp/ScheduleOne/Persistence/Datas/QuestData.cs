using System;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000425 RID: 1061
	[Serializable]
	public class QuestData : SaveData
	{
		// Token: 0x06001586 RID: 5510 RVA: 0x0005F9F8 File Offset: 0x0005DBF8
		public QuestData(string guid, EQuestState state, bool isTracked, string title, string desc, bool expires, GameDateTimeData expiry, QuestEntryData[] entries)
		{
			this.GUID = guid;
			this.State = state;
			this.IsTracked = isTracked;
			this.Title = title;
			this.Description = desc;
			this.Expires = expires;
			this.ExpiryDate = expiry;
			this.Entries = entries;
		}

		// Token: 0x04001445 RID: 5189
		public string GUID;

		// Token: 0x04001446 RID: 5190
		public EQuestState State;

		// Token: 0x04001447 RID: 5191
		public bool IsTracked;

		// Token: 0x04001448 RID: 5192
		public string Title;

		// Token: 0x04001449 RID: 5193
		public string Description;

		// Token: 0x0400144A RID: 5194
		public bool Expires;

		// Token: 0x0400144B RID: 5195
		public GameDateTimeData ExpiryDate;

		// Token: 0x0400144C RID: 5196
		public QuestEntryData[] Entries;
	}
}
