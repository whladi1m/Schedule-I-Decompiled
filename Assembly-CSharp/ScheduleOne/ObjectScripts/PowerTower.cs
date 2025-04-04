using System;
using ScheduleOne.ConstructableScripts;

namespace ScheduleOne.ObjectScripts
{
	// Token: 0x02000BC4 RID: 3012
	public class PowerTower : Constructable_GridBased
	{
		// Token: 0x06005496 RID: 21654 RVA: 0x001643C2 File Offset: 0x001625C2
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PowerTowerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.ObjectScripts.PowerTowerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x001643DB File Offset: 0x001625DB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.ObjectScripts.PowerTowerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.ObjectScripts.PowerTowerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x001643F4 File Offset: 0x001625F4
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x00164402 File Offset: 0x00162602
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04003EBB RID: 16059
		private bool dll_Excuted;

		// Token: 0x04003EBC RID: 16060
		private bool dll_Excuted;
	}
}
