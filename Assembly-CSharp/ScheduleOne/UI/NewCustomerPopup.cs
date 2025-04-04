using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x020009EC RID: 2540
	public class NewCustomerPopup : Singleton<NewCustomerPopup>
	{
		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x0600448B RID: 17547 RVA: 0x0011F1EE File Offset: 0x0011D3EE
		// (set) Token: 0x0600448C RID: 17548 RVA: 0x0011F1F6 File Offset: 0x0011D3F6
		public bool IsPlaying { get; protected set; }

		// Token: 0x0600448D RID: 17549 RVA: 0x0011F1FF File Offset: 0x0011D3FF
		protected override void Awake()
		{
			base.Awake();
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.DisableEntries();
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x0011F22C File Offset: 0x0011D42C
		public void PlayPopup(Customer customer)
		{
			this.IsPlaying = true;
			RectTransform rectTransform = null;
			int num = 0;
			for (int i = 0; i < this.Entries.Length; i++)
			{
				num++;
				if (!this.Entries[i].gameObject.activeSelf)
				{
					rectTransform = this.Entries[i];
					break;
				}
			}
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.Find("Mask/Icon").GetComponent<Image>().sprite = customer.NPC.MugshotSprite;
			rectTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = customer.NPC.FirstName + "\n" + customer.NPC.LastName;
			rectTransform.gameObject.SetActive(true);
			if (num == 1)
			{
				this.Title.text = "New Customer Unlocked!";
			}
			else
			{
				this.Title.text = "New Customers Unlocked!";
			}
			if (this.routine != null)
			{
				base.StopCoroutine(this.routine);
				this.Anim.Stop();
				this.routine = null;
			}
			this.routine = base.StartCoroutine(this.<PlayPopup>g__Routine|13_0());
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x0011F344 File Offset: 0x0011D544
		private void DisableEntries()
		{
			for (int i = 0; i < this.Entries.Length; i++)
			{
				this.Entries[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x0011F37F File Offset: 0x0011D57F
		[CompilerGenerated]
		private IEnumerator <PlayPopup>g__Routine|13_0()
		{
			yield return new WaitUntil(() => !Singleton<DealCompletionPopup>.Instance.IsPlaying);
			this.Group.alpha = 0.01f;
			this.Canvas.enabled = true;
			this.Container.gameObject.SetActive(true);
			this.SoundEffect.Play();
			this.Anim.Play();
			yield return new WaitForSeconds(0.1f);
			yield return new WaitUntil(() => this.Group.alpha == 0f);
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
			this.routine = null;
			this.IsPlaying = false;
			this.DisableEntries();
			yield break;
		}

		// Token: 0x04003269 RID: 12905
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400326A RID: 12906
		public RectTransform Container;

		// Token: 0x0400326B RID: 12907
		public CanvasGroup Group;

		// Token: 0x0400326C RID: 12908
		public Animation Anim;

		// Token: 0x0400326D RID: 12909
		public TextMeshProUGUI Title;

		// Token: 0x0400326E RID: 12910
		public RectTransform[] Entries;

		// Token: 0x0400326F RID: 12911
		public AudioSourceController SoundEffect;

		// Token: 0x04003270 RID: 12912
		private Coroutine routine;
	}
}
