using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200043D RID: 1085
	public class WateringCanLoader : ItemLoader
	{
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x0006036C File Offset: 0x0005E56C
		public override string ItemType
		{
			get
			{
				return typeof(WateringCanData).Name;
			}
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x00060380 File Offset: 0x0005E580
		public override ItemInstance LoadItem(string itemString)
		{
			WateringCanData wateringCanData = base.LoadData<WateringCanData>(itemString);
			if (wateringCanData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (wateringCanData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(wateringCanData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + wateringCanData.ID, null);
				return null;
			}
			return new WateringCanInstance(item, wateringCanData.Quantity, wateringCanData.CurrentFillAmount);
		}
	}
}
