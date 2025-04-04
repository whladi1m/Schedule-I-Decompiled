using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200043E RID: 1086
	public class WeedLoader : ItemLoader
	{
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x000603FE File Offset: 0x0005E5FE
		public override string ItemType
		{
			get
			{
				return typeof(WeedData).Name;
			}
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x00060410 File Offset: 0x0005E610
		public override ItemInstance LoadItem(string itemString)
		{
			WeedData weedData = base.LoadData<WeedData>(itemString);
			if (weedData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (weedData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(weedData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + weedData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(weedData.Quality, out equality) ? equality : EQuality.Standard;
			PackagingDefinition packaging = null;
			if (weedData.PackagingID != string.Empty)
			{
				ItemDefinition item2 = Registry.GetItem(weedData.PackagingID);
				if (item != null)
				{
					packaging = (item2 as PackagingDefinition);
				}
			}
			return new WeedInstance(item, weedData.Quantity, quality, packaging);
		}
	}
}
