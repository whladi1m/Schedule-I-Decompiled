using System;
using ScheduleOne.Dialogue;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000496 RID: 1174
	public class Dean : NPC
	{
		// Token: 0x06001A28 RID: 6696 RVA: 0x0007063A File Offset: 0x0006E83A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.CharacterClasses.Dean_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x000702CA File Offset: 0x0006E4CA
		public bool TattooChoiceValid(out string reason)
		{
			reason = string.Empty;
			return true;
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0007064E File Offset: 0x0006E84E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DeanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DeanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00070667 File Offset: 0x0006E867
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DeanAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DeanAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00070680 File Offset: 0x0006E880
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0007068E File Offset: 0x0006E88E
		protected virtual void dll()
		{
			base.Awake();
			this.dialogueHandler.GetComponent<DialogueController>().Choices[0].isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.TattooChoiceValid);
		}

		// Token: 0x04001665 RID: 5733
		private bool dll_Excuted;

		// Token: 0x04001666 RID: 5734
		private bool dll_Excuted;
	}
}
