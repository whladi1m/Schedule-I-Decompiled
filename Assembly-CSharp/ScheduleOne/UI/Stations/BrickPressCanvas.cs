using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000A41 RID: 2625
	public class BrickPressCanvas : Singleton<BrickPressCanvas>
	{
		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060046B7 RID: 18103 RVA: 0x00128012 File Offset: 0x00126212
		// (set) Token: 0x060046B8 RID: 18104 RVA: 0x0012801A File Offset: 0x0012621A
		public bool isOpen { get; protected set; }

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060046B9 RID: 18105 RVA: 0x00128023 File Offset: 0x00126223
		// (set) Token: 0x060046BA RID: 18106 RVA: 0x0012802B File Offset: 0x0012622B
		public BrickPress Press { get; protected set; }

		// Token: 0x060046BB RID: 18107 RVA: 0x00128034 File Offset: 0x00126234
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x00128058 File Offset: 0x00126258
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false, true);
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x0012806C File Offset: 0x0012626C
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
					return;
				}
				PackagingStation.EState state = this.Press.GetState();
				if (state == PackagingStation.EState.CanBegin)
				{
					this.InstructionLabel.enabled = false;
					this.BeginButton.interactable = true;
					return;
				}
				if (state == PackagingStation.EState.InsufficentProduct)
				{
					this.InstructionLabel.text = "Drag 20x product into input slots";
				}
				else if (state == PackagingStation.EState.OutputSlotFull)
				{
					this.InstructionLabel.text = "Output slot is full!";
				}
				else if (state == PackagingStation.EState.Mismatch)
				{
					this.InstructionLabel.text = "Output slot is full!";
				}
				this.InstructionLabel.enabled = true;
				this.BeginButton.interactable = false;
			}
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x00128124 File Offset: 0x00126324
		public void SetIsOpen(BrickPress press, bool open, bool removeUI = true)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			this.Press = press;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				if (open)
				{
					PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				}
			}
			if (press != null)
			{
				for (int i = 0; i < this.ProductSlotUIs.Length; i++)
				{
					this.ProductSlotUIs[i].AssignSlot(press.InputSlots[i]);
				}
				this.OutputSlotUI.AssignSlot(press.OutputSlot);
			}
			else
			{
				for (int j = 0; j < this.ProductSlotUIs.Length; j++)
				{
					this.ProductSlotUIs[j].ClearSlot();
				}
				this.OutputSlotUI.ClearSlot();
			}
			if (open)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				this.Update();
			}
			else if (removeUI)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(open, true);
			if (open)
			{
				List<ItemSlot> list = new List<ItemSlot>();
				list.AddRange(press.InputSlots);
				list.Add(press.OutputSlot);
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			}
			if (this.isOpen)
			{
				Singleton<CompassManager>.Instance.SetVisible(false);
			}
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x0012828C File Offset: 0x0012648C
		public void BeginButtonPressed()
		{
			if (this.Press.GetState() != PackagingStation.EState.CanBegin)
			{
				return;
			}
			ProductItemInstance product;
			if (!this.Press.HasSufficientProduct(out product))
			{
				return;
			}
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			new UseBrickPress(this.Press, product);
			this.SetIsOpen(null, false, false);
		}

		// Token: 0x04003475 RID: 13429
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003476 RID: 13430
		public RectTransform Container;

		// Token: 0x04003477 RID: 13431
		public ItemSlotUI[] ProductSlotUIs;

		// Token: 0x04003478 RID: 13432
		public ItemSlotUI OutputSlotUI;

		// Token: 0x04003479 RID: 13433
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x0400347A RID: 13434
		public Button BeginButton;
	}
}
