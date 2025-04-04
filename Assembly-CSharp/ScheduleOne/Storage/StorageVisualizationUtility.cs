using System;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleOne.Storage
{
	// Token: 0x02000896 RID: 2198
	public static class StorageVisualizationUtility
	{
		// Token: 0x06003BDB RID: 15323 RVA: 0x000FC108 File Offset: 0x000FA308
		public static Dictionary<StorableItemInstance, int> GetVisualRepresentation(Dictionary<StorableItemInstance, int> inputDictionary, int TotalFootprintSize)
		{
			int num = TotalFootprintSize;
			List<StorableItemInstance> list = inputDictionary.Keys.ToList<StorableItemInstance>();
			Dictionary<StorableItemInstance, int> dictionary = new Dictionary<StorableItemInstance, int>();
			while (num > 0 && list.Count > 0)
			{
				List<StorableItemInstance> list2 = new List<StorableItemInstance>();
				list2.AddRange(list);
				foreach (StorableItemInstance storableItemInstance in list2)
				{
					if (storableItemInstance == null || storableItemInstance.StoredItem == null)
					{
						list.Remove(storableItemInstance);
					}
					else
					{
						int num2 = storableItemInstance.StoredItem.FootprintY * storableItemInstance.StoredItem.FootprintX;
						if (num < num2)
						{
							list.Remove(storableItemInstance);
						}
						else
						{
							if (!dictionary.ContainsKey(storableItemInstance))
							{
								dictionary.Add(storableItemInstance, 0);
							}
							Dictionary<StorableItemInstance, int> dictionary2 = dictionary;
							StorableItemInstance key = storableItemInstance;
							int num3 = dictionary2[key];
							dictionary2[key] = num3 + 1;
							num -= num2;
							key = storableItemInstance;
							num3 = inputDictionary[key];
							inputDictionary[key] = num3 - 1;
							if (inputDictionary[storableItemInstance] <= 0)
							{
								list.Remove(storableItemInstance);
							}
						}
					}
				}
			}
			return dictionary;
		}
	}
}
