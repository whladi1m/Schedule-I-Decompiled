using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Networking;
using ScheduleOne.Persistence;
using ScheduleOne.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009E5 RID: 2533
	public class LoadingScreen : PersistentSingleton<LoadingScreen>
	{
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06004466 RID: 17510 RVA: 0x0011E94C File Offset: 0x0011CB4C
		// (set) Token: 0x06004467 RID: 17511 RVA: 0x0011E954 File Offset: 0x0011CB54
		public bool IsOpen { get; protected set; }

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06004468 RID: 17512 RVA: 0x0011E95D File Offset: 0x0011CB5D
		public Sprite[] ContextualBackgroundImages
		{
			get
			{
				if (!this.isLoadingTutorial)
				{
					return this.BackgroundImages;
				}
				return this.TutorialBackgroundImages;
			}
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0011E974 File Offset: 0x0011CB74
		protected override void Awake()
		{
			base.Awake();
			if (Singleton<LoadingScreen>.Instance == null || Singleton<LoadingScreen>.Instance != this)
			{
				return;
			}
			this.loadingMessages = this.LoadingMessagesDatabase.Strings;
			this.currentBackgroundImageIndex = UnityEngine.Random.Range(0, this.ContextualBackgroundImages.Length);
			for (int i = 0; i < this.ContextualBackgroundImages.Length; i++)
			{
				int num = UnityEngine.Random.Range(0, this.ContextualBackgroundImages.Length);
				Sprite sprite = this.ContextualBackgroundImages[i];
				this.ContextualBackgroundImages[i] = this.ContextualBackgroundImages[num];
				this.ContextualBackgroundImages[num] = sprite;
			}
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Group.alpha = 0f;
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x0011EA2E File Offset: 0x0011CC2E
		protected void Update()
		{
			if (this.IsOpen)
			{
				this.LoadStatusLabel.text = Singleton<LoadManager>.Instance.GetLoadStatusText();
			}
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x0011EA50 File Offset: 0x0011CC50
		public void Open(bool loadingTutorial = false)
		{
			if (this.IsOpen)
			{
				return;
			}
			this.isLoadingTutorial = loadingTutorial;
			this.TutorialContainer.gameObject.SetActive(loadingTutorial);
			if (loadingTutorial && Singleton<Lobby>.Instance.IsInLobby && Singleton<Lobby>.Instance.PlayerCount > 1)
			{
				this.CoopTutorialHint.gameObject.SetActive(true);
			}
			else
			{
				this.CoopTutorialHint.gameObject.SetActive(false);
			}
			this.LoadingMessageLabel.text = this.loadingMessages[UnityEngine.Random.Range(0, this.loadingMessages.Length)];
			this.IsOpen = true;
			Singleton<MusicPlayer>.Instance.SetTrackEnabled("Loading Screen", true);
			this.Fade(1f);
			this.AnimateBackground();
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0011EB06 File Offset: 0x0011CD06
		public void Close()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			Singleton<MusicPlayer>.Instance.SetTrackEnabled("Loading Screen", false);
			Singleton<MusicPlayer>.Instance.StopTrack("Loading Screen");
			this.Fade(0f);
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0011EB42 File Offset: 0x0011CD42
		private void AnimateBackground()
		{
			if (this.animateBackgroundRoutine != null)
			{
				base.StopCoroutine(this.animateBackgroundRoutine);
			}
			if (this.scaleBackgroundRoutine != null)
			{
				base.StopCoroutine(this.scaleBackgroundRoutine);
			}
			this.animateBackgroundRoutine = base.StartCoroutine(this.<AnimateBackground>g__Routine|30_0());
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0011EB80 File Offset: 0x0011CD80
		private void Fade(float endAlpha)
		{
			LoadingScreen.<>c__DisplayClass31_0 CS$<>8__locals1 = new LoadingScreen.<>c__DisplayClass31_0();
			CS$<>8__locals1.endAlpha = endAlpha;
			CS$<>8__locals1.<>4__this = this;
			if (this.fadeRoutine != null)
			{
				base.StopCoroutine(this.fadeRoutine);
			}
			this.fadeRoutine = base.StartCoroutine(CS$<>8__locals1.<Fade>g__Routine|0());
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0011EBCF File Offset: 0x0011CDCF
		[CompilerGenerated]
		private IEnumerator <AnimateBackground>g__Routine|30_0()
		{
			LoadingScreen.<>c__DisplayClass30_0 CS$<>8__locals1 = new LoadingScreen.<>c__DisplayClass30_0();
			this.currentBackgroundImageIndex++;
			this.BackgroundImage1.color = new Color(1f, 1f, 1f, 0f);
			this.BackgroundImage2.color = new Color(1f, 1f, 1f, 0f);
			Image prevImage = null;
			CS$<>8__locals1.nextImage = this.BackgroundImage1;
			while (this.IsOpen || this.Group.alpha > 0f)
			{
				this.currentBackgroundImageIndex %= this.ContextualBackgroundImages.Length;
				CS$<>8__locals1.nextImage.sprite = this.ContextualBackgroundImages[this.currentBackgroundImageIndex];
				this.scaleBackgroundRoutine = base.StartCoroutine(CS$<>8__locals1.<AnimateBackground>g__ScaleRoutine|1(CS$<>8__locals1.nextImage.transform, 10f));
				for (float i = 0f; i < 1f; i += Time.deltaTime)
				{
					if (prevImage != null)
					{
						prevImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, i / 1f));
					}
					CS$<>8__locals1.nextImage.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, i / 1f));
					yield return new WaitForEndOfFrame();
					if (prevImage != null)
					{
						prevImage.color = new Color(1f, 1f, 1f, 0f);
					}
					CS$<>8__locals1.nextImage.color = new Color(1f, 1f, 1f, 1f);
				}
				yield return new WaitForSeconds(8f);
				prevImage = CS$<>8__locals1.nextImage;
				CS$<>8__locals1.nextImage = ((CS$<>8__locals1.nextImage == this.BackgroundImage1) ? this.BackgroundImage2 : this.BackgroundImage1);
				this.currentBackgroundImageIndex++;
			}
			yield break;
		}

		// Token: 0x04003234 RID: 12852
		public const float FADE_TIME = 0.25f;

		// Token: 0x04003235 RID: 12853
		public const float BACKGROUND_IMAGE_TIME = 8f;

		// Token: 0x04003236 RID: 12854
		public const float BACKGROUND_IMAGE_FADE_TIME = 1f;

		// Token: 0x04003238 RID: 12856
		public StringDatabase LoadingMessagesDatabase;

		// Token: 0x04003239 RID: 12857
		public Sprite[] BackgroundImages;

		// Token: 0x0400323A RID: 12858
		public Sprite[] TutorialBackgroundImages;

		// Token: 0x0400323B RID: 12859
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400323C RID: 12860
		public CanvasGroup Group;

		// Token: 0x0400323D RID: 12861
		public TextMeshProUGUI LoadStatusLabel;

		// Token: 0x0400323E RID: 12862
		public TextMeshProUGUI LoadingMessageLabel;

		// Token: 0x0400323F RID: 12863
		public Image BackgroundImage1;

		// Token: 0x04003240 RID: 12864
		public Image BackgroundImage2;

		// Token: 0x04003241 RID: 12865
		public RectTransform TutorialContainer;

		// Token: 0x04003242 RID: 12866
		public RectTransform CoopTutorialHint;

		// Token: 0x04003243 RID: 12867
		private string[] loadingMessages;

		// Token: 0x04003244 RID: 12868
		private int currentBackgroundImageIndex;

		// Token: 0x04003245 RID: 12869
		private Coroutine fadeRoutine;

		// Token: 0x04003246 RID: 12870
		private Coroutine animateBackgroundRoutine;

		// Token: 0x04003247 RID: 12871
		private Coroutine scaleBackgroundRoutine;

		// Token: 0x04003248 RID: 12872
		private bool isLoadingTutorial;
	}
}
