using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using UnityEngine.Events;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000691 RID: 1681
	public class DialogueController_SkateboardSeller : DialogueController
	{
		// Token: 0x06002E81 RID: 11905 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000C2D08 File Offset: 0x000C0F08
		public override void ChoiceCallback(string choiceLabel)
		{
			DialogueController_SkateboardSeller.Option option = this.Options.Find((DialogueController_SkateboardSeller.Option x) => x.Name == choiceLabel);
			if (option != null)
			{
				this.chosenWeapon = option;
				this.handler.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByLabel("FINALIZE"));
			}
			if (choiceLabel == "CONFIRM" && this.chosenWeapon != null)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.chosenWeapon.Price, true, false);
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.chosenWeapon.Item.GetDefaultInstance(1));
				this.npc.Inventory.InsertItem(NetworkSingleton<MoneyManager>.Instance.GetCashInstance(this.chosenWeapon.Price), true);
				if (this.chosenWeapon.Item.ID == "goldenskateboard")
				{
					Singleton<AchievementManager>.Instance.UnlockAchievement(AchievementManager.EAchievement.ROLLING_IN_STYLE);
				}
				if (this.onPurchase != null)
				{
					this.onPurchase.Invoke();
				}
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000C2E1D File Offset: 0x000C101D
		public override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (dialogueLabel == "ENTRY" && DialogueHandler.activeDialogue.name == "Skateboard_Sell")
			{
				existingChoices.AddRange(this.GetChoices(this.Options));
			}
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000C2E60 File Offset: 0x000C1060
		private List<DialogueChoiceData> GetChoices(List<DialogueController_SkateboardSeller.Option> options)
		{
			List<DialogueChoiceData> list = new List<DialogueChoiceData>();
			foreach (DialogueController_SkateboardSeller.Option option in options)
			{
				list.Add(new DialogueChoiceData
				{
					ChoiceText = option.Name + "<color=#54E717> (" + MoneyManager.FormatAmount(option.Price, false, false) + ")</color>",
					ChoiceLabel = option.Name
				});
			}
			return list;
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000C2EF0 File Offset: 0x000C10F0
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			DialogueController_SkateboardSeller.Option option = this.Options.Find((DialogueController_SkateboardSeller.Option x) => x.Name == choiceLabel);
			if (option != null)
			{
				if (!option.IsAvailable)
				{
					invalidReason = option.NotAvailableReason;
					return false;
				}
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance < option.Price)
				{
					invalidReason = "Insufficient cash";
					return false;
				}
			}
			if (choiceLabel == "CONFIRM" && !PlayerSingleton<PlayerInventory>.Instance.CanItemFitInInventory(this.chosenWeapon.Item.GetDefaultInstance(1), 1))
			{
				invalidReason = "Inventory full";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000C2F98 File Offset: 0x000C1198
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "FINALIZE" && this.chosenWeapon != null)
			{
				dialogueText = dialogueText.Replace("<NAME>", this.chosenWeapon.Name);
				dialogueText = dialogueText.Replace("<PRICE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.chosenWeapon.Price, false, false) + "</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x04002127 RID: 8487
		public List<DialogueController_SkateboardSeller.Option> Options = new List<DialogueController_SkateboardSeller.Option>();

		// Token: 0x04002128 RID: 8488
		private DialogueController_SkateboardSeller.Option chosenWeapon;

		// Token: 0x04002129 RID: 8489
		public UnityEvent onPurchase;

		// Token: 0x02000692 RID: 1682
		[Serializable]
		public class Option
		{
			// Token: 0x0400212A RID: 8490
			public string Name;

			// Token: 0x0400212B RID: 8491
			public float Price;

			// Token: 0x0400212C RID: 8492
			public bool IsAvailable;

			// Token: 0x0400212D RID: 8493
			public string NotAvailableReason;

			// Token: 0x0400212E RID: 8494
			public ItemDefinition Item;
		}
	}
}
