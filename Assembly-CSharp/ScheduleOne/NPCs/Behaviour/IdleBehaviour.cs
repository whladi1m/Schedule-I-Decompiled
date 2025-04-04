using System;
using FishNet;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000515 RID: 1301
	public class IdleBehaviour : Behaviour
	{
		// Token: 0x06001F55 RID: 8021 RVA: 0x000803AD File Offset: 0x0007E5AD
		protected override void Begin()
		{
			base.Begin();
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x000803B5 File Offset: 0x0007E5B5
		protected override void Resume()
		{
			base.Resume();
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x000803C0 File Offset: 0x0007E5C0
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.IdlePoint == null)
			{
				return;
			}
			if (!base.Npc.Movement.IsMoving)
			{
				if (!base.Npc.Movement.IsAsCloseAsPossible(this.IdlePoint.position, 0.5f))
				{
					this.facingDir = false;
					base.Npc.Movement.SetDestination(this.IdlePoint.position);
					return;
				}
				if (!this.facingDir)
				{
					this.facingDir = true;
					base.Npc.Movement.FaceDirection(this.IdlePoint.forward, 0.5f);
					return;
				}
			}
			else
			{
				this.facingDir = false;
			}
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x00080478 File Offset: 0x0007E678
		protected override void Pause()
		{
			base.Pause();
			this.facingDir = false;
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.Stop();
			}
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0008049E File Offset: 0x0007E69E
		protected override void End()
		{
			base.End();
			this.facingDir = false;
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.Stop();
			}
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000804C4 File Offset: 0x0007E6C4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.IdleBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.IdleBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000804DD File Offset: 0x0007E6DD
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.IdleBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.IdleBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x000804F6 File Offset: 0x0007E6F6
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x00080504 File Offset: 0x0007E704
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400186F RID: 6255
		public Transform IdlePoint;

		// Token: 0x04001870 RID: 6256
		private bool facingDir;

		// Token: 0x04001871 RID: 6257
		private bool dll_Excuted;

		// Token: 0x04001872 RID: 6258
		private bool dll_Excuted;
	}
}
