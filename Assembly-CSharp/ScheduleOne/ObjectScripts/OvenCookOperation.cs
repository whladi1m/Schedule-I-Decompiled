using System;
using FishNet.Serializing.Helping;
using ScheduleOne.ItemFramework;
using ScheduleOne.StationFramework;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BB0 RID: 2992
	[Serializable]
	public class OvenCookOperation
	{
		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x060052B0 RID: 21168 RVA: 0x0015C8E1 File Offset: 0x0015AAE1
		[CodegenExclude]
		public StorableItemDefinition Ingredient
		{
			get
			{
				if (this._itemDefinition == null)
				{
					this._itemDefinition = (Registry.GetItem(this.IngredientID) as StorableItemDefinition);
				}
				return this._itemDefinition;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x060052B1 RID: 21169 RVA: 0x0015C90D File Offset: 0x0015AB0D
		[CodegenExclude]
		public StorableItemDefinition Product
		{
			get
			{
				if (this._productionDefinition == null)
				{
					this._productionDefinition = (Registry.GetItem(this.ProductID) as StorableItemDefinition);
				}
				return this._productionDefinition;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x060052B2 RID: 21170 RVA: 0x0015C939 File Offset: 0x0015AB39
		[CodegenExclude]
		public CookableModule Cookable
		{
			get
			{
				if (this._cookable == null)
				{
					this._cookable = this.Ingredient.StationItem.GetModule<CookableModule>();
				}
				return this._cookable;
			}
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x0015C965 File Offset: 0x0015AB65
		public OvenCookOperation(string ingredientID, EQuality ingredientQuality, int ingredientQuantity, string productID)
		{
			this.IngredientID = ingredientID;
			this.IngredientQuality = ingredientQuality;
			this.IngredientQuantity = ingredientQuantity;
			this.ProductID = productID;
			this.CookProgress = 0;
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x0015C99F File Offset: 0x0015AB9F
		public OvenCookOperation(string ingredientID, EQuality ingredientQuality, int ingredientQuantity, string productID, int progress)
		{
			this.IngredientID = ingredientID;
			this.IngredientQuality = ingredientQuality;
			this.IngredientQuantity = ingredientQuantity;
			this.ProductID = productID;
			this.CookProgress = progress;
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x0015C9DA File Offset: 0x0015ABDA
		public OvenCookOperation()
		{
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x0015C9F0 File Offset: 0x0015ABF0
		public void UpdateCookProgress(int change)
		{
			this.CookProgress += change;
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x0015CA00 File Offset: 0x0015AC00
		public int GetCookDuration()
		{
			if (this.cookDuration == -1)
			{
				this.cookDuration = this.Ingredient.StationItem.GetModule<CookableModule>().CookTime;
			}
			return this.cookDuration;
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x0015CA2C File Offset: 0x0015AC2C
		public ItemInstance GetProductItem(int quantity)
		{
			ItemInstance defaultInstance = this.Product.GetDefaultInstance(quantity);
			if (defaultInstance is QualityItemInstance)
			{
				(defaultInstance as QualityItemInstance).Quality = this.IngredientQuality;
			}
			return defaultInstance;
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x0015CA60 File Offset: 0x0015AC60
		public bool IsReady()
		{
			return this.CookProgress >= this.GetCookDuration();
		}

		// Token: 0x04003DD1 RID: 15825
		[CodegenExclude]
		private StorableItemDefinition _itemDefinition;

		// Token: 0x04003DD2 RID: 15826
		[CodegenExclude]
		private StorableItemDefinition _productionDefinition;

		// Token: 0x04003DD3 RID: 15827
		[CodegenExclude]
		private CookableModule _cookable;

		// Token: 0x04003DD4 RID: 15828
		public string IngredientID;

		// Token: 0x04003DD5 RID: 15829
		public EQuality IngredientQuality;

		// Token: 0x04003DD6 RID: 15830
		public int IngredientQuantity = 1;

		// Token: 0x04003DD7 RID: 15831
		public string ProductID;

		// Token: 0x04003DD8 RID: 15832
		public int CookProgress;

		// Token: 0x04003DD9 RID: 15833
		[CodegenExclude]
		private int cookDuration = -1;
	}
}
