using System;

namespace ScheduleOne.NPCs
{
	// Token: 0x02000441 RID: 1089
	public class Donna : NPC
	{
		// Token: 0x060015C9 RID: 5577 RVA: 0x00060583 File Offset: 0x0005E783
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.DonnaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.DonnaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0006059C File Offset: 0x0005E79C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.DonnaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.DonnaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x000605B5 File Offset: 0x0005E7B5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x000605C3 File Offset: 0x0005E7C3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001476 RID: 5238
		private bool dll_Excuted;

		// Token: 0x04001477 RID: 5239
		private bool dll_Excuted;
	}
}
