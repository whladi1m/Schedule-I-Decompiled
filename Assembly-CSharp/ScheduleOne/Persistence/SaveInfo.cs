using System;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037B RID: 891
	public class SaveInfo
	{
		// Token: 0x0600142E RID: 5166 RVA: 0x0005A17C File Offset: 0x0005837C
		public SaveInfo(string savePath, int saveSlotNumber, string organisationName, DateTime dateCreated, DateTime dateLastPlayed, float networth, string saveVersion, MetaData metaData)
		{
			this.SavePath = savePath;
			this.SaveSlotNumber = saveSlotNumber;
			this.OrganisationName = organisationName;
			this.DateCreated = dateCreated;
			this.DateLastPlayed = dateLastPlayed;
			this.Networth = networth;
			this.SaveVersion = saveVersion;
			this.MetaData = metaData;
		}

		// Token: 0x04001307 RID: 4871
		public string SavePath;

		// Token: 0x04001308 RID: 4872
		public int SaveSlotNumber;

		// Token: 0x04001309 RID: 4873
		public string OrganisationName;

		// Token: 0x0400130A RID: 4874
		public DateTime DateCreated;

		// Token: 0x0400130B RID: 4875
		public DateTime DateLastPlayed;

		// Token: 0x0400130C RID: 4876
		public float Networth;

		// Token: 0x0400130D RID: 4877
		public string SaveVersion;

		// Token: 0x0400130E RID: 4878
		public MetaData MetaData;
	}
}
