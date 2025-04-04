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
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004E6 RID: 1254
	public class BrickPressBehaviour : Behaviour
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x000776E6 File Offset: 0x000758E6
		// (set) Token: 0x06001CE1 RID: 7393 RVA: 0x000776EE File Offset: 0x000758EE
		public BrickPress Press { get; protected set; }

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x000776F7 File Offset: 0x000758F7
		// (set) Token: 0x06001CE3 RID: 7395 RVA: 0x000776FF File Offset: 0x000758FF
		public bool PackagingInProgress { get; protected set; }

		// Token: 0x06001CE4 RID: 7396 RVA: 0x00077708 File Offset: 0x00075908
		protected override void Begin()
		{
			base.Begin();
			this.StartPackaging();
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x00077716 File Offset: 0x00075916
		protected override void Resume()
		{
			base.Resume();
			this.StartPackaging();
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x00077724 File Offset: 0x00075924
		protected override void Pause()
		{
			base.Pause();
			if (this.PackagingInProgress)
			{
				this.StopPackaging();
			}
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x0007773C File Offset: 0x0007593C
		protected override void End()
		{
			base.End();
			if (this.PackagingInProgress)
			{
				this.StopPackaging();
			}
			if (InstanceFinder.IsServer && this.Press != null && this.Press.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Press.SetNPCUser(null);
			}
		}

		// Token: 0x06001CE9 RID: 7401 RVA: 0x0007779C File Offset: 0x0007599C
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.PackagingInProgress)
			{
				if (this.IsStationReady(this.Press))
				{
					if (base.Npc.Movement.IsMoving)
					{
						return;
					}
					if (this.IsAtStation())
					{
						this.BeginPackaging();
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

		// Token: 0x06001CEA RID: 7402 RVA: 0x00077800 File Offset: 0x00075A00
		private void StartPackaging()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsStationReady(this.Press))
			{
				Console.LogWarning(base.Npc.fullName + " has no station to work with", null);
				base.Disable_Networked(null);
				return;
			}
			this.Press.SetNPCUser(base.Npc.NetworkObject);
		}

		// Token: 0x06001CEB RID: 7403 RVA: 0x0007785C File Offset: 0x00075A5C
		public void AssignStation(BrickPress press)
		{
			this.Press = press;
		}

		// Token: 0x06001CEC RID: 7404 RVA: 0x00077865 File Offset: 0x00075A65
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(this.Press.StandPoint.position, 0.5f);
		}

		// Token: 0x06001CED RID: 7405 RVA: 0x0007788C File Offset: 0x00075A8C
		public void GoToStation()
		{
			base.Npc.Movement.SetDestination(this.Press.StandPoint.position);
		}

		// Token: 0x06001CEE RID: 7406 RVA: 0x000778B0 File Offset: 0x00075AB0
		[ObserversRpc(RunLocally = true)]
		public void BeginPackaging()
		{
			this.RpcWriter___Observers_BeginPackaging_2166136261();
			this.RpcLogic___BeginPackaging_2166136261();
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000778C9 File Offset: 0x00075AC9
		private void StopPackaging()
		{
			if (this.packagingRoutine != null)
			{
				base.StopCoroutine(this.packagingRoutine);
			}
			this.PackagingInProgress = false;
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000778E8 File Offset: 0x00075AE8
		public bool IsStationReady(BrickPress press)
		{
			return !(press == null) && press.GetState() == PackagingStation.EState.CanBegin && (!((IUsable)press).IsInUse || !(press.NPCUserObject != base.Npc.NetworkObject)) && base.Npc.Movement.CanGetTo(press.StandPoint.position, 1f);
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x00077951 File Offset: 0x00075B51
		[CompilerGenerated]
		private IEnumerator <BeginPackaging>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", true);
			float packageTime = 15f / (base.Npc as Packager).PackagingSpeedMultiplier;
			for (float i = 0f; i < packageTime; i += Time.deltaTime)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.Press.uiPoint.position, 0, false);
				yield return new WaitForEndOfFrame();
			}
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", false);
			yield return new WaitForSeconds(0.2f);
			base.Npc.Avatar.Anim.SetTrigger("GrabItem");
			this.Press.PlayPressAnim();
			yield return new WaitForSeconds(1f);
			ProductItemInstance product;
			if (InstanceFinder.IsServer && this.Press.HasSufficientProduct(out product))
			{
				this.Press.CompletePress(product);
			}
			this.PackagingInProgress = false;
			this.packagingRoutine = null;
			yield break;
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00077960 File Offset: 0x00075B60
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BrickPressBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.BrickPressBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginPackaging_2166136261));
		}

		// Token: 0x06001CF4 RID: 7412 RVA: 0x00077990 File Offset: 0x00075B90
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BrickPressBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.BrickPressBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001CF5 RID: 7413 RVA: 0x000779A9 File Offset: 0x00075BA9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001CF6 RID: 7414 RVA: 0x000779B8 File Offset: 0x00075BB8
		private void RpcWriter___Observers_BeginPackaging_2166136261()
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

		// Token: 0x06001CF7 RID: 7415 RVA: 0x00077A64 File Offset: 0x00075C64
		public void RpcLogic___BeginPackaging_2166136261()
		{
			if (this.PackagingInProgress)
			{
				return;
			}
			if (this.Press == null)
			{
				return;
			}
			this.PackagingInProgress = true;
			base.Npc.Movement.FaceDirection(this.Press.StandPoint.forward, 0.5f);
			this.packagingRoutine = base.StartCoroutine(this.<BeginPackaging>g__Package|20_0());
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x00077AC8 File Offset: 0x00075CC8
		private void RpcReader___Observers_BeginPackaging_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginPackaging_2166136261();
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x00077AF2 File Offset: 0x00075CF2
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001778 RID: 6008
		public const float BASE_PACKAGING_TIME = 15f;

		// Token: 0x0400177B RID: 6011
		private Coroutine packagingRoutine;

		// Token: 0x0400177C RID: 6012
		private bool dll_Excuted;

		// Token: 0x0400177D RID: 6013
		private bool dll_Excuted;
	}
}
