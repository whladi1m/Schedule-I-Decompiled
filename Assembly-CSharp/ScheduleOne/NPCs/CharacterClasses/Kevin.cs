using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B2 RID: 1202
	public class Kevin : NPC
	{
		// Token: 0x06001AC1 RID: 6849 RVA: 0x00071253 File Offset: 0x0006F453
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KevinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KevinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0007126C File Offset: 0x0006F46C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KevinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KevinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x00071285 File Offset: 0x0006F485
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x00071293 File Offset: 0x0006F493
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016A8 RID: 5800
		private bool dll_Excuted;

		// Token: 0x040016A9 RID: 5801
		private bool dll_Excuted;
	}
}
