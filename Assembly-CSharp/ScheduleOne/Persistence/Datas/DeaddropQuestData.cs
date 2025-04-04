using System;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000424 RID: 1060
	[Serializable]
	public class DeaddropQuestData : QuestData
	{
		// Token: 0x06001585 RID: 5509 RVA: 0x0005F9D0 File Offset: 0x0005DBD0
		public DeaddropQuestData(string guid, EQuestState state, bool isTracked, string title, string desc, bool isTimed, GameDateTimeData expiry, QuestEntryData[] entries, string deaddropGUID) : base(guid, state, isTracked, title, desc, isTimed, expiry, entries)
		{
			this.DeaddropGUID = deaddropGUID;
		}

		// Token: 0x04001444 RID: 5188
		public string DeaddropGUID;
	}
}
