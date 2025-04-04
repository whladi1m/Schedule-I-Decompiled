using System;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000938 RID: 2360
	[Serializable]
	public class ItemSlot
	{
		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x0010CD87 File Offset: 0x0010AF87
		// (set) Token: 0x06003FF7 RID: 16375 RVA: 0x0010CD8F File Offset: 0x0010AF8F
		public ItemInstance ItemInstance { get; protected set; }

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x0010CD98 File Offset: 0x0010AF98
		// (set) Token: 0x06003FF9 RID: 16377 RVA: 0x0010CDA0 File Offset: 0x0010AFA0
		public IItemSlotOwner SlotOwner { get; protected set; }

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003FFA RID: 16378 RVA: 0x0010CDA9 File Offset: 0x0010AFA9
		private int SlotIndex
		{
			get
			{
				return this.SlotOwner.ItemSlots.IndexOf(this);
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06003FFB RID: 16379 RVA: 0x0010CDBC File Offset: 0x0010AFBC
		public int Quantity
		{
			get
			{
				if (this.ItemInstance == null)
				{
					return 0;
				}
				return this.ItemInstance.Quantity;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003FFC RID: 16380 RVA: 0x0010CDD3 File Offset: 0x0010AFD3
		public bool IsAtCapacity
		{
			get
			{
				return this.ItemInstance != null && this.Quantity >= this.ItemInstance.StackLimit;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003FFD RID: 16381 RVA: 0x0010CDF5 File Offset: 0x0010AFF5
		public bool IsLocked
		{
			get
			{
				return this.ActiveLock != null;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003FFE RID: 16382 RVA: 0x0010CE00 File Offset: 0x0010B000
		// (set) Token: 0x06003FFF RID: 16383 RVA: 0x0010CE08 File Offset: 0x0010B008
		public ItemSlotLock ActiveLock { get; protected set; }

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06004000 RID: 16384 RVA: 0x0010CE11 File Offset: 0x0010B011
		// (set) Token: 0x06004001 RID: 16385 RVA: 0x0010CE19 File Offset: 0x0010B019
		public bool IsRemovalLocked { get; protected set; }

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06004002 RID: 16386 RVA: 0x0010CE22 File Offset: 0x0010B022
		// (set) Token: 0x06004003 RID: 16387 RVA: 0x0010CE2A File Offset: 0x0010B02A
		public bool IsAddLocked { get; protected set; }

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x0010CE33 File Offset: 0x0010B033
		// (set) Token: 0x06004005 RID: 16389 RVA: 0x0010CE3B File Offset: 0x0010B03B
		protected List<ItemFilter> Filters { get; set; } = new List<ItemFilter>();

		// Token: 0x06004006 RID: 16390 RVA: 0x0010CE44 File Offset: 0x0010B044
		public void SetSlotOwner(IItemSlotOwner owner)
		{
			this.SlotOwner = owner;
			this.SlotOwner.ItemSlots.Add(this);
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x0010CE5E File Offset: 0x0010B05E
		public void ReplicateStoredInstance()
		{
			if (this.SlotOwner == null)
			{
				return;
			}
			this.SlotOwner.SetStoredInstance(null, this.SlotIndex, this.ItemInstance);
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x0010CE84 File Offset: 0x0010B084
		public virtual void SetStoredItem(ItemInstance instance, bool _internal = false)
		{
			if (this.IsLocked)
			{
				Console.LogError("SetStoredInstance called on ItemSlot that is locked! Refusing.", null);
				return;
			}
			if (this.IsRemovalLocked)
			{
				Console.LogWarning("SetStoredItem called on ItemSlot that isRemovalLocked. You probably shouldn't do this.", null);
			}
			if (_internal || this.SlotOwner == null)
			{
				if (this.ItemInstance != null)
				{
					this.ClearStoredInstance(true);
				}
				this.ItemInstance = instance;
				if (this.ItemInstance != null)
				{
					ItemInstance itemInstance = this.ItemInstance;
					itemInstance.onDataChanged = (Action)Delegate.Combine(itemInstance.onDataChanged, new Action(this.ItemDataChanged));
					ItemInstance itemInstance2 = this.ItemInstance;
					itemInstance2.requestClearSlot = (Action)Delegate.Combine(itemInstance2.requestClearSlot, new Action(this.ClearItemInstanceRequested));
				}
				if (this.onItemDataChanged != null)
				{
					this.onItemDataChanged();
				}
				if (this.onItemInstanceChanged != null)
				{
					this.onItemInstanceChanged();
				}
				this.ItemDataChanged();
				return;
			}
			this.SlotOwner.SetStoredInstance(null, this.SlotIndex, instance);
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x0010CF74 File Offset: 0x0010B174
		public virtual void InsertItem(ItemInstance item)
		{
			if (this.ItemInstance == null)
			{
				this.AddItem(item, false);
				return;
			}
			if (this.ItemInstance.CanStackWith(item, true))
			{
				this.ChangeQuantity(item.Quantity, false);
				return;
			}
			Console.LogWarning("InsertItem called with item that cannot stack with current item. Refusing.", null);
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x0010CFAF File Offset: 0x0010B1AF
		public virtual void AddItem(ItemInstance item, bool _internal = false)
		{
			if (this.ItemInstance == null)
			{
				this.SetStoredItem(item, _internal);
				return;
			}
			if (!this.ItemInstance.CanStackWith(item, true))
			{
				Console.LogWarning("AddItem called with item that cannot stack with current item. Refusing.", null);
				return;
			}
			this.ChangeQuantity(item.Quantity, _internal);
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x0010CFEC File Offset: 0x0010B1EC
		public virtual void ClearStoredInstance(bool _internal = false)
		{
			if (this.IsLocked)
			{
				Console.LogError("ClearStoredInstance called on ItemSlot that is locked! Refusing.", null);
				return;
			}
			if (this.IsRemovalLocked)
			{
				Console.LogError("ClearStoredInstance called on ItemSlot that is removal locked! Refusing.", null);
				return;
			}
			if (this.ItemInstance == null)
			{
				return;
			}
			if (_internal || this.SlotOwner == null)
			{
				ItemInstance itemInstance = this.ItemInstance;
				itemInstance.onDataChanged = (Action)Delegate.Remove(itemInstance.onDataChanged, new Action(this.ItemDataChanged));
				ItemInstance itemInstance2 = this.ItemInstance;
				itemInstance2.requestClearSlot = (Action)Delegate.Remove(itemInstance2.requestClearSlot, new Action(this.ClearItemInstanceRequested));
				this.ItemInstance = null;
				if (this.onItemDataChanged != null)
				{
					this.onItemDataChanged();
				}
				if (this.onItemInstanceChanged != null)
				{
					this.onItemInstanceChanged();
					return;
				}
			}
			else
			{
				this.SlotOwner.SetStoredInstance(null, this.SlotIndex, null);
			}
		}

		// Token: 0x0600400C RID: 16396 RVA: 0x0010D0C8 File Offset: 0x0010B2C8
		public void SetQuantity(int amount, bool _internal = false)
		{
			if (this.IsLocked)
			{
				Console.LogError("SetQuantity called on ItemSlot that is locked! Refusing.", null);
				return;
			}
			if (this.ItemInstance == null)
			{
				Console.LogWarning("ChangeQuantity called but ItemInstance is null", null);
				return;
			}
			if (amount < this.ItemInstance.Quantity && this.IsRemovalLocked)
			{
				Console.LogError("SetQuantity called on ItemSlot and passed lower quantity that current, and isRemovalLocked = true. Refusing.", null);
				return;
			}
			if (_internal || this.SlotOwner == null)
			{
				this.ItemInstance.SetQuantity(amount);
				return;
			}
			this.SlotOwner.SetItemSlotQuantity(this.SlotIndex, amount);
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x0010D14C File Offset: 0x0010B34C
		public void ChangeQuantity(int change, bool _internal = false)
		{
			if (this.IsLocked)
			{
				Console.LogWarning("isLocked = true!", null);
				return;
			}
			if (this.ItemInstance == null)
			{
				Console.LogWarning("ChangeQuantity called but ItemInstance is null", null);
				return;
			}
			if (this.IsRemovalLocked && change < 0)
			{
				Console.Log("Removal locked!", null);
				return;
			}
			if (_internal || this.SlotOwner == null)
			{
				this.ItemInstance.ChangeQuantity(change);
				return;
			}
			this.SlotOwner.SetItemSlotQuantity(this.SlotIndex, this.Quantity + change);
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x0010D1CA File Offset: 0x0010B3CA
		protected virtual void ItemDataChanged()
		{
			if (this.ItemInstance != null && this.ItemInstance.Quantity <= 0)
			{
				this.ClearStoredInstance(false);
				return;
			}
			if (this.onItemDataChanged != null)
			{
				this.onItemDataChanged();
			}
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x0010D1FD File Offset: 0x0010B3FD
		protected virtual void ClearItemInstanceRequested()
		{
			this.ClearStoredInstance(false);
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0010D206 File Offset: 0x0010B406
		public void AddFilter(ItemFilter filter)
		{
			this.Filters.Add(filter);
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x0010D214 File Offset: 0x0010B414
		public void ApplyLock(NetworkObject lockOwner, string lockReason, bool _internal = false)
		{
			if (_internal || this.SlotOwner == null)
			{
				this.ActiveLock = new ItemSlotLock(this, lockOwner, lockReason);
				if (this.onLocked != null)
				{
					this.onLocked();
					return;
				}
			}
			else
			{
				this.SlotOwner.SetSlotLocked(null, this.SlotIndex, true, lockOwner, lockReason);
			}
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x0010D264 File Offset: 0x0010B464
		public void RemoveLock(bool _internal = false)
		{
			if (_internal || this.SlotOwner == null)
			{
				this.ActiveLock = null;
				if (this.onUnlocked != null)
				{
					this.onUnlocked();
					return;
				}
			}
			else
			{
				this.SlotOwner.SetSlotLocked(null, this.SlotIndex, false, null, string.Empty);
			}
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0010D2B0 File Offset: 0x0010B4B0
		public void SetIsRemovalLocked(bool locked)
		{
			this.IsRemovalLocked = locked;
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x0010D2B9 File Offset: 0x0010B4B9
		public void SetIsAddLocked(bool locked)
		{
			this.IsAddLocked = locked;
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x0010D2C4 File Offset: 0x0010B4C4
		public virtual bool DoesItemMatchFilters(ItemInstance item)
		{
			using (List<ItemFilter>.Enumerator enumerator = this.Filters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.DoesItemMatchFilter(item))
					{
						return false;
					}
				}
			}
			return !(item is CashInstance) || this.CanSlotAcceptCash();
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0010D330 File Offset: 0x0010B530
		public virtual int GetCapacityForItem(ItemInstance item)
		{
			if (!this.DoesItemMatchFilters(item))
			{
				return 0;
			}
			if (this.ItemInstance == null || this.ItemInstance.CanStackWith(item, false))
			{
				return item.StackLimit - this.Quantity;
			}
			return 0;
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x000022C9 File Offset: 0x000004C9
		public virtual bool CanSlotAcceptCash()
		{
			return true;
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x0010D364 File Offset: 0x0010B564
		public static bool TryInsertItemIntoSet(List<ItemSlot> ItemSlots, ItemInstance item)
		{
			int num = item.Quantity;
			int num2 = 0;
			while (num2 < ItemSlots.Count && num > 0)
			{
				if (!ItemSlots[num2].IsLocked && !ItemSlots[num2].IsAddLocked && ItemSlots[num2].ItemInstance != null && ItemSlots[num2].ItemInstance.CanStackWith(item, true))
				{
					int num3 = Mathf.Min(item.StackLimit - ItemSlots[num2].ItemInstance.Quantity, num);
					num -= num3;
					ItemSlots[num2].ChangeQuantity(num3, false);
				}
				num2++;
			}
			int num4 = 0;
			while (num4 < ItemSlots.Count && num > 0)
			{
				if (!ItemSlots[num4].IsLocked && !ItemSlots[num4].IsAddLocked && ItemSlots[num4].ItemInstance == null)
				{
					num -= item.StackLimit;
					ItemSlots[num4].SetStoredItem(item, false);
					break;
				}
				num4++;
			}
			return num <= 0;
		}

		// Token: 0x04002E06 RID: 11782
		public Action onItemDataChanged;

		// Token: 0x04002E07 RID: 11783
		public Action onItemInstanceChanged;

		// Token: 0x04002E09 RID: 11785
		public Action onLocked;

		// Token: 0x04002E0A RID: 11786
		public Action onUnlocked;
	}
}
