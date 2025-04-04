using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000AB9 RID: 2745
	public class MessageSenderInterface : MonoBehaviour
	{
		// Token: 0x060049DD RID: 18909 RVA: 0x00135318 File Offset: 0x00133518
		public void Awake()
		{
			this.SetVisibility(MessageSenderInterface.EVisibility.Hidden);
			this.ComposeButton.onClick.AddListener(delegate()
			{
				this.SetVisibility(MessageSenderInterface.EVisibility.Expanded);
			});
			Button[] cancelButtons = this.CancelButtons;
			for (int i = 0; i < cancelButtons.Length; i++)
			{
				cancelButtons[i].onClick.AddListener(delegate()
				{
					this.SetVisibility(MessageSenderInterface.EVisibility.Docked);
				});
			}
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x00135376 File Offset: 0x00133576
		public void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 15);
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x0013538B File Offset: 0x0013358B
		private void Exit(ExitAction exit)
		{
			if (exit.used)
			{
				return;
			}
			if (this.Visibility == MessageSenderInterface.EVisibility.Expanded)
			{
				this.SetVisibility(MessageSenderInterface.EVisibility.Docked);
				exit.used = true;
			}
		}

		// Token: 0x060049E0 RID: 18912 RVA: 0x001353B0 File Offset: 0x001335B0
		public void SetVisibility(MessageSenderInterface.EVisibility visibility)
		{
			this.Visibility = visibility;
			RectTransform[] array = this.DockedUIElements;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(visibility == MessageSenderInterface.EVisibility.Docked);
			}
			array = this.ExpandedUIElements;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(visibility == MessageSenderInterface.EVisibility.Expanded);
			}
			if (visibility == MessageSenderInterface.EVisibility.Expanded)
			{
				this.UpdateSendables();
			}
			this.SendablesContainer.gameObject.SetActive(visibility == MessageSenderInterface.EVisibility.Expanded);
			this.Menu.anchoredPosition = new Vector2(0f, (this.Visibility == MessageSenderInterface.EVisibility.Expanded) ? this.ExpandedMenuYPos : this.DockedMenuYPos);
			base.gameObject.SetActive(visibility > MessageSenderInterface.EVisibility.Hidden);
			for (int j = 0; j < this.sendableBubbles.Count; j++)
			{
				this.sendableBubbles[j].RefreshDisplayedText();
			}
		}

		// Token: 0x060049E1 RID: 18913 RVA: 0x00135490 File Offset: 0x00133690
		public void UpdateSendables()
		{
			for (int i = 0; i < this.sendableBubbles.Count; i++)
			{
				SendableMessage sendableMessage = this.sendableMap[this.sendableBubbles[i]];
				string text;
				if (!sendableMessage.ShouldShow())
				{
					this.sendableBubbles[i].gameObject.SetActive(false);
				}
				else if (sendableMessage.IsValid(out text))
				{
					this.sendableBubbles[i].button.interactable = true;
					this.sendableBubbles[i].gameObject.SetActive(true);
				}
				else
				{
					this.sendableBubbles[i].button.interactable = false;
					this.sendableBubbles[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x0013555C File Offset: 0x0013375C
		public void AddSendable(SendableMessage sendable)
		{
			MessageBubble component = UnityEngine.Object.Instantiate<GameObject>(PlayerSingleton<MessagesApp>.Instance.messageBubblePrefab, this.SendablesContainer).GetComponent<MessageBubble>();
			component.SetupBubble(sendable.Text, MessageBubble.Alignment.Center, true);
			component.button.onClick.AddListener(delegate()
			{
				this.SendableSelected(sendable);
			});
			this.sendableBubbles.Add(component);
			this.sendableMap.Add(component, sendable);
			this.UpdateSendables();
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x001355EB File Offset: 0x001337EB
		protected virtual void SendableSelected(SendableMessage sendable)
		{
			sendable.Send(true, -1);
			this.SetVisibility(MessageSenderInterface.EVisibility.Hidden);
		}

		// Token: 0x0400375B RID: 14171
		public MessageSenderInterface.EVisibility Visibility;

		// Token: 0x0400375C RID: 14172
		[Header("Settings")]
		public float DockedMenuYPos;

		// Token: 0x0400375D RID: 14173
		public float ExpandedMenuYPos;

		// Token: 0x0400375E RID: 14174
		[Header("References")]
		public RectTransform Menu;

		// Token: 0x0400375F RID: 14175
		public RectTransform SendablesContainer;

		// Token: 0x04003760 RID: 14176
		public RectTransform[] DockedUIElements;

		// Token: 0x04003761 RID: 14177
		public RectTransform[] ExpandedUIElements;

		// Token: 0x04003762 RID: 14178
		public Button ComposeButton;

		// Token: 0x04003763 RID: 14179
		public Button[] CancelButtons;

		// Token: 0x04003764 RID: 14180
		private List<MessageBubble> sendableBubbles = new List<MessageBubble>();

		// Token: 0x04003765 RID: 14181
		private Dictionary<MessageBubble, SendableMessage> sendableMap = new Dictionary<MessageBubble, SendableMessage>();

		// Token: 0x02000ABA RID: 2746
		public enum EVisibility
		{
			// Token: 0x04003767 RID: 14183
			Hidden,
			// Token: 0x04003768 RID: 14184
			Docked,
			// Token: 0x04003769 RID: 14185
			Expanded
		}
	}
}
