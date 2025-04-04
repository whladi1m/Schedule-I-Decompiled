using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004C5 RID: 1221
	public class Pearl : NPC
	{
		// Token: 0x06001B35 RID: 6965 RVA: 0x00071D32 File Offset: 0x0006FF32
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PearlAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PearlAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x00071D4B File Offset: 0x0006FF4B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PearlAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PearlAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00071D64 File Offset: 0x0006FF64
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x00071D72 File Offset: 0x0006FF72
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016DC RID: 5852
		private bool dll_Excuted;

		// Token: 0x040016DD RID: 5853
		private bool dll_Excuted;
	}
}
