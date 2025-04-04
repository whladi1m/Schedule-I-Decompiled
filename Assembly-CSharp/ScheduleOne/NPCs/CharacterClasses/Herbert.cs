using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004A2 RID: 1186
	public class Herbert : NPC
	{
		// Token: 0x06001A6F RID: 6767 RVA: 0x00070C28 File Offset: 0x0006EE28
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.HerbertAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.HerbertAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00070C41 File Offset: 0x0006EE41
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.HerbertAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.HerbertAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00070C5A File Offset: 0x0006EE5A
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00070C68 File Offset: 0x0006EE68
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001685 RID: 5765
		private bool dll_Excuted;

		// Token: 0x04001686 RID: 5766
		private bool dll_Excuted;
	}
}
