using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004C6 RID: 1222
	public class Peggy : NPC
	{
		// Token: 0x06001B3A RID: 6970 RVA: 0x00071D86 File Offset: 0x0006FF86
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PeggyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.PeggyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x00071D9F File Offset: 0x0006FF9F
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PeggyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.PeggyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x00071DB8 File Offset: 0x0006FFB8
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00071DC6 File Offset: 0x0006FFC6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016DE RID: 5854
		private bool dll_Excuted;

		// Token: 0x040016DF RID: 5855
		private bool dll_Excuted;
	}
}
