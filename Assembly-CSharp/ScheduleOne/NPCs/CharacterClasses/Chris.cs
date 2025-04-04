using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000494 RID: 1172
	public class Chris : NPC
	{
		// Token: 0x06001A1D RID: 6685 RVA: 0x000704EF File Offset: 0x0006E6EF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ChrisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ChrisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00070508 File Offset: 0x0006E708
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ChrisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ChrisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00070521 File Offset: 0x0006E721
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x0007052F File Offset: 0x0006E72F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400165E RID: 5726
		private bool dll_Excuted;

		// Token: 0x0400165F RID: 5727
		private bool dll_Excuted;
	}
}
