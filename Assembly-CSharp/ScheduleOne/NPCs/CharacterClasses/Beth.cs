using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000490 RID: 1168
	public class Beth : NPC
	{
		// Token: 0x06001A09 RID: 6665 RVA: 0x00070397 File Offset: 0x0006E597
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.BethAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.BethAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x000703B0 File Offset: 0x0006E5B0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.BethAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.BethAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x000703C9 File Offset: 0x0006E5C9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x000703D7 File Offset: 0x0006E5D7
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001656 RID: 5718
		private bool dll_Excuted;

		// Token: 0x04001657 RID: 5719
		private bool dll_Excuted;
	}
}
