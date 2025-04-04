using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200049F RID: 1183
	public class Geraldine : NPC
	{
		// Token: 0x06001A60 RID: 6752 RVA: 0x00070B2C File Offset: 0x0006ED2C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GeraldineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GeraldineAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00070B45 File Offset: 0x0006ED45
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GeraldineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GeraldineAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x00070B5E File Offset: 0x0006ED5E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x00070B6C File Offset: 0x0006ED6C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400167F RID: 5759
		private bool dll_Excuted;

		// Token: 0x04001680 RID: 5760
		private bool dll_Excuted;
	}
}
