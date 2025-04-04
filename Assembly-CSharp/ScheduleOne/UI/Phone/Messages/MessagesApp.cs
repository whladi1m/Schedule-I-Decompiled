using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Messaging;
using ScheduleOne.Persistence;
using ScheduleOne.UI.Tooltips;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000AB5 RID: 2741
	public class MessagesApp : App<MessagesApp>
	{
		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x00134E12 File Offset: 0x00133012
		// (set) Token: 0x060049C7 RID: 18887 RVA: 0x00134E1A File Offset: 0x0013301A
		public MSGConversation currentConversation { get; private set; }

		// Token: 0x060049C8 RID: 18888 RVA: 0x00134E24 File Offset: 0x00133024
		protected override void Start()
		{
			base.Start();
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
			Singleton<LoadManager>.Instance.onPreSceneChange.RemoveListener(new UnityAction(this.Clean));
			Singleton<LoadManager>.Instance.onPreSceneChange.AddListener(new UnityAction(this.Clean));
			this.dialoguePage.gameObject.SetActive(false);
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x00134EB4 File Offset: 0x001330B4
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x00134EBC File Offset: 0x001330BC
		private void Loaded()
		{
			MessagesApp.ActiveConversations = (from x in MessagesApp.ActiveConversations
			orderby x.index
			select x).ToList<MSGConversation>();
			this.RepositionEntries();
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x00134EF7 File Offset: 0x001330F7
		private void Clean()
		{
			MessagesApp.Conversations.Clear();
			MessagesApp.ActiveConversations.Clear();
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x00134F10 File Offset: 0x00133110
		public void CreateConversationUI(MSGConversation c, out RectTransform entry, out RectTransform container)
		{
			entry = UnityEngine.Object.Instantiate<GameObject>(this.conversationEntryPrefab, this.conversationEntryContainer).GetComponent<RectTransform>();
			entry.Find("Name").GetComponent<Text>().text = (c.IsSenderKnown ? c.contactName : "Unknown");
			entry.Find("IconMask/Icon").GetComponent<Image>().sprite = (c.IsSenderKnown ? c.sender.MugshotSprite : this.BlankAvatarSprite);
			entry.SetAsLastSibling();
			if (c.Categories != null && c.Categories.Count > 0)
			{
				MessagesApp.CategoryInfo categoryInfo = this.GetCategoryInfo(c.Categories[0]);
				RectTransform component = entry.Find("Category").GetComponent<RectTransform>();
				Text component2 = component.Find("Label").GetComponent<Text>();
				component2.text = categoryInfo.Name[0].ToString();
				LayoutRebuilder.ForceRebuildLayoutImmediate(component2.rectTransform);
				component.GetComponent<Image>().color = categoryInfo.Color;
				component.anchoredPosition = new Vector2(225f + entry.Find("Name").GetComponent<Text>().preferredWidth, component.anchoredPosition.y);
				component.gameObject.SetActive(true);
			}
			else
			{
				entry.Find("Category").gameObject.SetActive(false);
			}
			container = UnityEngine.Object.Instantiate<GameObject>(this.conversationContainerPrefab, this.conversationContainer).GetComponent<RectTransform>();
			this.RepositionEntries();
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x00135094 File Offset: 0x00133294
		public void RepositionEntries()
		{
			for (int i = 0; i < MessagesApp.ActiveConversations.Count; i++)
			{
				MessagesApp.ActiveConversations[i].RepositionEntry();
			}
			for (int j = 0; j < MessagesApp.ActiveConversations.Count; j++)
			{
				MessagesApp.ActiveConversations[j].RepositionEntry();
			}
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x001350EB File Offset: 0x001332EB
		public void ReturnButtonClicked()
		{
			if (this.currentConversation != null)
			{
				this.currentConversation.SetOpen(false);
			}
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x00135101 File Offset: 0x00133301
		public void RefreshNotifications()
		{
			base.SetNotificationCount(this.unreadConversations.Count);
			Singleton<HUD>.Instance.UnreadMessagesPrompt.gameObject.SetActive(this.unreadConversations.Count > 0);
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x00135136 File Offset: 0x00133336
		public override void Exit(ExitAction exit)
		{
			if (!base.isOpen || exit.used)
			{
				base.Exit(exit);
				return;
			}
			if (this.currentConversation != null)
			{
				this.currentConversation.SetOpen(false);
				exit.used = true;
			}
			base.Exit(exit);
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x00135174 File Offset: 0x00133374
		public void SetCurrentConversation(MSGConversation conversation)
		{
			if (conversation == this.currentConversation)
			{
				return;
			}
			MSGConversation currentConversation = this.currentConversation;
			this.currentConversation = conversation;
			if (currentConversation != null)
			{
				currentConversation.SetOpen(false);
			}
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x001351A4 File Offset: 0x001333A4
		public MessagesApp.CategoryInfo GetCategoryInfo(EConversationCategory category)
		{
			return this.categoryInfos.Find((MessagesApp.CategoryInfo x) => x.Category == category);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x001351D8 File Offset: 0x001333D8
		public void FilterByCategory(int category)
		{
			for (int i = 0; i < this.CategoryButtons.Length; i++)
			{
				this.CategoryButtons[i].interactable = true;
			}
			for (int j = 0; j < MessagesApp.ActiveConversations.Count; j++)
			{
				MessagesApp.ActiveConversations[j].entry.gameObject.SetActive(MessagesApp.ActiveConversations[j].Categories.Contains((EConversationCategory)category));
			}
			this.ClearFilterButton.gameObject.SetActive(true);
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x0013525C File Offset: 0x0013345C
		public void ClearFilter()
		{
			for (int i = 0; i < MessagesApp.ActiveConversations.Count; i++)
			{
				MessagesApp.ActiveConversations[i].entry.gameObject.SetActive(true);
			}
			for (int j = 0; j < this.CategoryButtons.Length; j++)
			{
				this.CategoryButtons[j].interactable = true;
			}
			this.ClearFilterButton.gameObject.SetActive(false);
		}

		// Token: 0x04003738 RID: 14136
		public static List<MSGConversation> Conversations = new List<MSGConversation>();

		// Token: 0x04003739 RID: 14137
		public static List<MSGConversation> ActiveConversations = new List<MSGConversation>();

		// Token: 0x0400373A RID: 14138
		public List<MessagesApp.CategoryInfo> categoryInfos;

		// Token: 0x0400373B RID: 14139
		[Header("References")]
		[SerializeField]
		protected RectTransform conversationEntryContainer;

		// Token: 0x0400373C RID: 14140
		[SerializeField]
		protected RectTransform conversationContainer;

		// Token: 0x0400373D RID: 14141
		public GameObject homePage;

		// Token: 0x0400373E RID: 14142
		public GameObject dialoguePage;

		// Token: 0x0400373F RID: 14143
		public Text dialoguePageNameText;

		// Token: 0x04003740 RID: 14144
		public RectTransform relationshipContainer;

		// Token: 0x04003741 RID: 14145
		public Scrollbar relationshipScrollbar;

		// Token: 0x04003742 RID: 14146
		public Tooltip relationshipTooltip;

		// Token: 0x04003743 RID: 14147
		public RectTransform standardsContainer;

		// Token: 0x04003744 RID: 14148
		public Image standardsStar;

		// Token: 0x04003745 RID: 14149
		public Tooltip standardsTooltip;

		// Token: 0x04003746 RID: 14150
		public RectTransform iconContainerRect;

		// Token: 0x04003747 RID: 14151
		public Image iconImage;

		// Token: 0x04003748 RID: 14152
		public Sprite BlankAvatarSprite;

		// Token: 0x04003749 RID: 14153
		public DealWindowSelector DealWindowSelector;

		// Token: 0x0400374A RID: 14154
		public PhoneShopInterface PhoneShopInterface;

		// Token: 0x0400374B RID: 14155
		public CounterofferInterface CounterofferInterface;

		// Token: 0x0400374C RID: 14156
		public RectTransform ClearFilterButton;

		// Token: 0x0400374D RID: 14157
		public Button[] CategoryButtons;

		// Token: 0x0400374E RID: 14158
		public AudioSourceController MessageReceivedSound;

		// Token: 0x0400374F RID: 14159
		public AudioSourceController MessageSentSound;

		// Token: 0x04003750 RID: 14160
		[Header("Prefabs")]
		[SerializeField]
		protected GameObject conversationEntryPrefab;

		// Token: 0x04003751 RID: 14161
		[SerializeField]
		protected GameObject conversationContainerPrefab;

		// Token: 0x04003752 RID: 14162
		public GameObject messageBubblePrefab;

		// Token: 0x04003753 RID: 14163
		public List<MSGConversation> unreadConversations = new List<MSGConversation>();

		// Token: 0x02000AB6 RID: 2742
		[Serializable]
		public class CategoryInfo
		{
			// Token: 0x04003755 RID: 14165
			public EConversationCategory Category;

			// Token: 0x04003756 RID: 14166
			public string Name;

			// Token: 0x04003757 RID: 14167
			public Color Color;
		}
	}
}
