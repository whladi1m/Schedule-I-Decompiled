using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000442 RID: 1090
	public class Doris : NPC
	{
		// Token: 0x060015CE RID: 5582 RVA: 0x000605D7 File Offset: 0x0005E7D7
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.DorisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.DorisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x000605F0 File Offset: 0x0005E7F0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.DorisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.DorisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x00060609 File Offset: 0x0005E809
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x00060617 File Offset: 0x0005E817
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001478 RID: 5240
		private bool dll_Excuted;

		// Token: 0x04001479 RID: 5241
		private bool dll_Excuted;
	}
}
