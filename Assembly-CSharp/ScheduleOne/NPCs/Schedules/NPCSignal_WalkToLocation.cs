using System;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x0200047B RID: 1147
	public class NPCSignal_WalkToLocation : NPCSignal
	{
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x0006E8F6 File Offset: 0x0006CAF6
		public new string ActionName
		{
			get
			{
				return "Walk to location";
			}
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0006E8FD File Offset: 0x0006CAFD
		public override string GetName()
		{
			return this.ActionName + " (" + this.Destination.name + ")";
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0006E91F File Offset: 0x0006CB1F
		public override void Started()
		{
			base.Started();
			base.SetDestination(this.Destination.position, true);
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0006E939 File Offset: 0x0006CB39
		public override void ActiveUpdate()
		{
			base.ActiveUpdate();
			if (!this.npc.Movement.IsMoving && !this.IsAtDestination())
			{
				base.SetDestination(this.Destination.position, true);
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x0006DBFC File Offset: 0x0006BDFC
		public override void LateStarted()
		{
			base.LateStarted();
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x0006E96D File Offset: 0x0006CB6D
		public override void Interrupt()
		{
			base.Interrupt();
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0006DC58 File Offset: 0x0006BE58
		public override void Resume()
		{
			base.Resume();
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0006E997 File Offset: 0x0006CB97
		public override void Skipped()
		{
			base.Skipped();
			if (this.WarpIfSkipped)
			{
				this.npc.Movement.Warp(this.Destination.position);
			}
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0006E9C2 File Offset: 0x0006CBC2
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.Destination.position) < this.DestinationThreshold;
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0006E9EC File Offset: 0x0006CBEC
		protected override void WalkCallback(NPCMovement.WalkResult result)
		{
			base.WalkCallback(result);
			if (!base.IsActive)
			{
				return;
			}
			if (result != NPCMovement.WalkResult.Success)
			{
				Debug.LogWarning("NPC walk to location not successful");
				return;
			}
			this.ReachedDestination();
			this.End();
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x0006EA19 File Offset: 0x0006CC19
		[ObserversRpc]
		private void ReachedDestination()
		{
			this.RpcWriter___Observers_ReachedDestination_2166136261();
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0006EA3B File Offset: 0x0006CC3B
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_WalkToLocationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_WalkToLocationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_ReachedDestination_2166136261));
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0006EA6B File Offset: 0x0006CC6B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_WalkToLocationAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_WalkToLocationAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0006EA84 File Offset: 0x0006CC84
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0006EA94 File Offset: 0x0006CC94
		private void RpcWriter___Observers_ReachedDestination_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0006EB3D File Offset: 0x0006CD3D
		private void RpcLogic___ReachedDestination_2166136261()
		{
			if (this.FaceDestinationDir)
			{
				this.npc.Movement.FaceDirection(this.Destination.forward, 0.5f);
			}
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0006EB68 File Offset: 0x0006CD68
		private void RpcReader___Observers_ReachedDestination_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReachedDestination_2166136261();
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0006EB88 File Offset: 0x0006CD88
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001606 RID: 5638
		public Transform Destination;

		// Token: 0x04001607 RID: 5639
		public bool FaceDestinationDir = true;

		// Token: 0x04001608 RID: 5640
		public float DestinationThreshold = 1f;

		// Token: 0x04001609 RID: 5641
		public bool WarpIfSkipped;

		// Token: 0x0400160A RID: 5642
		private bool dll_Excuted;

		// Token: 0x0400160B RID: 5643
		private bool dll_Excuted;
	}
}
