using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.ItemLoaders;
using UnityEngine;

namespace ScheduleOne.Persistence
{
	// Token: 0x02000364 RID: 868
	public static class ItemDeserializer
	{
		// Token: 0x060013AD RID: 5037 RVA: 0x00057AF4 File Offset: 0x00055CF4
		public static ItemInstance LoadItem(string itemString)
		{
			ItemData itemData = null;
			try
			{
				itemData = JsonUtility.FromJson<ItemData>(itemString);
			}
			catch (Exception ex)
			{
				string str = "Failed to deserialize ItemData from JSON: ";
				string str2 = "\nException: ";
				Exception ex2 = ex;
				Console.LogError(str + itemString + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				return null;
			}
			if (itemData == null)
			{
				Console.LogWarning("Failed to deserialize ItemData from JSON: " + itemString, null);
				return null;
			}
			ItemLoader itemLoader = Singleton<LoadManager>.Instance.GetItemLoader(itemData.DataType);
			if (itemLoader == null)
			{
				Console.LogError("Failed to find item loader for " + itemData.DataType, null);
				return null;
			}
			return itemLoader.LoadItem(itemString);
		}
	}
}
