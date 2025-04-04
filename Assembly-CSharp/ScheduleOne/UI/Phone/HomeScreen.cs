using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A93 RID: 2707
	public class HomeScreen : PlayerSingleton<HomeScreen>
	{
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x060048DE RID: 18654 RVA: 0x00130FE6 File Offset: 0x0012F1E6
		// (set) Token: 0x060048DF RID: 18655 RVA: 0x00130FEE File Offset: 0x0012F1EE
		public bool isOpen { get; protected set; } = true;

		// Token: 0x060048E0 RID: 18656 RVA: 0x00130FF7 File Offset: 0x0012F1F7
		protected override void Start()
		{
			base.Start();
			this.SetIsOpen(true);
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x00131008 File Offset: 0x0012F208
		public override void OnStartClient(bool IsOwner)
		{
			base.OnStartClient(IsOwner);
			if (!IsOwner)
			{
				return;
			}
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
			Phone instance2 = PlayerSingleton<Phone>.Instance;
			instance2.onPhoneOpened = (Action)Delegate.Combine(instance2.onPhoneOpened, new Action(this.PhoneOpened));
			Phone instance3 = PlayerSingleton<Phone>.Instance;
			instance3.onPhoneClosed = (Action)Delegate.Combine(instance3.onPhoneClosed, new Action(this.PhoneClosed));
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x00131093 File Offset: 0x0012F293
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x001310C9 File Offset: 0x0012F2C9
		protected void PhoneOpened()
		{
			if (this.isOpen)
			{
				this.SetCanvasActive(true);
			}
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x001310DA File Offset: 0x0012F2DA
		protected void PhoneClosed()
		{
			this.delayedSetOpenRoutine = base.StartCoroutine(this.DelayedSetCanvasActive(false, 0.25f));
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x001310F4 File Offset: 0x0012F2F4
		private IEnumerator DelayedSetCanvasActive(bool active, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.delayedSetOpenRoutine = null;
			this.SetCanvasActive(active);
			yield break;
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x00131111 File Offset: 0x0012F311
		public void SetIsOpen(bool o)
		{
			this.isOpen = o;
			this.SetCanvasActive(o);
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x00131121 File Offset: 0x0012F321
		public void SetCanvasActive(bool a)
		{
			if (this.delayedSetOpenRoutine != null)
			{
				base.StopCoroutine(this.delayedSetOpenRoutine);
			}
			this.canvas.enabled = a;
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x00131144 File Offset: 0x0012F344
		protected virtual void Update()
		{
			if (PlayerSingleton<Phone>.Instance.IsOpen && this.isOpen)
			{
				int num = -1;
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					num = 0;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					num = 1;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					num = 2;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					num = 3;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha5))
				{
					num = 4;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha6))
				{
					num = 5;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha7))
				{
					num = 6;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha8))
				{
					num = 7;
				}
				else if (Input.GetKeyDown(KeyCode.Alpha9))
				{
					num = 8;
				}
				if (num != -1 && this.appIcons.Count > num)
				{
					this.appIcons[num].onClick.Invoke();
				}
			}
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x00131208 File Offset: 0x0012F408
		protected virtual void MinPass()
		{
			if (NetworkSingleton<GameManager>.Instance.IsTutorial)
			{
				int num = TimeManager.Get24HourTimeFromMinSum(Mathf.RoundToInt(Mathf.Round((float)NetworkSingleton<TimeManager>.Instance.DailyMinTotal / 60f) * 60f));
				this.timeText.text = TimeManager.Get12HourTime((float)num, true) + " " + NetworkSingleton<TimeManager>.Instance.CurrentDay.ToString();
				return;
			}
			this.timeText.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true) + " " + NetworkSingleton<TimeManager>.Instance.CurrentDay.ToString();
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x001312BC File Offset: 0x0012F4BC
		public Button GenerateAppIcon<T>(App<T> prog) where T : PlayerSingleton<T>
		{
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.appIconPrefab, this.appIconContainer).GetComponent<RectTransform>();
			component.Find("Mask/Image").GetComponent<Image>().sprite = prog.AppIcon;
			component.Find("Label").GetComponent<Text>().text = prog.IconLabel;
			this.appIcons.Add(component.GetComponent<Button>());
			return component.GetComponent<Button>();
		}

		// Token: 0x04003643 RID: 13891
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x04003644 RID: 13892
		[SerializeField]
		protected Text timeText;

		// Token: 0x04003645 RID: 13893
		[SerializeField]
		protected RectTransform appIconContainer;

		// Token: 0x04003646 RID: 13894
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject appIconPrefab;

		// Token: 0x04003647 RID: 13895
		protected List<Button> appIcons = new List<Button>();

		// Token: 0x04003648 RID: 13896
		private Coroutine delayedSetOpenRoutine;
	}
}
