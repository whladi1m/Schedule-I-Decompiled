using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x0200050B RID: 1291
	public class StopDryingRackBehaviour : Behaviour
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x0007E5A2 File Offset: 0x0007C7A2
		// (set) Token: 0x06001ECD RID: 7885 RVA: 0x0007E5AA File Offset: 0x0007C7AA
		public DryingRack Rack { get; protected set; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x0007E5B3 File Offset: 0x0007C7B3
		// (set) Token: 0x06001ECF RID: 7887 RVA: 0x0007E5BB File Offset: 0x0007C7BB
		public bool WorkInProgress { get; protected set; }

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0007E5C4 File Offset: 0x0007C7C4
		protected override void Begin()
		{
			base.Begin();
			this.StartWork();
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0007E5D2 File Offset: 0x0007C7D2
		protected override void Resume()
		{
			base.Resume();
			this.StartWork();
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0007E5E0 File Offset: 0x0007C7E0
		protected override void Pause()
		{
			base.Pause();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0007E5F8 File Offset: 0x0007C7F8
		protected override void End()
		{
			base.End();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
			if (InstanceFinder.IsServer && this.Rack != null && this.Rack.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Rack.SetNPCUser(null);
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x0007E658 File Offset: 0x0007C858
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.WorkInProgress)
			{
				if (this.IsRackReady(this.Rack))
				{
					if (base.Npc.Movement.IsMoving)
					{
						return;
					}
					if (this.IsAtStation())
					{
						this.BeginAction();
						return;
					}
					this.GoToStation();
					return;
				}
				else
				{
					base.Disable_Networked(null);
				}
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0007E6BC File Offset: 0x0007C8BC
		private void StartWork()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsRackReady(this.Rack))
			{
				Console.LogWarning(base.Npc.fullName + " has no station to work with", null);
				base.Disable_Networked(null);
				return;
			}
			this.Rack.SetNPCUser(base.Npc.NetworkObject);
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0007E718 File Offset: 0x0007C918
		public void AssignRack(DryingRack rack)
		{
			this.Rack = rack;
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0007E721 File Offset: 0x0007C921
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(NavMeshUtility.GetAccessPoint(this.Rack, base.Npc).position, 0.5f);
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0007E74E File Offset: 0x0007C94E
		public void GoToStation()
		{
			base.Npc.Movement.SetDestination(NavMeshUtility.GetAccessPoint(this.Rack, base.Npc).position);
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x0007E778 File Offset: 0x0007C978
		[ObserversRpc(RunLocally = true)]
		public void BeginAction()
		{
			this.RpcWriter___Observers_BeginAction_2166136261();
			this.RpcLogic___BeginAction_2166136261();
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x0007E791 File Offset: 0x0007C991
		private void StopCauldron()
		{
			if (this.workRoutine != null)
			{
				base.StopCoroutine(this.workRoutine);
			}
			this.WorkInProgress = false;
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x0007E7B0 File Offset: 0x0007C9B0
		public bool IsRackReady(DryingRack rack)
		{
			if (rack == null)
			{
				return false;
			}
			if (((IUsable)rack).IsInUse && (rack.PlayerUserObject != null || rack.NPCUserObject != base.Npc.NetworkObject))
			{
				return false;
			}
			List<DryingOperation> operationsAtTargetQuality = rack.GetOperationsAtTargetQuality();
			bool flag = false;
			foreach (DryingOperation dryingOperation in operationsAtTargetQuality)
			{
				if (rack.GetOutputCapacityForOperation(dryingOperation, dryingOperation.GetQuality()) > 0)
				{
					flag = true;
				}
			}
			return flag && base.Npc.Movement.CanGetTo(rack.transform.position, 1f);
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x0007E878 File Offset: 0x0007CA78
		[CompilerGenerated]
		private IEnumerator <BeginAction>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			base.Npc.Avatar.Anim.SetTrigger("GrabItem");
			yield return new WaitForSeconds(0.5f);
			if (InstanceFinder.IsServer)
			{
				DryingOperation dryingOperation = this.Rack.GetOperationsAtTargetQuality().FirstOrDefault((DryingOperation x) => this.Rack.GetOutputCapacityForOperation(x, x.GetQuality()) > 0);
				if (dryingOperation != null)
				{
					this.Rack.TryEndOperation(this.Rack.DryingOperations.IndexOf(dryingOperation), true, dryingOperation.GetQuality(), UnityEngine.Random.Range(int.MinValue, int.MaxValue));
				}
			}
			this.WorkInProgress = false;
			this.workRoutine = null;
			yield break;
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x0007E89E File Offset: 0x0007CA9E
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StopDryingRackBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StopDryingRackBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginAction_2166136261));
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0007E8CE File Offset: 0x0007CACE
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StopDryingRackBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StopDryingRackBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x0007E8E7 File Offset: 0x0007CAE7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x0007E8F8 File Offset: 0x0007CAF8
		private void RpcWriter___Observers_BeginAction_2166136261()
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
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x0007E9A4 File Offset: 0x0007CBA4
		public void RpcLogic___BeginAction_2166136261()
		{
			if (this.WorkInProgress)
			{
				return;
			}
			if (this.Rack == null)
			{
				return;
			}
			this.WorkInProgress = true;
			base.Npc.Movement.FacePoint(this.Rack.uiPoint.position, 0.5f);
			this.workRoutine = base.StartCoroutine(this.<BeginAction>g__Package|20_0());
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0007EA08 File Offset: 0x0007CC08
		private void RpcReader___Observers_BeginAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginAction_2166136261();
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x0007EA32 File Offset: 0x0007CC32
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400183E RID: 6206
		public const float TIME_PER_ITEM = 1f;

		// Token: 0x04001841 RID: 6209
		private Coroutine workRoutine;

		// Token: 0x04001842 RID: 6210
		private bool dll_Excuted;

		// Token: 0x04001843 RID: 6211
		private bool dll_Excuted;
	}
}
