using System;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.MainMenu;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x020009F5 RID: 2549
	public class PauseMenu : Singleton<PauseMenu>
	{
		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060044BD RID: 17597 RVA: 0x00120046 File Offset: 0x0011E246
		// (set) Token: 0x060044BE RID: 17598 RVA: 0x0012004E File Offset: 0x0011E24E
		public bool IsPaused { get; protected set; }

		// Token: 0x060044BF RID: 17599 RVA: 0x00120057 File Offset: 0x0011E257
		protected override void Awake()
		{
			base.Awake();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), -100);
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x00120072 File Offset: 0x0011E272
		protected override void Start()
		{
			base.Start();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x00120097 File Offset: 0x0011E297
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (action.exitType == ExitType.RightClick)
			{
				return;
			}
			if (this.justResumed)
			{
				return;
			}
			if (GameInput.IsTyping)
			{
				return;
			}
			if (this.IsPaused)
			{
				this.Resume();
				return;
			}
			this.Pause();
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x001200D1 File Offset: 0x0011E2D1
		private void Update()
		{
			bool instanceExists = PlayerSingleton<PlayerCamera>.InstanceExists;
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x001200D9 File Offset: 0x0011E2D9
		private void LateUpdate()
		{
			if (!PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				return;
			}
			this.noActiveUIElements = (PlayerSingleton<PlayerCamera>.Instance.activeUIElementCount == 0);
			this.justPaused = false;
			this.justResumed = false;
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x00120104 File Offset: 0x0011E304
		public void Pause()
		{
			Console.Log("Game paused", null);
			this.IsPaused = true;
			this.justPaused = true;
			if (this.FeedbackForm != null)
			{
				this.FeedbackForm.PrepScreenshot();
			}
			if (Singleton<Settings>.InstanceExists && Singleton<Settings>.Instance.PausingFreezesTime)
			{
				Time.timeScale = 0f;
			}
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				this.couldLook = PlayerSingleton<PlayerCamera>.Instance.canLook;
				this.lockedMouse = (Cursor.lockState == CursorLockMode.Locked);
				this.crosshairVisible = Singleton<HUD>.Instance.crosshair.gameObject.activeSelf;
				this.hudVisible = Singleton<HUD>.Instance.canvas.enabled;
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(true, 0.075f);
				Singleton<HUD>.Instance.canvas.enabled = false;
			}
			this.Screen.Open(false);
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x00120218 File Offset: 0x0011E418
		public void Resume()
		{
			Console.Log("Game resumed", null);
			this.IsPaused = false;
			this.justResumed = true;
			if (Singleton<Settings>.InstanceExists && Singleton<Settings>.Instance.PausingFreezesTime)
			{
				if (NetworkSingleton<TimeManager>.Instance.SleepInProgress)
				{
					Time.timeScale = 1f;
				}
				else
				{
					Time.timeScale = 1f;
				}
			}
			if (PlayerSingleton<PlayerCamera>.InstanceExists)
			{
				if (this.couldLook)
				{
					PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				}
				if (this.lockedMouse && (!Singleton<CharacterCreator>.InstanceExists || !Singleton<CharacterCreator>.Instance.IsOpen))
				{
					PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				}
				PlayerSingleton<PlayerCamera>.Instance.SetDoFActive(false, 0.075f);
			}
			if (Singleton<HUD>.InstanceExists)
			{
				Singleton<HUD>.Instance.SetCrosshairVisible(this.crosshairVisible);
				Singleton<HUD>.Instance.canvas.enabled = this.hudVisible;
			}
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.Screen.Close(false);
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x00120317 File Offset: 0x0011E517
		public void StuckButtonClicked()
		{
			this.Resume();
			PlayerSingleton<PlayerMovement>.Instance.WarpToNavMesh();
		}

		// Token: 0x0400329F RID: 12959
		public Canvas Canvas;

		// Token: 0x040032A0 RID: 12960
		public RectTransform Container;

		// Token: 0x040032A1 RID: 12961
		public MainMenuScreen Screen;

		// Token: 0x040032A2 RID: 12962
		public FeedbackForm FeedbackForm;

		// Token: 0x040032A3 RID: 12963
		private bool noActiveUIElements = true;

		// Token: 0x040032A4 RID: 12964
		private bool justPaused;

		// Token: 0x040032A5 RID: 12965
		private bool justResumed;

		// Token: 0x040032A6 RID: 12966
		private bool couldLook;

		// Token: 0x040032A7 RID: 12967
		private bool lockedMouse;

		// Token: 0x040032A8 RID: 12968
		private bool crosshairVisible;

		// Token: 0x040032A9 RID: 12969
		private bool hudVisible;

		// Token: 0x040032AA RID: 12970
		public UnityEvent onPause;

		// Token: 0x040032AB RID: 12971
		public UnityEvent onResume;
	}
}
