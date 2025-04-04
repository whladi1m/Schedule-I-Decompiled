using System;
using ScheduleOne.Dialogue;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200048E RID: 1166
	public class Anna : NPC
	{
		// Token: 0x060019FC RID: 6652 RVA: 0x000702B6 File Offset: 0x0006E4B6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.CharacterClasses.Anna_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x000702CA File Offset: 0x0006E4CA
		public bool HairCutChoiceValid(out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x000702D4 File Offset: 0x0006E4D4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AnnaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AnnaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x000702ED File Offset: 0x0006E4ED
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AnnaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AnnaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00070306 File Offset: 0x0006E506
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00070314 File Offset: 0x0006E514
		protected virtual void dll()
		{
			base.Awake();
			this.dialogueHandler.GetComponent<DialogueController>().Choices[0].isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.HairCutChoiceValid);
		}

		// Token: 0x04001652 RID: 5714
		private bool dll_Excuted;

		// Token: 0x04001653 RID: 5715
		private bool dll_Excuted;
	}
}
