using System;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041E RID: 1054
	[Serializable]
	public class MethProductData : ProductData
	{
		// Token: 0x0600157F RID: 5503 RVA: 0x0005F8C5 File Offset: 0x0005DAC5
		public MethProductData(string name, string id, EDrugType drugType, string[] properties, MethAppearanceSettings appearanceSettings) : base(name, id, drugType, properties)
		{
			this.AppearanceSettings = appearanceSettings;
		}

		// Token: 0x0400142C RID: 5164
		public MethAppearanceSettings AppearanceSettings;
	}
}
