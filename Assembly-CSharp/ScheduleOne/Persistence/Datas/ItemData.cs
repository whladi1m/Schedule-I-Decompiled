using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003DE RID: 990
	[Serializable]
	public class ItemData : SaveData
	{
		// Token: 0x0600153C RID: 5436 RVA: 0x0005F061 File Offset: 0x0005D261
		public ItemData(string iD, int quantity)
		{
			this.ID = iD;
			this.Quantity = quantity;
		}

		// Token: 0x04001391 RID: 5009
		public string ID;

		// Token: 0x04001392 RID: 5010
		public int Quantity;
	}
}
