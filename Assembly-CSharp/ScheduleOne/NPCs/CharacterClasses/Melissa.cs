using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004BD RID: 1213
	public class Melissa : NPC
	{
		// Token: 0x06001B06 RID: 6918 RVA: 0x00071933 File Offset: 0x0006FB33
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MelissaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MelissaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0007194C File Offset: 0x0006FB4C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MelissaAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MelissaAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x00071965 File Offset: 0x0006FB65
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x00071973 File Offset: 0x0006FB73
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016C6 RID: 5830
		private bool dll_Excuted;

		// Token: 0x040016C7 RID: 5831
		private bool dll_Excuted;
	}
}
