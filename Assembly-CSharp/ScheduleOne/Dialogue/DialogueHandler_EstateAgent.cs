using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Property;

namespace ScheduleOne.Dialogue
{
	// Token: 0x020006A3 RID: 1699
	public class DialogueHandler_EstateAgent : ControlledDialogueHandler
	{
		// Token: 0x06002EDA RID: 11994 RVA: 0x000C3F34 File Offset: 0x000C2134
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			Property property = Property.UnownedProperties.Find((Property x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			Business business = Business.UnownedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			if (property != null && NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < property.Price)
			{
				invalidReason = "Insufficient balance";
				return false;
			}
			if (business != null && NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance < business.Price)
			{
				invalidReason = "Insufficient balance";
				return false;
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000C3FD4 File Offset: 0x000C21D4
		public override bool ShouldChoiceBeShown(string choiceLabel)
		{
			Property property = Property.OwnedProperties.Find((Property x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			Business business = Business.OwnedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			return (!(property != null) || !property.IsOwned) && (!(business != null) || !business.IsOwned) && base.ShouldChoiceBeShown(choiceLabel);
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000C4050 File Offset: 0x000C2250
		protected override void ChoiceCallback(string choiceLabel)
		{
			Property x3 = Property.UnownedProperties.Find((Property x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			Business x2 = Business.UnownedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			if (x3 != null)
			{
				this.selectedProperty = x3;
			}
			if (x2 != null)
			{
				this.selectedBusiness = x2;
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x000C40C4 File Offset: 0x000C22C4
		protected override void DialogueCallback(string choiceLabel)
		{
			if (choiceLabel == "CONFIRM_BUY")
			{
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(this.selectedProperty.PropertyName + " purchase", -this.selectedProperty.Price, 1f, string.Empty);
				this.selectedProperty.SetOwned();
			}
			if (choiceLabel == "CONFIRM_BUY_BUSINESS")
			{
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction(this.selectedBusiness.PropertyName + " purchase", -this.selectedBusiness.Price, 1f, string.Empty);
				this.selectedBusiness.SetOwned();
			}
			base.DialogueCallback(choiceLabel);
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x000C4174 File Offset: 0x000C2374
		protected override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "CONFIRM")
			{
				return dialogueText.Replace("<PROPERTY>", this.selectedProperty.PropertyName.ToLower());
			}
			if (dialogueLabel == "CONFIRM_BUSINESS")
			{
				return dialogueText.Replace("<BUSINESS>", this.selectedBusiness.PropertyName.ToLower());
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x000C41DC File Offset: 0x000C23DC
		protected override string ModifyChoiceText(string choiceLabel, string choiceText)
		{
			Property property = Property.UnownedProperties.Find((Property x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			Business business = Business.UnownedBusinesses.Find((Business x) => x.PropertyCode.ToLower() == choiceLabel.ToLower());
			if (property != null)
			{
				return choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(property.Price, false, false) + ")</color>");
			}
			if (business != null)
			{
				return choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(business.Price, false, false) + ")</color>");
			}
			if (choiceLabel == "CONFIRM_CHOICE")
			{
				if (this.selectedProperty != null)
				{
					return choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(this.selectedProperty.Price, false, false) + ")</color>");
				}
				if (this.selectedBusiness != null)
				{
					return choiceText.Replace("(<PRICE>)", "<color=#19BEF0>(" + MoneyManager.FormatAmount(this.selectedBusiness.Price, false, false) + ")</color>");
				}
			}
			return base.ModifyChoiceText(choiceLabel, choiceText);
		}

		// Token: 0x0400215E RID: 8542
		private Property selectedProperty;

		// Token: 0x0400215F RID: 8543
		private Business selectedBusiness;
	}
}
