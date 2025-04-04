using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Tools;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Storage
{
	// Token: 0x0200088E RID: 2190
	public class StorageEntity : NetworkBehaviour, IItemSlotOwner
	{
		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003B6A RID: 15210 RVA: 0x000FA284 File Offset: 0x000F8484
		public bool IsOpened
		{
			get
			{
				return this.CurrentAccessor != null;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06003B6B RID: 15211 RVA: 0x000FA292 File Offset: 0x000F8492
		// (set) Token: 0x06003B6C RID: 15212 RVA: 0x000FA29A File Offset: 0x000F849A
		public Player CurrentAccessor { get; protected set; }

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003B6D RID: 15213 RVA: 0x00065648 File Offset: 0x00063848
		public int ItemCount
		{
			get
			{
				return ((IItemSlotOwner)this).GetTotalItemCount();
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003B6E RID: 15214 RVA: 0x000FA2A3 File Offset: 0x000F84A3
		// (set) Token: 0x06003B6F RID: 15215 RVA: 0x000FA2AB File Offset: 0x000F84AB
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x06003B70 RID: 15216 RVA: 0x000FA2B4 File Offset: 0x000F84B4
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Storage.StorageEntity_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x000FA2D4 File Offset: 0x000F84D4
		protected virtual void Start()
		{
			MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
			instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Combine(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			if (this.EmptyOnSleep)
			{
				ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Combine(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(this.ClearContents));
			}
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x000FA330 File Offset: 0x000F8530
		protected virtual void OnDestroy()
		{
			if (NetworkSingleton<MoneyManager>.InstanceExists)
			{
				MoneyManager instance = NetworkSingleton<MoneyManager>.Instance;
				instance.onNetworthCalculation = (Action<MoneyManager.FloatContainer>)Delegate.Remove(instance.onNetworthCalculation, new Action<MoneyManager.FloatContainer>(this.GetNetworth));
			}
			ScheduleOne.GameTime.TimeManager.onSleepStart = (Action)Delegate.Remove(ScheduleOne.GameTime.TimeManager.onSleepStart, new Action(this.ClearContents));
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x000FA38C File Offset: 0x000F858C
		private void GetNetworth(MoneyManager.FloatContainer container)
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null)
				{
					container.ChangeValue(this.ItemSlots[i].ItemInstance.GetMonetaryValue());
				}
			}
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x000FA3DE File Offset: 0x000F85DE
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			((IItemSlotOwner)this).SendItemsToClient(connection);
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x000FA3F0 File Offset: 0x000F85F0
		protected virtual void FixedUpdate()
		{
			if (this.IsOpened && this.CurrentAccessor == Player.Local && this.MaxAccessDistance > 0f && Vector3.Distance(PlayerSingleton<PlayerMovement>.Instance.transform.position, base.transform.position) > this.MaxAccessDistance + 1f)
			{
				this.Close();
			}
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x000FA458 File Offset: 0x000F8658
		public Dictionary<StorableItemInstance, int> GetContentsDictionary()
		{
			Dictionary<StorableItemInstance, int> dictionary = new Dictionary<StorableItemInstance, int>();
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance is StorableItemInstance && this.ItemSlots[i].Quantity > 0 && !dictionary.ContainsKey(this.ItemSlots[i].ItemInstance as StorableItemInstance))
				{
					dictionary.Add(this.ItemSlots[i].ItemInstance as StorableItemInstance, this.ItemSlots[i].Quantity);
				}
			}
			return dictionary;
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x000FA512 File Offset: 0x000F8712
		public bool CanItemFit(ItemInstance item, int quantity = 1)
		{
			return this.HowManyCanFit(item) >= quantity;
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x000FA524 File Offset: 0x000F8724
		public int HowManyCanFit(ItemInstance item)
		{
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (!this.ItemSlots[i].IsLocked && !this.ItemSlots[i].IsAddLocked)
				{
					if (this.ItemSlots[i].ItemInstance == null)
					{
						num += item.StackLimit;
					}
					else if (this.ItemSlots[i].ItemInstance.CanStackWith(item, true))
					{
						num += item.StackLimit - this.ItemSlots[i].ItemInstance.Quantity;
					}
				}
			}
			return num;
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x000FA5D0 File Offset: 0x000F87D0
		public void InsertItem(ItemInstance item, bool network = true)
		{
			if (!this.CanItemFit(item, item.Quantity))
			{
				Console.LogWarning("StorageEntity InsertItem() called but CanItemFit() returned false", null);
				return;
			}
			int num = item.Quantity;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (!this.ItemSlots[i].IsLocked && !this.ItemSlots[i].IsAddLocked)
				{
					if (this.ItemSlots[i].ItemInstance == null)
					{
						num -= item.StackLimit;
						this.ItemSlots[i].SetStoredItem(item, !network);
						return;
					}
					if (this.ItemSlots[i].ItemInstance.CanStackWith(item, true))
					{
						int num2 = Mathf.Min(item.StackLimit - this.ItemSlots[i].ItemInstance.Quantity, num);
						num -= num2;
						this.ItemSlots[i].ChangeQuantity(-num2, network);
					}
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x000FA6D4 File Offset: 0x000F88D4
		protected virtual void ContentsChanged()
		{
			if (this.onContentsChanged != null)
			{
				this.onContentsChanged.Invoke();
			}
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x000FA6EC File Offset: 0x000F88EC
		public List<ItemInstance> GetAllItems()
		{
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null)
				{
					list.Add(this.ItemSlots[i].ItemInstance);
				}
			}
			return list;
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x000FA740 File Offset: 0x000F8940
		public void LoadFromItemSet(ItemInstance[] items)
		{
			int num = 0;
			while (num < items.Length && num < this.ItemSlots.Count)
			{
				this.ItemSlots[num].SetStoredItem(items[num], false);
				num++;
			}
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x000FA780 File Offset: 0x000F8980
		public void ClearContents()
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				this.ItemSlots[i].ClearStoredInstance(false);
			}
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x000FA7B5 File Offset: 0x000F89B5
		public void Open()
		{
			if (!this.CanBeOpened())
			{
				Console.LogWarning("StorageEntity Open() called but CanBeOpened() returned false", null);
				return;
			}
			Singleton<StorageMenu>.Instance.Open(this);
			this.SendAccessor(Player.Local.NetworkObject);
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x000FA7E6 File Offset: 0x000F89E6
		public void Close()
		{
			if (Singleton<StorageMenu>.Instance.OpenedStorageEntity != this)
			{
				Console.LogWarning("StorageEntity Close() called but StorageMenu.Instance.OpenedStorageEntity != this", null);
				return;
			}
			Singleton<StorageMenu>.Instance.CloseMenu();
			this.SendAccessor(null);
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x000FA817 File Offset: 0x000F8A17
		protected virtual void OnOpened()
		{
			if (this.onOpened != null)
			{
				this.onOpened.Invoke();
			}
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x000FA82C File Offset: 0x000F8A2C
		protected virtual void OnClosed()
		{
			if (this.onClosed != null)
			{
				this.onClosed.Invoke();
			}
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x000FA841 File Offset: 0x000F8A41
		public virtual bool CanBeOpened()
		{
			return !Singleton<ManagementClipboard>.Instance.IsEquipped && this.AccessSettings != StorageEntity.EAccessSettings.Closed && (this.AccessSettings != StorageEntity.EAccessSettings.SinglePlayerOnly || !(this.CurrentAccessor != null));
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x000FA875 File Offset: 0x000F8A75
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		private void SendAccessor(NetworkObject accessor)
		{
			this.RpcWriter___Server_SendAccessor_3323014238(accessor);
			this.RpcLogic___SendAccessor_3323014238(accessor);
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x000FA88C File Offset: 0x000F8A8C
		[ObserversRpc(RunLocally = true)]
		private void SetAccessor(NetworkObject accessor)
		{
			this.RpcWriter___Observers_SetAccessor_3323014238(accessor);
			this.RpcLogic___SetAccessor_3323014238(accessor);
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x000FA8AD File Offset: 0x000F8AAD
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x000FA8D4 File Offset: 0x000F8AD4
		[ObserversRpc(RunLocally = true)]
		[TargetRpc(RunLocally = true)]
		private void SetStoredInstance_Internal(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
			else
			{
				this.RpcWriter___Target_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x000FA933 File Offset: 0x000F8B33
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x000FA951 File Offset: 0x000F8B51
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x000FA96F File Offset: 0x000F8B6F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x000FA9A8 File Offset: 0x000F8BA8
		[TargetRpc(RunLocally = true)]
		[ObserversRpc(RunLocally = true)]
		private void SetSlotLocked_Internal(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
			else
			{
				this.RpcWriter___Target_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x000FAA7C File Offset: 0x000F8C7C
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Storage.StorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Storage.StorageEntityAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendAccessor_3323014238));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetAccessor_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(8U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x000FAB80 File Offset: 0x000F8D80
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Storage.StorageEntityAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Storage.StorageEntityAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x000FAB93 File Offset: 0x000F8D93
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x000FABA4 File Offset: 0x000F8DA4
		private void RpcWriter___Server_SendAccessor_3323014238(NetworkObject accessor)
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
			writer.WriteNetworkObject(accessor);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x000FAC4B File Offset: 0x000F8E4B
		private void RpcLogic___SendAccessor_3323014238(NetworkObject accessor)
		{
			this.SetAccessor(accessor);
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x000FAC54 File Offset: 0x000F8E54
		private void RpcReader___Server_SendAccessor_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject accessor = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAccessor_3323014238(accessor);
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x000FAC94 File Offset: 0x000F8E94
		private void RpcWriter___Observers_SetAccessor_3323014238(NetworkObject accessor)
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
			writer.WriteNetworkObject(accessor);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x000FAD4C File Offset: 0x000F8F4C
		private void RpcLogic___SetAccessor_3323014238(NetworkObject accessor)
		{
			Player currentAccessor = this.CurrentAccessor;
			if (accessor != null)
			{
				this.CurrentAccessor = accessor.GetComponent<Player>();
			}
			else
			{
				this.CurrentAccessor = null;
			}
			if (this.CurrentAccessor != null && currentAccessor == null)
			{
				this.OnOpened();
			}
			if (this.CurrentAccessor == null && currentAccessor != null)
			{
				this.OnClosed();
			}
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x000FADB8 File Offset: 0x000F8FB8
		private void RpcReader___Observers_SetAccessor_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject accessor = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetAccessor_3323014238(accessor);
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x000FADF4 File Offset: 0x000F8FF4
		private void RpcWriter___Server_SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x000FAEBA File Offset: 0x000F90BA
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x000FAEE4 File Offset: 0x000F90E4
		private void RpcReader___Server_SetStoredInstance_2652194801(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_2652194801(conn2, itemSlotIndex, instance);
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x000FAF4C File Offset: 0x000F914C
		private void RpcWriter___Observers_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x000FB014 File Offset: 0x000F9214
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x000FB040 File Offset: 0x000F9240
		private void RpcReader___Observers_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(null, itemSlotIndex, instance);
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x000FB094 File Offset: 0x000F9294
		private void RpcWriter___Target_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendTargetRpc(4U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x000FB15C File Offset: 0x000F935C
		private void RpcReader___Target_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(base.LocalConnection, itemSlotIndex, instance);
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x000FB1B4 File Offset: 0x000F93B4
		private void RpcWriter___Server_SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x000FB272 File Offset: 0x000F9472
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x000FB27C File Offset: 0x000F947C
		private void RpcReader___Server_SetItemSlotQuantity_1692629761(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x000FB2D8 File Offset: 0x000F94D8
		private void RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x000FB3A5 File Offset: 0x000F95A5
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x000FB3BC File Offset: 0x000F95BC
		private void RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x000FB414 File Offset: 0x000F9614
		private void RpcWriter___Server_SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendServerRpc(7U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x000FB4F4 File Offset: 0x000F96F4
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x000FB524 File Offset: 0x000F9724
		private void RpcReader___Server_SetSlotLocked_3170825843(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_3170825843(conn2, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x000FB5AC File Offset: 0x000F97AC
		private void RpcWriter___Target_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendTargetRpc(8U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x000FB68D File Offset: 0x000F988D
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x000FB6BC File Offset: 0x000F98BC
		private void RpcReader___Target_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(base.LocalConnection, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x000FB738 File Offset: 0x000F9938
		private void RpcWriter___Observers_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
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
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x000FB81C File Offset: 0x000F9A1C
		private void RpcReader___Observers_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(null, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x000FB890 File Offset: 0x000F9A90
		protected virtual void dll()
		{
			for (int i = 0; i < this.SlotCount; i++)
			{
				ItemSlot itemSlot = new ItemSlot();
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.ContentsChanged));
				itemSlot.SetSlotOwner(this);
			}
		}

		// Token: 0x04002B03 RID: 11011
		public const int MAX_SLOTS = 20;

		// Token: 0x04002B05 RID: 11013
		[Header("Settings")]
		public string StorageEntityName = "Storage Entity";

		// Token: 0x04002B06 RID: 11014
		public string StorageEntitySubtitle = string.Empty;

		// Token: 0x04002B07 RID: 11015
		[Range(1f, 20f)]
		public int SlotCount = 5;

		// Token: 0x04002B08 RID: 11016
		public bool EmptyOnSleep;

		// Token: 0x04002B09 RID: 11017
		[Header("Display Settings")]
		[Tooltip("How many rows to enforce when display contents in StorageMenu")]
		[Range(1f, 5f)]
		public int DisplayRowCount = 1;

		// Token: 0x04002B0A RID: 11018
		[Header("Access Settings")]
		public StorageEntity.EAccessSettings AccessSettings = StorageEntity.EAccessSettings.Full;

		// Token: 0x04002B0B RID: 11019
		[Tooltip("If the distance between this StorageEntity and the player is greater than this, the StorageMenu will be closed.")]
		[Range(0f, 10f)]
		public float MaxAccessDistance = 6f;

		// Token: 0x04002B0D RID: 11021
		[Header("Events")]
		[Tooltip("Invoked when this StorageEntity is accessed in the StorageMenu")]
		public UnityEvent onOpened;

		// Token: 0x04002B0E RID: 11022
		[Tooltip("Invoked when the StorageMenu is closed.")]
		public UnityEvent onClosed;

		// Token: 0x04002B0F RID: 11023
		[Tooltip("Invoked when the contents change in any way. i.e. an item is added, removed, or the quantity of an item changes.")]
		public UnityEvent onContentsChanged;

		// Token: 0x04002B10 RID: 11024
		private bool dll_Excuted;

		// Token: 0x04002B11 RID: 11025
		private bool dll_Excuted;

		// Token: 0x0200088F RID: 2191
		public enum EAccessSettings
		{
			// Token: 0x04002B13 RID: 11027
			Closed,
			// Token: 0x04002B14 RID: 11028
			SinglePlayerOnly,
			// Token: 0x04002B15 RID: 11029
			Full
		}
	}
}
