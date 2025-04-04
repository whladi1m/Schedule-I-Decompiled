using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Levelling;
using ScheduleOne.StationFramework;
using ScheduleOne.Storage;
using ScheduleOne.UI.Shop;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x0200093C RID: 2364
	[CreateAssetMenu(fileName = "StorableItemDefinition", menuName = "ScriptableObjects/StorableItemDefinition", order = 1)]
	[Serializable]
	public class StorableItemDefinition : ItemDefinition
	{
		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06004029 RID: 16425 RVA: 0x0010D5CC File Offset: 0x0010B7CC
		public bool IsPurchasable
		{
			get
			{
				return !this.RequiresLevelToPurchase || NetworkSingleton<LevelManager>.Instance.GetFullRank() >= this.RequiredRank;
			}
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x0010D5ED File Offset: 0x0010B7ED
		public override ItemInstance GetDefaultInstance(int quantity = 1)
		{
			return new StorableItemInstance(this, quantity);
		}

		// Token: 0x04002E13 RID: 11795
		[Header("Purchasing")]
		public float BasePurchasePrice = 10f;

		// Token: 0x04002E14 RID: 11796
		public List<ShopListing.CategoryInstance> ShopCategories = new List<ShopListing.CategoryInstance>();

		// Token: 0x04002E15 RID: 11797
		public bool RequiresLevelToPurchase;

		// Token: 0x04002E16 RID: 11798
		public FullRank RequiredRank;

		// Token: 0x04002E17 RID: 11799
		[Header("Storable Item")]
		public StoredItem StoredItem;

		// Token: 0x04002E18 RID: 11800
		[Tooltip("Optional station item if this item can be used at a station.")]
		public StationItem StationItem;
	}
}
