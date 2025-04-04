using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.UI
{
	// Token: 0x02000A1A RID: 2586
	public class QuestEntryHUDUI : MonoBehaviour
	{
		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060045C4 RID: 17860 RVA: 0x00124692 File Offset: 0x00122892
		// (set) Token: 0x060045C5 RID: 17861 RVA: 0x0012469A File Offset: 0x0012289A
		public QuestEntry QuestEntry { get; private set; }

		// Token: 0x060045C6 RID: 17862 RVA: 0x001246A4 File Offset: 0x001228A4
		public void Initialize(QuestEntry entry)
		{
			this.QuestEntry = entry;
			this.MainLabel.text = entry.Title;
			QuestHUDUI hudUI = this.QuestEntry.ParentQuest.hudUI;
			hudUI.onUpdateUI = (Action)Delegate.Combine(hudUI.onUpdateUI, new Action(this.UpdateUI));
			if (this.QuestEntry.State == EQuestState.Active)
			{
				this.FadeIn();
			}
			else
			{
				this.QuestEntry.onStart.AddListener(new UnityAction(this.FadeIn));
			}
			this.QuestEntry.onEnd.AddListener(new UnityAction(this.EntryEnded));
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x0012474C File Offset: 0x0012294C
		public void Destroy()
		{
			QuestHUDUI hudUI = this.QuestEntry.ParentQuest.hudUI;
			hudUI.onUpdateUI = (Action)Delegate.Remove(hudUI.onUpdateUI, new Action(this.UpdateUI));
			this.QuestEntry.onStart.RemoveListener(new UnityAction(this.FadeIn));
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x001247B4 File Offset: 0x001229B4
		public virtual void UpdateUI()
		{
			if (this.QuestEntry.State != EQuestState.Active)
			{
				if (!this.Animation.isPlaying)
				{
					base.gameObject.SetActive(false);
				}
				return;
			}
			if (this.QuestEntry.ParentQuest.ActiveEntryCount > 1)
			{
				this.MainLabel.text = "• " + this.QuestEntry.Title;
			}
			else
			{
				this.MainLabel.text = this.QuestEntry.Title;
			}
			base.gameObject.SetActive(true);
			this.MainLabel.ForceMeshUpdate(false, false);
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0012484D File Offset: 0x00122A4D
		private void FadeIn()
		{
			this.QuestEntry.UpdateEntryUI();
			base.transform.SetAsLastSibling();
			this.Animation.Play("Quest entry enter");
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x00124876 File Offset: 0x00122A76
		private void EntryEnded()
		{
			if (this.QuestEntry.State == EQuestState.Completed)
			{
				this.Complete();
				return;
			}
			this.FadeOut();
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x00124893 File Offset: 0x00122A93
		private void FadeOut()
		{
			this.Animation.Play("Quest entry exit");
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<FadeOut>g__Routine|11_0());
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x001248B7 File Offset: 0x00122AB7
		private void Complete()
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.Animation.Play("Quest entry complete");
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<Complete>g__Routine|12_0());
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x001248F5 File Offset: 0x00122AF5
		[CompilerGenerated]
		private IEnumerator <FadeOut>g__Routine|11_0()
		{
			yield return new WaitForSeconds(this.Animation.GetClip("Quest entry exit").length);
			base.gameObject.SetActive(false);
			this.QuestEntry.UpdateEntryUI();
			yield break;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x00124904 File Offset: 0x00122B04
		[CompilerGenerated]
		private IEnumerator <Complete>g__Routine|12_0()
		{
			yield return new WaitForSeconds(3f);
			this.FadeOut();
			yield break;
		}

		// Token: 0x04003394 RID: 13204
		[Header("References")]
		public TextMeshProUGUI MainLabel;

		// Token: 0x04003395 RID: 13205
		public Animation Animation;
	}
}
