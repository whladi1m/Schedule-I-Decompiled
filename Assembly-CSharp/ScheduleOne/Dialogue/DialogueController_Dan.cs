using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200068C RID: 1676
	public class DialogueController_Dan : DialogueController
	{
		// Token: 0x06002E6A RID: 11882 RVA: 0x000C25FA File Offset: 0x000C07FA
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "GIVE_ITEM")
			{
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.ItemToGive.GetDefaultInstance(1));
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x04002118 RID: 8472
		public ItemDefinition ItemToGive;
	}
}
