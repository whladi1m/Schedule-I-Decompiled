using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000524 RID: 1316
	public class ScheduleBehaviour : Behaviour
	{
		// Token: 0x06002013 RID: 8211 RVA: 0x000842E9 File Offset: 0x000824E9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.ScheduleBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x000842FD File Offset: 0x000824FD
		protected override void Begin()
		{
			base.Begin();
			this.schedule.EnableSchedule();
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x00084310 File Offset: 0x00082510
		protected override void Resume()
		{
			base.Resume();
			this.schedule.EnableSchedule();
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x00084323 File Offset: 0x00082523
		protected override void Pause()
		{
			base.Pause();
			this.schedule.DisableSchedule();
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x00084336 File Offset: 0x00082536
		protected override void End()
		{
			base.End();
			this.schedule.DisableSchedule();
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x00084349 File Offset: 0x00082549
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.ScheduleBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.ScheduleBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x00084362 File Offset: 0x00082562
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.ScheduleBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.ScheduleBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0008437B File Offset: 0x0008257B
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x00084389 File Offset: 0x00082589
		protected virtual void dll()
		{
			base.Awake();
		}

		// Token: 0x040018E1 RID: 6369
		[Header("References")]
		public NPCScheduleManager schedule;

		// Token: 0x040018E2 RID: 6370
		private bool dll_Excuted;

		// Token: 0x040018E3 RID: 6371
		private bool dll_Excuted;
	}
}
