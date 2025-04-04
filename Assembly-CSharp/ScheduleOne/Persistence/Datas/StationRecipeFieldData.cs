using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003FD RID: 1021
	[Serializable]
	public class StationRecipeFieldData
	{
		// Token: 0x0600155C RID: 5468 RVA: 0x0005F29E File Offset: 0x0005D49E
		public StationRecipeFieldData(string recipeID)
		{
			this.RecipeID = recipeID;
		}

		// Token: 0x040013C1 RID: 5057
		public string RecipeID;
	}
}
