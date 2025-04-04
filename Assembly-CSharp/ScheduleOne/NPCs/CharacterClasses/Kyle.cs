using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B4 RID: 1204
	public class Kyle : NPC
	{
		// Token: 0x06001ACB RID: 6859 RVA: 0x000712FB File Offset: 0x0006F4FB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KyleAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KyleAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00071314 File Offset: 0x0006F514
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KyleAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KyleAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0007132D File Offset: 0x0006F52D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x0007133B File Offset: 0x0006F53B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016AC RID: 5804
		private bool dll_Excuted;

		// Token: 0x040016AD RID: 5805
		private bool dll_Excuted;
	}
}
