using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI
{
	// Token: 0x02000A1D RID: 2589
	public class QuestHUDUI : MonoBehaviour
	{
		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060045DC RID: 17884 RVA: 0x00124A0B File Offset: 0x00122C0B
		// (set) Token: 0x060045DD RID: 17885 RVA: 0x00124A13 File Offset: 0x00122C13
		public Quest Quest { get; private set; }

		// Token: 0x060045DE RID: 17886 RVA: 0x00124A1C File Offset: 0x00122C1C
		public void Initialize(Quest quest)
		{
			this.Quest = quest;
			Quest quest2 = this.Quest;
			quest2.onSubtitleChanged = (Action)Delegate.Combine(quest2.onSubtitleChanged, new Action(this.UpdateMainLabel));
			UnityEngine.Object.Instantiate<RectTransform>(this.Quest.IconPrefab, base.transform.Find("Title/IconContainer")).GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 20f);
			this.UpdateUI();
			if (this.Quest.QuestState == EQuestState.Active)
			{
				this.FadeIn();
			}
			else
			{
				this.Quest.onQuestBegin.AddListener(new UnityAction(this.FadeIn));
				base.gameObject.SetActive(false);
			}
			this.Quest.onQuestEnd.AddListener(new UnityAction<EQuestState>(this.EntryEnded));
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x00124AF0 File Offset: 0x00122CF0
		public void Destroy()
		{
			Quest quest = this.Quest;
			quest.onSubtitleChanged = (Action)Delegate.Remove(quest.onSubtitleChanged, new Action(this.UpdateMainLabel));
			QuestEntryHUDUI[] componentsInChildren = base.GetComponentsInChildren<QuestEntryHUDUI>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Destroy();
			}
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x00124B44 File Offset: 0x00122D44
		public void UpdateUI()
		{
			this.UpdateMainLabel();
			this.UpdateExpiry();
			if (this.onUpdateUI != null)
			{
				this.onUpdateUI();
			}
			this.hudUILayout.CalculateLayoutInputVertical();
			this.hudUILayout.SetLayoutVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.hudUILayout.transform);
			this.hudUILayout.enabled = false;
			this.hudUILayout.enabled = true;
			this.UpdateShade();
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<UpdateUI>g__DelayFix|13_0());
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x00124BCA File Offset: 0x00122DCA
		public void UpdateMainLabel()
		{
			this.MainLabel.text = this.Quest.GetQuestTitle() + this.Quest.Subtitle;
			this.MainLabel.ForceMeshUpdate(false, false);
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x000045B1 File Offset: 0x000027B1
		public void UpdateExpiry()
		{
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x00124BFF File Offset: 0x00122DFF
		public void UpdateShade()
		{
			this.Shade.sizeDelta = new Vector2(550f, this.hudUILayout.preferredHeight + 120f);
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x00124C27 File Offset: 0x00122E27
		public void BopIcon()
		{
			base.transform.Find("Title/IconContainer").GetComponent<Animation>().Play();
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x00124C44 File Offset: 0x00122E44
		private void FadeIn()
		{
			if (this.Quest.IsTracked)
			{
				base.gameObject.SetActive(true);
			}
			this.Animation.Play("Quest enter");
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x00124C70 File Offset: 0x00122E70
		private void EntryEnded(EQuestState endState)
		{
			if (endState == EQuestState.Completed)
			{
				this.Complete();
				return;
			}
			this.FadeOut();
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x00124C83 File Offset: 0x00122E83
		private void FadeOut()
		{
			this.Animation.Play("Quest exit");
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<FadeOut>g__Routine|20_0());
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x00124CA7 File Offset: 0x00122EA7
		private void Complete()
		{
			this.Animation.Play("Quest complete");
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Complete>g__Routine|21_0());
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x00124CDE File Offset: 0x00122EDE
		[CompilerGenerated]
		private IEnumerator <UpdateUI>g__DelayFix|13_0()
		{
			yield return new WaitForEndOfFrame();
			this.hudUILayout.CalculateLayoutInputVertical();
			this.hudUILayout.SetLayoutVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.hudUILayout.transform);
			this.hudUILayout.enabled = false;
			this.hudUILayout.enabled = true;
			this.UpdateShade();
			yield break;
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x00124CED File Offset: 0x00122EED
		[CompilerGenerated]
		private IEnumerator <FadeOut>g__Routine|20_0()
		{
			yield return new WaitForSeconds(0.5f);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x00124CFC File Offset: 0x00122EFC
		[CompilerGenerated]
		private IEnumerator <Complete>g__Routine|21_0()
		{
			yield return new WaitForSeconds(3f);
			this.FadeOut();
			yield break;
		}

		// Token: 0x0400339C RID: 13212
		public string CriticalTimeColor = "FF7A7A";

		// Token: 0x0400339E RID: 13214
		[Header("References")]
		public RectTransform EntryContainer;

		// Token: 0x0400339F RID: 13215
		public TextMeshProUGUI MainLabel;

		// Token: 0x040033A0 RID: 13216
		public VerticalLayoutGroup hudUILayout;

		// Token: 0x040033A1 RID: 13217
		public Animation Animation;

		// Token: 0x040033A2 RID: 13218
		public RectTransform Shade;

		// Token: 0x040033A3 RID: 13219
		public Action onUpdateUI;
	}
}
