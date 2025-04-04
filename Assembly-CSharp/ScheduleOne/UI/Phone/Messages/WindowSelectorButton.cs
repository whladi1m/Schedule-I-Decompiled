using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000ABC RID: 2748
	public class WindowSelectorButton : MonoBehaviour
	{
		// Token: 0x060049E9 RID: 18921 RVA: 0x0013563F File Offset: 0x0013383F
		private void Awake()
		{
			this.HoverIndicator.gameObject.SetActive(true);
			this.HoverIndicator.localScale = Vector3.one;
			this.Button.onClick.AddListener(new UnityAction(this.Clicked));
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x0013567E File Offset: 0x0013387E
		public void SetInteractable(bool interactable)
		{
			this.Button.interactable = interactable;
			this.InactiveOverlay.SetActive(!interactable);
			if (!interactable)
			{
				this.SetHoverIndicator(false);
			}
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x001356A5 File Offset: 0x001338A5
		public void HoverStart()
		{
			if (!this.Button.interactable)
			{
				return;
			}
			this.SetHoverIndicator(true);
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x001356BC File Offset: 0x001338BC
		public void HoverEnd()
		{
			if (!this.Button.interactable)
			{
				return;
			}
			this.SetHoverIndicator(false);
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x001356D3 File Offset: 0x001338D3
		public void Clicked()
		{
			if (this.OnSelected != null)
			{
				this.OnSelected.Invoke();
			}
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x001356E8 File Offset: 0x001338E8
		public void SetHoverIndicator(bool shown)
		{
			WindowSelectorButton.<>c__DisplayClass13_0 CS$<>8__locals1 = new WindowSelectorButton.<>c__DisplayClass13_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.shown = shown;
			if (this.hoverRoutine != null)
			{
				base.StopCoroutine(this.hoverRoutine);
			}
			this.hoverRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SetHoverIndicator>g__Routine|0());
		}

		// Token: 0x0400376C RID: 14188
		public const float SELECTION_INDICATOR_SCALE = 1.1f;

		// Token: 0x0400376D RID: 14189
		public const float INDICATOR_LERP_TIME = 0.075f;

		// Token: 0x0400376E RID: 14190
		public UnityEvent OnSelected;

		// Token: 0x0400376F RID: 14191
		public EDealWindow WindowType;

		// Token: 0x04003770 RID: 14192
		[Header("References")]
		public Button Button;

		// Token: 0x04003771 RID: 14193
		public GameObject InactiveOverlay;

		// Token: 0x04003772 RID: 14194
		public RectTransform HoverIndicator;

		// Token: 0x04003773 RID: 14195
		private Coroutine hoverRoutine;
	}
}
