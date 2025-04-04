using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.NPCs.CharacterClasses;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006A9 RID: 1705
	public class DialogueHandler_VehicleSalesman : ControlledDialogueHandler
	{
		// Token: 0x06002EF3 RID: 12019 RVA: 0x000C44D4 File Offset: 0x000C26D4
		protected override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			Jeremy.DealershipListing dealershipListing = this.Salesman.Listings.Find((Jeremy.DealershipListing x) => x.vehicleCode.ToLower() == choiceLabel.ToLower());
			if (choiceLabel == "BUY_CASH")
			{
				if (this.selectedVehicle != null)
				{
					choiceText = choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(this.selectedVehicle.price, false, false) + ")</color>");
				}
			}
			else if (choiceLabel == "BUY_ONLINE")
			{
				if (this.selectedVehicle != null)
				{
					choiceText = choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(this.selectedVehicle.price, false, false) + ")</color>");
				}
			}
			else if (dealershipListing != null)
			{
				choiceText = choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(dealershipListing.price, false, false) + ")</color>");
			}
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000C45DC File Offset: 0x000C27DC
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (choiceLabel == "BUY_CASH")
			{
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance < this.selectedVehicle.price)
				{
					invalidReason = "Insufficient cash";
					return false;
				}
			}
			else if (choiceLabel == "BUY_ONLINE" && NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < this.selectedVehicle.price)
			{
				invalidReason = "Insufficient balance";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000C464C File Offset: 0x000C284C
		protected override void ChoiceCallback(string choiceLabel)
		{
			if (choiceLabel == "BUY_CASH")
			{
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= this.selectedVehicle.price)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.selectedVehicle.price, true, false);
					this.Salesman.Dealership.SpawnVehicle(this.selectedVehicle.vehicleCode);
					return;
				}
			}
			else if (choiceLabel == "BUY_ONLINE")
			{
				if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.selectedVehicle.price)
				{
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(this.selectedVehicle.vehicleCode + " purchase", -this.selectedVehicle.price, 1f, string.Empty);
					this.Salesman.Dealership.SpawnVehicle(this.selectedVehicle.vehicleCode);
					return;
				}
			}
			else
			{
				Jeremy.DealershipListing dealershipListing = this.Salesman.Listings.Find((Jeremy.DealershipListing x) => x.vehicleCode.ToLower() == choiceLabel.ToLower());
				if (dealershipListing != null)
				{
					this.selectedVehicle = dealershipListing;
				}
				base.ChoiceCallback(choiceLabel);
			}
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000C4776 File Offset: 0x000C2976
		protected override int CheckBranch(string branchLabel)
		{
			if (!(branchLabel == "BRANCH_CAN_AFFORD"))
			{
				return base.CheckBranch(branchLabel);
			}
			if (this.selectedVehicle == null)
			{
				return 0;
			}
			if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.selectedVehicle.price)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000C47B1 File Offset: 0x000C29B1
		protected override void DialogueCallback(string choiceLabel)
		{
			base.DialogueCallback(choiceLabel);
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000C47BA File Offset: 0x000C29BA
		protected override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "CONFIRM")
			{
				return dialogueText.Replace("<VEHICLE>", this.selectedVehicle.vehicleName);
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x04002166 RID: 8550
		public Jeremy Salesman;

		// Token: 0x04002167 RID: 8551
		public Jeremy.DealershipListing selectedVehicle;
	}
}
