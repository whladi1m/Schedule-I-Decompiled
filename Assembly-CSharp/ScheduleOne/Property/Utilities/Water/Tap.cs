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
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Interaction;
using ScheduleOne.ItemFramework;
using ScheduleOne.Management;
using ScheduleOne.ObjectScripts;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.PlayerScripts;
using ScheduleOne.PlayerTasks;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Property.Utilities.Water
{
	// Token: 0x0200080A RID: 2058
	public class Tap : NetworkBehaviour, IUsable
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06003821 RID: 14369 RVA: 0x000ED425 File Offset: 0x000EB625
		// (set) Token: 0x06003822 RID: 14370 RVA: 0x000ED42D File Offset: 0x000EB62D
		public bool IsHeldOpen
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<IsHeldOpen>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<IsHeldOpen>k__BackingField(value, true);
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06003823 RID: 14371 RVA: 0x000ED437 File Offset: 0x000EB637
		public float ActualFlowRate
		{
			get
			{
				return 6f * this.tapFlow;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06003824 RID: 14372 RVA: 0x000ED445 File Offset: 0x000EB645
		// (set) Token: 0x06003825 RID: 14373 RVA: 0x000ED44D File Offset: 0x000EB64D
		public NetworkObject NPCUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<NPCUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<NPCUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06003826 RID: 14374 RVA: 0x000ED457 File Offset: 0x000EB657
		// (set) Token: 0x06003827 RID: 14375 RVA: 0x000ED45F File Offset: 0x000EB65F
		public NetworkObject PlayerUserObject
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<PlayerUserObject>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.sync___set_value_<PlayerUserObject>k__BackingField(value, true);
			}
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x000ED469 File Offset: 0x000EB669
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Property.Utilities.Water.Tap_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x000ED480 File Offset: 0x000EB680
		protected virtual void LateUpdate()
		{
			float num = 2f;
			if (this.IsHeldOpen)
			{
				this.tapFlow = Mathf.Clamp(this.tapFlow + Time.deltaTime * num, 0f, 1f);
			}
			else
			{
				this.tapFlow = Mathf.Clamp(this.tapFlow - Time.deltaTime * num, 0f, 1f);
			}
			this.UpdateTapVisuals();
			this.UpdateWaterSound();
			if (!this.intObjSetThisFrame)
			{
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
			}
			this.intObjSetThisFrame = false;
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x000ED50A File Offset: 0x000EB70A
		public void SetInteractableObject(string message, InteractableObject.EInteractableState state)
		{
			this.intObjSetThisFrame = true;
			this.IntObj.SetMessage(message);
			this.IntObj.SetInteractableState(state);
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x000ED52C File Offset: 0x000EB72C
		protected void UpdateTapVisuals()
		{
			this.Handle.transform.localEulerAngles = new Vector3(0f, -this.tapFlow * 360f, 0f);
			if (this.tapFlow > 0f)
			{
				this.WaterParticles.main.startSize = new ParticleSystem.MinMaxCurve(0.075f * this.tapFlow, 0.1f * this.tapFlow);
				if (!this.WaterParticles.isPlaying)
				{
					this.WaterParticles.Play();
					return;
				}
			}
			else if (this.WaterParticles.isPlaying)
			{
				this.WaterParticles.Stop();
			}
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x000ED5D4 File Offset: 0x000EB7D4
		protected void UpdateWaterSound()
		{
			if (this.tapFlow > 0.01f)
			{
				this.WaterRunningSound.VolumeMultiplier = this.tapFlow;
				if (!this.WaterRunningSound.isPlaying)
				{
					this.WaterRunningSound.Play();
					return;
				}
			}
			else if (this.WaterRunningSound.isPlaying)
			{
				this.WaterRunningSound.Stop();
			}
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x000ED630 File Offset: 0x000EB830
		public void Hovered()
		{
			if (this.CanInteract())
			{
				this.IntObj.SetMessage("Fill watering can");
				this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Default);
				return;
			}
			this.IntObj.SetInteractableState(InteractableObject.EInteractableState.Disabled);
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x000ED663 File Offset: 0x000EB863
		public void Interacted()
		{
			if (!this.CanInteract())
			{
				return;
			}
			HotbarSlot equippedSlot = PlayerSingleton<PlayerInventory>.Instance.equippedSlot;
			new FillWateringCan(this, ((equippedSlot != null) ? equippedSlot.ItemInstance : null) as WateringCanInstance);
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x000ED690 File Offset: 0x000EB890
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetPlayerUser(NetworkObject playerObject)
		{
			this.RpcWriter___Server_SetPlayerUser_3323014238(playerObject);
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x000ED6B1 File Offset: 0x000EB8B1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SetNPCUser(NetworkObject npcObject)
		{
			this.RpcWriter___Server_SetNPCUser_3323014238(npcObject);
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x000ED6C7 File Offset: 0x000EB8C7
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetHeldOpen(bool open)
		{
			this.RpcWriter___Server_SetHeldOpen_1140765316(open);
			this.RpcLogic___SetHeldOpen_1140765316(open);
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x000ED6E0 File Offset: 0x000EB8E0
		protected virtual bool CanInteract()
		{
			HotbarSlot equippedSlot = PlayerSingleton<PlayerInventory>.Instance.equippedSlot;
			ItemInstance itemInstance = (equippedSlot != null) ? equippedSlot.ItemInstance : null;
			if (itemInstance == null)
			{
				return false;
			}
			WateringCanInstance wateringCanInstance = itemInstance as WateringCanInstance;
			return wateringCanInstance != null && wateringCanInstance.CurrentFillAmount < 15f && !((IUsable)this).IsInUse;
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x000ED72F File Offset: 0x000EB92F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendWateringCanModel(string ID)
		{
			this.RpcWriter___Server_SendWateringCanModel_3615296227(ID);
			this.RpcLogic___SendWateringCanModel_3615296227(ID);
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x000ED745 File Offset: 0x000EB945
		[ObserversRpc(RunLocally = true)]
		private void CreateWateringCanModel(string ID)
		{
			this.RpcWriter___Observers_CreateWateringCanModel_3615296227(ID);
			this.RpcLogic___CreateWateringCanModel_3615296227(ID);
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x000ED75B File Offset: 0x000EB95B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SendClearWateringCanModelModel()
		{
			this.RpcWriter___Server_SendClearWateringCanModelModel_2166136261();
			this.RpcLogic___SendClearWateringCanModelModel_2166136261();
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x000ED769 File Offset: 0x000EB969
		[ObserversRpc(RunLocally = true)]
		private void ClearWateringCanModel()
		{
			this.RpcWriter___Observers_ClearWateringCanModel_2166136261();
			this.RpcLogic___ClearWateringCanModel_2166136261();
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x000ED778 File Offset: 0x000EB978
		public GameObject CreateWateringCanModel_Local(string ID, bool force = false)
		{
			if (this.wateringCanModel != null && !force)
			{
				return null;
			}
			WateringCanDefinition wateringCanDefinition = Registry.GetItem(ID) as WateringCanDefinition;
			if (wateringCanDefinition == null)
			{
				Console.LogWarning("CreateWateringCanModel_Local: WateringCanDefinition not found", null);
				return null;
			}
			this.wateringCanModel = UnityEngine.Object.Instantiate<GameObject>(wateringCanDefinition.FunctionalWateringCanPrefab, base.transform);
			this.wateringCanModel.transform.position = this.WateringCamPos.position;
			this.wateringCanModel.GetComponent<Rigidbody>().position = this.WateringCamPos.position;
			this.wateringCanModel.transform.rotation = this.WateringCamPos.rotation;
			this.wateringCanModel.GetComponent<Rigidbody>().rotation = this.WateringCamPos.rotation;
			this.wateringCanModel.GetComponent<FunctionalWateringCan>().enabled = false;
			return this.wateringCanModel;
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x000ED854 File Offset: 0x000EBA54
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Property.Utilities.Water.TapAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Property.Utilities.Water.TapAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<PlayerUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 2U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<PlayerUserObject>k__BackingField);
			this.syncVar___<NPCUserObject>k__BackingField = new SyncVar<NetworkObject>(this, 1U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<NPCUserObject>k__BackingField);
			this.syncVar___<IsHeldOpen>k__BackingField = new SyncVar<bool>(this, 0U, WritePermission.ClientUnsynchronized, ReadPermission.Observers, -1f, Channel.Reliable, this.<IsHeldOpen>k__BackingField);
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SetPlayerUser_3323014238));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_SetNPCUser_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SetHeldOpen_1140765316));
			base.RegisterServerRpc(3U, new ServerRpcDelegate(this.RpcReader___Server_SendWateringCanModel_3615296227));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_CreateWateringCanModel_3615296227));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendClearWateringCanModelModel_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_ClearWateringCanModel_2166136261));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Property.Utilities.Water.Tap));
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x000ED9A6 File Offset: 0x000EBBA6
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Property.Utilities.Water.TapAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Property.Utilities.Water.TapAssembly-CSharp.dll_Excuted = true;
			this.syncVar___<PlayerUserObject>k__BackingField.SetRegistered();
			this.syncVar___<NPCUserObject>k__BackingField.SetRegistered();
			this.syncVar___<IsHeldOpen>k__BackingField.SetRegistered();
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x000ED9DA File Offset: 0x000EBBDA
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x000ED9E8 File Offset: 0x000EBBE8
		private void RpcWriter___Server_SetPlayerUser_3323014238(NetworkObject playerObject)
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
			writer.WriteNetworkObject(playerObject);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000EDA90 File Offset: 0x000EBC90
		public void RpcLogic___SetPlayerUser_3323014238(NetworkObject playerObject)
		{
			if (this.PlayerUserObject != null && this.PlayerUserObject.Owner.IsLocalClient && playerObject != null && !playerObject.Owner.IsLocalClient)
			{
				Singleton<GameInput>.Instance.ExitAll();
			}
			this.PlayerUserObject = playerObject;
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x000EDAE4 File Offset: 0x000EBCE4
		private void RpcReader___Server_SetPlayerUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetPlayerUser_3323014238(playerObject);
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000EDB24 File Offset: 0x000EBD24
		private void RpcWriter___Server_SetNPCUser_3323014238(NetworkObject npcObject)
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
			writer.WriteNetworkObject(npcObject);
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x000EDBCB File Offset: 0x000EBDCB
		public void RpcLogic___SetNPCUser_3323014238(NetworkObject npcObject)
		{
			this.NPCUserObject = npcObject;
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x000EDBD4 File Offset: 0x000EBDD4
		private void RpcReader___Server_SetNPCUser_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject npcObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetNPCUser_3323014238(npcObject);
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x000EDC14 File Offset: 0x000EBE14
		private void RpcWriter___Server_SetHeldOpen_1140765316(bool open)
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
			writer.WriteBoolean(open);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x000EDCBB File Offset: 0x000EBEBB
		public void RpcLogic___SetHeldOpen_1140765316(bool open)
		{
			if (open && !this.IsHeldOpen)
			{
				this.SqueakSound.Play();
			}
			this.IsHeldOpen = open;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x000EDCDC File Offset: 0x000EBEDC
		private void RpcReader___Server_SetHeldOpen_1140765316(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			bool open = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetHeldOpen_1140765316(open);
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x000EDD1C File Offset: 0x000EBF1C
		private void RpcWriter___Server_SendWateringCanModel_3615296227(string ID)
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
			writer.WriteString(ID);
			base.SendServerRpc(3U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x000EDDC3 File Offset: 0x000EBFC3
		public void RpcLogic___SendWateringCanModel_3615296227(string ID)
		{
			this.CreateWateringCanModel(ID);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000EDDCC File Offset: 0x000EBFCC
		private void RpcReader___Server_SendWateringCanModel_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendWateringCanModel_3615296227(id);
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x000EDE0C File Offset: 0x000EC00C
		private void RpcWriter___Observers_CreateWateringCanModel_3615296227(string ID)
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
			writer.WriteString(ID);
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x000EDEC2 File Offset: 0x000EC0C2
		private void RpcLogic___CreateWateringCanModel_3615296227(string ID)
		{
			this.wateringCanModel = this.CreateWateringCanModel_Local(ID, false);
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x000EDED4 File Offset: 0x000EC0D4
		private void RpcReader___Observers_CreateWateringCanModel_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___CreateWateringCanModel_3615296227(id);
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x000EDF10 File Offset: 0x000EC110
		private void RpcWriter___Server_SendClearWateringCanModelModel_2166136261()
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
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000EDFAA File Offset: 0x000EC1AA
		public void RpcLogic___SendClearWateringCanModelModel_2166136261()
		{
			this.ClearWateringCanModel();
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x000EDFB4 File Offset: 0x000EC1B4
		private void RpcReader___Server_SendClearWateringCanModelModel_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendClearWateringCanModelModel_2166136261();
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000EDFE4 File Offset: 0x000EC1E4
		private void RpcWriter___Observers_ClearWateringCanModel_2166136261()
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
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000EE08D File Offset: 0x000EC28D
		private void RpcLogic___ClearWateringCanModel_2166136261()
		{
			if (this.wateringCanModel != null)
			{
				UnityEngine.Object.Destroy(this.wateringCanModel);
				this.wateringCanModel = null;
			}
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x000EE0B0 File Offset: 0x000EC2B0
		private void RpcReader___Observers_ClearWateringCanModel_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ClearWateringCanModel_2166136261();
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06003851 RID: 14417 RVA: 0x000EE0DA File Offset: 0x000EC2DA
		// (set) Token: 0x06003852 RID: 14418 RVA: 0x000EE0E2 File Offset: 0x000EC2E2
		public bool SyncAccessor_<IsHeldOpen>k__BackingField
		{
			get
			{
				return this.<IsHeldOpen>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<IsHeldOpen>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<IsHeldOpen>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x000EE120 File Offset: 0x000EC320
		public virtual bool Tap(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<PlayerUserObject>k__BackingField(this.syncVar___<PlayerUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<PlayerUserObject>k__BackingField(value, Boolean2);
				return true;
			}
			else if (UInt321 == 1U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_<NPCUserObject>k__BackingField(this.syncVar___<NPCUserObject>k__BackingField.GetValue(true), true);
					return true;
				}
				NetworkObject value2 = PooledReader0.ReadNetworkObject();
				this.sync___set_value_<NPCUserObject>k__BackingField(value2, Boolean2);
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
					this.sync___set_value_<IsHeldOpen>k__BackingField(this.syncVar___<IsHeldOpen>k__BackingField.GetValue(true), true);
					return true;
				}
				bool value3 = PooledReader0.ReadBoolean();
				this.sync___set_value_<IsHeldOpen>k__BackingField(value3, Boolean2);
				return true;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x000EE1FA File Offset: 0x000EC3FA
		// (set) Token: 0x06003855 RID: 14421 RVA: 0x000EE202 File Offset: 0x000EC402
		public NetworkObject SyncAccessor_<NPCUserObject>k__BackingField
		{
			get
			{
				return this.<NPCUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<NPCUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<NPCUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x000EE23E File Offset: 0x000EC43E
		// (set) Token: 0x06003857 RID: 14423 RVA: 0x000EE246 File Offset: 0x000EC446
		public NetworkObject SyncAccessor_<PlayerUserObject>k__BackingField
		{
			get
			{
				return this.<PlayerUserObject>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<PlayerUserObject>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<PlayerUserObject>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x000EE282 File Offset: 0x000EC482
		private void dll()
		{
			this.IntObj.onHovered.AddListener(new UnityAction(this.Hovered));
			this.IntObj.onInteractStart.AddListener(new UnityAction(this.Interacted));
		}

		// Token: 0x040028E0 RID: 10464
		public const float MaxFlowRate = 6f;

		// Token: 0x040028E2 RID: 10466
		[Header("References")]
		public InteractableObject IntObj;

		// Token: 0x040028E3 RID: 10467
		public Transform CameraPos;

		// Token: 0x040028E4 RID: 10468
		public Transform WateringCamPos;

		// Token: 0x040028E5 RID: 10469
		public Collider HandleCollider;

		// Token: 0x040028E6 RID: 10470
		public Transform Handle;

		// Token: 0x040028E7 RID: 10471
		public Clickable HandleClickable;

		// Token: 0x040028E8 RID: 10472
		public ParticleSystem WaterParticles;

		// Token: 0x040028E9 RID: 10473
		public AudioSourceController SqueakSound;

		// Token: 0x040028EA RID: 10474
		public AudioSourceController WaterRunningSound;

		// Token: 0x040028ED RID: 10477
		private float tapFlow;

		// Token: 0x040028EE RID: 10478
		private GameObject wateringCanModel;

		// Token: 0x040028EF RID: 10479
		private bool intObjSetThisFrame;

		// Token: 0x040028F0 RID: 10480
		public SyncVar<bool> syncVar___<IsHeldOpen>k__BackingField;

		// Token: 0x040028F1 RID: 10481
		public SyncVar<NetworkObject> syncVar___<NPCUserObject>k__BackingField;

		// Token: 0x040028F2 RID: 10482
		public SyncVar<NetworkObject> syncVar___<PlayerUserObject>k__BackingField;

		// Token: 0x040028F3 RID: 10483
		private bool dll_Excuted;

		// Token: 0x040028F4 RID: 10484
		private bool dll_Excuted;
	}
}
