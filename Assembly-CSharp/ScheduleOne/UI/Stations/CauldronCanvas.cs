using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000A42 RID: 2626
	public class CauldronCanvas : Singleton<CauldronCanvas>
	{
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x001282E0 File Offset: 0x001264E0
		// (set) Token: 0x060046C2 RID: 18114 RVA: 0x001282E8 File Offset: 0x001264E8
		public bool isOpen { get; protected set; }

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060046C3 RID: 18115 RVA: 0x001282F1 File Offset: 0x001264F1
		// (set) Token: 0x060046C4 RID: 18116 RVA: 0x001282F9 File Offset: 0x001264F9
		public Cauldron Cauldron { get; protected set; }

		// Token: 0x060046C5 RID: 18117 RVA: 0x00128302 File Offset: 0x00126502
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x00128326 File Offset: 0x00126526
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false, true);
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x00128338 File Offset: 0x00126538
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
				}
				Cauldron.EState state = this.Cauldron.GetState();
				if (state == Cauldron.EState.Ready)
				{
					this.InstructionLabel.enabled = false;
					this.BeginButton.interactable = true;
					return;
				}
				if (state == Cauldron.EState.Cooking)
				{
					this.InstructionLabel.text = "Cooking in progress...";
				}
				else if (state == Cauldron.EState.MissingIngredients)
				{
					this.InstructionLabel.text = "Insert <color=#FFC73D>" + 20.ToString() + "x</color> coca leaves and <color=#FFC73D>1x</color> gasoline";
				}
				else if (state == Cauldron.EState.OutputFull)
				{
					this.InstructionLabel.text = "Output is full";
				}
				this.InstructionLabel.enabled = true;
				this.BeginButton.interactable = false;
			}
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x00128400 File Offset: 0x00126600
		public void SetIsOpen(Cauldron cauldron, bool open, bool removeUI = true)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.SetActive(open);
			this.Cauldron = cauldron;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				if (open)
				{
					PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				}
			}
			if (cauldron != null)
			{
				for (int i = 0; i < this.IngredientSlotUIs.Count; i++)
				{
					this.IngredientSlotUIs[i].AssignSlot(cauldron.IngredientSlots[i]);
				}
				this.LiquidSlotUI.AssignSlot(this.Cauldron.LiquidSlot);
				this.OutputSlotUI.AssignSlot(this.Cauldron.OutputSlot);
			}
			else
			{
				foreach (ItemSlotUI itemSlotUI in this.IngredientSlotUIs)
				{
					itemSlotUI.ClearSlot();
				}
				this.LiquidSlotUI.ClearSlot();
				this.OutputSlotUI.ClearSlot();
			}
			if (open)
			{
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
				List<ItemSlot> list = new List<ItemSlot>();
				list.AddRange(cauldron.IngredientSlots);
				list.Add(cauldron.LiquidSlot);
				list.Add(cauldron.OutputSlot);
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			}
			if (this.isOpen)
			{
				this.Update();
				Singleton<CompassManager>.Instance.SetVisible(false);
			}
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x001285BC File Offset: 0x001267BC
		public void BeginButtonPressed()
		{
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			new CauldronTask(this.Cauldron);
			this.SetIsOpen(null, false, false);
		}

		// Token: 0x0400347D RID: 13437
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400347E RID: 13438
		public GameObject Container;

		// Token: 0x0400347F RID: 13439
		public List<ItemSlotUI> IngredientSlotUIs;

		// Token: 0x04003480 RID: 13440
		public ItemSlotUI LiquidSlotUI;

		// Token: 0x04003481 RID: 13441
		public ItemSlotUI OutputSlotUI;

		// Token: 0x04003482 RID: 13442
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x04003483 RID: 13443
		public Button BeginButton;
	}
}
