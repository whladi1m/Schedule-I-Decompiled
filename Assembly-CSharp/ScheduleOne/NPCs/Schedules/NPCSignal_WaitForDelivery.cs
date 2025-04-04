using System;
using FishNet;
using ScheduleOne.Economy;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x0200047A RID: 1146
	public class NPCSignal_WaitForDelivery : NPCSignal
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x0006E620 File Offset: 0x0006C820
		public new string ActionName
		{
			get
			{
				return "Wait for delivery";
			}
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0006E627 File Offset: 0x0006C827
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Schedules.NPCSignal_WaitForDelivery_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0006E63B File Offset: 0x0006C83B
		protected override void OnValidate()
		{
			base.OnValidate();
			this.priority = 100;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0006E64B File Offset: 0x0006C84B
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0006E653 File Offset: 0x0006C853
		public override void Started()
		{
			base.Started();
			base.SetDestination(this.Location.CustomerStandPoint.position, true);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0006E674 File Offset: 0x0006C874
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (this.npc.Movement.IsMoving)
			{
				if (Vector3.Distance(this.npc.Movement.CurrentDestination, this.Location.CustomerStandPoint.position) > 1.5f)
				{
					base.SetDestination(this.Location.CustomerStandPoint.position, true);
				}
				return;
			}
			if (!this.IsAtDestination())
			{
				base.SetDestination(this.Location.CustomerStandPoint.position, true);
				return;
			}
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0006E70E File Offset: 0x0006C90E
		public override void LateStarted()
		{
			base.LateStarted();
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			base.SetDestination(this.Location.CustomerStandPoint.position, true);
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0006E746 File Offset: 0x0006C946
		public override void JumpTo()
		{
			base.JumpTo();
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			base.SetDestination(this.Location.CustomerStandPoint.position, true);
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0006E77E File Offset: 0x0006C97E
		public override void Interrupt()
		{
			base.Interrupt();
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(false);
			this.npc.Movement.Stop();
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0006E7A7 File Offset: 0x0006C9A7
		public override void Resume()
		{
			base.Resume();
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0006E7C0 File Offset: 0x0006C9C0
		public override void End()
		{
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(false);
			base.StartedThisCycle = false;
			base.End();
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0006E7E0 File Offset: 0x0006C9E0
		public override void Skipped()
		{
			base.Skipped();
			if (InstanceFinder.IsServer)
			{
				this.npc.Movement.Warp(this.Location.CustomerStandPoint.position);
			}
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0006E80F File Offset: 0x0006CA0F
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.Location.CustomerStandPoint.position) < 1.5f;
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0006E840 File Offset: 0x0006CA40
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				return;
			}
			this.npc.Movement.FaceDirection(this.Location.CustomerStandPoint.forward, 0.5f);
			this.npc.GetComponent<Customer>().SetIsAwaitingDelivery(true);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0006E898 File Offset: 0x0006CA98
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_WaitForDeliveryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_WaitForDeliveryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0006E8B1 File Offset: 0x0006CAB1
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_WaitForDeliveryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_WaitForDeliveryAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0006E8CA File Offset: 0x0006CACA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0006E8D8 File Offset: 0x0006CAD8
		protected virtual void dll()
		{
			base.Awake();
			this.priority = 1000;
			this.MaxDuration = 720;
		}

		// Token: 0x04001602 RID: 5634
		public const float DESTINATION_THRESHOLD = 1.5f;

		// Token: 0x04001603 RID: 5635
		public DeliveryLocation Location;

		// Token: 0x04001604 RID: 5636
		private bool dll_Excuted;

		// Token: 0x04001605 RID: 5637
		private bool dll_Excuted;
	}
}
