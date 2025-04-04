using System;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004D8 RID: 1240
	public class AttendingDealBehaviour : Behaviour
	{
		// Token: 0x06001BB4 RID: 7092 RVA: 0x00072BD2 File Offset: 0x00070DD2
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.AttendingDealBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.AttendingDealBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x00072BEB File Offset: 0x00070DEB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.AttendingDealBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.AttendingDealBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x00072C04 File Offset: 0x00070E04
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x00072C12 File Offset: 0x00070E12
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001716 RID: 5910
		private bool dll_Excuted;

		// Token: 0x04001717 RID: 5911
		private bool dll_Excuted;
	}
}
