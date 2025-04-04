using System;

namespace ScheduleOne.Storage
{
	// Token: 0x02000885 RID: 2181
	public class DisposalStorageEntity : StorageEntity
	{
		// Token: 0x06003AF8 RID: 15096 RVA: 0x000F821A File Offset: 0x000F641A
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.DisposalStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.DisposalStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x000F8233 File Offset: 0x000F6433
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.DisposalStorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.DisposalStorageEntityAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x000F824C File Offset: 0x000F644C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x000F825A File Offset: 0x000F645A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002AE0 RID: 10976
		private bool dll_Excuted;

		// Token: 0x04002AE1 RID: 10977
		private bool dll_Excuted;
	}
}
