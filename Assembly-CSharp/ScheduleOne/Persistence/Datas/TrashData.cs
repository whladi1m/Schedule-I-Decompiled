using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042D RID: 1069
	[Serializable]
	public class TrashData : SaveData
	{
		// Token: 0x06001594 RID: 5524 RVA: 0x0005FBC6 File Offset: 0x0005DDC6
		public TrashData(TrashItemData[] trash)
		{
			this.Items = trash;
		}

		// Token: 0x0400145F RID: 5215
		public TrashItemData[] Items;
	}
}
