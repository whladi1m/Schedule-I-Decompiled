using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003FE RID: 1022
	[Serializable]
	public class MetaData : SaveData
	{
		// Token: 0x0600155D RID: 5469 RVA: 0x0005F2AD File Offset: 0x0005D4AD
		public MetaData(DateTimeData creationDate, DateTimeData lastPlayedDate, string creationVersion, string lastSaveVersion, bool playTutorial)
		{
			this.CreationDate = creationDate;
			this.LastPlayedDate = lastPlayedDate;
			this.CreationVersion = creationVersion;
			this.LastSaveVersion = lastSaveVersion;
			this.PlayTutorial = playTutorial;
		}

		// Token: 0x040013C2 RID: 5058
		public DateTimeData CreationDate;

		// Token: 0x040013C3 RID: 5059
		public DateTimeData LastPlayedDate;

		// Token: 0x040013C4 RID: 5060
		public string CreationVersion;

		// Token: 0x040013C5 RID: 5061
		public string LastSaveVersion;

		// Token: 0x040013C6 RID: 5062
		public bool PlayTutorial;
	}
}
