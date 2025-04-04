using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004AE RID: 1198
	public class Joyce : NPC
	{
		// Token: 0x06001AAD RID: 6829 RVA: 0x00071103 File Offset: 0x0006F303
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JoyceAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JoyceAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x0007111C File Offset: 0x0006F31C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JoyceAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JoyceAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x00071135 File Offset: 0x0006F335
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00071143 File Offset: 0x0006F343
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016A0 RID: 5792
		private bool dll_Excuted;

		// Token: 0x040016A1 RID: 5793
		private bool dll_Excuted;
	}
}
