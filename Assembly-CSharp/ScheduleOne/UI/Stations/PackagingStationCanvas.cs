using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using ScheduleOne.Product.Packaging;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000A4D RID: 2637
	public class PackagingStationCanvas : Singleton<PackagingStationCanvas>
	{
		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06004727 RID: 18215 RVA: 0x0012AC77 File Offset: 0x00128E77
		// (set) Token: 0x06004728 RID: 18216 RVA: 0x0012AC7F File Offset: 0x00128E7F
		public bool isOpen { get; protected set; }

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06004729 RID: 18217 RVA: 0x0012AC88 File Offset: 0x00128E88
		// (set) Token: 0x0600472A RID: 18218 RVA: 0x0012AC90 File Offset: 0x00128E90
		public PackagingStation PackagingStation { get; protected set; }

		// Token: 0x0600472B RID: 18219 RVA: 0x0012AC99 File Offset: 0x00128E99
		protected override void Awake()
		{
			base.Awake();
			this.BeginButton.onClick.AddListener(new UnityAction(this.BeginButtonPressed));
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x0012ACBD File Offset: 0x00128EBD
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false, true);
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x0012ACD0 File Offset: 0x00128ED0
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				if (this.CurrentMode == PackagingStation.EMode.Package)
				{
					this.ButtonLabel.text = "PACK";
				}
				else
				{
					this.ButtonLabel.text = "UNPACK";
				}
				if (this.BeginButton.interactable && GameInput.GetButtonDown(GameInput.ButtonCode.Submit))
				{
					this.BeginButtonPressed();
					return;
				}
				PackagingStation.EState state = this.PackagingStation.GetState(this.CurrentMode);
				if (state == PackagingStation.EState.CanBegin)
				{
					this.InstructionLabel.enabled = false;
					this.InstructionShadow.enabled = false;
					this.BeginButton.interactable = true;
					return;
				}
				if (this.CurrentMode == PackagingStation.EMode.Package)
				{
					if (state == PackagingStation.EState.MissingItems)
					{
						this.InstructionLabel.text = "Drag product + packaging into slots";
						this.InstructionLabel.color = Color.white;
					}
					else if (state == PackagingStation.EState.InsufficentProduct)
					{
						this.InstructionLabel.text = "This packaging type requires <color=#FFC73D>" + (this.PackagingStation.PackagingSlot.ItemInstance.Definition as PackagingDefinition).Quantity.ToString() + "x</color> product";
						this.InstructionLabel.color = Color.white;
					}
					else if (state == PackagingStation.EState.OutputSlotFull)
					{
						this.InstructionLabel.text = "Output slot is full!";
						this.InstructionLabel.color = this.InstructionWarningColor;
					}
					else if (state == PackagingStation.EState.Mismatch)
					{
						this.InstructionLabel.text = "Output slot is full!";
						this.InstructionLabel.color = this.InstructionWarningColor;
					}
				}
				else if (state == PackagingStation.EState.MissingItems)
				{
					this.InstructionLabel.text = "Drag packaged product into output";
					this.InstructionLabel.color = Color.white;
				}
				else if (state == PackagingStation.EState.PackageSlotFull)
				{
					this.InstructionLabel.text = "Unpackaged items won't fit!";
					this.InstructionLabel.color = this.InstructionWarningColor;
				}
				else if (state == PackagingStation.EState.ProductSlotFull)
				{
					this.InstructionLabel.text = "Unpackaged items won't fit!";
					this.InstructionLabel.color = this.InstructionWarningColor;
				}
				this.InstructionLabel.enabled = true;
				this.InstructionShadow.enabled = true;
				this.BeginButton.interactable = false;
			}
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x0012AEE0 File Offset: 0x001290E0
		public void SetIsOpen(PackagingStation station, bool open, bool removeUI = true)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			this.PackagingStation = station;
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
				if (open)
				{
					PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				}
			}
			if (station != null)
			{
				this.PackagingSlotUI.AssignSlot(station.PackagingSlot);
				this.ProductSlotUI.AssignSlot(station.ProductSlot);
				this.OutputSlotUI.AssignSlot(station.OutputSlot);
			}
			else
			{
				this.PackagingSlotUI.ClearSlot();
				this.ProductSlotUI.ClearSlot();
				this.OutputSlotUI.ClearSlot();
			}
			if (open)
			{
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				if (this.ShowShiftClickHint && station.OutputSlot.Quantity > 0)
				{
					Singleton<HintDisplay>.Instance.ShowHint_20s("<Input_QuickMove><h1> + click</h> an item to quickly move it");
				}
			}
			else if (removeUI)
			{
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(open, true);
			if (open)
			{
				if (this.CurrentMode == PackagingStation.EMode.Package)
				{
					Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
					{
						station.ProductSlot,
						station.PackagingSlot,
						station.OutputSlot
					});
				}
				else
				{
					Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
					{
						station.OutputSlot,
						station.PackagingSlot,
						station.ProductSlot
					});
				}
			}
			if (this.isOpen)
			{
				Singleton<CompassManager>.Instance.SetVisible(false);
			}
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x0012B0A8 File Offset: 0x001292A8
		public void BeginButtonPressed()
		{
			if (this.PackagingStation == null)
			{
				return;
			}
			if (this.PackagingStation.GetState(this.CurrentMode) != PackagingStation.EState.CanBegin)
			{
				return;
			}
			if (this.CurrentMode == PackagingStation.EMode.Unpackage)
			{
				this.PackagingStation.Unpack();
				Singleton<TaskManager>.Instance.PlayTaskCompleteSound();
				return;
			}
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			this.PackagingStation.StartTask();
			if (this.ShowHintOnOpen)
			{
				Singleton<HintDisplay>.Instance.ShowHint_20s("When performing tasks at stations, click and drag items to move them.");
			}
			this.SetIsOpen(null, false, false);
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0012B130 File Offset: 0x00129330
		private void UpdateSlotPositions()
		{
			if (this.PackagingStation != null)
			{
				this.PackagingSlotUI.Rect.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(this.PackagingStation.PackagingSlotPosition.position);
				this.ProductSlotUI.Rect.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(this.PackagingStation.ProductSlotPosition.position);
				this.OutputSlotUI.Rect.position = PlayerSingleton<PlayerCamera>.Instance.Camera.WorldToScreenPoint(this.PackagingStation.OutputSlotPosition.position);
			}
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0012B1DB File Offset: 0x001293DB
		public void ToggleMode()
		{
			this.SetMode((this.CurrentMode == PackagingStation.EMode.Package) ? PackagingStation.EMode.Unpackage : PackagingStation.EMode.Package);
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0012B1F0 File Offset: 0x001293F0
		public void SetMode(PackagingStation.EMode mode)
		{
			this.CurrentMode = mode;
			if (mode == PackagingStation.EMode.Package)
			{
				this.ModeAnimation.Play("Packaging station switch to package");
			}
			else
			{
				this.ModeAnimation.Play("Packaging station switch to unpackage");
			}
			if (this.CurrentMode == PackagingStation.EMode.Package)
			{
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
				{
					this.PackagingStation.ProductSlot,
					this.PackagingStation.PackagingSlot,
					this.PackagingStation.OutputSlot
				});
				return;
			}
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), new List<ItemSlot>
			{
				this.PackagingStation.OutputSlot,
				this.PackagingStation.PackagingSlot,
				this.PackagingStation.ProductSlot
			});
		}

		// Token: 0x040034DB RID: 13531
		public bool ShowHintOnOpen;

		// Token: 0x040034DC RID: 13532
		public bool ShowShiftClickHint;

		// Token: 0x040034DD RID: 13533
		public PackagingStation.EMode CurrentMode;

		// Token: 0x040034DE RID: 13534
		public Color InstructionWarningColor;

		// Token: 0x040034DF RID: 13535
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040034E0 RID: 13536
		public GameObject Container;

		// Token: 0x040034E1 RID: 13537
		public ItemSlotUI PackagingSlotUI;

		// Token: 0x040034E2 RID: 13538
		public ItemSlotUI ProductSlotUI;

		// Token: 0x040034E3 RID: 13539
		public ItemSlotUI OutputSlotUI;

		// Token: 0x040034E4 RID: 13540
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x040034E5 RID: 13541
		public Image InstructionShadow;

		// Token: 0x040034E6 RID: 13542
		public Button BeginButton;

		// Token: 0x040034E7 RID: 13543
		public Animation ModeAnimation;

		// Token: 0x040034E8 RID: 13544
		public TextMeshProUGUI ButtonLabel;
	}
}
