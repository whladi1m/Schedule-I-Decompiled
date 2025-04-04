using System;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Transporting;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007A7 RID: 1959
	public class Forklift : LandVehicle
	{
		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06003514 RID: 13588 RVA: 0x000DEFD0 File Offset: 0x000DD1D0
		// (set) Token: 0x06003515 RID: 13589 RVA: 0x000DEFD8 File Offset: 0x000DD1D8
		public float targetForkHeight
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<targetForkHeight>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			protected set
			{
				this.RpcWriter___Server_set_targetForkHeight_431000436(value);
				this.RpcLogic___set_targetForkHeight_431000436(value);
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06003516 RID: 13590 RVA: 0x000DEFEE File Offset: 0x000DD1EE
		// (set) Token: 0x06003517 RID: 13591 RVA: 0x000DEFF6 File Offset: 0x000DD1F6
		public float actualForkHeight
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<actualForkHeight>k__BackingField;
			}
			[CompilerGenerated]
			[ServerRpc(RunLocally = true)]
			protected set
			{
				this.RpcWriter___Server_set_actualForkHeight_431000436(value);
				this.RpcLogic___set_actualForkHeight_431000436(value);
			}
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x000DF00C File Offset: 0x000DD20C
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vehicles.Forklift_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x000DF02C File Offset: 0x000DD22C
		protected override void Update()
		{
			base.Update();
			if (base.localPlayerIsDriver)
			{
				this.targetForkHeight = this.lastFrameTargetForkHeight;
				int num = 0;
				if (Input.GetKey(KeyCode.UpArrow))
				{
					num++;
				}
				if (Input.GetKey(KeyCode.DownArrow))
				{
					num--;
				}
				this.targetForkHeight = Mathf.Clamp(this.targetForkHeight + (float)num * Time.deltaTime * this.liftMoveRate, 0f, 1f);
			}
			this.lastFrameTargetForkHeight = this.targetForkHeight;
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x000DF0AC File Offset: 0x000DD2AC
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost))
			{
				this.forkRb.isKinematic = false;
				this.joint.targetPosition = new Vector3(0f, Mathf.Lerp(this.lift_MinY, this.lift_MaxY, this.targetForkHeight), 0f);
				Vector3 vector = this.forkRb.transform.position - base.transform.TransformPoint(this.joint.connectedAnchor);
				vector = base.transform.InverseTransformVector(vector);
				this.actualForkHeight = 1f - Mathf.InverseLerp(this.lift_MinY, this.lift_MaxY, vector.y);
			}
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000DF178 File Offset: 0x000DD378
		protected new virtual void LateUpdate()
		{
			if (!base.localPlayerIsDriver && (!InstanceFinder.IsHost || base.CurrentPlayerOccupancy > 0))
			{
				this.forkRb.isKinematic = true;
				this.forkRb.transform.position = base.transform.TransformPoint(this.joint.connectedAnchor + new Vector3(0f, -Mathf.Lerp(this.lift_MinY, this.lift_MaxY, this.actualForkHeight), 0f));
				this.forkRb.transform.rotation = base.transform.rotation;
			}
			this.steeringWheel.localEulerAngles = new Vector3(0f, base.SyncAccessor_currentSteerAngle * this.steeringWheelAngleMultiplier, 0f);
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x000DF260 File Offset: 0x000DD460
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.ForkliftAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.ForkliftAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___<actualForkHeight>k__BackingField = new SyncVar<float>(this, 4U, WritePermission.ServerOnly, ReadPermission.Observers, 0.04f, Channel.Unreliable, this.<actualForkHeight>k__BackingField);
			this.syncVar___<targetForkHeight>k__BackingField = new SyncVar<float>(this, 3U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Unreliable, this.<targetForkHeight>k__BackingField);
			base.RegisterServerRpc(17U, new ServerRpcDelegate(this.RpcReader___Server_set_targetForkHeight_431000436));
			base.RegisterServerRpc(18U, new ServerRpcDelegate(this.RpcReader___Server_set_actualForkHeight_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Vehicles.Forklift));
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x000DF31A File Offset: 0x000DD51A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.ForkliftAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.ForkliftAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___<actualForkHeight>k__BackingField.SetRegistered();
			this.syncVar___<targetForkHeight>k__BackingField.SetRegistered();
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x000DF349 File Offset: 0x000DD549
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x000DF358 File Offset: 0x000DD558
		private void RpcWriter___Server_set_targetForkHeight_431000436(float value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendServerRpc(17U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x000DF45E File Offset: 0x000DD65E
		protected void RpcLogic___set_targetForkHeight_431000436(float value)
		{
			this.sync___set_value_<targetForkHeight>k__BackingField(value, true);
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x000DF468 File Offset: 0x000DD668
		private void RpcReader___Server_set_targetForkHeight_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_targetForkHeight_431000436(value);
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x000DF4BC File Offset: 0x000DD6BC
		private void RpcWriter___Server_set_actualForkHeight_431000436(float value)
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
			if (!base.IsOwner)
			{
				NetworkManager networkManager2 = base.NetworkManager;
				if (networkManager2 == null)
				{
					networkManager2 = InstanceFinder.NetworkManager;
				}
				if (networkManager2 != null)
				{
					networkManager2.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because you are not the owner of this object. .");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendServerRpc(18U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x000DF5C2 File Offset: 0x000DD7C2
		protected void RpcLogic___set_actualForkHeight_431000436(float value)
		{
			this.sync___set_value_<actualForkHeight>k__BackingField(value, true);
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x000DF5CC File Offset: 0x000DD7CC
		private void RpcReader___Server_set_actualForkHeight_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (!base.OwnerMatches(conn))
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___set_actualForkHeight_431000436(value);
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06003526 RID: 13606 RVA: 0x000DF620 File Offset: 0x000DD820
		// (set) Token: 0x06003527 RID: 13607 RVA: 0x000DF628 File Offset: 0x000DD828
		public float SyncAccessor_<targetForkHeight>k__BackingField
		{
			get
			{
				return this.<targetForkHeight>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<targetForkHeight>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<targetForkHeight>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x000DF664 File Offset: 0x000DD864
		public virtual bool Forklift(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 4U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<actualForkHeight>k__BackingField(this.syncVar___<actualForkHeight>k__BackingField.GetValue(true), true);
					return true;
				}
				float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_<actualForkHeight>k__BackingField(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 3U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<targetForkHeight>k__BackingField(this.syncVar___<targetForkHeight>k__BackingField.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_<targetForkHeight>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06003529 RID: 13609 RVA: 0x000DF704 File Offset: 0x000DD904
		// (set) Token: 0x0600352A RID: 13610 RVA: 0x000DF70C File Offset: 0x000DD90C
		public float SyncAccessor_<actualForkHeight>k__BackingField
		{
			get
			{
				return this.<actualForkHeight>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<actualForkHeight>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<actualForkHeight>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x000DF748 File Offset: 0x000DD948
		protected virtual void dll()
		{
			base.Awake();
			Vector3 position = this.forkRb.transform.position;
			Quaternion rotation = this.forkRb.transform.rotation;
			this.forkRb.transform.SetParent(null);
			this.forkRb.transform.position = position;
			this.forkRb.transform.rotation = rotation;
		}

		// Token: 0x0400264D RID: 9805
		[Header("Forklift References")]
		[SerializeField]
		protected Transform steeringWheel;

		// Token: 0x0400264E RID: 9806
		[SerializeField]
		protected Rigidbody forkRb;

		// Token: 0x0400264F RID: 9807
		[SerializeField]
		protected ConfigurableJoint joint;

		// Token: 0x04002650 RID: 9808
		[Header("Forklift settings")]
		[SerializeField]
		protected float steeringWheelAngleMultiplier = 2f;

		// Token: 0x04002651 RID: 9809
		[SerializeField]
		protected float lift_MinY;

		// Token: 0x04002652 RID: 9810
		[SerializeField]
		protected float lift_MaxY;

		// Token: 0x04002653 RID: 9811
		[SerializeField]
		protected float liftMoveRate = 0.5f;

		// Token: 0x04002655 RID: 9813
		private float lastFrameTargetForkHeight;

		// Token: 0x04002657 RID: 9815
		public SyncVar<float> syncVar___<targetForkHeight>k__BackingField;

		// Token: 0x04002658 RID: 9816
		public SyncVar<float> syncVar___<actualForkHeight>k__BackingField;

		// Token: 0x04002659 RID: 9817
		private bool dll_Excuted;

		// Token: 0x0400265A RID: 9818
		private bool dll_Excuted;
	}
}
