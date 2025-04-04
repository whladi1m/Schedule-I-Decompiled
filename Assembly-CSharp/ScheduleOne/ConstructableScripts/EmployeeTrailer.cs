using System;

namespace ScheduleOne.ConstructableScripts
{
	// Token: 0x02000911 RID: 2321
	public class EmployeeTrailer : Constructable_GridBased
	{
		// Token: 0x06003EE9 RID: 16105 RVA: 0x0010994A File Offset: 0x00107B4A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.EmployeeTrailerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ConstructableScripts.EmployeeTrailerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x00109963 File Offset: 0x00107B63
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ConstructableScripts.EmployeeTrailerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ConstructableScripts.EmployeeTrailerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x0010997C File Offset: 0x00107B7C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x0010998A File Offset: 0x00107B8A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002D66 RID: 11622
		private bool dll_Excuted;

		// Token: 0x04002D67 RID: 11623
		private bool dll_Excuted;
	}
}
