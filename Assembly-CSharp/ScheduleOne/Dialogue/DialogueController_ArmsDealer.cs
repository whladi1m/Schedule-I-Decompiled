using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Dialogue
{
	// Token: 0x02000688 RID: 1672
	public class DialogueController_ArmsDealer : DialogueController
	{
		// Token: 0x06002E5E RID: 11870 RVA: 0x000C226A File Offset: 0x000C046A
		private void Awake()
		{
			this.allWeapons = new List<DialogueController_ArmsDealer.WeaponOption>();
			this.allWeapons.AddRange(this.MeleeWeapons);
			this.allWeapons.AddRange(this.RangedWeapons);
			this.allWeapons.AddRange(this.Ammo);
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000C22AC File Offset: 0x000C04AC
		public override void ChoiceCallback(string choiceLabel)
		{
			DialogueController_ArmsDealer.WeaponOption weaponOption = this.allWeapons.Find((DialogueController_ArmsDealer.WeaponOption x) => x.Name == choiceLabel);
			if (weaponOption != null)
			{
				this.chosenWeapon = weaponOption;
				this.handler.ShowNode(DialogueHandler.activeDialogue.GetDialogueNodeByLabel("FINALIZE"));
			}
			if (choiceLabel == "CONFIRM" && this.chosenWeapon != null)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.chosenWeapon.Price, true, false);
				PlayerSingleton<PlayerInventory>.Instance.AddItemToInventory(this.chosenWeapon.Item.GetDefaultInstance(1));
			}
			base.ChoiceCallback(choiceLabel);
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000C235C File Offset: 0x000C055C
		public override void ModifyChoiceList(string dialogueLabel, ref List<DialogueChoiceData> existingChoices)
		{
			if (dialogueLabel == "MELEE_SELECTION")
			{
				existingChoices.AddRange(this.GetWeaponChoices(this.MeleeWeapons));
			}
			if (dialogueLabel == "RANGED_SELECTION")
			{
				existingChoices.AddRange(this.GetWeaponChoices(this.RangedWeapons));
			}
			if (dialogueLabel == "AMMO_SELECTION")
			{
				existingChoices.AddRange(this.GetWeaponChoices(this.Ammo));
			}
			base.ModifyChoiceList(dialogueLabel, ref existingChoices);
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000C23D4 File Offset: 0x000C05D4
		private List<DialogueChoiceData> GetWeaponChoices(List<DialogueController_ArmsDealer.WeaponOption> options)
		{
			List<DialogueChoiceData> list = new List<DialogueChoiceData>();
			foreach (DialogueController_ArmsDealer.WeaponOption weaponOption in options)
			{
				list.Add(new DialogueChoiceData
				{
					ChoiceText = weaponOption.Name + "<color=#54E717> (" + MoneyManager.FormatAmount(weaponOption.Price, false, false) + ")</color>",
					ChoiceLabel = weaponOption.Name
				});
			}
			return list;
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x000C2464 File Offset: 0x000C0664
		public override bool CheckChoice(string choiceLabel, out string invalidReason)
		{
			DialogueController_ArmsDealer.WeaponOption weaponOption = this.allWeapons.Find((DialogueController_ArmsDealer.WeaponOption x) => x.Name == choiceLabel);
			if (weaponOption != null)
			{
				if (!weaponOption.IsAvailable)
				{
					invalidReason = weaponOption.NotAvailableReason;
					return false;
				}
				if (weaponOption.Item.RequiresLevelToPurchase && NetworkSingleton<LevelManager>.Instance.GetFullRank() < weaponOption.Item.RequiredRank)
				{
					string str = "Available at ";
					FullRank requiredRank = weaponOption.Item.RequiredRank;
					invalidReason = str + requiredRank.ToString();
					return false;
				}
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance < weaponOption.Price)
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

		// Token: 0x06002E63 RID: 11875 RVA: 0x000C255C File Offset: 0x000C075C
		public override string ModifyDialogueText(string dialogueLabel, string dialogueText)
		{
			if (dialogueLabel == "FINALIZE" && this.chosenWeapon != null)
			{
				dialogueText = dialogueText.Replace("<WEAPON>", this.chosenWeapon.Name);
				dialogueText = dialogueText.Replace("<PRICE>", "<color=#54E717>" + MoneyManager.FormatAmount(this.chosenWeapon.Price, false, false) + "</color>");
			}
			return base.ModifyDialogueText(dialogueLabel, dialogueText);
		}

		// Token: 0x0400210C RID: 8460
		public List<DialogueController_ArmsDealer.WeaponOption> MeleeWeapons;

		// Token: 0x0400210D RID: 8461
		public List<DialogueController_ArmsDealer.WeaponOption> RangedWeapons;

		// Token: 0x0400210E RID: 8462
		public List<DialogueController_ArmsDealer.WeaponOption> Ammo;

		// Token: 0x0400210F RID: 8463
		private List<DialogueController_ArmsDealer.WeaponOption> allWeapons;

		// Token: 0x04002110 RID: 8464
		private DialogueController_ArmsDealer.WeaponOption chosenWeapon;

		// Token: 0x02000689 RID: 1673
		[Serializable]
		public class WeaponOption
		{
			// Token: 0x04002111 RID: 8465
			public string Name;

			// Token: 0x04002112 RID: 8466
			public float Price;

			// Token: 0x04002113 RID: 8467
			public bool IsAvailable;

			// Token: 0x04002114 RID: 8468
			public string NotAvailableReason;

			// Token: 0x04002115 RID: 8469
			public StorableItemDefinition Item;
		}
	}
}
