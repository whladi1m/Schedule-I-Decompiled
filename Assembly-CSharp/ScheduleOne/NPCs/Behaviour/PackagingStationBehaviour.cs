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
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004F5 RID: 1269
	public class PackagingStationBehaviour : Behaviour
	{
		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x0007A0A8 File Offset: 0x000782A8
		// (set) Token: 0x06001DA7 RID: 7591 RVA: 0x0007A0B0 File Offset: 0x000782B0
		public PackagingStation Station { get; protected set; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0007A0B9 File Offset: 0x000782B9
		// (set) Token: 0x06001DA9 RID: 7593 RVA: 0x0007A0C1 File Offset: 0x000782C1
		public bool PackagingInProgress { get; protected set; }

		// Token: 0x06001DAA RID: 7594 RVA: 0x0007A0CA File Offset: 0x000782CA
		protected override void Begin()
		{
			base.Begin();
			this.StartPackaging();
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0007A0D8 File Offset: 0x000782D8
		protected override void Resume()
		{
			base.Resume();
			this.StartPackaging();
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0007A0E6 File Offset: 0x000782E6
		protected override void Pause()
		{
			base.Pause();
			if (this.PackagingInProgress)
			{
				this.StopPackaging();
			}
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0007A0FC File Offset: 0x000782FC
		protected override void End()
		{
			base.End();
			if (this.PackagingInProgress)
			{
				this.StopPackaging();
			}
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x0007A15C File Offset: 0x0007835C
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.PackagingInProgress)
			{
				if (this.IsStationReady(this.Station))
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

		// Token: 0x06001DB0 RID: 7600 RVA: 0x0007A1C0 File Offset: 0x000783C0
		private void StartPackaging()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.IsStationReady(this.Station))
			{
				Console.LogWarning(base.Npc.fullName + " has no station to work with", null);
				base.Disable_Networked(null);
				return;
			}
			this.Station.SetNPCUser(base.Npc.NetworkObject);
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x0007A21C File Offset: 0x0007841C
		public void AssignStation(PackagingStation station)
		{
			this.Station = station;
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x0007A225 File Offset: 0x00078425
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(this.Station.StandPoint.position, 0.5f);
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x0007A24C File Offset: 0x0007844C
		public void GoToStation()
		{
			base.Npc.Movement.SetDestination(this.Station.StandPoint.position);
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x0007A270 File Offset: 0x00078470
		[ObserversRpc(RunLocally = true)]
		public void BeginPackaging()
		{
			this.RpcWriter___Observers_BeginPackaging_2166136261();
			this.RpcLogic___BeginPackaging_2166136261();
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x0007A28C File Offset: 0x0007848C
		private void StopPackaging()
		{
			if (this.packagingRoutine != null)
			{
				base.StopCoroutine(this.packagingRoutine);
			}
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", false);
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
			this.PackagingInProgress = false;
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x0007A310 File Offset: 0x00078510
		public bool IsStationReady(PackagingStation station)
		{
			return !(station == null) && station.GetState(PackagingStation.EMode.Package) == PackagingStation.EState.CanBegin && (!((IUsable)station).IsInUse || !(station.NPCUserObject != base.Npc.NetworkObject)) && base.Npc.Movement.CanGetTo(station.StandPoint.position, 1f);
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x0007A37A File Offset: 0x0007857A
		[CompilerGenerated]
		private IEnumerator <BeginPackaging>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", true);
			float packageTime = 5f / (base.Npc as Packager).PackagingSpeedMultiplier * this.Station.PackagerEmployeeSpeedMultiplier;
			for (float i = 0f; i < packageTime; i += Time.deltaTime)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.Station.Container.position, 0, false);
				yield return new WaitForEndOfFrame();
			}
			base.Npc.Avatar.Anim.SetBool("UsePackagingStation", false);
			if (InstanceFinder.IsServer)
			{
				this.Station.PackSingleInstance();
			}
			Console.Log("Packaging done!", null);
			this.PackagingInProgress = false;
			this.packagingRoutine = null;
			yield break;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x0007A389 File Offset: 0x00078589
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PackagingStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.PackagingStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginPackaging_2166136261));
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x0007A3B9 File Offset: 0x000785B9
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PackagingStationBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.PackagingStationBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x0007A3D2 File Offset: 0x000785D2
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x0007A3E0 File Offset: 0x000785E0
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

		// Token: 0x06001DBD RID: 7613 RVA: 0x0007A48C File Offset: 0x0007868C
		public void RpcLogic___BeginPackaging_2166136261()
		{
			if (this.PackagingInProgress)
			{
				return;
			}
			if (this.Station == null)
			{
				return;
			}
			this.PackagingInProgress = true;
			base.Npc.Movement.FaceDirection(this.Station.StandPoint.forward, 0.5f);
			this.packagingRoutine = base.StartCoroutine(this.<BeginPackaging>g__Package|20_0());
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0007A4F0 File Offset: 0x000786F0
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

		// Token: 0x06001DBF RID: 7615 RVA: 0x0007A51A File Offset: 0x0007871A
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040017C3 RID: 6083
		public const float BASE_PACKAGING_TIME = 5f;

		// Token: 0x040017C6 RID: 6086
		private Coroutine packagingRoutine;

		// Token: 0x040017C7 RID: 6087
		private bool dll_Excuted;

		// Token: 0x040017C8 RID: 6088
		private bool dll_Excuted;
	}
}
