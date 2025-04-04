using System;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Quests;
using ScheduleOne.UI.Relations;
using TMPro;
using UnityEngine;

namespace ScheduleOne.UI
{
	// Token: 0x020009B6 RID: 2486
	public class DealCompletionPopup : Singleton<DealCompletionPopup>
	{
		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600433C RID: 17212 RVA: 0x00119B87 File Offset: 0x00117D87
		// (set) Token: 0x0600433D RID: 17213 RVA: 0x00119B8F File Offset: 0x00117D8F
		public bool IsPlaying { get; protected set; }

		// Token: 0x0600433E RID: 17214 RVA: 0x00119B98 File Offset: 0x00117D98
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x00119BC0 File Offset: 0x00117DC0
		public void PlayPopup(Customer customer, float satisfaction, float originalRelationshipDelta, float basePayment, List<Contract.BonusPayment> bonuses)
		{
			DealCompletionPopup.<>c__DisplayClass18_0 CS$<>8__locals1 = new DealCompletionPopup.<>c__DisplayClass18_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.customer = customer;
			CS$<>8__locals1.bonuses = bonuses;
			CS$<>8__locals1.originalRelationshipDelta = originalRelationshipDelta;
			CS$<>8__locals1.basePayment = basePayment;
			CS$<>8__locals1.satisfaction = satisfaction;
			this.IsPlaying = true;
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
			}
			this.routine = base.StartCoroutine(CS$<>8__locals1.<PlayPopup>g__Routine|0());
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x00119C2C File Offset: 0x00117E2C
		private void SetRelationshipLabel(float delta)
		{
			ERelationshipCategory category = RelationshipCategory.GetCategory(delta);
			this.RelationshipLabel.text = category.ToString();
			this.RelationshipLabel.color = RelationshipCategory.GetColor(category);
		}

		// Token: 0x0400310D RID: 12557
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400310E RID: 12558
		public RectTransform Container;

		// Token: 0x0400310F RID: 12559
		public CanvasGroup Group;

		// Token: 0x04003110 RID: 12560
		public Animation Anim;

		// Token: 0x04003111 RID: 12561
		public TextMeshProUGUI Title;

		// Token: 0x04003112 RID: 12562
		public TextMeshProUGUI PaymentLabel;

		// Token: 0x04003113 RID: 12563
		public TextMeshProUGUI SatisfactionValueLabel;

		// Token: 0x04003114 RID: 12564
		public RelationCircle RelationCircle;

		// Token: 0x04003115 RID: 12565
		public TextMeshProUGUI RelationshipLabel;

		// Token: 0x04003116 RID: 12566
		public Gradient SatisfactionGradient;

		// Token: 0x04003117 RID: 12567
		public AudioSourceController SoundEffect;

		// Token: 0x04003118 RID: 12568
		public TextMeshProUGUI[] BonusLabels;

		// Token: 0x04003119 RID: 12569
		private Coroutine routine;
	}
}
