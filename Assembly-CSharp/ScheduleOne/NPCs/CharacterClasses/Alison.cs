using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200048D RID: 1165
	public class Alison : NPC
	{
		// Token: 0x060019F8 RID: 6648 RVA: 0x00070262 File Offset: 0x0006E462
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AlisonAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AlisonAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0007027B File Offset: 0x0006E47B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AlisonAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AlisonAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00070294 File Offset: 0x0006E494
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x000702A2 File Offset: 0x0006E4A2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001650 RID: 5712
		private bool dll_Excuted;

		// Token: 0x04001651 RID: 5713
		private bool dll_Excuted;
	}
}
