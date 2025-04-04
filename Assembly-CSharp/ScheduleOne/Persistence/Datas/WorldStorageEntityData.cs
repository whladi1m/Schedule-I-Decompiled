using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000432 RID: 1074
	[Serializable]
	public class WorldStorageEntityData : SaveData
	{
		// Token: 0x0600159A RID: 5530 RVA: 0x0005FC93 File Offset: 0x0005DE93
		public WorldStorageEntityData(Guid guid, ItemSet contents)
		{
			this.GUID = guid.ToString();
			this.Contents = contents;
		}

		// Token: 0x0400146E RID: 5230
		public string GUID;

		// Token: 0x0400146F RID: 5231
		public ItemSet Contents;
	}
}
