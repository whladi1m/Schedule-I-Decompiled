using System;

namespace ScheduleOne.Product
{
	// Token: 0x020008D2 RID: 2258
	[Serializable]
	public class NewMixOperation
	{
		// Token: 0x06003D06 RID: 15622 RVA: 0x00100369 File Offset: 0x000FE569
		public NewMixOperation(string productID, string ingredientID)
		{
			this.ProductID = productID;
			this.IngredientID = ingredientID;
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x0000494F File Offset: 0x00002B4F
		public NewMixOperation()
		{
		}

		// Token: 0x04002C26 RID: 11302
		public string ProductID;

		// Token: 0x04002C27 RID: 11303
		public string IngredientID;
	}
}
