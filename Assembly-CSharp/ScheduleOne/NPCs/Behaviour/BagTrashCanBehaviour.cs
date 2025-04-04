using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.ObjectScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004E4 RID: 1252
	public class BagTrashCanBehaviour : Behaviour
	{
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x000771AB File Offset: 0x000753AB
		// (set) Token: 0x06001CC2 RID: 7362 RVA: 0x000771B3 File Offset: 0x000753B3
		public TrashContainerItem TargetTrashCan { get; private set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x000771BC File Offset: 0x000753BC
		private Cleaner Cleaner
		{
			get
			{
				return (Cleaner)base.Npc;
			}
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000771C9 File Offset: 0x000753C9
		public void SetTargetTrashCan(TrashContainerItem trashCan)
		{
			this.TargetTrashCan = trashCan;
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000771D2 File Offset: 0x000753D2
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000771E0 File Offset: 0x000753E0
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000771EE File Offset: 0x000753EE
		private void StartAction()
		{
			if (base.Npc.Avatar.CurrentEquippable != null)
			{
				base.Npc.SetEquippable_Return(string.Empty);
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x00077219 File Offset: 0x00075419
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0007723D File Offset: 0x0007543D
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x0007724C File Offset: 0x0007544C
		private void StopAllActions()
		{
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
			base.Npc.SetAnimationBool("PatSoil", false);
			base.Npc.SetCrouched_Networked(false);
			if (this.actionCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.actionCoroutine);
				this.actionCoroutine = null;
			}
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000772B8 File Offset: 0x000754B8
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.Movement.IsMoving)
			{
				return;
			}
			if (this.actionCoroutine != null)
			{
				return;
			}
			if (!this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				return;
			}
			if (this.IsAtDestination())
			{
				this.PerformAction();
				return;
			}
			this.GoToTarget();
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00077315 File Offset: 0x00075515
		private void GoToTarget()
		{
			if (!this.AreActionConditionsMet(true))
			{
				base.Disable_Networked(null);
				return;
			}
			base.SetDestination(NavMeshUtility.GetAccessPoint(this.TargetTrashCan, base.Npc).position, true);
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00077345 File Offset: 0x00075545
		[ObserversRpc(RunLocally = true)]
		private void PerformAction()
		{
			this.RpcWriter___Observers_PerformAction_2166136261();
			this.RpcLogic___PerformAction_2166136261();
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x00077353 File Offset: 0x00075553
		private bool IsAtDestination()
		{
			return Vector3.Distance(base.Npc.transform.position, this.TargetTrashCan.transform.position) <= 2f;
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x00077384 File Offset: 0x00075584
		private bool AreActionConditionsMet(bool checkAccess)
		{
			if (this.TargetTrashCan == null)
			{
				return false;
			}
			if (this.TargetTrashCan.Container.NormalizedTrashLevel == 0f)
			{
				return false;
			}
			if (checkAccess)
			{
				Transform accessPoint = NavMeshUtility.GetAccessPoint(this.TargetTrashCan, base.Npc);
				if (accessPoint == null)
				{
					return false;
				}
				if (!base.Npc.Movement.CanGetTo(accessPoint.position, 2f))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000773FA File Offset: 0x000755FA
		[CompilerGenerated]
		private IEnumerator <PerformAction>g__Action|21_0()
		{
			if (InstanceFinder.IsServer && !this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				yield break;
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.FacePoint(this.TargetTrashCan.transform.position, 0.5f);
			}
			yield return new WaitForSeconds(0.4f);
			base.Npc.SetAnimationBool("PatSoil", true);
			base.Npc.SetCrouched_Networked(true);
			if (this.onPerfomAction != null)
			{
				this.onPerfomAction.Invoke();
			}
			yield return new WaitForSeconds(3f);
			if (InstanceFinder.IsServer && this.AreActionConditionsMet(false))
			{
				this.TargetTrashCan.Container.BagTrash();
				if (this.onPerfomDone != null)
				{
					this.onPerfomDone.Invoke();
				}
			}
			base.Npc.SetAnimationBool("PatSoil", false);
			yield return new WaitForSeconds(0.2f);
			this.actionCoroutine = null;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x00077409 File Offset: 0x00075609
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BagTrashCanBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BagTrashCanBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_PerformAction_2166136261));
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x00077439 File Offset: 0x00075639
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BagTrashCanBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BagTrashCanBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x00077452 File Offset: 0x00075652
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x00077460 File Offset: 0x00075660
		private void RpcWriter___Observers_PerformAction_2166136261()
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

		// Token: 0x06001CD7 RID: 7383 RVA: 0x00077509 File Offset: 0x00075709
		private void RpcLogic___PerformAction_2166136261()
		{
			if (this.actionCoroutine != null)
			{
				return;
			}
			this.actionCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<PerformAction>g__Action|21_0());
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x0007752C File Offset: 0x0007572C
		private void RpcReader___Observers_PerformAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___PerformAction_2166136261();
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x00077556 File Offset: 0x00075756
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400176D RID: 5997
		public const float ACTION_MAX_DISTANCE = 2f;

		// Token: 0x0400176E RID: 5998
		public const float BAG_TIME = 3f;

		// Token: 0x04001770 RID: 6000
		private Coroutine actionCoroutine;

		// Token: 0x04001771 RID: 6001
		public UnityEvent onPerfomAction;

		// Token: 0x04001772 RID: 6002
		public UnityEvent onPerfomDone;

		// Token: 0x04001773 RID: 6003
		private bool dll_Excuted;

		// Token: 0x04001774 RID: 6004
		private bool dll_Excuted;
	}
}
