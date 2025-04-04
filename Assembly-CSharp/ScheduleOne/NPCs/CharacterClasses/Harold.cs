using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A1 RID: 1185
	public class Harold : NPC
	{
		// Token: 0x06001A6A RID: 6762 RVA: 0x00070BD4 File Offset: 0x0006EDD4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.HaroldAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.HaroldAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x00070BED File Offset: 0x0006EDED
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.HaroldAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.HaroldAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x00070C06 File Offset: 0x0006EE06
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00070C14 File Offset: 0x0006EE14
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001683 RID: 5763
		private bool dll_Excuted;

		// Token: 0x04001684 RID: 5764
		private bool dll_Excuted;
	}
}
