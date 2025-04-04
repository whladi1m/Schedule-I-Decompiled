using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000429 RID: 1065
	[Serializable]
	public class TextMessageData
	{
		// Token: 0x0600158E RID: 5518 RVA: 0x0005FB13 File Offset: 0x0005DD13
		public TextMessageData(int sender, int messageID, string text, bool endOfChain)
		{
			this.Sender = sender;
			this.MessageID = messageID;
			this.Text = text;
			this.EndOfChain = endOfChain;
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0005FB38 File Offset: 0x0005DD38
		public TextMessageData()
		{
			this.Sender = 0;
			this.MessageID = 0;
			this.Text = "";
			this.EndOfChain = false;
		}

		// Token: 0x04001456 RID: 5206
		public int Sender;

		// Token: 0x04001457 RID: 5207
		public int MessageID;

		// Token: 0x04001458 RID: 5208
		public string Text;

		// Token: 0x04001459 RID: 5209
		public bool EndOfChain;
	}
}
