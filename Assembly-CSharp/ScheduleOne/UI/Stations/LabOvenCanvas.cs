using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.StationFramework;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000A4B RID: 2635
	public class LabOvenCanvas : Singleton<LabOvenCanvas>
	{
		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06004700 RID: 18176 RVA: 0x001298BB File Offset: 0x00127ABB
		// (set) Token: 0x06004701 RID: 18177 RVA: 0x001298C3 File Offset: 0x00127AC3
		public bool isOpen { get; protected set; }

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06004702 RID: 18178 RVA: 0x001298CC File Offset: 0x00127ACC
		// (set) Token: 0x06004703 RID: 18179 RVA: 0x001298D4 File Offset: 0x00127AD4
		public LabOven Oven { get; protected set; }

		// Token: 0x06004704 RID: 18180 RVA: 0x001298DD File Offset: 0x00127ADD
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x00129901 File Offset: 0x00127B01
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false, true);
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x00129914 File Offset: 0x00127B14
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				this.BeginButton.interactable = this.CanBegin();
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
					return;
				}
				if (this.Oven.CurrentOperation != null)
				{
					this.ProgressImg.fillAmount = Mathf.Clamp01((float)this.Oven.CurrentOperation.CookProgress / (float)this.Oven.CurrentOperation.GetCookDuration());
					this.BeginButtonLabel.text = "COLLECT";
					if (this.Oven.CurrentOperation.CookProgress < this.Oven.CurrentOperation.GetCookDuration())
					{
						this.InstructionLabel.text = "Cooking in progress...";
						this.InstructionLabel.enabled = true;
						this.ErrorLabel.enabled = false;
						return;
					}
					if (this.DoesOvenOutputHaveSpace())
					{
						this.InstructionLabel.text = "Ready to collect product";
						this.InstructionLabel.enabled = true;
						this.ErrorLabel.enabled = false;
						return;
					}
					this.ErrorLabel.text = "Not enough space in output slot";
					this.ErrorLabel.enabled = true;
					this.InstructionLabel.enabled = false;
					return;
				}
				else
				{
					this.ProgressContainer.gameObject.SetActive(false);
					this.BeginButtonLabel.text = "BEGIN";
					if (this.Oven.IngredientSlot.ItemInstance != null)
					{
						if (this.Oven.IsIngredientCookable())
						{
							this.InstructionLabel.text = "Ready to begin cooking";
							this.InstructionLabel.enabled = true;
							return;
						}
						this.InstructionLabel.enabled = false;
						this.ErrorLabel.enabled = true;
						this.ErrorLabel.text = "Ingredient is not cookable";
						return;
					}
					else
					{
						this.InstructionLabel.text = "Place cookable item in ingredient slot";
						this.InstructionLabel.enabled = true;
						this.ErrorLabel.enabled = false;
					}
				}
			}
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x00129B00 File Offset: 0x00127D00
		public void SetIsOpen(LabOven oven, bool open, bool removeUI = true)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			this.Oven = oven;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				if (open)
				{
					PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				}
			}
			if (oven != null)
			{
				this.IngredientSlotUI.AssignSlot(oven.IngredientSlot);
				this.OutputSlotUI.AssignSlot(oven.OutputSlot);
			}
			else
			{
				this.IngredientSlotUI.ClearSlot();
				this.OutputSlotUI.ClearSlot();
			}
			if (open)
			{
				this.RefreshActiveOperation();
				this.Update();
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			}
			else if (removeUI)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(open, true);
			if (open)
			{
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
				{
					this.Oven.IngredientSlot,
					this.Oven.OutputSlot
				});
			}
			if (this.isOpen)
			{
				Singleton<CompassManager>.Instance.SetVisible(false);
			}
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x00129C48 File Offset: 0x00127E48
		public void BeginButtonPressed()
		{
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			if (this.Oven.CurrentOperation != null)
			{
				new FinalizeLabOven(this.Oven);
			}
			else if ((this.Oven.IngredientSlot.ItemInstance.Definition as StorableItemDefinition).StationItem.GetModule<CookableModule>().CookType == CookableModule.ECookableType.Liquid)
			{
				new StartLabOvenTask(this.Oven);
			}
			else
			{
				new LabOvenSolidTask(this.Oven);
			}
			this.SetIsOpen(null, false, false);
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x00129CCC File Offset: 0x00127ECC
		public bool CanBegin()
		{
			if (this.Oven == null)
			{
				return false;
			}
			if (this.Oven.CurrentOperation != null)
			{
				return this.Oven.CurrentOperation.CookProgress >= this.Oven.CurrentOperation.GetCookDuration() && this.DoesOvenOutputHaveSpace();
			}
			return this.Oven.IsIngredientCookable();
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x00129D34 File Offset: 0x00127F34
		private bool DoesOvenOutputHaveSpace()
		{
			return this.Oven.OutputSlot.GetCapacityForItem(this.Oven.CurrentOperation.Product.GetDefaultInstance(1)) >= this.Oven.CurrentOperation.Cookable.ProductQuantity;
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x00129D84 File Offset: 0x00127F84
		private void RefreshActiveOperation()
		{
			if (this.Oven.CurrentOperation != null)
			{
				this.IngredientIcon.sprite = this.Oven.CurrentOperation.Ingredient.Icon;
				this.ProductIcon.sprite = this.Oven.CurrentOperation.Product.Icon;
			}
		}

		// Token: 0x040034B7 RID: 13495
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040034B8 RID: 13496
		public GameObject Container;

		// Token: 0x040034B9 RID: 13497
		public ItemSlotUI IngredientSlotUI;

		// Token: 0x040034BA RID: 13498
		public ItemSlotUI OutputSlotUI;

		// Token: 0x040034BB RID: 13499
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x040034BC RID: 13500
		public TextMeshProUGUI ErrorLabel;

		// Token: 0x040034BD RID: 13501
		public Button BeginButton;

		// Token: 0x040034BE RID: 13502
		public TextMeshProUGUI BeginButtonLabel;

		// Token: 0x040034BF RID: 13503
		public RectTransform ProgressContainer;

		// Token: 0x040034C0 RID: 13504
		public Image IngredientIcon;

		// Token: 0x040034C1 RID: 13505
		public Image ProgressImg;

		// Token: 0x040034C2 RID: 13506
		public Image ProductIcon;
	}
}
