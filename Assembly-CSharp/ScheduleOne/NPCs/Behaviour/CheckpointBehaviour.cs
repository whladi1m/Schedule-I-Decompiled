using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.ItemFramework;
using ScheduleOne.Law;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Police;
using ScheduleOne.Product;
using ScheduleOne.Storage;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x020004DC RID: 1244
	public class CheckpointBehaviour : Behaviour
	{
		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001C3B RID: 7227 RVA: 0x000751AE File Offset: 0x000733AE
		// (set) Token: 0x06001C3C RID: 7228 RVA: 0x000751B6 File Offset: 0x000733B6
		public CheckpointManager.ECheckpointLocation AssignedCheckpoint { get; protected set; }

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x000751BF File Offset: 0x000733BF
		// (set) Token: 0x06001C3E RID: 7230 RVA: 0x000751C7 File Offset: 0x000733C7
		public RoadCheckpoint Checkpoint { get; protected set; }

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x000751D0 File Offset: 0x000733D0
		// (set) Token: 0x06001C40 RID: 7232 RVA: 0x000751D8 File Offset: 0x000733D8
		public bool IsSearching { get; protected set; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001C41 RID: 7233 RVA: 0x000751E1 File Offset: 0x000733E1
		// (set) Token: 0x06001C42 RID: 7234 RVA: 0x000751E9 File Offset: 0x000733E9
		public LandVehicle CurrentSearchedVehicle { get; protected set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x000751F2 File Offset: 0x000733F2
		// (set) Token: 0x06001C44 RID: 7236 RVA: 0x000751FA File Offset: 0x000733FA
		public Player Initiator { get; protected set; }

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x00075203 File Offset: 0x00073403
		private Transform standPoint
		{
			get
			{
				return this.Checkpoint.StandPoints[Mathf.Clamp(this.Checkpoint.AssignedNPCs.IndexOf(base.Npc), 0, this.Checkpoint.StandPoints.Length - 1)];
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x0007523C File Offset: 0x0007343C
		private DialogueDatabase dialogueDatabase
		{
			get
			{
				return base.Npc.dialogueHandler.Database;
			}
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x00075250 File Offset: 0x00073450
		protected override void Begin()
		{
			base.Begin();
			this.Checkpoint = NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.AssignedCheckpoint);
			if (!this.Checkpoint.AssignedNPCs.Contains(base.Npc))
			{
				this.Checkpoint.AssignedNPCs.Add(base.Npc);
			}
			this.Checkpoint.onPlayerWalkThrough.AddListener(new UnityAction<Player>(this.PlayerWalkedThroughCheckPoint));
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000752C4 File Offset: 0x000734C4
		protected override void Resume()
		{
			base.Resume();
			this.Checkpoint = NetworkSingleton<CheckpointManager>.Instance.GetCheckpoint(this.AssignedCheckpoint);
			if (!this.Checkpoint.AssignedNPCs.Contains(base.Npc))
			{
				this.Checkpoint.AssignedNPCs.Add(base.Npc);
			}
			this.Checkpoint.onPlayerWalkThrough.AddListener(new UnityAction<Player>(this.PlayerWalkedThroughCheckPoint));
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x00075338 File Offset: 0x00073538
		protected override void End()
		{
			base.End();
			this.IsSearching = false;
			if (this.Checkpoint.AssignedNPCs.Contains(base.Npc))
			{
				this.Checkpoint.AssignedNPCs.Remove(base.Npc);
			}
			if (this.CurrentSearchedVehicle != null && this.trunkOpened)
			{
				this.CurrentSearchedVehicle.Trunk.SetIsOpen(false);
			}
			this.Checkpoint.onPlayerWalkThrough.RemoveListener(new UnityAction<Player>(this.PlayerWalkedThroughCheckPoint));
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x000753C4 File Offset: 0x000735C4
		protected override void Pause()
		{
			base.Pause();
			this.IsSearching = false;
			if (this.Checkpoint.AssignedNPCs.Contains(base.Npc))
			{
				this.Checkpoint.AssignedNPCs.Remove(base.Npc);
			}
			if (this.CurrentSearchedVehicle != null && this.trunkOpened)
			{
				this.CurrentSearchedVehicle.Trunk.SetIsOpen(false);
			}
			this.Checkpoint.onPlayerWalkThrough.RemoveListener(new UnityAction<Player>(this.PlayerWalkedThroughCheckPoint));
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x00075450 File Offset: 0x00073650
		public override void ActiveMinPass()
		{
			base.ActiveMinPass();
			if (this.IsSearching && !base.Npc.Movement.IsMoving && base.Npc.Movement.IsAsCloseAsPossible(this.GetSearchPoint(), 0.5f))
			{
				if (!this.CurrentSearchedVehicle.Trunk.IsOpen)
				{
					StorageDoorAnimation trunk = this.CurrentSearchedVehicle.Trunk;
					if (trunk != null)
					{
						trunk.SetIsOpen(true);
					}
					this.trunkOpened = true;
				}
			}
			else if (this.trunkOpened && this.CurrentSearchedVehicle != null)
			{
				StorageDoorAnimation trunk2 = this.CurrentSearchedVehicle.Trunk;
				if (trunk2 != null)
				{
					trunk2.SetIsOpen(false);
				}
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.Checkpoint == null || this.Checkpoint.ActivationState == RoadCheckpoint.ECheckpointState.Disabled)
			{
				base.Disable_Networked(null);
				return;
			}
			if (!this.IsSearching)
			{
				if (!base.Npc.Movement.IsMoving)
				{
					if (base.Npc.Movement.IsAsCloseAsPossible(this.standPoint.position, 0.5f))
					{
						if (!base.Npc.Movement.FaceDirectionInProgress)
						{
							base.Npc.Movement.FaceDirection(this.standPoint.forward, 0.5f);
							return;
						}
					}
					else if (base.Npc.Movement.CanMove())
					{
						base.Npc.Movement.SetDestination(this.standPoint.position);
						return;
					}
				}
			}
			else
			{
				if (!this.Checkpoint.SearchArea1.vehicles.Contains(this.CurrentSearchedVehicle) && !this.Checkpoint.SearchArea2.vehicles.Contains(this.CurrentSearchedVehicle))
				{
					this.StopSearch();
					return;
				}
				if (!base.Npc.Movement.IsMoving)
				{
					if (base.Npc.Movement.IsAsCloseAsPossible(this.GetSearchPoint(), 1f))
					{
						if (!base.Npc.Movement.FaceDirectionInProgress)
						{
							base.Npc.Movement.FacePoint(this.CurrentSearchedVehicle.transform.position, 0.5f);
						}
						this.currentLookTime += 1f;
						if (this.currentLookTime >= 1.5f)
						{
							this.ConcludeSearch();
							return;
						}
					}
					else
					{
						this.currentLookTime = 0f;
						if (base.Npc.Movement.CanMove())
						{
							base.Npc.Movement.SetDestination(this.GetSearchPoint());
						}
					}
				}
			}
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000756D0 File Offset: 0x000738D0
		[ObserversRpc(RunLocally = true)]
		public void SetCheckpoint(CheckpointManager.ECheckpointLocation loc)
		{
			this.RpcWriter___Observers_SetCheckpoint_4087078542(loc);
			this.RpcLogic___SetCheckpoint_4087078542(loc);
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000756E6 File Offset: 0x000738E6
		[ObserversRpc(RunLocally = true)]
		public void SetInitiator(NetworkObject init)
		{
			this.RpcWriter___Observers_SetInitiator_3323014238(init);
			this.RpcLogic___SetInitiator_3323014238(init);
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000756FC File Offset: 0x000738FC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void StartSearch(NetworkObject targetVehicle, NetworkObject initiator)
		{
			this.RpcWriter___Server_StartSearch_3694055493(targetVehicle, initiator);
			this.RpcLogic___StartSearch_3694055493(targetVehicle, initiator);
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00075728 File Offset: 0x00073928
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void StopSearch()
		{
			this.RpcWriter___Server_StopSearch_2166136261();
			this.RpcLogic___StopSearch_2166136261();
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00075741 File Offset: 0x00073941
		[ObserversRpc(RunLocally = true)]
		public void SetIsSearching(bool s)
		{
			this.RpcWriter___Observers_SetIsSearching_1140765316(s);
			this.RpcLogic___SetIsSearching_1140765316(s);
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x00075758 File Offset: 0x00073958
		private Vector3 GetSearchPoint()
		{
			return this.CurrentSearchedVehicle.transform.position - this.CurrentSearchedVehicle.transform.forward * (this.CurrentSearchedVehicle.boundingBoxDimensions.z / 2f + 0.75f);
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x000757AC File Offset: 0x000739AC
		[ObserversRpc(RunLocally = true)]
		private void ConcludeSearch()
		{
			this.RpcWriter___Observers_ConcludeSearch_2166136261();
			this.RpcLogic___ConcludeSearch_2166136261();
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x000757C8 File Offset: 0x000739C8
		private bool DoesVehicleContainIllicitItems()
		{
			if (this.CurrentSearchedVehicle == null)
			{
				return false;
			}
			(from x in this.CurrentSearchedVehicle.Storage.ItemSlots
			select x.ItemInstance).ToList<ItemInstance>();
			foreach (ItemSlot itemSlot in this.CurrentSearchedVehicle.Storage.ItemSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
						if (productItemInstance.AppliedPackaging == null || productItemInstance.AppliedPackaging.StealthLevel <= this.Checkpoint.MaxStealthLevel)
						{
							return true;
						}
					}
					else if (itemSlot.ItemInstance.Definition.legalStatus != ELegalStatus.Legal)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000758CC File Offset: 0x00073ACC
		private void PlayerWalkedThroughCheckPoint(Player player)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (player.CrimeData.TimeSinceLastBodySearch < 60f)
			{
				return;
			}
			if (player.CrimeData.CurrentPursuitLevel != PlayerCrimeData.EPursuitLevel.None)
			{
				return;
			}
			if (NetworkSingleton<CurfewManager>.Instance.IsCurrentlyActive)
			{
				return;
			}
			if (this.Checkpoint.AssignedNPCs.Count == 0)
			{
				return;
			}
			List<NPC> list = new List<NPC>();
			for (int i = 0; i < this.Checkpoint.AssignedNPCs.Count; i++)
			{
				Transform transform = this.Checkpoint.StandPoints[Mathf.Clamp(i, 0, this.Checkpoint.StandPoints.Length - 1)];
				if (Vector3.Distance(this.Checkpoint.AssignedNPCs[i].transform.position, transform.position) < 6f)
				{
					list.Add(this.Checkpoint.AssignedNPCs[i]);
				}
			}
			NPC x = null;
			float num = float.MaxValue;
			for (int j = 0; j < list.Count; j++)
			{
				float num2 = Vector3.Distance(player.transform.position, list[j].transform.position);
				if (num2 < num)
				{
					num = num2;
					x = list[j];
				}
			}
			if (num > 6f)
			{
				return;
			}
			if (x != base.Npc)
			{
				return;
			}
			player.CrimeData.ResetBodysearchCooldown();
			(base.Npc as PoliceOfficer).ConductBodySearch(player);
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x00075A34 File Offset: 0x00073C34
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CheckpointBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.Behaviour.CheckpointBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(15U, new ClientRpcDelegate(this.RpcReader___Observers_SetCheckpoint_4087078542));
			base.RegisterObserversRpc(16U, new ClientRpcDelegate(this.RpcReader___Observers_SetInitiator_3323014238));
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_StartSearch_3694055493));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_StopSearch_2166136261));
			base.RegisterObserversRpc(19U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsSearching_1140765316));
			base.RegisterObserversRpc(20U, new ClientRpcDelegate(this.RpcReader___Observers_ConcludeSearch_2166136261));
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00075AE2 File Offset: 0x00073CE2
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CheckpointBehaviourAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.Behaviour.CheckpointBehaviourAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00075AFB File Offset: 0x00073CFB
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x00075B0C File Offset: 0x00073D0C
		private void RpcWriter___Observers_SetCheckpoint_4087078542(CheckpointManager.ECheckpointLocation loc)
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
			writer.Write___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generated(loc);
			base.SendObserversRpc(15U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00075BC2 File Offset: 0x00073DC2
		public void RpcLogic___SetCheckpoint_4087078542(CheckpointManager.ECheckpointLocation loc)
		{
			this.AssignedCheckpoint = loc;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00075BCC File Offset: 0x00073DCC
		private void RpcReader___Observers_SetCheckpoint_4087078542(PooledReader PooledReader0, Channel channel)
		{
			CheckpointManager.ECheckpointLocation loc = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Law.CheckpointManager/ECheckpointLocationFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCheckpoint_4087078542(loc);
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00075C08 File Offset: 0x00073E08
		private void RpcWriter___Observers_SetInitiator_3323014238(NetworkObject init)
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
			writer.WriteNetworkObject(init);
			base.SendObserversRpc(16U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x00075CBE File Offset: 0x00073EBE
		public void RpcLogic___SetInitiator_3323014238(NetworkObject init)
		{
			this.Initiator = init.GetComponent<Player>();
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x00075CCC File Offset: 0x00073ECC
		private void RpcReader___Observers_SetInitiator_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject init = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetInitiator_3323014238(init);
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x00075D08 File Offset: 0x00073F08
		private void RpcWriter___Server_StartSearch_3694055493(NetworkObject targetVehicle, NetworkObject initiator)
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
			writer.WriteNetworkObject(targetVehicle);
			writer.WriteNetworkObject(initiator);
			base.SendServerRpc(17U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x00075DBC File Offset: 0x00073FBC
		public void RpcLogic___StartSearch_3694055493(NetworkObject targetVehicle, NetworkObject initiator)
		{
			this.currentLookTime = 0f;
			this.SetIsSearching(true);
			this.SetInitiator(initiator);
			this.CurrentSearchedVehicle = targetVehicle.GetComponent<LandVehicle>();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			base.Npc.Movement.SpeedController.AddSpeedControl(new NPCSpeedController.SpeedControl("searchingvehicle", 20, 0.15f));
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x00075E1C File Offset: 0x0007401C
		private void RpcReader___Server_StartSearch_3694055493(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject targetVehicle = PooledReader0.ReadNetworkObject();
			NetworkObject initiator = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___StartSearch_3694055493(targetVehicle, initiator);
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x00075E6C File Offset: 0x0007406C
		private void RpcWriter___Server_StopSearch_2166136261()
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
			base.SendServerRpc(18U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x00075F08 File Offset: 0x00074108
		public void RpcLogic___StopSearch_2166136261()
		{
			this.SetIsSearching(false);
			if (this.CurrentSearchedVehicle != null && this.trunkOpened)
			{
				StorageDoorAnimation trunk = this.CurrentSearchedVehicle.Trunk;
				if (trunk != null)
				{
					trunk.SetIsOpen(false);
				}
			}
			this.CurrentSearchedVehicle = null;
			this.Initiator = null;
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			base.Npc.Movement.SpeedController.RemoveSpeedControl("searchingvehicle");
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x00075F7C File Offset: 0x0007417C
		private void RpcReader___Server_StopSearch_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___StopSearch_2166136261();
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00075FAC File Offset: 0x000741AC
		private void RpcWriter___Observers_SetIsSearching_1140765316(bool s)
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
			writer.WriteBoolean(s);
			base.SendObserversRpc(19U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00076062 File Offset: 0x00074262
		public void RpcLogic___SetIsSearching_1140765316(bool s)
		{
			this.IsSearching = s;
			if (this.IsSearching)
			{
				base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "checkpoint_search_start"), 3f);
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x0007609C File Offset: 0x0007429C
		private void RpcReader___Observers_SetIsSearching_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool s = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsSearching_1140765316(s);
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000760D8 File Offset: 0x000742D8
		private void RpcWriter___Observers_ConcludeSearch_2166136261()
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
			base.SendObserversRpc(20U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x00076184 File Offset: 0x00074384
		private void RpcLogic___ConcludeSearch_2166136261()
		{
			if (this.CurrentSearchedVehicle == null)
			{
				Console.LogWarning("ConcludeSearch called with null vehicle", null);
			}
			if (this.CurrentSearchedVehicle != null && this.DoesVehicleContainIllicitItems() && this.Initiator != null)
			{
				base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "checkpoint_items_found"), 3f);
				if (this.Initiator == Player.Local)
				{
					Player.Local.CrimeData.AddCrime(new TransportingIllicitItems(), 1);
					Player.Local.CrimeData.SetPursuitLevel(PlayerCrimeData.EPursuitLevel.Arresting);
					(base.Npc as PoliceOfficer).BeginFootPursuit_Networked(Player.Local.NetworkObject, true);
				}
			}
			else
			{
				base.Npc.dialogueHandler.ShowWorldspaceDialogue(this.dialogueDatabase.GetLine(EDialogueModule.Police, "checkpoint_all_clear"), 3f);
				if (this.Checkpoint.SearchArea1.vehicles.Contains(this.CurrentSearchedVehicle))
				{
					this.Checkpoint.SetGate1Open(true);
				}
				else if (this.Checkpoint.SearchArea1.vehicles.Contains(this.CurrentSearchedVehicle))
				{
					this.Checkpoint.SetGate2Open(true);
				}
				else
				{
					this.Checkpoint.SetGate1Open(true);
					this.Checkpoint.SetGate2Open(true);
				}
			}
			this.StopSearch();
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000762F4 File Offset: 0x000744F4
		private void RpcReader___Observers_ConcludeSearch_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ConcludeSearch_2166136261();
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x0007631E File Offset: 0x0007451E
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04001747 RID: 5959
		public const float LOOK_TIME = 1.5f;

		// Token: 0x0400174D RID: 5965
		private float currentLookTime;

		// Token: 0x0400174E RID: 5966
		private bool trunkOpened;

		// Token: 0x0400174F RID: 5967
		private bool dll_Excuted;

		// Token: 0x04001750 RID: 5968
		private bool dll_Excuted;
	}
}
