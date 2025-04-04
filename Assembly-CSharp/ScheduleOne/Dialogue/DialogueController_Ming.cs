using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Property;
using ScheduleOne.Quests;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200068E RID: 1678
	public class DialogueController_Ming : DialogueController
	{
		// Token: 0x06002E73 RID: 11891 RVA: 0x000C2A20 File Offset: 0x000C0C20
		protected override void Start()
		{
			base.Start();
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = this.BuyText;
			dialogueChoice.Conversation = this.BuyDialogue;
			dialogueChoice.Enabled = true;
			dialogueChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.CanBuyRoom);
			DialogueController.DialogueChoice dialogueChoice2 = new DialogueController.DialogueChoice();
			dialogueChoice2.ChoiceText = this.RemindText;
			dialogueChoice2.Conversation = this.RemindLocationDialogue;
			dialogueChoice2.Enabled = true;
			dialogueChoice2.shouldShowCheck = ((bool enabled) => this.Property.IsOwned);
			this.AddDialogueChoice(dialogueChoice, 0);
			this.AddDialogueChoice(dialogueChoice2, 0);
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000C2AB4 File Offset: 0x000C0CB4
		private bool CanBuyRoom(bool enabled)
		{
			if (this.Property.IsOwned)
			{
				return false;
			}
			if (this.PurchaseRoomQuests.Length != 0)
			{
				return this.PurchaseRoomQuests.FirstOrDefault((QuestEntry q) => q.State == EQuestState.Active) != null;
			}
			return true;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000C2B0B File Offset: 0x000C0D0B
		public override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			if (choiceLabel == "CHOICE_CONFIRM")
			{
				choiceText = choiceText.Replace("<PRICE>", "<color=#54E717>(" + MoneyManager.FormatAmount(this.Price, false, false) + ")</color>");
			}
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000C2B4B File Offset: 0x000C0D4B
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "ENTRY")
			{
				dialogueText = dialogueText.Replace("<PRICE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.Price, false, false) + "</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000C2B8B File Offset: 0x000C0D8B
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (choiceLabel == "CHOICE_CONFIRM" && NetworkSingleton<MoneyManager>.Instance.cashBalance < this.Price)
			{
				invalidReason = "Insufficient cash";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000C2BC0 File Offset: 0x000C0DC0
		public override void ChoiceCallback(string choiceLabel)
		{
			if (choiceLabel == "CHOICE_CONFIRM")
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.Price, true, false);
				this.npc.Inventory.InsertItem(NetworkSingleton<MoneyManager>.Instance.GetCashInstance(this.Price), true);
				this.Property.SetOwned();
				if (this.onPurchase != null)
				{
					this.onPurchase.Invoke();
				}
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x0400211C RID: 8476
		public Property Property;

		// Token: 0x0400211D RID: 8477
		public float Price = 500f;

		// Token: 0x0400211E RID: 8478
		public DialogueContainer BuyDialogue;

		// Token: 0x0400211F RID: 8479
		public string BuyText = "I'd like to buy the room";

		// Token: 0x04002120 RID: 8480
		public string RemindText = "Where is my room?";

		// Token: 0x04002121 RID: 8481
		public DialogueContainer RemindLocationDialogue;

		// Token: 0x04002122 RID: 8482
		public QuestEntry[] PurchaseRoomQuests;

		// Token: 0x04002123 RID: 8483
		public UnityEvent onPurchase;
	}
}
