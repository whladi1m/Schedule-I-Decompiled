using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004AF RID: 1199
	public class Karen : NPC
	{
		// Token: 0x06001AB2 RID: 6834 RVA: 0x00071157 File Offset: 0x0006F357
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KarenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KarenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x00071170 File Offset: 0x0006F370
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KarenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KarenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x00071189 File Offset: 0x0006F389
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x00071197 File Offset: 0x0006F397
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016A2 RID: 5794
		private bool dll_Excuted;

		// Token: 0x040016A3 RID: 5795
		private bool dll_Excuted;
	}
}
