using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004AD RID: 1197
	public class Jessi : NPC
	{
		// Token: 0x06001AA8 RID: 6824 RVA: 0x000710AF File Offset: 0x0006F2AF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JessiAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JessiAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x000710C8 File Offset: 0x0006F2C8
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JessiAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JessiAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x000710E1 File Offset: 0x0006F2E1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x000710EF File Offset: 0x0006F2EF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400169E RID: 5790
		private bool dll_Excuted;

		// Token: 0x0400169F RID: 5791
		private bool dll_Excuted;
	}
}
