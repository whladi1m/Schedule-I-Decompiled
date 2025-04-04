using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B3 RID: 1203
	public class Kim : NPC
	{
		// Token: 0x06001AC6 RID: 6854 RVA: 0x000712A7 File Offset: 0x0006F4A7
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KimAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.KimAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x000712C0 File Offset: 0x0006F4C0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KimAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.KimAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x000712D9 File Offset: 0x0006F4D9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000712E7 File Offset: 0x0006F4E7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016AA RID: 5802
		private bool dll_Excuted;

		// Token: 0x040016AB RID: 5803
		private bool dll_Excuted;
	}
}
