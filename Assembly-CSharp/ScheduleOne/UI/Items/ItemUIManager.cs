using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Items
{
	// Token: 0x02000B49 RID: 2889
	public class ItemUIManager : Singleton<ItemUIManager>
	{
		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x00143312 File Offset: 0x00141512
		// (set) Token: 0x06004CC6 RID: 19654 RVA: 0x0014331A File Offset: 0x0014151A
		public bool DraggingEnabled { get; protected set; }

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06004CC7 RID: 19655 RVA: 0x00143323 File Offset: 0x00141523
		// (set) Token: 0x06004CC8 RID: 19656 RVA: 0x0014332B File Offset: 0x0014152B
		public ItemSlotUI HoveredSlot { get; protected set; }

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06004CC9 RID: 19657 RVA: 0x00143334 File Offset: 0x00141534
		// (set) Token: 0x06004CCA RID: 19658 RVA: 0x0014333C File Offset: 0x0014153C
		public bool QuickMoveEnabled { get; protected set; }

		// Token: 0x06004CCB RID: 19659 RVA: 0x00143345 File Offset: 0x00141545
		protected override void Awake()
		{
			base.Awake();
			this.InputsContainer.gameObject.SetActive(false);
			this.ItemQuantityPrompt.gameObject.SetActive(false);
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x00143370 File Offset: 0x00141570
		protected virtual void Update()
		{
			this.HoveredSlot = null;
			if (this.DraggingEnabled)
			{
				CursorManager.ECursorType cursorAppearance = CursorManager.ECursorType.Default;
				this.HoveredSlot = this.GetHoveredItemSlot();
				if (this.HoveredSlot != null && this.CanDragFromSlot(this.HoveredSlot))
				{
					cursorAppearance = CursorManager.ECursorType.OpenHand;
				}
				if (this.HoveredSlot != null && this.draggedSlot == null && this.HoveredSlot.assignedSlot != null && this.HoveredSlot.assignedSlot.Quantity > 0)
				{
					if (this.InfoPanel.CurrentItem != this.HoveredSlot.assignedSlot.ItemInstance)
					{
						this.InfoPanel.Open(this.HoveredSlot.assignedSlot.ItemInstance, this.HoveredSlot.Rect);
					}
				}
				else
				{
					ItemDefinitionInfoHoverable hoveredItemInfo = this.GetHoveredItemInfo();
					if (hoveredItemInfo != null)
					{
						this.InfoPanel.Open(hoveredItemInfo.AssignedItem, hoveredItemInfo.transform as RectTransform);
					}
					else if (this.InfoPanel.IsOpen)
					{
						this.InfoPanel.Close();
					}
				}
				if (this.draggedSlot != null)
				{
					cursorAppearance = CursorManager.ECursorType.Grab;
					if (!GameInput.GetButton(GameInput.ButtonCode.PrimaryClick) && !GameInput.GetButton(GameInput.ButtonCode.SecondaryClick) && !GameInput.GetButton(GameInput.ButtonCode.TertiaryClick))
					{
						this.EndDrag();
					}
				}
				else if ((GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick) || GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick) || GameInput.GetButtonDown(GameInput.ButtonCode.TertiaryClick)) && this.HoveredSlot != null)
				{
					this.SlotClicked(this.HoveredSlot);
				}
				Singleton<CursorManager>.Instance.SetCursorAppearance(cursorAppearance);
			}
			if (this.draggedSlot != null && this.customDragAmount)
			{
				if (this.isDraggingCash)
				{
					CashInstance instance = this.draggedSlot.assignedSlot.ItemInstance as CashInstance;
					this.UpdateCashDragAmount(instance);
					return;
				}
				if (GameInput.MouseScrollDelta > 0f)
				{
					this.SetDraggedAmount(Mathf.Clamp(this.draggedAmount + 1, 1, this.draggedSlot.assignedSlot.Quantity));
					return;
				}
				if (GameInput.MouseScrollDelta < 0f)
				{
					this.SetDraggedAmount(Mathf.Clamp(this.draggedAmount - 1, 1, this.draggedSlot.assignedSlot.Quantity));
				}
			}
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x00143594 File Offset: 0x00141794
		protected virtual void LateUpdate()
		{
			if (this.DraggingEnabled && this.draggedSlot != null)
			{
				this.tempIcon.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - this.mouseOffset;
				if (this.customDragAmount)
				{
					this.ItemQuantityPrompt.position = this.tempIcon.position + new Vector3(0f, this.tempIcon.rect.height * 0.5f + 25f, 0f);
				}
			}
			this.UpdateCashDragSelectorUI();
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x00143648 File Offset: 0x00141848
		private void UpdateCashDragSelectorUI()
		{
			if (this.draggedSlot != null && this.draggedSlot.assignedSlot != null && this.draggedSlot.assignedSlot.ItemInstance != null && this.draggedSlot.assignedSlot.ItemInstance is CashInstance && this.customDragAmount)
			{
				ItemInstance itemInstance = this.draggedSlot.assignedSlot.ItemInstance;
				this.tempIcon.Find("Balance").GetComponent<TextMeshProUGUI>().text = MoneyManager.FormatAmount(this.draggedCashAmount, false, false);
				this.CashDragAmountContainer.position = this.tempIcon.position + new Vector3(0f, this.tempIcon.rect.height * 0.5f + 15f, 0f);
				this.CashDragAmountContainer.gameObject.SetActive(true);
				return;
			}
			this.CashDragAmountContainer.gameObject.SetActive(false);
		}

		// Token: 0x06004CCF RID: 19663 RVA: 0x00143754 File Offset: 0x00141954
		private void UpdateCashDragAmount(CashInstance instance)
		{
			float[] array = new float[]
			{
				50f,
				10f,
				1f
			};
			float[] array2 = new float[]
			{
				100f,
				10f,
				1f
			};
			float num = 0f;
			if (GameInput.MouseScrollDelta > 0f)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (this.draggedCashAmount >= array2[i])
					{
						num = array[i];
						break;
					}
				}
			}
			else if (GameInput.MouseScrollDelta < 0f)
			{
				for (int j = 0; j < array.Length; j++)
				{
					if (this.draggedCashAmount > array2[j])
					{
						num = -array[j];
						break;
					}
				}
			}
			if (num == 0f)
			{
				return;
			}
			this.draggedCashAmount = Mathf.Clamp(this.draggedCashAmount + num, 1f, Mathf.Min(instance.Balance, 1000f));
		}

		// Token: 0x06004CD0 RID: 19664 RVA: 0x0014381C File Offset: 0x00141A1C
		public void SetDraggingEnabled(bool enabled, bool modifierPromptsVisible = true)
		{
			this.DraggingEnabled = enabled;
			if (!this.DraggingEnabled && this.draggedSlot != null)
			{
				this.EndDrag();
			}
			if (this.InfoPanel.IsOpen)
			{
				this.InfoPanel.Close();
			}
			if (!enabled)
			{
				this.DisableQuickMove();
			}
			this.InputsContainer.gameObject.SetActive(this.DraggingEnabled && modifierPromptsVisible);
			Singleton<HUD>.Instance.discardSlot.gameObject.SetActive(this.DraggingEnabled);
		}

		// Token: 0x06004CD1 RID: 19665 RVA: 0x001438A0 File Offset: 0x00141AA0
		public void EnableQuickMove(List<ItemSlot> primarySlots, List<ItemSlot> secondarySlots)
		{
			this.QuickMoveEnabled = true;
			this.PrimarySlots = new List<ItemSlot>();
			this.PrimarySlots.AddRange(primarySlots);
			this.SecondarySlots = new List<ItemSlot>();
			this.SecondarySlots.AddRange(secondarySlots);
			this.InputsContainer.gameObject.SetActive(this.QuickMoveEnabled);
		}

		// Token: 0x06004CD2 RID: 19666 RVA: 0x001438F8 File Offset: 0x00141AF8
		private List<ItemSlot> GetQuickMoveSlots(ItemSlot sourceSlot)
		{
			if (sourceSlot == null || sourceSlot.ItemInstance == null)
			{
				return new List<ItemSlot>();
			}
			List<ItemSlot> list = this.PrimarySlots.Contains(sourceSlot) ? this.SecondarySlots : this.PrimarySlots;
			List<ItemSlot> list2 = new List<ItemSlot>();
			foreach (ItemSlot itemSlot in list)
			{
				if (!itemSlot.IsLocked && !itemSlot.IsAddLocked && !itemSlot.IsRemovalLocked && itemSlot.DoesItemMatchFilters(sourceSlot.ItemInstance) && (itemSlot.GetCapacityForItem(sourceSlot.ItemInstance) > 0 || sourceSlot.ItemInstance is CashInstance))
				{
					list2.Add(itemSlot);
				}
			}
			return list2;
		}

		// Token: 0x06004CD3 RID: 19667 RVA: 0x001439BC File Offset: 0x00141BBC
		public void DisableQuickMove()
		{
			this.QuickMoveEnabled = false;
		}

		// Token: 0x06004CD4 RID: 19668 RVA: 0x001439C8 File Offset: 0x00141BC8
		private ItemSlotUI GetHoveredItemSlot()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;
			foreach (GraphicRaycaster baseRaycaster in this.Raycasters)
			{
				List<RaycastResult> list = new List<RaycastResult>();
				baseRaycaster.Raycast(pointerEventData, list);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].gameObject.GetComponentInParent<ItemSlotUI>())
					{
						return list[j].gameObject.GetComponentInParent<ItemSlotUI>();
					}
				}
			}
			return null;
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x00143A60 File Offset: 0x00141C60
		private ItemDefinitionInfoHoverable GetHoveredItemInfo()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;
			foreach (GraphicRaycaster baseRaycaster in this.Raycasters)
			{
				List<RaycastResult> list = new List<RaycastResult>();
				baseRaycaster.Raycast(pointerEventData, list);
				for (int j = 0; j < list.Count; j++)
				{
					ItemDefinitionInfoHoverable componentInParent = list[j].gameObject.GetComponentInParent<ItemDefinitionInfoHoverable>();
					if (componentInParent != null && componentInParent.enabled)
					{
						return componentInParent;
					}
				}
			}
			return null;
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x00143AF4 File Offset: 0x00141CF4
		private void SlotClicked(ItemSlotUI ui)
		{
			if (!this.CanDragFromSlot(ui))
			{
				return;
			}
			if (this.DraggingEnabled)
			{
				if (this.draggedSlot != null)
				{
					return;
				}
				if (ui.assignedSlot.ItemInstance != null && !ui.assignedSlot.IsLocked && !ui.assignedSlot.IsRemovalLocked)
				{
					this.mouseOffset = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - new Vector2(ui.ItemUI.Rect.position.x, ui.ItemUI.Rect.position.y);
					this.draggedSlot = ui;
					this.isDraggingCash = (this.draggedSlot.assignedSlot.ItemInstance is CashInstance);
					if (this.isDraggingCash)
					{
						this.StartDragCash();
						return;
					}
					this.customDragAmount = false;
					this.draggedAmount = this.draggedSlot.assignedSlot.Quantity;
					if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
					{
						this.draggedAmount = 1;
						this.customDragAmount = true;
						this.mouseOffset += new Vector2(-10f, -15f);
					}
					if (GameInput.GetButton(GameInput.ButtonCode.QuickMove) && this.QuickMoveEnabled)
					{
						List<ItemSlot> quickMoveSlots = this.GetQuickMoveSlots(this.draggedSlot.assignedSlot);
						if (quickMoveSlots.Count > 0)
						{
							int num = 0;
							int num2 = 0;
							while (num2 < quickMoveSlots.Count && num < this.draggedAmount)
							{
								if (quickMoveSlots[num2].ItemInstance != null && quickMoveSlots[num2].ItemInstance.CanStackWith(this.draggedSlot.assignedSlot.ItemInstance, false))
								{
									int num3 = Mathf.Min(quickMoveSlots[num2].GetCapacityForItem(this.draggedSlot.assignedSlot.ItemInstance), this.draggedAmount - num);
									quickMoveSlots[num2].AddItem(this.draggedSlot.assignedSlot.ItemInstance.GetCopy(num3), false);
									num += num3;
								}
								num2++;
							}
							int num4 = 0;
							while (num4 < quickMoveSlots.Count && num < this.draggedAmount)
							{
								int num5 = Mathf.Min(quickMoveSlots[num4].GetCapacityForItem(this.draggedSlot.assignedSlot.ItemInstance), this.draggedAmount - num);
								quickMoveSlots[num4].AddItem(this.draggedSlot.assignedSlot.ItemInstance.GetCopy(num5), false);
								num += num5;
								num4++;
							}
							this.draggedSlot.assignedSlot.ChangeQuantity(-num, false);
						}
						this.draggedSlot = null;
						if (this.onItemMoved != null)
						{
							this.onItemMoved.Invoke();
						}
						return;
					}
					if (this.onDragStart != null)
					{
						this.onDragStart.Invoke();
					}
					this.ItemQuantityPrompt.gameObject.SetActive(this.customDragAmount);
					this.tempIcon = this.draggedSlot.DuplicateIcon(Singleton<HUD>.Instance.transform, this.draggedAmount);
					this.draggedSlot.IsBeingDragged = true;
					if (this.draggedAmount == this.draggedSlot.assignedSlot.Quantity)
					{
						this.draggedSlot.SetVisible(false);
						return;
					}
					this.draggedSlot.OverrideDisplayedQuantity(this.draggedSlot.assignedSlot.Quantity - this.draggedAmount);
				}
			}
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x00143E4C File Offset: 0x0014204C
		private void StartDragCash()
		{
			CashInstance cashInstance = this.draggedSlot.assignedSlot.ItemInstance as CashInstance;
			this.draggedCashAmount = Mathf.Min(cashInstance.Balance, 1000f);
			this.draggedAmount = 1;
			if (GameInput.GetButtonDown(GameInput.ButtonCode.SecondaryClick))
			{
				this.draggedAmount = 1;
				this.draggedCashAmount = Mathf.Min(cashInstance.Balance, 100f);
				this.mouseOffset += new Vector2(-10f, -15f);
				this.customDragAmount = true;
			}
			if (this.draggedCashAmount <= 0f)
			{
				this.draggedSlot = null;
				return;
			}
			if (GameInput.GetButton(GameInput.ButtonCode.QuickMove) && this.QuickMoveEnabled)
			{
				List<ItemSlot> quickMoveSlots = this.GetQuickMoveSlots(this.draggedSlot.assignedSlot);
				if (quickMoveSlots.Count > 0)
				{
					Debug.Log("Quick-moving " + this.draggedAmount.ToString() + " items...");
					float a = this.draggedCashAmount;
					float num = 0f;
					int num2 = 0;
					while (num2 < quickMoveSlots.Count && num < (float)this.draggedAmount)
					{
						ItemSlot itemSlot = quickMoveSlots[num2];
						if (itemSlot.ItemInstance != null)
						{
							CashInstance cashInstance2 = itemSlot.ItemInstance as CashInstance;
							if (cashInstance2 != null)
							{
								float num3;
								if (itemSlot is CashSlot)
								{
									num3 = Mathf.Min(a, float.MaxValue - cashInstance2.Balance);
								}
								else
								{
									num3 = Mathf.Min(a, 1000f - cashInstance2.Balance);
								}
								cashInstance2.ChangeBalance(num3);
								itemSlot.ReplicateStoredInstance();
								num += num3;
							}
						}
						else
						{
							CashInstance cashInstance3 = Registry.GetItem("cash").GetDefaultInstance(1) as CashInstance;
							cashInstance3.SetBalance(this.draggedCashAmount, false);
							itemSlot.SetStoredItem(cashInstance3, false);
							num += this.draggedCashAmount;
						}
						num2++;
					}
					if (num >= cashInstance.Balance)
					{
						this.draggedSlot.assignedSlot.ClearStoredInstance(false);
					}
					else
					{
						cashInstance.ChangeBalance(-num);
						this.draggedSlot.assignedSlot.ReplicateStoredInstance();
					}
				}
				if (this.onItemMoved != null)
				{
					this.onItemMoved.Invoke();
				}
				this.draggedSlot = null;
				return;
			}
			if (this.onDragStart != null)
			{
				this.onDragStart.Invoke();
			}
			if (this.draggedSlot.assignedSlot != PlayerSingleton<PlayerInventory>.Instance.cashSlot)
			{
				this.CashSlotHintAnim.Play();
			}
			this.tempIcon = this.draggedSlot.DuplicateIcon(Singleton<HUD>.Instance.transform, this.draggedAmount);
			this.tempIcon.Find("Balance").GetComponent<TextMeshProUGUI>().text = MoneyManager.FormatAmount(this.draggedCashAmount, false, false);
			this.draggedSlot.IsBeingDragged = true;
			if (this.draggedCashAmount >= cashInstance.Balance)
			{
				this.draggedSlot.SetVisible(false);
				return;
			}
			(this.draggedSlot.ItemUI as ItemUI_Cash).SetDisplayedBalance(cashInstance.Balance - this.draggedCashAmount);
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x00144140 File Offset: 0x00142340
		private void EndDrag()
		{
			if (this.isDraggingCash)
			{
				this.EndCashDrag();
				return;
			}
			if (this.CanDragFromSlot(this.draggedSlot) && this.HoveredSlot != null && this.HoveredSlot != this.draggedSlot && this.HoveredSlot.assignedSlot != null && !this.HoveredSlot.assignedSlot.IsLocked && !this.HoveredSlot.assignedSlot.IsAddLocked && this.HoveredSlot.assignedSlot.DoesItemMatchFilters(this.draggedSlot.assignedSlot.ItemInstance))
			{
				if (this.HoveredSlot.assignedSlot.ItemInstance == null)
				{
					this.HoveredSlot.assignedSlot.SetStoredItem(this.draggedSlot.assignedSlot.ItemInstance.GetCopy(this.draggedAmount), false);
					this.draggedSlot.assignedSlot.ChangeQuantity(-this.draggedAmount, false);
				}
				else if (this.HoveredSlot.assignedSlot.ItemInstance.CanStackWith(this.draggedSlot.assignedSlot.ItemInstance, false))
				{
					while (this.HoveredSlot.assignedSlot.Quantity < this.HoveredSlot.assignedSlot.ItemInstance.StackLimit)
					{
						if (this.draggedAmount <= 0)
						{
							break;
						}
						this.HoveredSlot.assignedSlot.ChangeQuantity(1, false);
						this.draggedSlot.assignedSlot.ChangeQuantity(-1, false);
						this.draggedAmount--;
					}
				}
				else if (this.draggedAmount == this.draggedSlot.assignedSlot.Quantity)
				{
					ItemInstance itemInstance = this.draggedSlot.assignedSlot.ItemInstance;
					ItemInstance itemInstance2 = this.HoveredSlot.assignedSlot.ItemInstance;
					this.draggedSlot.assignedSlot.SetStoredItem(itemInstance2, false);
					this.HoveredSlot.assignedSlot.SetStoredItem(itemInstance, false);
				}
				else if (this.HoveredSlot.assignedSlot.ItemInstance == null)
				{
					this.HoveredSlot.assignedSlot.SetStoredItem(this.draggedSlot.assignedSlot.ItemInstance, false);
					this.draggedSlot.assignedSlot.ClearStoredInstance(false);
				}
				if (this.onItemMoved != null)
				{
					this.onItemMoved.Invoke();
				}
			}
			if (this.draggedSlot != null)
			{
				this.draggedSlot.SetVisible(true);
				this.draggedSlot.UpdateUI();
				this.draggedSlot.IsBeingDragged = false;
				this.draggedSlot = null;
			}
			if (this.tempIcon != null)
			{
				UnityEngine.Object.Destroy(this.tempIcon.gameObject);
				this.tempIcon = null;
			}
			this.ItemQuantityPrompt.gameObject.SetActive(false);
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x00144414 File Offset: 0x00142614
		private void SetDraggedAmount(int amount)
		{
			ItemUIManager.<>c__DisplayClass49_0 CS$<>8__locals1 = new ItemUIManager.<>c__DisplayClass49_0();
			CS$<>8__locals1.<>4__this = this;
			this.draggedAmount = amount;
			CS$<>8__locals1.quantityText = this.tempIcon.Find("Quantity").GetComponent<TextMeshProUGUI>();
			if (CS$<>8__locals1.quantityText != null && CS$<>8__locals1.quantityText.gameObject.name == "Quantity")
			{
				CS$<>8__locals1.quantityText.text = this.draggedAmount.ToString() + "x";
				CS$<>8__locals1.quantityText.enabled = (this.draggedAmount > 1);
			}
			if (this.draggedAmount == this.draggedSlot.assignedSlot.Quantity)
			{
				this.draggedSlot.SetVisible(false);
			}
			else
			{
				this.draggedSlot.OverrideDisplayedQuantity(this.draggedSlot.assignedSlot.Quantity - this.draggedAmount);
				this.draggedSlot.SetVisible(true);
			}
			if (CS$<>8__locals1.quantityText != null)
			{
				if (this.quantityChangePopRoutine != null)
				{
					base.StopCoroutine(this.quantityChangePopRoutine);
				}
				this.quantityChangePopRoutine = base.StartCoroutine(CS$<>8__locals1.<SetDraggedAmount>g__LerpQuantityTextSize|0());
			}
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x00144538 File Offset: 0x00142738
		private void EndCashDrag()
		{
			CashInstance cashInstance = null;
			if (this.draggedSlot != null && this.draggedSlot.assignedSlot != null)
			{
				cashInstance = (this.draggedSlot.assignedSlot.ItemInstance as CashInstance);
			}
			this.CashSlotHintAnim.Stop();
			this.CashSlotHintAnimCanvasGroup.alpha = 0f;
			if (this.CanDragFromSlot(this.draggedSlot) && this.HoveredSlot != null && this.CanCashBeDraggedIntoSlot(this.HoveredSlot) && !this.HoveredSlot.assignedSlot.IsLocked && !this.HoveredSlot.assignedSlot.IsAddLocked)
			{
				if (this.HoveredSlot.assignedSlot is HotbarSlot && !(this.HoveredSlot.assignedSlot is CashSlot))
				{
					this.HoveredSlot = Singleton<HUD>.Instance.cashSlotUI.GetComponent<CashSlotUI>();
				}
				float num = Mathf.Min(this.draggedCashAmount, cashInstance.Balance);
				if (num > 0f)
				{
					float num2 = num;
					if (this.HoveredSlot.assignedSlot.ItemInstance != null)
					{
						CashInstance cashInstance2 = this.HoveredSlot.assignedSlot.ItemInstance as CashInstance;
						if (this.HoveredSlot.assignedSlot is CashSlot)
						{
							num2 = Mathf.Min(num, float.MaxValue - cashInstance2.Balance);
						}
						else
						{
							num2 = Mathf.Min(num, 1000f - cashInstance2.Balance);
						}
						cashInstance2.ChangeBalance(num2);
						this.HoveredSlot.assignedSlot.ReplicateStoredInstance();
					}
					else
					{
						CashInstance cashInstance3 = Registry.GetItem("cash").GetDefaultInstance(1) as CashInstance;
						cashInstance3.SetBalance(num2, false);
						this.HoveredSlot.assignedSlot.SetStoredItem(cashInstance3, false);
					}
					if (num2 >= cashInstance.Balance)
					{
						this.draggedSlot.assignedSlot.ClearStoredInstance(false);
					}
					else
					{
						cashInstance.ChangeBalance(-num2);
						this.draggedSlot.assignedSlot.ReplicateStoredInstance();
					}
				}
			}
			this.draggedSlot.SetVisible(true);
			this.draggedSlot.UpdateUI();
			this.draggedSlot.IsBeingDragged = false;
			this.draggedSlot = null;
			UnityEngine.Object.Destroy(this.tempIcon.gameObject);
			Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x00144770 File Offset: 0x00142970
		public bool CanDragFromSlot(ItemSlotUI slotUI)
		{
			return !(slotUI == null) && slotUI.assignedSlot != null && slotUI.assignedSlot.ItemInstance != null && !slotUI.assignedSlot.IsLocked && !slotUI.assignedSlot.IsRemovalLocked;
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x001447BE File Offset: 0x001429BE
		public bool CanCashBeDraggedIntoSlot(ItemSlotUI ui)
		{
			return !(ui == null) && ui.assignedSlot != null && (ui.assignedSlot.ItemInstance == null || ui.assignedSlot.ItemInstance is CashInstance);
		}

		// Token: 0x04003A18 RID: 14872
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003A19 RID: 14873
		public GraphicRaycaster[] Raycasters;

		// Token: 0x04003A1A RID: 14874
		public RectTransform CashDragAmountContainer;

		// Token: 0x04003A1B RID: 14875
		public RectTransform InputsContainer;

		// Token: 0x04003A1C RID: 14876
		public ItemInfoPanel InfoPanel;

		// Token: 0x04003A1D RID: 14877
		public RectTransform ItemQuantityPrompt;

		// Token: 0x04003A1E RID: 14878
		public Animation CashSlotHintAnim;

		// Token: 0x04003A1F RID: 14879
		public CanvasGroup CashSlotHintAnimCanvasGroup;

		// Token: 0x04003A20 RID: 14880
		[Header("Prefabs")]
		public ItemSlotUI ItemSlotUIPrefab;

		// Token: 0x04003A21 RID: 14881
		public ItemUI DefaultItemUIPrefab;

		// Token: 0x04003A22 RID: 14882
		public ItemSlotUI HotbarSlotUIPrefab;

		// Token: 0x04003A23 RID: 14883
		private ItemSlotUI draggedSlot;

		// Token: 0x04003A24 RID: 14884
		private Vector2 mouseOffset = Vector2.zero;

		// Token: 0x04003A25 RID: 14885
		private int draggedAmount;

		// Token: 0x04003A26 RID: 14886
		private RectTransform tempIcon;

		// Token: 0x04003A27 RID: 14887
		private bool isDraggingCash;

		// Token: 0x04003A28 RID: 14888
		private float draggedCashAmount;

		// Token: 0x04003A29 RID: 14889
		private List<ItemSlot> PrimarySlots = new List<ItemSlot>();

		// Token: 0x04003A2A RID: 14890
		private List<ItemSlot> SecondarySlots = new List<ItemSlot>();

		// Token: 0x04003A2B RID: 14891
		private bool customDragAmount;

		// Token: 0x04003A2C RID: 14892
		private Coroutine quantityChangePopRoutine;

		// Token: 0x04003A2D RID: 14893
		public UnityEvent onDragStart;

		// Token: 0x04003A2E RID: 14894
		public UnityEvent onItemMoved;
	}
}
