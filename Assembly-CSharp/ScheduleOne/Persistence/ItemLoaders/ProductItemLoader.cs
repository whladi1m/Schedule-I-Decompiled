using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200043A RID: 1082
	public class ProductItemLoader : ItemLoader
	{
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060015AF RID: 5551 RVA: 0x00060163 File Offset: 0x0005E363
		public override string ItemType
		{
			get
			{
				return typeof(ProductItemData).Name;
			}
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00060174 File Offset: 0x0005E374
		public override ItemInstance LoadItem(string itemString)
		{
			ProductItemData productItemData = base.LoadData<ProductItemData>(itemString);
			if (productItemData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (productItemData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(productItemData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + productItemData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(productItemData.Quality, out equality) ? equality : EQuality.Standard;
			PackagingDefinition packaging = null;
			if (productItemData.PackagingID != string.Empty)
			{
				ItemDefinition item2 = Registry.GetItem(productItemData.PackagingID);
				if (item != null)
				{
					packaging = (item2 as PackagingDefinition);
				}
			}
			return new ProductItemInstance(item, productItemData.Quantity, quality, packaging);
		}
	}
}
