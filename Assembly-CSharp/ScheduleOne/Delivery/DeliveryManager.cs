using System;
using System.Collections.Generic;
using System.IO;
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
using ScheduleOne.GameTime;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.Property;
using ScheduleOne.UI.Phone.Delivery;
using ScheduleOne.UI.Shop;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Delivery
{
	// Token: 0x02000703 RID: 1795
	public class DeliveryManager : NetworkSingleton<DeliveryManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06003080 RID: 12416 RVA: 0x000C9F3D File Offset: 0x000C813D
		public string SaveFolderName
		{
			get
			{
				return "Deliveries";
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06003081 RID: 12417 RVA: 0x000C9F3D File Offset: 0x000C813D
		public string SaveFileName
		{
			get
			{
				return "Deliveries";
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x000C9F44 File Offset: 0x000C8144
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06003083 RID: 12419 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06003084 RID: 12420 RVA: 0x000C9F4C File Offset: 0x000C814C
		// (set) Token: 0x06003085 RID: 12421 RVA: 0x000C9F54 File Offset: 0x000C8154
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06003086 RID: 12422 RVA: 0x000C9F5D File Offset: 0x000C815D
		// (set) Token: 0x06003087 RID: 12423 RVA: 0x000C9F65 File Offset: 0x000C8165
		public List<string> LocalExtraFolders { get; set; } = new List<string>
		{
			"DeliveryVehicles"
		};

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000C9F6E File Offset: 0x000C816E
		// (set) Token: 0x06003089 RID: 12425 RVA: 0x000C9F76 File Offset: 0x000C8176
		public bool HasChanged { get; set; }

		// Token: 0x0600308A RID: 12426 RVA: 0x000C9F7F File Offset: 0x000C817F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Delivery.DeliveryManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000C9F93 File Offset: 0x000C8193
		protected override void Start()
		{
			base.Start();
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onMinutePass = (Action)Delegate.Combine(instance.onMinutePass, new Action(this.OnMinPass));
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x000C9FC4 File Offset: 0x000C81C4
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			foreach (DeliveryInstance delivery in this.Deliveries)
			{
				this.SendDelivery(delivery);
			}
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000CA020 File Offset: 0x000C8220
		private void OnMinPass()
		{
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				return;
			}
			foreach (DeliveryInstance deliveryInstance in this.Deliveries.ToArray())
			{
				deliveryInstance.OnMinPass();
				if (InstanceFinder.IsServer)
				{
					if (deliveryInstance.TimeUntilArrival == 0 && deliveryInstance.Status != EDeliveryStatus.Arrived)
					{
						if (this.IsLoadingBayFree(deliveryInstance.Destination, deliveryInstance.LoadingDockIndex))
						{
							deliveryInstance.AddItemsToDeliveryVehicle();
							this.SetDeliveryState(deliveryInstance.DeliveryID, EDeliveryStatus.Arrived);
						}
						else if (deliveryInstance.Status != EDeliveryStatus.Waiting)
						{
							this.SetDeliveryState(deliveryInstance.DeliveryID, EDeliveryStatus.Waiting);
						}
					}
					if (deliveryInstance.Status == EDeliveryStatus.Arrived)
					{
						if (!this.minsSinceVehicleEmpty.ContainsKey(deliveryInstance))
						{
							this.minsSinceVehicleEmpty.Add(deliveryInstance, 0);
						}
						if (deliveryInstance.ActiveVehicle.Vehicle.Storage.ItemCount == 0 && deliveryInstance.ActiveVehicle.Vehicle.Storage.CurrentAccessor == null)
						{
							Dictionary<DeliveryInstance, int> dictionary = this.minsSinceVehicleEmpty;
							DeliveryInstance key = deliveryInstance;
							int num = dictionary[key];
							dictionary[key] = num + 1;
							if (this.minsSinceVehicleEmpty[deliveryInstance] >= 3)
							{
								this.SetDeliveryState(deliveryInstance.DeliveryID, EDeliveryStatus.Completed);
							}
						}
						else
						{
							this.minsSinceVehicleEmpty[deliveryInstance] = 0;
						}
					}
				}
			}
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000CA15F File Offset: 0x000C835F
		public bool IsLoadingBayFree(Property destination, int loadingDockIndex)
		{
			return !destination.LoadingDocks[loadingDockIndex].IsInUse;
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000CA171 File Offset: 0x000C8371
		[ServerRpc(RequireOwnership = false)]
		public void SendDelivery(DeliveryInstance delivery)
		{
			this.RpcWriter___Server_SendDelivery_2813439055(delivery);
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000CA180 File Offset: 0x000C8380
		[ObserversRpc]
		[TargetRpc]
		private void ReceiveDelivery(NetworkConnection conn, DeliveryInstance delivery)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveDelivery_2795369214(conn, delivery);
			}
			else
			{
				this.RpcWriter___Target_ReceiveDelivery_2795369214(conn, delivery);
			}
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000CA1B4 File Offset: 0x000C83B4
		[ObserversRpc(RunLocally = true)]
		private void SetDeliveryState(string deliveryID, EDeliveryStatus status)
		{
			this.RpcWriter___Observers_SetDeliveryState_316609003(deliveryID, status);
			this.RpcLogic___SetDeliveryState_316609003(deliveryID, status);
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000CA1E0 File Offset: 0x000C83E0
		private DeliveryInstance GetDelivery(string deliveryID)
		{
			return this.Deliveries.FirstOrDefault((DeliveryInstance d) => d.DeliveryID == deliveryID);
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x000CA214 File Offset: 0x000C8414
		public DeliveryInstance GetDelivery(Property destination)
		{
			return this.Deliveries.FirstOrDefault((DeliveryInstance d) => d.DestinationCode == destination.PropertyCode);
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x000CA248 File Offset: 0x000C8448
		public DeliveryInstance GetActiveShopDelivery(DeliveryShop shop)
		{
			return this.Deliveries.FirstOrDefault((DeliveryInstance d) => d.StoreName == shop.MatchingShopInterfaceName);
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000CA27C File Offset: 0x000C847C
		public ShopInterface GetShopInterface(string shopName)
		{
			return this.AllShops.Find((ShopInterface x) => x.ShopName == shopName);
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000CA2AD File Offset: 0x000C84AD
		public virtual string GetSaveString()
		{
			return new DeliveriesData(this.Deliveries.ToArray()).GetJson(true);
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x000CA2C8 File Offset: 0x000C84C8
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> result = new List<string>();
			this.writtenVehicles.Clear();
			((ISaveable)this).GetContainerFolder(parentFolderPath);
			string parentFolderPath2 = ((ISaveable)this).WriteFolder(parentFolderPath, "DeliveryVehicles");
			foreach (DeliveryInstance deliveryInstance in this.Deliveries)
			{
				if (!(deliveryInstance.ActiveVehicle == null))
				{
					new SaveRequest(deliveryInstance.ActiveVehicle.Vehicle, parentFolderPath2);
					this.writtenVehicles.Add(deliveryInstance.ActiveVehicle.Vehicle.SaveFolderName);
				}
			}
			return result;
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000CA378 File Offset: 0x000C8578
		public virtual void DeleteUnapprovedFiles(string parentFolderPath)
		{
			string[] directories = Directory.GetDirectories(((ISaveable)this).WriteFolder(parentFolderPath, "DeliveryVehicles"));
			for (int i = 0; i < directories.Length; i++)
			{
				if (!this.writtenVehicles.Contains(directories[i]))
				{
					try
					{
						Directory.Delete(directories[i], true);
					}
					catch (Exception ex)
					{
						Console.LogError("Failed to delete unapproved vehicle folder: " + directories[i] + " - " + ex.Message, null);
					}
				}
			}
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x000CA460 File Offset: 0x000C8660
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Delivery.DeliveryManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Delivery.DeliveryManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendDelivery_2813439055));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveDelivery_2795369214));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveDelivery_2795369214));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetDeliveryState_316609003));
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x000CA4E0 File Offset: 0x000C86E0
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Delivery.DeliveryManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Delivery.DeliveryManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x000CA4F9 File Offset: 0x000C86F9
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x000CA508 File Offset: 0x000C8708
		private void RpcWriter___Server_SendDelivery_2813439055(DeliveryInstance delivery)
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
			writer.Write___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generated(delivery);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x000CA5AF File Offset: 0x000C87AF
		public void RpcLogic___SendDelivery_2813439055(DeliveryInstance delivery)
		{
			this.ReceiveDelivery(null, delivery);
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000CA5BC File Offset: 0x000C87BC
		private void RpcReader___Server_SendDelivery_2813439055(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			DeliveryInstance delivery = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendDelivery_2813439055(delivery);
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000CA5F0 File Offset: 0x000C87F0
		private void RpcWriter___Observers_ReceiveDelivery_2795369214(NetworkConnection conn, DeliveryInstance delivery)
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
			writer.Write___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generated(delivery);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000CA6A8 File Offset: 0x000C88A8
		private void RpcLogic___ReceiveDelivery_2795369214(NetworkConnection conn, DeliveryInstance delivery)
		{
			if (this.GetDelivery(delivery.DeliveryID) != null)
			{
				return;
			}
			this.Deliveries.Add(delivery);
			delivery.SetStatus(delivery.Status);
			if (this.onDeliveryCreated != null)
			{
				this.onDeliveryCreated.Invoke(delivery);
			}
			this.HasChanged = true;
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000CA6F8 File Offset: 0x000C88F8
		private void RpcReader___Observers_ReceiveDelivery_2795369214(PooledReader PooledReader0, Channel channel)
		{
			DeliveryInstance delivery = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveDelivery_2795369214(null, delivery);
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000CA72C File Offset: 0x000C892C
		private void RpcWriter___Target_ReceiveDelivery_2795369214(NetworkConnection conn, DeliveryInstance delivery)
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
			writer.Write___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generated(delivery);
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000CA7E4 File Offset: 0x000C89E4
		private void RpcReader___Target_ReceiveDelivery_2795369214(PooledReader PooledReader0, Channel channel)
		{
			DeliveryInstance delivery = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Delivery.DeliveryInstanceFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveDelivery_2795369214(base.LocalConnection, delivery);
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x000CA81C File Offset: 0x000C8A1C
		private void RpcWriter___Observers_SetDeliveryState_316609003(string deliveryID, EDeliveryStatus status)
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
			writer.WriteString(deliveryID);
			writer.Write___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generated(status);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x000CA8E0 File Offset: 0x000C8AE0
		private void RpcLogic___SetDeliveryState_316609003(string deliveryID, EDeliveryStatus status)
		{
			DeliveryInstance delivery = this.GetDelivery(deliveryID);
			if (delivery != null)
			{
				delivery.SetStatus(status);
			}
			if (status == EDeliveryStatus.Completed)
			{
				if (this.onDeliveryCompleted != null)
				{
					this.onDeliveryCompleted.Invoke(delivery);
				}
				this.Deliveries.Remove(delivery);
			}
			this.HasChanged = true;
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x000CA92C File Offset: 0x000C8B2C
		private void RpcReader___Observers_SetDeliveryState_316609003(PooledReader PooledReader0, Channel channel)
		{
			string deliveryID = PooledReader0.ReadString();
			EDeliveryStatus status = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Delivery.EDeliveryStatusFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetDeliveryState_316609003(deliveryID, status);
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x000CA978 File Offset: 0x000C8B78
		protected virtual void dll()
		{
			base.Awake();
			this.AllShops = new List<ShopInterface>(UnityEngine.Object.FindObjectsOfType<ShopInterface>());
			this.InitializeSaveable();
		}

		// Token: 0x040022CA RID: 8906
		[HideInInspector]
		public List<ShopInterface> AllShops = new List<ShopInterface>();

		// Token: 0x040022CB RID: 8907
		public List<DeliveryInstance> Deliveries = new List<DeliveryInstance>();

		// Token: 0x040022CC RID: 8908
		public UnityEvent<DeliveryInstance> onDeliveryCreated;

		// Token: 0x040022CD RID: 8909
		public UnityEvent<DeliveryInstance> onDeliveryCompleted;

		// Token: 0x040022CE RID: 8910
		private DeliveriesLoader loader = new DeliveriesLoader();

		// Token: 0x040022D2 RID: 8914
		private List<string> writtenVehicles = new List<string>();

		// Token: 0x040022D3 RID: 8915
		private Dictionary<DeliveryInstance, int> minsSinceVehicleEmpty = new Dictionary<DeliveryInstance, int>();

		// Token: 0x040022D4 RID: 8916
		private bool dll_Excuted;

		// Token: 0x040022D5 RID: 8917
		private bool dll_Excuted;
	}
}
