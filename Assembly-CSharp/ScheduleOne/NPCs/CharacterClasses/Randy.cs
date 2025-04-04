using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004C9 RID: 1225
	public class Randy : NPC
	{
		// Token: 0x06001B49 RID: 6985 RVA: 0x00071E82 File Offset: 0x00070082
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.RandyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.RandyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x00071E9B File Offset: 0x0007009B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.RandyAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.RandyAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x00071EB4 File Offset: 0x000700B4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x00071EC2 File Offset: 0x000700C2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016E4 RID: 5860
		private bool dll_Excuted;

		// Token: 0x040016E5 RID: 5861
		private bool dll_Excuted;
	}
}
