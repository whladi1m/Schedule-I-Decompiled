using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000443 RID: 1091
	public class Jerry : NPC
	{
		// Token: 0x060015D3 RID: 5587 RVA: 0x0006062B File Offset: 0x0005E82B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.JerryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.JerryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00060644 File Offset: 0x0005E844
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.JerryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.JerryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0006065D File Offset: 0x0005E85D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x0006066B File Offset: 0x0005E86B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400147A RID: 5242
		private bool dll_Excuted;

		// Token: 0x0400147B RID: 5243
		private bool dll_Excuted;
	}
}
