using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000497 RID: 1175
	public class Dennis : NPC
	{
		// Token: 0x06001A30 RID: 6704 RVA: 0x000706BD File Offset: 0x0006E8BD
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DennisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.DennisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x000706D6 File Offset: 0x0006E8D6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DennisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.DennisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x000706EF File Offset: 0x0006E8EF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x000706FD File Offset: 0x0006E8FD
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001667 RID: 5735
		private bool dll_Excuted;

		// Token: 0x04001668 RID: 5736
		private bool dll_Excuted;
	}
}
