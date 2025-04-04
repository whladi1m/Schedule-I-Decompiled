using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B1 RID: 1201
	public class Keith : NPC
	{
		// Token: 0x06001ABC RID: 6844 RVA: 0x000711FF File Offset: 0x0006F3FF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KeithAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KeithAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x00071218 File Offset: 0x0006F418
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KeithAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KeithAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00071231 File Offset: 0x0006F431
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0007123F File Offset: 0x0006F43F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016A6 RID: 5798
		private bool dll_Excuted;

		// Token: 0x040016A7 RID: 5799
		private bool dll_Excuted;
	}
}
