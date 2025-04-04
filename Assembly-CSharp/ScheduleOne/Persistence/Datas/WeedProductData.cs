using System;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000420 RID: 1056
	[Serializable]
	public class WeedProductData : ProductData
	{
		// Token: 0x06001581 RID: 5505 RVA: 0x0005F8FF File Offset: 0x0005DAFF
		public WeedProductData(string name, string id, EDrugType drugType, string[] properties, WeedAppearanceSettings appearanceSettings) : base(name, id, drugType, properties)
		{
			this.AppearanceSettings = appearanceSettings;
		}

		// Token: 0x04001431 RID: 5169
		public WeedAppearanceSettings AppearanceSettings;
	}
}
