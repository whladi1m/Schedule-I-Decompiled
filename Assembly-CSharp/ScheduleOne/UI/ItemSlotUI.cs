using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009E0 RID: 2528
	public class ItemSlotUI : MonoBehaviour
	{
		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06004434 RID: 17460 RVA: 0x0011D8CF File Offset: 0x0011BACF
		// (set) Token: 0x06004435 RID: 17461 RVA: 0x0011D8D7 File Offset: 0x0011BAD7
		public ItemSlot assignedSlot { get; protected set; }

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06004436 RID: 17462 RVA: 0x0011D8E0 File Offset: 0x0011BAE0
		// (set) Token: 0x06004437 RID: 17463 RVA: 0x0011D8E8 File Offset: 0x0011BAE8
		public ItemUI ItemUI { get; protected set; }

		// Token: 0x06004438 RID: 17464 RVA: 0x0011D8F4 File Offset: 0x0011BAF4
		public virtual void AssignSlot(ItemSlot s)
		{
			if (s == null)
			{
				Console.LogWarning("AssignSlot passed null slot. Use ClearSlot() instead", null);
			}
			this.assignedSlot = s;
			ItemSlot assignedSlot = this.assignedSlot;
			assignedSlot.onItemInstanceChanged = (Action)Delegate.Combine(assignedSlot.onItemInstanceChanged, new Action(this.UpdateUI));
			ItemSlot assignedSlot2 = this.assignedSlot;
			assignedSlot2.onLocked = (Action)Delegate.Combine(assignedSlot2.onLocked, new Action(this.Lock));
			ItemSlot assignedSlot3 = this.assignedSlot;
			assignedSlot3.onUnlocked = (Action)Delegate.Combine(assignedSlot3.onUnlocked, new Action(this.Unlock));
			this.SetHighlighted(false);
			if (this.assignedSlot is HotbarSlot)
			{
				HotbarSlot hotbarSlot = this.assignedSlot as HotbarSlot;
				hotbarSlot.onEquipChanged = (HotbarSlot.EquipEvent)Delegate.Combine(hotbarSlot.onEquipChanged, new HotbarSlot.EquipEvent(this.SetHighlighted));
			}
			if (s.IsLocked)
			{
				this.SetLockVisible(true);
			}
			this.UpdateUI();
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x0011D9E4 File Offset: 0x0011BBE4
		public virtual void ClearSlot()
		{
			if (this.assignedSlot != null)
			{
				ItemSlot assignedSlot = this.assignedSlot;
				assignedSlot.onItemInstanceChanged = (Action)Delegate.Remove(assignedSlot.onItemInstanceChanged, new Action(this.UpdateUI));
				ItemSlot assignedSlot2 = this.assignedSlot;
				assignedSlot2.onLocked = (Action)Delegate.Remove(assignedSlot2.onLocked, new Action(this.Lock));
				ItemSlot assignedSlot3 = this.assignedSlot;
				assignedSlot3.onUnlocked = (Action)Delegate.Remove(assignedSlot3.onUnlocked, new Action(this.Unlock));
				if (this.assignedSlot is HotbarSlot)
				{
					HotbarSlot hotbarSlot = this.assignedSlot as HotbarSlot;
					hotbarSlot.onEquipChanged = (HotbarSlot.EquipEvent)Delegate.Remove(hotbarSlot.onEquipChanged, new HotbarSlot.EquipEvent(this.SetHighlighted));
				}
				this.assignedSlot = null;
				this.SetLockVisible(false);
				this.UpdateUI();
			}
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0011DABF File Offset: 0x0011BCBF
		public void OnDestroy()
		{
			if (this.assignedSlot != null)
			{
				ItemSlot assignedSlot = this.assignedSlot;
				assignedSlot.onItemInstanceChanged = (Action)Delegate.Remove(assignedSlot.onItemInstanceChanged, new Action(this.UpdateUI));
			}
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x0011DAF4 File Offset: 0x0011BCF4
		public virtual void UpdateUI()
		{
			if (this.ItemUI != null)
			{
				this.ItemUI.Destroy();
				this.ItemUI = null;
			}
			if (this.assignedSlot != null && this.assignedSlot.ItemInstance != null)
			{
				ItemUI original = Singleton<ItemUIManager>.Instance.DefaultItemUIPrefab;
				if (this.assignedSlot.ItemInstance.Definition.CustomItemUI != null)
				{
					original = this.assignedSlot.ItemInstance.Definition.CustomItemUI;
				}
				this.ItemUI = UnityEngine.Object.Instantiate<ItemUI>(original, this.ItemContainer).GetComponent<ItemUI>();
				this.ItemUI.transform.SetAsLastSibling();
				this.ItemUI.Setup(this.assignedSlot.ItemInstance);
			}
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0011DBB4 File Offset: 0x0011BDB4
		public void SetHighlighted(bool h)
		{
			if (h)
			{
				this.Background.color = this.highlightColor;
				return;
			}
			this.Background.color = this.normalColor;
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0011DBE6 File Offset: 0x0011BDE6
		public void SetNormalColor(Color color)
		{
			this.normalColor = color;
			this.SetHighlighted(false);
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x0011DBFB File Offset: 0x0011BDFB
		public void SetHighlightColor(Color color)
		{
			this.highlightColor = color;
			this.SetHighlighted(false);
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x0011DC10 File Offset: 0x0011BE10
		private void Lock()
		{
			this.SetLockVisible(true);
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0011DC19 File Offset: 0x0011BE19
		private void Unlock()
		{
			this.SetLockVisible(false);
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0011DC22 File Offset: 0x0011BE22
		public void SetLockVisible(bool vis)
		{
			this.LockContainer.gameObject.SetActive(vis);
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0011DC35 File Offset: 0x0011BE35
		public RectTransform DuplicateIcon(Transform parent, int overriddenQuantity = -1)
		{
			if (this.ItemUI == null)
			{
				return null;
			}
			return this.ItemUI.DuplicateIcon(parent, overriddenQuantity);
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x0011DC54 File Offset: 0x0011BE54
		public void SetVisible(bool shown)
		{
			if (this.ItemUI != null)
			{
				this.ItemUI.SetVisible(shown);
			}
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x0011DC70 File Offset: 0x0011BE70
		public void OverrideDisplayedQuantity(int quantity)
		{
			if (this.ItemUI == null)
			{
				return;
			}
			this.ItemUI.SetDisplayedQuantity(quantity);
		}

		// Token: 0x04003210 RID: 12816
		public Color32 normalColor = new Color32(140, 140, 140, 40);

		// Token: 0x04003211 RID: 12817
		public Color32 highlightColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 60);

		// Token: 0x04003213 RID: 12819
		[HideInInspector]
		public bool IsBeingDragged;

		// Token: 0x04003214 RID: 12820
		[Header("References")]
		public RectTransform Rect;

		// Token: 0x04003215 RID: 12821
		public Image Background;

		// Token: 0x04003216 RID: 12822
		public GameObject LockContainer;

		// Token: 0x04003217 RID: 12823
		public RectTransform ItemContainer;
	}
}
