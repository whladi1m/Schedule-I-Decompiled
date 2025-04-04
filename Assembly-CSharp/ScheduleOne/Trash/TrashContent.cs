using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;

namespace ScheduleOne.Trash
{
	// Token: 0x02000815 RID: 2069
	[Serializable]
	public class TrashContent
	{
		// Token: 0x060038C0 RID: 14528 RVA: 0x000F0130 File Offset: 0x000EE330
		public void AddTrash(string trashID, int quantity)
		{
			TrashContent.Entry entry = this.Entries.Find((TrashContent.Entry e) => e.TrashID == trashID);
			if (entry == null)
			{
				entry = new TrashContent.Entry(trashID, 0);
				this.Entries.Add(entry);
			}
			this.Entries.Remove(entry);
			this.Entries.Add(entry);
			entry.Quantity += quantity;
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x000F01A8 File Offset: 0x000EE3A8
		public void RemoveTrash(string trashID, int quantity)
		{
			TrashContent.Entry entry = this.Entries.Find((TrashContent.Entry e) => e.TrashID == trashID);
			if (entry == null)
			{
				return;
			}
			entry.Quantity -= quantity;
			if (entry.Quantity <= 0)
			{
				this.Entries.Remove(entry);
			}
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x000F0204 File Offset: 0x000EE404
		public int GetTrashQuantity(string trashID)
		{
			TrashContent.Entry entry = this.Entries.Find((TrashContent.Entry e) => e.TrashID == trashID);
			if (entry == null)
			{
				return 0;
			}
			return entry.Quantity;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x000F0241 File Offset: 0x000EE441
		public void Clear()
		{
			this.Entries.Clear();
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x000F0250 File Offset: 0x000EE450
		public int GetTotalSize()
		{
			int num = 0;
			foreach (TrashContent.Entry entry in this.Entries)
			{
				num += entry.Quantity * entry.UnitSize;
			}
			return num;
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x000F02B0 File Offset: 0x000EE4B0
		public TrashContentData GetData()
		{
			TrashContentData trashContentData = new TrashContentData();
			trashContentData.TrashIDs = new string[this.Entries.Count];
			trashContentData.TrashQuantities = new int[this.Entries.Count];
			for (int i = 0; i < this.Entries.Count; i++)
			{
				trashContentData.TrashIDs[i] = this.Entries[i].TrashID;
				trashContentData.TrashQuantities[i] = this.Entries[i].Quantity;
			}
			return trashContentData;
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x000F0338 File Offset: 0x000EE538
		public void LoadFromData(TrashContentData data)
		{
			for (int i = 0; i < data.TrashIDs.Length; i++)
			{
				this.AddTrash(data.TrashIDs[i], data.TrashQuantities[i]);
			}
		}

		// Token: 0x0400292B RID: 10539
		public List<TrashContent.Entry> Entries = new List<TrashContent.Entry>();

		// Token: 0x02000816 RID: 2070
		[Serializable]
		public class Entry
		{
			// Token: 0x1700080E RID: 2062
			// (get) Token: 0x060038C8 RID: 14536 RVA: 0x000F0381 File Offset: 0x000EE581
			// (set) Token: 0x060038C9 RID: 14537 RVA: 0x000F0389 File Offset: 0x000EE589
			public int UnitSize { get; private set; }

			// Token: 0x1700080F RID: 2063
			// (get) Token: 0x060038CA RID: 14538 RVA: 0x000F0392 File Offset: 0x000EE592
			// (set) Token: 0x060038CB RID: 14539 RVA: 0x000F039A File Offset: 0x000EE59A
			public int UnitValue { get; private set; }

			// Token: 0x060038CC RID: 14540 RVA: 0x000F03A4 File Offset: 0x000EE5A4
			public Entry(string id, int quantity)
			{
				this.TrashID = id;
				this.Quantity = quantity;
				TrashItem trashPrefab = NetworkSingleton<TrashManager>.Instance.GetTrashPrefab(id);
				if (trashPrefab != null)
				{
					this.UnitSize = trashPrefab.Size;
					this.UnitValue = trashPrefab.SellValue;
				}
			}

			// Token: 0x0400292C RID: 10540
			public string TrashID;

			// Token: 0x0400292D RID: 10541
			public int Quantity;
		}
	}
}
