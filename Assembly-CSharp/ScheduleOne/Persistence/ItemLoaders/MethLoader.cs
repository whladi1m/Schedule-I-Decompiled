using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000439 RID: 1081
	public class MethLoader : ItemLoader
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x0006008C File Offset: 0x0005E28C
		public override string ItemType
		{
			get
			{
				return typeof(MethData).Name;
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x000600A0 File Offset: 0x0005E2A0
		public override ItemInstance LoadItem(string itemString)
		{
			MethData methData = base.LoadData<MethData>(itemString);
			if (methData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (methData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(methData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + methData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(methData.Quality, out equality) ? equality : EQuality.Standard;
			PackagingDefinition packaging = null;
			if (methData.PackagingID != string.Empty)
			{
				ItemDefinition item2 = Registry.GetItem(methData.PackagingID);
				if (item != null)
				{
					packaging = (item2 as PackagingDefinition);
				}
			}
			return new MethInstance(item, methData.Quantity, quality, packaging);
		}
	}
}
