using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A0D RID: 2573
	public class SleepCanvas : Singleton<SleepCanvas>
	{
		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06004570 RID: 17776 RVA: 0x001232C7 File Offset: 0x001214C7
		// (set) Token: 0x06004571 RID: 17777 RVA: 0x001232CF File Offset: 0x001214CF
		public bool IsMenuOpen { get; protected set; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06004572 RID: 17778 RVA: 0x001232D8 File Offset: 0x001214D8
		// (set) Token: 0x06004573 RID: 17779 RVA: 0x001232E0 File Offset: 0x001214E0
		public string QueuedSleepMessage { get; protected set; } = string.Empty;

		// Token: 0x06004574 RID: 17780 RVA: 0x001232EC File Offset: 0x001214EC
		protected override void Awake()
		{
			base.Awake();
			this.IncreaseButton.onClick.AddListener(delegate()
			{
				this.ChangeSleepAmount(1);
			});
			this.DecreaseButton.onClick.AddListener(delegate()
			{
				this.ChangeSleepAmount(-1);
			});
			this.SleepButton.onClick.AddListener(new UnityAction(this.SleepButtonPressed));
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 1);
			TimeManager.onSleepStart = (Action)Delegate.Combine(TimeManager.onSleepStart, new Action(this.SleepStart));
			this.TimeLabel.enabled = false;
			this.WakeLabel.enabled = false;
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x0012339D File Offset: 0x0012159D
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (this.IsMenuOpen && action.exitType == ExitType.Escape)
			{
				action.used = true;
				this.SetIsOpen(false);
			}
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x001233C8 File Offset: 0x001215C8
		public void SetIsOpen(bool open)
		{
			this.IsMenuOpen = open;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
			if (open)
			{
				this.Update();
				NetworkSingleton<TimeManager>.Instance.SetWakeTime(this.ClampWakeTime(700));
				this.UpdateTimeLabels();
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				PlayerSingleton<PlayerMovement>.Instance.canMove = false;
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				this.Canvas.enabled = true;
				this.Container.gameObject.SetActive(true);
			}
			else
			{
				Player.Local.CurrentBed = null;
				Player.Local.SetReadyToSleep(false);
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, true, false);
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
			}
			this.MenuContainer.gameObject.SetActive(open);
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x001234EC File Offset: 0x001216EC
		public void Update()
		{
			if (this.IsMenuOpen)
			{
				this.UpdateHourSetting();
				this.UpdateTimeLabels();
				this.UpdateSleepButton();
			}
			if (this.Canvas.enabled)
			{
				this.CurrentTimeLabel.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
			}
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x0012353C File Offset: 0x0012173C
		public void AddPostSleepEvent(IPostSleepEvent postSleepEvent)
		{
			Console.Log("Adding post sleep event: " + postSleepEvent.GetType().Name, null);
			this.queuedPostSleepEvents.Add(postSleepEvent);
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x00123565 File Offset: 0x00121765
		private void UpdateHourSetting()
		{
			this.IncreaseButton.interactable = true;
			this.DecreaseButton.interactable = true;
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x0012357F File Offset: 0x0012177F
		private void UpdateTimeLabels()
		{
			this.EndTimeLabel.text = TimeManager.Get12HourTime(700f, true);
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x00123597 File Offset: 0x00121797
		private void UpdateSleepButton()
		{
			if (Player.Local.IsReadyToSleep)
			{
				this.SleepButtonLabel.text = "Waiting for other players";
				return;
			}
			this.SleepButtonLabel.text = "Sleep";
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x001235C8 File Offset: 0x001217C8
		private void ChangeSleepAmount(int change)
		{
			int num = TimeManager.AddMinutesTo24HourTime(700, change * 60);
			num = this.ClampWakeTime(num);
			NetworkSingleton<TimeManager>.Instance.SetWakeTime(num);
			this.UpdateHourSetting();
			this.UpdateTimeLabels();
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x00123604 File Offset: 0x00121804
		private int ClampWakeTime(int time)
		{
			int currentTime = NetworkSingleton<TimeManager>.Instance.CurrentTime;
			int time2 = TimeManager.AddMinutesTo24HourTime(currentTime, 60 - currentTime % 100);
			int startTime = TimeManager.AddMinutesTo24HourTime(time2, 240);
			int endTime = TimeManager.AddMinutesTo24HourTime(time2, 720);
			return this.ClampTime(time, startTime, endTime);
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x0012364C File Offset: 0x0012184C
		private int ClampTime(int time, int startTime, int endTime)
		{
			if (endTime > startTime)
			{
				if (time < startTime)
				{
					return startTime;
				}
				if (time > endTime)
				{
					return endTime;
				}
			}
			else if (time < startTime && time > endTime)
			{
				int max = TimeManager.AddMinutesTo24HourTime(endTime, 720);
				if (TimeManager.IsGivenTimeWithinRange(time, endTime, max))
				{
					return endTime;
				}
				return startTime;
			}
			return time;
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x0012368C File Offset: 0x0012188C
		private void SleepButtonPressed()
		{
			Player.Local.SetReadyToSleep(!Player.Local.IsReadyToSleep);
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x001236A8 File Offset: 0x001218A8
		private void SleepStart()
		{
			Player.Local.SetReadyToSleep(false);
			this.MenuContainer.gameObject.SetActive(false);
			this.IsMenuOpen = false;
			int num = 700;
			this.WakeLabel.text = "Waking up at " + TimeManager.Get12HourTime((float)num, true);
			base.StartCoroutine(this.<SleepStart>g__Sleep|41_0());
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x00123708 File Offset: 0x00121908
		private void LerpBlackOverlay(float transparency, float lerpTime)
		{
			SleepCanvas.<>c__DisplayClass42_0 CS$<>8__locals1 = new SleepCanvas.<>c__DisplayClass42_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.transparency = transparency;
			CS$<>8__locals1.lerpTime = lerpTime;
			if (CS$<>8__locals1.transparency > 0f)
			{
				this.BlackOverlay.enabled = true;
			}
			base.StartCoroutine(CS$<>8__locals1.<LerpBlackOverlay>g__Routine|0());
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00123758 File Offset: 0x00121958
		public void QueueSleepMessage(string message, float displayTime = 3f)
		{
			Console.Log(string.Concat(new string[]
			{
				"Queueing sleep message: ",
				message,
				" for ",
				displayTime.ToString(),
				" seconds"
			}), null);
			this.QueuedSleepMessage = message;
			this.QueuedMessageDisplayTime = displayTime;
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x001237DA File Offset: 0x001219DA
		[CompilerGenerated]
		private IEnumerator <SleepStart>g__Sleep|41_0()
		{
			this.BlackOverlay.enabled = true;
			this.SleepMessageLabel.text = string.Empty;
			if (InstanceFinder.IsServer)
			{
				Console.Log("Resetting host sleep done", null);
				NetworkSingleton<TimeManager>.Instance.ResetHostSleepDone();
			}
			Singleton<HUD>.Instance.canvas.enabled = false;
			this.LerpBlackOverlay(1f, 0.5f);
			yield return new WaitForSecondsRealtime(0.5f);
			if (this.onSleepFullyFaded != null)
			{
				this.onSleepFullyFaded.Invoke();
			}
			yield return new WaitForSecondsRealtime(0.5f);
			NetworkSingleton<DailySummary>.Instance.Open();
			yield return new WaitUntil(() => !NetworkSingleton<DailySummary>.Instance.IsOpen);
			this.queuedPostSleepEvents = (from x in this.queuedPostSleepEvents
			orderby x.Order
			select x).ToList<IPostSleepEvent>();
			using (List<IPostSleepEvent>.Enumerator enumerator = this.queuedPostSleepEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SleepCanvas.<>c__DisplayClass41_0 CS$<>8__locals1 = new SleepCanvas.<>c__DisplayClass41_0();
					CS$<>8__locals1.pse = enumerator.Current;
					yield return new WaitForSecondsRealtime(0.5f);
					Console.Log("Running post sleep event: " + CS$<>8__locals1.pse.GetType().Name, null);
					CS$<>8__locals1.pse.StartEvent();
					yield return new WaitUntil(() => !CS$<>8__locals1.pse.IsRunning);
					CS$<>8__locals1 = null;
				}
			}
			List<IPostSleepEvent>.Enumerator enumerator = default(List<IPostSleepEvent>.Enumerator);
			this.queuedPostSleepEvents.Clear();
			if (InstanceFinder.IsServer)
			{
				Console.Log("Marking host sleep done", null);
				NetworkSingleton<TimeManager>.Instance.MarkHostSleepDone();
			}
			else
			{
				this.WaitingForHostLabel.enabled = true;
				yield return new WaitUntil(() => NetworkSingleton<TimeManager>.Instance.HostDailySummaryDone);
				this.WaitingForHostLabel.enabled = false;
			}
			NetworkSingleton<TimeManager>.Instance.FastForwardToWakeTime();
			this.TimeLabel.enabled = true;
			if (InstanceFinder.IsServer)
			{
				Singleton<SaveManager>.Instance.DelayedSave();
			}
			yield return new WaitForSecondsRealtime(1f);
			this.TimeLabel.enabled = false;
			if (this.onSleepEndFade != null)
			{
				this.onSleepEndFade.Invoke();
			}
			if (!string.IsNullOrEmpty(this.QueuedSleepMessage))
			{
				yield return new WaitForSecondsRealtime(0.5f);
				this.SleepMessageLabel.text = this.QueuedSleepMessage;
				this.QueuedSleepMessage = string.Empty;
				this.SleepMessageGroup.alpha = 0f;
				float lerpTime = 0.5f;
				for (float i = 0f; i < lerpTime; i += Time.deltaTime)
				{
					this.SleepMessageGroup.alpha = i / lerpTime;
					yield return new WaitForEndOfFrame();
				}
				this.SleepMessageGroup.alpha = 1f;
				yield return new WaitForSecondsRealtime(this.QueuedMessageDisplayTime);
				for (float i = 0f; i < lerpTime; i += Time.deltaTime)
				{
					this.SleepMessageGroup.alpha = 1f - i / lerpTime;
					yield return new WaitForEndOfFrame();
				}
				this.SleepMessageGroup.alpha = 0f;
				yield return new WaitForSecondsRealtime(0.5f);
			}
			PlayerSingleton<PlayerCamera>.Instance.StopTransformOverride(0f, false, true);
			this.TimeLabel.enabled = false;
			this.WakeLabel.enabled = false;
			if (!NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				Singleton<HUD>.Instance.canvas.enabled = true;
			}
			yield return new WaitForSecondsRealtime(0.1f);
			if (!NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				this.SetIsOpen(false);
			}
			this.LerpBlackOverlay(0f, 0.5f);
			yield break;
			yield break;
		}

		// Token: 0x0400334F RID: 13135
		public const int MaxSleepTime = 12;

		// Token: 0x04003350 RID: 13136
		public const int MinSleepTime = 4;

		// Token: 0x04003353 RID: 13139
		private float QueuedMessageDisplayTime;

		// Token: 0x04003354 RID: 13140
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04003355 RID: 13141
		public RectTransform Container;

		// Token: 0x04003356 RID: 13142
		public RectTransform MenuContainer;

		// Token: 0x04003357 RID: 13143
		public TextMeshProUGUI CurrentTimeLabel;

		// Token: 0x04003358 RID: 13144
		public Button IncreaseButton;

		// Token: 0x04003359 RID: 13145
		public Button DecreaseButton;

		// Token: 0x0400335A RID: 13146
		public TextMeshProUGUI EndTimeLabel;

		// Token: 0x0400335B RID: 13147
		public Button SleepButton;

		// Token: 0x0400335C RID: 13148
		public TextMeshProUGUI SleepButtonLabel;

		// Token: 0x0400335D RID: 13149
		public Image BlackOverlay;

		// Token: 0x0400335E RID: 13150
		public TextMeshProUGUI SleepMessageLabel;

		// Token: 0x0400335F RID: 13151
		public CanvasGroup SleepMessageGroup;

		// Token: 0x04003360 RID: 13152
		public TextMeshProUGUI TimeLabel;

		// Token: 0x04003361 RID: 13153
		public TextMeshProUGUI WakeLabel;

		// Token: 0x04003362 RID: 13154
		public TextMeshProUGUI WaitingForHostLabel;

		// Token: 0x04003363 RID: 13155
		public UnityEvent onSleepFullyFaded;

		// Token: 0x04003364 RID: 13156
		public UnityEvent onSleepEndFade;

		// Token: 0x04003365 RID: 13157
		private List<IPostSleepEvent> queuedPostSleepEvents = new List<IPostSleepEvent>();
	}
}
