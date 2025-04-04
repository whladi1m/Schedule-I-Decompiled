using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004D2 RID: 1234
	public class Walter : NPC
	{
		// Token: 0x06001B90 RID: 7056 RVA: 0x000727E8 File Offset: 0x000709E8
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.WalterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.WalterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x00072801 File Offset: 0x00070A01
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.WalterAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.WalterAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x0007281A File Offset: 0x00070A1A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x00072828 File Offset: 0x00070A28
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001709 RID: 5897
		private bool dll_Excuted;

		// Token: 0x0400170A RID: 5898
		private bool dll_Excuted;
	}
}
