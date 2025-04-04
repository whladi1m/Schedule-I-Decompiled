using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004B7 RID: 1207
	public class Lisa : NPC
	{
		// Token: 0x06001ADD RID: 6877 RVA: 0x00071445 File Offset: 0x0006F645
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LisaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.LisaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x0007145E File Offset: 0x0006F65E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LisaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.LisaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x00071477 File Offset: 0x0006F677
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x00071485 File Offset: 0x0006F685
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016B5 RID: 5813
		private bool dll_Excuted;

		// Token: 0x040016B6 RID: 5814
		private bool dll_Excuted;
	}
}
