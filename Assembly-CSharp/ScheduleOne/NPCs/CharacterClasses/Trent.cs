using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D0 RID: 1232
	public class Trent : NPC
	{
		// Token: 0x06001B85 RID: 7045 RVA: 0x000726A8 File Offset: 0x000708A8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.TrentAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.TrentAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x000726C1 File Offset: 0x000708C1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.TrentAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.TrentAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000726DA File Offset: 0x000708DA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000726E8 File Offset: 0x000708E8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001703 RID: 5891
		private bool dll_Excuted;

		// Token: 0x04001704 RID: 5892
		private bool dll_Excuted;
	}
}
