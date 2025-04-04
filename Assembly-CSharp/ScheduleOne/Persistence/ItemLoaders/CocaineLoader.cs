using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000436 RID: 1078
	public class CocaineLoader : ItemLoader
	{
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x0005FDFE File Offset: 0x0005DFFE
		public override string ItemType
		{
			get
			{
				return typeof(CocaineData).Name;
			}
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x0005FE10 File Offset: 0x0005E010
		public override ItemInstance LoadItem(string itemString)
		{
			CocaineData cocaineData = base.LoadData<CocaineData>(itemString);
			if (cocaineData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (cocaineData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(cocaineData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + cocaineData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(cocaineData.Quality, out equality) ? equality : EQuality.Standard;
			PackagingDefinition packaging = null;
			if (cocaineData.PackagingID != string.Empty)
			{
				ItemDefinition item2 = Registry.GetItem(cocaineData.PackagingID);
				if (item != null)
				{
					packaging = (item2 as PackagingDefinition);
				}
			}
			return new CocaineInstance(item, cocaineData.Quantity, quality, packaging);
		}
	}
}
