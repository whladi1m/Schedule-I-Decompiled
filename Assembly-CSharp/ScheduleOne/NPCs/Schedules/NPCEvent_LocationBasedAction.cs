using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000469 RID: 1129
	public class NPCEvent_LocationBasedAction : NPCEvent
	{
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001830 RID: 6192 RVA: 0x0006A8D4 File Offset: 0x00068AD4
		public new string ActionName
		{
			get
			{
				return "Location-based action";
			}
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0006A8DC File Offset: 0x00068ADC
		public override string GetName()
		{
			if (this.Destination == null)
			{
				return this.ActionName + " (No destination set)";
			}
			return this.ActionName + " (" + this.Destination.name + ")";
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0006A928 File Offset: 0x00068B28
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (!base.IsActive)
			{
				return;
			}
			if (this.IsActionStarted)
			{
				this.StartAction(connection);
			}
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0006A949 File Offset: 0x00068B49
		public override void Started()
		{
			base.Started();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.Destination.position, true);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0006A974 File Offset: 0x00068B74
		public override void ActiveMinPassed()
		{
			base.ActiveMinPassed();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.npc.Movement.IsMoving)
			{
				if (Vector3.Distance(this.npc.Movement.CurrentDestination, this.Destination.position) > this.DestinationThreshold)
				{
					base.SetDestination(this.Destination.position, true);
					return;
				}
			}
			else if (!this.IsAtDestination())
			{
				base.SetDestination(this.Destination.position, true);
			}
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0006A9F6 File Offset: 0x00068BF6
		public override void LateStarted()
		{
			base.LateStarted();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.Destination.position, true);
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0006AA20 File Offset: 0x00068C20
		public override void JumpTo()
		{
			base.JumpTo();
			if (!this.IsAtDestination())
			{
				if (this.npc.Movement.IsMoving)
				{
					this.npc.Movement.Stop();
				}
				if (InstanceFinder.IsServer)
				{
					this.npc.Movement.Warp(this.Destination.position);
				}
			}
			if (InstanceFinder.IsServer)
			{
				this.StartAction(null);
			}
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x0006AA8D File Offset: 0x00068C8D
		public override void End()
		{
			base.End();
			if (this.IsActionStarted)
			{
				this.EndAction();
			}
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x0006AAA3 File Offset: 0x00068CA3
		public override void Interrupt()
		{
			base.Interrupt();
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			if (this.IsActionStarted)
			{
				this.EndAction();
			}
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x0006AADB File Offset: 0x00068CDB
		public override void Resume()
		{
			base.Resume();
			if (this.IsAtDestination())
			{
				this.WalkCallback(NPCMovement.WalkResult.Success);
				return;
			}
			base.SetDestination(this.Destination.position, true);
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x0006AB05 File Offset: 0x00068D05
		public override void Skipped()
		{
			base.Skipped();
			if (this.WarpIfSkipped)
			{
				this.npc.Movement.Warp(this.Destination.position);
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x0006AB30 File Offset: 0x00068D30
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.Destination.position) < this.DestinationThreshold;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0006AB5A File Offset: 0x00068D5A
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
			if (InstanceFinder.IsServer)
			{
				this.StartAction(null);
			}
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0006AB80 File Offset: 0x00068D80
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		protected virtual void StartAction(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_StartAction_328543758(conn);
				this.RpcLogic___StartAction_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_StartAction_328543758(conn);
			}
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0006ABB5 File Offset: 0x00068DB5
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndAction()
		{
			this.RpcWriter___Observers_EndAction_2166136261();
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0006ABE0 File Offset: 0x00068DE0
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_LocationBasedActionAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_LocationBasedActionAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartAction_328543758));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_StartAction_328543758));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EndAction_2166136261));
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0006AC49 File Offset: 0x00068E49
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_LocationBasedActionAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_LocationBasedActionAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0006AC62 File Offset: 0x00068E62
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0006AC70 File Offset: 0x00068E70
		private void RpcWriter___Observers_StartAction_328543758(NetworkConnection conn)
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

		// Token: 0x06001844 RID: 6212 RVA: 0x0006AD1C File Offset: 0x00068F1C
		protected virtual void RpcLogic___StartAction_328543758(NetworkConnection conn)
		{
			if (this.IsActionStarted)
			{
				return;
			}
			if (this.FaceDestinationDir)
			{
				this.npc.Movement.FaceDirection(this.Destination.forward, 0.5f);
			}
			this.IsActionStarted = true;
			if (this.onStartAction != null)
			{
				this.onStartAction.Invoke();
			}
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x0006AD74 File Offset: 0x00068F74
		private void RpcReader___Observers_StartAction_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartAction_328543758(null);
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0006ADA0 File Offset: 0x00068FA0
		private void RpcWriter___Target_StartAction_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(1U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0006AE48 File Offset: 0x00069048
		private void RpcReader___Target_StartAction_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___StartAction_328543758(base.LocalConnection);
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0006AE70 File Offset: 0x00069070
		private void RpcWriter___Observers_EndAction_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0006AF19 File Offset: 0x00069119
		protected virtual void RpcLogic___EndAction_2166136261()
		{
			if (!this.IsActionStarted)
			{
				return;
			}
			this.IsActionStarted = false;
			if (this.onEndAction != null)
			{
				this.onEndAction.Invoke();
			}
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0006AF40 File Offset: 0x00069140
		private void RpcReader___Observers_EndAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0006AF6A File Offset: 0x0006916A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040015A0 RID: 5536
		public Transform Destination;

		// Token: 0x040015A1 RID: 5537
		public bool FaceDestinationDir = true;

		// Token: 0x040015A2 RID: 5538
		public float DestinationThreshold = 1f;

		// Token: 0x040015A3 RID: 5539
		public bool WarpIfSkipped;

		// Token: 0x040015A4 RID: 5540
		public bool IsActionStarted;

		// Token: 0x040015A5 RID: 5541
		public UnityEvent onStartAction;

		// Token: 0x040015A6 RID: 5542
		public UnityEvent onEndAction;

		// Token: 0x040015A7 RID: 5543
		private bool dll_Excuted;

		// Token: 0x040015A8 RID: 5544
		private bool dll_Excuted;
	}
}
