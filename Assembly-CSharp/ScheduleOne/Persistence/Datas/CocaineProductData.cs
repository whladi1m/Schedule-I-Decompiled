using System;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041D RID: 1053
	[Serializable]
	public class CocaineProductData : ProductData
	{
		// Token: 0x0600157E RID: 5502 RVA: 0x0005F8B0 File Offset: 0x0005DAB0
		public CocaineProductData(string name, string id, EDrugType drugType, string[] properties, CocaineAppearanceSettings appearanceSettings) : base(name, id, drugType, properties)
		{
			this.AppearanceSettings = appearanceSettings;
		}

		// Token: 0x0400142B RID: 5163
		public CocaineAppearanceSettings AppearanceSettings;
	}
}
