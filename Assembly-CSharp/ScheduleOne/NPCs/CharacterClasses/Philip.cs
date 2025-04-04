using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004C8 RID: 1224
	public class Philip : NPC
	{
		// Token: 0x06001B44 RID: 6980 RVA: 0x00071E2E File Offset: 0x0007002E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PhilipAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PhilipAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00071E47 File Offset: 0x00070047
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PhilipAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PhilipAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00071E60 File Offset: 0x00070060
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x00071E6E File Offset: 0x0007006E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016E2 RID: 5858
		private bool dll_Excuted;

		// Token: 0x040016E3 RID: 5859
		private bool dll_Excuted;
	}
}
