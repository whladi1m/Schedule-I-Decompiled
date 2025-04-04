using System;
using ScheduleOne.Property;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004C0 RID: 1216
	public class Ming : NPC
	{
		// Token: 0x06001B14 RID: 6932 RVA: 0x00060907 File Offset: 0x0005EB07
		public override string GetNameAddress()
		{
			return base.fullName;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x00071A2F File Offset: 0x0006FC2F
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.MingAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x00071A48 File Offset: 0x0006FC48
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.MingAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x00071A61 File Offset: 0x0006FC61
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x00071A6F File Offset: 0x0006FC6F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016CC RID: 5836
		public Property Property;

		// Token: 0x040016CD RID: 5837
		private bool dll_Excuted;

		// Token: 0x040016CE RID: 5838
		private bool dll_Excuted;
	}
}
