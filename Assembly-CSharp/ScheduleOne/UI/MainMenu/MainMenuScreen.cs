using System;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI.MainMenu
{
	// Token: 0x02000B11 RID: 2833
	public class MainMenuScreen : MonoBehaviour
	{
		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06004B90 RID: 19344 RVA: 0x0013CDBF File Offset: 0x0013AFBF
		// (set) Token: 0x06004B91 RID: 19345 RVA: 0x0013CDC7 File Offset: 0x0013AFC7
		public bool IsOpen { get; protected set; }

		// Token: 0x06004B92 RID: 19346 RVA: 0x0013CDD0 File Offset: 0x0013AFD0
		protected virtual void Awake()
		{
			this.Rect = base.GetComponent<RectTransform>();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), this.ExitInputPriority);
			if (this.OpenOnStart)
			{
				this.Group.alpha = 1f;
				this.Rect.localScale = new Vector3(1f, 1f, 1f);
				base.gameObject.SetActive(true);
				this.IsOpen = true;
			}
			else
			{
				this.Group.alpha = 0f;
				this.Rect.localScale = new Vector3(1.25f, 1.25f, 1.25f);
				base.gameObject.SetActive(false);
				this.IsOpen = false;
			}
			if (this.OpenOnStart)
			{
				Singleton<MusicPlayer>.Instance.SetTrackEnabled("Main Menu", true);
			}
		}

		// Token: 0x06004B93 RID: 19347 RVA: 0x0013CEA7 File Offset: 0x0013B0A7
		private void OnDestroy()
		{
			if (Singleton<MusicPlayer>.Instance != null)
			{
				Singleton<MusicPlayer>.Instance.SetTrackEnabled("Main Menu", false);
			}
		}

		// Token: 0x06004B94 RID: 19348 RVA: 0x0013CEC6 File Offset: 0x0013B0C6
		protected virtual void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (action.exitType == ExitType.RightClick)
			{
				return;
			}
			if (this.PreviousScreen == null)
			{
				return;
			}
			if (this.IsOpen)
			{
				this.Close(true);
				action.used = true;
			}
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x0013CEFF File Offset: 0x0013B0FF
		public virtual void Open(bool closePrevious)
		{
			this.IsOpen = true;
			this.Lerp(true);
			if (closePrevious && this.PreviousScreen != null)
			{
				this.PreviousScreen.Close(false);
			}
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x0013CF2C File Offset: 0x0013B12C
		public virtual void Close(bool openPrevious)
		{
			this.IsOpen = false;
			this.Lerp(false);
			if (openPrevious && this.PreviousScreen != null)
			{
				this.PreviousScreen.Open(false);
			}
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x0013CF5C File Offset: 0x0013B15C
		private void Lerp(bool open)
		{
			MainMenuScreen.<>c__DisplayClass17_0 CS$<>8__locals1 = new MainMenuScreen.<>c__DisplayClass17_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.open = open;
			if (this.lerpRoutine != null)
			{
				base.StopCoroutine(this.lerpRoutine);
			}
			if (CS$<>8__locals1.open)
			{
				base.gameObject.SetActive(true);
			}
			if (this.Rect == null)
			{
				this.Rect = base.GetComponent<RectTransform>();
			}
			this.lerpRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Lerp>g__Routine|0());
		}

		// Token: 0x040038D4 RID: 14548
		public const float LERP_TIME = 0.075f;

		// Token: 0x040038D5 RID: 14549
		public const float LERP_SCALE = 1.25f;

		// Token: 0x040038D7 RID: 14551
		[Header("Settings")]
		public int ExitInputPriority;

		// Token: 0x040038D8 RID: 14552
		public bool OpenOnStart;

		// Token: 0x040038D9 RID: 14553
		[Header("References")]
		public MainMenuScreen PreviousScreen;

		// Token: 0x040038DA RID: 14554
		public CanvasGroup Group;

		// Token: 0x040038DB RID: 14555
		private RectTransform Rect;

		// Token: 0x040038DC RID: 14556
		private Coroutine lerpRoutine;
	}
}
