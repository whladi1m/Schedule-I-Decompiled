using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000AB4 RID: 2740
	[Serializable]
	public class MessageChain
	{
		// Token: 0x060049C4 RID: 18884 RVA: 0x00134DCF File Offset: 0x00132FCF
		public static MessageChain Combine(MessageChain a, MessageChain b)
		{
			MessageChain messageChain = new MessageChain();
			messageChain.Messages.AddRange(a.Messages);
			messageChain.Messages.AddRange(b.Messages);
			return messageChain;
		}

		// Token: 0x04003736 RID: 14134
		[TextArea(2, 10)]
		public List<string> Messages = new List<string>();

		// Token: 0x04003737 RID: 14135
		[HideInInspector]
		public int id = -1;
	}
}
