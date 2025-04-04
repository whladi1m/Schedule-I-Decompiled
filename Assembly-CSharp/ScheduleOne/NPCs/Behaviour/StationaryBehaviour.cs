using System;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000526 RID: 1318
	public class StationaryBehaviour : Behaviour
	{
		// Token: 0x0600202F RID: 8239 RVA: 0x0007EBB7 File Offset: 0x0007CDB7
		protected override void Begin()
		{
			base.Begin();
			base.Npc.Movement.Stop();
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x00084628 File Offset: 0x00082828
		protected override void Resume()
		{
			base.Resume();
			base.Npc.Movement.Stop();
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x00084640 File Offset: 0x00082840
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StationaryBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StationaryBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x00084659 File Offset: 0x00082859
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StationaryBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StationaryBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x00084672 File Offset: 0x00082872
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00084680 File Offset: 0x00082880
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040018EE RID: 6382
		private bool dll_Excuted;

		// Token: 0x040018EF RID: 6383
		private bool dll_Excuted;
	}
}
