using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003C8 RID: 968
	[Serializable]
	public class ItemSet
	{
		// Token: 0x06001511 RID: 5393 RVA: 0x0005EA14 File Offset: 0x0005CC14
		public ItemSet(List<ItemData> items)
		{
			this.Items = new string[items.Count];
			for (int i = 0; i < items.Count; i++)
			{
				this.Items[i] = items[i].GetJson(false);
			}
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0005EA5E File Offset: 0x0005CC5E
		public string GetJSON()
		{
			return JsonUtility.ToJson(this, true);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0005EA68 File Offset: 0x0005CC68
		public ItemSet(List<ItemInstance> items)
		{
			this.Items = new string[items.Count];
			for (int i = 0; i < items.Count; i++)
			{
				this.Items[i] = items[i].GetItemData().GetJson(false);
			}
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0005EAB8 File Offset: 0x0005CCB8
		public ItemSet(List<ItemSlot> itemSlots)
		{
			this.Items = new string[itemSlots.Count];
			for (int i = 0; i < itemSlots.Count; i++)
			{
				if (itemSlots[i].ItemInstance != null)
				{
					this.Items[i] = itemSlots[i].ItemInstance.GetItemData().GetJson(false);
				}
				else
				{
					this.Items[i] = new ItemData(string.Empty, 0).GetJson(false);
				}
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0005EB38 File Offset: 0x0005CD38
		public ItemSet(ItemSlot[] itemSlots)
		{
			this.Items = new string[itemSlots.Length];
			for (int i = 0; i < itemSlots.Length; i++)
			{
				if (itemSlots[i].ItemInstance != null)
				{
					this.Items[i] = itemSlots[i].ItemInstance.GetItemData().GetJson(false);
				}
				else
				{
					this.Items[i] = new ItemData(string.Empty, 0).GetJson(false);
				}
			}
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0005EBA8 File Offset: 0x0005CDA8
		public static ItemInstance[] Deserialize(string json)
		{
			ItemSet itemSet = null;
			try
			{
				itemSet = JsonUtility.FromJson<ItemSet>(json);
			}
			catch (Exception ex)
			{
				string str = "Failed to deserialize ItemSet from JSON: ";
				string str2 = "\nException: ";
				Exception ex2 = ex;
				Console.LogError(str + json + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				return new ItemInstance[0];
			}
			ItemInstance[] array = new ItemInstance[itemSet.Items.Length];
			for (int i = 0; i < itemSet.Items.Length; i++)
			{
				array[i] = ItemDeserializer.LoadItem(itemSet.Items[i]);
			}
			return array;
		}

		// Token: 0x0400135E RID: 4958
		public string[] Items;
	}
}
