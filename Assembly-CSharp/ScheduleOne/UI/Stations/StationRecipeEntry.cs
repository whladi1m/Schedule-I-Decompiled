using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.ItemFramework;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000A45 RID: 2629
	public class StationRecipeEntry : MonoBehaviour
	{
		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060046DE RID: 18142 RVA: 0x00128E0D File Offset: 0x0012700D
		// (set) Token: 0x060046DF RID: 18143 RVA: 0x00128E15 File Offset: 0x00127015
		public bool IsValid { get; private set; }

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060046E0 RID: 18144 RVA: 0x00128E1E File Offset: 0x0012701E
		// (set) Token: 0x060046E1 RID: 18145 RVA: 0x00128E26 File Offset: 0x00127026
		public StationRecipe Recipe { get; private set; }

		// Token: 0x060046E2 RID: 18146 RVA: 0x00128E30 File Offset: 0x00127030
		public void AssignRecipe(StationRecipe recipe)
		{
			this.Recipe = recipe;
			this.Icon.sprite = recipe.Product.Item.Icon;
			this.TitleLabel.text = recipe.RecipeTitle;
			if (recipe.Product.Quantity > 1)
			{
				this.TitleLabel.text = this.TitleLabel.text + "(" + recipe.Product.Quantity.ToString() + "x)";
			}
			this.Icon.GetComponent<ItemDefinitionInfoHoverable>().AssignedItem = recipe.Product.Item;
			int num = recipe.CookTime_Mins / 60;
			int num2 = recipe.CookTime_Mins % 60;
			this.CookingTimeLabel.text = string.Format("{0}h", num);
			if (num2 > 0)
			{
				TextMeshProUGUI cookingTimeLabel = this.CookingTimeLabel;
				cookingTimeLabel.text += string.Format(" {0}m", num2);
			}
			this.IngredientQuantities = new TextMeshProUGUI[this.IngredientRects.Length];
			for (int i = 0; i < this.IngredientRects.Length; i++)
			{
				if (i < recipe.Ingredients.Count)
				{
					this.IngredientRects[i].Find("Icon").GetComponent<Image>().sprite = recipe.Ingredients[i].Item.Icon;
					this.IngredientQuantities[i] = this.IngredientRects[i].Find("Quantity").GetComponent<TextMeshProUGUI>();
					this.IngredientQuantities[i].text = recipe.Ingredients[i].Quantity.ToString() + "x";
					this.IngredientRects[i].GetComponent<ItemDefinitionInfoHoverable>().AssignedItem = recipe.Ingredients[i].Item;
					this.IngredientRects[i].gameObject.SetActive(true);
				}
				else
				{
					this.IngredientRects[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x00129030 File Offset: 0x00127230
		public void RefreshValidity(List<ItemInstance> ingredients)
		{
			if (!this.Recipe.Unlocked)
			{
				this.IsValid = false;
				base.gameObject.SetActive(false);
				return;
			}
			this.IsValid = true;
			for (int i = 0; i < this.Recipe.Ingredients.Count; i++)
			{
				List<ItemInstance> list = new List<ItemInstance>();
				using (List<ItemDefinition>.Enumerator enumerator = this.Recipe.Ingredients[i].Items.GetEnumerator())
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
				if (num >= this.Recipe.Ingredients[i].Quantity)
				{
					this.IngredientQuantities[i].color = StationRecipeEntry.ValidColor;
				}
				else
				{
					this.IngredientQuantities[i].color = StationRecipeEntry.InvalidColor;
					this.IsValid = false;
				}
			}
			base.gameObject.SetActive(true);
			this.Button.interactable = this.IsValid;
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x00129194 File Offset: 0x00127394
		public float GetIngredientsMatchDelta(List<ItemInstance> ingredients)
		{
			int num = this.Recipe.Ingredients.Sum((StationRecipe.IngredientQuantity x) => x.Quantity);
			int num2 = 0;
			for (int i = 0; i < this.Recipe.Ingredients.Count; i++)
			{
				List<ItemInstance> list = new List<ItemInstance>();
				using (List<ItemDefinition>.Enumerator enumerator = this.Recipe.Ingredients[i].Items.GetEnumerator())
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
				int num3 = 0;
				for (int j = 0; j < list.Count; j++)
				{
					num3 += list[j].Quantity;
				}
				num2 += Mathf.Min(num3, this.Recipe.Ingredients[i].Quantity);
			}
			return (float)num2 / (float)num;
		}

		// Token: 0x04003499 RID: 13465
		public static Color ValidColor = Color.white;

		// Token: 0x0400349A RID: 13466
		public static Color InvalidColor = new Color32(byte.MaxValue, 80, 80, byte.MaxValue);

		// Token: 0x0400349B RID: 13467
		public Button Button;

		// Token: 0x0400349C RID: 13468
		public Image Icon;

		// Token: 0x0400349D RID: 13469
		public TextMeshProUGUI TitleLabel;

		// Token: 0x0400349E RID: 13470
		public TextMeshProUGUI CookingTimeLabel;

		// Token: 0x0400349F RID: 13471
		public RectTransform[] IngredientRects;

		// Token: 0x040034A0 RID: 13472
		private TextMeshProUGUI[] IngredientQuantities;
	}
}
