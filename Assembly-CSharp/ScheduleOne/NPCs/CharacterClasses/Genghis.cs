using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200049D RID: 1181
	public class Genghis : NPC
	{
		// Token: 0x06001A56 RID: 6742 RVA: 0x00070A84 File Offset: 0x0006EC84
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GenghisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GenghisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00070A9D File Offset: 0x0006EC9D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GenghisAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GenghisAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00070AB6 File Offset: 0x0006ECB6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00070AC4 File Offset: 0x0006ECC4
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400167B RID: 5755
		private bool dll_Excuted;

		// Token: 0x0400167C RID: 5756
		private bool dll_Excuted;
	}
}
