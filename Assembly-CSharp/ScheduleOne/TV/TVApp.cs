using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.UI;
using UnityEngine;

namespace ScheduleOne.TV
{
	// Token: 0x020002A1 RID: 673
	public class TVApp : MonoBehaviour
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0003ED20 File Offset: 0x0003CF20
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0003ED28 File Offset: 0x0003CF28
		public bool IsOpen { get; private set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0003ED31 File Offset: 0x0003CF31
		public bool IsPaused
		{
			get
			{
				return this.PauseScreen != null && this.PauseScreen.IsPaused;
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0003ED4E File Offset: 0x0003CF4E
		protected virtual void Awake()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 3);
			this.CanvasGroup.alpha = 0f;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0003ED72 File Offset: 0x0003CF72
		private void OnDestroy()
		{
			GameInput.DeregisterExitListener(new GameInput.ExitDelegate(this.Exit));
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0003ED88 File Offset: 0x0003CF88
		public virtual void Open()
		{
			this.IsOpen = true;
			this.Canvas.gameObject.SetActive(true);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.ActiveMinPass));
			TimeManager instance2 = NetworkSingleton<TimeManager>.Instance;
			instance2.onMinutePass = (Action)Delegate.Combine(instance2.onMinutePass, new Action(this.ActiveMinPass));
			this.Lerp(1f, 1f);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0003EE0C File Offset: 0x0003D00C
		public virtual void Close()
		{
			this.IsOpen = false;
			this.Canvas.gameObject.SetActive(false);
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.ActiveMinPass));
			if (this.PreviousScreen != null)
			{
				this.Lerp(0.67f, 0f);
			}
			else
			{
				this.Lerp(1.5f, 0f);
			}
			if (this.PreviousScreen != null)
			{
				this.PreviousScreen.Open();
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000045B1 File Offset: 0x000027B1
		public virtual void Resume()
		{
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0003EEA1 File Offset: 0x0003D0A1
		private void Lerp(float endScale, float endAlpha)
		{
			if (this.lerpCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.lerpCoroutine);
			}
			this.lerpCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<Lerp>g__Lerp|23_0(endScale, endAlpha));
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void ActiveMinPass()
		{
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0003EED4 File Offset: 0x0003D0D4
		private void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			if (!this.CanClose && !this.Pauseable)
			{
				this.PreviousScreen.Open();
				return;
			}
			action.used = true;
			if (this.Pauseable && this.PauseScreen != null)
			{
				this.TryPause();
				return;
			}
			this.Close();
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0003EF39 File Offset: 0x0003D139
		protected virtual void TryPause()
		{
			this.PauseScreen.Pause();
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0003EF5C File Offset: 0x0003D15C
		[CompilerGenerated]
		private IEnumerator <Lerp>g__Lerp|23_0(float endScale, float endAlpha)
		{
			if (this.Canvas == null)
			{
				yield break;
			}
			this.Canvas.gameObject.SetActive(true);
			float startScale = this.Canvas.transform.localScale.x;
			float startAlpha = this.CanvasGroup.alpha;
			float lerpTime = Mathf.Abs(endScale - startScale) / 0.5f * 0.12f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				if (this.Canvas == null)
				{
					yield break;
				}
				this.Canvas.transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, i / lerpTime);
				this.CanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			if (this.Canvas != null)
			{
				this.Canvas.transform.localScale = Vector3.one * endScale;
				this.CanvasGroup.alpha = endAlpha;
				if (endAlpha == 0f)
				{
					this.Canvas.gameObject.SetActive(false);
				}
			}
			this.lerpCoroutine = null;
			yield break;
		}

		// Token: 0x04000EB8 RID: 3768
		public const float SCALE_MIN = 0.67f;

		// Token: 0x04000EB9 RID: 3769
		public const float SCALE_MAX = 1.5f;

		// Token: 0x04000EBA RID: 3770
		public const float LERP_TIME = 0.12f;

		// Token: 0x04000EBC RID: 3772
		[Header("Settings")]
		public bool CanClose = true;

		// Token: 0x04000EBD RID: 3773
		public string AppName;

		// Token: 0x04000EBE RID: 3774
		public Sprite Icon;

		// Token: 0x04000EBF RID: 3775
		public bool Pauseable = true;

		// Token: 0x04000EC0 RID: 3776
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04000EC1 RID: 3777
		[HideInInspector]
		public TVApp PreviousScreen;

		// Token: 0x04000EC2 RID: 3778
		public CanvasGroup CanvasGroup;

		// Token: 0x04000EC3 RID: 3779
		public TVPauseScreen PauseScreen;

		// Token: 0x04000EC4 RID: 3780
		private Coroutine lerpCoroutine;
	}
}
