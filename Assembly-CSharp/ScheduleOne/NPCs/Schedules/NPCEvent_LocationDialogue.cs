using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Dialogue;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x0200046A RID: 1130
	public class NPCEvent_LocationDialogue : NPCEvent
	{
		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x0006AF7E File Offset: 0x0006917E
		public new string ActionName
		{
			get
			{
				return "Location-based dialogue";
			}
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x0006AF88 File Offset: 0x00069188
		public override string GetName()
		{
			if (this.Destination == null)
			{
				return this.ActionName + " (No destination set)";
			}
			string actionName = this.ActionName;
			string str = " (";
			Transform destination = this.Destination;
			return actionName + str + ((destination != null) ? destination.name : null) + ")";
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x0006AFDB File Offset: 0x000691DB
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

		// Token: 0x0600184F RID: 6223 RVA: 0x0006AFFC File Offset: 0x000691FC
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

		// Token: 0x06001850 RID: 6224 RVA: 0x0006B028 File Offset: 0x00069228
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
			else if (this.IsAtDestination())
			{
				if (this.FaceDestinationDir && !this.npc.Movement.FaceDirectionInProgress && Vector3.Angle(base.transform.forward, this.Destination.forward) > 5f)
				{
					this.npc.Movement.FaceDirection(this.Destination.forward, 0.5f);
					return;
				}
			}
			else
			{
				base.SetDestination(this.Destination.position, true);
			}
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0006B10A File Offset: 0x0006930A
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

		// Token: 0x06001852 RID: 6226 RVA: 0x0006B134 File Offset: 0x00069334
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
				this.WalkCallback(NPCMovement.WalkResult.Success);
			}
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0006B1A1 File Offset: 0x000693A1
		public override void End()
		{
			base.End();
			if (this.IsActionStarted)
			{
				this.EndAction();
			}
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x0006B1B7 File Offset: 0x000693B7
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

		// Token: 0x06001855 RID: 6229 RVA: 0x0006B1EF File Offset: 0x000693EF
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

		// Token: 0x06001856 RID: 6230 RVA: 0x0006B219 File Offset: 0x00069419
		public override void Skipped()
		{
			base.Skipped();
			if (this.WarpIfSkipped)
			{
				this.npc.Movement.Warp(this.Destination.position);
			}
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x0006B244 File Offset: 0x00069444
		private bool IsAtDestination()
		{
			return Vector3.Distance(this.npc.Movement.FootPosition, this.Destination.position) < this.DestinationThreshold;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0006B26E File Offset: 0x0006946E
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

		// Token: 0x06001859 RID: 6233 RVA: 0x0006B294 File Offset: 0x00069494
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

		// Token: 0x0600185A RID: 6234 RVA: 0x0006B2CC File Offset: 0x000694CC
		[ObserversRpc(RunLocally = true)]
		protected virtual void EndAction()
		{
			this.RpcWriter___Observers_EndAction_2166136261();
			this.RpcLogic___EndAction_2166136261();
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x0006B310 File Offset: 0x00069510
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_LocationDialogueAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCEvent_LocationDialogueAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartAction_328543758));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_StartAction_328543758));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EndAction_2166136261));
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0006B379 File Offset: 0x00069579
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_LocationDialogueAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCEvent_LocationDialogueAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0006B392 File Offset: 0x00069592
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0006B3A0 File Offset: 0x000695A0
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

		// Token: 0x06001860 RID: 6240 RVA: 0x0006B44C File Offset: 0x0006964C
		protected virtual void RpcLogic___StartAction_328543758(NetworkConnection conn)
		{
			if (this.IsActionStarted)
			{
				Console.LogWarning("Dialogue action already started", null);
				return;
			}
			if (this.FaceDestinationDir)
			{
				this.npc.Movement.FaceDirection(this.Destination.forward, 0.5f);
			}
			this.IsActionStarted = true;
			DialogueController component = this.npc.dialogueHandler.GetComponent<DialogueController>();
			if (this.DialogueOverride != null)
			{
				component.OverrideContainer = this.DialogueOverride;
				return;
			}
			component.OverrideContainer = null;
			if (component.GreetingOverrides.Count > this.GreetingOverrideToEnable && this.GreetingOverrideToEnable >= 0)
			{
				component.GreetingOverrides[this.GreetingOverrideToEnable].ShouldShow = true;
			}
			if (component.Choices.Count > this.ChoiceToEnable && this.ChoiceToEnable >= 0)
			{
				component.Choices[this.ChoiceToEnable].Enabled = true;
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x0006B538 File Offset: 0x00069738
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

		// Token: 0x06001862 RID: 6242 RVA: 0x0006B564 File Offset: 0x00069764
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

		// Token: 0x06001863 RID: 6243 RVA: 0x0006B60C File Offset: 0x0006980C
		private void RpcReader___Target_StartAction_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___StartAction_328543758(base.LocalConnection);
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0006B634 File Offset: 0x00069834
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

		// Token: 0x06001865 RID: 6245 RVA: 0x0006B6E0 File Offset: 0x000698E0
		protected virtual void RpcLogic___EndAction_2166136261()
		{
			if (!this.IsActionStarted)
			{
				return;
			}
			this.IsActionStarted = false;
			DialogueController component = this.npc.dialogueHandler.GetComponent<DialogueController>();
			if (this.DialogueOverride != null)
			{
				component.OverrideContainer = null;
				return;
			}
			if (component.GreetingOverrides.Count > this.GreetingOverrideToEnable && this.GreetingOverrideToEnable >= 0)
			{
				component.GreetingOverrides[this.GreetingOverrideToEnable].ShouldShow = false;
			}
			if (component.Choices.Count > this.ChoiceToEnable && this.ChoiceToEnable >= 0)
			{
				component.Choices[this.ChoiceToEnable].Enabled = false;
			}
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0006B78C File Offset: 0x0006998C
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

		// Token: 0x06001867 RID: 6247 RVA: 0x0006B7B6 File Offset: 0x000699B6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040015A9 RID: 5545
		public Transform Destination;

		// Token: 0x040015AA RID: 5546
		public bool FaceDestinationDir = true;

		// Token: 0x040015AB RID: 5547
		public float DestinationThreshold = 1f;

		// Token: 0x040015AC RID: 5548
		public bool WarpIfSkipped;

		// Token: 0x040015AD RID: 5549
		[Header("Dialogue Settings")]
		public int GreetingOverrideToEnable = -1;

		// Token: 0x040015AE RID: 5550
		public int ChoiceToEnable = -1;

		// Token: 0x040015AF RID: 5551
		public DialogueContainer DialogueOverride;

		// Token: 0x040015B0 RID: 5552
		protected bool IsActionStarted;

		// Token: 0x040015B1 RID: 5553
		private bool dll_Excuted;

		// Token: 0x040015B2 RID: 5554
		private bool dll_Excuted;
	}
}
