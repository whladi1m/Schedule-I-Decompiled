using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.Persistence;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004BC RID: 1212
	public class Marco : NPC
	{
		// Token: 0x06001AF5 RID: 6901 RVA: 0x000715EC File Offset: 0x0006F7EC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.CharacterClasses.Marco_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0007160B File Offset: 0x0006F80B
		protected override void Start()
		{
			base.Start();
			Singleton<VehicleModMenu>.Instance.onPaintPurchased.AddListener(delegate()
			{
				this.dialogueHandler.ShowWorldspaceDialogue_5s("Thanks buddy");
			});
			Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(this.Loaded));
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x00071649 File Offset: 0x0006F849
		private bool ShouldShowRecoverVehicle(bool enabled)
		{
			return Player.Local.LastDrivenVehicle != null;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0007165B File Offset: 0x0006F85B
		private bool RecoverVehicleValid(out string reason)
		{
			if (Player.Local.LastDrivenVehicle == null)
			{
				reason = "You have no vehicle to recover";
				return false;
			}
			if (Player.Local.LastDrivenVehicle.isOccupied)
			{
				reason = "Someone is in the vehicle";
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0007169C File Offset: 0x0006F89C
		private bool RepaintVehicleValid(out string reason)
		{
			if (this.VehicleDetector.closestVehicle == null)
			{
				reason = "Vehicle must be parked inside the shop";
				return false;
			}
			if (this.VehicleDetector.closestVehicle.isOccupied)
			{
				reason = "Someone is in the vehicle";
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000716E8 File Offset: 0x0006F8E8
		private void RecoverVehicle()
		{
			LandVehicle lastDrivenVehicle = Player.Local.LastDrivenVehicle;
			if (lastDrivenVehicle == null)
			{
				return;
			}
			lastDrivenVehicle.AlignTo(this.VehicleRecoveryPoint, EParkingAlignment.RearToKerb, true);
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x00071718 File Offset: 0x0006F918
		private void Loaded()
		{
			Singleton<LoadManager>.Instance.onLoadComplete.RemoveListener(new UnityAction(this.Loaded));
			if (!NetworkSingleton<VariableDatabase>.Instance.GetValue<bool>(this.GreetedVariable))
			{
				this.EnableGreeting();
			}
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0007174D File Offset: 0x0006F94D
		private void EnableGreeting()
		{
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.GreetingDialogue;
			this.dialogueHandler.onConversationStart.AddListener(new UnityAction(this.SetGreeted));
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x00071784 File Offset: 0x0006F984
		private void SetGreeted()
		{
			this.dialogueHandler.onConversationStart.RemoveListener(new UnityAction(this.SetGreeted));
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.GreetedVariable, true.ToString(), true);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00071814 File Offset: 0x0006FA14
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MarcoAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MarcoAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0007182D File Offset: 0x0006FA2D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MarcoAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MarcoAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00071846 File Offset: 0x0006FA46
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00071854 File Offset: 0x0006FA54
		protected virtual void dll()
		{
			base.Awake();
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = "My vehicle is stuck";
			dialogueChoice.Enabled = true;
			dialogueChoice.shouldShowCheck = new DialogueController.DialogueChoice.ShouldShowCheck(this.ShouldShowRecoverVehicle);
			dialogueChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.RecoverVehicleValid);
			dialogueChoice.Conversation = this.RecoveryConversation;
			dialogueChoice.onChoosen.AddListener(new UnityAction(this.RecoverVehicle));
			DialogueController.DialogueChoice dialogueChoice2 = new DialogueController.DialogueChoice();
			dialogueChoice2.ChoiceText = "I'd like to repaint my vehicle";
			dialogueChoice2.Enabled = true;
			dialogueChoice2.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.RepaintVehicleValid);
			dialogueChoice2.onChoosen.AddListener(delegate()
			{
				Singleton<VehicleModMenu>.Instance.Open(this.VehicleDetector.closestVehicle);
			});
			this.dialogueHandler.GetComponent<DialogueController>().Choices.Add(dialogueChoice2);
			this.dialogueHandler.GetComponent<DialogueController>().Choices.Add(dialogueChoice);
		}

		// Token: 0x040016BF RID: 5823
		public Transform VehicleRecoveryPoint;

		// Token: 0x040016C0 RID: 5824
		public VehicleDetector VehicleDetector;

		// Token: 0x040016C1 RID: 5825
		public DialogueContainer RecoveryConversation;

		// Token: 0x040016C2 RID: 5826
		public DialogueContainer GreetingDialogue;

		// Token: 0x040016C3 RID: 5827
		public string GreetedVariable = "MarcoGreeted";

		// Token: 0x040016C4 RID: 5828
		private bool dll_Excuted;

		// Token: 0x040016C5 RID: 5829
		private bool dll_Excuted;
	}
}
