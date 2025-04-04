using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200043F RID: 1087
	public class Billy : NPC
	{
		// Token: 0x060015BF RID: 5567 RVA: 0x000604DB File Offset: 0x0005E6DB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.BillyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.BillyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x000604F4 File Offset: 0x0005E6F4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.BillyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.BillyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0006050D File Offset: 0x0005E70D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0006051B File Offset: 0x0005E71B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001472 RID: 5234
		private bool dll_Excuted;

		// Token: 0x04001473 RID: 5235
		private bool dll_Excuted;
	}
}
