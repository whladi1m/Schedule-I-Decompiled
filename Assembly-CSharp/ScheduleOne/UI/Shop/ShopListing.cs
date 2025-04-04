using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using UnityEngine;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A65 RID: 2661
	[Serializable]
	public class ShopListing
	{
		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060047E2 RID: 18402 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool IsInStock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x060047E3 RID: 18403 RVA: 0x0012DBF4 File Offset: 0x0012BDF4
		public float Price
		{
			get
			{
				if (!this.OverridePrice)
				{
					return this.Item.BasePurchasePrice;
				}
				return this.OverriddenPrice;
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060047E4 RID: 18404 RVA: 0x0012DC10 File Offset: 0x0012BE10
		// (set) Token: 0x060047E5 RID: 18405 RVA: 0x0012DC18 File Offset: 0x0012BE18
		public int CurrentQuantity { get; protected set; }

		// Token: 0x060047E6 RID: 18406 RVA: 0x0012DC21 File Offset: 0x0012BE21
		public void Restock()
		{
			this.CurrentQuantity = this.StockQuantity;
			if (this.onQuantityChanged != null)
			{
				this.onQuantityChanged();
			}
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x0012DC42 File Offset: 0x0012BE42
		public virtual bool ShouldShow()
		{
			return !this.EnforceMinimumGameCreationVersion || SaveManager.GetVersionNumber(Singleton<MetadataManager>.Instance.CreationVersion) >= this.MinimumGameCreationVersion;
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0012DC68 File Offset: 0x0012BE68
		public virtual bool DoesListingMatchCategoryFilter(EShopCategory category)
		{
			return category == EShopCategory.All || this.Item.ShopCategories.Find((ShopListing.CategoryInstance x) => x.Category == category) != null;
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x0012DCAB File Offset: 0x0012BEAB
		public virtual bool DoesListingMatchSearchTerm(string searchTerm)
		{
			return this.Item.Name.ToLower().Contains(searchTerm.ToLower());
		}

		// Token: 0x04003582 RID: 13698
		public string name;

		// Token: 0x04003583 RID: 13699
		public StorableItemDefinition Item;

		// Token: 0x04003584 RID: 13700
		[Header("Pricing")]
		[SerializeField]
		protected bool OverridePrice;

		// Token: 0x04003585 RID: 13701
		[SerializeField]
		protected float OverriddenPrice = 10f;

		// Token: 0x04003586 RID: 13702
		[Header("Stock")]
		public int StockQuantity = -1;

		// Token: 0x04003587 RID: 13703
		[Header("Settings")]
		public bool EnforceMinimumGameCreationVersion;

		// Token: 0x04003588 RID: 13704
		public float MinimumGameCreationVersion = 27f;

		// Token: 0x04003589 RID: 13705
		public bool CanBeDelivered;

		// Token: 0x0400358B RID: 13707
		public Action onQuantityChanged;

		// Token: 0x02000A66 RID: 2662
		[Serializable]
		public class CategoryInstance
		{
			// Token: 0x0400358C RID: 13708
			public EShopCategory Category;
		}
	}
}
