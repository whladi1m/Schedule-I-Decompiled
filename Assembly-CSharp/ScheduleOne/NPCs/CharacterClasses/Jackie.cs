using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A5 RID: 1189
	public class Jackie : NPC
	{
		// Token: 0x06001A7E RID: 6782 RVA: 0x00070D24 File Offset: 0x0006EF24
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JackieAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.JackieAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00070D3D File Offset: 0x0006EF3D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JackieAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.JackieAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00070D56 File Offset: 0x0006EF56
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x00070D64 File Offset: 0x0006EF64
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400168B RID: 5771
		private bool dll_Excuted;

		// Token: 0x0400168C RID: 5772
		private bool dll_Excuted;
	}
}
