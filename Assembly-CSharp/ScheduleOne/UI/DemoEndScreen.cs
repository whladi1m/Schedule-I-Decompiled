using System;
using EasyButtons;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using Steamworks;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009BA RID: 2490
	public class DemoEndScreen : MonoBehaviour
	{
		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x0600434C RID: 17228 RVA: 0x0011A273 File Offset: 0x00118473
		// (set) Token: 0x0600434D RID: 17229 RVA: 0x0011A27B File Offset: 0x0011847B
		public bool IsOpen { get; private set; }

		// Token: 0x0600434E RID: 17230 RVA: 0x0011A284 File Offset: 0x00118484
		public void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x0011A2B5 File Offset: 0x001184B5
		private void OnDestroy()
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x000045B1 File Offset: 0x000027B1
		[Button]
		public void Open()
		{
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x0011A2C8 File Offset: 0x001184C8
		private void Update()
		{
			if (this.IsOpen)
			{
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
			}
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x0011A2E0 File Offset: 0x001184E0
		public void Close()
		{
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.25f);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
			PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x0011A371 File Offset: 0x00118571
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
			if (action.exitType != ExitType.Escape)
			{
				return;
			}
			action.used = true;
			this.Close();
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x0011A39C File Offset: 0x0011859C
		public void LinkClicked()
		{
			if (SteamManager.Initialized)
			{
				SteamFriends.ActivateGameOverlayToStore(new AppId_t(3164500U), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			}
		}

		// Token: 0x04003129 RID: 12585
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400312A RID: 12586
		public RectTransform Container;
	}
}
