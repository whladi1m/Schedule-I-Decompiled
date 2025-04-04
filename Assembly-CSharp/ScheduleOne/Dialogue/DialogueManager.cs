using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006AC RID: 1708
	public class DialogueManager : Singleton<DialogueManager>
	{
		// Token: 0x06002EFE RID: 12030 RVA: 0x000C4824 File Offset: 0x000C2A24
		public DialogueModule Get(EDialogueModule moduleType)
		{
			DialogueModule dialogueModule = this.DefaultModules.Find((DialogueModule x) => x.ModuleType == moduleType);
			if (dialogueModule == null)
			{
				Debug.LogError("Generic module not found for: " + moduleType.ToString());
			}
			return dialogueModule;
		}

		// Token: 0x0400216A RID: 8554
		public DialogueDatabase DefaultDatabase;

		// Token: 0x0400216B RID: 8555
		public List<DialogueModule> DefaultModules = new List<DialogueModule>();
	}
}
