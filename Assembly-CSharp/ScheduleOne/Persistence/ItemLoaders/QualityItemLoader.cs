using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200043B RID: 1083
	public class QualityItemLoader : ItemLoader
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x00060237 File Offset: 0x0005E437
		public override string ItemType
		{
			get
			{
				return typeof(QualityItemData).Name;
			}
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x00060248 File Offset: 0x0005E448
		public override ItemInstance LoadItem(string itemString)
		{
			QualityItemData qualityItemData = base.LoadData<QualityItemData>(itemString);
			if (qualityItemData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (qualityItemData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(qualityItemData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + qualityItemData.ID, null);
				return null;
			}
			EQuality equality;
			EQuality quality = Enum.TryParse<EQuality>(qualityItemData.Quality, out equality) ? equality : EQuality.Standard;
			return new QualityItemInstance(item, qualityItemData.Quantity, quality);
		}
	}
}
