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
using ScheduleOne.Money;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000476 RID: 1142
	public class NPCSignal_UseATM : NPCSignal
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x0006DB0C File Offset: 0x0006BD0C
		public new string ActionName
		{
			get
			{
				return "Use ATM";
			}
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0006DB13 File Offset: 0x0006BD13
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0006DB1B File Offset: 0x0006BD1B
		public override void Started()
		{
			base.Started();
			if (this.ATM == null)
			{
				Debug.LogWarning("No ATM found for NPC to use");
				this.End();
				return;
			}
			base.SetDestination(this.ATM.AccessPoint.position, true);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0006DB5C File Offset: 0x0006BD5C
		public override void ActiveMinPassed()
		{
			base.MinPassed();
			if (this.ATM == null)
			{
				this.End();
				return;
			}
			if (this.purchaseCoroutine != null)
			{
				return;
			}
			if (!this.npc.Movement.IsMoving)
			{
				if (this.IsAtDestination())
				{
					if (this.purchaseCoroutine == null)
					{
						this.Purchase();
						return;
					}
				}
				else
				{
					Debug.DrawLine(this.npc.Movement.FootPosition, this.ATM.AccessPoint.position, Color.red, 1f);
					base.SetDestination(this.ATM.AccessPoint.position, true);
				}
			}
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0006DBFC File Offset: 0x0006BDFC
		public override void LateStarted()
		{
			base.LateStarted();
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0006DC04 File Offset: 0x0006BE04
		public override void Interrupt()
		{
			base.Interrupt();
			if (this.npc.Movement.IsMoving)
			{
				this.npc.Movement.Stop();
			}
			if (this.purchaseCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.purchaseCoroutine);
				this.purchaseCoroutine = null;
			}
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0006DC58 File Offset: 0x0006BE58
		public override void Resume()
		{
			base.Resume();
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0006C448 File Offset: 0x0006A648
		public override void Skipped()
		{
			base.Skipped();
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0006DC60 File Offset: 0x0006BE60
		private bool IsAtDestination()
		{
			return !(this.ATM == null) && Vector3.Distance(this.npc.Movement.FootPosition, this.ATM.AccessPoint.position) < 2f;
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0006DC9E File Offset: 0x0006BE9E
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
			this.Purchase();
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0006DCBC File Offset: 0x0006BEBC
		[ObserversRpc(RunLocally = true)]
		public void Purchase()
		{
			this.RpcWriter___Observers_Purchase_2166136261();
			this.RpcLogic___Purchase_2166136261();
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x0006DCD5 File Offset: 0x0006BED5
		[CompilerGenerated]
		private IEnumerator <Purchase>g__Purchase|14_0()
		{
			if (this.ATM.IsBroken)
			{
				this.End();
				this.purchaseCoroutine = null;
				yield break;
			}
			yield return new WaitForSeconds(2f);
			this.npc.SetAnimationTrigger_Networked(null, "GrabItem");
			yield return new WaitForSeconds(1f);
			this.End();
			this.purchaseCoroutine = null;
			yield break;
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x0006DCE4 File Offset: 0x0006BEE4
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_UseATMAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_UseATMAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_Purchase_2166136261));
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x0006DD14 File Offset: 0x0006BF14
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_UseATMAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_UseATMAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x0006DD2D File Offset: 0x0006BF2D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x0006DD3C File Offset: 0x0006BF3C
		private void RpcWriter___Observers_Purchase_2166136261()
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

		// Token: 0x0600193A RID: 6458 RVA: 0x0006DDE8 File Offset: 0x0006BFE8
		public void RpcLogic___Purchase_2166136261()
		{
			if (this.purchaseCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.purchaseCoroutine);
			}
			this.npc.Movement.FaceDirection(this.ATM.AccessPoint.forward, 0.5f);
			this.purchaseCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<Purchase>g__Purchase|14_0());
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x0006DE48 File Offset: 0x0006C048
		private void RpcReader___Observers_Purchase_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Purchase_2166136261();
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x0006DE72 File Offset: 0x0006C072
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040015F1 RID: 5617
		private const float destinationThreshold = 2f;

		// Token: 0x040015F2 RID: 5618
		public ATM ATM;

		// Token: 0x040015F3 RID: 5619
		private Coroutine purchaseCoroutine;

		// Token: 0x040015F4 RID: 5620
		private bool dll_Excuted;

		// Token: 0x040015F5 RID: 5621
		private bool dll_Excuted;
	}
}
