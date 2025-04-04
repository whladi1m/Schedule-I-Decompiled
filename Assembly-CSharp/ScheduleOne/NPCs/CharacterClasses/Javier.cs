using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A7 RID: 1191
	public class Javier : NPC
	{
		// Token: 0x06001A88 RID: 6792 RVA: 0x00070DCC File Offset: 0x0006EFCC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JavierAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JavierAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x00070DE5 File Offset: 0x0006EFE5
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JavierAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JavierAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00070DFE File Offset: 0x0006EFFE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x00070E0C File Offset: 0x0006F00C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400168F RID: 5775
		private bool dll_Excuted;

		// Token: 0x04001690 RID: 5776
		private bool dll_Excuted;
	}
}
