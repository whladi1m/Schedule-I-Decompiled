using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A4 RID: 1188
	public class Jack : NPC
	{
		// Token: 0x06001A79 RID: 6777 RVA: 0x00070CD0 File Offset: 0x0006EED0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x00070CE9 File Offset: 0x0006EEE9
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JackAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JackAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x00070D02 File Offset: 0x0006EF02
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00070D10 File Offset: 0x0006EF10
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001689 RID: 5769
		private bool dll_Excuted;

		// Token: 0x0400168A RID: 5770
		private bool dll_Excuted;
	}
}
