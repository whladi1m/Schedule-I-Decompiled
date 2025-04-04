using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.TV
{
	// Token: 0x020002A3 RID: 675
	public class TVHomeScreen : TVApp
	{
		// Token: 0x06000E20 RID: 3616 RVA: 0x0003F154 File Offset: 0x0003D354
		protected override void Awake()
		{
			base.Awake();
			TVApp[] apps = this.Apps;
			for (int i = 0; i < apps.Length; i++)
			{
				TVApp app = apps[i];
				app.PreviousScreen = this;
				app.CanvasGroup.alpha = 0f;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.AppButtonPrefab, this.AppButtonContainer);
				gameObject.transform.Find("Icon").GetComponent<Image>().sprite = app.Icon;
				gameObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = app.AppName;
				gameObject.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.AppSelected(app);
				});
				app.Close();
			}
			this.Interface.onPlayerAdded.AddListener(new UnityAction<Player>(this.PlayerChange));
			this.Interface.onPlayerRemoved.AddListener(new UnityAction<Player>(this.PlayerChange));
			this.Close();
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0003F278 File Offset: 0x0003D478
		public override void Open()
		{
			base.Open();
			this.UpdateTimeLabel();
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0003F286 File Offset: 0x0003D486
		public override void Close()
		{
			base.Close();
			if (this.skipExit)
			{
				this.skipExit = false;
				return;
			}
			this.Interface.Close();
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0003F2A9 File Offset: 0x0003D4A9
		protected override void ActiveMinPass()
		{
			base.ActiveMinPass();
			this.UpdateTimeLabel();
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0003F2B7 File Offset: 0x0003D4B7
		private void UpdateTimeLabel()
		{
			this.TimeLabel.text = TimeManager.Get12HourTime((float)NetworkSingleton<TimeManager>.Instance.CurrentTime, true);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0003F2D5 File Offset: 0x0003D4D5
		private void AppSelected(TVApp app)
		{
			this.skipExit = true;
			this.Close();
			app.Open();
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0003F2EC File Offset: 0x0003D4EC
		private void PlayerChange(Player player)
		{
			for (int i = 0; i < this.PlayerDisplays.Length; i++)
			{
				if (this.Interface.Players.Count > i)
				{
					this.PlayerDisplays[i].Find("Name").GetComponent<TextMeshProUGUI>().text = this.Interface.Players[i].PlayerName;
					this.PlayerDisplays[i].gameObject.SetActive(true);
				}
				else
				{
					this.PlayerDisplays[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x04000ECE RID: 3790
		[Header("References")]
		public TVInterface Interface;

		// Token: 0x04000ECF RID: 3791
		public TVApp[] Apps;

		// Token: 0x04000ED0 RID: 3792
		public RectTransform AppButtonContainer;

		// Token: 0x04000ED1 RID: 3793
		public RectTransform[] PlayerDisplays;

		// Token: 0x04000ED2 RID: 3794
		public TextMeshProUGUI TimeLabel;

		// Token: 0x04000ED3 RID: 3795
		[Header("Prefabs")]
		public GameObject AppButtonPrefab;

		// Token: 0x04000ED4 RID: 3796
		private bool skipExit;
	}
}
