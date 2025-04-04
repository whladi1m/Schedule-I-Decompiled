using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.Money;
using UnityEngine;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000695 RID: 1685
	public class DialogueController_Supplier : DialogueController
	{
		// Token: 0x06002E8D RID: 11917 RVA: 0x000C3041 File Offset: 0x000C1241
		protected override void Start()
		{
			base.Start();
			this.dealer = (this.npc as Dealer);
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000C305C File Offset: 0x000C125C
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (DialogueHandler.activeDialogue.name == "Supplier_Recruitment" && dialogueLabel == "ENTRY")
			{
				dialogueText = dialogueText.Replace("<SIGNING_FEE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.dealer.SigningFee, false, false) + "</color>");
				dialogueText = dialogueText.Replace("<CUT>", "<color=#54E717>" + Mathf.RoundToInt(this.dealer.Cut * 100f).ToString() + "%</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x000C30FC File Offset: 0x000C12FC
		public override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			if (DialogueHandler.activeDialogue.name == "Supplier_Recruitment" && choiceLabel == "CONFIRM")
			{
				choiceText = choiceText.Replace("<SIGNING_FEE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.dealer.SigningFee, false, false) + "</color>");
			}
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x000C3164 File Offset: 0x000C1364
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (DialogueHandler.activeDialogue.name == "Supplier_Recruitment" && choiceLabel == "CONFIRM" && NetworkSingleton<MoneyManager>.Instance.cashBalance < this.dealer.SigningFee)
			{
				invalidReason = "Insufficient cash";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x000C31BC File Offset: 0x000C13BC
		public override void ChoiceCallback(string choiceLabel)
		{
			if (DialogueHandler.activeDialogue.name == "Supplier_Recruitment" && choiceLabel == "CONFIRM")
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.dealer.SigningFee, true, false);
				this.dealer.InitialRecruitment();
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x04002131 RID: 8497
		public Dealer dealer;
	}
}
