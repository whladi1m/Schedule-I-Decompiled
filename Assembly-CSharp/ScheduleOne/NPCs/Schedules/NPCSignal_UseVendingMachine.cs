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
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Schedules
{
	// Token: 0x02000478 RID: 1144
	public class NPCSignal_UseVendingMachine : NPCSignal
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001943 RID: 6467 RVA: 0x0006DF51 File Offset: 0x0006C151
		public new string ActionName
		{
			get
			{
				return "Use Vending Machine";
			}
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0006DF58 File Offset: 0x0006C158
		public override string GetName()
		{
			return this.ActionName;
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0006DF60 File Offset: 0x0006C160
		public override void Started()
		{
			base.Started();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.TargetMachine = this.GetTargetMachine();
			if (this.TargetMachine == null)
			{
				Debug.LogWarning("No vending machine found for NPC to use");
				this.End();
				return;
			}
			base.SetDestination(this.TargetMachine.AccessPoint.position, true);
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0006DFC0 File Offset: 0x0006C1C0
		public override void MinPassed()
		{
			base.MinPassed();
			if (!base.IsActive)
			{
				return;
			}
			if (!this.npc.Movement.IsMoving)
			{
				if (this.TargetMachine == null)
				{
					this.TargetMachine = this.GetTargetMachine();
				}
				if (this.TargetMachine == null)
				{
					Debug.LogWarning("No vending machine found for NPC to use");
					this.End();
					return;
				}
				if (this.TargetMachine.AccessPoint == null)
				{
					Debug.LogWarning("Vending machine has no access point");
					this.End();
					return;
				}
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
					if (this.npc.Movement.CanGetTo(this.TargetMachine.AccessPoint.position, 1f))
					{
						base.SetDestination(this.TargetMachine.AccessPoint.position, true);
						return;
					}
					Debug.LogWarning("Unable to reach vending machine");
					this.End();
					return;
				}
			}
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0006DBFC File Offset: 0x0006BDFC
		public override void LateStarted()
		{
			base.LateStarted();
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0006E0B8 File Offset: 0x0006C2B8
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

		// Token: 0x06001949 RID: 6473 RVA: 0x0006DC58 File Offset: 0x0006BE58
		public override void Resume()
		{
			base.Resume();
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0006C448 File Offset: 0x0006A648
		public override void Skipped()
		{
			base.Skipped();
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0006E10C File Offset: 0x0006C30C
		private bool IsAtDestination()
		{
			return !(this.TargetMachine == null) && Vector3.Distance(this.npc.Movement.FootPosition, this.TargetMachine.AccessPoint.position) < 1f;
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0006E14C File Offset: 0x0006C34C
		private VendingMachine GetTargetMachine()
		{
			if (this.MachineOverride != null && base.movement.CanGetTo(this.MachineOverride.AccessPoint.position, 1f))
			{
				return this.MachineOverride;
			}
			VendingMachine result = null;
			float num = float.MaxValue;
			foreach (VendingMachine vendingMachine in VendingMachine.AllMachines)
			{
				if (base.movement.CanGetTo(vendingMachine.AccessPoint.position, 1f))
				{
					float num2 = Vector3.Distance(this.npc.Movement.FootPosition, vendingMachine.AccessPoint.position);
					if (num2 < num)
					{
						result = vendingMachine;
						num = num2;
					}
				}
			}
			return result;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0006E220 File Offset: 0x0006C420
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

		// Token: 0x0600194E RID: 6478 RVA: 0x0006E240 File Offset: 0x0006C440
		[ObserversRpc(RunLocally = true)]
		public void Purchase()
		{
			this.RpcWriter___Observers_Purchase_2166136261();
			this.RpcLogic___Purchase_2166136261();
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0006E259 File Offset: 0x0006C459
		private bool CheckItem()
		{
			if (this.TargetMachine.lastDroppedItem == null || this.TargetMachine.lastDroppedItem.gameObject == null)
			{
				this.ItemWasStolen();
				this.End();
				return false;
			}
			return true;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0006E295 File Offset: 0x0006C495
		private void ItemWasStolen()
		{
			this.npc.Avatar.EmotionManager.AddEmotionOverride("Annoyed", "drinkstolen", 20f, 0);
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0006E2BC File Offset: 0x0006C4BC
		[CompilerGenerated]
		private IEnumerator <Purchase>g__Purchase|16_0()
		{
			yield return new WaitForSeconds(1f);
			if (this.TargetMachine == null || this.TargetMachine.IsBroken)
			{
				this.purchaseCoroutine = null;
				this.End();
				yield break;
			}
			this.TargetMachine.PurchaseRoutine();
			yield return new WaitForSeconds(1f);
			if (!this.CheckItem())
			{
				this.purchaseCoroutine = null;
				this.End();
				yield break;
			}
			this.npc.SetAnimationTrigger_Networked(null, "GrabItem");
			yield return new WaitForSeconds(0.4f);
			if (!this.CheckItem())
			{
				this.purchaseCoroutine = null;
				this.End();
				yield break;
			}
			this.TargetMachine.RemoveLastDropped();
			yield return new WaitForSeconds(0.5f);
			this.End();
			this.purchaseCoroutine = null;
			this.npc.Avatar.EmotionManager.AddEmotionOverride("Cheery", "energydrink", 5f, 0);
			yield break;
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0006E2CB File Offset: 0x0006C4CB
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachineAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_Purchase_2166136261));
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0006E2FB File Offset: 0x0006C4FB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachineAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Schedules.NPCSignal_UseVendingMachineAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0006E314 File Offset: 0x0006C514
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0006E324 File Offset: 0x0006C524
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

		// Token: 0x06001957 RID: 6487 RVA: 0x0006E3D0 File Offset: 0x0006C5D0
		public void RpcLogic___Purchase_2166136261()
		{
			if (this.purchaseCoroutine != null)
			{
				Singleton<CoroutineService>.Instance.StopCoroutine(this.purchaseCoroutine);
			}
			if (this.TargetMachine == null)
			{
				this.TargetMachine = this.GetTargetMachine();
			}
			if (this.TargetMachine != null)
			{
				this.npc.Movement.FaceDirection(this.TargetMachine.AccessPoint.forward, 0.5f);
			}
			this.purchaseCoroutine = Singleton<CoroutineService>.Instance.StartCoroutine(this.<Purchase>g__Purchase|16_0());
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0006E458 File Offset: 0x0006C658
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

		// Token: 0x06001959 RID: 6489 RVA: 0x0006E482 File Offset: 0x0006C682
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040015F9 RID: 5625
		private const float destinationThreshold = 1f;

		// Token: 0x040015FA RID: 5626
		public VendingMachine MachineOverride;

		// Token: 0x040015FB RID: 5627
		private VendingMachine TargetMachine;

		// Token: 0x040015FC RID: 5628
		private Coroutine purchaseCoroutine;

		// Token: 0x040015FD RID: 5629
		private bool dll_Excuted;

		// Token: 0x040015FE RID: 5630
		private bool dll_Excuted;
	}
}
