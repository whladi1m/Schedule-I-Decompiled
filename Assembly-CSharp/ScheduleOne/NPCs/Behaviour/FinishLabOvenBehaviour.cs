using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Employees;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004ED RID: 1261
	public class FinishLabOvenBehaviour : Behaviour
	{
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x00078AD8 File Offset: 0x00076CD8
		// (set) Token: 0x06001D4A RID: 7498 RVA: 0x00078AE0 File Offset: 0x00076CE0
		public LabOven targetOven { get; private set; }

		// Token: 0x06001D4B RID: 7499 RVA: 0x00078AE9 File Offset: 0x00076CE9
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviour_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00078AFD File Offset: 0x00076CFD
		public void SetTargetOven(LabOven oven)
		{
			this.targetOven = oven;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x00078B08 File Offset: 0x00076D08
		protected override void End()
		{
			base.End();
			if (this.targetOven != null)
			{
				this.targetOven.Door.SetPosition(0f);
				this.targetOven.ClearShards();
				this.targetOven.RemoveTrayAnimation.Stop();
				this.targetOven.ResetSquareTray();
			}
			this.Disable();
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x00078B6C File Offset: 0x00076D6C
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.actionRoutine != null)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.targetOven.UIPoint.position, 5, false);
				return;
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!base.Npc.Movement.IsMoving)
			{
				if (this.IsAtStation())
				{
					this.StartAction();
					return;
				}
				base.SetDestination(this.GetStationAccessPoint(), true);
			}
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x00078BE5 File Offset: 0x00076DE5
		[ObserversRpc(RunLocally = true)]
		private void StartAction()
		{
			this.RpcWriter___Observers_StartAction_2166136261();
			this.RpcLogic___StartAction_2166136261();
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x00078BF4 File Offset: 0x00076DF4
		private bool CanActionStart()
		{
			return !(this.targetOven == null) && (!((IUsable)this.targetOven).IsInUse || !(((IUsable)this.targetOven).NPCUserObject != base.Npc.NetworkObject)) && this.targetOven.CurrentOperation != null && this.targetOven.CurrentOperation.IsReady() && this.targetOven.CanOutputSpaceFitCurrentOperation();
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x00078C70 File Offset: 0x00076E70
		private void StopAction()
		{
			this.targetOven.SetNPCUser(null);
			base.Npc.SetEquippable_Networked(null, string.Empty);
			base.Npc.SetAnimationBool_Networked(null, "UseHammer", false);
			if (this.actionRoutine != null)
			{
				base.StopCoroutine(this.actionRoutine);
				this.actionRoutine = null;
			}
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x00078CC7 File Offset: 0x00076EC7
		private Vector3 GetStationAccessPoint()
		{
			if (this.targetOven == null)
			{
				return base.Npc.transform.position;
			}
			return ((ITransitEntity)this.targetOven).AccessPoints[0].position;
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x00078CFA File Offset: 0x00076EFA
		private bool IsAtStation()
		{
			return !(this.targetOven == null) && Vector3.Distance(base.Npc.transform.position, this.GetStationAccessPoint()) < 1f;
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x00078D2E File Offset: 0x00076F2E
		[CompilerGenerated]
		private IEnumerator <StartAction>g__ActionRoutine|11_0()
		{
			this.targetOven.SetNPCUser(base.Npc.NetworkObject);
			base.Npc.Movement.FacePoint(this.targetOven.transform.position, 0.5f);
			yield return new WaitForSeconds(0.5f);
			if (!this.CanActionStart())
			{
				this.StopAction();
				base.End_Networked(null);
				yield break;
			}
			base.Npc.SetEquippable_Networked(null, "Avatar/Equippables/Hammer");
			this.targetOven.Door.SetPosition(1f);
			this.targetOven.WireTray.SetPosition(1f);
			yield return new WaitForSeconds(0.5f);
			this.targetOven.SquareTray.SetParent(this.targetOven.transform);
			this.targetOven.RemoveTrayAnimation.Play();
			yield return new WaitForSeconds(0.1f);
			this.targetOven.Door.SetPosition(0f);
			yield return new WaitForSeconds(1f);
			base.Npc.SetAnimationBool_Networked(null, "UseHammer", true);
			yield return new WaitForSeconds(10f);
			base.Npc.SetAnimationBool_Networked(null, "UseHammer", false);
			this.targetOven.Shatter(this.targetOven.CurrentOperation.Cookable.ProductQuantity, this.targetOven.CurrentOperation.Cookable.ProductShardPrefab.gameObject);
			yield return new WaitForSeconds(1f);
			ItemInstance productItem = this.targetOven.CurrentOperation.GetProductItem(this.targetOven.CurrentOperation.Cookable.ProductQuantity * this.targetOven.CurrentOperation.IngredientQuantity);
			this.targetOven.OutputSlot.AddItem(productItem, false);
			this.targetOven.SendCookOperation(null);
			this.StopAction();
			base.End_Networked(null);
			yield break;
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x00078D3D File Offset: 0x00076F3D
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_StartAction_2166136261));
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x00078D6D File Offset: 0x00076F6D
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.FinishLabOvenBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x00078D86 File Offset: 0x00076F86
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x00078D94 File Offset: 0x00076F94
		private void RpcWriter___Observers_StartAction_2166136261()
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

		// Token: 0x06001D5A RID: 7514 RVA: 0x00078E3D File Offset: 0x0007703D
		private void RpcLogic___StartAction_2166136261()
		{
			if (this.actionRoutine != null)
			{
				return;
			}
			if (this.targetOven == null)
			{
				return;
			}
			this.actionRoutine = base.StartCoroutine(this.<StartAction>g__ActionRoutine|11_0());
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x00078E6C File Offset: 0x0007706C
		private void RpcReader___Observers_StartAction_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartAction_2166136261();
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x00078E96 File Offset: 0x00077096
		protected virtual void dll()
		{
			base.Awake();
			this.chemist = (base.Npc as Chemist);
		}

		// Token: 0x0400179B RID: 6043
		public const float HARVEST_TIME = 10f;

		// Token: 0x0400179D RID: 6045
		private Chemist chemist;

		// Token: 0x0400179E RID: 6046
		private Coroutine actionRoutine;

		// Token: 0x0400179F RID: 6047
		private bool dll_Excuted;

		// Token: 0x040017A0 RID: 6048
		private bool dll_Excuted;
	}
}
