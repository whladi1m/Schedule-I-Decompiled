using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E6 RID: 998
	[Serializable]
	public class WeedData : ProductItemData
	{
		// Token: 0x06001544 RID: 5444 RVA: 0x0005F043 File Offset: 0x0005D243
		public WeedData(string iD, int quantity, string quality, string packagingID) : base(iD, quantity, quality, packagingID)
		{
		}
	}
}
