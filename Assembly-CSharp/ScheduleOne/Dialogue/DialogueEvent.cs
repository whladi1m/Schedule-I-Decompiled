using System;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200069B RID: 1691
	[Serializable]
	public class DialogueEvent
	{
		// Token: 0x0400213A RID: 8506
		public DialogueContainer Dialogue;

		// Token: 0x0400213B RID: 8507
		public UnityEvent onDialogueEnded;

		// Token: 0x0400213C RID: 8508
		public DialogueNodeEvent[] NodeEvents;
	}
}
