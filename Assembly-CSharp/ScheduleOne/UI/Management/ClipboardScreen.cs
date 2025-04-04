using System;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.UI.Management
{
	// Token: 0x02000ACC RID: 2764
	public class ClipboardScreen : MonoBehaviour
	{
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06004A2F RID: 18991 RVA: 0x00136FFF File Offset: 0x001351FF
		// (set) Token: 0x06004A30 RID: 18992 RVA: 0x00137007 File Offset: 0x00135207
		public bool IsOpen { get; protected set; }

		// Token: 0x06004A31 RID: 18993 RVA: 0x00137010 File Offset: 0x00135210
		protected virtual void Start()
		{
			if (this.OpenOnStart)
			{
				this.IsOpen = true;
				this.Container.anchoredPosition = new Vector2(0f, this.Container.anchoredPosition.y);
			}
			else
			{
				this.IsOpen = false;
				this.Container.anchoredPosition = new Vector2(this.ClosedOffset, this.Container.anchoredPosition.y);
				this.Container.gameObject.SetActive(false);
			}
			if (this.UseExitListener)
			{
				GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), this.ExitActionPriority);
			}
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x001370B0 File Offset: 0x001352B0
		private void Exit(ExitAction exitAction)
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (exitAction.used)
			{
				return;
			}
			exitAction.used = true;
			this.Close();
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x001370D1 File Offset: 0x001352D1
		public virtual void Open()
		{
			this.Container.gameObject.SetActive(true);
			this.IsOpen = true;
			this.Lerp(true, null);
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x001370F3 File Offset: 0x001352F3
		public virtual void Close()
		{
			this.IsOpen = false;
			this.Lerp(false, delegate
			{
				this.Container.gameObject.SetActive(false);
			});
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x00137110 File Offset: 0x00135310
		private void Lerp(bool open, Action callback)
		{
			ClipboardScreen.<>c__DisplayClass14_0 CS$<>8__locals1 = new ClipboardScreen.<>c__DisplayClass14_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.open = open;
			CS$<>8__locals1.callback = callback;
			if (this.lerpRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.lerpRoutine);
			}
			this.lerpRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<Lerp>g__Routine|0());
		}

		// Token: 0x040037C7 RID: 14279
		[Header("References")]
		public RectTransform Container;

		// Token: 0x040037C8 RID: 14280
		[Header("Settings")]
		public float ClosedOffset = 420f;

		// Token: 0x040037C9 RID: 14281
		public bool OpenOnStart;

		// Token: 0x040037CA RID: 14282
		public bool UseExitListener = true;

		// Token: 0x040037CB RID: 14283
		public int ExitActionPriority = 10;

		// Token: 0x040037CC RID: 14284
		private Coroutine lerpRoutine;
	}
}
