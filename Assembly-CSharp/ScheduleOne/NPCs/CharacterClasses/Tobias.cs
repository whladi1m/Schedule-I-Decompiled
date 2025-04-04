using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004CF RID: 1231
	public class Tobias : NPC
	{
		// Token: 0x06001B80 RID: 7040 RVA: 0x00072654 File Offset: 0x00070854
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.TobiasAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.TobiasAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0007266D File Offset: 0x0007086D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.TobiasAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.TobiasAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x00072686 File Offset: 0x00070886
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x00072694 File Offset: 0x00070894
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001701 RID: 5889
		private bool dll_Excuted;

		// Token: 0x04001702 RID: 5890
		private bool dll_Excuted;
	}
}
