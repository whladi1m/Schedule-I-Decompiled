using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.UI.Tooltips;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.ProductManagerApp
{
	// Token: 0x02000A99 RID: 2713
	public class ProductAppDetailPanel : MonoBehaviour
	{
		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06004917 RID: 18711 RVA: 0x00131A45 File Offset: 0x0012FC45
		// (set) Token: 0x06004918 RID: 18712 RVA: 0x00131A4D File Offset: 0x0012FC4D
		public ProductDefinition ActiveProduct { get; protected set; }

		// Token: 0x06004919 RID: 18713 RVA: 0x00131A56 File Offset: 0x0012FC56
		public void Awake()
		{
			this.ListedForSale.onValueChanged.AddListener(delegate(bool value)
			{
				this.ListingToggled();
			});
			this.ValueLabel.onEndEdit.AddListener(delegate(string value)
			{
				this.PriceSubmitted(value);
			});
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x00131A90 File Offset: 0x0012FC90
		public void SetActiveProduct(ProductDefinition productDefinition)
		{
			this.ActiveProduct = productDefinition;
			bool flag = ProductManager.DiscoveredProducts.Contains(productDefinition);
			if (this.ActiveProduct != null)
			{
				this.NameLabel.text = productDefinition.Name;
				this.SuggestedPriceLabel.text = "Suggested: " + MoneyManager.FormatAmount(productDefinition.MarketValue, false, false);
				this.UpdatePrice();
				if (flag)
				{
					this.DescLabel.text = productDefinition.Description;
				}
				else
				{
					this.DescLabel.text = "???";
				}
				for (int i = 0; i < this.PropertyLabels.Length; i++)
				{
					if (productDefinition.Properties.Count > i)
					{
						this.PropertyLabels[i].text = "•  " + productDefinition.Properties[i].Name;
						this.PropertyLabels[i].color = productDefinition.Properties[i].LabelColor;
						this.PropertyLabels[i].gameObject.SetActive(true);
					}
					else
					{
						this.PropertyLabels[i].gameObject.SetActive(false);
					}
				}
				for (int j = 0; j < this.RecipeEntries.Length; j++)
				{
					if (productDefinition.Recipes.Count > j)
					{
						this.RecipeEntries[j].gameObject.SetActive(true);
						if (productDefinition.Recipes[j].Ingredients[0].Item is ProductDefinition)
						{
							this.RecipeEntries[j].Find("Product").GetComponent<Image>().sprite = productDefinition.Recipes[j].Ingredients[0].Item.Icon;
							this.RecipeEntries[j].Find("Product").GetComponent<Tooltip>().text = productDefinition.Recipes[j].Ingredients[0].Item.Name;
							this.RecipeEntries[j].Find("Mixer").GetComponent<Image>().sprite = productDefinition.Recipes[j].Ingredients[1].Item.Icon;
							this.RecipeEntries[j].Find("Mixer").GetComponent<Tooltip>().text = productDefinition.Recipes[j].Ingredients[1].Item.Name;
						}
						else
						{
							this.RecipeEntries[j].Find("Product").GetComponent<Image>().sprite = productDefinition.Recipes[j].Ingredients[1].Item.Icon;
							this.RecipeEntries[j].Find("Product").GetComponent<Tooltip>().text = productDefinition.Recipes[j].Ingredients[1].Item.Name;
							this.RecipeEntries[j].Find("Mixer").GetComponent<Image>().sprite = productDefinition.Recipes[j].Ingredients[0].Item.Icon;
							this.RecipeEntries[j].Find("Mixer").GetComponent<Tooltip>().text = productDefinition.Recipes[j].Ingredients[0].Item.Name;
						}
						this.RecipeEntries[j].Find("Output").GetComponent<Image>().sprite = productDefinition.Icon;
						this.RecipeEntries[j].Find("Output").GetComponent<Tooltip>().text = productDefinition.Name;
					}
					else
					{
						this.RecipeEntries[j].gameObject.SetActive(false);
					}
				}
				this.RecipesLabel.gameObject.SetActive(productDefinition.Recipes.Count > 0);
				this.NothingSelected.gameObject.SetActive(false);
				this.Container.gameObject.SetActive(true);
				this.AddictionSlider.value = productDefinition.GetAddictiveness();
				this.AddictionLabel.text = Mathf.FloorToInt(productDefinition.GetAddictiveness() * 100f).ToString() + "%";
				this.AddictionLabel.color = Color.Lerp(this.AddictionColor_Min, this.AddictionColor_Max, productDefinition.GetAddictiveness());
				ContentSizeFitter[] componentsInChildren = base.GetComponentsInChildren<ContentSizeFitter>();
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					componentsInChildren[k].enabled = false;
					componentsInChildren[k].enabled = true;
				}
				this.LayoutGroup.enabled = false;
				this.LayoutGroup.enabled = true;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.LayoutGroup.GetComponent<RectTransform>());
				this.ScrollRect.enabled = false;
				this.ScrollRect.enabled = true;
				this.ScrollRect.verticalNormalizedPosition = 1f;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.ScrollRect.GetComponent<RectTransform>());
			}
			else
			{
				this.NothingSelected.gameObject.SetActive(true);
				this.Container.gameObject.SetActive(false);
			}
			this.UpdateListed();
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x00131FB7 File Offset: 0x001301B7
		private void Update()
		{
			if (PlayerSingleton<ProductManagerApp>.Instance.isOpen)
			{
				this.UpdateListed();
			}
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x00131FCB File Offset: 0x001301CB
		private void UpdateListed()
		{
			this.ListedForSale.SetIsOnWithoutNotify(ProductManager.ListedProducts.Contains(this.ActiveProduct));
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x00131FE8 File Offset: 0x001301E8
		private void UpdatePrice()
		{
			this.ValueLabel.SetTextWithoutNotify(NetworkSingleton<ProductManager>.Instance.GetPrice(this.ActiveProduct).ToString());
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x00132018 File Offset: 0x00130218
		private void ListingToggled()
		{
			if (!NetworkSingleton<ProductManager>.InstanceExists)
			{
				return;
			}
			if (this.ActiveProduct == null)
			{
				return;
			}
			if (ProductManager.ListedProducts.Contains(this.ActiveProduct))
			{
				NetworkSingleton<ProductManager>.Instance.SetProductListed(this.ActiveProduct.ID, false);
			}
			else
			{
				NetworkSingleton<ProductManager>.Instance.SetProductListed(this.ActiveProduct.ID, true);
			}
			Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
			this.UpdateListed();
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x0013208C File Offset: 0x0013028C
		private void PriceSubmitted(string value)
		{
			if (!NetworkSingleton<ProductManager>.InstanceExists)
			{
				return;
			}
			if (!PlayerSingleton<ProductManagerApp>.Instance.isOpen)
			{
				return;
			}
			if (!PlayerSingleton<Phone>.Instance.IsOpen)
			{
				return;
			}
			if (this.ActiveProduct == null)
			{
				return;
			}
			float value2;
			if (float.TryParse(value, out value2))
			{
				NetworkSingleton<ProductManager>.Instance.SendPrice(this.ActiveProduct.ID, value2);
				Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
			}
			this.UpdatePrice();
		}

		// Token: 0x04003678 RID: 13944
		public Color AddictionColor_Min;

		// Token: 0x04003679 RID: 13945
		public Color AddictionColor_Max;

		// Token: 0x0400367A RID: 13946
		[Header("References")]
		public GameObject NothingSelected;

		// Token: 0x0400367B RID: 13947
		public GameObject Container;

		// Token: 0x0400367C RID: 13948
		public Text NameLabel;

		// Token: 0x0400367D RID: 13949
		public InputField ValueLabel;

		// Token: 0x0400367E RID: 13950
		public Text SuggestedPriceLabel;

		// Token: 0x0400367F RID: 13951
		public Toggle ListedForSale;

		// Token: 0x04003680 RID: 13952
		public Text DescLabel;

		// Token: 0x04003681 RID: 13953
		public Text[] PropertyLabels;

		// Token: 0x04003682 RID: 13954
		public RectTransform Listed;

		// Token: 0x04003683 RID: 13955
		public RectTransform Delisted;

		// Token: 0x04003684 RID: 13956
		public RectTransform NotDiscovered;

		// Token: 0x04003685 RID: 13957
		public RectTransform RecipesLabel;

		// Token: 0x04003686 RID: 13958
		public RectTransform[] RecipeEntries;

		// Token: 0x04003687 RID: 13959
		public VerticalLayoutGroup LayoutGroup;

		// Token: 0x04003688 RID: 13960
		public Scrollbar AddictionSlider;

		// Token: 0x04003689 RID: 13961
		public Text AddictionLabel;

		// Token: 0x0400368A RID: 13962
		public ScrollRect ScrollRect;
	}
}
