using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.UI;
using ScheduleOne.UI.Phone;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Messaging
{
	// Token: 0x0200053D RID: 1341
	[Serializable]
	public class MSGConversation : ISaveable
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060020F6 RID: 8438 RVA: 0x00087A9C File Offset: 0x00085C9C
		// (set) Token: 0x060020F7 RID: 8439 RVA: 0x00087AA4 File Offset: 0x00085CA4
		public bool IsSenderKnown { get; protected set; } = true;

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x00087AAD File Offset: 0x00085CAD
		// (set) Token: 0x060020F9 RID: 8441 RVA: 0x00087AB5 File Offset: 0x00085CB5
		public int index { get; protected set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060020FA RID: 8442 RVA: 0x00087ABE File Offset: 0x00085CBE
		// (set) Token: 0x060020FB RID: 8443 RVA: 0x00087AC6 File Offset: 0x00085CC6
		public bool isOpen { get; protected set; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060020FC RID: 8444 RVA: 0x00087ACF File Offset: 0x00085CCF
		// (set) Token: 0x060020FD RID: 8445 RVA: 0x00087AD7 File Offset: 0x00085CD7
		public bool rollingOut { get; protected set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x00087AE0 File Offset: 0x00085CE0
		// (set) Token: 0x060020FF RID: 8447 RVA: 0x00087AE8 File Offset: 0x00085CE8
		public bool EntryVisible { get; protected set; } = true;

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x00087AF1 File Offset: 0x00085CF1
		public bool AreResponsesActive
		{
			get
			{
				return this.currentResponses.Count > 0;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06002101 RID: 8449 RVA: 0x00087B01 File Offset: 0x00085D01
		public string SaveFolderName
		{
			get
			{
				return "MessageConversation";
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x00087B01 File Offset: 0x00085D01
		public string SaveFileName
		{
			get
			{
				return "MessageConversation";
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06002103 RID: 8451 RVA: 0x0004691A File Offset: 0x00044B1A
		public Loader Loader
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06002104 RID: 8452 RVA: 0x00014002 File Offset: 0x00012202
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06002105 RID: 8453 RVA: 0x00087B08 File Offset: 0x00085D08
		// (set) Token: 0x06002106 RID: 8454 RVA: 0x00087B10 File Offset: 0x00085D10
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06002107 RID: 8455 RVA: 0x00087B19 File Offset: 0x00085D19
		// (set) Token: 0x06002108 RID: 8456 RVA: 0x00087B21 File Offset: 0x00085D21
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x00087B2A File Offset: 0x00085D2A
		// (set) Token: 0x0600210A RID: 8458 RVA: 0x00087B32 File Offset: 0x00085D32
		public bool HasChanged { get; set; }

		// Token: 0x0600210B RID: 8459 RVA: 0x00087B3C File Offset: 0x00085D3C
		public MSGConversation(NPC _npc, string _contactName)
		{
			this.contactName = _contactName;
			this.sender = _npc;
			MessagesApp.Conversations.Insert(0, this);
			this.index = 0;
			NetworkSingleton<MessagingManager>.Instance.Register(_npc, this);
			this.InitializeSaveable();
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00087C05 File Offset: 0x00085E05
		public void SetCategories(List<EConversationCategory> cat)
		{
			this.Categories = cat;
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00087C0E File Offset: 0x00085E0E
		public void MoveToTop()
		{
			MessagesApp.ActiveConversations.Remove(this);
			MessagesApp.ActiveConversations.Insert(0, this);
			this.index = 0;
			PlayerSingleton<MessagesApp>.Instance.RepositionEntries();
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00087C3C File Offset: 0x00085E3C
		protected void CreateUI()
		{
			if (this.uiCreated)
			{
				return;
			}
			this.uiCreated = true;
			PlayerSingleton<MessagesApp>.Instance.CreateConversationUI(this, out this.entry, out this.container);
			MessagesApp.ActiveConversations.Add(this);
			this.entryPreviewText = this.entry.Find("Preview").GetComponent<Text>();
			this.unreadDot = this.entry.Find("UnreadDot").GetComponent<RectTransform>();
			this.slider = this.entry.Find("Slider").GetComponent<Slider>();
			this.sliderFill = this.slider.fillRect.GetComponent<Image>();
			this.entry.Find("Button").GetComponent<Button>().onClick.AddListener(new UnityAction(this.EntryClicked));
			Button component = this.entry.Find("Hide").GetComponent<Button>();
			if (this.sender.ConversationCanBeHidden)
			{
				component.gameObject.SetActive(true);
				component.onClick.AddListener(delegate()
				{
					this.SetEntryVisibility(false);
				});
			}
			else
			{
				component.gameObject.SetActive(false);
			}
			this.scrollRectContainer = this.container.Find("ScrollContainer").GetComponent<RectTransform>();
			this.scrollRect = this.scrollRectContainer.Find("ScrollRect").GetComponent<ScrollRect>();
			this.bubbleContainer = this.scrollRect.transform.Find("Viewport/Content").GetComponent<RectTransform>();
			this.entryPreviewText.text = string.Empty;
			this.unreadDot.gameObject.SetActive(!this.read && this.messageHistory.Count > 0);
			this.responseContainer = this.container.Find("Responses").GetComponent<RectTransform>();
			this.senderInterface = this.container.Find("SenderInterface").GetComponent<MessageSenderInterface>();
			for (int i = 0; i < this.Sendables.Count; i++)
			{
				this.senderInterface.AddSendable(this.Sendables[i]);
			}
			this.RepositionEntry();
			this.SetResponseContainerVisible(false);
			this.SetOpen(false);
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00087E67 File Offset: 0x00086067
		private void EnsureUIExists()
		{
			if (!this.uiCreated)
			{
				this.CreateUI();
			}
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x00087E78 File Offset: 0x00086078
		protected void RefreshPreviewText()
		{
			if (this.bubbles.Count == 0)
			{
				this.entryPreviewText.text = string.Empty;
				return;
			}
			this.entryPreviewText.text = this.bubbles[this.bubbles.Count - 1].text;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00087ECB File Offset: 0x000860CB
		public void RepositionEntry()
		{
			if (this.entry == null)
			{
				return;
			}
			this.entry.SetSiblingIndex(MessagesApp.ActiveConversations.IndexOf(this));
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00087EF4 File Offset: 0x000860F4
		public void SetIsKnown(bool known)
		{
			this.IsSenderKnown = known;
			if (this.entry != null)
			{
				this.entry.Find("Name").GetComponent<Text>().text = (this.IsSenderKnown ? this.contactName : "Unknown");
				this.entry.Find("IconMask/Icon").GetComponent<Image>().sprite = (this.IsSenderKnown ? this.sender.MugshotSprite : PlayerSingleton<MessagesApp>.Instance.BlankAvatarSprite);
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00087F7E File Offset: 0x0008617E
		public void EntryClicked()
		{
			this.SetOpen(true);
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00087F88 File Offset: 0x00086188
		public void SetOpen(bool open)
		{
			this.isOpen = open;
			PlayerSingleton<MessagesApp>.Instance.homePage.gameObject.SetActive(!open);
			PlayerSingleton<MessagesApp>.Instance.dialoguePage.gameObject.SetActive(open);
			if (open)
			{
				PlayerSingleton<MessagesApp>.Instance.SetCurrentConversation(this);
				PlayerSingleton<MessagesApp>.Instance.relationshipContainer.gameObject.SetActive(false);
				PlayerSingleton<MessagesApp>.Instance.standardsContainer.gameObject.SetActive(false);
				float y = 0f;
				if (this.sender.ShowRelationshipInfo)
				{
					y = 20f;
					PlayerSingleton<MessagesApp>.Instance.relationshipScrollbar.value = this.sender.RelationData.NormalizedRelationDelta;
					PlayerSingleton<MessagesApp>.Instance.relationshipTooltip.text = RelationshipCategory.GetCategory(this.sender.RelationData.RelationDelta).ToString();
					PlayerSingleton<MessagesApp>.Instance.relationshipContainer.gameObject.SetActive(true);
					Customer customer;
					if (this.sender.TryGetComponent<Customer>(out customer))
					{
						PlayerSingleton<MessagesApp>.Instance.standardsStar.color = ItemQuality.GetColor(customer.CustomerData.Standards.GetCorrespondingQuality());
						PlayerSingleton<MessagesApp>.Instance.standardsTooltip.text = customer.CustomerData.Standards.GetName() + " standards.";
						PlayerSingleton<MessagesApp>.Instance.standardsContainer.gameObject.SetActive(true);
					}
				}
				PlayerSingleton<MessagesApp>.Instance.dialoguePageNameText.text = (this.IsSenderKnown ? this.contactName : "Unknown");
				PlayerSingleton<MessagesApp>.Instance.dialoguePageNameText.rectTransform.anchoredPosition = new Vector2(-PlayerSingleton<MessagesApp>.Instance.dialoguePageNameText.preferredWidth / 2f + 30f, y);
				PlayerSingleton<MessagesApp>.Instance.iconContainerRect.anchoredPosition = new Vector2(-PlayerSingleton<MessagesApp>.Instance.dialoguePageNameText.preferredWidth / 2f - 30f, PlayerSingleton<MessagesApp>.Instance.iconContainerRect.anchoredPosition.y);
				PlayerSingleton<MessagesApp>.Instance.iconImage.sprite = (this.IsSenderKnown ? this.sender.MugshotSprite : PlayerSingleton<MessagesApp>.Instance.BlankAvatarSprite);
				this.SetRead(true);
				this.CheckSendLoop();
				for (int i = 0; i < this.responseRects.Count; i++)
				{
					this.responseRects[i].gameObject.GetComponent<MessageBubble>().RefreshDisplayedText();
				}
				for (int j = 0; j < this.bubbles.Count; j++)
				{
					this.bubbles[j].autosetPosition = false;
					this.bubbles[j].RefreshDisplayedText();
				}
			}
			else
			{
				PlayerSingleton<MessagesApp>.Instance.SetCurrentConversation(null);
			}
			this.container.gameObject.SetActive(open);
			this.SetResponseContainerVisible(this.AreResponsesActive);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x0008826C File Offset: 0x0008646C
		protected virtual void RenderMessage(Message m)
		{
			MessageBubble component = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<MessagesApp>.Instance.messageBubblePrefab, this.bubbleContainer).GetComponent<MessageBubble>();
			component.SetupBubble(m.text, (m.sender == Message.ESenderType.Other) ? MessageBubble.Alignment.Left : MessageBubble.Alignment.Right, false);
			float num = 0f;
			for (int i = 0; i < this.bubbles.Count; i++)
			{
				num += this.bubbles[i].height;
				num += this.bubbles[i].spacingAbove;
			}
			bool flag = false;
			if (this.messageHistory.IndexOf(m) > 0 && this.messageHistory[this.messageHistory.IndexOf(m) - 1].sender == m.sender)
			{
				flag = true;
			}
			float num2 = MessageBubble.baseBubbleSpacing;
			if (!flag)
			{
				num2 *= 10f;
			}
			if (flag && this.messageHistory[this.messageHistory.IndexOf(m) - 1].endOfGroup)
			{
				num2 *= 20f;
			}
			component.container.anchoredPosition = new Vector2(component.container.anchoredPosition.x, -num - num2 - component.height / 2f);
			component.spacingAbove = num2;
			component.showTriangle = true;
			if (flag && !this.messageHistory[this.messageHistory.IndexOf(m) - 1].endOfGroup)
			{
				this.bubbles[this.bubbles.Count - 1].showTriangle = false;
			}
			this.bubbleContainer.sizeDelta = new Vector2(this.bubbleContainer.sizeDelta.x, num + component.height + num2 + MessageBubble.baseBubbleSpacing * 10f);
			this.scrollRect.verticalNormalizedPosition = 0f;
			this.bubbles.Add(component);
			if (m.sender == Message.ESenderType.Player && PlayerSingleton<MessagesApp>.Instance.isOpen && PlayerSingleton<Phone>.Instance.IsOpen)
			{
				PlayerSingleton<MessagesApp>.Instance.MessageSentSound.Play();
			}
			else if (PlayerSingleton<Phone>.Instance.IsOpen && PlayerSingleton<MessagesApp>.Instance.isOpen && (this.isOpen || PlayerSingleton<MessagesApp>.Instance.currentConversation == null))
			{
				PlayerSingleton<MessagesApp>.Instance.MessageReceivedSound.Play();
			}
			if (this.onMessageRendered != null)
			{
				this.onMessageRendered();
			}
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000884BE File Offset: 0x000866BE
		public void SetEntryVisibility(bool v)
		{
			if (!v && !this.sender.ConversationCanBeHidden)
			{
				return;
			}
			this.EntryVisible = v;
			this.entry.gameObject.SetActive(v);
			if (!v)
			{
				this.SetRead(true);
			}
			this.HasChanged = true;
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000884FC File Offset: 0x000866FC
		public void SetRead(bool r)
		{
			this.read = r;
			if (this.read)
			{
				if (PlayerSingleton<MessagesApp>.Instance.unreadConversations.Contains(this))
				{
					PlayerSingleton<MessagesApp>.Instance.unreadConversations.Remove(this);
					PlayerSingleton<MessagesApp>.Instance.RefreshNotifications();
				}
			}
			else if (!PlayerSingleton<MessagesApp>.Instance.unreadConversations.Contains(this))
			{
				PlayerSingleton<MessagesApp>.Instance.unreadConversations.Add(this);
				PlayerSingleton<MessagesApp>.Instance.RefreshNotifications();
			}
			if (this.unreadDot != null)
			{
				this.unreadDot.gameObject.SetActive(!this.read);
			}
			this.HasChanged = true;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000885A4 File Offset: 0x000867A4
		public void SendMessage(Message message, bool notify = true, bool network = true)
		{
			this.EnsureUIExists();
			if (message.messageId == -1)
			{
				message.messageId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
			if (this.messageHistory.Find((Message x) => x.messageId == message.messageId) != null)
			{
				return;
			}
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.SendMessage(message, notify, this.sender.ID);
				return;
			}
			this.messageHistory.Add(message);
			if (this.messageHistory.Count > 10)
			{
				this.messageHistory.RemoveAt(0);
			}
			if (message.sender == Message.ESenderType.Other && notify)
			{
				this.SetEntryVisibility(true);
				if (!this.isOpen)
				{
					this.SetRead(false);
				}
				if (!this.isOpen || !PlayerSingleton<MessagesApp>.Instance.isOpen || !PlayerSingleton<Phone>.Instance.IsOpen)
				{
					Singleton<NotificationsManager>.Instance.SendNotification(this.IsSenderKnown ? this.contactName : "Unknown", message.text, PlayerSingleton<MessagesApp>.Instance.AppIcon, 5f, true);
				}
			}
			this.RenderMessage(message);
			this.RefreshPreviewText();
			this.MoveToTop();
			this.HasChanged = true;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000886F4 File Offset: 0x000868F4
		public void SendMessageChain(MessageChain messages, float initialDelay = 0f, bool notify = true, bool network = true)
		{
			MSGConversation.<>c__DisplayClass83_0 CS$<>8__locals1 = new MSGConversation.<>c__DisplayClass83_0();
			CS$<>8__locals1.messages = messages;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.notify = notify;
			this.EnsureUIExists();
			if (CS$<>8__locals1.messages.id == -1)
			{
				CS$<>8__locals1.messages.id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
			if (this.messageChainHistory.Find((MessageChain x) => x.id == CS$<>8__locals1.messages.id) != null)
			{
				return;
			}
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.SendMessageChain(CS$<>8__locals1.messages, this.sender.ID, initialDelay, CS$<>8__locals1.notify);
				return;
			}
			this.messageChainHistory.Add(CS$<>8__locals1.messages);
			this.HasChanged = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<SendMessageChain>g__Routine|1(CS$<>8__locals1.messages, initialDelay));
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000887BC File Offset: 0x000869BC
		public MSGConversationData GetSaveData()
		{
			List<TextMessageData> list = new List<TextMessageData>();
			for (int i = 0; i < this.messageHistory.Count; i++)
			{
				list.Add(this.messageHistory[i].GetSaveData());
			}
			List<TextResponseData> list2 = new List<TextResponseData>();
			for (int j = 0; j < this.currentResponses.Count; j++)
			{
				list2.Add(new TextResponseData(this.currentResponses[j].text, this.currentResponses[j].label));
			}
			return new MSGConversationData(MessagesApp.ActiveConversations.IndexOf(this), this.read, list.ToArray(), list2.ToArray(), !this.EntryVisible);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x00088870 File Offset: 0x00086A70
		public virtual string GetSaveString()
		{
			return this.GetSaveData().GetJson(true);
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x00088880 File Offset: 0x00086A80
		public virtual void Load(MSGConversationData data)
		{
			this.EnsureUIExists();
			this.index = data.ConversationIndex;
			this.SetRead(data.Read);
			if (data.MessageHistory != null)
			{
				for (int i = 0; i < data.MessageHistory.Length; i++)
				{
					Message message = new Message(data.MessageHistory[i]);
					this.messageHistory.Add(message);
					if (this.messageHistory.Count > 10)
					{
						this.messageHistory.RemoveAt(0);
					}
					this.RenderMessage(message);
				}
			}
			else
			{
				Console.LogWarning("Message history null!", null);
			}
			if (data.ActiveResponses != null)
			{
				List<Response> list = new List<Response>();
				for (int j = 0; j < data.ActiveResponses.Length; j++)
				{
					list.Add(new Response(data.ActiveResponses[j].Text, data.ActiveResponses[j].Label, null, false));
				}
				if (list.Count > 0)
				{
					this.ShowResponses(list, 0f, true);
				}
			}
			else
			{
				Console.LogWarning("Message reponses null!", null);
			}
			this.RefreshPreviewText();
			this.HasChanged = false;
			bool isHidden = data.IsHidden;
			if (data.IsHidden)
			{
				this.SetEntryVisibility(false);
			}
			if (this.onLoaded != null)
			{
				this.onLoaded();
			}
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x000889B0 File Offset: 0x00086BB0
		public void SetSliderValue(float value, Color color)
		{
			if (this.slider == null)
			{
				return;
			}
			this.slider.value = value;
			this.sliderFill.color = color;
			this.slider.gameObject.SetActive(value > 0f);
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x000889FC File Offset: 0x00086BFC
		public Response GetResponse(string label)
		{
			return this.currentResponses.Find((Response x) => x.label == label);
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00088A30 File Offset: 0x00086C30
		public void ShowResponses(List<Response> _responses, float showResponseDelay = 0f, bool network = true)
		{
			MSGConversation.<>c__DisplayClass89_0 CS$<>8__locals1 = new MSGConversation.<>c__DisplayClass89_0();
			CS$<>8__locals1.showResponseDelay = showResponseDelay;
			CS$<>8__locals1.<>4__this = this;
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.ShowResponses(this.sender.ID, _responses, CS$<>8__locals1.showResponseDelay);
				return;
			}
			this.EnsureUIExists();
			this.currentResponses = _responses;
			this.ClearResponseUI();
			for (int i = 0; i < _responses.Count; i++)
			{
				this.CreateResponseUI(_responses[i]);
			}
			if (CS$<>8__locals1.showResponseDelay == 0f)
			{
				this.SetResponseContainerVisible(true);
			}
			else
			{
				Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<ShowResponses>g__Routine|0());
			}
			this.HasChanged = true;
			if (this.onResponsesShown != null)
			{
				this.onResponsesShown();
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00088AE4 File Offset: 0x00086CE4
		protected void CreateResponseUI(Response r)
		{
			this.EnsureUIExists();
			MessageBubble component = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<MessagesApp>.Instance.messageBubblePrefab, this.responseContainer).GetComponent<MessageBubble>();
			float num = 5f;
			float num2 = 25f;
			component.bubble_MinWidth = this.responseContainer.rect.width - num2 * 2f;
			component.bubble_MaxWidth = this.responseContainer.rect.width - num2 * 2f;
			component.autosetPosition = false;
			component.SetupBubble(r.text, MessageBubble.Alignment.Center, true);
			float num3 = num2;
			for (int i = 0; i < this.responseRects.Count; i++)
			{
				num3 += this.responseRects[i].gameObject.GetComponent<MessageBubble>().height;
				num3 += num;
			}
			component.container.anchoredPosition = new Vector2(0f, -num3 - 35f);
			this.responseRects.Add(component.container);
			component.button.interactable = true;
			bool network = !r.disableDefaultResponseBehaviour;
			component.button.onClick.AddListener(delegate()
			{
				this.ResponseChosen(r, network);
			});
			this.responseContainer.sizeDelta = new Vector2(this.responseContainer.sizeDelta.x, num3 + component.height + num2);
			this.responseContainer.anchoredPosition = new Vector2(0f, this.responseContainer.sizeDelta.y / 2f);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00088C98 File Offset: 0x00086E98
		private void RefreshResponseContainer()
		{
			for (int i = 0; i < this.responseRects.Count; i++)
			{
				this.responseRects[i].gameObject.GetComponent<MessageBubble>().RefreshDisplayedText();
			}
			float num = 5f;
			float num2 = 25f;
			float num3 = num2;
			for (int j = 0; j < this.responseRects.Count; j++)
			{
				num3 += this.responseRects[j].gameObject.GetComponent<MessageBubble>().height;
				num3 += num;
			}
			this.responseContainer.sizeDelta = new Vector2(this.responseContainer.sizeDelta.x, num3 + num2);
			this.responseContainer.anchoredPosition = new Vector2(0f, this.responseContainer.sizeDelta.y / 2f);
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00088D70 File Offset: 0x00086F70
		protected void ClearResponseUI()
		{
			for (int i = 0; i < this.responseRects.Count; i++)
			{
				UnityEngine.Object.Destroy(this.responseRects[i].gameObject);
			}
			this.responseRects.Clear();
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00088DB4 File Offset: 0x00086FB4
		public void SetResponseContainerVisible(bool v)
		{
			if (v)
			{
				this.scrollRectContainer.offsetMin = new Vector2(0f, this.responseContainer.sizeDelta.y);
			}
			else
			{
				this.scrollRectContainer.offsetMin = new Vector2(0f, 0f);
			}
			this.responseContainer.gameObject.SetActive(v);
			this.bubbleContainer.anchoredPosition = new Vector2(this.bubbleContainer.anchoredPosition.x, Mathf.Clamp(this.bubbleContainer.anchoredPosition.y, 1100f, float.MaxValue));
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00088E58 File Offset: 0x00087058
		public void ResponseChosen(Response r, bool network)
		{
			if (!this.AreResponsesActive)
			{
				return;
			}
			if (r.disableDefaultResponseBehaviour)
			{
				if (r.callback != null)
				{
					r.callback();
				}
				return;
			}
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.SendResponse(this.currentResponses.IndexOf(r), this.sender.ID);
				return;
			}
			this.ClearResponses(false);
			this.RenderMessage(new Message(r.text, Message.ESenderType.Player, true, -1));
			this.HasChanged = true;
			this.MoveToTop();
			if (r.callback != null)
			{
				r.callback();
			}
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x00088EEA File Offset: 0x000870EA
		public void ClearResponses(bool network = false)
		{
			this.ClearResponseUI();
			this.SetResponseContainerVisible(false);
			this.currentResponses.Clear();
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.ClearResponses(this.sender.ID);
			}
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x00088F1C File Offset: 0x0008711C
		public SendableMessage CreateSendableMessage(string text)
		{
			SendableMessage sendableMessage = new SendableMessage(text, this);
			this.Sendables.Add(sendableMessage);
			if (this.uiCreated)
			{
				this.senderInterface.AddSendable(sendableMessage);
			}
			return sendableMessage;
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x00088F52 File Offset: 0x00087152
		public void SendPlayerMessage(int sendableIndex, int sentIndex, bool network)
		{
			if (network)
			{
				NetworkSingleton<MessagingManager>.Instance.SendPlayerMessage(sendableIndex, sentIndex, this.sender.ID);
				return;
			}
			this.Sendables[sendableIndex].Send(false, sentIndex);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x00088F84 File Offset: 0x00087184
		public void RenderPlayerMessage(SendableMessage sendable)
		{
			Message m = new Message(sendable.Text, Message.ESenderType.Player, true, -1);
			this.RenderMessage(m);
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x00088FA7 File Offset: 0x000871A7
		private void CheckSendLoop()
		{
			this.CanSendNewMessage();
			PlayerSingleton<MessagesApp>.Instance.StartCoroutine(this.<CheckSendLoop>g__Loop|99_0());
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x00088FC4 File Offset: 0x000871C4
		private bool CanSendNewMessage()
		{
			if (this.rollingOut)
			{
				return false;
			}
			if (this.AreResponsesActive)
			{
				return false;
			}
			return this.Sendables.FirstOrDefault((SendableMessage x) => x.ShouldShow()) != null;
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x0008901D File Offset: 0x0008721D
		[CompilerGenerated]
		private IEnumerator <CheckSendLoop>g__Loop|99_0()
		{
			while (this.isOpen)
			{
				if (this.CanSendNewMessage())
				{
					if (this.senderInterface.Visibility == MessageSenderInterface.EVisibility.Hidden)
					{
						this.senderInterface.SetVisibility(MessageSenderInterface.EVisibility.Docked);
					}
				}
				else if (this.senderInterface.Visibility != MessageSenderInterface.EVisibility.Hidden)
				{
					this.senderInterface.SetVisibility(MessageSenderInterface.EVisibility.Hidden);
				}
				this.scrollRect.GetComponent<RectTransform>().offsetMin = new Vector2(0f, (this.senderInterface.Visibility == MessageSenderInterface.EVisibility.Docked) ? 200f : 0f);
				yield return new WaitForEndOfFrame();
			}
			this.senderInterface.SetVisibility(MessageSenderInterface.EVisibility.Hidden);
			this.scrollRect.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
			yield break;
		}

		// Token: 0x04001950 RID: 6480
		public const int MAX_MESSAGE_HISTORY = 10;

		// Token: 0x04001951 RID: 6481
		public string contactName = string.Empty;

		// Token: 0x04001952 RID: 6482
		public NPC sender;

		// Token: 0x04001954 RID: 6484
		public List<Message> messageHistory = new List<Message>();

		// Token: 0x04001955 RID: 6485
		public List<MessageChain> messageChainHistory = new List<MessageChain>();

		// Token: 0x04001956 RID: 6486
		public List<MessageBubble> bubbles = new List<MessageBubble>();

		// Token: 0x04001957 RID: 6487
		public List<SendableMessage> Sendables = new List<SendableMessage>();

		// Token: 0x04001958 RID: 6488
		public bool read = true;

		// Token: 0x0400195D RID: 6493
		public List<EConversationCategory> Categories = new List<EConversationCategory>();

		// Token: 0x0400195E RID: 6494
		public RectTransform entry;

		// Token: 0x0400195F RID: 6495
		protected RectTransform container;

		// Token: 0x04001960 RID: 6496
		protected RectTransform bubbleContainer;

		// Token: 0x04001961 RID: 6497
		protected RectTransform scrollRectContainer;

		// Token: 0x04001962 RID: 6498
		protected ScrollRect scrollRect;

		// Token: 0x04001963 RID: 6499
		protected Text entryPreviewText;

		// Token: 0x04001964 RID: 6500
		protected RectTransform unreadDot;

		// Token: 0x04001965 RID: 6501
		protected Slider slider;

		// Token: 0x04001966 RID: 6502
		protected Image sliderFill;

		// Token: 0x04001967 RID: 6503
		protected RectTransform responseContainer;

		// Token: 0x04001968 RID: 6504
		protected MessageSenderInterface senderInterface;

		// Token: 0x04001969 RID: 6505
		private bool uiCreated;

		// Token: 0x0400196A RID: 6506
		public Action onMessageRendered;

		// Token: 0x0400196B RID: 6507
		public Action onLoaded;

		// Token: 0x0400196C RID: 6508
		public Action onResponsesShown;

		// Token: 0x0400196D RID: 6509
		public List<Response> currentResponses = new List<Response>();

		// Token: 0x0400196E RID: 6510
		private List<RectTransform> responseRects = new List<RectTransform>();
	}
}
