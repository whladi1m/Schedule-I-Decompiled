using System;
using ScheduleOne.Clothing;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000435 RID: 1077
	public class ClothingLoader : ItemLoader
	{
		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x0600159F RID: 5535 RVA: 0x0005FD6D File Offset: 0x0005DF6D
		public override string ItemType
		{
			get
			{
				return typeof(ClothingData).Name;
			}
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x0005FD80 File Offset: 0x0005DF80
		public override ItemInstance LoadItem(string itemString)
		{
			ClothingData clothingData = base.LoadData<ClothingData>(itemString);
			if (clothingData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (clothingData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(clothingData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + clothingData.ID, null);
				return null;
			}
			return new ClothingInstance(item, clothingData.Quantity, clothingData.Color);
		}
	}
}
