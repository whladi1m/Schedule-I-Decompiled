using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000498 RID: 1176
	public class Elizabeth : NPC
	{
		// Token: 0x06001A35 RID: 6709 RVA: 0x00070711 File Offset: 0x0006E911
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ElizabethAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ElizabethAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x0007072A File Offset: 0x0006E92A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ElizabethAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ElizabethAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x00070743 File Offset: 0x0006E943
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x00070751 File Offset: 0x0006E951
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001669 RID: 5737
		private bool dll_Excuted;

		// Token: 0x0400166A RID: 5738
		private bool dll_Excuted;
	}
}
