using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Connection;
using FishNet.Object;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000929 RID: 2345
	public interface IItemSlotOwner
	{
		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06003F88 RID: 16264
		// (set) Token: 0x06003F89 RID: 16265
		List<ItemSlot> ItemSlots { get; set; }

		// Token: 0x06003F8A RID: 16266
		void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance);

		// Token: 0x06003F8B RID: 16267
		void SetItemSlotQuantity(int itemSlotIndex, int quantity);

		// Token: 0x06003F8C RID: 16268
		void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason);

		// Token: 0x06003F8D RID: 16269 RVA: 0x0010B8E0 File Offset: 0x00109AE0
		void SendItemsToClient(NetworkConnection conn)
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].IsLocked)
				{
					this.SetSlotLocked(conn, i, true, this.ItemSlots[i].ActiveLock.LockOwner, this.ItemSlots[i].ActiveLock.LockReason);
				}
				if (this.ItemSlots[i].ItemInstance != null)
				{
					this.SetStoredInstance(conn, i, this.ItemSlots[i].ItemInstance);
				}
			}
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0010B97A File Offset: 0x00109B7A
		int GetTotalItemCount()
		{
			return this.ItemSlots.Sum((ItemSlot x) => x.Quantity);
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x0010B9A8 File Offset: 0x00109BA8
		int GetItemCount(string id)
		{
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance.ID == id)
				{
					num += this.ItemSlots[i].Quantity;
				}
			}
			return num;
		}
	}
}
