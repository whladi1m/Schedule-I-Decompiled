using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A82 RID: 2690
	public class JournalApp : App<JournalApp>
	{
		// Token: 0x0600486B RID: 18539 RVA: 0x0012F2EF File Offset: 0x0012D4EF
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x0012F2F7 File Offset: 0x0012D4F7
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.MinPass));
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x0012F326 File Offset: 0x0012D526
		public override void SetOpen(bool open)
		{
			base.SetOpen(open);
			if (!open && this.currentDetailsPanel != null)
			{
				this.currentDetailsPanelQuest.DestroyDetailDisplay();
				this.currentDetailsPanel = null;
				this.currentDetailsPanelQuest = null;
			}
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x0012F35C File Offset: 0x0012D55C
		protected override void Update()
		{
			base.Update();
			if (base.isOpen)
			{
				this.RefreshDetailsPanel();
				this.NoTasksLabel.enabled = (Quest.ActiveQuests.Count == 0);
				this.NoDetailsLabel.enabled = (this.currentDetailsPanel == null);
			}
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x0012F3AC File Offset: 0x0012D5AC
		private void RefreshDetailsPanel()
		{
			if (Quest.HoveredQuest != null)
			{
				if (this.currentDetailsPanelQuest != Quest.HoveredQuest)
				{
					if (this.currentDetailsPanel != null)
					{
						this.currentDetailsPanelQuest.DestroyDetailDisplay();
						this.currentDetailsPanel = null;
						this.currentDetailsPanelQuest = null;
					}
					this.currentDetailsPanel = Quest.HoveredQuest.CreateDetailDisplay(this.DetailsPanelContainer);
					this.currentDetailsPanelQuest = Quest.HoveredQuest;
					return;
				}
			}
			else if (this.currentDetailsPanel != null)
			{
				this.currentDetailsPanelQuest.DestroyDetailDisplay();
				this.currentDetailsPanel = null;
				this.currentDetailsPanelQuest = null;
			}
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x0012F448 File Offset: 0x0012D648
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (NetworkSingleton<TimeManager>.InstanceExists)
			{
				TimeManager instance = NetworkSingleton<TimeManager>.Instance;
				instance.onMinutePass = (Action)Delegate.Remove(instance.onMinutePass, new Action(this.MinPass));
			}
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x0012F47E File Offset: 0x0012D67E
		protected virtual void MinPass()
		{
			bool isOpen = base.isOpen;
		}

		// Token: 0x040035BF RID: 13759
		[Header("References")]
		public RectTransform EntryContainer;

		// Token: 0x040035C0 RID: 13760
		public Text NoTasksLabel;

		// Token: 0x040035C1 RID: 13761
		public Text NoDetailsLabel;

		// Token: 0x040035C2 RID: 13762
		public RectTransform DetailsPanelContainer;

		// Token: 0x040035C3 RID: 13763
		[Header("Entry prefabs")]
		public GameObject GenericEntry;

		// Token: 0x040035C4 RID: 13764
		[Header("Details panel prefabs")]
		public GameObject GenericDetailsPanel;

		// Token: 0x040035C5 RID: 13765
		[Header("Quest Entry prefab")]
		public GameObject GenericQuestEntry;

		// Token: 0x040035C6 RID: 13766
		[Header("HUD entry prefabs")]
		public QuestHUDUI QuestHUDUIPrefab;

		// Token: 0x040035C7 RID: 13767
		public QuestEntryHUDUI QuestEntryHUDUIPrefab;

		// Token: 0x040035C8 RID: 13768
		protected Quest currentDetailsPanelQuest;

		// Token: 0x040035C9 RID: 13769
		protected RectTransform currentDetailsPanel;
	}
}
