using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200049E RID: 1182
	public class George : NPC
	{
		// Token: 0x06001A5B RID: 6747 RVA: 0x00070AD8 File Offset: 0x0006ECD8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GeorgeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GeorgeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00070AF1 File Offset: 0x0006ECF1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GeorgeAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GeorgeAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x00070B0A File Offset: 0x0006ED0A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x00070B18 File Offset: 0x0006ED18
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400167D RID: 5757
		private bool dll_Excuted;

		// Token: 0x0400167E RID: 5758
		private bool dll_Excuted;
	}
}
