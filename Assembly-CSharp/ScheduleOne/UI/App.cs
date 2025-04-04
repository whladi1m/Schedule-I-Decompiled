using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI.Phone;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009F6 RID: 2550
	public abstract class App<T> : PlayerSingleton<T> where T : PlayerSingleton<T>
	{
		// Token: 0x060044C8 RID: 17608 RVA: 0x00120338 File Offset: 0x0011E538
		public static App<T> GetApp(int index)
		{
			if (index < 0 || index >= App<T>.Apps.Count)
			{
				return null;
			}
			return App<T>.Apps[index];
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060044C9 RID: 17609 RVA: 0x00120358 File Offset: 0x0011E558
		// (set) Token: 0x060044CA RID: 17610 RVA: 0x00120360 File Offset: 0x0011E560
		public bool isOpen { get; protected set; }

		// Token: 0x060044CB RID: 17611 RVA: 0x0012036C File Offset: 0x0011E56C
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				return;
			}
			if (!this.AvailableInTutorial && NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				this.appContainer.gameObject.SetActive(false);
				return;
			}
			this.GenerateHomeScreenIcon();
			App<T>.Apps.Add(this);
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x001203BC File Offset: 0x0011E5BC
		protected override void Start()
		{
			base.Start();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			Phone instance = PlayerSingleton<Phone>.Instance;
			instance.closeApps = (Action)Delegate.Combine(instance.closeApps, new Action(this.Close));
			Phone instance2 = PlayerSingleton<Phone>.Instance;
			instance2.onPhoneOpened = (Action)Delegate.Combine(instance2.onPhoneOpened, new Action(this.OnPhoneOpened));
			this.SetOpen(false);
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x00120436 File Offset: 0x0011E636
		private void Close()
		{
			if (this.isOpen)
			{
				this.SetOpen(false);
			}
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x00120447 File Offset: 0x0011E647
		protected virtual void Update()
		{
			if (this.isOpen && PlayerSingleton<Phone>.Instance.IsOpen && this.IsHoveringButton() && GameInput.GetButtonDown(GameInput.ButtonCode.PrimaryClick))
			{
				this.SetOpen(false);
			}
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x00120474 File Offset: 0x0011E674
		private bool IsHoveringButton()
		{
			RaycastHit raycastHit;
			return Physics.Raycast(Singleton<GameplayMenu>.Instance.OverlayCamera.ScreenPointToRay(Input.mousePosition), out raycastHit, 2f, 1 << LayerMask.NameToLayer("Overlay")) && raycastHit.collider.gameObject.name == "Button";
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x001204D4 File Offset: 0x0011E6D4
		private void GenerateHomeScreenIcon()
		{
			this.appIconButton = PlayerSingleton<HomeScreen>.Instance.GenerateAppIcon<T>(this);
			this.appIconButton.onClick.AddListener(new UnityAction(this.ShortcutClicked));
			this.notificationContainer = this.appIconButton.transform.Find("Notifications").GetComponent<RectTransform>();
			this.notificationText = this.notificationContainer.Find("Text").GetComponent<Text>();
			this.notificationContainer.gameObject.SetActive(false);
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0012055A File Offset: 0x0011E75A
		public void SetNotificationCount(int amount)
		{
			this.notificationText.text = amount.ToString();
			this.notificationContainer.gameObject.SetActive(amount > 0);
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x00120582 File Offset: 0x0011E782
		protected virtual void OnPhoneOpened()
		{
			if (this.isOpen)
			{
				if (this.Orientation == App<T>.EOrientation.Horizontal)
				{
					PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(0.6f);
					return;
				}
				PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(1f);
			}
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x001205B3 File Offset: 0x0011E7B3
		private void ShortcutClicked()
		{
			this.SetOpen(!this.isOpen);
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x001205C4 File Offset: 0x0011E7C4
		public virtual void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (this.isOpen && PlayerSingleton<Phone>.InstanceExists && PlayerSingleton<Phone>.Instance.IsOpen)
			{
				exit.used = true;
				this.SetOpen(false);
			}
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x001205F8 File Offset: 0x0011E7F8
		public virtual void SetOpen(bool open)
		{
			if (open && Phone.ActiveApp != null)
			{
				Console.LogWarning(Phone.ActiveApp.name + " is already open", null);
			}
			this.isOpen = open;
			PlayerSingleton<AppsCanvas>.Instance.SetIsOpen(open);
			PlayerSingleton<HomeScreen>.Instance.SetIsOpen(!open);
			if (this.isOpen)
			{
				if (this.Orientation == App<T>.EOrientation.Horizontal)
				{
					PlayerSingleton<Phone>.Instance.SetIsHorizontal(true);
					PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(0.6f);
				}
				else
				{
					PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(1f);
				}
				Phone.ActiveApp = base.gameObject;
			}
			else
			{
				if (Phone.ActiveApp == base.gameObject)
				{
					Phone.ActiveApp = null;
				}
				PlayerSingleton<Phone>.Instance.SetIsHorizontal(false);
				PlayerSingleton<Phone>.Instance.SetLookOffsetMultiplier(1f);
				Singleton<CursorManager>.Instance.SetCursorAppearance(CursorManager.ECursorType.Default);
			}
			this.appContainer.gameObject.SetActive(open);
		}

		// Token: 0x040032AC RID: 12972
		public static List<App<T>> Apps = new List<App<T>>();

		// Token: 0x040032AD RID: 12973
		[Header("Settings")]
		public string AppName;

		// Token: 0x040032AE RID: 12974
		public string IconLabel;

		// Token: 0x040032AF RID: 12975
		public Sprite AppIcon;

		// Token: 0x040032B0 RID: 12976
		public App<T>.EOrientation Orientation;

		// Token: 0x040032B1 RID: 12977
		public bool AvailableInTutorial = true;

		// Token: 0x040032B2 RID: 12978
		[Header("References")]
		[SerializeField]
		protected RectTransform appContainer;

		// Token: 0x040032B3 RID: 12979
		protected RectTransform notificationContainer;

		// Token: 0x040032B4 RID: 12980
		protected Text notificationText;

		// Token: 0x040032B6 RID: 12982
		protected Button appIconButton;

		// Token: 0x020009F7 RID: 2551
		public enum EOrientation
		{
			// Token: 0x040032B8 RID: 12984
			Horizontal,
			// Token: 0x040032B9 RID: 12985
			Vertical
		}
	}
}
