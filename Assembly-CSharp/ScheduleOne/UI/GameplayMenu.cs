using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Compass;
using ScheduleOne.UI.Items;
using ScheduleOne.UI.Phone;
using ScheduleOne.UI.Phone.Map;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009C8 RID: 2504
	public class GameplayMenu : Singleton<GameplayMenu>
	{
		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060043B1 RID: 17329 RVA: 0x0011BDCF File Offset: 0x00119FCF
		// (set) Token: 0x060043B2 RID: 17330 RVA: 0x0011BDD7 File Offset: 0x00119FD7
		public bool IsOpen { get; protected set; }

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CharacterScreenEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x060043B4 RID: 17332 RVA: 0x0011BDE0 File Offset: 0x00119FE0
		// (set) Token: 0x060043B5 RID: 17333 RVA: 0x0011BDE8 File Offset: 0x00119FE8
		public GameplayMenu.EGameplayScreen CurrentScreen { get; protected set; }

		// Token: 0x060043B6 RID: 17334 RVA: 0x0011BDF4 File Offset: 0x00119FF4
		protected override void Start()
		{
			base.Start();
			this.OverlayCamera.enabled = false;
			this.OverlayLight.enabled = false;
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, -2f, base.transform.localPosition.z);
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 0);
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0011BE68 File Offset: 0x0011A068
		public void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (exit.exitType == ExitType.RightClick && Singleton<ItemUIManager>.InstanceExists && Singleton<ItemUIManager>.Instance.CanDragFromSlot(Singleton<ItemUIManager>.Instance.HoveredSlot))
			{
				return;
			}
			if (this.IsOpen)
			{
				exit.used = true;
				this.SetIsOpen(false);
			}
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0011BEBC File Offset: 0x0011A0BC
		protected virtual void Update()
		{
			if (!GameInput.IsTyping && !Singleton<PauseMenu>.Instance.IsPaused && (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0 || this.IsOpen))
			{
				if (GameInput.GetButtonDown(GameInput.ButtonCode.TogglePhone))
				{
					this.SetIsOpen(!this.IsOpen);
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.OpenMap) && !GameManager.IS_TUTORIAL)
				{
					if (PlayerSingleton<MapApp>.Instance.isOpen && this.IsOpen && this.CurrentScreen == GameplayMenu.EGameplayScreen.Phone)
					{
						this.SetIsOpen(false);
					}
					else
					{
						this.<Update>g__PrepAppOpen|22_0();
						PlayerSingleton<MapApp>.Instance.SetOpen(true);
					}
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.OpenJournal))
				{
					if (PlayerSingleton<JournalApp>.Instance.isOpen && this.IsOpen && this.CurrentScreen == GameplayMenu.EGameplayScreen.Phone)
					{
						this.SetIsOpen(false);
					}
					else
					{
						this.<Update>g__PrepAppOpen|22_0();
						PlayerSingleton<JournalApp>.Instance.SetOpen(true);
					}
				}
				if (GameInput.GetButtonDown(GameInput.ButtonCode.OpenTexts))
				{
					if (PlayerSingleton<MessagesApp>.Instance.isOpen && this.IsOpen && this.CurrentScreen == GameplayMenu.EGameplayScreen.Phone)
					{
						this.SetIsOpen(false);
					}
					else
					{
						this.<Update>g__PrepAppOpen|22_0();
						PlayerSingleton<MessagesApp>.Instance.SetOpen(true);
					}
				}
				if (this.IsOpen)
				{
					bool characterScreenEnabled = this.CharacterScreenEnabled;
				}
			}
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x0011BFE4 File Offset: 0x0011A1E4
		public void SetScreen(GameplayMenu.EGameplayScreen screen)
		{
			GameplayMenu.<>c__DisplayClass23_0 CS$<>8__locals1 = new GameplayMenu.<>c__DisplayClass23_0();
			CS$<>8__locals1.screen = screen;
			CS$<>8__locals1.<>4__this = this;
			if (this.CurrentScreen == CS$<>8__locals1.screen)
			{
				return;
			}
			CS$<>8__locals1.previousScreen = this.CurrentScreen;
			this.CurrentScreen = CS$<>8__locals1.screen;
			if (CS$<>8__locals1.screen == GameplayMenu.EGameplayScreen.Phone)
			{
				PlayerSingleton<Phone>.Instance.SetIsOpen(true);
			}
			else if (CS$<>8__locals1.screen == GameplayMenu.EGameplayScreen.Character)
			{
				Singleton<CharacterDisplay>.Instance.SetOpen(true);
			}
			if (this.screenChangeRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.screenChangeRoutine);
			}
			Singleton<GameplayMenuInterface>.Instance.SetSelected(CS$<>8__locals1.screen);
			this.screenChangeRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetScreen>g__ScreenChange|0());
		}

		// Token: 0x060043BA RID: 17338 RVA: 0x0011C094 File Offset: 0x0011A294
		public void SetIsOpen(bool open)
		{
			this.IsOpen = open;
			if (open)
			{
				this.OverlayLight.enabled = true;
			}
			if (this.CurrentScreen == GameplayMenu.EGameplayScreen.Phone)
			{
				if (open)
				{
					PlayerSingleton<Phone>.Instance.SetIsOpen(true);
				}
				else
				{
					PlayerSingleton<Phone>.Instance.SetIsOpen(false);
				}
			}
			else if (this.CurrentScreen == GameplayMenu.EGameplayScreen.Character)
			{
				if (open)
				{
					Singleton<CharacterDisplay>.Instance.SetOpen(true);
				}
				else
				{
					Singleton<CharacterDisplay>.Instance.SetOpen(false);
				}
			}
			if (this.IsOpen)
			{
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
				PlayerSingleton<PlayerMovement>.Instance.canMove = false;
				Singleton<ItemUIManager>.Instance.SetDraggingEnabled(true, false);
				Singleton<CompassManager>.Instance.SetVisible(false);
				Player.Local.SendEquippable_Networked("Avatar/Equippables/Phone_Lowered");
				Singleton<InputPromptsCanvas>.Instance.LoadModule("phone");
				Singleton<GameplayMenuInterface>.Instance.Open();
			}
			else
			{
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				if (Player.Local.CurrentVehicle == null)
				{
					PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
					PlayerSingleton<PlayerMovement>.Instance.canMove = true;
					PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
				}
				else
				{
					Singleton<HUD>.Instance.SetCrosshairVisible(false);
				}
				Singleton<ItemUIManager>.Instance.SetDraggingEnabled(false, true);
				Singleton<CompassManager>.Instance.SetVisible(true);
				Player.Local.SendEquippable_Networked(string.Empty);
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
				Singleton<GameplayMenuInterface>.Instance.Close();
			}
			if (this.openCloseRoutine != null)
			{
				base.StopCoroutine(this.openCloseRoutine);
			}
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (this.IsOpen)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
			}
			this.openCloseRoutine = base.StartCoroutine(this.<SetIsOpen>g__SetIsOpenRoutine|24_0(open));
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x0011C25D File Offset: 0x0011A45D
		[CompilerGenerated]
		private void <Update>g__PrepAppOpen|22_0()
		{
			if (!this.IsOpen)
			{
				this.SetIsOpen(true);
			}
			if (this.CurrentScreen != GameplayMenu.EGameplayScreen.Phone)
			{
				this.SetScreen(GameplayMenu.EGameplayScreen.Phone);
			}
			if (Phone.ActiveApp != null)
			{
				PlayerSingleton<Phone>.Instance.RequestCloseApp();
			}
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x0011C294 File Offset: 0x0011A494
		[CompilerGenerated]
		private IEnumerator <SetIsOpen>g__SetIsOpenRoutine|24_0(bool open)
		{
			if (open)
			{
				this.OverlayCamera.enabled = true;
			}
			float num = 1f - base.transform.localPosition.y / -2f;
			float adjustedLerpTime = 0.06f;
			float startVert = base.transform.localPosition.y;
			float endVert = 0f;
			if (open)
			{
				adjustedLerpTime *= 1f - num;
				endVert = 0.02f;
			}
			else
			{
				adjustedLerpTime *= num;
				endVert = -2f;
			}
			PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(open, adjustedLerpTime);
			for (float i = 0f; i < adjustedLerpTime; i += Time.deltaTime)
			{
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(startVert, endVert, i / adjustedLerpTime), base.transform.localPosition.z);
				yield return new WaitForEndOfFrame();
			}
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, endVert, base.transform.localPosition.z);
			if (!open)
			{
				this.OverlayCamera.enabled = false;
				this.OverlayLight.enabled = false;
			}
			this.openCloseRoutine = null;
			yield break;
		}

		// Token: 0x0400317F RID: 12671
		public const float OpenVerticalOffset = 0.02f;

		// Token: 0x04003180 RID: 12672
		public const float ClosedVerticalOffset = -2f;

		// Token: 0x04003181 RID: 12673
		public const float OpenTime = 0.06f;

		// Token: 0x04003182 RID: 12674
		public const float SlideTime = 0.12f;

		// Token: 0x04003185 RID: 12677
		[Header("References")]
		public Camera OverlayCamera;

		// Token: 0x04003186 RID: 12678
		public Light OverlayLight;

		// Token: 0x04003187 RID: 12679
		[Header("Settings")]
		public float ContainerOffset_PhoneScreen = -0.1f;

		// Token: 0x04003188 RID: 12680
		private Coroutine openCloseRoutine;

		// Token: 0x04003189 RID: 12681
		private Coroutine screenChangeRoutine;

		// Token: 0x020009C9 RID: 2505
		public enum EGameplayScreen
		{
			// Token: 0x0400318B RID: 12683
			Phone,
			// Token: 0x0400318C RID: 12684
			Character
		}
	}
}
