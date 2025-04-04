using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000400 RID: 1024
	[Serializable]
	public class MSGConversationData : SaveData
	{
		// Token: 0x0600155F RID: 5471 RVA: 0x0005F2FF File Offset: 0x0005D4FF
		public MSGConversationData(int conversationIndex, bool read, TextMessageData[] messageHistory, TextResponseData[] activeResponses, bool isHidden)
		{
			this.ConversationIndex = conversationIndex;
			this.Read = read;
			this.MessageHistory = messageHistory;
			this.ActiveResponses = activeResponses;
			this.IsHidden = isHidden;
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x0005F32C File Offset: 0x0005D52C
		public MSGConversationData()
		{
			this.ConversationIndex = 0;
			this.Read = false;
			this.MessageHistory = new TextMessageData[0];
			this.ActiveResponses = new TextResponseData[0];
			this.IsHidden = false;
		}

		// Token: 0x040013CB RID: 5067
		public int ConversationIndex;

		// Token: 0x040013CC RID: 5068
		public bool Read;

		// Token: 0x040013CD RID: 5069
		public TextMessageData[] MessageHistory;

		// Token: 0x040013CE RID: 5070
		public TextResponseData[] ActiveResponses;

		// Token: 0x040013CF RID: 5071
		public bool IsHidden;
	}
}
