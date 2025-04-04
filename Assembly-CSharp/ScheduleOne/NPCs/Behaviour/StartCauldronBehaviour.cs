using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000501 RID: 1281
	public class StartCauldronBehaviour : Behaviour
	{
		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001E3A RID: 7738 RVA: 0x0007C5B8 File Offset: 0x0007A7B8
		// (set) Token: 0x06001E3B RID: 7739 RVA: 0x0007C5C0 File Offset: 0x0007A7C0
		public Cauldron Station { get; protected set; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x0007C5C9 File Offset: 0x0007A7C9
		// (set) Token: 0x06001E3D RID: 7741 RVA: 0x0007C5D1 File Offset: 0x0007A7D1
		public bool WorkInProgress { get; protected set; }

		// Token: 0x06001E3E RID: 7742 RVA: 0x0007C5DA File Offset: 0x0007A7DA
		protected override void Begin()
		{
			base.Begin();
			this.StartWork();
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0007C5E8 File Offset: 0x0007A7E8
		protected override void Resume()
		{
			base.Resume();
			this.StartWork();
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0007C5F8 File Offset: 0x0007A7F8
		protected override void Pause()
		{
			base.Pause();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x00077227 File Offset: 0x00075427
		public override void Disable()
		{
			base.Disable();
			if (base.Active)
			{
				this.End();
			}
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0007C658 File Offset: 0x0007A858
		protected override void End()
		{
			base.End();
			if (this.WorkInProgress)
			{
				this.StopCauldron();
			}
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0007C6B8 File Offset: 0x0007A8B8
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.WorkInProgress)
			{
				if (this.IsStationReady(this.Station))
				{
					if (base.Npc.Movement.IsMoving)
					{
						return;
					}
					if (this.IsAtStation())
					{
						this.BeginCauldron();
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

		// Token: 0x06001E44 RID: 7748 RVA: 0x0007C71C File Offset: 0x0007A91C
		private void StartWork()
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

		// Token: 0x06001E45 RID: 7749 RVA: 0x0007C778 File Offset: 0x0007A978
		public void AssignStation(Cauldron station)
		{
			this.Station = station;
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0007C781 File Offset: 0x0007A981
		public bool IsAtStation()
		{
			return base.Npc.Movement.IsAsCloseAsPossible(this.Station.StandPoint.position, 0.5f);
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0007C7A8 File Offset: 0x0007A9A8
		public void GoToStation()
		{
			base.SetDestination(this.Station.StandPoint.position, true);
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0007C7C4 File Offset: 0x0007A9C4
		[ObserversRpc(RunLocally = true)]
		public void BeginCauldron()
		{
			this.RpcWriter___Observers_BeginCauldron_2166136261();
			this.RpcLogic___BeginCauldron_2166136261();
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0007C7E0 File Offset: 0x0007A9E0
		private void StopCauldron()
		{
			if (this.workRoutine != null)
			{
				base.StopCoroutine(this.workRoutine);
			}
			if (InstanceFinder.IsServer && this.Station != null && this.Station.NPCUserObject == base.Npc.NetworkObject)
			{
				this.Station.SetNPCUser(null);
			}
			this.WorkInProgress = false;
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x0007C848 File Offset: 0x0007AA48
		public bool IsStationReady(Cauldron station)
		{
			return !(station == null) && station.GetState() == Cauldron.EState.Ready && (!((IUsable)station).IsInUse || (!(station.PlayerUserObject != null) && !(station.NPCUserObject != base.Npc.NetworkObject))) && base.Npc.Movement.CanGetTo(station.StandPoint.position, 1f);
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x0007C8C0 File Offset: 0x0007AAC0
		[CompilerGenerated]
		private IEnumerator <BeginCauldron>g__Package|20_0()
		{
			yield return new WaitForEndOfFrame();
			base.Npc.Avatar.Anim.SetBool("UseChemistryStation", true);
			float packageTime = 15f;
			for (float i = 0f; i < packageTime; i += Time.deltaTime)
			{
				base.Npc.Avatar.LookController.OverrideLookTarget(this.Station.LinkOrigin.position, 0, false);
				yield return new WaitForEndOfFrame();
			}
			base.Npc.Avatar.Anim.SetBool("UseChemistryStation", false);
			if (InstanceFinder.IsServer)
			{
				EQuality quality = this.Station.RemoveIngredients();
				this.Station.StartCookOperation(null, this.Station.CookTime, quality);
			}
			this.WorkInProgress = false;
			this.workRoutine = null;
			yield break;
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0007C8CF File Offset: 0x0007AACF
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartCauldronBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.StartCauldronBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_BeginCauldron_2166136261));
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x0007C8FF File Offset: 0x0007AAFF
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartCauldronBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.StartCauldronBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0007C918 File Offset: 0x0007AB18
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x0007C928 File Offset: 0x0007AB28
		private void RpcWriter___Observers_BeginCauldron_2166136261()
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

		// Token: 0x06001E51 RID: 7761 RVA: 0x0007C9D4 File Offset: 0x0007ABD4
		public void RpcLogic___BeginCauldron_2166136261()
		{
			if (this.WorkInProgress)
			{
				return;
			}
			if (this.Station == null)
			{
				return;
			}
			this.WorkInProgress = true;
			base.Npc.Movement.FaceDirection(this.Station.StandPoint.forward, 0.5f);
			this.workRoutine = base.StartCoroutine(this.<BeginCauldron>g__Package|20_0());
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x0007CA38 File Offset: 0x0007AC38
		private void RpcReader___Observers_BeginCauldron_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginCauldron_2166136261();
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0007CA62 File Offset: 0x0007AC62
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001806 RID: 6150
		public const float START_CAULDRON_TIME = 15f;

		// Token: 0x04001809 RID: 6153
		private Coroutine workRoutine;

		// Token: 0x0400180A RID: 6154
		private bool dll_Excuted;

		// Token: 0x0400180B RID: 6155
		private bool dll_Excuted;
	}
}
