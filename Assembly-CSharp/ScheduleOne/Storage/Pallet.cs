using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.ItemFramework;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tiles;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x02000888 RID: 2184
	public class Pallet : NetworkBehaviour, IStorageEntity
	{
		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06003B0F RID: 15119 RVA: 0x000F8536 File Offset: 0x000F6736
		public bool isEmpty
		{
			get
			{
				return this._storedItemContainer.childCount == 0;
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06003B10 RID: 15120 RVA: 0x000F8546 File Offset: 0x000F6746
		protected bool carriedByForklift
		{
			get
			{
				return this.forkliftsInContact.Count > 0;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x000F8556 File Offset: 0x000F6756
		public Transform storedItemContainer
		{
			get
			{
				return this._storedItemContainer;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06003B12 RID: 15122 RVA: 0x000F855E File Offset: 0x000F675E
		public Dictionary<StoredItem, Employee> reservedItems
		{
			get
			{
				return this._reservedItems;
			}
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x000F8566 File Offset: 0x000F6766
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Storage.Pallet_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x000F857A File Offset: 0x000F677A
		public override void OnStartServer()
		{
			base.OnStartServer();
			if (this.currentSlot == null)
			{
				this.rb.isKinematic = false;
				this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			}
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x000F85A8 File Offset: 0x000F67A8
		[ServerRpc(RequireOwnership = false)]
		protected virtual void SetOwner(NetworkConnection conn)
		{
			this.RpcWriter___Server_SetOwner_328543758(conn);
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x000F85C0 File Offset: 0x000F67C0
		public override void OnOwnershipClient(NetworkConnection prevOwner)
		{
			base.OnOwnershipClient(prevOwner);
			if (base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost))
			{
				if (this.rb != null)
				{
					this.rb.interpolation = RigidbodyInterpolation.Interpolate;
					this.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
					this.rb.isKinematic = false;
				}
				if (!Pallet.palletsOwnedByLocalPlayer.Contains(this))
				{
					Pallet.palletsOwnedByLocalPlayer.Add(this);
					return;
				}
			}
			else
			{
				if (this.rb != null)
				{
					this.rb.interpolation = RigidbodyInterpolation.None;
					this.rb.isKinematic = true;
				}
				if (Pallet.palletsOwnedByLocalPlayer.Contains(this))
				{
					Pallet.palletsOwnedByLocalPlayer.Remove(this);
				}
			}
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x000F8676 File Offset: 0x000F6876
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			this.SendItemsToClient(connection);
			if (this.currentSlot != null)
			{
				this.BindToSlot(connection, this.currentSlot.GUID);
			}
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x000F86A8 File Offset: 0x000F68A8
		private void SendItemsToClient(NetworkConnection connection)
		{
			StoredItem[] componentsInChildren = this._storedItemContainer.GetComponentsInChildren<StoredItem>();
			List<StorageGrid> storageGrids = this.GetStorageGrids();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				this.CreateStoredItem(connection, componentsInChildren[i].item, storageGrids.IndexOf(componentsInChildren[i].parentGrid), componentsInChildren[i].CoordinatePairs[0].coord2, componentsInChildren[i].Rotation, "", false);
			}
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x000F871C File Offset: 0x000F691C
		public virtual void DestroyPallet()
		{
			base.Despawn(null);
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x000F8738 File Offset: 0x000F6938
		protected virtual void Update()
		{
			this.timeSinceSlotCheck += Time.deltaTime;
			if (this.currentSlot == null)
			{
				this.timeBoundToSlot = 0f;
				return;
			}
			this.timeBoundToSlot += Time.deltaTime;
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x000F8778 File Offset: 0x000F6978
		protected virtual void FixedUpdate()
		{
			if (base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost))
			{
				if (this.carriedByForklift)
				{
					if (this.currentSlot != null && this.timeBoundToSlot > 1f)
					{
						Console.Log("Exiting", null);
						this.ExitSlot_Server();
					}
				}
				else if (this.currentSlot == null && this.timeSinceSlotCheck >= 0.5f)
				{
					this.timeSinceSlotCheck = 0f;
					Collider[] array = Physics.OverlapSphere(base.transform.position, 0.3f, 1 << LayerMask.NameToLayer("Pallet"), QueryTriggerInteraction.Collide);
					for (int i = 0; i < array.Length; i++)
					{
						PalletSlot componentInParent = array[i].gameObject.GetComponentInParent<PalletSlot>();
						if (componentInParent != null && componentInParent.occupant == null)
						{
							this.BindToSlot_Server(componentInParent.GUID);
							break;
						}
					}
				}
				if (base.transform.position.y < -20f && this.currentSlot == null)
				{
					if (this.rb != null)
					{
						this.rb.velocity = Vector3.zero;
						this.rb.angularVelocity = Vector3.zero;
					}
					float num = 0f;
					if (MapHeightSampler.Sample(base.transform.position.x, out num, base.transform.position.z))
					{
						this.SetPosition(new Vector3(base.transform.position.x, num + 3f, base.transform.position.z));
					}
					else
					{
						this.SetPosition(MapHeightSampler.ResetPosition);
					}
				}
			}
			this.UpdateOwnership();
			this.forkliftsInContact.Clear();
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x000F8947 File Offset: 0x000F6B47
		private void SetPosition(Vector3 position)
		{
			base.transform.position = position;
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x000F8958 File Offset: 0x000F6B58
		private void UpdateOwnership()
		{
			if (this.forkliftsInContact.Count == 0)
			{
				if (base.IsOwner && !InstanceFinder.IsHost)
				{
					base.NetworkObject.SetLocalOwnership(null);
					this.SetOwner(null);
					return;
				}
			}
			else
			{
				NetworkConnection owner = this.forkliftsInContact[0].Owner;
				if (base.Owner != owner && owner == Player.Local.Connection)
				{
					base.NetworkObject.SetLocalOwnership(Player.Local.Connection);
					this.SetOwner(Player.Local.Connection);
				}
			}
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x000F89EC File Offset: 0x000F6BEC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void BindToSlot_Server(Guid slotGuid)
		{
			this.RpcWriter___Server_BindToSlot_Server_1272046255(slotGuid);
			this.RpcLogic___BindToSlot_Server_1272046255(slotGuid);
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x000F8A04 File Offset: 0x000F6C04
		[ObserversRpc]
		[TargetRpc]
		private void BindToSlot(NetworkConnection conn, Guid slotGuid)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_BindToSlot_454078614(conn, slotGuid);
			}
			else
			{
				this.RpcWriter___Target_BindToSlot_454078614(conn, slotGuid);
			}
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x000F8A37 File Offset: 0x000F6C37
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void ExitSlot_Server()
		{
			this.RpcWriter___Server_ExitSlot_Server_2166136261();
			this.RpcLogic___ExitSlot_Server_2166136261();
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x000F8A48 File Offset: 0x000F6C48
		[ObserversRpc]
		private void ExitSlot()
		{
			this.RpcWriter___Observers_ExitSlot_2166136261();
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x000F8A5C File Offset: 0x000F6C5C
		public void TriggerStay(Collider other)
		{
			Forklift forklift = other.gameObject.GetComponentInParent<Forklift>();
			if (forklift == null)
			{
				ForkliftFork componentInParent = other.gameObject.GetComponentInParent<ForkliftFork>();
				if (componentInParent != null)
				{
					forklift = componentInParent.forklift;
				}
			}
			if (other.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
			{
				return;
			}
			if (forklift != null && !this.forkliftsInContact.Contains(forklift))
			{
				this.forkliftsInContact.Add(forklift);
			}
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x000F8AD5 File Offset: 0x000F6CD5
		public List<StoredItem> GetStoredItems()
		{
			return new List<StoredItem>(this.storedItemContainer.GetComponentsInChildren<StoredItem>());
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x000F8AE7 File Offset: 0x000F6CE7
		public List<StorageGrid> GetStorageGrids()
		{
			return new List<StorageGrid>
			{
				this.storageGrid
			};
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x000F8AFC File Offset: 0x000F6CFC
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public void CreateStoredItem(NetworkConnection conn, StorableItemInstance item, int gridIndex, Vector2 originCoord, float rotation, string jobID = "", bool network = true)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateStoredItem_913707843(conn, item, gridIndex, originCoord, rotation, jobID, network);
				this.RpcLogic___CreateStoredItem_913707843(conn, item, gridIndex, originCoord, rotation, jobID, network);
			}
			else
			{
				this.RpcWriter___Target_CreateStoredItem_913707843(conn, item, gridIndex, originCoord, rotation, jobID, network);
			}
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x000F8B79 File Offset: 0x000F6D79
		[ServerRpc(RequireOwnership = false)]
		private void CreateStoredItem_Server(StorableItemInstance data, int gridIndex, Vector2 originCoord, float rotation, string jobID)
		{
			this.RpcWriter___Server_CreateStoredItem_Server_1890711751(data, gridIndex, originCoord, rotation, jobID);
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x000F8B98 File Offset: 0x000F6D98
		[ObserversRpc(RunLocally = true)]
		public void DestroyStoredItem(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
		{
			this.RpcWriter___Observers_DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
			this.RpcLogic___DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x000F8BD1 File Offset: 0x000F6DD1
		[ServerRpc(RequireOwnership = false)]
		private void DestroyStoredItem_Server(int gridIndex, Coordinate coord, string jobID)
		{
			this.RpcWriter___Server_DestroyStoredItem_Server_3952619116(gridIndex, coord, jobID);
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x000F8C28 File Offset: 0x000F6E28
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.PalletAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.PalletAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetOwner_328543758));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_BindToSlot_Server_1272046255));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_BindToSlot_454078614));
			base.RegisterTargetRpc(3U, new ClientRpcDelegate(this.RpcReader___Target_BindToSlot_454078614));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_ExitSlot_Server_2166136261));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_ExitSlot_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_CreateStoredItem_913707843));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_CreateStoredItem_913707843));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_CreateStoredItem_Server_1890711751));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyStoredItem_3261517793));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_DestroyStoredItem_Server_3952619116));
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x000F8D43 File Offset: 0x000F6F43
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.PalletAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.PalletAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x000F8D56 File Offset: 0x000F6F56
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x000F8D64 File Offset: 0x000F6F64
		private void RpcWriter___Server_SetOwner_328543758(NetworkConnection conn)
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
			writer.WriteNetworkConnection(conn);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x000F8E0C File Offset: 0x000F700C
		protected virtual void RpcLogic___SetOwner_328543758(NetworkConnection conn)
		{
			Console.Log("Setting pallet owner to: " + conn.ClientId.ToString(), null);
			if (base.Owner != null && Player.GetPlayer(base.Owner) != null)
			{
				Player.GetPlayer(base.Owner).objectsTemporarilyOwnedByPlayer.Remove(base.NetworkObject);
			}
			if (conn != null && Player.GetPlayer(conn) != null)
			{
				Player.GetPlayer(conn).objectsTemporarilyOwnedByPlayer.Add(base.NetworkObject);
			}
			base.NetworkObject.GiveOwnership(conn);
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x000F8EAC File Offset: 0x000F70AC
		private void RpcReader___Server_SetOwner_328543758(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetOwner_328543758(conn2);
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x000F8EE0 File Offset: 0x000F70E0
		private void RpcWriter___Server_BindToSlot_Server_1272046255(Guid slotGuid)
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
			writer.WriteGuidAllocated(slotGuid);
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000F8F87 File Offset: 0x000F7187
		public void RpcLogic___BindToSlot_Server_1272046255(Guid slotGuid)
		{
			this.BindToSlot(null, slotGuid);
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x000F8F94 File Offset: 0x000F7194
		private void RpcReader___Server_BindToSlot_Server_1272046255(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Guid slotGuid = PooledReader0.ReadGuid();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___BindToSlot_Server_1272046255(slotGuid);
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x000F8FD4 File Offset: 0x000F71D4
		private void RpcWriter___Observers_BindToSlot_454078614(NetworkConnection conn, Guid slotGuid)
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
			writer.WriteGuidAllocated(slotGuid);
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x000F908C File Offset: 0x000F728C
		private void RpcLogic___BindToSlot_454078614(NetworkConnection conn, Guid slotGuid)
		{
			this.currentSlotGUID = slotGuid;
			this.currentSlot = GUIDManager.GetObject<PalletSlot>(slotGuid);
			if (this.currentSlot == null)
			{
				this.currentSlotGUID = Guid.Empty;
				Console.LogWarning("BindToSlot called but slotGuid is not valid", null);
				return;
			}
			this.currentSlot.SetOccupant(this);
			this.networkTransform.enabled = false;
			UnityEngine.Object.Destroy(this.rb);
			base.transform.SetParent(this.currentSlot.transform);
			base.transform.position = this.currentSlot.transform.position + this.currentSlot.transform.up * 0.1f;
			Vector3 vector = this.currentSlot.transform.forward;
			if (Vector3.Angle(base.transform.forward, -this.currentSlot.transform.forward) < Vector3.Angle(base.transform.forward, vector))
			{
				vector = -this.currentSlot.transform.forward;
			}
			if (Vector3.Angle(base.transform.forward, this.currentSlot.transform.right) < Vector3.Angle(base.transform.forward, vector))
			{
				vector = this.currentSlot.transform.right;
			}
			if (Vector3.Angle(base.transform.forward, -this.currentSlot.transform.right) < Vector3.Angle(base.transform.forward, vector))
			{
				vector = -this.currentSlot.transform.right;
			}
			base.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
			base.transform.localEulerAngles = new Vector3(0f, base.transform.localEulerAngles.y, 0f);
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x000F9278 File Offset: 0x000F7478
		private void RpcReader___Observers_BindToSlot_454078614(PooledReader PooledReader0, Channel channel)
		{
			Guid slotGuid = PooledReader0.ReadGuid();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___BindToSlot_454078614(null, slotGuid);
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x000F92AC File Offset: 0x000F74AC
		private void RpcWriter___Target_BindToSlot_454078614(NetworkConnection conn, Guid slotGuid)
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
			writer.WriteGuidAllocated(slotGuid);
			base.SendTargetRpc(3U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x000F9364 File Offset: 0x000F7564
		private void RpcReader___Target_BindToSlot_454078614(PooledReader PooledReader0, Channel channel)
		{
			Guid slotGuid = PooledReader0.ReadGuid();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___BindToSlot_454078614(base.LocalConnection, slotGuid);
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x000F939C File Offset: 0x000F759C
		private void RpcWriter___Server_ExitSlot_Server_2166136261()
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
			base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x000F9436 File Offset: 0x000F7636
		public void RpcLogic___ExitSlot_Server_2166136261()
		{
			this.ExitSlot();
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x000F9440 File Offset: 0x000F7640
		private void RpcReader___Server_ExitSlot_Server_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ExitSlot_Server_2166136261();
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x000F9470 File Offset: 0x000F7670
		private void RpcWriter___Observers_ExitSlot_2166136261()
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
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x000F951C File Offset: 0x000F771C
		private void RpcLogic___ExitSlot_2166136261()
		{
			if (this.currentSlot == null)
			{
				return;
			}
			this.currentSlot.SetOccupant(null);
			base.transform.SetParent(null);
			if (this.rb == null)
			{
				this.rb = base.gameObject.AddComponent<Rigidbody>();
			}
			this.rb.mass = this.rb_Mass;
			this.rb.drag = this.rb_Drag;
			this.rb.angularDrag = this.rb_AngularDrag;
			this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			if (base.IsOwner || (base.OwnerId == -1 && InstanceFinder.IsHost))
			{
				Console.Log("Exit slot, owner", null);
				this.rb.isKinematic = false;
				this.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			}
			else
			{
				Console.Log("Exit slot, not owner", null);
				this.rb.isKinematic = true;
				this.rb.interpolation = RigidbodyInterpolation.None;
			}
			this.networkTransform.enabled = true;
			this.currentSlotGUID = default(Guid);
			this.currentSlot = null;
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x000F9630 File Offset: 0x000F7830
		private void RpcReader___Observers_ExitSlot_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ExitSlot_2166136261();
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x000F9650 File Offset: 0x000F7850
		private void RpcWriter___Observers_CreateStoredItem_913707843(NetworkConnection conn, StorableItemInstance item, int gridIndex, Vector2 originCoord, float rotation, string jobID = "", bool network = true)
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
			writer.WriteStorableItemInstance(item);
			writer.WriteInt32(gridIndex, AutoPackType.Packed);
			writer.WriteVector2(originCoord);
			writer.WriteSingle(rotation, AutoPackType.Unpacked);
			writer.WriteString(jobID);
			writer.WriteBoolean(network);
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x000F9754 File Offset: 0x000F7954
		public void RpcLogic___CreateStoredItem_913707843(NetworkConnection conn, StorableItemInstance item, int gridIndex, Vector2 originCoord, float rotation, string jobID = "", bool network = true)
		{
			if (jobID != "")
			{
				if (this.completedJobs.Contains(jobID))
				{
					return;
				}
			}
			else
			{
				jobID = Guid.NewGuid().ToString();
			}
			this.completedJobs.Add(jobID);
			UnityEngine.Object.Instantiate<StoredItem>(item.StoredItem, this.storedItemContainer).GetComponent<StoredItem>();
			if (network)
			{
				this.CreateStoredItem_Server(item, gridIndex, originCoord, rotation, jobID);
			}
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x000F97CC File Offset: 0x000F79CC
		private void RpcReader___Observers_CreateStoredItem_913707843(PooledReader PooledReader0, Channel channel)
		{
			StorableItemInstance item = PooledReader0.ReadStorableItemInstance();
			int gridIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			Vector2 originCoord = PooledReader0.ReadVector2();
			float rotation = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			string jobID = PooledReader0.ReadString();
			bool network = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateStoredItem_913707843(null, item, gridIndex, originCoord, rotation, jobID, network);
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x000F9868 File Offset: 0x000F7A68
		private void RpcWriter___Target_CreateStoredItem_913707843(NetworkConnection conn, StorableItemInstance item, int gridIndex, Vector2 originCoord, float rotation, string jobID = "", bool network = true)
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
			writer.WriteStorableItemInstance(item);
			writer.WriteInt32(gridIndex, AutoPackType.Packed);
			writer.WriteVector2(originCoord);
			writer.WriteSingle(rotation, AutoPackType.Unpacked);
			writer.WriteString(jobID);
			writer.WriteBoolean(network);
			base.SendTargetRpc(7U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x000F9968 File Offset: 0x000F7B68
		private void RpcReader___Target_CreateStoredItem_913707843(PooledReader PooledReader0, Channel channel)
		{
			StorableItemInstance item = PooledReader0.ReadStorableItemInstance();
			int gridIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			Vector2 originCoord = PooledReader0.ReadVector2();
			float rotation = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			string jobID = PooledReader0.ReadString();
			bool network = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateStoredItem_913707843(base.LocalConnection, item, gridIndex, originCoord, rotation, jobID, network);
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x000F9A00 File Offset: 0x000F7C00
		private void RpcWriter___Server_CreateStoredItem_Server_1890711751(StorableItemInstance data, int gridIndex, Vector2 originCoord, float rotation, string jobID)
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
			writer.WriteStorableItemInstance(data);
			writer.WriteInt32(gridIndex, AutoPackType.Packed);
			writer.WriteVector2(originCoord);
			writer.WriteSingle(rotation, AutoPackType.Unpacked);
			writer.WriteString(jobID);
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x000F9AE5 File Offset: 0x000F7CE5
		private void RpcLogic___CreateStoredItem_Server_1890711751(StorableItemInstance data, int gridIndex, Vector2 originCoord, float rotation, string jobID)
		{
			this.CreateStoredItem(null, data, gridIndex, originCoord, rotation, jobID, false);
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x000F9AF8 File Offset: 0x000F7CF8
		private void RpcReader___Server_CreateStoredItem_Server_1890711751(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			StorableItemInstance data = PooledReader0.ReadStorableItemInstance();
			int gridIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			Vector2 originCoord = PooledReader0.ReadVector2();
			float rotation = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			string jobID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___CreateStoredItem_Server_1890711751(data, gridIndex, originCoord, rotation, jobID);
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x000F9B78 File Offset: 0x000F7D78
		private void RpcWriter___Observers_DestroyStoredItem_3261517793(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
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
			writer.WriteInt32(gridIndex, AutoPackType.Packed);
			writer.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated(coord);
			writer.WriteString(jobID);
			writer.WriteBoolean(network);
			base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x000F9C5C File Offset: 0x000F7E5C
		public void RpcLogic___DestroyStoredItem_3261517793(int gridIndex, Coordinate coord, string jobID = "", bool network = true)
		{
			if (jobID != "")
			{
				if (this.completedJobs.Contains(jobID))
				{
					return;
				}
			}
			else
			{
				jobID = Guid.NewGuid().ToString();
			}
			this.completedJobs.Add(jobID);
			List<StorageGrid> storageGrids = this.GetStorageGrids();
			if (gridIndex > storageGrids.Count)
			{
				Console.LogError("DestroyStoredItem: grid index out of range", null);
				return;
			}
			if (storageGrids[gridIndex].GetTile(coord) == null)
			{
				Console.LogError("DestroyStoredItem: no tile found at " + ((coord != null) ? coord.ToString() : null), null);
				return;
			}
			storageGrids[gridIndex].GetTile(coord).occupant.Destroy_Internal();
			if (network)
			{
				this.DestroyStoredItem_Server(gridIndex, coord, jobID);
			}
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x000F9D1C File Offset: 0x000F7F1C
		private void RpcReader___Observers_DestroyStoredItem_3261517793(PooledReader PooledReader0, Channel channel)
		{
			int gridIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			Coordinate coord = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds(PooledReader0);
			string jobID = PooledReader0.ReadString();
			bool network = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DestroyStoredItem_3261517793(gridIndex, coord, jobID, network);
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x000F9D90 File Offset: 0x000F7F90
		private void RpcWriter___Server_DestroyStoredItem_Server_3952619116(int gridIndex, Coordinate coord, string jobID)
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
			writer.WriteInt32(gridIndex, AutoPackType.Packed);
			writer.Write___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generated(coord);
			writer.WriteString(jobID);
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x000F9E56 File Offset: 0x000F8056
		private void RpcLogic___DestroyStoredItem_Server_3952619116(int gridIndex, Coordinate coord, string jobID)
		{
			this.DestroyStoredItem(gridIndex, coord, jobID, false);
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x000F9E64 File Offset: 0x000F8064
		private void RpcReader___Server_DestroyStoredItem_Server_3952619116(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int gridIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			Coordinate coord = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Tiles.CoordinateFishNet.Serializing.Generateds(PooledReader0);
			string jobID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___DestroyStoredItem_Server_3952619116(gridIndex, coord, jobID);
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x000F9EBC File Offset: 0x000F80BC
		protected virtual void dll()
		{
			this.rb_Mass = this.rb.mass;
			this.rb_Drag = this.rb.drag;
			this.rb_AngularDrag = this.rb.angularDrag;
		}

		// Token: 0x04002AE6 RID: 10982
		public static List<Pallet> palletsOwnedByLocalPlayer = new List<Pallet>();

		// Token: 0x04002AE7 RID: 10983
		public static int sizeX = 6;

		// Token: 0x04002AE8 RID: 10984
		public static int sizeY = 6;

		// Token: 0x04002AE9 RID: 10985
		[Header("Reference")]
		public Transform _storedItemContainer;

		// Token: 0x04002AEA RID: 10986
		public Rigidbody rb;

		// Token: 0x04002AEB RID: 10987
		public StorageGrid storageGrid;

		// Token: 0x04002AEC RID: 10988
		public NetworkTransform networkTransform;

		// Token: 0x04002AED RID: 10989
		protected List<Forklift> forkliftsInContact = new List<Forklift>();

		// Token: 0x04002AEE RID: 10990
		public Guid currentSlotGUID;

		// Token: 0x04002AEF RID: 10991
		private PalletSlot currentSlot;

		// Token: 0x04002AF0 RID: 10992
		private float timeSinceSlotCheck;

		// Token: 0x04002AF1 RID: 10993
		private float timeBoundToSlot;

		// Token: 0x04002AF2 RID: 10994
		private float rb_Mass;

		// Token: 0x04002AF3 RID: 10995
		private float rb_Drag;

		// Token: 0x04002AF4 RID: 10996
		private float rb_AngularDrag;

		// Token: 0x04002AF5 RID: 10997
		protected Dictionary<StoredItem, Employee> _reservedItems = new Dictionary<StoredItem, Employee>();

		// Token: 0x04002AF6 RID: 10998
		private List<string> completedJobs = new List<string>();

		// Token: 0x04002AF7 RID: 10999
		private bool dll_Excuted;

		// Token: 0x04002AF8 RID: 11000
		private bool dll_Excuted;
	}
}
