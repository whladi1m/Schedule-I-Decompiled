using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.UI.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A28 RID: 2600
	public class StorageMenu : Singleton<StorageMenu>
	{
		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600462C RID: 17964 RVA: 0x00125ADD File Offset: 0x00123CDD
		// (set) Token: 0x0600462D RID: 17965 RVA: 0x00125AE5 File Offset: 0x00123CE5
		public bool IsOpen { get; protected set; }

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600462E RID: 17966 RVA: 0x00125AEE File Offset: 0x00123CEE
		// (set) Token: 0x0600462F RID: 17967 RVA: 0x00125AF6 File Offset: 0x00123CF6
		public StorageEntity OpenedStorageEntity { get; protected set; }

		// Token: 0x06004630 RID: 17968 RVA: 0x00125AFF File Offset: 0x00123CFF
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x00125B36 File Offset: 0x00123D36
		public virtual void Open(IItemSlotOwner owner, string title, string subtitle)
		{
			this.IsOpen = true;
			this.OpenedStorageEntity = null;
			this.SlotGridLayout.constraintCount = 1;
			this.Open(title, subtitle, owner);
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x00125B5B File Offset: 0x00123D5B
		public virtual void Open(StorageEntity entity)
		{
			this.IsOpen = true;
			this.OpenedStorageEntity = entity;
			this.SlotGridLayout.constraintCount = entity.DisplayRowCount;
			this.Open(entity.StorageEntityName, entity.StorageEntitySubtitle, entity);
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x00125B90 File Offset: 0x00123D90
		private void Open(string title, string subtitle, IItemSlotOwner owner)
		{
			this.IsOpen = true;
			this.TitleLabel.text = title;
			this.SubtitleLabel.text = subtitle;
			for (int i = 0; i < this.SlotsUIs.Length; i++)
			{
				if (owner.ItemSlots.Count > i)
				{
					this.SlotsUIs[i].gameObject.SetActive(true);
					this.SlotsUIs[i].AssignSlot(owner.ItemSlots[i]);
				}
				else
				{
					this.SlotsUIs[i].ClearSlot();
					this.SlotsUIs[i].gameObject.SetActive(false);
				}
			}
			int constraintCount = this.SlotGridLayout.constraintCount;
			this.CloseButton.anchoredPosition = new Vector2(0f, (float)constraintCount * -this.SlotGridLayout.cellSize.y - 60f);
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.06f);
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, true);
			Singleton<ItemUIManager>.Instance.EnableQuickMove(PlayerSingleton<PlayerInventory>.Instance.GetAllInventorySlots(), owner.ItemSlots.ToList<ItemSlot>());
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x00125D13 File Offset: 0x00123F13
		public void Close()
		{
			if (this.OpenedStorageEntity != null)
			{
				this.OpenedStorageEntity.Close();
				return;
			}
			this.CloseMenu();
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x00125D38 File Offset: 0x00123F38
		public virtual void CloseMenu()
		{
			this.IsOpen = false;
			this.OpenedStorageEntity = null;
			for (int i = 0; i < this.SlotsUIs.Length; i++)
			{
				this.SlotsUIs[i].ClearSlot();
				this.SlotsUIs[i].gameObject.SetActive(false);
			}
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.06f);
			PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
			Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			if (this.onClosed != null)
			{
				this.onClosed.Invoke();
			}
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x00125E18 File Offset: 0x00124018
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				if (this.OpenedStorageEntity != null)
				{
					this.OpenedStorageEntity.Close();
					return;
				}
				this.CloseMenu();
			}
		}

		// Token: 0x040033E2 RID: 13282
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040033E3 RID: 13283
		public RectTransform Container;

		// Token: 0x040033E4 RID: 13284
		public TextMeshProUGUI TitleLabel;

		// Token: 0x040033E5 RID: 13285
		public TextMeshProUGUI SubtitleLabel;

		// Token: 0x040033E6 RID: 13286
		public RectTransform SlotContainer;

		// Token: 0x040033E7 RID: 13287
		public ItemSlotUI[] SlotsUIs;

		// Token: 0x040033E8 RID: 13288
		public GridLayoutGroup SlotGridLayout;

		// Token: 0x040033E9 RID: 13289
		public RectTransform CloseButton;

		// Token: 0x040033EA RID: 13290
		public UnityEvent onClosed;
	}
}
