using System;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200041F RID: 1055
	[Serializable]
	public class ProductData : SaveData
	{
		// Token: 0x06001580 RID: 5504 RVA: 0x0005F8DA File Offset: 0x0005DADA
		public ProductData(string name, string id, EDrugType drugType, string[] properties)
		{
			this.Name = name;
			this.ID = id;
			this.DrugType = drugType;
			this.Properties = properties;
		}

		// Token: 0x0400142D RID: 5165
		public string Name;

		// Token: 0x0400142E RID: 5166
		public string ID;

		// Token: 0x0400142F RID: 5167
		public EDrugType DrugType;

		// Token: 0x04001430 RID: 5168
		public string[] Properties;
	}
}
