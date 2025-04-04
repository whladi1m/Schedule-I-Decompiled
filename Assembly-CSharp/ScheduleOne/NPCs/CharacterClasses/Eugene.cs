using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000499 RID: 1177
	public class Eugene : NPC
	{
		// Token: 0x06001A3A RID: 6714 RVA: 0x00070765 File Offset: 0x0006E965
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.EugeneAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.EugeneAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0007077E File Offset: 0x0006E97E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.EugeneAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.EugeneAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00070797 File Offset: 0x0006E997
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x000707A5 File Offset: 0x0006E9A5
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400166B RID: 5739
		private bool dll_Excuted;

		// Token: 0x0400166C RID: 5740
		private bool dll_Excuted;
	}
}
