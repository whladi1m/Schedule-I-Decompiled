using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A9 RID: 1193
	public class Jen : NPC
	{
		// Token: 0x06001A92 RID: 6802 RVA: 0x00070E74 File Offset: 0x0006F074
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x00070E8D File Offset: 0x0006F08D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JenAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JenAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00070EA6 File Offset: 0x0006F0A6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x00070EB4 File Offset: 0x0006F0B4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001693 RID: 5779
		private bool dll_Excuted;

		// Token: 0x04001694 RID: 5780
		private bool dll_Excuted;
	}
}
