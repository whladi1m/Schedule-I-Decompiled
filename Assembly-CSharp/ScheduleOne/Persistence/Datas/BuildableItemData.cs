using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200040C RID: 1036
	[Serializable]
	public class BuildableItemData : SaveData
	{
		// Token: 0x0600156C RID: 5484 RVA: 0x0005F584 File Offset: 0x0005D784
		public BuildableItemData(Guid guid, ItemInstance item, int loadOrder)
		{
			this.GUID = guid.ToString();
			this.ItemString = item.GetItemData().GetJson(true);
			this.LoadOrder = loadOrder;
		}

		// Token: 0x040013F2 RID: 5106
		public string GUID;

		// Token: 0x040013F3 RID: 5107
		public string ItemString;

		// Token: 0x040013F4 RID: 5108
		public int LoadOrder;
	}
}
