using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200049C RID: 1180
	public class Frank : NPC
	{
		// Token: 0x06001A51 RID: 6737 RVA: 0x00070A30 File Offset: 0x0006EC30
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FrankAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.FrankAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x00070A49 File Offset: 0x0006EC49
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FrankAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.FrankAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x00070A62 File Offset: 0x0006EC62
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00070A70 File Offset: 0x0006EC70
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001679 RID: 5753
		private bool dll_Excuted;

		// Token: 0x0400167A RID: 5754
		private bool dll_Excuted;
	}
}
