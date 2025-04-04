using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.Messaging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000AB0 RID: 2736
	public class DealWindowSelector : MonoBehaviour
	{
		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x060049B0 RID: 18864 RVA: 0x0013462E File Offset: 0x0013282E
		// (set) Token: 0x060049B1 RID: 18865 RVA: 0x00134636 File Offset: 0x00132836
		public bool IsOpen { get; private set; }

		// Token: 0x060049B2 RID: 18866 RVA: 0x00134640 File Offset: 0x00132840
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			this.buttons = new WindowSelectorButton[]
			{
				this.MorningButton,
				this.AfternoonButton,
				this.NightButton,
				this.LateNightButton
			};
			WindowSelectorButton[] array = this.buttons;
			for (int i = 0; i < array.Length; i++)
			{
				WindowSelectorButton button = array[i];
				button.OnSelected.AddListener(delegate()
				{
					this.ButtonClicked(button.WindowType);
				});
			}
			this.SetIsOpen(false);
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x001346DD File Offset: 0x001328DD
		public void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			action.used = true;
			this.SetIsOpen(false);
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x001346FF File Offset: 0x001328FF
		public void SetIsOpen(bool open)
		{
			this.SetIsOpen(open, null, null);
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x0013470C File Offset: 0x0013290C
		public void SetIsOpen(bool open, MSGConversation conversation, Action<EDealWindow> callback = null)
		{
			this.IsOpen = open;
			if (open)
			{
				this.UpdateTime();
				this.UpdateWindowValidity();
				conversation.onMessageRendered = (Action)Delegate.Combine(conversation.onMessageRendered, new Action(this.Close));
			}
			else
			{
				callback = null;
				if (conversation != null)
				{
					conversation.onMessageRendered = (Action)Delegate.Remove(conversation.onMessageRendered, new Action(this.Close));
				}
				WindowSelectorButton[] array = this.buttons;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetHoverIndicator(false);
				}
			}
			if (open && NetworkSingleton<GameManager>.Instance.IsTutorial && !this.hintShown)
			{
				this.hintShown = true;
				Singleton<HintDisplay>.Instance.ShowHint_20s("You can complete deals any time within the window you choose. For now, choose the morning window.");
			}
			this.Container.gameObject.SetActive(open);
			this.callback = callback;
		}

		// Token: 0x060049B6 RID: 18870 RVA: 0x001347DE File Offset: 0x001329DE
		public void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.UpdateTime();
			this.UpdateWindowValidity();
		}

		// Token: 0x060049B7 RID: 18871 RVA: 0x001347F8 File Offset: 0x001329F8
		private void UpdateTime()
		{
			this.CurrentTimeLabel.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
			float t = (float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 1440f;
			this.CurrentTimeArm.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, -360f, t));
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x00134860 File Offset: 0x00132A60
		private void UpdateWindowValidity()
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				this.MorningButton.SetInteractable(true);
				this.AfternoonButton.SetInteractable(false);
				this.NightButton.SetInteractable(false);
				this.LateNightButton.SetInteractable(false);
				return;
			}
			int dailyMinTotal = NetworkSingleton<TimeManager>.Instance.DailyMinTotal;
			foreach (WindowSelectorButton windowSelectorButton in this.buttons)
			{
				int num = TimeManager.GetMinSumFrom24HourTime(DealWindowInfo.GetWindowInfo(windowSelectorButton.WindowType).EndTime);
				if (dailyMinTotal > num)
				{
					num += 1440;
				}
				windowSelectorButton.SetInteractable(num - dailyMinTotal > 120);
			}
		}

		// Token: 0x060049B9 RID: 18873 RVA: 0x001348FB File Offset: 0x00132AFB
		private void Close()
		{
			this.SetIsOpen(false);
		}

		// Token: 0x060049BA RID: 18874 RVA: 0x00134904 File Offset: 0x00132B04
		private void ButtonClicked(EDealWindow window)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (this.OnSelected != null)
			{
				this.OnSelected.Invoke(window);
			}
			if (this.callback != null)
			{
				this.callback(window);
			}
			this.SetIsOpen(false);
		}

		// Token: 0x0400370B RID: 14091
		public const float TIME_ARM_ROTATION_0000 = 0f;

		// Token: 0x0400370C RID: 14092
		public const float TIME_ARM_ROTATION_2400 = -360f;

		// Token: 0x0400370D RID: 14093
		public const int WINDOW_CUTOFF_MINS = 120;

		// Token: 0x0400370E RID: 14094
		public UnityEvent<EDealWindow> OnSelected;

		// Token: 0x04003710 RID: 14096
		[Header("References")]
		public GameObject Container;

		// Token: 0x04003711 RID: 14097
		public WindowSelectorButton MorningButton;

		// Token: 0x04003712 RID: 14098
		public WindowSelectorButton AfternoonButton;

		// Token: 0x04003713 RID: 14099
		public WindowSelectorButton NightButton;

		// Token: 0x04003714 RID: 14100
		public WindowSelectorButton LateNightButton;

		// Token: 0x04003715 RID: 14101
		public RectTransform CurrentTimeArm;

		// Token: 0x04003716 RID: 14102
		public Text CurrentTimeLabel;

		// Token: 0x04003717 RID: 14103
		private Action<EDealWindow> callback;

		// Token: 0x04003718 RID: 14104
		private WindowSelectorButton[] buttons;

		// Token: 0x04003719 RID: 14105
		private bool hintShown;
	}
}
