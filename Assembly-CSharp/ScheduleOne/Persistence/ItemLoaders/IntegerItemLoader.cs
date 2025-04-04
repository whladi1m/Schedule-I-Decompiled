using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000437 RID: 1079
	public class IntegerItemLoader : ItemLoader
	{
		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x0005FED3 File Offset: 0x0005E0D3
		public override string ItemType
		{
			get
			{
				return typeof(IntegerItemData).Name;
			}
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x0005FEE4 File Offset: 0x0005E0E4
		public override ItemInstance LoadItem(string itemString)
		{
			IntegerItemData integerItemData = base.LoadData<IntegerItemData>(itemString);
			if (integerItemData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (integerItemData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(integerItemData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + integerItemData.ID, null);
				return null;
			}
			return new IntegerItemInstance(item, integerItemData.Quantity, integerItemData.Value);
		}
	}
}
