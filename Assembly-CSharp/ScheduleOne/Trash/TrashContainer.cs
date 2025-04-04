using System;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Trash
{
	// Token: 0x02000812 RID: 2066
	public class TrashContainer : NetworkBehaviour
	{
		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06003897 RID: 14487 RVA: 0x000EF6BC File Offset: 0x000ED8BC
		// (set) Token: 0x06003898 RID: 14488 RVA: 0x000EF6C4 File Offset: 0x000ED8C4
		public TrashContent Content { get; protected set; } = new TrashContent();

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06003899 RID: 14489 RVA: 0x000EF6CD File Offset: 0x000ED8CD
		public int TrashLevel
		{
			get
			{
				return this.Content.GetTotalSize();
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x0600389A RID: 14490 RVA: 0x000EF6DA File Offset: 0x000ED8DA
		public float NormalizedTrashLevel
		{
			get
			{
				return (float)this.Content.GetTotalSize() / (float)this.TrashCapacity;
			}
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x000EF6F0 File Offset: 0x000ED8F0
		public virtual void AddTrash(TrashItem item)
		{
			this.SendTrash(item.ID, 1);
			item.DestroyTrash();
			if (InstanceFinder.IsServer)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("ContainedTrashItems", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("ContainedTrashItems") + 1f).ToString(), true);
			}
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x000EF744 File Offset: 0x000ED944
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.Content.GetTotalSize() > 0)
			{
				this.LoadContent(connection, this.Content.GetData());
			}
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x000EF76D File Offset: 0x000ED96D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendTrash(string trashID, int quantity)
		{
			this.RpcWriter___Server_SendTrash_3643459082(trashID, quantity);
			this.RpcLogic___SendTrash_3643459082(trashID, quantity);
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x000EF78C File Offset: 0x000ED98C
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void AddTrash(NetworkConnection conn, string trashID, int quantity)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_AddTrash_3905681115(conn, trashID, quantity);
				this.RpcLogic___AddTrash_3905681115(conn, trashID, quantity);
			}
			else
			{
				this.RpcWriter___Target_AddTrash_3905681115(conn, trashID, quantity);
			}
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x000EF7D9 File Offset: 0x000ED9D9
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendClear()
		{
			this.RpcWriter___Server_SendClear_2166136261();
			this.RpcLogic___SendClear_2166136261();
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x000EF7E7 File Offset: 0x000ED9E7
		[ObserversRpc(RunLocally = true)]
		private void Clear()
		{
			this.RpcWriter___Observers_Clear_2166136261();
			this.RpcLogic___Clear_2166136261();
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x000EF7F5 File Offset: 0x000ED9F5
		[TargetRpc]
		private void LoadContent(NetworkConnection conn, TrashContentData data)
		{
			this.RpcWriter___Target_LoadContent_189522235(conn, data);
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x000EF808 File Offset: 0x000EDA08
		public void TriggerEnter(Collider other)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.TrashLevel >= this.TrashCapacity)
			{
				return;
			}
			TrashItem componentInParent = other.GetComponentInParent<TrashItem>();
			if (componentInParent == null)
			{
				return;
			}
			if (!componentInParent.CanGoInContainer)
			{
				return;
			}
			this.AddTrash(componentInParent);
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x000EF84D File Offset: 0x000EDA4D
		public bool CanBeBagged()
		{
			return this.TrashLevel > 0;
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x000EF858 File Offset: 0x000EDA58
		public void BagTrash()
		{
			NetworkSingleton<TrashManager>.Instance.CreateTrashBag(NetworkSingleton<TrashManager>.Instance.TrashBagPrefab.ID, this.TrashBagDropLocation.position, this.TrashBagDropLocation.rotation, this.Content.GetData(), this.TrashBagDropLocation.forward * 3f, "", false);
			this.SendClear();
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("TrashContainersBagged", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("TrashContainersBagged") + 1f).ToString(), true);
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x000EF90C File Offset: 0x000EDB0C
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Trash.TrashContainerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Trash.TrashContainerAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendTrash_3643459082));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_AddTrash_3905681115));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_AddTrash_3905681115));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendClear_2166136261));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_Clear_2166136261));
			base.RegisterTargetRpc(5U, new ClientRpcDelegate(this.RpcReader___Target_LoadContent_189522235));
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x000EF9B4 File Offset: 0x000EDBB4
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Trash.TrashContainerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Trash.TrashContainerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x000EF9C7 File Offset: 0x000EDBC7
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x000EF9D8 File Offset: 0x000EDBD8
		private void RpcWriter___Server_SendTrash_3643459082(string trashID, int quantity)
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
			writer.WriteString(trashID);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x000EFA91 File Offset: 0x000EDC91
		private void RpcLogic___SendTrash_3643459082(string trashID, int quantity)
		{
			this.AddTrash(null, trashID, quantity);
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x000EFA9C File Offset: 0x000EDC9C
		private void RpcReader___Server_SendTrash_3643459082(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string trashID = PooledReader0.ReadString();
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendTrash_3643459082(trashID, quantity);
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x000EFAF0 File Offset: 0x000EDCF0
		private void RpcWriter___Observers_AddTrash_3905681115(NetworkConnection conn, string trashID, int quantity)
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
			writer.WriteString(trashID);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x000EFBB8 File Offset: 0x000EDDB8
		private void RpcLogic___AddTrash_3905681115(NetworkConnection conn, string trashID, int quantity)
		{
			this.Content.AddTrash(trashID, quantity);
			if (this.onTrashAdded != null)
			{
				this.onTrashAdded.Invoke(trashID);
			}
			if (this.onTrashLevelChanged != null)
			{
				this.onTrashLevelChanged.Invoke();
			}
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000EFBF0 File Offset: 0x000EDDF0
		private void RpcReader___Observers_AddTrash_3905681115(PooledReader PooledReader0, Channel channel)
		{
			string trashID = PooledReader0.ReadString();
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddTrash_3905681115(null, trashID, quantity);
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x000EFC44 File Offset: 0x000EDE44
		private void RpcWriter___Target_AddTrash_3905681115(NetworkConnection conn, string trashID, int quantity)
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
			writer.WriteString(trashID);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x000EFD0C File Offset: 0x000EDF0C
		private void RpcReader___Target_AddTrash_3905681115(PooledReader PooledReader0, Channel channel)
		{
			string trashID = PooledReader0.ReadString();
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddTrash_3905681115(base.LocalConnection, trashID, quantity);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x000EFD5C File Offset: 0x000EDF5C
		private void RpcWriter___Server_SendClear_2166136261()
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
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x000EFDF6 File Offset: 0x000EDFF6
		private void RpcLogic___SendClear_2166136261()
		{
			this.Clear();
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x000EFE00 File Offset: 0x000EE000
		private void RpcReader___Server_SendClear_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendClear_2166136261();
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x000EFE30 File Offset: 0x000EE030
		private void RpcWriter___Observers_Clear_2166136261()
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
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x000EFED9 File Offset: 0x000EE0D9
		private void RpcLogic___Clear_2166136261()
		{
			this.Content.Clear();
			if (this.onTrashLevelChanged != null)
			{
				this.onTrashLevelChanged.Invoke();
			}
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x000EFEFC File Offset: 0x000EE0FC
		private void RpcReader___Observers_Clear_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___Clear_2166136261();
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x000EFF28 File Offset: 0x000EE128
		private void RpcWriter___Target_LoadContent_189522235(NetworkConnection conn, TrashContentData data)
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
			writer.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated(data);
			base.SendTargetRpc(5U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x000EFFDD File Offset: 0x000EE1DD
		private void RpcLogic___LoadContent_189522235(NetworkConnection conn, TrashContentData data)
		{
			this.Content.LoadFromData(data);
			if (this.onTrashLevelChanged != null)
			{
				this.onTrashLevelChanged.Invoke();
			}
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x000F0000 File Offset: 0x000EE200
		private void RpcReader___Target_LoadContent_189522235(PooledReader PooledReader0, Channel channel)
		{
			TrashContentData data = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___LoadContent_189522235(base.LocalConnection, data);
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x000EF9C7 File Offset: 0x000EDBC7
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0400291E RID: 10526
		[Header("Settings")]
		[Range(1f, 50f)]
		public int TrashCapacity = 10;

		// Token: 0x0400291F RID: 10527
		[Header("Settings")]
		public Transform TrashBagDropLocation;

		// Token: 0x04002920 RID: 10528
		public UnityEvent<string> onTrashAdded;

		// Token: 0x04002921 RID: 10529
		public UnityEvent onTrashLevelChanged;

		// Token: 0x04002922 RID: 10530
		private bool dll_Excuted;

		// Token: 0x04002923 RID: 10531
		private bool dll_Excuted;
	}
}
