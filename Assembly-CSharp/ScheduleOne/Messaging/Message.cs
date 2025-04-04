using System;
using ScheduleOne.Persistence.Datas;
using UnityEngine;

namespace ScheduleOne.Messaging
{
	// Token: 0x02000538 RID: 1336
	[Serializable]
	public class Message
	{
		// Token: 0x060020AC RID: 8364 RVA: 0x00086328 File Offset: 0x00084528
		public Message()
		{
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00086337 File Offset: 0x00084537
		public Message(string _text, Message.ESenderType _type, bool _endOfGroup = false, int _messageId = -1)
		{
			this.text = _text;
			this.sender = _type;
			this.endOfGroup = _endOfGroup;
			if (_messageId == -1)
			{
				this.messageId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00086375 File Offset: 0x00084575
		public Message(TextMessageData data)
		{
			this.text = data.Text;
			this.sender = (Message.ESenderType)data.Sender;
			this.endOfGroup = data.EndOfChain;
			this.messageId = data.MessageID;
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000863B4 File Offset: 0x000845B4
		public TextMessageData GetSaveData()
		{
			return new TextMessageData((int)this.sender, this.messageId, this.text, this.endOfGroup);
		}

		// Token: 0x04001941 RID: 6465
		public int messageId = -1;

		// Token: 0x04001942 RID: 6466
		public string text;

		// Token: 0x04001943 RID: 6467
		public Message.ESenderType sender;

		// Token: 0x04001944 RID: 6468
		public bool endOfGroup;

		// Token: 0x02000539 RID: 1337
		public enum ESenderType
		{
			// Token: 0x04001946 RID: 6470
			Player,
			// Token: 0x04001947 RID: 6471
			Other
		}
	}
}
