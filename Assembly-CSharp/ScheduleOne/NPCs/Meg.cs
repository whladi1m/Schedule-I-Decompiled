using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000444 RID: 1092
	public class Meg : NPC
	{
		// Token: 0x060015D8 RID: 5592 RVA: 0x0006067F File Offset: 0x0005E87F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.MegAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.MegAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060015D9 RID: 5593 RVA: 0x00060698 File Offset: 0x0005E898
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.MegAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.MegAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060015DA RID: 5594 RVA: 0x000606B1 File Offset: 0x0005E8B1
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x000606BF File Offset: 0x0005E8BF
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400147C RID: 5244
		private bool dll_Excuted;

		// Token: 0x0400147D RID: 5245
		private bool dll_Excuted;
	}
}
