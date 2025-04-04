using System;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004BF RID: 1215
	public class Mick : NPC
	{
		// Token: 0x06001B10 RID: 6928 RVA: 0x000719DB File Offset: 0x0006FBDB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MickAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MickAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000719F4 File Offset: 0x0006FBF4
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MickAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MickAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x00071A0D File Offset: 0x0006FC0D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x00071A1B File Offset: 0x0006FC1B
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016CA RID: 5834
		private bool dll_Excuted;

		// Token: 0x040016CB RID: 5835
		private bool dll_Excuted;
	}
}
