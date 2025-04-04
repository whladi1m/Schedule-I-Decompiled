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
using ScheduleOne.Trash;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004E8 RID: 1256
	public class DisposeTrashBagBehaviour : Behaviour
	{
		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x00077CD0 File Offset: 0x00075ED0
		// (set) Token: 0x06001D01 RID: 7425 RVA: 0x00077CD8 File Offset: 0x00075ED8
		public TrashBag TargetBag { get; private set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x000771BC File Offset: 0x000753BC
		private Cleaner Cleaner
		{
			get
			{
				return (Cleaner)base.Npc;
			}
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x00077CE1 File Offset: 0x00075EE1
		public void SetTargetBag(TrashBag bag)
		{
			this.TargetBag = bag;
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x00077CEA File Offset: 0x00075EEA
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x00077CF8 File Offset: 0x00075EF8
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000045B1 File Offset: 0x000027B1
		private void StartAction()
		{
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x00077D06 File Offset: 0x00075F06
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x00077D14 File Offset: 0x00075F14
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x00077D24 File Offset: 0x00075F24
		private void StopAllActions()
		{
			if (base.Npc.Movement.IsMoving)
			{
				base.Npc.Movement.Stop();
			}
			if (this.grabRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.grabRoutine);
				this.grabRoutine = null;
			}
			if (this.dropRoutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.dropRoutine);
				this.dropRoutine = null;
			}
			if (base.Npc.Avatar.CurrentEquippable != null && base.Npc.Avatar.CurrentEquippable.AssetPath == this.TRASH_BAG_ASSET_PATH)
			{
				base.Npc.SetEquippable_Return(string.Empty);
			}
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x00077DDC File Offset: 0x00075FDC
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
			if (this.grabRoutine != null)
			{
				return;
			}
			if (this.dropRoutine != null)
			{
				return;
			}
			if (!this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				return;
			}
			if (this.heldTrash == null)
			{
				if (this.IsAtDestination())
				{
					this.GrabTrash();
					return;
				}
				this.GoToTarget();
				return;
			}
			else
			{
				if (this.IsAtDestination())
				{
					this.DropTrash();
					return;
				}
				this.GoToTarget();
				return;
			}
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x00077E60 File Offset: 0x00076060
		private void GoToTarget()
		{
			if (!this.AreActionConditionsMet(true))
			{
				base.Disable_Networked(null);
				return;
			}
			if (this.heldTrash == null)
			{
				base.SetDestination(this.TargetBag.transform.position, true);
				return;
			}
			base.SetDestination(this.Cleaner.AssignedProperty.DisposalArea.StandPoint.position, true);
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x00077EBF File Offset: 0x000760BF
		[ObserversRpc(RunLocally = true)]
		private void GrabTrash()
		{
			this.RpcWriter___Observers_GrabTrash_2166136261();
			this.RpcLogic___GrabTrash_2166136261();
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x00077ECD File Offset: 0x000760CD
		[ObserversRpc(RunLocally = true)]
		private void DropTrash()
		{
			this.RpcWriter___Observers_DropTrash_2166136261();
			this.RpcLogic___DropTrash_2166136261();
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x00077EDC File Offset: 0x000760DC
		private bool IsAtDestination()
		{
			if (this.heldTrash == null)
			{
				return Vector3.Distance(base.Npc.transform.position, this.TargetBag.transform.position) <= 2f;
			}
			return Vector3.Distance(base.Npc.transform.position, this.Cleaner.AssignedProperty.DisposalArea.StandPoint.position) <= 2f;
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x00077F5C File Offset: 0x0007615C
		private bool AreActionConditionsMet(bool checkAccess)
		{
			if (this.heldTrash == null)
			{
				if (this.TargetBag == null)
				{
					return false;
				}
				if (this.TargetBag.Draggable.IsBeingDragged)
				{
					return false;
				}
				if (checkAccess && !base.Npc.Movement.CanGetTo(this.TargetBag.transform.position, 2f))
				{
					return false;
				}
			}
			else if (checkAccess && !base.Npc.Movement.CanGetTo(this.Cleaner.AssignedProperty.DisposalArea.StandPoint.position, 2f))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x0007800B File Offset: 0x0007620B
		[CompilerGenerated]
		private IEnumerator <GrabTrash>g__Action|21_0()
		{
			if (InstanceFinder.IsServer && !this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				yield break;
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.FacePoint(this.TargetBag.transform.position, 0.5f);
			}
			yield return new WaitForSeconds(0.3f);
			base.Npc.SetAnimationTrigger("GrabItem");
			if (InstanceFinder.IsServer)
			{
				if (!this.AreActionConditionsMet(false))
				{
					base.Disable_Networked(null);
					this.grabRoutine = null;
					yield break;
				}
				base.Npc.SetEquippable_Networked(null, this.TRASH_BAG_ASSET_PATH);
				this.heldTrash = this.TargetBag.Content;
				this.TargetBag.DestroyTrash();
			}
			yield return new WaitForSeconds(0.2f);
			this.grabRoutine = null;
			yield break;
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x0007801A File Offset: 0x0007621A
		[CompilerGenerated]
		private IEnumerator <DropTrash>g__Action|22_0()
		{
			if (InstanceFinder.IsServer && !this.AreActionConditionsMet(false))
			{
				base.Disable_Networked(null);
				yield break;
			}
			base.Npc.Movement.FaceDirection(this.Cleaner.AssignedProperty.DisposalArea.StandPoint.forward, 0.5f);
			yield return new WaitForSeconds(0.5f);
			if (InstanceFinder.IsServer)
			{
				Transform trashDropPoint = this.Cleaner.AssignedProperty.DisposalArea.TrashDropPoint;
				NetworkSingleton<TrashManager>.Instance.CreateTrashBag("trashbag", trashDropPoint.position, UnityEngine.Random.rotation, this.heldTrash.GetData(), default(Vector3), "", false);
				this.heldTrash = null;
				base.Npc.SetEquippable_Networked(null, string.Empty);
			}
			yield return new WaitForSeconds(0.2f);
			this.dropRoutine = null;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x0007802C File Offset: 0x0007622C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.DisposeTrashBagBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.DisposeTrashBagBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_GrabTrash_2166136261));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_DropTrash_2166136261));
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x0007807E File Offset: 0x0007627E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.DisposeTrashBagBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.DisposeTrashBagBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x00078097 File Offset: 0x00076297
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000780A8 File Offset: 0x000762A8
		private void RpcWriter___Observers_GrabTrash_2166136261()
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

		// Token: 0x06001D18 RID: 7448 RVA: 0x00078151 File Offset: 0x00076351
		private void RpcLogic___GrabTrash_2166136261()
		{
			if (this.grabRoutine != null)
			{
				return;
			}
			this.grabRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<GrabTrash>g__Action|21_0());
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x00078174 File Offset: 0x00076374
		private void RpcReader___Observers_GrabTrash_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___GrabTrash_2166136261();
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000781A0 File Offset: 0x000763A0
		private void RpcWriter___Observers_DropTrash_2166136261()
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
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x00078249 File Offset: 0x00076449
		private void RpcLogic___DropTrash_2166136261()
		{
			if (this.dropRoutine != null)
			{
				return;
			}
			this.dropRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<DropTrash>g__Action|22_0());
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x0007826C File Offset: 0x0007646C
		private void RpcReader___Observers_DropTrash_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DropTrash_2166136261();
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x00078296 File Offset: 0x00076496
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001783 RID: 6019
		public string TRASH_BAG_ASSET_PATH = "Avatar/Equippables/TrashBag";

		// Token: 0x04001784 RID: 6020
		public const float GRAB_MAX_DISTANCE = 2f;

		// Token: 0x04001786 RID: 6022
		private TrashContent heldTrash;

		// Token: 0x04001787 RID: 6023
		private Coroutine grabRoutine;

		// Token: 0x04001788 RID: 6024
		private Coroutine dropRoutine;

		// Token: 0x04001789 RID: 6025
		private bool dll_Excuted;

		// Token: 0x0400178A RID: 6026
		private bool dll_Excuted;
	}
}
