using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A0 RID: 1184
	public class Greg : NPC
	{
		// Token: 0x06001A65 RID: 6757 RVA: 0x00070B80 File Offset: 0x0006ED80
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GregAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.GregAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x00070B99 File Offset: 0x0006ED99
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GregAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.GregAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00070BB2 File Offset: 0x0006EDB2
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x00070BC0 File Offset: 0x0006EDC0
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001681 RID: 5761
		private bool dll_Excuted;

		// Token: 0x04001682 RID: 5762
		private bool dll_Excuted;
	}
}
