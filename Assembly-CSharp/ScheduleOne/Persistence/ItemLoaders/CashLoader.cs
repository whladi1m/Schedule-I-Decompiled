using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000434 RID: 1076
	public class CashLoader : ItemLoader
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600159C RID: 5532 RVA: 0x0005FCCC File Offset: 0x0005DECC
		public override string ItemType
		{
			get
			{
				return typeof(CashData).Name;
			}
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x0005FCE8 File Offset: 0x0005DEE8
		public override ItemInstance LoadItem(string itemString)
		{
			CashData cashData = base.LoadData<CashData>(itemString);
			if (cashData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (cashData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(cashData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + cashData.ID, null);
				return null;
			}
			CashInstance cashInstance = new CashInstance(item, cashData.Quantity);
			cashInstance.SetBalance(cashData.CashBalance, false);
			return cashInstance;
		}
	}
}
