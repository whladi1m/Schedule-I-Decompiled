using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Packaging;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Properties;
using ScheduleOne.StationFramework;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008D3 RID: 2259
	[CreateAssetMenu(fileName = "ProductDefinition", menuName = "ScriptableObjects/ProductDefinition", order = 1)]
	[Serializable]
	public class ProductDefinition : PropertyItemDefinition, ISaveable
	{
		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06003D08 RID: 15624 RVA: 0x0010037F File Offset: 0x000FE57F
		public EDrugType DrugType
		{
			get
			{
				return this.DrugTypes[0].DrugType;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06003D09 RID: 15625 RVA: 0x00100392 File Offset: 0x000FE592
		public float Price
		{
			get
			{
				return NetworkSingleton<ProductManager>.Instance.GetPrice(this);
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06003D0A RID: 15626 RVA: 0x0010039F File Offset: 0x000FE59F
		// (set) Token: 0x06003D0B RID: 15627 RVA: 0x001003A7 File Offset: 0x000FE5A7
		public List<StationRecipe> Recipes { get; private set; } = new List<StationRecipe>();

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06003D0C RID: 15628 RVA: 0x001003B0 File Offset: 0x000FE5B0
		public string SaveFolderName
		{
			get
			{
				return SaveManager.SanitizeFileName(this.ID);
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06003D0D RID: 15629 RVA: 0x001003B0 File Offset: 0x000FE5B0
		public string SaveFileName
		{
			get
			{
				return SaveManager.SanitizeFileName(this.ID);
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06003D0E RID: 15630 RVA: 0x0004691A File Offset: 0x00044B1A
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06003D0F RID: 15631 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06003D10 RID: 15632 RVA: 0x001003BD File Offset: 0x000FE5BD
		// (set) Token: 0x06003D11 RID: 15633 RVA: 0x001003C5 File Offset: 0x000FE5C5
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06003D12 RID: 15634 RVA: 0x001003CE File Offset: 0x000FE5CE
		// (set) Token: 0x06003D13 RID: 15635 RVA: 0x001003D6 File Offset: 0x000FE5D6
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06003D14 RID: 15636 RVA: 0x001003DF File Offset: 0x000FE5DF
		// (set) Token: 0x06003D15 RID: 15637 RVA: 0x001003E7 File Offset: 0x000FE5E7
		public bool HasChanged { get; set; }

		// Token: 0x06003D16 RID: 15638 RVA: 0x001003F0 File Offset: 0x000FE5F0
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new ProductItemInstance(this, quantity, EQuality.Standard, null);
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x001003FB File Offset: 0x000FE5FB
		public void OnValidate()
		{
			this.MarketValue = ProductManager.CalculateProductValue(this, this.BasePrice);
			this.CleanRecipes();
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x00100418 File Offset: 0x000FE618
		public void Initialize(List<Property> properties, List<EDrugType> drugTypes)
		{
			base.Initialize(properties);
			this.DrugTypes = new List<DrugTypeContainer>();
			for (int i = 0; i < drugTypes.Count; i++)
			{
				this.DrugTypes.Add(new DrugTypeContainer
				{
					DrugType = drugTypes[i]
				});
			}
			this.CleanRecipes();
			this.MarketValue = ProductManager.CalculateProductValue(this, this.BasePrice);
			this.InitializeSaveable();
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x00100484 File Offset: 0x000FE684
		public float GetAddictiveness()
		{
			float num = this.BaseAddictiveness;
			for (int i = 0; i < this.Properties.Count; i++)
			{
				num += this.Properties[i].Addictiveness;
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x001004C8 File Offset: 0x000FE6C8
		public void CleanRecipes()
		{
			for (int i = this.Recipes.Count - 1; i >= 0; i--)
			{
				if (this.Recipes[i] == null)
				{
					this.Recipes.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x0010050D File Offset: 0x000FE70D
		public void AddRecipe(StationRecipe recipe)
		{
			if (recipe.Product.Item != this)
			{
				Debug.LogError("Recipe product does not match this product.");
				return;
			}
			if (!this.Recipes.Contains(recipe))
			{
				this.Recipes.Add(recipe);
			}
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00100548 File Offset: 0x000FE748
		public virtual string GetSaveString()
		{
			string[] array = new string[this.Properties.Count];
			for (int i = 0; i < this.Properties.Count; i++)
			{
				array[i] = this.Properties[i].ID;
			}
			return new ProductData(this.Name, this.ID, this.DrugTypes[0].DrugType, array).GetJson(true);
		}

		// Token: 0x04002C28 RID: 11304
		[Header("Product Settings")]
		public List<DrugTypeContainer> DrugTypes;

		// Token: 0x04002C29 RID: 11305
		public float LawIntensityChange = 1f;

		// Token: 0x04002C2A RID: 11306
		public float BasePrice = 1f;

		// Token: 0x04002C2B RID: 11307
		public float MarketValue = 1f;

		// Token: 0x04002C2C RID: 11308
		public FunctionalProduct FunctionalProduct;

		// Token: 0x04002C2D RID: 11309
		public int EffectsDuration = 180;

		// Token: 0x04002C2E RID: 11310
		[Range(0f, 1f)]
		public float BaseAddictiveness;

		// Token: 0x04002C2F RID: 11311
		[Header("Packaging that can be applied to this product. MUST BE ORDERED FROm LOWEST TO HIGHEST QUANTITY")]
		public PackagingDefinition[] ValidPackaging;
	}
}
