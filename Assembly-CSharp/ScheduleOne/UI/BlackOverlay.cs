using System;
using System.Collections;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009A0 RID: 2464
	public class BlackOverlay : Singleton<BlackOverlay>
	{
		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06004297 RID: 17047 RVA: 0x00117074 File Offset: 0x00115274
		// (set) Token: 0x06004298 RID: 17048 RVA: 0x0011707C File Offset: 0x0011527C
		public bool isShown { get; protected set; }

		// Token: 0x06004299 RID: 17049 RVA: 0x00117085 File Offset: 0x00115285
		protected override void Awake()
		{
			base.Awake();
			this.isShown = false;
			this.canvas.enabled = false;
			this.group.alpha = 0f;
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x001170B0 File Offset: 0x001152B0
		public void Open(float fadeTime = 0.5f)
		{
			this.isShown = true;
			this.canvas.enabled = true;
			if (this.fadeRoutine != null)
			{
				base.StopCoroutine(this.fadeRoutine);
			}
			this.fadeRoutine = base.StartCoroutine(this.Fade(1f, fadeTime));
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x001170FC File Offset: 0x001152FC
		public void Close(float fadeTime = 0.5f)
		{
			this.isShown = false;
			if (this.fadeRoutine != null)
			{
				base.StopCoroutine(this.fadeRoutine);
			}
			this.fadeRoutine = base.StartCoroutine(this.Fade(0f, fadeTime));
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x00117131 File Offset: 0x00115331
		private IEnumerator Fade(float endOpacity, float fadeTime)
		{
			float start = this.group.alpha;
			for (float i = 0f; i < fadeTime; i += Time.deltaTime)
			{
				this.group.alpha = Mathf.Lerp(start, endOpacity, i / fadeTime);
				yield return new WaitForEndOfFrame();
			}
			this.group.alpha = endOpacity;
			if (endOpacity == 0f)
			{
				this.canvas.enabled = false;
			}
			this.fadeRoutine = null;
			yield break;
		}

		// Token: 0x04003087 RID: 12423
		[Header("References")]
		public Canvas canvas;

		// Token: 0x04003088 RID: 12424
		public CanvasGroup group;

		// Token: 0x04003089 RID: 12425
		private Coroutine fadeRoutine;
	}
}
