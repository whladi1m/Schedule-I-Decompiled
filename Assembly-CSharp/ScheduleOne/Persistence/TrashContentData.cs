using System;
using System.Collections.Generic;
using ScheduleOne.Trash;

namespace ScheduleOne.Persistence
{
	// Token: 0x0200037A RID: 890
	[Serializable]
	public class TrashContentData
	{
		// Token: 0x0600142B RID: 5163 RVA: 0x0005A037 File Offset: 0x00058237
		public TrashContentData()
		{
			this.TrashIDs = new string[0];
			this.TrashQuantities = new int[0];
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0005A057 File Offset: 0x00058257
		public TrashContentData(string[] trashIDs, int[] trashQuantities)
		{
			this.TrashIDs = trashIDs;
			this.TrashQuantities = trashQuantities;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0005A070 File Offset: 0x00058270
		public TrashContentData(List<TrashItem> trashItems)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (TrashItem trashItem in trashItems)
			{
				if (!dictionary.ContainsKey(trashItem.ID))
				{
					dictionary.Add(trashItem.ID, 0);
				}
				Dictionary<string, int> dictionary2 = dictionary;
				string id = trashItem.ID;
				int num = dictionary2[id];
				dictionary2[id] = num + 1;
			}
			this.TrashIDs = new string[dictionary.Count];
			this.TrashQuantities = new int[dictionary.Count];
			int num2 = 0;
			foreach (KeyValuePair<string, int> keyValuePair in dictionary)
			{
				this.TrashIDs[num2] = keyValuePair.Key;
				this.TrashQuantities[num2] = keyValuePair.Value;
				num2++;
			}
		}

		// Token: 0x04001305 RID: 4869
		public string[] TrashIDs;

		// Token: 0x04001306 RID: 4870
		public int[] TrashQuantities;
	}
}
