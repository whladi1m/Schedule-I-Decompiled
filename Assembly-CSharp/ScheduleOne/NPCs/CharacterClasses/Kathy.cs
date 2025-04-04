using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B0 RID: 1200
	public class Kathy : NPC
	{
		// Token: 0x06001AB7 RID: 6839 RVA: 0x000711AB File Offset: 0x0006F3AB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KathyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KathyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x000711C4 File Offset: 0x0006F3C4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KathyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KathyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x000711DD File Offset: 0x0006F3DD
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000711EB File Offset: 0x0006F3EB
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016A4 RID: 5796
		private bool dll_Excuted;

		// Token: 0x040016A5 RID: 5797
		private bool dll_Excuted;
	}
}
