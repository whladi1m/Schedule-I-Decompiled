using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B9 RID: 1209
	public class Lucy : NPC
	{
		// Token: 0x06001AE7 RID: 6887 RVA: 0x000714ED File Offset: 0x0006F6ED
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LucyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LucyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x00071506 File Offset: 0x0006F706
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LucyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LucyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0007151F File Offset: 0x0006F71F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0007152D File Offset: 0x0006F72D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016B9 RID: 5817
		private bool dll_Excuted;

		// Token: 0x040016BA RID: 5818
		private bool dll_Excuted;
	}
}
