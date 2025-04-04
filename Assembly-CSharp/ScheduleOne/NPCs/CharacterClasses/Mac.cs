using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004BB RID: 1211
	public class Mac : NPC
	{
		// Token: 0x06001AF1 RID: 6897 RVA: 0x00071595 File Offset: 0x0006F795
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MacAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MacAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000715AE File Offset: 0x0006F7AE
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MacAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MacAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000715C7 File Offset: 0x0006F7C7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000715D5 File Offset: 0x0006F7D5
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016BD RID: 5821
		private bool dll_Excuted;

		// Token: 0x040016BE RID: 5822
		private bool dll_Excuted;
	}
}
