using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using UnityEngine;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x02000438 RID: 1080
	public class ItemLoader
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0005FF62 File Offset: 0x0005E162
		public virtual string ItemType
		{
			get
			{
				return typeof(ItemData).Name;
			}
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0005FF73 File Offset: 0x0005E173
		public ItemLoader()
		{
			Singleton<LoadManager>.Instance.ItemLoaders.Add(this);
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0005FF8C File Offset: 0x0005E18C
		public virtual ItemInstance LoadItem(string itemString)
		{
			ItemData itemData = this.LoadData<ItemData>(itemString);
			if (itemData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (itemData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(itemData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + itemData.ID, null);
				return null;
			}
			return new StorableItemInstance(item, itemData.Quantity);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00060004 File Offset: 0x0005E204
		protected T LoadData<T>(string itemString) where T : ItemData
		{
			T result = default(T);
			try
			{
				result = JsonUtility.FromJson<T>(itemString);
			}
			catch (Exception ex)
			{
				string[] array = new string[5];
				int num = 0;
				Type type = base.GetType();
				array[num] = ((type != null) ? type.ToString() : null);
				array[1] = " error parsing item data: ";
				array[2] = itemString;
				array[3] = "\n";
				int num2 = 4;
				Exception ex2 = ex;
				array[num2] = ((ex2 != null) ? ex2.ToString() : null);
				Console.LogError(string.Concat(array), null);
				return default(T);
			}
			return result;
		}
	}
}
