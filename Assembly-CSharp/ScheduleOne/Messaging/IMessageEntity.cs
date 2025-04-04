using System;

namespace ScheduleOne.Messaging
{
	// Token: 0x02000537 RID: 1335
	public interface IMessageEntity
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060020A8 RID: 8360
		// (set) Token: 0x060020A9 RID: 8361
		MSGConversation MsgConversation { get; set; }

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060020AA RID: 8362
		// (remove) Token: 0x060020AB RID: 8363
		event ResponseCallback onResponseChosen;
	}
}
