using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000411 RID: 1041
	public class LabOvenData : GridItemData
	{
		// Token: 0x06001571 RID: 5489 RVA: 0x0005F6B8 File Offset: 0x0005D8B8
		public LabOvenData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet inputContents, ItemSet outputContents, string ingredientID, int currentIngredientQuantity, EQuality ingredientQuality, string productID, int currentCookProgress) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.InputContents = inputContents;
			this.OutputContents = outputContents;
			this.CurrentIngredientID = ingredientID;
			this.CurrentIngredientQuantity = currentIngredientQuantity;
			this.CurrentIngredientQuality = ingredientQuality;
			this.CurrentProductID = productID;
			this.CurrentCookProgress = currentCookProgress;
		}

		// Token: 0x04001407 RID: 5127
		public ItemSet InputContents;

		// Token: 0x04001408 RID: 5128
		public ItemSet OutputContents;

		// Token: 0x04001409 RID: 5129
		public string CurrentIngredientID;

		// Token: 0x0400140A RID: 5130
		public int CurrentIngredientQuantity;

		// Token: 0x0400140B RID: 5131
		public EQuality CurrentIngredientQuality;

		// Token: 0x0400140C RID: 5132
		public string CurrentProductID;

		// Token: 0x0400140D RID: 5133
		public int CurrentCookProgress;
	}
}
