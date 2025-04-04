using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004AA RID: 1194
	public class Jennifer : NPC
	{
		// Token: 0x06001A97 RID: 6807 RVA: 0x00070EC8 File Offset: 0x0006F0C8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JenniferAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JenniferAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00070EE1 File Offset: 0x0006F0E1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JenniferAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JenniferAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x00070EFA File Offset: 0x0006F0FA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x00070F08 File Offset: 0x0006F108
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001695 RID: 5781
		private bool dll_Excuted;

		// Token: 0x04001696 RID: 5782
		private bool dll_Excuted;
	}
}
