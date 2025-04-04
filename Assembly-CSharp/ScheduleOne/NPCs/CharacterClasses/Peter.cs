using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004C7 RID: 1223
	public class Peter : NPC
	{
		// Token: 0x06001B3F RID: 6975 RVA: 0x00071DDA File Offset: 0x0006FFDA
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PeterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PeterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00071DF3 File Offset: 0x0006FFF3
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PeterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PeterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00071E0C File Offset: 0x0007000C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00071E1A File Offset: 0x0007001A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016E0 RID: 5856
		private bool dll_Excuted;

		// Token: 0x040016E1 RID: 5857
		private bool dll_Excuted;
	}
}
