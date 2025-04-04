using System;
using System.Linq;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000697 RID: 1687
	[Serializable]
	public class DialogueChain
	{
		// Token: 0x06002E95 RID: 11925 RVA: 0x000C324B File Offset: 0x000C144B
		public MessageChain GetMessageChain()
		{
			return new MessageChain
			{
				Messages = this.Lines.ToList<string>()
			};
		}

		// Token: 0x04002133 RID: 8499
		[TextArea(1, 10)]
		public string[] Lines;
	}
}
