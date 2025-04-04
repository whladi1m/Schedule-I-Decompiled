using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E1 RID: 993
	[Serializable]
	public class ProductItemData : QualityItemData
	{
		// Token: 0x0600153F RID: 5439 RVA: 0x0005F077 File Offset: 0x0005D277
		public ProductItemData(string iD, int quantity, string quality, string packagingID) : base(iD, quantity, quality)
		{
			this.PackagingID = packagingID;
		}

		// Token: 0x04001393 RID: 5011
		public string PackagingID;
	}
}
