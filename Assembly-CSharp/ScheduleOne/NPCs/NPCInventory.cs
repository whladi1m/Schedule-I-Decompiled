using System;
using System.Collections.Generic;
using System.Linq;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using ScheduleOne.UI;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs
{
	// Token: 0x0200044E RID: 1102
	public class NPCInventory : NetworkBehaviour, IItemSlotOwner
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x000654AD File Offset: 0x000636AD
		// (set) Token: 0x060016FB RID: 5883 RVA: 0x000654B5 File Offset: 0x000636B5
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x060016FC RID: 5884 RVA: 0x000654C0 File Offset: 0x000636C0
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.NPCs.NPCInventory_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000654DF File Offset: 0x000636DF
		protected virtual void Start()
		{
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnSleepStart));
			NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.AddListener(new UnityAction(this.OnSleepStart));
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00065519 File Offset: 0x00063719
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			((IItemSlotOwner)this).SendItemsToClient(connection);
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00065532 File Offset: 0x00063732
		private void OnDestroy()
		{
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.InstanceExists)
			{
				NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance._onSleepStart.RemoveListener(new UnityAction(this.OnSleepStart));
			}
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00065558 File Offset: 0x00063758
		protected virtual void OnSleepStart()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.ClearInventoryEachNight)
			{
				foreach (ItemSlot itemSlot in this.ItemSlots)
				{
					itemSlot.ClearStoredInstance(false);
				}
			}
			if (this.GetTotalItemCount() >= 3)
			{
				return;
			}
			if (this.RandomCash)
			{
				int num = UnityEngine.Random.Range(this.RandomCashMin, this.RandomCashMax);
				if (num > 0)
				{
					CashInstance cashInstance = NetworkSingleton<MoneyManager>.Instance.GetCashInstance((float)num);
					this.InsertItem(cashInstance, true);
				}
			}
			if (this.RandomItems)
			{
				int num2 = UnityEngine.Random.Range(this.RandomItemMin, this.RandomItemMax + 1);
				for (int i = 0; i < num2; i++)
				{
					ItemInstance defaultInstance = this.RandomItemDefinitions[UnityEngine.Random.Range(0, this.RandomItemDefinitions.Length)].GetDefaultInstance(1);
					this.InsertItem(defaultInstance, true);
				}
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00065648 File Offset: 0x00063848
		public int GetItemCount()
		{
			return ((IItemSlotOwner)this).GetTotalItemCount();
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00065650 File Offset: 0x00063850
		public int _GetItemAmount(string id)
		{
			return ((IItemSlotOwner)this).GetItemCount(id);
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x0006565C File Offset: 0x0006385C
		public int GetIdenticalItemAmount(ItemInstance item)
		{
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].Quantity != 0 && this.ItemSlots[i].ItemInstance.CanStackWith(item, false))
				{
					num += this.ItemSlots[i].Quantity;
				}
			}
			return num;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x000656C4 File Offset: 0x000638C4
		public int GetMaxItemCount(string[] ids)
		{
			int[] array = new int[ids.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((IItemSlotOwner)this).GetItemCount(ids[i]);
			}
			if (array.Length == 0)
			{
				return 0;
			}
			return array.Max();
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00065700 File Offset: 0x00063900
		public bool CanItemFit(ItemInstance item, int quantity = 1)
		{
			return this.HowManyCanFit(item) >= quantity;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00065710 File Offset: 0x00063910
		public int HowManyCanFit(ItemInstance item)
		{
			if (item == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i] != null && !this.ItemSlots[i].IsLocked && !this.ItemSlots[i].IsAddLocked)
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

		// Token: 0x06001707 RID: 5895 RVA: 0x000657D0 File Offset: 0x000639D0
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
					if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance.CanStackWith(item, true))
					{
						int num2 = Mathf.Min(item.StackLimit - this.ItemSlots[i].ItemInstance.Quantity, num);
						num -= num2;
						this.ItemSlots[i].ChangeQuantity(num2, network);
					}
					if (num <= 0)
					{
						return;
					}
				}
			}
			for (int j = 0; j < this.ItemSlots.Count; j++)
			{
				if (!this.ItemSlots[j].IsLocked && !this.ItemSlots[j].IsAddLocked)
				{
					if (this.ItemSlots[j].ItemInstance == null)
					{
						num -= item.StackLimit;
						this.ItemSlots[j].SetStoredItem(item, !network);
						return;
					}
					if (num <= 0)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00065928 File Offset: 0x00063B28
		public ItemInstance GetFirstItem(string id, NPCInventory.ItemFilter filter = null)
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance.ID == id && (filter == null || filter(this.ItemSlots[i].ItemInstance)))
				{
					return this.ItemSlots[i].ItemInstance;
				}
			}
			return null;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x000659AC File Offset: 0x00063BAC
		public ItemInstance GetFirstIdenticalItem(ItemInstance item, NPCInventory.ItemFilter filter = null)
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null && this.ItemSlots[i].ItemInstance.CanStackWith(item, false) && (filter == null || filter(this.ItemSlots[i].ItemInstance)))
				{
					return this.ItemSlots[i].ItemInstance;
				}
			}
			return null;
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x00065A2B File Offset: 0x00063C2B
		protected virtual void InventoryContentsChanged()
		{
			if (this.onContentsChanged != null)
			{
				this.onContentsChanged.Invoke();
			}
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00065A40 File Offset: 0x00063C40
		public int GetTotalItemCount()
		{
			int num = 0;
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].ItemInstance != null)
				{
					num += this.ItemSlots[i].ItemInstance.Quantity;
				}
			}
			return num;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00065A92 File Offset: 0x00063C92
		public void Hovered()
		{
			if (this.CanPickpocket())
			{
				this.PickpocketIntObj.SetMessage("Pickpocket");
				this.PickpocketIntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.PickpocketIntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00065AC5 File Offset: 0x00063CC5
		public void Interacted()
		{
			if (this.CanPickpocket())
			{
				this.StartPickpocket();
			}
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x00065AD5 File Offset: 0x00063CD5
		private void StartPickpocket()
		{
			Singleton<PickpocketScreen>.Instance.Open(this.npc);
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00065AE7 File Offset: 0x00063CE7
		public void ExpirePickpocket()
		{
			this.timeOnLastExpire = Time.time;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00065AF4 File Offset: 0x00063CF4
		private bool CanPickpocket()
		{
			return this.CanBePickpocketed && PlayerSingleton<PlayerMovement>.Instance.isCrouched && Player.Local.CrimeData.CurrentPursuitLevel == PlayerCrimeData.EPursuitLevel.None && Time.time - this.timeOnLastExpire >= 30f && this.npc.IsConscious && !this.npc.behaviour.CallPoliceBehaviour.Active && !this.npc.behaviour.CombatBehaviour.Active && !this.npc.behaviour.FacePlayerBehaviour.Active && !this.npc.behaviour.FleeBehaviour.Active && !this.npc.behaviour.GenericDialogueBehaviour.Active && !this.npc.behaviour.StationaryBehaviour.Active && !this.npc.behaviour.RequestProductBehaviour.Active && !GameManager.IS_TUTORIAL;
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00065C0C File Offset: 0x00063E0C
		[Button]
		public void PrintInventoryContents()
		{
			for (int i = 0; i < this.ItemSlots.Count; i++)
			{
				if (this.ItemSlots[i].Quantity != 0)
				{
					Console.Log(string.Concat(new string[]
					{
						"Slot ",
						i.ToString(),
						": ",
						this.ItemSlots[i].ItemInstance.Name,
						" x",
						this.ItemSlots[i].Quantity.ToString()
					}), null);
				}
			}
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x00065CAD File Offset: 0x00063EAD
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x00065CD4 File Offset: 0x00063ED4
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

		// Token: 0x06001714 RID: 5908 RVA: 0x00065D33 File Offset: 0x00063F33
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x00065D51 File Offset: 0x00063F51
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x00065D6F File Offset: 0x00063F6F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00065DA8 File Offset: 0x00063FA8
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

		// Token: 0x06001719 RID: 5913 RVA: 0x00065E8C File Offset: 0x0006408C
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCInventoryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.NPCInventoryAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x00065F62 File Offset: 0x00064162
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.NPCInventoryAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.NPCInventoryAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00065F75 File Offset: 0x00064175
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00065F84 File Offset: 0x00064184
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

		// Token: 0x0600171D RID: 5917 RVA: 0x0006604A File Offset: 0x0006424A
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x00066074 File Offset: 0x00064274
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

		// Token: 0x0600171F RID: 5919 RVA: 0x000660DC File Offset: 0x000642DC
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

		// Token: 0x06001720 RID: 5920 RVA: 0x000661A4 File Offset: 0x000643A4
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x000661D0 File Offset: 0x000643D0
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

		// Token: 0x06001722 RID: 5922 RVA: 0x00066224 File Offset: 0x00064424
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

		// Token: 0x06001723 RID: 5923 RVA: 0x000662EC File Offset: 0x000644EC
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

		// Token: 0x06001724 RID: 5924 RVA: 0x00066344 File Offset: 0x00064544
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

		// Token: 0x06001725 RID: 5925 RVA: 0x00066402 File Offset: 0x00064602
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0006640C File Offset: 0x0006460C
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

		// Token: 0x06001727 RID: 5927 RVA: 0x00066468 File Offset: 0x00064668
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

		// Token: 0x06001728 RID: 5928 RVA: 0x00066535 File Offset: 0x00064735
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x0006654C File Offset: 0x0006474C
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

		// Token: 0x0600172A RID: 5930 RVA: 0x000665A4 File Offset: 0x000647A4
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

		// Token: 0x0600172B RID: 5931 RVA: 0x00066684 File Offset: 0x00064884
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x0600172C RID: 5932 RVA: 0x000666B4 File Offset: 0x000648B4
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

		// Token: 0x0600172D RID: 5933 RVA: 0x0006673C File Offset: 0x0006493C
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

		// Token: 0x0600172E RID: 5934 RVA: 0x0006681D File Offset: 0x00064A1D
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x0006684C File Offset: 0x00064A4C
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

		// Token: 0x06001730 RID: 5936 RVA: 0x000668C8 File Offset: 0x00064AC8
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

		// Token: 0x06001731 RID: 5937 RVA: 0x000669AC File Offset: 0x00064BAC
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

		// Token: 0x06001732 RID: 5938 RVA: 0x00066A20 File Offset: 0x00064C20
		protected virtual void dll()
		{
			for (int i = 0; i < this.SlotCount; i++)
			{
				ItemSlot itemSlot = new ItemSlot();
				itemSlot.SetSlotOwner(this);
				itemSlot.onItemDataChanged = (Action)Delegate.Combine(itemSlot.onItemDataChanged, new Action(this.InventoryContentsChanged));
			}
			if (Application.isEditor)
			{
				ItemDefinition[] testItems = this.TestItems;
				for (int j = 0; j < testItems.Length; j++)
				{
					ItemInstance defaultInstance = testItems[j].GetDefaultInstance(1);
					this.InsertItem(defaultInstance, true);
				}
			}
			this.npc = base.GetComponent<NPC>();
			this.PickpocketIntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.PickpocketIntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x040014ED RID: 5357
		public InteractableObject PickpocketIntObj;

		// Token: 0x040014EE RID: 5358
		public const float COOLDOWN = 30f;

		// Token: 0x040014EF RID: 5359
		[Header("Settings")]
		public int SlotCount = 5;

		// Token: 0x040014F0 RID: 5360
		public bool CanBePickpocketed = true;

		// Token: 0x040014F1 RID: 5361
		public bool ClearInventoryEachNight = true;

		// Token: 0x040014F2 RID: 5362
		public ItemDefinition[] TestItems;

		// Token: 0x040014F3 RID: 5363
		[Header("Random cash")]
		public bool RandomCash = true;

		// Token: 0x040014F4 RID: 5364
		public int RandomCashMin;

		// Token: 0x040014F5 RID: 5365
		public int RandomCashMax = 100;

		// Token: 0x040014F6 RID: 5366
		[Header("Random items")]
		public bool RandomItems = true;

		// Token: 0x040014F7 RID: 5367
		public StorableItemDefinition[] RandomItemDefinitions;

		// Token: 0x040014F8 RID: 5368
		public int RandomItemMin = -1;

		// Token: 0x040014F9 RID: 5369
		public int RandomItemMax = 2;

		// Token: 0x040014FA RID: 5370
		private NPC npc;

		// Token: 0x040014FC RID: 5372
		public UnityEvent onContentsChanged;

		// Token: 0x040014FD RID: 5373
		private float timeOnLastExpire = -100f;

		// Token: 0x040014FE RID: 5374
		private bool dll_Excuted;

		// Token: 0x040014FF RID: 5375
		private bool dll_Excuted;

		// Token: 0x0200044F RID: 1103
		// (Invoke) Token: 0x06001734 RID: 5940
		public delegate bool ItemFilter(ItemInstance item);
	}
}
