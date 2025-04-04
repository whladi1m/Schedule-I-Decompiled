using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x02000492 RID: 1170
	public class Carl : NPC
	{
		// Token: 0x06001A13 RID: 6675 RVA: 0x00070447 File Offset: 0x0006E647
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.CarlAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.CarlAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00070460 File Offset: 0x0006E660
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.CarlAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.CarlAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x00070479 File Offset: 0x0006E679
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x00070487 File Offset: 0x0006E687
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400165A RID: 5722
		private bool dll_Excuted;

		// Token: 0x0400165B RID: 5723
		private bool dll_Excuted;
	}
}
