using System;
using System.Collections;
using System.Collections.Generic;
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
using ScheduleOne.Trash;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004EB RID: 1259
	public class EmptyTrashGrabberBehaviour : Behaviour
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x00078535 File Offset: 0x00076735
		// (set) Token: 0x06001D2B RID: 7467 RVA: 0x0007853D File Offset: 0x0007673D
		public TrashContainerItem TargetTrashCan { get; private set; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x000771BC File Offset: 0x000753BC
		private Cleaner Cleaner
		{
			get
			{
				return (Cleaner)base.Npc;
			}
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x00078546 File Offset: 0x00076746
		public void SetTargetTrashCan(TrashContainerItem trashCan)
		{
			this.TargetTrashCan = trashCan;
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x0007854F File Offset: 0x0007674F
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x0007855D File Offset: 0x0007675D
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x0007856C File Offset: 0x0007676C
		private void StartAction()
		{
			if (base.Npc.Avatar.CurrentEquippable == null || base.Npc.Avatar.CurrentEquippable.AssetPath != "Tools/TrashGrabber/Bin_AvatarEquippable")
			{
				base.Npc.SetEquippable_Return("Tools/TrashGrabber/Bin_AvatarEquippable");
			}
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x000785C3 File Offset: 0x000767C3
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x000785D1 File Offset: 0x000767D1
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x000785E0 File Offset: 0x000767E0
		private void StopAllActions()
		{
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
			if (this.actionCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.actionCoroutine);
				this.actionCoroutine = null;
			}
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x00078630 File Offset: 0x00076830
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

		// Token: 0x06001D36 RID: 7478 RVA: 0x00078690 File Offset: 0x00076890
		private void GoToTarget()
		{
			if (!this.AreActionConditionsMet(true))
			{
				base.Disable_Networked(null);
				return;
			}
			Transform accessPoint = NavMeshUtility.GetAccessPoint(this.TargetTrashCan, base.Npc);
			if (accessPoint == null)
			{
				base.Disable_Networked(null);
				return;
			}
			base.SetDestination(accessPoint.position, true);
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x000786DE File Offset: 0x000768DE
		[ObserversRpc(RunLocally = true)]
		private void PerformAction()
		{
			this.RpcWriter___Observers_PerformAction_2166136261();
			this.RpcLogic___PerformAction_2166136261();
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x000786EC File Offset: 0x000768EC
		private bool IsAtDestination()
		{
			return Vector3.Distance(base.Npc.transform.position, this.TargetTrashCan.transform.position) <= 2f;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x00078720 File Offset: 0x00076920
		private bool AreActionConditionsMet(bool checkAccess)
		{
			return !(this.TargetTrashCan == null) && this.TargetTrashCan.Container.NormalizedTrashLevel < 1f && this.Cleaner.trashGrabberInstance.GetTotalSize() != 0 && (!checkAccess || base.Npc.Movement.CanGetTo(this.TargetTrashCan.transform.position, 2f));
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x00078797 File Offset: 0x00076997
		[CompilerGenerated]
		private IEnumerator <PerformAction>g__Action|20_0()
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
			yield return new WaitForSeconds(0.3f);
			if (this.onPerfomAction != null)
			{
				this.onPerfomAction.Invoke();
			}
			yield return new WaitForSeconds(0.4f);
			if (InstanceFinder.IsServer)
			{
				while (this.AreActionConditionsMet(false))
				{
					List<string> trashIDs = this.Cleaner.trashGrabberInstance.GetTrashIDs();
					string id = trashIDs[trashIDs.Count - 1];
					this.Cleaner.trashGrabberInstance.RemoveTrash(id, 1);
					NetworkSingleton<TrashManager>.Instance.CreateTrashItem(id, this.TargetTrashCan.transform.position + Vector3.up * 1.5f, UnityEngine.Random.rotation, default(Vector3), "", false);
					yield return new WaitForSeconds(0.5f);
				}
			}
			yield return new WaitForSeconds(0.2f);
			this.actionCoroutine = null;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000787A6 File Offset: 0x000769A6
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.EmptyTrashGrabberBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.EmptyTrashGrabberBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_PerformAction_2166136261));
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000787D6 File Offset: 0x000769D6
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.EmptyTrashGrabberBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.EmptyTrashGrabberBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000787EF File Offset: 0x000769EF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x00078800 File Offset: 0x00076A00
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

		// Token: 0x06001D40 RID: 7488 RVA: 0x000788A9 File Offset: 0x00076AA9
		private void RpcLogic___PerformAction_2166136261()
		{
			if (this.actionCoroutine != null)
			{
				return;
			}
			this.actionCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<PerformAction>g__Action|20_0());
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000788CC File Offset: 0x00076ACC
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

		// Token: 0x06001D42 RID: 7490 RVA: 0x000788F6 File Offset: 0x00076AF6
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001791 RID: 6033
		public const float ACTION_MAX_DISTANCE = 2f;

		// Token: 0x04001792 RID: 6034
		public const string EQUIPPABLE_ASSET_PATH = "Tools/TrashGrabber/Bin_AvatarEquippable";

		// Token: 0x04001794 RID: 6036
		private Coroutine actionCoroutine;

		// Token: 0x04001795 RID: 6037
		public UnityEvent onPerfomAction;

		// Token: 0x04001796 RID: 6038
		private bool dll_Excuted;

		// Token: 0x04001797 RID: 6039
		private bool dll_Excuted;
	}
}
