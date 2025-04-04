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
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004F7 RID: 1271
	public class PickUpTrashBehaviour : Behaviour
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x0007A692 File Offset: 0x00078892
		// (set) Token: 0x06001DC7 RID: 7623 RVA: 0x0007A69A File Offset: 0x0007889A
		public TrashItem TargetTrash { get; private set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x000771BC File Offset: 0x000753BC
		private Cleaner Cleaner
		{
			get
			{
				return (Cleaner)base.Npc;
			}
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0007A6A3 File Offset: 0x000788A3
		public void SetTargetTrash(TrashItem trash)
		{
			this.TargetTrash = trash;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0007A6AC File Offset: 0x000788AC
		protected override void Begin()
		{
			base.Begin();
			this.StartAction();
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0007A6BA File Offset: 0x000788BA
		protected override void Resume()
		{
			base.Resume();
			this.StartAction();
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0007A6C8 File Offset: 0x000788C8
		private void StartAction()
		{
			if (base.Npc.Avatar.CurrentEquippable == null || base.Npc.Avatar.CurrentEquippable.AssetPath != "Tools/TrashGrabber/TrashGrabber_AvatarEquippable")
			{
				base.Npc.SetEquippable_Return("Tools/TrashGrabber/TrashGrabber_AvatarEquippable");
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0007A71F File Offset: 0x0007891F
		protected override void Pause()
		{
			base.Pause();
			this.StopAllActions();
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0007A72D File Offset: 0x0007892D
		protected override void End()
		{
			base.End();
			this.StopAllActions();
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x0007A73C File Offset: 0x0007893C
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

		// Token: 0x06001DD1 RID: 7633 RVA: 0x0007A78C File Offset: 0x0007898C
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (base.Npc.Movement.IsMoving)
			{
				if (this.Cleaner.DEBUG)
				{
					Console.Log("Waiting for movement to finish", null);
				}
				return;
			}
			if (this.actionCoroutine != null)
			{
				if (this.Cleaner.DEBUG)
				{
					Console.Log("Waiting for action to finish", null);
				}
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

		// Token: 0x06001DD2 RID: 7634 RVA: 0x0007A81C File Offset: 0x00078A1C
		private void GoToTarget()
		{
			if (this.Cleaner.DEBUG)
			{
				Console.Log("Going to target", null);
			}
			if (!this.AreActionConditionsMet(true))
			{
				base.Disable_Networked(null);
				return;
			}
			base.SetDestination(this.TargetTrash.transform.position, true);
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x0007A869 File Offset: 0x00078A69
		[ObserversRpc(RunLocally = true)]
		private void PerformAction()
		{
			this.RpcWriter___Observers_PerformAction_2166136261();
			this.RpcLogic___PerformAction_2166136261();
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x0007A877 File Offset: 0x00078A77
		private bool IsAtDestination()
		{
			return Vector3.Distance(base.Npc.transform.position, this.TargetTrash.transform.position) <= 2f;
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0007A8A8 File Offset: 0x00078AA8
		private bool AreActionConditionsMet(bool checkAccess)
		{
			return !(this.TargetTrash == null) && !this.TargetTrash.Draggable.IsBeingDragged && (!checkAccess || base.Npc.Movement.CanGetTo(this.TargetTrash.transform.position, 2f));
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x0007A906 File Offset: 0x00078B06
		[CompilerGenerated]
		private IEnumerator <PerformAction>g__Action|20_0()
		{
			if (InstanceFinder.IsServer && !this.AreActionConditionsMet(false))
			{
				this.actionCoroutine = null;
				base.Disable_Networked(null);
				yield break;
			}
			if (InstanceFinder.IsServer)
			{
				base.Npc.Movement.FacePoint(this.TargetTrash.transform.position, 0.5f);
			}
			yield return new WaitForSeconds(0.3f);
			if (this.onPerfomAction != null)
			{
				this.onPerfomAction.Invoke();
			}
			yield return new WaitForSeconds(0.4f);
			if (InstanceFinder.IsServer)
			{
				this.Cleaner.trashGrabberInstance.AddTrash(this.TargetTrash.ID, 1);
				if (this.TargetTrash != null)
				{
					this.TargetTrash.DestroyTrash();
				}
			}
			yield return new WaitForSeconds(0.2f);
			this.actionCoroutine = null;
			base.Disable_Networked(null);
			yield break;
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x0007A915 File Offset: 0x00078B15
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PickUpTrashBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PickUpTrashBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_PerformAction_2166136261));
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x0007A945 File Offset: 0x00078B45
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PickUpTrashBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PickUpTrashBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x0007A95E File Offset: 0x00078B5E
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0007A96C File Offset: 0x00078B6C
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

		// Token: 0x06001DDC RID: 7644 RVA: 0x0007AA15 File Offset: 0x00078C15
		private void RpcLogic___PerformAction_2166136261()
		{
			if (this.Cleaner.DEBUG)
			{
				Console.Log("Picking up trash", null);
			}
			if (this.actionCoroutine != null)
			{
				return;
			}
			this.actionCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<PerformAction>g__Action|20_0());
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0007AA50 File Offset: 0x00078C50
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

		// Token: 0x06001DDE RID: 7646 RVA: 0x0007AA7A File Offset: 0x00078C7A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017CE RID: 6094
		public const float ACTION_MAX_DISTANCE = 2f;

		// Token: 0x040017CF RID: 6095
		public const string EQUIPPABLE_ASSET_PATH = "Tools/TrashGrabber/TrashGrabber_AvatarEquippable";

		// Token: 0x040017D1 RID: 6097
		private Coroutine actionCoroutine;

		// Token: 0x040017D2 RID: 6098
		public UnityEvent onPerfomAction;

		// Token: 0x040017D3 RID: 6099
		private bool dll_Excuted;

		// Token: 0x040017D4 RID: 6100
		private bool dll_Excuted;
	}
}
