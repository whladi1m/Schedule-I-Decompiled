using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using UnityEngine;

namespace ScheduleOne.Management
{
	// Token: 0x02000571 RID: 1393
	public interface ITransitEntity
	{
		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x060022DE RID: 8926
		string Name { get; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x060022DF RID: 8927
		// (set) Token: 0x060022E0 RID: 8928
		List<ItemSlot> InputSlots { get; set; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x060022E1 RID: 8929
		// (set) Token: 0x060022E2 RID: 8930
		List<ItemSlot> OutputSlots { get; set; }

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x060022E3 RID: 8931
		Transform LinkOrigin { get; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x060022E4 RID: 8932
		Transform[] AccessPoints { get; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x060022E5 RID: 8933
		bool Selectable { get; }

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x060022E6 RID: 8934
		bool IsAcceptingItems { get; }

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x060022E7 RID: 8935
		bool IsDestroyed { get; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x060022E8 RID: 8936
		Guid GUID { get; }

		// Token: 0x060022E9 RID: 8937
		void ShowOutline(Color color);

		// Token: 0x060022EA RID: 8938
		void HideOutline();

		// Token: 0x060022EB RID: 8939 RVA: 0x0008F040 File Offset: 0x0008D240
		void InsertItemIntoInput(ItemInstance item, NPC inserter = null)
		{
			if (this.GetInputCapacityForItem(item, inserter) < item.Quantity)
			{
				Console.LogWarning("ITransitEntity InsertItem() called but item won't fit!", null);
				return;
			}
			int num = item.Quantity;
			for (int i = 0; i < this.InputSlots.Count; i++)
			{
				if (!this.InputSlots[i].IsLocked && !this.InputSlots[i].IsAddLocked)
				{
					int capacityForItem = this.InputSlots[i].GetCapacityForItem(item);
					if (capacityForItem > 0)
					{
						int num2 = Mathf.Min(capacityForItem, num);
						if (this.InputSlots[i].ItemInstance == null)
						{
							this.InputSlots[i].SetStoredItem(item, false);
						}
						else
						{
							this.InputSlots[i].ChangeQuantity(num2, false);
						}
						num -= num2;
					}
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0008F114 File Offset: 0x0008D314
		void InsertItemIntoOutput(ItemInstance item, NPC inserter = null)
		{
			if (this.GetOutputCapacityForItem(item, inserter) < item.Quantity)
			{
				Console.LogWarning("ITransitEntity InsertItem() called but item won't fit!", null);
				return;
			}
			int num = item.Quantity;
			for (int i = 0; i < this.OutputSlots.Count; i++)
			{
				if (!this.OutputSlots[i].IsLocked && !this.OutputSlots[i].IsAddLocked)
				{
					int capacityForItem = this.OutputSlots[i].GetCapacityForItem(item);
					if (capacityForItem > 0)
					{
						int num2 = Mathf.Min(capacityForItem, num);
						if (this.OutputSlots[i].ItemInstance == null)
						{
							this.OutputSlots[i].SetStoredItem(item, false);
						}
						else
						{
							this.OutputSlots[i].ChangeQuantity(num2, false);
						}
						num -= num2;
					}
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x0008F1E8 File Offset: 0x0008D3E8
		int GetInputCapacityForItem(ItemInstance item, NPC asker = null)
		{
			int num = 0;
			NetworkObject networkObject = (asker != null) ? asker.NetworkObject : null;
			int i = 0;
			while (i < this.InputSlots.Count)
			{
				if (!this.InputSlots[i].IsLocked && !this.InputSlots[i].IsAddLocked)
				{
					goto IL_83;
				}
				bool flag = false;
				if (networkObject != null && this.InputSlots[i].ActiveLock != null && this.InputSlots[i].ActiveLock.LockOwner == networkObject)
				{
					flag = true;
				}
				if (flag)
				{
					goto IL_83;
				}
				IL_98:
				i++;
				continue;
				IL_83:
				num += this.InputSlots[i].GetCapacityForItem(item);
				goto IL_98;
			}
			return num;
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x0008F2A4 File Offset: 0x0008D4A4
		int GetOutputCapacityForItem(ItemInstance item, NPC asker = null)
		{
			int num = 0;
			NetworkObject networkObject = (asker != null) ? asker.NetworkObject : null;
			int i = 0;
			while (i < this.OutputSlots.Count)
			{
				if (!this.OutputSlots[i].IsLocked && !this.OutputSlots[i].IsAddLocked)
				{
					goto IL_83;
				}
				bool flag = false;
				if (networkObject != null && this.OutputSlots[i].ActiveLock != null && this.OutputSlots[i].ActiveLock.LockOwner == networkObject)
				{
					flag = true;
				}
				if (flag)
				{
					goto IL_83;
				}
				IL_98:
				i++;
				continue;
				IL_83:
				num += this.OutputSlots[i].GetCapacityForItem(item);
				goto IL_98;
			}
			return num;
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x0008F360 File Offset: 0x0008D560
		ItemSlot GetOutputItemContainer(ItemInstance item)
		{
			return this.OutputSlots.FirstOrDefault((ItemSlot x) => x.ItemInstance == item);
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x0008F394 File Offset: 0x0008D594
		List<ItemSlot> ReserveInputSlotsForItem(ItemInstance item, NetworkObject locker)
		{
			List<ItemSlot> list = new List<ItemSlot>();
			int num = item.Quantity;
			for (int i = 0; i < this.InputSlots.Count; i++)
			{
				int capacityForItem = this.InputSlots[i].GetCapacityForItem(item);
				if (capacityForItem != 0)
				{
					int num2 = Mathf.Min(capacityForItem, num);
					num -= num2;
					this.InputSlots[i].ApplyLock(locker, "Employee is about to place an item here", false);
					list.Add(this.InputSlots[i]);
					if (num <= 0)
					{
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x0008F418 File Offset: 0x0008D618
		void RemoveSlotLocks(NetworkObject locker)
		{
			for (int i = 0; i < this.InputSlots.Count; i++)
			{
				if (this.InputSlots[i].ActiveLock != null && this.InputSlots[i].ActiveLock.LockOwner == locker)
				{
					this.InputSlots[i].RemoveLock(false);
				}
			}
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x0008F480 File Offset: 0x0008D680
		ItemSlot GetFirstSlotContainingItem(string id, ITransitEntity.ESlotType searchType)
		{
			if (searchType == ITransitEntity.ESlotType.Output || searchType == ITransitEntity.ESlotType.Both)
			{
				for (int i = 0; i < this.OutputSlots.Count; i++)
				{
					if (this.OutputSlots[i].ItemInstance != null && this.OutputSlots[i].ItemInstance.ID == id)
					{
						return this.OutputSlots[i];
					}
				}
			}
			if (searchType == ITransitEntity.ESlotType.Input || searchType == ITransitEntity.ESlotType.Both)
			{
				for (int j = 0; j < this.InputSlots.Count; j++)
				{
					if (this.InputSlots[j].ItemInstance != null && this.InputSlots[j].ItemInstance.ID == id)
					{
						return this.InputSlots[j];
					}
				}
			}
			return null;
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0008F548 File Offset: 0x0008D748
		ItemSlot GetFirstSlotContainingTemplateItem(ItemInstance templateItem, ITransitEntity.ESlotType searchType)
		{
			if (searchType == ITransitEntity.ESlotType.Output || searchType == ITransitEntity.ESlotType.Both)
			{
				for (int i = 0; i < this.OutputSlots.Count; i++)
				{
					if (this.OutputSlots[i].ItemInstance != null && this.OutputSlots[i].ItemInstance.CanStackWith(templateItem, false))
					{
						return this.OutputSlots[i];
					}
				}
			}
			if (searchType == ITransitEntity.ESlotType.Input || searchType == ITransitEntity.ESlotType.Both)
			{
				for (int j = 0; j < this.InputSlots.Count; j++)
				{
					if (this.InputSlots[j].ItemInstance != null && this.InputSlots[j].ItemInstance.CanStackWith(templateItem, false))
					{
						return this.InputSlots[j];
					}
				}
			}
			return null;
		}

		// Token: 0x02000572 RID: 1394
		public enum ESlotType
		{
			// Token: 0x04001A32 RID: 6706
			Input,
			// Token: 0x04001A33 RID: 6707
			Output,
			// Token: 0x04001A34 RID: 6708
			Both
		}
	}
}
