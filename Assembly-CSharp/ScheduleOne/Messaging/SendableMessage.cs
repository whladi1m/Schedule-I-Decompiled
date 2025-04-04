using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Messaging
{
	// Token: 0x02000548 RID: 1352
	public class SendableMessage
	{
		// Token: 0x06002150 RID: 8528 RVA: 0x000894CF File Offset: 0x000876CF
		public SendableMessage(string text, MSGConversation conversation)
		{
			this.Text = text;
			this.conversation = conversation;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000894F0 File Offset: 0x000876F0
		public virtual bool ShouldShow()
		{
			return this.ShouldShowCheck == null || this.ShouldShowCheck(this);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00089508 File Offset: 0x00087708
		public virtual bool IsValid(out string invalidReason)
		{
			if (this.IsValidCheck != null)
			{
				return this.IsValidCheck(this, out invalidReason);
			}
			invalidReason = "";
			return true;
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x00089528 File Offset: 0x00087728
		public virtual void Send(bool network, int id = -1)
		{
			if (id != -1)
			{
				if (this.sentIDs.Contains(id))
				{
					return;
				}
			}
			else
			{
				id = UnityEngine.Random.Range(0, int.MaxValue);
			}
			if (this.onSelected != null)
			{
				this.onSelected();
			}
			if (this.disableDefaultSendBehaviour)
			{
				return;
			}
			if (network)
			{
				this.conversation.SendPlayerMessage(this.conversation.Sendables.IndexOf(this), id, true);
				return;
			}
			this.sentIDs.Add(id);
			this.conversation.RenderPlayerMessage(this);
			if (this.onSent != null)
			{
				this.onSent();
			}
		}

		// Token: 0x0400198F RID: 6543
		public string Text;

		// Token: 0x04001990 RID: 6544
		public SendableMessage.BoolCheck ShouldShowCheck;

		// Token: 0x04001991 RID: 6545
		public SendableMessage.ValidityCheck IsValidCheck;

		// Token: 0x04001992 RID: 6546
		public Action onSelected;

		// Token: 0x04001993 RID: 6547
		public Action onSent;

		// Token: 0x04001994 RID: 6548
		private MSGConversation conversation;

		// Token: 0x04001995 RID: 6549
		public bool disableDefaultSendBehaviour;

		// Token: 0x04001996 RID: 6550
		private List<int> sentIDs = new List<int>();

		// Token: 0x02000549 RID: 1353
		// (Invoke) Token: 0x06002155 RID: 8533
		public delegate bool BoolCheck(SendableMessage message);

		// Token: 0x0200054A RID: 1354
		// (Invoke) Token: 0x06002159 RID: 8537
		public delegate bool ValidityCheck(SendableMessage message, out string invalidReason);
	}
}
