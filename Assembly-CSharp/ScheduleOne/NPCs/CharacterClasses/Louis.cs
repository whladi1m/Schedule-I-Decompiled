using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B8 RID: 1208
	public class Louis : NPC
	{
		// Token: 0x06001AE2 RID: 6882 RVA: 0x00071499 File Offset: 0x0006F699
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LouisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LouisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x000714B2 File Offset: 0x0006F6B2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LouisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LouisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x000714CB File Offset: 0x0006F6CB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x000714D9 File Offset: 0x0006F6D9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016B7 RID: 5815
		private bool dll_Excuted;

		// Token: 0x040016B8 RID: 5816
		private bool dll_Excuted;
	}
}
