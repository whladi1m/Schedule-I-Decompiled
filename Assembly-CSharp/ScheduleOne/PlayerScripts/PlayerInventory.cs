using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Equipping;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.UI;
using ScheduleOne.UI.Items;
using ScheduleOne.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005E8 RID: 1512
	public class PlayerInventory : PlayerSingleton<PlayerInventory>
	{
		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x000A1787 File Offset: 0x0009F987
		public int TOTAL_SLOT_COUNT
		{
			get
			{
				return 9 + (this.ManagementSlotEnabled ? 1 : 0);
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x000A1798 File Offset: 0x0009F998
		// (set) Token: 0x0600277C RID: 10108 RVA: 0x000A17A0 File Offset: 0x0009F9A0
		public CashSlot cashSlot { get; private set; }

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x000A17A9 File Offset: 0x0009F9A9
		// (set) Token: 0x0600277E RID: 10110 RVA: 0x000A17B1 File Offset: 0x0009F9B1
		public CashInstance cashInstance { get; protected set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600277F RID: 10111 RVA: 0x000A17BA File Offset: 0x0009F9BA
		// (set) Token: 0x06002780 RID: 10112 RVA: 0x000A17C2 File Offset: 0x0009F9C2
		public int EquippedSlotIndex { get; protected set; } = -1;

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06002781 RID: 10113 RVA: 0x000A17CB File Offset: 0x0009F9CB
		// (set) Token: 0x06002782 RID: 10114 RVA: 0x000A17D3 File Offset: 0x0009F9D3
		public bool HotbarEnabled { get; protected set; } = true;

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002783 RID: 10115 RVA: 0x000A17DC File Offset: 0x0009F9DC
		// (set) Token: 0x06002784 RID: 10116 RVA: 0x000A17E4 File Offset: 0x0009F9E4
		public bool EquippingEnabled { get; protected set; } = true;

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x000A17ED File Offset: 0x0009F9ED
		// (set) Token: 0x06002786 RID: 10118 RVA: 0x000A17F5 File Offset: 0x0009F9F5
		public Equippable equippable { get; protected set; }

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x000A17FE File Offset: 0x0009F9FE
		public HotbarSlot equippedSlot
		{
			get
			{
				if (this.EquippedSlotIndex == -1)
				{
					return null;
				}
				return this.IndexAllSlots(this.EquippedSlotIndex);
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06002788 RID: 10120 RVA: 0x000A1817 File Offset: 0x0009FA17
		public bool isAnythingEquipped
		{
			get
			{
				return this.equippedSlot != null && this.equippedSlot.ItemInstance != null;
			}
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000A1834 File Offset: 0x0009FA34
		public HotbarSlot IndexAllSlots(int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (this.ManagementSlotEnabled)
			{
				if (index < this.hotbarSlots.Count)
				{
					return this.hotbarSlots[index];
				}
				if (index == 8)
				{
					return this.clipboardSlot;
				}
				if (index == 9)
				{
					return this.cashSlot;
				}
				return null;
			}
			else
			{
				if (index < this.hotbarSlots.Count)
				{
					return this.hotbarSlots[index];
				}
				if (index == 8)
				{
					return this.cashSlot;
				}
				return null;
			}
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000A18AA File Offset: 0x0009FAAA
		protected override void Awake()
		{
			base.Awake();
			this.SetupInventoryUI();
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000A18B8 File Offset: 0x0009FAB8
		private void SetupInventoryUI()
		{
			for (int i = 0; i < 8; i++)
			{
				HotbarSlot hotbarSlot = new HotbarSlot();
				this.hotbarSlots.Add(hotbarSlot);
				ItemSlotUI component = UnityEngine.Object.Instantiate<ItemSlotUI>(Singleton<ItemUIManager>.Instance.HotbarSlotUIPrefab, Singleton<HUD>.Instance.SlotContainer).GetComponent<ItemSlotUI>();
				component.AssignSlot(hotbarSlot);
				this.slotUIs.Add(component);
			}
			this.clipboardSlot = new ClipboardSlot();
			this.clipboardSlot.SetStoredItem(Registry.GetItem("managementclipboard").GetDefaultInstance(1), false);
			this.clipboardSlot.AddFilter(new ItemFilter_ID(new List<string>
			{
				"managementclipboard"
			}));
			this.clipboardSlot.SetIsRemovalLocked(true);
			this.clipboardSlot.SetIsAddLocked(true);
			Singleton<HUD>.Instance.managementSlotUI.AssignSlot(this.clipboardSlot);
			Singleton<HUD>.Instance.managementSlotContainer.gameObject.SetActive(false);
			this.slotUIs.Add(Singleton<HUD>.Instance.managementSlotUI);
			this.cashSlot = new CashSlot();
			this.cashSlot.SetStoredItem(Registry.GetItem("cash").GetDefaultInstance(1), false);
			this.cashInstance = (this.cashSlot.ItemInstance as CashInstance);
			this.cashSlot.AddFilter(new ItemFilter_Category(new List<EItemCategory>
			{
				EItemCategory.Cash
			}));
			Singleton<HUD>.Instance.cashSlotUI.GetComponent<CashSlotUI>().AssignSlot(this.cashSlot);
			this.slotUIs.Add(Singleton<HUD>.Instance.cashSlotUI.GetComponent<ItemSlotUI>());
			this.discardSlot = new ItemSlot();
			Singleton<HUD>.Instance.discardSlot.AssignSlot(this.discardSlot);
			this.RepositionUI();
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000A1A68 File Offset: 0x0009FC68
		private void RepositionUI()
		{
			float num = 0f;
			float num2 = 20f;
			for (int i = 0; i < 8; i++)
			{
				ItemSlotUI itemSlotUI = this.slotUIs[i];
				itemSlotUI.Rect.Find("Background/Index").GetComponent<TextMeshProUGUI>().text = ((i + 1) % 10).ToString();
				itemSlotUI.Rect.anchoredPosition = new Vector2(num + itemSlotUI.Rect.sizeDelta.x / 2f + num2, 0f);
				num += itemSlotUI.Rect.sizeDelta.x + num2;
				if (i == 7)
				{
					itemSlotUI.Rect.Find("Seperator").gameObject.SetActive(true);
					itemSlotUI.Rect.Find("Seperator").GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 0f);
					num += num2;
				}
			}
			int num3 = 8;
			if (this.ManagementSlotEnabled)
			{
				Singleton<HUD>.Instance.managementSlotUI.transform.Find("Background/Index").GetComponent<Text>().text = ((num3 + 1) % 10).ToString();
				Singleton<HUD>.Instance.managementSlotContainer.anchoredPosition = new Vector2(num + Singleton<HUD>.Instance.managementSlotContainer.sizeDelta.x / 2f + num2, 0f);
				num += Singleton<HUD>.Instance.managementSlotContainer.sizeDelta.x + num2;
				num3++;
			}
			Singleton<HUD>.Instance.managementSlotContainer.gameObject.SetActive(this.ManagementSlotEnabled);
			Singleton<HUD>.Instance.cashSlotUI.Find("Background/Index").GetComponent<Text>().text = ((num3 + 1) % 10).ToString();
			Singleton<HUD>.Instance.cashSlotContainer.anchoredPosition = new Vector2(num + Singleton<HUD>.Instance.cashSlotContainer.sizeDelta.x / 2f + num2, 0f);
			num += Singleton<HUD>.Instance.cashSlotContainer.sizeDelta.x + num2;
			Singleton<HUD>.Instance.SlotContainer.anchoredPosition = new Vector2(-num / 2f, Singleton<HUD>.Instance.SlotContainer.anchoredPosition.y);
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000A1CB8 File Offset: 0x0009FEB8
		protected override void Start()
		{
			base.Start();
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				HotbarSlot slot = this.hotbarSlots[i];
				Player.Local.SetInventoryItem(i, slot.ItemInstance);
				int index = i;
				HotbarSlot slot2 = slot;
				slot2.onItemDataChanged = (Action)Delegate.Combine(slot2.onItemDataChanged, new Action(delegate()
				{
					this.UpdateInventoryVariables();
					Player.Local.SetInventoryItem(index, slot.ItemInstance);
				}));
			}
			Player.Local.SetInventoryItem(8, this.cashSlot.ItemInstance);
			CashSlot cashSlot = this.cashSlot;
			cashSlot.onItemDataChanged = (Action)Delegate.Combine(cashSlot.onItemDataChanged, new Action(delegate()
			{
				this.UpdateInventoryVariables();
				Player.Local.SetInventoryItem(8, this.cashSlot.ItemInstance);
			}));
			if (this.giveStartupItems)
			{
				this.GiveStartupItems();
			}
			if (!GameManager.IS_TUTORIAL)
			{
				BoolVariable boolVariable = NetworkSingleton<VariableDatabase>.Instance.GetVariable("ClipboardAcquired") as BoolVariable;
				if (boolVariable.Value)
				{
					this.ClipboardAcquiredVarChange(true);
					return;
				}
				boolVariable.OnValueChanged.AddListener(new UnityAction<bool>(this.ClipboardAcquiredVarChange));
			}
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000A1DD4 File Offset: 0x0009FFD4
		private void GiveStartupItems()
		{
			if (!Application.isEditor && !Debug.isDebugBuild)
			{
				return;
			}
			foreach (PlayerInventory.ItemAmount itemAmount in this.startupItems)
			{
				this.AddItemToInventory(itemAmount.Definition.GetDefaultInstance(itemAmount.Amount));
			}
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000A1E48 File Offset: 0x000A0048
		protected virtual void Update()
		{
			this.UpdateHotbarSelection();
			if (this.isAnythingEquipped && this.HotbarEnabled)
			{
				this.currentEquipTime += Time.deltaTime;
			}
			else
			{
				this.currentEquipTime = 0f;
			}
			if (this.isAnythingEquipped)
			{
				Singleton<HUD>.Instance.selectedItemLabel.text = this.equippedSlot.ItemInstance.Name;
				Singleton<HUD>.Instance.selectedItemLabel.color = this.equippedSlot.ItemInstance.LabelDisplayColor;
				if (this.currentEquipTime > 2f)
				{
					float num = Mathf.Clamp01((this.currentEquipTime - 2f) / 0.5f);
					Singleton<HUD>.Instance.selectedItemLabel.color = new Color(Singleton<HUD>.Instance.selectedItemLabel.color.r, Singleton<HUD>.Instance.selectedItemLabel.color.g, Singleton<HUD>.Instance.selectedItemLabel.color.b, 1f - num);
				}
				else
				{
					Singleton<HUD>.Instance.selectedItemLabel.color = new Color(Singleton<HUD>.Instance.selectedItemLabel.color.r, Singleton<HUD>.Instance.selectedItemLabel.color.g, Singleton<HUD>.Instance.selectedItemLabel.color.b, 1f);
				}
			}
			else
			{
				Singleton<HUD>.Instance.selectedItemLabel.text = string.Empty;
			}
			if (this.discardSlot.ItemInstance != null && !Singleton<HUD>.Instance.discardSlot.IsBeingDragged)
			{
				this.currentDiscardTime += Time.deltaTime;
				Singleton<HUD>.Instance.discardSlotFill.fillAmount = this.currentDiscardTime / 1.5f;
				if (this.currentDiscardTime >= 1.5f)
				{
					this.discardSlot.ClearStoredInstance(false);
					return;
				}
			}
			else
			{
				this.currentDiscardTime = 0f;
				Singleton<HUD>.Instance.discardSlotFill.fillAmount = 0f;
			}
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000A2040 File Offset: 0x000A0240
		private void UpdateHotbarSelection()
		{
			if (!this.HotbarEnabled)
			{
				return;
			}
			if (!this.EquippingEnabled)
			{
				return;
			}
			if (GameInput.IsTyping)
			{
				return;
			}
			if (Singleton<PauseMenu>.Instance.IsPaused)
			{
				return;
			}
			int num = -1;
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				num = 0;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				num = 1;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				num = 2;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				num = 3;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				num = 4;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				num = 5;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				num = 6;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				num = 7;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				num = 8;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				num = 9;
			}
			if (num == -1)
			{
				float mouseScrollDelta = GameInput.MouseScrollDelta;
				if (mouseScrollDelta < 0f)
				{
					num = this.EquippedSlotIndex + 1;
					if (num >= this.TOTAL_SLOT_COUNT)
					{
						num = 0;
					}
				}
				else if (mouseScrollDelta > 0f)
				{
					num = this.EquippedSlotIndex - 1;
					if (num < 0)
					{
						num = this.TOTAL_SLOT_COUNT - 1;
					}
				}
			}
			if (num == -1 && GameInput.GetButtonDown(GameInput.ButtonCode.TertiaryClick))
			{
				if (this.EquippedSlotIndex != -1)
				{
					num = this.EquippedSlotIndex;
				}
				else if (this.PreviousEquippedSlotIndex != -1)
				{
					num = this.PreviousEquippedSlotIndex;
				}
			}
			if (num != -1)
			{
				if (num >= this.TOTAL_SLOT_COUNT)
				{
					return;
				}
				if (num != this.EquippedSlotIndex && this.EquippedSlotIndex != -1)
				{
					this.IndexAllSlots(this.EquippedSlotIndex).Unequip();
					this.currentEquipTime = 0f;
				}
				this.PreviousEquippedSlotIndex = this.EquippedSlotIndex;
				this.EquippedSlotIndex = -1;
				if (this.IndexAllSlots(num).IsEquipped)
				{
					this.IndexAllSlots(num).Unequip();
					return;
				}
				this.Equip(this.IndexAllSlots(num));
				this.EquippedSlotIndex = num;
				PlayerSingleton<ViewmodelSway>.Instance.RefreshViewmodel();
			}
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000A21F6 File Offset: 0x000A03F6
		public void Equip(HotbarSlot slot)
		{
			slot.Equip();
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000A21FE File Offset: 0x000A03FE
		public void SetInventoryEnabled(bool enabled)
		{
			this.HotbarEnabled = enabled;
			if (this.onInventoryStateChanged != null)
			{
				this.onInventoryStateChanged(enabled);
			}
			Singleton<HUD>.Instance.HotbarContainer.gameObject.SetActive(enabled);
			this.SetEquippingEnabled(enabled);
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000A2238 File Offset: 0x000A0438
		public void SetEquippingEnabled(bool enabled)
		{
			if (this.EquippingEnabled == enabled)
			{
				return;
			}
			this.EquippingEnabled = enabled;
			this.equipContainer.gameObject.SetActive(enabled);
			if (enabled)
			{
				if (this.PriorEquippedSlotIndex != -1)
				{
					this.EquippedSlotIndex = this.PriorEquippedSlotIndex;
					this.Equip(this.IndexAllSlots(this.EquippedSlotIndex));
				}
			}
			else
			{
				this.PriorEquippedSlotIndex = this.EquippedSlotIndex;
				if (this.EquippedSlotIndex != -1)
				{
					this.IndexAllSlots(this.EquippedSlotIndex).Unequip();
					this.EquippedSlotIndex = -1;
				}
			}
			foreach (ItemSlotUI itemSlotUI in this.slotUIs)
			{
				itemSlotUI.Rect.Find("Background/Index").gameObject.SetActive(enabled);
			}
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000A2318 File Offset: 0x000A0518
		private void ClipboardAcquiredVarChange(bool newVal)
		{
			this.SetManagementClipboardEnabled(newVal);
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000A2321 File Offset: 0x000A0521
		public void SetManagementClipboardEnabled(bool enabled)
		{
			if (GameManager.IS_TUTORIAL)
			{
				enabled = false;
			}
			this.ManagementSlotEnabled = enabled;
			this.RepositionUI();
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000A233C File Offset: 0x000A053C
		public void SetViewmodelVisible(bool visible)
		{
			PlayerSingleton<PlayerCamera>.Instance.Camera.cullingMask = (visible ? (PlayerSingleton<PlayerCamera>.Instance.Camera.cullingMask | 1 << LayerMask.NameToLayer("Viewmodel")) : (PlayerSingleton<PlayerCamera>.Instance.Camera.cullingMask & ~(1 << LayerMask.NameToLayer("Viewmodel"))));
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000A239C File Offset: 0x000A059C
		public bool CanItemFitInInventory(ItemInstance item, int quantity = 1)
		{
			if (item == null)
			{
				Console.LogWarning("CanItemFitInInventory: item is null!", null);
				return false;
			}
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance == null)
				{
					quantity -= item.StackLimit;
				}
				else if (this.hotbarSlots[i].ItemInstance.CanStackWith(item, true))
				{
					quantity -= item.StackLimit - this.hotbarSlots[i].ItemInstance.Quantity;
				}
			}
			return quantity <= 0;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000A2430 File Offset: 0x000A0630
		public void AddItemToInventory(ItemInstance item)
		{
			if (item == null)
			{
				Console.LogError("AddItemToInventory: item is null!", null);
				return;
			}
			if (!item.IsValidInstance())
			{
				Console.LogError("AddItemToInventory: item is not valid!", null);
				return;
			}
			if (!this.CanItemFitInInventory(item, 1))
			{
				Console.LogWarning("AddItemToInventory: item won't fit!", null);
				return;
			}
			int num = item.Quantity;
			int num2 = 0;
			while (num2 < this.hotbarSlots.Count && num != 0)
			{
				if (this.hotbarSlots[num2].ItemInstance != null && this.hotbarSlots[num2].ItemInstance.CanStackWith(item, false))
				{
					int num3 = Mathf.Min(num, this.hotbarSlots[num2].ItemInstance.StackLimit - this.hotbarSlots[num2].Quantity);
					if (num3 > 0)
					{
						this.hotbarSlots[num2].ChangeQuantity(num3, false);
						num -= num3;
					}
				}
				num2++;
			}
			int num4 = 0;
			while (num4 < this.hotbarSlots.Count && num != 0)
			{
				if (this.hotbarSlots[num4].ItemInstance == null)
				{
					this.hotbarSlots[num4].SetStoredItem(item.GetCopy(num), false);
					num = 0;
				}
				num4++;
			}
			if (num > 0)
			{
				Console.LogWarning("Could not add full amount of '" + item.Name + "' to inventory!", null);
			}
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000A257C File Offset: 0x000A077C
		public uint GetAmountOfItem(string ID)
		{
			uint num = 0U;
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null && this.hotbarSlots[i].ItemInstance.ID.ToLower() == ID.ToLower())
				{
					num += (uint)this.hotbarSlots[i].Quantity;
				}
			}
			return num;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000A25F4 File Offset: 0x000A07F4
		public void RemoveAmountOfItem(string ID, uint amount = 1U)
		{
			uint num = amount;
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null && this.hotbarSlots[i].ItemInstance.ID.ToLower() == ID.ToLower())
				{
					uint num2 = num;
					if ((ulong)num2 > (ulong)((long)this.hotbarSlots[i].Quantity))
					{
						num2 = (uint)this.hotbarSlots[i].Quantity;
					}
					num -= num2;
					this.hotbarSlots[i].ChangeQuantity((int)(-(int)num2), false);
					if (num <= 0U)
					{
						break;
					}
				}
			}
			if (num > 0U)
			{
				Console.LogWarning("Could not fully remove " + amount.ToString() + " " + ID, null);
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000A26C4 File Offset: 0x000A08C4
		public void ClearInventory()
		{
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null)
				{
					this.hotbarSlots[i].ClearStoredInstance(false);
				}
			}
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000A270C File Offset: 0x000A090C
		public void RemoveProductFromInventory(EStealthLevel maxStealth)
		{
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null && this.hotbarSlots[i].ItemInstance is ProductItemInstance)
				{
					ProductItemInstance productItemInstance = this.hotbarSlots[i].ItemInstance as ProductItemInstance;
					EStealthLevel estealthLevel = EStealthLevel.None;
					if (productItemInstance.AppliedPackaging != null)
					{
						estealthLevel = productItemInstance.AppliedPackaging.StealthLevel;
					}
					if (estealthLevel <= maxStealth)
					{
						this.hotbarSlots[i].ClearStoredInstance(false);
					}
				}
			}
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000A27A8 File Offset: 0x000A09A8
		public void RemoveRandomItemsFromInventory()
		{
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				if (this.hotbarSlots[i].ItemInstance != null && UnityEngine.Random.Range(0, 3) == 0)
				{
					int num = UnityEngine.Random.Range(1, this.hotbarSlots[i].ItemInstance.Quantity + 1);
					this.hotbarSlots[i].ChangeQuantity(-num, false);
				}
			}
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000A281D File Offset: 0x000A0A1D
		public void SetEquippable(Equippable eq)
		{
			this.equippable = eq;
			if (this.equippable != null && this.onItemEquipped != null)
			{
				this.onItemEquipped.Invoke();
			}
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000A2848 File Offset: 0x000A0A48
		public void Reequip()
		{
			HotbarSlot equippedSlot = this.equippedSlot;
			if (equippedSlot != null)
			{
				equippedSlot.Unequip();
				this.currentEquipTime = 0f;
				this.Equip(equippedSlot);
			}
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000A2878 File Offset: 0x000A0A78
		public List<ItemSlot> GetAllInventorySlots()
		{
			List<ItemSlot> list = new List<ItemSlot>();
			for (int i = 0; i < this.hotbarSlots.Count; i++)
			{
				list.Add(this.hotbarSlots[i]);
			}
			list.Add(this.cashSlot);
			return list;
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000A28C0 File Offset: 0x000A0AC0
		private void UpdateInventoryVariables()
		{
			if (!NetworkSingleton<VariableDatabase>.InstanceExists)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.ItemVariables.Count; i++)
			{
				int num3 = 0;
				for (int j = 0; j < this.hotbarSlots.Count; j++)
				{
					if (this.hotbarSlots[j].ItemInstance != null && this.hotbarSlots[j].ItemInstance.ID.ToLower() == this.ItemVariables[i].Definition.ID.ToLower())
					{
						num3 += this.hotbarSlots[j].Quantity;
					}
					if (this.hotbarSlots[j].ItemInstance != null && NetworkSingleton<ProductManager>.Instance.ValidMixIngredients.Contains(this.hotbarSlots[j].ItemInstance.Definition))
					{
						num += this.hotbarSlots[j].Quantity;
					}
				}
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.ItemVariables[i].VariableName, num3.ToString(), false);
			}
			int num4 = 0;
			for (int k = 0; k < this.hotbarSlots.Count; k++)
			{
				if (this.hotbarSlots[k].ItemInstance != null && this.hotbarSlots[k].ItemInstance is ProductItemInstance)
				{
					if (this.hotbarSlots[k].ItemInstance is ProductItemInstance && (this.hotbarSlots[k].ItemInstance as ProductItemInstance).AppliedPackaging != null)
					{
						num4 += this.hotbarSlots[k].Quantity;
					}
					if (this.hotbarSlots[k].ItemInstance is WeedInstance)
					{
						num2 += this.hotbarSlots[k].Quantity;
					}
				}
			}
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Inventory_Weed_Count", num2.ToString(), false);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Inventory_Packaged_Product", num4.ToString(), false);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Inventory_MixingIngredients", num.ToString(), false);
		}

		// Token: 0x04001CAA RID: 7338
		public const float LABEL_DISPLAY_TIME = 2f;

		// Token: 0x04001CAB RID: 7339
		public const float LABEL_FADE_TIME = 0.5f;

		// Token: 0x04001CAC RID: 7340
		public const float DISCARD_TIME = 1.5f;

		// Token: 0x04001CAD RID: 7341
		public const int INVENTORY_SLOT_COUNT = 8;

		// Token: 0x04001CAE RID: 7342
		[Header("Startup Items (Editor only)")]
		[SerializeField]
		private bool giveStartupItems;

		// Token: 0x04001CAF RID: 7343
		[SerializeField]
		private List<PlayerInventory.ItemAmount> startupItems = new List<PlayerInventory.ItemAmount>();

		// Token: 0x04001CB0 RID: 7344
		[Header("References")]
		public Transform equipContainer;

		// Token: 0x04001CB1 RID: 7345
		public List<HotbarSlot> hotbarSlots = new List<HotbarSlot>();

		// Token: 0x04001CB4 RID: 7348
		private ClipboardSlot clipboardSlot;

		// Token: 0x04001CB5 RID: 7349
		private List<ItemSlotUI> slotUIs = new List<ItemSlotUI>();

		// Token: 0x04001CB6 RID: 7350
		private ItemSlot discardSlot;

		// Token: 0x04001CB7 RID: 7351
		[Header("Item Variables")]
		public List<PlayerInventory.ItemVariable> ItemVariables = new List<PlayerInventory.ItemVariable>();

		// Token: 0x04001CBC RID: 7356
		public Action<bool> onInventoryStateChanged;

		// Token: 0x04001CBD RID: 7357
		private int PriorEquippedSlotIndex = -1;

		// Token: 0x04001CBE RID: 7358
		private int PreviousEquippedSlotIndex = -1;

		// Token: 0x04001CBF RID: 7359
		public UnityEvent onPreItemEquipped;

		// Token: 0x04001CC0 RID: 7360
		public UnityEvent onItemEquipped;

		// Token: 0x04001CC1 RID: 7361
		private bool ManagementSlotEnabled;

		// Token: 0x04001CC2 RID: 7362
		public float currentEquipTime;

		// Token: 0x04001CC3 RID: 7363
		protected float currentDiscardTime;

		// Token: 0x020005E9 RID: 1513
		[Serializable]
		public class ItemVariable
		{
			// Token: 0x04001CC4 RID: 7364
			public ItemDefinition Definition;

			// Token: 0x04001CC5 RID: 7365
			public string VariableName;
		}

		// Token: 0x020005EA RID: 1514
		[Serializable]
		private class ItemAmount
		{
			// Token: 0x04001CC6 RID: 7366
			public ItemDefinition Definition;

			// Token: 0x04001CC7 RID: 7367
			public int Amount = 10;
		}
	}
}
