using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009A6 RID: 2470
	[RequireComponent(typeof(Button))]
	[RequireComponent(typeof(EventTrigger))]
	public class ButtonScaler : MonoBehaviour
	{
		// Token: 0x060042C0 RID: 17088 RVA: 0x00117D80 File Offset: 0x00115F80
		private void Awake()
		{
			this.button = base.GetComponent<Button>();
			EventTrigger component = base.GetComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.Hovered();
			});
			component.triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerExit;
			entry2.callback.AddListener(delegate(BaseEventData data)
			{
				this.HoverEnd();
			});
			component.triggers.Add(entry2);
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x00117DFE File Offset: 0x00115FFE
		private void Hovered()
		{
			if (!this.button.interactable)
			{
				return;
			}
			this.SetScale(this.HoverScale);
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x00117E1A File Offset: 0x0011601A
		private void HoverEnd()
		{
			if (!this.button.interactable)
			{
				return;
			}
			this.SetScale(1f);
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x00117E38 File Offset: 0x00116038
		private void SetScale(float endScale)
		{
			ButtonScaler.<>c__DisplayClass8_0 CS$<>8__locals1 = new ButtonScaler.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.endScale = endScale;
			if (this.scaleCoroutine != null)
			{
				base.StopCoroutine(this.scaleCoroutine);
			}
			this.scaleCoroutine = base.StartCoroutine(CS$<>8__locals1.<SetScale>g__Routine|0());
		}

		// Token: 0x040030B7 RID: 12471
		public RectTransform ScaleTarget;

		// Token: 0x040030B8 RID: 12472
		public float HoverScale = 1.1f;

		// Token: 0x040030B9 RID: 12473
		public float ScaleTime = 0.1f;

		// Token: 0x040030BA RID: 12474
		private Coroutine scaleCoroutine;

		// Token: 0x040030BB RID: 12475
		private Button button;
	}
}
