using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Levelling;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A21 RID: 2593
	public class RankUpCanvas : MonoBehaviour, IPostSleepEvent
	{
		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x060045FF RID: 17919 RVA: 0x00124E90 File Offset: 0x00123090
		// (set) Token: 0x06004600 RID: 17920 RVA: 0x00124E98 File Offset: 0x00123098
		public bool IsRunning { get; private set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x00124EA1 File Offset: 0x001230A1
		// (set) Token: 0x06004602 RID: 17922 RVA: 0x00124EA9 File Offset: 0x001230A9
		public int Order { get; private set; }

		// Token: 0x06004603 RID: 17923 RVA: 0x00124EB4 File Offset: 0x001230B4
		public void Start()
		{
			this.Canvas.enabled = false;
			LevelManager instance = NetworkSingleton<LevelManager>.Instance;
			instance.onRankUp = (Action<FullRank, FullRank>)Delegate.Combine(instance.onRankUp, new Action<FullRank, FullRank>(this.RankUp));
			NetworkSingleton<TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.QueuePostSleepEvent));
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x00124F0E File Offset: 0x0012310E
		private void QueuePostSleepEvent()
		{
			if (!GameManager.IS_TUTORIAL)
			{
				Singleton<SleepCanvas>.Instance.AddPostSleepEvent(this);
			}
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x00124F24 File Offset: 0x00123124
		public void StartEvent()
		{
			RankUpCanvas.<>c__DisplayClass25_0 CS$<>8__locals1 = new RankUpCanvas.<>c__DisplayClass25_0();
			CS$<>8__locals1.<>4__this = this;
			this.IsRunning = true;
			this.OpenCloseAnim.Play("Rank up open");
			int xpGained = NetworkSingleton<DailySummary>.Instance.xpGained;
			int num = NetworkSingleton<LevelManager>.Instance.TotalXP - xpGained;
			FullRank fullRank = NetworkSingleton<LevelManager>.Instance.GetFullRank(num);
			int num2 = num - NetworkSingleton<LevelManager>.Instance.GetTotalXPForRank(fullRank);
			int i = xpGained;
			CS$<>8__locals1.progressDisplays = new List<Tuple<FullRank, int, int>>();
			FullRank fullRank2 = fullRank;
			while (i > 0)
			{
				int num3 = Mathf.Min(i, NetworkSingleton<LevelManager>.Instance.GetXPForTier(fullRank2.Rank));
				if (fullRank2 == fullRank)
				{
					num3 = Mathf.Min(num3, NetworkSingleton<LevelManager>.Instance.GetXPForTier(fullRank2.Rank) - num2);
					CS$<>8__locals1.progressDisplays.Add(new Tuple<FullRank, int, int>(fullRank2, num2, num3 + num2));
				}
				else
				{
					CS$<>8__locals1.progressDisplays.Add(new Tuple<FullRank, int, int>(fullRank2, 0, num3));
				}
				i -= num3;
				fullRank2 = fullRank2.NextRank();
			}
			this.ProgressSlider.value = (float)num2 / (float)NetworkSingleton<LevelManager>.Instance.GetXPForTier(fullRank.Rank);
			this.ProgressLabel.text = num2.ToString() + " / " + NetworkSingleton<LevelManager>.Instance.GetXPForTier(fullRank.Rank).ToString() + " XP";
			this.OldRankLabel.text = FullRank.GetString(fullRank);
			this.coroutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<StartEvent>g__Routine|0());
			this.queuedRankUps.Clear();
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x001250B4 File Offset: 0x001232B4
		public void EndEvent()
		{
			if (!this.IsRunning)
			{
				return;
			}
			this.IsRunning = false;
			if (this.coroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.coroutine);
				this.coroutine = null;
			}
			this.OpenCloseAnim.Play();
			this.OpenCloseAnim.Play("Rank up close");
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x0012510D File Offset: 0x0012330D
		public void RankUp(FullRank oldRank, FullRank newRank)
		{
			this.queuedRankUps.Add(new Tuple<FullRank, FullRank>(oldRank, newRank));
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x00125124 File Offset: 0x00123324
		private void PlayRankupAnimation(FullRank oldRank, FullRank newRank, bool playSound)
		{
			this.Canvas.enabled = true;
			this.OldRankLabel.text = FullRank.GetString(oldRank);
			this.NewRankLabel.text = FullRank.GetString(newRank);
			List<Unlockable> list = new List<Unlockable>();
			if (NetworkSingleton<LevelManager>.Instance.Unlockables.ContainsKey(newRank))
			{
				list = NetworkSingleton<LevelManager>.Instance.Unlockables[newRank];
			}
			this.UnlockedItemsContainer.gameObject.SetActive(list.Count > 0);
			for (int i = 0; i < this.UnlockedItems.Length; i++)
			{
				if (i < list.Count)
				{
					this.UnlockedItems[i].Find("Icon").GetComponent<Image>().sprite = list[i].Icon;
					this.UnlockedItems[i].GetComponentInChildren<TextMeshProUGUI>().text = list[i].Title;
					this.UnlockedItems[i].gameObject.SetActive(true);
				}
				else
				{
					this.UnlockedItems[i].gameObject.SetActive(false);
				}
			}
			this.ExtraUnlocksLabel.text = ((list.Count > this.UnlockedItems.Length) ? ("+" + (list.Count - this.UnlockedItems.Length).ToString() + " more") : "");
			this.RankUpAnim.Play();
			if (playSound)
			{
				this.SoundEffect.Play();
			}
		}

		// Token: 0x040033AF RID: 13231
		public Animation OpenCloseAnim;

		// Token: 0x040033B0 RID: 13232
		public Animation RankUpAnim;

		// Token: 0x040033B1 RID: 13233
		public TextMeshProUGUI OldRankLabel;

		// Token: 0x040033B2 RID: 13234
		public TextMeshProUGUI NewRankLabel;

		// Token: 0x040033B3 RID: 13235
		public Canvas Canvas;

		// Token: 0x040033B4 RID: 13236
		public GameObject UnlockedItemsContainer;

		// Token: 0x040033B5 RID: 13237
		public RectTransform[] UnlockedItems;

		// Token: 0x040033B6 RID: 13238
		public TextMeshProUGUI ExtraUnlocksLabel;

		// Token: 0x040033B7 RID: 13239
		public AudioSourceController SoundEffect;

		// Token: 0x040033B8 RID: 13240
		public Slider ProgressSlider;

		// Token: 0x040033B9 RID: 13241
		public TextMeshProUGUI ProgressLabel;

		// Token: 0x040033BA RID: 13242
		public AudioSourceController BlipSound;

		// Token: 0x040033BB RID: 13243
		public AudioSourceController ClickSound;

		// Token: 0x040033BC RID: 13244
		private Coroutine coroutine;

		// Token: 0x040033BD RID: 13245
		private List<Tuple<FullRank, FullRank>> queuedRankUps = new List<Tuple<FullRank, FullRank>>();
	}
}
