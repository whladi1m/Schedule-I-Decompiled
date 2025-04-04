using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005C7 RID: 1479
	public class HotbarSlot : ItemSlot
	{
		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x060024C4 RID: 9412 RVA: 0x00094385 File Offset: 0x00092585
		// (set) Token: 0x060024C5 RID: 9413 RVA: 0x0009438D File Offset: 0x0009258D
		public bool IsEquipped { get; protected set; }

		// Token: 0x060024C6 RID: 9414 RVA: 0x00094398 File Offset: 0x00092598
		public override void SetStoredItem(ItemInstance instance, bool _internal = false)
		{
			if ((_internal || base.SlotOwner == null) && this.IsEquipped && this.Equippable != null)
			{
				this.Equippable.Unequip();
				this.Equippable = null;
			}
			base.SetStoredItem(instance, _internal);
			if ((_internal || base.SlotOwner == null) && this.IsEquipped && instance != null && instance.Equippable != null)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped != null)
				{
					PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped.Invoke();
				}
				this.Equippable = UnityEngine.Object.Instantiate<GameObject>(instance.Equippable.gameObject, PlayerSingleton<PlayerInventory>.Instance.equipContainer).GetComponent<Equippable>();
				this.Equippable.Equip(instance);
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x00094450 File Offset: 0x00092650
		public override void ClearStoredInstance(bool _internal = false)
		{
			if ((_internal || base.SlotOwner == null) && this.IsEquipped && this.Equippable != null)
			{
				this.Equippable.Unequip();
				this.Equippable = null;
			}
			base.ClearStoredInstance(_internal);
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x0009448C File Offset: 0x0009268C
		public virtual void Equip()
		{
			this.IsEquipped = true;
			if (base.ItemInstance != null && base.ItemInstance.Equippable != null)
			{
				if (PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped != null)
				{
					PlayerSingleton<PlayerInventory>.Instance.onPreItemEquipped.Invoke();
				}
				this.Equippable = UnityEngine.Object.Instantiate<GameObject>(base.ItemInstance.Equippable.gameObject, PlayerSingleton<PlayerInventory>.Instance.equipContainer).GetComponent<Equippable>();
				this.Equippable.Equip(base.ItemInstance);
			}
			if (this.onEquipChanged != null)
			{
				this.onEquipChanged(true);
			}
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00094525 File Offset: 0x00092725
		public virtual void Unequip()
		{
			if (this.Equippable != null)
			{
				this.Equippable.Unequip();
				this.Equippable = null;
			}
			this.IsEquipped = false;
			if (this.onEquipChanged != null)
			{
				this.onEquipChanged(false);
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00014002 File Offset: 0x00012202
		public override bool CanSlotAcceptCash()
		{
			return false;
		}

		// Token: 0x04001B6A RID: 7018
		public Equippable Equippable;

		// Token: 0x04001B6B RID: 7019
		public HotbarSlot.EquipEvent onEquipChanged;

		// Token: 0x020005C8 RID: 1480
		// (Invoke) Token: 0x060024CD RID: 9421
		public delegate void EquipEvent(bool equipped);
	}
}
