using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Product;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000421 RID: 1057
	[Serializable]
	public class ProductManagerData : SaveData
	{
		// Token: 0x06001582 RID: 5506 RVA: 0x0005F914 File Offset: 0x0005DB14
		public ProductManagerData(string[] discoveredProducts, string[] listedProducts, NewMixOperation activeOperation, bool isMixComplete, MixRecipeData[] mixRecipes, StringIntPair[] productPrices, string[] favouritedProducts)
		{
			this.DiscoveredProducts = discoveredProducts;
			this.ListedProducts = listedProducts;
			this.ActiveMixOperation = activeOperation;
			this.IsMixComplete = isMixComplete;
			this.MixRecipes = mixRecipes;
			this.ProductPrices = productPrices;
			this.FavouritedProducts = favouritedProducts;
		}

		// Token: 0x04001432 RID: 5170
		public string[] DiscoveredProducts;

		// Token: 0x04001433 RID: 5171
		public string[] ListedProducts;

		// Token: 0x04001434 RID: 5172
		public NewMixOperation ActiveMixOperation;

		// Token: 0x04001435 RID: 5173
		public bool IsMixComplete;

		// Token: 0x04001436 RID: 5174
		public MixRecipeData[] MixRecipes;

		// Token: 0x04001437 RID: 5175
		public StringIntPair[] ProductPrices;

		// Token: 0x04001438 RID: 5176
		public string[] FavouritedProducts;
	}
}
