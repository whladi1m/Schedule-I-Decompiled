using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Items;
using ScheduleOne.UI.Stations.Drying_rack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Stations
{
	// Token: 0x02000A49 RID: 2633
	public class DryingRackCanvas : Singleton<DryingRackCanvas>
	{
		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060046EE RID: 18158 RVA: 0x00129335 File Offset: 0x00127535
		// (set) Token: 0x060046EF RID: 18159 RVA: 0x0012933D File Offset: 0x0012753D
		public bool isOpen { get; protected set; }

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060046F0 RID: 18160 RVA: 0x00129346 File Offset: 0x00127546
		// (set) Token: 0x060046F1 RID: 18161 RVA: 0x0012934E File Offset: 0x0012754E
		public DryingRack Rack { get; protected set; }

		// Token: 0x060046F2 RID: 18162 RVA: 0x00129357 File Offset: 0x00127557
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x0012935F File Offset: 0x0012755F
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(null, false);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x00129395 File Offset: 0x00127595
		private void MinPass()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.UpdateDryingOperations();
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x001293A6 File Offset: 0x001275A6
		protected virtual void Update()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.UpdateUI();
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x001293B8 File Offset: 0x001275B8
		private void UpdateUI()
		{
			this.InsertButton.interactable = this.Rack.CanStartOperation();
			this.CapacityLabel.text = this.Rack.GetTotalDryingItems().ToString() + " / " + this.Rack.ItemCapacity.ToString();
			this.CapacityLabel.color = ((this.Rack.GetTotalDryingItems() >= this.Rack.ItemCapacity) ? new Color32(byte.MaxValue, 50, 50, byte.MaxValue) : Color.white);
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x00129458 File Offset: 0x00127658
		private void UpdateDryingOperations()
		{
			foreach (DryingOperationUI dryingOperationUI in this.operationUIs)
			{
				RectTransform alignment = null;
				DryingOperation assignedOperation = dryingOperationUI.AssignedOperation;
				if (assignedOperation.StartQuality == EQuality.Trash)
				{
					alignment = this.IndicatorAlignments[0];
				}
				else if (assignedOperation.StartQuality == EQuality.Poor)
				{
					alignment = this.IndicatorAlignments[1];
				}
				else if (assignedOperation.StartQuality == EQuality.Standard)
				{
					alignment = this.IndicatorAlignments[2];
				}
				else if (assignedOperation.StartQuality == EQuality.Premium)
				{
					alignment = this.IndicatorAlignments[3];
				}
				else
				{
					Console.LogWarning("Alignment not found for quality: " + assignedOperation.StartQuality.ToString(), null);
				}
				dryingOperationUI.SetAlignment(alignment);
			}
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x00129528 File Offset: 0x00127728
		private void UpdateQuantities()
		{
			foreach (DryingOperationUI dryingOperationUI in this.operationUIs)
			{
				dryingOperationUI.RefreshQuantity();
			}
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x00129578 File Offset: 0x00127778
		public void SetIsOpen(DryingRack rack, bool open)
		{
			this.isOpen = open;
			this.Canvas.enabled = open;
			this.Container.gameObject.SetActive(open);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			}
			if (open)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				this.InputSlotUI.AssignSlot(rack.InputSlot);
				this.OutputSlotUI.AssignSlot(rack.OutputSlot);
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				for (int i = 0; i < rack.DryingOperations.Count; i++)
				{
					this.CreateOperationUI(rack.DryingOperations[i]);
				}
				rack.onOperationStart = (Action<DryingOperation>)Delegate.Combine(rack.onOperationStart, new Action<DryingOperation>(this.CreateOperationUI));
				rack.onOperationComplete = (Action<DryingOperation>)Delegate.Combine(rack.onOperationComplete, new Action<DryingOperation>(this.DestroyOperationUI));
				rack.onOperationsChanged = (Action)Delegate.Combine(rack.onOperationsChanged, new Action(this.UpdateQuantities));
			}
			else
			{
				this.InputSlotUI.ClearSlot();
				this.OutputSlotUI.ClearSlot();
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
				if (this.Rack != null)
				{
					DryingRack rack2 = this.Rack;
					rack2.onOperationStart = (Action<DryingOperation>)Delegate.Remove(rack2.onOperationStart, new Action<DryingOperation>(this.CreateOperationUI));
					DryingRack rack3 = this.Rack;
					rack3.onOperationComplete = (Action<DryingOperation>)Delegate.Remove(rack3.onOperationComplete, new Action<DryingOperation>(this.DestroyOperationUI));
					DryingRack rack4 = this.Rack;
					rack4.onOperationsChanged = (Action)Delegate.Remove(rack4.onOperationsChanged, new Action(this.UpdateQuantities));
				}
				foreach (DryingOperationUI dryingOperationUI in this.operationUIs)
				{
					UnityEngine.Object.Destroy(dryingOperationUI.gameObject);
				}
				this.operationUIs.Clear();
			}
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(open, true);
			if (open)
			{
				List<ItemSlot> list = new List<ItemSlot>();
				list.AddRange(rack.InputSlots);
				list.Add(rack.OutputSlot);
				Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), list);
			}
			this.Rack = rack;
			if (open)
			{
				this.UpdateUI();
				this.MinPass();
			}
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x00129800 File Offset: 0x00127A00
		private void CreateOperationUI(DryingOperation operation)
		{
			DryingOperationUI dryingOperationUI = UnityEngine.Object.Instantiate<DryingOperationUI>(this.IndicatorPrefab, this.IndicatorContainer);
			dryingOperationUI.SetOperation(operation);
			this.operationUIs.Add(dryingOperationUI);
			this.UpdateDryingOperations();
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x00129838 File Offset: 0x00127A38
		private void DestroyOperationUI(DryingOperation operation)
		{
			DryingOperationUI dryingOperationUI = this.operationUIs.FirstOrDefault((DryingOperationUI x) => x.AssignedOperation == operation);
			if (dryingOperationUI != null)
			{
				this.operationUIs.Remove(dryingOperationUI);
				UnityEngine.Object.Destroy(dryingOperationUI.gameObject);
			}
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x0012988B File Offset: 0x00127A8B
		public void Insert()
		{
			this.Rack.StartOperation();
		}

		// Token: 0x040034A9 RID: 13481
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040034AA RID: 13482
		public RectTransform Container;

		// Token: 0x040034AB RID: 13483
		public ItemSlotUI InputSlotUI;

		// Token: 0x040034AC RID: 13484
		public ItemSlotUI OutputSlotUI;

		// Token: 0x040034AD RID: 13485
		public TextMeshProUGUI InstructionLabel;

		// Token: 0x040034AE RID: 13486
		public TextMeshProUGUI CapacityLabel;

		// Token: 0x040034AF RID: 13487
		public Button InsertButton;

		// Token: 0x040034B0 RID: 13488
		public RectTransform IndicatorContainer;

		// Token: 0x040034B1 RID: 13489
		public RectTransform[] IndicatorAlignments;

		// Token: 0x040034B2 RID: 13490
		[Header("Prefabs")]
		public DryingOperationUI IndicatorPrefab;

		// Token: 0x040034B3 RID: 13491
		private List<DryingOperationUI> operationUIs = new List<DryingOperationUI>();
	}
}
