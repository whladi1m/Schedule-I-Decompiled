using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x020009D2 RID: 2514
	public class GenericUIScreen : MonoBehaviour
	{
		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060043EA RID: 17386 RVA: 0x0011CB0B File Offset: 0x0011AD0B
		// (set) Token: 0x060043EB RID: 17387 RVA: 0x0011CB13 File Offset: 0x0011AD13
		public bool IsOpen { get; private set; }

		// Token: 0x060043EC RID: 17388 RVA: 0x0011CB1C File Offset: 0x0011AD1C
		private void Awake()
		{
			if (this.UseExitActions)
			{
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), this.ExitActionPriority);
			}
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x0011CB40 File Offset: 0x0011AD40
		public void Open()
		{
			this.IsOpen = true;
			PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.Name);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
			PlayerSingleton<PlayerMovement>.Instance.canMove = false;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(false);
			if (this.onOpen != null)
			{
				this.onOpen.Invoke();
			}
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0011CBA4 File Offset: 0x0011ADA4
		public void Close()
		{
			this.IsOpen = false;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.Name);
			if (this.ReenableControlsOnClose)
			{
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			}
			if (this.ReenableInventoryOnClose)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			}
			if (!this.ReenableEquippingOnClose)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
			}
			if (this.onClose != null)
			{
				this.onClose.Invoke();
			}
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0011CC29 File Offset: 0x0011AE29
		private void Exit(ExitAction action)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (action.used)
			{
				return;
			}
			if (this.CanExitWithRightClick || action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.Close();
			}
		}

		// Token: 0x040031B6 RID: 12726
		[Header("Settings")]
		public string Name;

		// Token: 0x040031B7 RID: 12727
		public bool UseExitActions = true;

		// Token: 0x040031B8 RID: 12728
		public int ExitActionPriority;

		// Token: 0x040031B9 RID: 12729
		public bool CanExitWithRightClick = true;

		// Token: 0x040031BA RID: 12730
		public bool ReenableControlsOnClose = true;

		// Token: 0x040031BB RID: 12731
		public bool ReenableInventoryOnClose = true;

		// Token: 0x040031BC RID: 12732
		public bool ReenableEquippingOnClose = true;

		// Token: 0x040031BD RID: 12733
		public UnityEvent onOpen;

		// Token: 0x040031BE RID: 12734
		public UnityEvent onClose;
	}
}
