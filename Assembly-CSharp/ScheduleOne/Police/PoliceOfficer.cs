using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework.Customization;
using ScheduleOne.AvatarFramework.Equipping;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.FX;
using ScheduleOne.Law;
using ScheduleOne.Map;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Behaviour;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Vehicles;
using ScheduleOne.Vision;
using ScheduleOne.VoiceOver;
using UnityEngine;

namespace ScheduleOne.Police
{
	// Token: 0x02000334 RID: 820
	public class PoliceOfficer : NPC
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060011F9 RID: 4601 RVA: 0x0004E2F0 File Offset: 0x0004C4F0
		// (set) Token: 0x060011FA RID: 4602 RVA: 0x0004E2F8 File Offset: 0x0004C4F8
		public NetworkObject TargetPlayerNOB
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<TargetPlayerNOB>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<TargetPlayerNOB>k__BackingField(value, true);
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0004E304 File Offset: 0x0004C504
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Police.PoliceOfficer_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0004E323 File Offset: 0x0004C523
		protected override void Start()
		{
			base.Start();
			this.belt = this.Avatar.GetComponentInChildren<PoliceBelt>();
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0004E33C File Offset: 0x0004C53C
		protected override void Update()
		{
			base.Update();
			if (InstanceFinder.IsServer)
			{
				this.UpdateBodySearch();
			}
			this.UpdateChatter();
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0004E358 File Offset: 0x0004C558
		protected void FixedUpdate()
		{
			for (int i = 0; i < Player.PlayerList.Count; i++)
			{
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Wanted].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Suspicious].Enabled = (this.TargetPlayerNOB == null && !Player.PlayerList[i].CrimeData.BodySearchPending && Player.PlayerList[i].CrimeData.TimeSinceLastBodySearch > 30f);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.DisobeyingCurfew].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.DrugDealing].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Vandalizing].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Pickpocketing].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.Brandishing].Enabled = (this.TargetPlayerNOB == null);
				this.awareness.VisionCone.StateSettings[Player.PlayerList[i]][PlayerVisualState.EVisualState.DischargingWeapon].Enabled = (this.TargetPlayerNOB == null);
			}
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0004E572 File Offset: 0x0004C772
		protected override void MinPass()
		{
			base.MinPass();
			if (base.CurrentBuilding == null && InstanceFinder.IsServer && this.AutoDeactivate)
			{
				this.CheckDeactivation();
			}
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0004E5A0 File Offset: 0x0004C7A0
		private void CheckDeactivation()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.TargetPlayerNOB != null)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.behaviour.ScheduleManager.ActiveAction != null)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.CheckpointBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.FootPatrolBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.VehiclePatrolBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.BodySearchBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.SentryBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (!base.IsConscious)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.behaviour.RagdollBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.behaviour.GenericDialogueBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			if (this.behaviour.FacePlayerBehaviour.Active)
			{
				this.timeSinceReadyToPool = 0f;
				this.timeSinceOutOfSight = 0f;
				return;
			}
			this.timeSinceReadyToPool += 1f;
			if (this.timeSinceReadyToPool < 1f)
			{
				return;
			}
			if (!this.movement.IsMoving && Singleton<Map>.InstanceExists)
			{
				if (this.movement.IsAsCloseAsPossible(Singleton<Map>.Instance.PoliceStation.Doors[0].transform.position, 1f))
				{
					this.Deactivate();
					return;
				}
				if (this.movement.CanGetTo(Singleton<Map>.Instance.PoliceStation.Doors[0].transform.position, 1f))
				{
					this.movement.SetDestination(Singleton<Map>.Instance.PoliceStation.Doors[0].transform.position);
				}
				else
				{
					this.Deactivate();
				}
			}
			bool flag = false;
			foreach (Player player in Player.PlayerList)
			{
				if (player.IsPointVisibleToPlayer(this.Avatar.CenterPoint, 30f, 5f))
				{
					flag = true;
					break;
				}
				if (this.AssignedVehicle != null && player.IsPointVisibleToPlayer(this.AssignedVehicle.transform.position, 30f, 5f))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.timeSinceReadyToPool += 1f;
				this.timeSinceOutOfSight += 1f;
				if (this.timeSinceOutOfSight > 1f)
				{
					this.Deactivate();
					return;
				}
			}
			else
			{
				this.timeSinceOutOfSight = 0f;
			}
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0004E8FC File Offset: 0x0004CAFC
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public virtual void BeginFootPursuit_Networked(NetworkObject target, bool includeColleagues = true)
		{
			this.RpcWriter___Server_BeginFootPursuit_Networked_419679943(target, includeColleagues);
			this.RpcLogic___BeginFootPursuit_Networked_419679943(target, includeColleagues);
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0004E928 File Offset: 0x0004CB28
		[ObserversRpc(RunLocally = true)]
		private void BeginFootPursuitTest(string playerCode)
		{
			this.RpcWriter___Observers_BeginFootPursuitTest_3615296227(playerCode);
			this.RpcLogic___BeginFootPursuitTest_3615296227(playerCode);
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0004E949 File Offset: 0x0004CB49
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public virtual void BeginVehiclePursuit_Networked(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			this.RpcWriter___Server_BeginVehiclePursuit_Networked_2261819652(target, vehicle, beginAsSighted);
			this.RpcLogic___BeginVehiclePursuit_Networked_2261819652(target, vehicle, beginAsSighted);
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0004E970 File Offset: 0x0004CB70
		[ObserversRpc(RunLocally = true)]
		private void BeginVehiclePursuit(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			this.RpcWriter___Observers_BeginVehiclePursuit_2261819652(target, vehicle, beginAsSighted);
			this.RpcLogic___BeginVehiclePursuit_2261819652(target, vehicle, beginAsSighted);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0004E9A1 File Offset: 0x0004CBA1
		public void BeginBodySearch_LocalPlayer()
		{
			this.BeginBodySearch_Networked(Player.Local.NetworkObject);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0004E9B3 File Offset: 0x0004CBB3
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public virtual void BeginBodySearch_Networked(NetworkObject target)
		{
			this.RpcWriter___Server_BeginBodySearch_Networked_3323014238(target);
			this.RpcLogic___BeginBodySearch_Networked_3323014238(target);
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0004E9C9 File Offset: 0x0004CBC9
		[ObserversRpc(RunLocally = true)]
		private void BeginBodySearch(NetworkObject target)
		{
			this.RpcWriter___Observers_BeginBodySearch_3323014238(target);
			this.RpcLogic___BeginBodySearch_3323014238(target);
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0004E9E0 File Offset: 0x0004CBE0
		[ObserversRpc(RunLocally = true)]
		public virtual void AssignToCheckpoint(CheckpointManager.ECheckpointLocation location)
		{
			this.RpcWriter___Observers_AssignToCheckpoint_4087078542(location);
			this.RpcLogic___AssignToCheckpoint_4087078542(location);
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0004EA01 File Offset: 0x0004CC01
		public void UnassignFromCheckpoint()
		{
			this.CheckpointBehaviour.Disable_Networked(null);
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = null;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x0004EA20 File Offset: 0x0004CC20
		public void StartFootPatrol(PatrolGroup group, bool warpToStartPoint)
		{
			this.FootPatrolBehaviour.SetGroup(group);
			this.FootPatrolBehaviour.Enable_Networked(null);
			if (warpToStartPoint)
			{
				this.movement.Warp(group.GetDestination(this));
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0004EA4F File Offset: 0x0004CC4F
		public void StartVehiclePatrol(VehiclePatrolRoute route, LandVehicle vehicle)
		{
			this.VehiclePatrolBehaviour.Vehicle = vehicle;
			this.VehiclePatrolBehaviour.SetRoute(route);
			this.VehiclePatrolBehaviour.Enable_Networked(null);
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0004EA75 File Offset: 0x0004CC75
		public virtual void AssignToSentryLocation(SentryLocation location)
		{
			this.SentryBehaviour.AssignLocation(location);
			this.SentryBehaviour.Enable();
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0004EA8E File Offset: 0x0004CC8E
		public void UnassignFromSentryLocation()
		{
			this.SentryBehaviour.UnassignLocation();
			this.SentryBehaviour.Disable();
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0004EAA6 File Offset: 0x0004CCA6
		public void Activate()
		{
			this.timeSinceReadyToPool = 0f;
			this.timeSinceOutOfSight = 0f;
			base.ExitBuilding("");
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0004EACC File Offset: 0x0004CCCC
		public void Deactivate()
		{
			if (!InstanceFinder.IsServer)
			{
				Console.LogError("Attempted to deactivate an officer on the client", null);
				return;
			}
			if (this.AssignedVehicle != null)
			{
				Singleton<CoroutineService>.Instance.StartCoroutine(this.<Deactivate>g__Wait|59_0());
			}
			base.EnterBuilding(null, Singleton<Map>.Instance.PoliceStation.GUID.ToString(), 0);
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x0004EB30 File Offset: 0x0004CD30
		protected override bool ShouldNoticeGeneralCrime(Player player)
		{
			return !(this.TargetPlayerNOB != null) && base.ShouldNoticeGeneralCrime(player);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00014002 File Offset: 0x00012202
		public override bool ShouldSave()
		{
			return false;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0004EB49 File Offset: 0x0004CD49
		public override string GetNameAddress()
		{
			return "Officer " + this.LastName;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0004EB5C File Offset: 0x0004CD5C
		private void UpdateChatter()
		{
			this.chatterCountDown -= Time.deltaTime;
			if (this.chatterCountDown <= 0f)
			{
				this.chatterCountDown = UnityEngine.Random.Range(15f, 45f);
				if (this.ChatterEnabled && this.ChatterVO.gameObject.activeInHierarchy)
				{
					this.ChatterVO.Play(EVOLineType.PoliceChatter);
				}
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x0004EBC4 File Offset: 0x0004CDC4
		private void ProcessVisionEvent(VisionEventReceipt visionEventReceipt)
		{
			if (PoliceOfficer.OnPoliceVisionEvent != null)
			{
				PoliceOfficer.OnPoliceVisionEvent(visionEventReceipt);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0004EBD8 File Offset: 0x0004CDD8
		public virtual void UpdateBodySearch()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.CanInvestigate())
			{
				return;
			}
			if (this.currentBodySearchInvestigation != null)
			{
				this.UpdateExistingInvestigation();
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x0004EBF9 File Offset: 0x0004CDF9
		private bool CanInvestigate()
		{
			return !this.VehiclePursuitBehaviour.Active && !this.PursuitBehaviour.Active && !this.BodySearchBehaviour.Active && !(base.CurrentBuilding != null);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0004EC38 File Offset: 0x0004CE38
		private void UpdateExistingInvestigation()
		{
			if (!this.CanInvestigatePlayer(this.currentBodySearchInvestigation.Target))
			{
				this.StopBodySearchInvestigation();
				return;
			}
			Player target = this.currentBodySearchInvestigation.Target;
			float playerVisibility = this.awareness.VisionCone.GetPlayerVisibility(target);
			float suspiciousness = target.VisualState.Suspiciousness;
			float num = Mathf.Lerp(0.2f, 2f, suspiciousness);
			float num2 = Mathf.Lerp(0.4f, 1f, playerVisibility);
			float num3 = Mathf.Lerp(1f, 0.05f, Vector3.Distance(this.Avatar.CenterPoint, target.Avatar.CenterPoint) / 12f);
			float num4 = num2 * num * num3;
			if (Application.isEditor && Input.GetKey(KeyCode.B))
			{
				num4 = 0.5f;
			}
			if (num4 < 0.08f)
			{
				num4 = -0.08f;
			}
			else if (num4 < 0.12f)
			{
				num4 = 0f;
			}
			this.currentBodySearchInvestigation.ChangeProgress(num4 * Time.deltaTime);
			if (this.currentBodySearchInvestigation.CurrentProgress >= 1f)
			{
				this.ConductBodySearch(this.currentBodySearchInvestigation.Target);
				this.StopBodySearchInvestigation();
				return;
			}
			if (this.currentBodySearchInvestigation.CurrentProgress <= -0.1f)
			{
				this.StopBodySearchInvestigation();
				return;
			}
			if (this.currentBodySearchInvestigation.CurrentProgress >= 0f)
			{
				float speed = Mathf.Lerp(0.05f, 0f, this.currentBodySearchInvestigation.CurrentProgress);
				base.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("consideringbodysearch", 5, speed));
				this.Avatar.LookController.OverrideLookTarget(target.EyePosition, 10, this.currentBodySearchInvestigation.CurrentProgress >= 0.2f);
			}
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0004EDEC File Offset: 0x0004CFEC
		private void CheckNewInvestigation()
		{
			if (this.currentBodySearchInvestigation != null)
			{
				return;
			}
			if (!this.CanInvestigate())
			{
				return;
			}
			if (this.BodySearchChance <= 0f)
			{
				return;
			}
			foreach (Player player in Player.PlayerList)
			{
				if (this.CanInvestigatePlayer(player) && Vector3.Distance(this.Avatar.CenterPoint, player.Avatar.CenterPoint) <= 8f)
				{
					float playerVisibility = this.awareness.VisionCone.GetPlayerVisibility(player);
					if (playerVisibility >= 0.2f)
					{
						float suspiciousness = player.VisualState.Suspiciousness;
						float num = Mathf.Lerp(0.2f, 2f, suspiciousness);
						float num2 = Mathf.Lerp(0.4f, 1f, playerVisibility);
						float num3 = Mathf.Lerp(0.5f, 1f, this.Suspicion);
						float num4 = Mathf.Clamp01(this.BodySearchChance * num * num2 * num3 * 1f);
						if (UnityEngine.Random.Range(0f, 1f) < num4)
						{
							this.currentBodySearchInvestigation = new Investigation(player);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0004EF30 File Offset: 0x0004D130
		private void StartBodySearchInvestigation(Player player)
		{
			Console.Log("Starting body search investigation", null);
			this.currentBodySearchInvestigation = new Investigation(player);
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0004EF49 File Offset: 0x0004D149
		private void StopBodySearchInvestigation()
		{
			this.currentBodySearchInvestigation = null;
			base.Movement.SpeedController.RemoveSpeedControl("consideringbodysearch");
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0004EF67 File Offset: 0x0004D167
		public void ConductBodySearch(Player player)
		{
			Console.Log("Conducting body search on " + player.PlayerName, null);
			this.BodySearchBehaviour.AssignTarget(null, player.NetworkObject);
			this.BodySearchBehaviour.Enable_Networked(null);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0004EFA0 File Offset: 0x0004D1A0
		private bool CanInvestigatePlayer(Player player)
		{
			return !(player == null) && player.Health.IsAlive && !player.CrimeData.BodySearchPending && player.CrimeData.CurrentPursuitLevel <= PlayerCrimeData.EPursuitLevel.None && player.CrimeData.TimeSinceLastBodySearch >= 60f && !player.IsArrested;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0004F06A File Offset: 0x0004D26A
		[CompilerGenerated]
		private IEnumerator <Deactivate>g__Wait|59_0()
		{
			yield return new WaitUntil(() => !this.AssignedVehicle.isOccupied);
			this.AssignedVehicle.DestroyVehicle();
			yield break;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0004F08C File Offset: 0x0004D28C
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Police.PoliceOfficerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Police.PoliceOfficerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<TargetPlayerNOB>k__BackingField = new SyncVar<NetworkObject>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<TargetPlayerNOB>k__BackingField);
			base.RegisterServerRpc(35U, new ServerRpcDelegate(this.RpcReader___Server_BeginFootPursuit_Networked_419679943));
			base.RegisterObserversRpc(36U, new ClientRpcDelegate(this.RpcReader___Observers_BeginFootPursuitTest_3615296227));
			base.RegisterServerRpc(37U, new ServerRpcDelegate(this.RpcReader___Server_BeginVehiclePursuit_Networked_2261819652));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_BeginVehiclePursuit_2261819652));
			base.RegisterServerRpc(39U, new ServerRpcDelegate(this.RpcReader___Server_BeginBodySearch_Networked_3323014238));
			base.RegisterObserversRpc(40U, new ClientRpcDelegate(this.RpcReader___Observers_BeginBodySearch_3323014238));
			base.RegisterObserversRpc(41U, new ClientRpcDelegate(this.RpcReader___Observers_AssignToCheckpoint_4087078542));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Police.PoliceOfficer));
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0004F18E File Offset: 0x0004D38E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Police.PoliceOfficerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Police.PoliceOfficerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<TargetPlayerNOB>k__BackingField.SetRegistered();
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0004F1B2 File Offset: 0x0004D3B2
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0004F1C0 File Offset: 0x0004D3C0
		private void RpcWriter___Server_BeginFootPursuit_Networked_419679943(NetworkObject target, bool includeColleagues = true)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(target);
			writer.WriteBoolean(includeColleagues);
			base.SendServerRpc(35U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0004F274 File Offset: 0x0004D474
		public virtual void RpcLogic___BeginFootPursuit_Networked_419679943(NetworkObject target, bool includeColleagues = true)
		{
			if (target == null)
			{
				Console.LogError("Attempted to begin foot pursuit with null target", null);
				return;
			}
			this.BeginFootPursuitTest(target.GetComponent<Player>().PlayerCode);
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			this.BodySearchBehaviour.Disable_Networked(null);
			if (includeColleagues)
			{
				if (this.FootPatrolBehaviour.Enabled && this.FootPatrolBehaviour.Group != null)
				{
					for (int i = 0; i < this.FootPatrolBehaviour.Group.Members.Count; i++)
					{
						if (!(this.FootPatrolBehaviour.Group.Members[i] == this))
						{
							(this.FootPatrolBehaviour.Group.Members[i] as PoliceOfficer).BeginFootPursuitTest(target.GetComponent<Player>().PlayerCode);
						}
					}
				}
				if (this.CheckpointBehaviour.Enabled && this.CheckpointBehaviour.Checkpoint != null)
				{
					for (int j = 0; j < this.CheckpointBehaviour.Checkpoint.AssignedNPCs.Count; j++)
					{
						if (!(this.CheckpointBehaviour.Checkpoint.AssignedNPCs[j] == this))
						{
							(this.CheckpointBehaviour.Checkpoint.AssignedNPCs[j] as PoliceOfficer).BeginFootPursuitTest(target.GetComponent<Player>().PlayerCode);
						}
					}
				}
				if (this.SentryBehaviour.Enabled && this.SentryBehaviour.AssignedLocation != null)
				{
					for (int k = 0; k < this.SentryBehaviour.AssignedLocation.AssignedOfficers.Count; k++)
					{
						if (!(this.SentryBehaviour.AssignedLocation.AssignedOfficers[k] == this))
						{
							this.SentryBehaviour.AssignedLocation.AssignedOfficers[k].BeginFootPursuitTest(target.GetComponent<Player>().PlayerCode);
						}
					}
				}
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0004F454 File Offset: 0x0004D654
		private void RpcReader___Server_BeginFootPursuit_Networked_419679943(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			bool includeColleagues = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___BeginFootPursuit_Networked_419679943(target, includeColleagues);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0004F4A4 File Offset: 0x0004D6A4
		private void RpcWriter___Observers_BeginFootPursuitTest_3615296227(string playerCode)
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
			writer.WriteString(playerCode);
			base.SendObserversRpc(36U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0004F55C File Offset: 0x0004D75C
		private void RpcLogic___BeginFootPursuitTest_3615296227(string playerCode)
		{
			this.TargetPlayerNOB = Player.GetPlayer(playerCode).NetworkObject;
			if (this.TargetPlayerNOB == null)
			{
				Console.LogError("Attempted to begin foot pursuit with null target", null);
				return;
			}
			this.PursuitBehaviour.AssignTarget(null, this.TargetPlayerNOB);
			this.PursuitBehaviour.Enable();
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0004F5B4 File Offset: 0x0004D7B4
		private void RpcReader___Observers_BeginFootPursuitTest_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string playerCode = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginFootPursuitTest_3615296227(playerCode);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0004F5F0 File Offset: 0x0004D7F0
		private void RpcWriter___Server_BeginVehiclePursuit_Networked_2261819652(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(target);
			writer.WriteNetworkObject(vehicle);
			writer.WriteBoolean(beginAsSighted);
			base.SendServerRpc(37U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0004F6B1 File Offset: 0x0004D8B1
		public virtual void RpcLogic___BeginVehiclePursuit_Networked_2261819652(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			this.BeginVehiclePursuit(target, vehicle, beginAsSighted);
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0004F6BC File Offset: 0x0004D8BC
		private void RpcReader___Server_BeginVehiclePursuit_Networked_2261819652(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			NetworkObject vehicle = PooledReader0.ReadNetworkObject();
			bool beginAsSighted = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___BeginVehiclePursuit_Networked_2261819652(target, vehicle, beginAsSighted);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0004F71C File Offset: 0x0004D91C
		private void RpcWriter___Observers_BeginVehiclePursuit_2261819652(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
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
			writer.WriteNetworkObject(target);
			writer.WriteNetworkObject(vehicle);
			writer.WriteBoolean(beginAsSighted);
			base.SendObserversRpc(38U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0004F7EC File Offset: 0x0004D9EC
		private void RpcLogic___BeginVehiclePursuit_2261819652(NetworkObject target, NetworkObject vehicle, bool beginAsSighted)
		{
			this.TargetPlayerNOB = target.GetComponent<Player>().NetworkObject;
			this.VehiclePursuitBehaviour.vehicle = vehicle.GetComponent<LandVehicle>();
			this.VehiclePursuitBehaviour.AssignTarget(this.TargetPlayerNOB.GetComponent<Player>());
			if (beginAsSighted)
			{
				this.VehiclePursuitBehaviour.BeginAsSighted();
			}
			this.VehiclePursuitBehaviour.Enable();
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0004F84C File Offset: 0x0004DA4C
		private void RpcReader___Observers_BeginVehiclePursuit_2261819652(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			NetworkObject vehicle = PooledReader0.ReadNetworkObject();
			bool beginAsSighted = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginVehiclePursuit_2261819652(target, vehicle, beginAsSighted);
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0004F8AC File Offset: 0x0004DAAC
		private void RpcWriter___Server_BeginBodySearch_Networked_3323014238(NetworkObject target)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkObject(target);
			base.SendServerRpc(39U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0004F953 File Offset: 0x0004DB53
		public virtual void RpcLogic___BeginBodySearch_Networked_3323014238(NetworkObject target)
		{
			this.BeginBodySearch(target);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0004F95C File Offset: 0x0004DB5C
		private void RpcReader___Server_BeginBodySearch_Networked_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___BeginBodySearch_Networked_3323014238(target);
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0004F99C File Offset: 0x0004DB9C
		private void RpcWriter___Observers_BeginBodySearch_3323014238(NetworkObject target)
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
			writer.WriteNetworkObject(target);
			base.SendObserversRpc(40U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0004FA52 File Offset: 0x0004DC52
		private void RpcLogic___BeginBodySearch_3323014238(NetworkObject target)
		{
			this.TargetPlayerNOB = target.GetComponent<Player>().NetworkObject;
			this.BodySearchBehaviour.AssignTarget(null, target);
			this.BodySearchBehaviour.Enable();
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0004FA80 File Offset: 0x0004DC80
		private void RpcReader___Observers_BeginBodySearch_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject target = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___BeginBodySearch_3323014238(target);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0004FABC File Offset: 0x0004DCBC
		private void RpcWriter___Observers_AssignToCheckpoint_4087078542(CheckpointManager.ECheckpointLocation location)
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
			writer.Write___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generated(location);
			base.SendObserversRpc(41U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0004FB74 File Offset: 0x0004DD74
		public virtual void RpcLogic___AssignToCheckpoint_4087078542(CheckpointManager.ECheckpointLocation location)
		{
			this.movement.Warp(NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(location).transform.position);
			this.CheckpointBehaviour.SetCheckpoint(location);
			this.CheckpointBehaviour.Enable();
			this.dialogueHandler.GetComponent<DialogueController>().OverrideContainer = this.CheckpointDialogue;
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0004FBD0 File Offset: 0x0004DDD0
		private void RpcReader___Observers_AssignToCheckpoint_4087078542(PooledReader PooledReader0, Channel channel)
		{
			CheckpointManager.ECheckpointLocation location = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AssignToCheckpoint_4087078542(location);
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600123A RID: 4666 RVA: 0x0004FC0B File Offset: 0x0004DE0B
		// (set) Token: 0x0600123B RID: 4667 RVA: 0x0004FC13 File Offset: 0x0004DE13
		public NetworkObject SyncAccessor_<TargetPlayerNOB>k__BackingField
		{
			get
			{
				return this.<TargetPlayerNOB>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<TargetPlayerNOB>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<TargetPlayerNOB>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0004FC50 File Offset: 0x0004DE50
		public virtual bool PoliceOfficer(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 != 1U)
			{
				return false;
			}
			if (PooledReader0 == null)
			{
				this.sync___set_value_<TargetPlayerNOB>k__BackingField(this.syncVar___<TargetPlayerNOB>k__BackingField.GetValue(true), true);
				return true;
			}
			NetworkObject value = PooledReader0.ReadNetworkObject();
			this.sync___set_value_<TargetPlayerNOB>k__BackingField(value, Boolean2);
			return true;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0004FCA4 File Offset: 0x0004DEA4
		protected virtual void dll()
		{
			base.Awake();
			if (!PoliceOfficer.Officers.Contains(this))
			{
				PoliceOfficer.Officers.Add(this);
			}
			this.PursuitBehaviour.onEnd.AddListener(delegate()
			{
				this.TargetPlayerNOB = null;
			});
			base.InvokeRepeating("CheckNewInvestigation", 1f, 1f);
			this.chatterCountDown = UnityEngine.Random.Range(15f, 45f);
			VisionCone visionCone = this.awareness.VisionCone;
			visionCone.onVisionEventFull = (VisionCone.EventStateChange)Delegate.Combine(visionCone.onVisionEventFull, new VisionCone.EventStateChange(this.ProcessVisionEvent));
		}

		// Token: 0x04001170 RID: 4464
		public const float DEACTIVATION_TIME = 1f;

		// Token: 0x04001171 RID: 4465
		public const float INVESTIGATION_COOLDOWN = 60f;

		// Token: 0x04001172 RID: 4466
		public const float INVESTIGATION_MAX_DISTANCE = 8f;

		// Token: 0x04001173 RID: 4467
		public const float INVESTIGATION_MIN_VISIBILITY = 0.2f;

		// Token: 0x04001174 RID: 4468
		public const float INVESTIGATION_CHECK_INTERVAL = 1f;

		// Token: 0x04001175 RID: 4469
		public const float BODY_SEARCH_CHANCE_DEFAULT = 0.1f;

		// Token: 0x04001176 RID: 4470
		public const float MIN_CHATTER_INTERVAL = 15f;

		// Token: 0x04001177 RID: 4471
		public const float MAX_CHATTER_INTERVAL = 45f;

		// Token: 0x04001178 RID: 4472
		public static Action<VisionEventReceipt> OnPoliceVisionEvent;

		// Token: 0x04001179 RID: 4473
		public static List<PoliceOfficer> Officers = new List<PoliceOfficer>();

		// Token: 0x0400117B RID: 4475
		public LandVehicle AssignedVehicle;

		// Token: 0x0400117C RID: 4476
		[Header("References")]
		public PursuitBehaviour PursuitBehaviour;

		// Token: 0x0400117D RID: 4477
		public VehiclePursuitBehaviour VehiclePursuitBehaviour;

		// Token: 0x0400117E RID: 4478
		public BodySearchBehaviour BodySearchBehaviour;

		// Token: 0x0400117F RID: 4479
		public CheckpointBehaviour CheckpointBehaviour;

		// Token: 0x04001180 RID: 4480
		public FootPatrolBehaviour FootPatrolBehaviour;

		// Token: 0x04001181 RID: 4481
		public ProximityCircle ProxCircle;

		// Token: 0x04001182 RID: 4482
		public VehiclePatrolBehaviour VehiclePatrolBehaviour;

		// Token: 0x04001183 RID: 4483
		public SentryBehaviour SentryBehaviour;

		// Token: 0x04001184 RID: 4484
		public PoliceChatterVO ChatterVO;

		// Token: 0x04001185 RID: 4485
		[Header("Dialogue")]
		public DialogueContainer CheckpointDialogue;

		// Token: 0x04001186 RID: 4486
		[Header("Tools")]
		public AvatarEquippable BatonPrefab;

		// Token: 0x04001187 RID: 4487
		public AvatarEquippable TaserPrefab;

		// Token: 0x04001188 RID: 4488
		public AvatarEquippable GunPrefab;

		// Token: 0x04001189 RID: 4489
		[Header("Settings")]
		public bool AutoDeactivate = true;

		// Token: 0x0400118A RID: 4490
		public bool ChatterEnabled = true;

		// Token: 0x0400118B RID: 4491
		[Header("Behaviour Settings")]
		[Range(0f, 1f)]
		public float Suspicion = 0.5f;

		// Token: 0x0400118C RID: 4492
		[Range(0f, 1f)]
		public float Leniency = 0.5f;

		// Token: 0x0400118D RID: 4493
		[Header("Body Search Settings")]
		[Range(0f, 1f)]
		public float BodySearchChance = 0.1f;

		// Token: 0x0400118E RID: 4494
		[Range(1f, 10f)]
		public float BodySearchDuration = 5f;

		// Token: 0x0400118F RID: 4495
		[HideInInspector]
		public PoliceBelt belt;

		// Token: 0x04001190 RID: 4496
		private float timeSinceReadyToPool;

		// Token: 0x04001191 RID: 4497
		private float timeSinceOutOfSight;

		// Token: 0x04001192 RID: 4498
		private float chatterCountDown;

		// Token: 0x04001193 RID: 4499
		private Investigation currentBodySearchInvestigation;

		// Token: 0x04001194 RID: 4500
		public SyncVar<NetworkObject> syncVar___<TargetPlayerNOB>k__BackingField;

		// Token: 0x04001195 RID: 4501
		private bool dll_Excuted;

		// Token: 0x04001196 RID: 4502
		private bool dll_Excuted;
	}
}
