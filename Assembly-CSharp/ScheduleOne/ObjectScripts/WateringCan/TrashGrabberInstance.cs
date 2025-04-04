using System;
using System.Collections.Generic;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Storage;
using ScheduleOne.Trash;

namespace ScheduleOne.ObjectScripts.WateringCan
{
	// Token: 0x02000BC8 RID: 3016
	[Serializable]
	public class TrashGrabberInstance : StorableItemInstance
	{
		// Token: 0x060054C5 RID: 21701 RVA: 0x00165077 File Offset: 0x00163277
		public TrashGrabberInstance()
		{
		}

		// Token: 0x060054C6 RID: 21702 RVA: 0x0016508A File Offset: 0x0016328A
		public TrashGrabberInstance(ItemDefinition definition, int quantity) : base(definition, quantity)
		{
		}

		// Token: 0x060054C7 RID: 21703 RVA: 0x001650A0 File Offset: 0x001632A0
		public override ItemInstance GetCopy(int overrideQuantity = -1)
		{
			int quantity = this.Quantity;
			if (overrideQuantity != -1)
			{
				quantity = overrideQuantity;
			}
			TrashGrabberInstance trashGrabberInstance = new TrashGrabberInstance(base.Definition, quantity);
			trashGrabberInstance.Content.LoadFromData(this.Content.GetData());
			return trashGrabberInstance;
		}

		// Token: 0x060054C8 RID: 21704 RVA: 0x001650DC File Offset: 0x001632DC
		public void LoadContentData(TrashContentData content)
		{
			this.Content.LoadFromData(content);
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x001650EA File Offset: 0x001632EA
		public override ItemData GetItemData()
		{
			return new TrashGrabberData(this.ID, this.Quantity, this.Content.GetData());
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x00165108 File Offset: 0x00163308
		public void AddTrash(string id, int quantity)
		{
			this.Content.AddTrash(id, quantity);
			base.InvokeDataChange();
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x0016511D File Offset: 0x0016331D
		public void RemoveTrash(string id, int quantity)
		{
			this.Content.RemoveTrash(id, quantity);
			base.InvokeDataChange();
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x00165132 File Offset: 0x00163332
		public void ClearTrash()
		{
			this.Content.Clear();
			base.InvokeDataChange();
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x00165145 File Offset: 0x00163345
		public int GetTotalSize()
		{
			return this.Content.GetTotalSize();
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x00165154 File Offset: 0x00163354
		public List<string> GetTrashIDs()
		{
			List<string> list = new List<string>();
			foreach (TrashContent.Entry entry in this.Content.Entries)
			{
				list.Add(entry.TrashID);
			}
			return list;
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x001651B8 File Offset: 0x001633B8
		public List<int> GetTrashQuantities()
		{
			List<int> list = new List<int>();
			foreach (TrashContent.Entry entry in this.Content.Entries)
			{
				list.Add(entry.Quantity);
			}
			return list;
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x0016521C File Offset: 0x0016341C
		public List<ushort> GetTrashUshortQuantities()
		{
			List<ushort> list = new List<ushort>();
			foreach (TrashContent.Entry entry in this.Content.Entries)
			{
				list.Add((ushort)entry.Quantity);
			}
			return list;
		}

		// Token: 0x04003ED1 RID: 16081
		public const int TRASH_CAPACITY = 20;

		// Token: 0x04003ED2 RID: 16082
		private TrashContent Content = new TrashContent();
	}
}
