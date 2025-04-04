using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004BA RID: 1210
	public class Ludwig : NPC
	{
		// Token: 0x06001AEC RID: 6892 RVA: 0x00071541 File Offset: 0x0006F741
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LudwigAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LudwigAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x0007155A File Offset: 0x0006F75A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LudwigAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LudwigAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x00071573 File Offset: 0x0006F773
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x00071581 File Offset: 0x0006F781
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016BB RID: 5819
		private bool dll_Excuted;

		// Token: 0x040016BC RID: 5820
		private bool dll_Excuted;
	}
}
