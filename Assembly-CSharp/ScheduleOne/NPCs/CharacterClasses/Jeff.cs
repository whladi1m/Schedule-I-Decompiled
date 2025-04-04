using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A8 RID: 1192
	public class Jeff : NPC
	{
		// Token: 0x06001A8D RID: 6797 RVA: 0x00070E20 File Offset: 0x0006F020
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JeffAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JeffAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x00070E39 File Offset: 0x0006F039
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JeffAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JeffAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x00070E52 File Offset: 0x0006F052
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x00070E60 File Offset: 0x0006F060
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001691 RID: 5777
		private bool dll_Excuted;

		// Token: 0x04001692 RID: 5778
		private bool dll_Excuted;
	}
}
