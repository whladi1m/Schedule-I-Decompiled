using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x0200048F RID: 1167
	public class Austin : NPC
	{
		// Token: 0x06001A04 RID: 6660 RVA: 0x00070343 File Offset: 0x0006E543
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AustinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.AustinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0007035C File Offset: 0x0006E55C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AustinAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.AustinAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x00070375 File Offset: 0x0006E575
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00070383 File Offset: 0x0006E583
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001654 RID: 5716
		private bool dll_Excuted;

		// Token: 0x04001655 RID: 5717
		private bool dll_Excuted;
	}
}
