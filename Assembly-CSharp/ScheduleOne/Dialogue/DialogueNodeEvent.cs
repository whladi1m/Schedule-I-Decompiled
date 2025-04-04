using System;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200069C RID: 1692
	[Serializable]
	public class DialogueNodeEvent
	{
		// Token: 0x0400213D RID: 8509
		public string NodeLabel;

		// Token: 0x0400213E RID: 8510
		public UnityEvent onNodeDisplayed;
	}
}
