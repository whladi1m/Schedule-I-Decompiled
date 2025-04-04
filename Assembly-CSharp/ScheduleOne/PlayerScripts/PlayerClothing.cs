using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.AvatarFramework;
using ScheduleOne.Clothing;
using ScheduleOne.ItemFramework;
using UnityEngine;

namespace ScheduleOne.PlayerScripts
{
	// Token: 0x020005E2 RID: 1506
	public class PlayerClothing : NetworkBehaviour, IItemSlotOwner
	{
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06002700 RID: 9984 RVA: 0x0009F113 File Offset: 0x0009D313
		// (set) Token: 0x06002701 RID: 9985 RVA: 0x0009F11B File Offset: 0x0009D31B
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06002702 RID: 9986 RVA: 0x0009F124 File Offset: 0x0009D324
		private AvatarSettings appearanceSettings
		{
			get
			{
				return this.Player.Avatar.CurrentSettings;
			}
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x0009F138 File Offset: 0x0009D338
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.PlayerScripts.PlayerClothing_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x00065519 File Offset: 0x00063719
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			((IItemSlotOwner)this).SendItemsToClient(connection);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x0009F158 File Offset: 0x0009D358
		public void InsertClothing(ClothingInstance clothing)
		{
			EClothingSlot slot = (clothing.Definition as ClothingDefinition).Slot;
			if (!this.ClothingSlots.ContainsKey(slot))
			{
				Console.LogError("No slot found for clothing slot type: " + slot.ToString(), null);
				return;
			}
			this.ClothingSlots[slot].SetStoredItem(clothing, false);
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x0009F1B5 File Offset: 0x0009D3B5
		protected virtual void ClothingChanged()
		{
			this.RefreshAppearance();
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x0009F1C0 File Offset: 0x0009D3C0
		public virtual void RefreshAppearance()
		{
			AvatarSettings avatarSettings = UnityEngine.Object.Instantiate<AvatarSettings>(this.appearanceSettings);
			for (int i = 0; i < avatarSettings.BodyLayerSettings.Count; i++)
			{
				if (!(avatarSettings.BodyLayerSettings[i].layerPath == string.Empty))
				{
					AvatarLayer avatarLayer = Resources.Load<AvatarLayer>(avatarSettings.BodyLayerSettings[i].layerPath);
					ClothingInstance clothingInstance;
					if (!(avatarLayer == null) && avatarLayer.Order > 19 && !this.TryGetInventoryClothing(avatarLayer.AssetPath, avatarSettings.BodyLayerSettings[i].layerTint, out clothingInstance))
					{
						avatarSettings.BodyLayerSettings.RemoveAt(i);
						i--;
					}
				}
			}
			for (int j = 0; j < avatarSettings.AccessorySettings.Count; j++)
			{
				if (!(avatarSettings.AccessorySettings[j].path == string.Empty))
				{
					Accessory accessory = Resources.Load<Accessory>(avatarSettings.AccessorySettings[j].path);
					ClothingInstance clothingInstance2;
					if (!(accessory == null) && !this.TryGetInventoryClothing(accessory.AssetPath, avatarSettings.AccessorySettings[j].color, out clothingInstance2))
					{
						avatarSettings.AccessorySettings.RemoveAt(j);
						j--;
					}
				}
			}
			for (int k = 0; k < this.ItemSlots.Count; k++)
			{
				if (this.ItemSlots[k].Quantity > 0)
				{
					ClothingInstance clothingInstance3 = this.ItemSlots[k].ItemInstance as ClothingInstance;
					if (clothingInstance3 != null && !this.IsClothingApplied(avatarSettings, clothingInstance3))
					{
						this.ApplyClothing(avatarSettings, clothingInstance3);
					}
				}
			}
			this.Player.SetAvatarSettings(avatarSettings);
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x0009F36C File Offset: 0x0009D56C
		private bool TryGetInventoryClothing(string assetPath, Color color, out ClothingInstance clothing)
		{
			clothing = null;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].Quantity > 0)
				{
					ClothingInstance clothingInstance = this.ItemSlots[i].ItemInstance as ClothingInstance;
					if (clothingInstance != null)
					{
						ClothingDefinition clothingDefinition = clothingInstance.Definition as ClothingDefinition;
						if (!(clothingDefinition == null) && clothingInstance.Color.GetActualColor() == color && clothingDefinition.ClothingAssetPath == assetPath)
						{
							clothing = clothingInstance;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x0009F3FC File Offset: 0x0009D5FC
		private bool IsClothingApplied(AvatarSettings settings, ClothingInstance clothing)
		{
			if (clothing == null)
			{
				return false;
			}
			ClothingDefinition clothingDefinition = clothing.Definition as ClothingDefinition;
			for (int i = 0; i < settings.BodyLayerSettings.Count; i++)
			{
				if (settings.BodyLayerSettings[i].layerPath == clothingDefinition.ClothingAssetPath && settings.BodyLayerSettings[i].layerTint == clothing.Color.GetActualColor())
				{
					return true;
				}
			}
			for (int j = 0; j < settings.AccessorySettings.Count; j++)
			{
				if (settings.AccessorySettings[j].path == clothingDefinition.ClothingAssetPath && settings.AccessorySettings[j].color == clothing.Color.GetActualColor())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x0009F4D0 File Offset: 0x0009D6D0
		private void ApplyClothing(AvatarSettings settings, ClothingInstance clothing)
		{
			if (clothing == null)
			{
				return;
			}
			ClothingDefinition clothingDefinition = clothing.Definition as ClothingDefinition;
			if (clothingDefinition.ApplicationType == EClothingApplicationType.BodyLayer)
			{
				settings.BodyLayerSettings.Add(new AvatarSettings.LayerSetting
				{
					layerPath = clothingDefinition.ClothingAssetPath,
					layerTint = clothing.Color.GetActualColor()
				});
				return;
			}
			if (clothingDefinition.ApplicationType == EClothingApplicationType.Accessory)
			{
				settings.AccessorySettings.Add(new AvatarSettings.AccessorySetting
				{
					path = clothingDefinition.ClothingAssetPath,
					color = clothing.Color.GetActualColor()
				});
				return;
			}
			Console.LogError("Unknown clothing application type: " + clothingDefinition.ApplicationType.ToString(), null);
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x0009F581 File Offset: 0x0009D781
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x0009F5A8 File Offset: 0x0009D7A8
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

		// Token: 0x0600270D RID: 9997 RVA: 0x0009F607 File Offset: 0x0009D807
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x0009F625 File Offset: 0x0009D825
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x0009F643 File Offset: 0x0009D843
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x0009F67C File Offset: 0x0009D87C
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

		// Token: 0x06002712 RID: 10002 RVA: 0x0009F724 File Offset: 0x0009D924
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerClothingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.PlayerScripts.PlayerClothingAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x0009F7FA File Offset: 0x0009D9FA
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerClothingAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.PlayerScripts.PlayerClothingAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x0009F80D File Offset: 0x0009DA0D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x0009F81C File Offset: 0x0009DA1C
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
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x0009F8E2 File Offset: 0x0009DAE2
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x0009F90C File Offset: 0x0009DB0C
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

		// Token: 0x06002718 RID: 10008 RVA: 0x0009F974 File Offset: 0x0009DB74
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
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x0009FA3C File Offset: 0x0009DC3C
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x0009FA68 File Offset: 0x0009DC68
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

		// Token: 0x0600271B RID: 10011 RVA: 0x0009FABC File Offset: 0x0009DCBC
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
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x0009FB84 File Offset: 0x0009DD84
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

		// Token: 0x0600271D RID: 10013 RVA: 0x0009FBDC File Offset: 0x0009DDDC
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
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x0009FC9A File Offset: 0x0009DE9A
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x0009FCA4 File Offset: 0x0009DEA4
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

		// Token: 0x06002720 RID: 10016 RVA: 0x0009FD00 File Offset: 0x0009DF00
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
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x0009FDCD File Offset: 0x0009DFCD
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x0009FDE4 File Offset: 0x0009DFE4
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

		// Token: 0x06002723 RID: 10019 RVA: 0x0009FE3C File Offset: 0x0009E03C
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
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x0009FF1C File Offset: 0x0009E11C
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x0009FF4C File Offset: 0x0009E14C
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

		// Token: 0x06002726 RID: 10022 RVA: 0x0009FFD4 File Offset: 0x0009E1D4
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
			base.SendTargetRpc(6U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000A00B5 File Offset: 0x0009E2B5
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000A00E4 File Offset: 0x0009E2E4
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

		// Token: 0x06002729 RID: 10025 RVA: 0x000A0160 File Offset: 0x0009E360
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
			base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000A0244 File Offset: 0x0009E444
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

		// Token: 0x0600272B RID: 10027 RVA: 0x000A02B8 File Offset: 0x0009E4B8
		protected virtual void dll()
		{
			foreach (object obj in Enum.GetValues(typeof(EClothingSlot)))
			{
				EClothingSlot eclothingSlot = (EClothingSlot)obj;
				ItemSlot itemSlot = new ItemSlot();
				itemSlot.SetSlotOwner(this);
				itemSlot.AddFilter(new ItemFilter_ClothingSlot(eclothingSlot));
				ItemSlot itemSlot2 = itemSlot;
				itemSlot2.onItemDataChanged = (Action)Delegate.Combine(itemSlot2.onItemDataChanged, new Action(this.ClothingChanged));
				this.ClothingSlots.Add(eclothingSlot, itemSlot);
			}
		}

		// Token: 0x04001C6A RID: 7274
		public Player Player;

		// Token: 0x04001C6C RID: 7276
		public Dictionary<EClothingSlot, ItemSlot> ClothingSlots = new Dictionary<EClothingSlot, ItemSlot>();

		// Token: 0x04001C6D RID: 7277
		private List<ClothingInstance> appliedClothing = new List<ClothingInstance>();

		// Token: 0x04001C6E RID: 7278
		private bool dll_Excuted;

		// Token: 0x04001C6F RID: 7279
		private bool dll_Excuted;
	}
}
