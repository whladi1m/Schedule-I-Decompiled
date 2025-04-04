using System;
using FishNet.Serializing.Helping;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Stations;
using UnityEngine;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000B99 RID: 2969
	public class ChemistryCookOperation
	{
		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x060050B4 RID: 20660 RVA: 0x00154350 File Offset: 0x00152550
		[CodegenExclude]
		public StationRecipe Recipe
		{
			get
			{
				if (this.recipe == null)
				{
					this.recipe = Singleton<ChemistryStationCanvas>.Instance.Recipes.Find((StationRecipe r) => r.RecipeID == this.RecipeID);
				}
				return this.recipe;
			}
		}

		// Token: 0x060050B5 RID: 20661 RVA: 0x00154387 File Offset: 0x00152587
		public ChemistryCookOperation(StationRecipe recipe, EQuality productQuality, Color startLiquidColor, float liquidLevel, int currentTime = 0)
		{
			this.RecipeID = recipe.RecipeID;
			this.ProductQuality = productQuality;
			this.StartLiquidColor = startLiquidColor;
			this.LiquidLevel = liquidLevel;
			this.CurrentTime = currentTime;
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x001543B9 File Offset: 0x001525B9
		public ChemistryCookOperation(string recipeID, EQuality productQuality, Color startLiquidColor, float liquidLevel, int currentTime = 0)
		{
			this.RecipeID = recipeID;
			this.ProductQuality = productQuality;
			this.StartLiquidColor = startLiquidColor;
			this.LiquidLevel = liquidLevel;
			this.CurrentTime = currentTime;
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x0000494F File Offset: 0x00002B4F
		public ChemistryCookOperation()
		{
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x001543E6 File Offset: 0x001525E6
		public void Progress(int mins)
		{
			this.CurrentTime += mins;
			int currentTime = this.CurrentTime;
			int cookTime_Mins = this.Recipe.CookTime_Mins;
		}

		// Token: 0x04003CAE RID: 15534
		[CodegenExclude]
		private StationRecipe recipe;

		// Token: 0x04003CAF RID: 15535
		public string RecipeID;

		// Token: 0x04003CB0 RID: 15536
		public EQuality ProductQuality;

		// Token: 0x04003CB1 RID: 15537
		public Color StartLiquidColor;

		// Token: 0x04003CB2 RID: 15538
		public float LiquidLevel;

		// Token: 0x04003CB3 RID: 15539
		public int CurrentTime;
	}
}
