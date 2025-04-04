using System;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006BA RID: 1722
	[Serializable]
	public class DialogueNodeData
	{
		// Token: 0x06002F20 RID: 12064 RVA: 0x000C4B68 File Offset: 0x000C2D68
		public DialogueNodeData GetCopy()
		{
			DialogueNodeData dialogueNodeData = new DialogueNodeData();
			dialogueNodeData.Guid = this.Guid;
			dialogueNodeData.DialogueText = this.DialogueText;
			dialogueNodeData.DialogueNodeLabel = this.DialogueNodeLabel;
			dialogueNodeData.Position = this.Position;
			for (int i = 0; i < this.choices.Length; i++)
			{
				this.choices.CopyTo(dialogueNodeData.choices, 0);
			}
			dialogueNodeData.VoiceLine = this.VoiceLine;
			return dialogueNodeData;
		}

		// Token: 0x04002189 RID: 8585
		public string Guid;

		// Token: 0x0400218A RID: 8586
		public string DialogueText;

		// Token: 0x0400218B RID: 8587
		public string DialogueNodeLabel;

		// Token: 0x0400218C RID: 8588
		public Vector2 Position;

		// Token: 0x0400218D RID: 8589
		public DialogueChoiceData[] choices;

		// Token: 0x0400218E RID: 8590
		public EVOLineType VoiceLine;
	}
}
