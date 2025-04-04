using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Product;
using ScheduleOne.Properties;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BB6 RID: 2998
	[Serializable]
	public class MixOperation
	{
		// Token: 0x06005364 RID: 21348 RVA: 0x0015F4CA File Offset: 0x0015D6CA
		public MixOperation(string productID, EQuality productQuality, string ingredientID, int quantity)
		{
			this.ProductID = productID;
			this.ProductQuality = productQuality;
			this.IngredientID = ingredientID;
			this.Quantity = quantity;
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x0000494F File Offset: 0x00002B4F
		public MixOperation()
		{
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x0015F4F0 File Offset: 0x0015D6F0
		public EDrugType GetOutput(out List<Property> properties)
		{
			ProductDefinition item = Registry.GetItem<ProductDefinition>(this.ProductID);
			PropertyItemDefinition item2 = Registry.GetItem<PropertyItemDefinition>(this.IngredientID);
			properties = PropertyMixCalculator.MixProperties(item.Properties, item2.Properties[0], item.DrugType);
			return item.DrugType;
		}

		// Token: 0x06005367 RID: 21351 RVA: 0x0015F53C File Offset: 0x0015D73C
		public bool IsOutputKnown(out ProductDefinition knownProduct)
		{
			List<Property> properties;
			EDrugType output = this.GetOutput(out properties);
			knownProduct = NetworkSingleton<ProductManager>.Instance.GetKnownProduct(output, properties);
			return knownProduct != null;
		}

		// Token: 0x04003E22 RID: 15906
		public string ProductID;

		// Token: 0x04003E23 RID: 15907
		public EQuality ProductQuality;

		// Token: 0x04003E24 RID: 15908
		public string IngredientID;

		// Token: 0x04003E25 RID: 15909
		public int Quantity;
	}
}
