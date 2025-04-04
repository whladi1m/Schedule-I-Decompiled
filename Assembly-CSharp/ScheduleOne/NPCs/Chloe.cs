using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000440 RID: 1088
	public class Chloe : NPC
	{
		// Token: 0x060015C4 RID: 5572 RVA: 0x0006052F File Offset: 0x0005E72F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.ChloeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.ChloeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x00060548 File Offset: 0x0005E748
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.ChloeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.ChloeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x00060561 File Offset: 0x0005E761
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0006056F File Offset: 0x0005E76F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001474 RID: 5236
		private bool dll_Excuted;

		// Token: 0x04001475 RID: 5237
		private bool dll_Excuted;
	}
}
