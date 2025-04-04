using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Money;
using ScheduleOne.NPCs.CharacterClasses;
using ScheduleOne.Property;
using ScheduleOne.Variables;

namespace ScheduleOne.Dialogue
{
	// Token: 0x0200068D RID: 1677
	public class DialogueController_Fixer : DialogueController
	{
		// Token: 0x06002E6C RID: 11884 RVA: 0x000C2628 File Offset: 0x000C0828
		public override void ChoiceCallback(string choiceLabel)
		{
			base.ChoiceCallback(choiceLabel);
			if (choiceLabel == "CONFIRM" && this.selectedProperty != null)
			{
				this.Confirm();
			}
			if (!(choiceLabel == "Botanist"))
			{
				if (!(choiceLabel == "Packager"))
				{
					if (!(choiceLabel == "Chemist"))
					{
						if (choiceLabel == "Cleaner")
						{
							this.selectedEmployeeType = EEmployeeType.Cleaner;
						}
					}
					else
					{
						this.selectedEmployeeType = EEmployeeType.Chemist;
					}
				}
				else
				{
					this.selectedEmployeeType = EEmployeeType.Handler;
				}
			}
			else
			{
				this.selectedEmployeeType = EEmployeeType.Botanist;
			}
			foreach (Property property in Property.OwnedProperties)
			{
				if (choiceLabel == property.PropertyCode)
				{
					this.selectedProperty = property;
					this.handler.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByLabel("FINALIZE"));
					break;
				}
			}
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000C2724 File Offset: 0x000C0924
		public override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (dialogueLabel == "SELECT_LOCATION")
			{
				foreach (Property property in Property.OwnedProperties)
				{
					if (!(property.PropertyCode == "rv") && !(property.PropertyCode == "motelroom"))
					{
						int count = property.GetUnassignedBeds().Count;
						string propertyName = property.PropertyName;
						existingChoices.Add(new DialogueChoiceData
						{
							ChoiceText = propertyName,
							ChoiceLabel = property.PropertyCode
						});
					}
				}
			}
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000C27E0 File Offset: 0x000C09E0
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			if (choiceLabel == "CONFIRM")
			{
				Employee employeePrefab = NetworkSingleton<EmployeeManager>.Instance.GetEmployeePrefab(this.selectedEmployeeType);
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance < employeePrefab.SigningFee + Fixer.GetAdditionalSigningFee())
				{
					invalidReason = "Insufficient cash";
					return false;
				}
			}
			foreach (Property property in Property.OwnedProperties)
			{
				if (choiceLabel == property.PropertyCode && property.Employees.Count >= property.EmployeeCapacity)
				{
					invalidReason = string.Concat(new string[]
					{
						"Employee limit reached (",
						property.Employees.Count.ToString(),
						"/",
						property.EmployeeCapacity.ToString(),
						")"
					});
					return false;
				}
			}
			return base.CheckChoice(choiceLabel, out invalidReason);
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000C28E4 File Offset: 0x000C0AE4
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "FINALIZE")
			{
				Employee employeePrefab = NetworkSingleton<EmployeeManager>.Instance.GetEmployeePrefab(this.selectedEmployeeType);
				dialogueText = dialogueText.Replace("<SIGN_FEE>", "<color=#54E717>" + MoneyManager.FormatAmount(employeePrefab.SigningFee + Fixer.GetAdditionalSigningFee(), false, false) + "</color>");
				dialogueText = dialogueText.Replace("<DAILY_WAGE>", "<color=#54E717>" + MoneyManager.FormatAmount(employeePrefab.DailyWage, false, false) + "</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000C296F File Offset: 0x000C0B6F
		public override bool DecideBranch(string branchLabel, out int index)
		{
			if (branchLabel == "IS_FIRST_WORKER")
			{
				if (this.lastConfirmationWasInitial)
				{
					index = 1;
				}
				else
				{
					index = 0;
				}
				return true;
			}
			return base.DecideBranch(branchLabel, out index);
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000C2998 File Offset: 0x000C0B98
		private void Confirm()
		{
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>("ClipboardAcquired"))
			{
				this.lastConfirmationWasInitial = true;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ClipboardAcquired", true.ToString(), true);
			}
			else
			{
				this.lastConfirmationWasInitial = false;
			}
			Employee employeePrefab = NetworkSingleton<EmployeeManager>.Instance.GetEmployeePrefab(this.selectedEmployeeType);
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-(employeePrefab.SigningFee + Fixer.GetAdditionalSigningFee()), true, false);
			NetworkSingleton<EmployeeManager>.Instance.CreateNewEmployee(this.selectedProperty, this.selectedEmployeeType);
		}

		// Token: 0x04002119 RID: 8473
		private EEmployeeType selectedEmployeeType;

		// Token: 0x0400211A RID: 8474
		private Property selectedProperty;

		// Token: 0x0400211B RID: 8475
		private bool lastConfirmationWasInitial;
	}
}
