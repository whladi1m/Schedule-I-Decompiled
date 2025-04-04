using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000493 RID: 1171
	public class Charles : NPC
	{
		// Token: 0x06001A18 RID: 6680 RVA: 0x0007049B File Offset: 0x0006E69B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.CharlesAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.CharlesAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x000704B4 File Offset: 0x0006E6B4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.CharlesAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.CharlesAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000704CD File Offset: 0x0006E6CD
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x000704DB File Offset: 0x0006E6DB
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400165C RID: 5724
		private bool dll_Excuted;

		// Token: 0x0400165D RID: 5725
		private bool dll_Excuted;
	}
}
