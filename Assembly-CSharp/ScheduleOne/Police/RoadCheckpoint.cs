using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Misc;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Vehicles;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Police
{
	// Token: 0x02000336 RID: 822
	public class RoadCheckpoint : NetworkBehaviour
	{
		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x0004FDB6 File Offset: 0x0004DFB6
		// (set) Token: 0x06001245 RID: 4677 RVA: 0x0004FDBE File Offset: 0x0004DFBE
		public RoadCheckpoint.ECheckpointState ActivationState { get; protected set; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x0004FDC7 File Offset: 0x0004DFC7
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x0004FDCF File Offset: 0x0004DFCF
		public bool Gate1Open
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<Gate1Open>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<Gate1Open>k__BackingField(value, true);
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x0004FDD9 File Offset: 0x0004DFD9
		// (set) Token: 0x06001249 RID: 4681 RVA: 0x0004FDE1 File Offset: 0x0004DFE1
		public bool Gate2Open
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<Gate2Open>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.sync___set_value_<Gate2Open>k__BackingField(value, true);
			}
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x0004FDEC File Offset: 0x0004DFEC
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Police.RoadCheckpoint_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0004FE0C File Offset: 0x0004E00C
		protected virtual void Update()
		{
			if (this.ActivationState != RoadCheckpoint.ECheckpointState.Disabled)
			{
				this.VehicleObstacle1.gameObject.SetActive(!this.Gate1Open);
				this.VehicleObstacle2.gameObject.SetActive(!this.Gate2Open);
				this.Stopper1.isActive = !this.Gate1Open;
				this.Stopper2.isActive = !this.Gate2Open;
			}
			if (this.ActivationState != this.appliedState)
			{
				this.ApplyState();
			}
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.OpenForNPCs)
			{
				if (this.NPCVehicleDetectionArea1.closestVehicle != null && this.NPCVehicleDetectionArea1.closestVehicle.OccupantNPCs[0] != null)
				{
					if (!this.Gate1Open)
					{
						this.SetGate1Open(true);
					}
					if (!this.Gate2Open)
					{
						this.SetGate2Open(true);
					}
				}
				if (this.NPCVehicleDetectionArea2.closestVehicle != null && this.NPCVehicleDetectionArea2.closestVehicle.OccupantNPCs[0] != null)
				{
					if (!this.Gate1Open)
					{
						this.SetGate1Open(true);
					}
					if (!this.Gate2Open)
					{
						this.SetGate2Open(true);
					}
				}
			}
			if (this.ActivationState != RoadCheckpoint.ECheckpointState.Disabled)
			{
				if (this.Gate1Open)
				{
					this.timeSinceGate1Open += Time.deltaTime;
					if (this.ImmediateVehicleDetector.vehicles.Count > 0)
					{
						this.vehicleDetectedSinceGate1Open = true;
					}
					if (this.timeSinceGate1Open > 15f || (this.vehicleDetectedSinceGate1Open && this.ImmediateVehicleDetector.vehicles.Count == 0))
					{
						this.SetGate1Open(false);
					}
				}
				else
				{
					this.timeSinceGate1Open = 0f;
					this.vehicleDetectedSinceGate1Open = false;
				}
				if (!this.Gate2Open)
				{
					this.timeSinceGate2Open = 0f;
					this.vehicleDetectedSinceGate2Open = false;
					return;
				}
				this.timeSinceGate2Open += Time.deltaTime;
				if (this.ImmediateVehicleDetector.vehicles.Count > 0)
				{
					this.vehicleDetectedSinceGate2Open = true;
				}
				if (this.timeSinceGate2Open > 15f || (this.vehicleDetectedSinceGate2Open && this.ImmediateVehicleDetector.vehicles.Count == 0))
				{
					this.SetGate2Open(false);
					return;
				}
			}
			else
			{
				this.timeSinceGate1Open = 0f;
				this.vehicleDetectedSinceGate1Open = false;
				this.timeSinceGate2Open = 0f;
				this.vehicleDetectedSinceGate2Open = false;
			}
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00050053 File Offset: 0x0004E253
		protected virtual void ApplyState()
		{
			this.appliedState = this.ActivationState;
			if (this.ActivationState == RoadCheckpoint.ECheckpointState.Disabled)
			{
				this.container.SetActive(false);
				return;
			}
			this.container.SetActive(true);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00050082 File Offset: 0x0004E282
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void Enable(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_Enable_328543758(conn);
				this.RpcLogic___Enable_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_Enable_328543758(conn);
			}
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x000500AC File Offset: 0x0004E2AC
		[ObserversRpc(RunLocally = true)]
		public void Disable()
		{
			this.RpcWriter___Observers_Disable_2166136261();
			this.RpcLogic___Disable_2166136261();
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x000500C5 File Offset: 0x0004E2C5
		public void SetGate1Open(bool o)
		{
			this.Gate1Open = o;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x000500CE File Offset: 0x0004E2CE
		public void SetGate2Open(bool o)
		{
			this.Gate2Open = o;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x000500D8 File Offset: 0x0004E2D8
		private void ResetTrafficCones()
		{
			if (this.trafficConeOriginalTransforms.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.TrafficCones.Length; i++)
			{
				this.TrafficCones[i].transform.position = this.trafficConeOriginalTransforms[this.TrafficCones[i]].Item1;
				this.TrafficCones[i].transform.rotation = this.trafficConeOriginalTransforms[this.TrafficCones[i]].Item2;
			}
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x0005015A File Offset: 0x0004E35A
		public void PlayerDetected(Player player)
		{
			if (this.onPlayerWalkThrough != null)
			{
				this.onPlayerWalkThrough.Invoke(player);
			}
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00050198 File Offset: 0x0004E398
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Police.RoadCheckpointAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Police.RoadCheckpointAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<Gate2Open>k__BackingField = new SyncVar<bool>(this, 1U, WritePermission.ServerOnly, ReadPermission.Observers, 0.25f, Channel.Reliable, this.<Gate2Open>k__BackingField);
			this.syncVar___<Gate1Open>k__BackingField = new SyncVar<bool>(this, 0U, WritePermission.ServerOnly, ReadPermission.Observers, 0.25f, Channel.Reliable, this.<Gate1Open>k__BackingField);
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_Enable_328543758));
			base.RegisterTargetRpc(1U, new ClientRpcDelegate(this.RpcReader___Target_Enable_328543758));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_Disable_2166136261));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Police.RoadCheckpoint));
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00050263 File Offset: 0x0004E463
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Police.RoadCheckpointAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Police.RoadCheckpointAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<Gate2Open>k__BackingField.SetRegistered();
			this.syncVar___<Gate1Open>k__BackingField.SetRegistered();
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0005028C File Offset: 0x0004E48C
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0005029C File Offset: 0x0004E49C
		private void RpcWriter___Observers_Enable_328543758(NetworkConnection conn)
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

		// Token: 0x06001258 RID: 4696 RVA: 0x00050345 File Offset: 0x0004E545
		public void RpcLogic___Enable_328543758(NetworkConnection conn)
		{
			this.ResetTrafficCones();
			this.ActivationState = RoadCheckpoint.ECheckpointState.Enabled;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00050354 File Offset: 0x0004E554
		private void RpcReader___Observers_Enable_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Enable_328543758(null);
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00050380 File Offset: 0x0004E580
		private void RpcWriter___Target_Enable_328543758(NetworkConnection conn)
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
			base.SendTargetRpc(1U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00050428 File Offset: 0x0004E628
		private void RpcReader___Target_Enable_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___Enable_328543758(base.LocalConnection);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00050450 File Offset: 0x0004E650
		private void RpcWriter___Observers_Disable_2166136261()
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
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x000504FC File Offset: 0x0004E6FC
		public void RpcLogic___Disable_2166136261()
		{
			this.ActivationState = RoadCheckpoint.ECheckpointState.Disabled;
			if (InstanceFinder.IsServer)
			{
				for (int i = 0; i < this.AssignedNPCs.Count; i++)
				{
					(this.AssignedNPCs[i] as PoliceOfficer).UnassignFromCheckpoint();
				}
			}
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00050544 File Offset: 0x0004E744
		private void RpcReader___Observers_Disable_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Disable_2166136261();
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x0005056E File Offset: 0x0004E76E
		// (set) Token: 0x06001260 RID: 4704 RVA: 0x00050576 File Offset: 0x0004E776
		public bool SyncAccessor_<Gate1Open>k__BackingField
		{
			get
			{
				return this.<Gate1Open>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<Gate1Open>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<Gate1Open>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000505B4 File Offset: 0x0004E7B4
		public virtual bool RoadCheckpoint(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<Gate2Open>k__BackingField(this.syncVar___<Gate2Open>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value = PooledReader0.ReadBoolean();
				this.sync___set_value_<Gate2Open>k__BackingField(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 0U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<Gate1Open>k__BackingField(this.syncVar___<Gate1Open>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value2 = PooledReader0.ReadBoolean();
				this.sync___set_value_<Gate1Open>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x0005064A File Offset: 0x0004E84A
		// (set) Token: 0x06001263 RID: 4707 RVA: 0x00050652 File Offset: 0x0004E852
		public bool SyncAccessor_<Gate2Open>k__BackingField
		{
			get
			{
				return this.<Gate2Open>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<Gate2Open>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<Gate2Open>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00050690 File Offset: 0x0004E890
		protected virtual void dll()
		{
			if (this.EnabledOnStart)
			{
				this.ActivationState = RoadCheckpoint.ECheckpointState.Enabled;
			}
			this.ApplyState();
			if (this.trafficConeOriginalTransforms.Count == 0)
			{
				for (int i = 0; i < this.TrafficCones.Length; i++)
				{
					this.trafficConeOriginalTransforms.Add(this.TrafficCones[i], new Tuple<Vector3, Quaternion>(this.TrafficCones[i].transform.position, this.TrafficCones[i].transform.rotation));
				}
			}
		}

		// Token: 0x0400119A RID: 4506
		public const float MAX_TIME_OPEN = 15f;

		// Token: 0x0400119C RID: 4508
		protected RoadCheckpoint.ECheckpointState appliedState;

		// Token: 0x0400119F RID: 4511
		public List<NPC> AssignedNPCs = new List<NPC>();

		// Token: 0x040011A0 RID: 4512
		[Header("Settings")]
		public EStealthLevel MaxStealthLevel;

		// Token: 0x040011A1 RID: 4513
		public bool OpenForNPCs = true;

		// Token: 0x040011A2 RID: 4514
		public bool EnabledOnStart;

		// Token: 0x040011A3 RID: 4515
		[Header("References")]
		[SerializeField]
		protected GameObject container;

		// Token: 0x040011A4 RID: 4516
		public CarStopper Stopper1;

		// Token: 0x040011A5 RID: 4517
		public CarStopper Stopper2;

		// Token: 0x040011A6 RID: 4518
		public VehicleDetector SearchArea1;

		// Token: 0x040011A7 RID: 4519
		public VehicleDetector SearchArea2;

		// Token: 0x040011A8 RID: 4520
		public VehicleObstacle VehicleObstacle1;

		// Token: 0x040011A9 RID: 4521
		public VehicleObstacle VehicleObstacle2;

		// Token: 0x040011AA RID: 4522
		public VehicleDetector NPCVehicleDetectionArea1;

		// Token: 0x040011AB RID: 4523
		public VehicleDetector NPCVehicleDetectionArea2;

		// Token: 0x040011AC RID: 4524
		public VehicleDetector ImmediateVehicleDetector;

		// Token: 0x040011AD RID: 4525
		public Rigidbody[] TrafficCones;

		// Token: 0x040011AE RID: 4526
		public Transform[] StandPoints;

		// Token: 0x040011AF RID: 4527
		protected Dictionary<Rigidbody, Tuple<Vector3, Quaternion>> trafficConeOriginalTransforms = new Dictionary<Rigidbody, Tuple<Vector3, Quaternion>>();

		// Token: 0x040011B0 RID: 4528
		private float timeSinceGate1Open;

		// Token: 0x040011B1 RID: 4529
		private bool vehicleDetectedSinceGate1Open;

		// Token: 0x040011B2 RID: 4530
		private float timeSinceGate2Open;

		// Token: 0x040011B3 RID: 4531
		private bool vehicleDetectedSinceGate2Open;

		// Token: 0x040011B4 RID: 4532
		public UnityEvent<Player> onPlayerWalkThrough;

		// Token: 0x040011B5 RID: 4533
		public SyncVar<bool> syncVar___<Gate1Open>k__BackingField;

		// Token: 0x040011B6 RID: 4534
		public SyncVar<bool> syncVar___<Gate2Open>k__BackingField;

		// Token: 0x040011B7 RID: 4535
		private bool dll_Excuted;

		// Token: 0x040011B8 RID: 4536
		private bool dll_Excuted;

		// Token: 0x02000337 RID: 823
		public enum ECheckpointState
		{
			// Token: 0x040011BA RID: 4538
			Disabled,
			// Token: 0x040011BB RID: 4539
			Enabled
		}
	}
}
