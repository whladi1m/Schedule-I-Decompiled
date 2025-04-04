using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.StationFramework
{
	// Token: 0x020008B8 RID: 2232
	[CreateAssetMenu(fileName = "StationRecipe", menuName = "StationFramework/StationRecipe", order = 1)]
	[Serializable]
	public class StationRecipe : ScriptableObject
	{
		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06003CB1 RID: 15537 RVA: 0x000FED56 File Offset: 0x000FCF56
		public float CookTemperatureLowerBound
		{
			get
			{
				return this.CookTemperature - this.CookTemperatureTolerance;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06003CB2 RID: 15538 RVA: 0x000FED65 File Offset: 0x000FCF65
		public float CookTemperatureUpperBound
		{
			get
			{
				return this.CookTemperature + this.CookTemperatureTolerance;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x000FED74 File Offset: 0x000FCF74
		public string RecipeID
		{
			get
			{
				return this.Product.Quantity.ToString() + "x" + this.Product.Item.ID;
			}
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x000FEDA0 File Offset: 0x000FCFA0
		public StorableItemInstance GetProductInstance(List<ItemInstance> ingredients)
		{
			StorableItemInstance storableItemInstance = this.Product.Item.GetDefaultInstance(this.Product.Quantity) as StorableItemInstance;
			if (storableItemInstance is QualityItemInstance)
			{
				EQuality quality = this.CalculateQuality(ingredients);
				(storableItemInstance as QualityItemInstance).Quality = quality;
			}
			return storableItemInstance;
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x000FEDEC File Offset: 0x000FCFEC
		public StorableItemInstance GetProductInstance(EQuality quality)
		{
			StorableItemInstance storableItemInstance = this.Product.Item.GetDefaultInstance(this.Product.Quantity) as StorableItemInstance;
			if (storableItemInstance is QualityItemInstance)
			{
				(storableItemInstance as QualityItemInstance).Quality = quality;
			}
			return storableItemInstance;
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x000FEE30 File Offset: 0x000FD030
		public bool DoIngredientsSuffice(List<ItemInstance> ingredients)
		{
			for (int i = 0; i < this.Ingredients.Count; i++)
			{
				List<ItemInstance> list = new List<ItemInstance>();
				using (List<ItemDefinition>.Enumerator enumerator = this.Ingredients[i].Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemDefinition ingredientVariant = enumerator.Current;
						List<ItemInstance> collection = (from x in ingredients
						where x.ID == ingredientVariant.ID
						select x).ToList<ItemInstance>();
						list.AddRange(collection);
					}
				}
				int num = 0;
				for (int j = 0; j < list.Count; j++)
				{
					num += list[j].Quantity;
				}
				if (num < this.Ingredients[i].Quantity)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x000FEF18 File Offset: 0x000FD118
		public EQuality CalculateQuality(List<ItemInstance> ingredients)
		{
			EQuality result = EQuality.Standard;
			if (this.QualityCalculationMethod == StationRecipe.EQualityCalculationMethod.Additive)
			{
				int num = 0;
				for (int i = 0; i < ingredients.Count; i++)
				{
					if (ingredients[i] is QualityItemInstance)
					{
						switch ((ingredients[i] as QualityItemInstance).Quality)
						{
						case EQuality.Trash:
							num -= 2;
							break;
						case EQuality.Poor:
							num--;
							break;
						case EQuality.Standard:
							num = num;
							break;
						case EQuality.Premium:
							num++;
							break;
						case EQuality.Heavenly:
							num += 2;
							break;
						}
					}
				}
				if ((float)num <= -2f)
				{
					result = EQuality.Trash;
				}
				else if ((float)num == -1f)
				{
					result = EQuality.Poor;
				}
				else if ((float)num == 0f)
				{
					result = EQuality.Standard;
				}
				else if ((float)num == 1f)
				{
					result = EQuality.Premium;
				}
				else if ((float)num >= 2f)
				{
					result = EQuality.Heavenly;
				}
			}
			return result;
		}

		// Token: 0x04002BCF RID: 11215
		[HideInInspector]
		public bool IsDiscovered;

		// Token: 0x04002BD0 RID: 11216
		public string RecipeTitle;

		// Token: 0x04002BD1 RID: 11217
		public bool Unlocked;

		// Token: 0x04002BD2 RID: 11218
		public List<StationRecipe.IngredientQuantity> Ingredients = new List<StationRecipe.IngredientQuantity>();

		// Token: 0x04002BD3 RID: 11219
		public StationRecipe.ItemQuantity Product;

		// Token: 0x04002BD4 RID: 11220
		public Color FinalLiquidColor = Color.white;

		// Token: 0x04002BD5 RID: 11221
		[Tooltip("The time it takes to cook this recipe in minutes")]
		public int CookTime_Mins = 180;

		// Token: 0x04002BD6 RID: 11222
		[Tooltip("The temperature at which this recipe should be cooked")]
		[Range(0f, 500f)]
		public float CookTemperature = 250f;

		// Token: 0x04002BD7 RID: 11223
		[Range(0f, 100f)]
		public float CookTemperatureTolerance = 25f;

		// Token: 0x04002BD8 RID: 11224
		public StationRecipe.EQualityCalculationMethod QualityCalculationMethod;

		// Token: 0x020008B9 RID: 2233
		public enum EQualityCalculationMethod
		{
			// Token: 0x04002BDA RID: 11226
			Additive
		}

		// Token: 0x020008BA RID: 2234
		[Serializable]
		public class ItemQuantity
		{
			// Token: 0x04002BDB RID: 11227
			public ItemDefinition Item;

			// Token: 0x04002BDC RID: 11228
			public int Quantity = 1;
		}

		// Token: 0x020008BB RID: 2235
		[Serializable]
		public class IngredientQuantity
		{
			// Token: 0x1700088E RID: 2190
			// (get) Token: 0x06003CBA RID: 15546 RVA: 0x000FF029 File Offset: 0x000FD229
			public ItemDefinition Item
			{
				get
				{
					return this.Items.FirstOrDefault<ItemDefinition>();
				}
			}

			// Token: 0x04002BDD RID: 11229
			public List<ItemDefinition> Items = new List<ItemDefinition>();

			// Token: 0x04002BDE RID: 11230
			public int Quantity = 1;
		}
	}
}
