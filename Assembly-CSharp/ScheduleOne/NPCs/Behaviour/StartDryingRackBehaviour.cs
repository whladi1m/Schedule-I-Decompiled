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
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000505 RID: 1285
	public class StartDryingRackBehaviour : Behaviour
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001E77 RID: 7799 RVA: 0x0007D4B4 File Offset: 0x0007B6B4
		// (set) Token: 0x06001E78 RID: 7800 RVA: 0x0007D4BC File Offset: 0x0007B6BC
		public DryingRack Rack { get; protected set; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001E79 RID: 7801 RVA: 0x0007D4C5 File Offset: 0x0007B6C5
		// (set) Token: 0x06001E7A RID: 7802 RVA: 0x0007D4CD File Offset: 0x0007B6CD
		public bool WorkInProgress { get; protected set; }

		// Token: 0x06001E7B RID: 7803 RVA: 0x0007D4D6 File Offset: 0x0007B6D6
		protected override void Begin()
		{
			base.Begin();
			this.StartWork();
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0007D4E4 File Offset: 0x0007B6E4
		protected override void Resume()
		{
			base.Resume();
			this.StartWork();
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0007D4F2 File Offset: 0x0007B6F2
		protected override void Pause()
		{
			base.Pause();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0007D508 File Offset: 0x0007B708
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

		// Token: 0x06001E80 RID: 7808 RVA: 0x0007D568 File Offset: 0x0007B768
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

		// Token: 0x06001E81 RID: 7809 RVA: 0x0007D5CC File Offset: 0x0007B7CC
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

		// Token: 0x06001E82 RID: 7810 RVA: 0x0007D628 File Offset: 0x0007B828
		public void AssignRack(DryingRack rack)
		{
			this.Rack = rack;
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0007D631 File Offset: 0x0007B831
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(NavMeshUtility.GetAccessPoint(this.Rack, base.Npc).position, 0.5f);
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0007D65E File Offset: 0x0007B85E
		public void GoToStation()
		{
			base.Npc.Movement.SetDestination(NavMeshUtility.GetAccessPoint(this.Rack, base.Npc).position);
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0007D688 File Offset: 0x0007B888
		[ObserversRpc(RunLocally = true)]
		public void BeginAction()
		{
			this.RpcWriter___Observers_BeginAction_2166136261();
			this.RpcLogic___BeginAction_2166136261();
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0007D6A1 File Offset: 0x0007B8A1
		private void StopCauldron()
		{
			if (this.workRoutine != null)
			{
				base.StopCoroutine(this.workRoutine);
			}
			this.WorkInProgress = false;
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x0007D6C0 File Offset: 0x0007B8C0
		public bool IsRackReady(DryingRack rack)
		{
			return !(rack == null) && (!((IUsable)rack).IsInUse || (!(rack.PlayerUserObject != null) && !(rack.NPCUserObject != base.Npc.NetworkObject))) && rack.InputSlot.Quantity > 0 && rack.GetTotalDryingItems() < rack.ItemCapacity && base.Npc.Movement.CanGetTo(rack.transform.position, 1f);
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0007D74D File Offset: 0x0007B94D
		[CompilerGenerated]
		private IEnumerator <BeginAction>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			this.Rack.InputSlot.ItemInstance.GetCopy(1);
			int itemCount = 0;
			while (this.Rack != null && this.Rack.InputSlot.Quantity > itemCount && this.Rack.GetTotalDryingItems() + itemCount < this.Rack.ItemCapacity)
			{
				base.Npc.Avatar.Anim.SetTrigger("GrabItem");
				yield return new WaitForSeconds(1f);
				int num = itemCount;
				itemCount = num + 1;
			}
			if (InstanceFinder.IsServer)
			{
				this.Rack.StartOperation();
			}
			this.WorkInProgress = false;
			this.workRoutine = null;
			yield break;
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0007D75C File Offset: 0x0007B95C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartDryingRackBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartDryingRackBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginAction_2166136261));
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0007D78C File Offset: 0x0007B98C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartDryingRackBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartDryingRackBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x0007D7A5 File Offset: 0x0007B9A5
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0007D7B4 File Offset: 0x0007B9B4
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

		// Token: 0x06001E8E RID: 7822 RVA: 0x0007D860 File Offset: 0x0007BA60
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

		// Token: 0x06001E8F RID: 7823 RVA: 0x0007D8C4 File Offset: 0x0007BAC4
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

		// Token: 0x06001E90 RID: 7824 RVA: 0x0007D8EE File Offset: 0x0007BAEE
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400181E RID: 6174
		public const float TIME_PER_ITEM = 1f;

		// Token: 0x04001821 RID: 6177
		private Coroutine workRoutine;

		// Token: 0x04001822 RID: 6178
		private bool dll_Excuted;

		// Token: 0x04001823 RID: 6179
		private bool dll_Excuted;
	}
}
