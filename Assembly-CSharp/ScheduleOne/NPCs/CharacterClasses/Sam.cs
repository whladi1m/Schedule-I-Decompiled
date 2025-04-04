using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004CB RID: 1227
	public class Sam : NPC
	{
		// Token: 0x06001B57 RID: 6999 RVA: 0x0007203D File Offset: 0x0007023D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SamAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.SamAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x00072056 File Offset: 0x00070256
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SamAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.SamAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0007206F File Offset: 0x0007026F
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0007207D File Offset: 0x0007027D
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016EF RID: 5871
		private bool dll_Excuted;

		// Token: 0x040016F0 RID: 5872
		private bool dll_Excuted;
	}
}
