using System;
using System.Collections.Generic;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000681 RID: 1665
	public class ControlledDialogueHandler : DialogueHandler
	{
		// Token: 0x06002E30 RID: 11824 RVA: 0x000C1A04 File Offset: 0x000BFC04
		protected override void Awake()
		{
			base.Awake();
			this.controller = base.GetComponent<DialogueController>();
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000C1A18 File Offset: 0x000BFC18
		protected override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			dialogueText = this.controller.ModifyDialogueText(dialogueLabel, dialogueText);
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x000C1A31 File Offset: 0x000BFC31
		protected override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			choiceText = this.controller.ModifyChoiceText(choiceLabel, choiceText);
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000C1A4A File Offset: 0x000BFC4A
		protected override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			this.controller.ModifyChoiceList(dialogueLabel, ref existingChoices);
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000C1A61 File Offset: 0x000BFC61
		protected override void ChoiceCallback(string choiceLabel)
		{
			this.controller.ChoiceCallback(choiceLabel);
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x000C1A78 File Offset: 0x000BFC78
		protected override int CheckBranch(string branchLabel)
		{
			int result;
			if (this.controller.DecideBranch(branchLabel, out result))
			{
				return result;
			}
			return base.CheckBranch(branchLabel);
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000C1A9E File Offset: 0x000BFC9E
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			return this.controller.CheckChoice(choiceLabel, out invalidReason) && base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x040020F0 RID: 8432
		private DialogueController controller;
	}
}
