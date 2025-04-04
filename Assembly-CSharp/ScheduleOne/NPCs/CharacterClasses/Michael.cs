using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004BE RID: 1214
	public class Michael : NPC
	{
		// Token: 0x06001B0B RID: 6923 RVA: 0x00071987 File Offset: 0x0006FB87
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MichaelAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MichaelAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x000719A0 File Offset: 0x0006FBA0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MichaelAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MichaelAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000719B9 File Offset: 0x0006FBB9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000719C7 File Offset: 0x0006FBC7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016C8 RID: 5832
		private bool dll_Excuted;

		// Token: 0x040016C9 RID: 5833
		private bool dll_Excuted;
	}
}
