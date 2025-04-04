using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A3 RID: 1187
	public class Igor : NPC
	{
		// Token: 0x06001A74 RID: 6772 RVA: 0x00070C7C File Offset: 0x0006EE7C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.IgorAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.IgorAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00070C95 File Offset: 0x0006EE95
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.IgorAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.IgorAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00070CAE File Offset: 0x0006EEAE
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x00070CBC File Offset: 0x0006EEBC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001687 RID: 5767
		private bool dll_Excuted;

		// Token: 0x04001688 RID: 5768
		private bool dll_Excuted;
	}
}
