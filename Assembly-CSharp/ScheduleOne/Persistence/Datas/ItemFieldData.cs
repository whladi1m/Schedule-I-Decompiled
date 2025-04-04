using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F1 RID: 1009
	[Serializable]
	public class ItemFieldData
	{
		// Token: 0x06001550 RID: 5456 RVA: 0x0005F1B7 File Offset: 0x0005D3B7
		public ItemFieldData(string itemID)
		{
			this.ItemID = itemID;
		}

		// Token: 0x040013AE RID: 5038
		public string ItemID;
	}
}
