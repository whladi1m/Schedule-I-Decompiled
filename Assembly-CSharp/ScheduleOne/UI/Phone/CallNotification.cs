using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A90 RID: 2704
	public class CallNotification : Singleton<CallNotification>
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x060048CF RID: 18639 RVA: 0x00130CF0 File Offset: 0x0012EEF0
		// (set) Token: 0x060048D0 RID: 18640 RVA: 0x00130CF8 File Offset: 0x0012EEF8
		public PhoneCallData ActiveCallData { get; private set; }

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x00130D01 File Offset: 0x0012EF01
		// (set) Token: 0x060048D2 RID: 18642 RVA: 0x00130D09 File Offset: 0x0012EF09
		public bool IsOpen { get; protected set; }

		// Token: 0x060048D3 RID: 18643 RVA: 0x00130D14 File Offset: 0x0012EF14
		protected override void Awake()
		{
			base.Awake();
			this.Group.alpha = 0f;
			this.Container.anchoredPosition = new Vector2(-600f, 0f);
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x00130D64 File Offset: 0x0012EF64
		public void SetIsOpen(bool visible, CallerID caller)
		{
			CallNotification.<>c__DisplayClass14_0 CS$<>8__locals1 = new CallNotification.<>c__DisplayClass14_0();
			CS$<>8__locals1.visible = visible;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.caller = caller;
			this.IsOpen = CS$<>8__locals1.visible;
			if (this.slideRoutine != null)
			{
				base.StopCoroutine(this.slideRoutine);
			}
			this.slideRoutine = base.StartCoroutine(CS$<>8__locals1.<SetIsOpen>g__Routine|0());
		}

		// Token: 0x0400362F RID: 13871
		public const float TIME_PER_CHAR = 0.015f;

		// Token: 0x04003632 RID: 13874
		[Header("References")]
		public RectTransform Container;

		// Token: 0x04003633 RID: 13875
		public Image ProfilePicture;

		// Token: 0x04003634 RID: 13876
		public CanvasGroup Group;

		// Token: 0x04003635 RID: 13877
		private Coroutine slideRoutine;
	}
}
