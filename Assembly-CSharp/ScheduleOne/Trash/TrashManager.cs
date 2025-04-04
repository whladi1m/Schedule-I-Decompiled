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
using ScheduleOne.Combat;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Trash
{
	// Token: 0x0200081D RID: 2077
	public class TrashManager : NetworkSingleton<TrashManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x000F12EC File Offset: 0x000EF4EC
		public string SaveFolderName
		{
			get
			{
				return "Trash";
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x0600391C RID: 14620 RVA: 0x000F12EC File Offset: 0x000EF4EC
		public string SaveFileName
		{
			get
			{
				return "Trash";
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x000F12F3 File Offset: 0x000EF4F3
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600391E RID: 14622 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600391F RID: 14623 RVA: 0x000F12FB File Offset: 0x000EF4FB
		// (set) Token: 0x06003920 RID: 14624 RVA: 0x000F1303 File Offset: 0x000EF503
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x000F130C File Offset: 0x000EF50C
		// (set) Token: 0x06003922 RID: 14626 RVA: 0x000F1314 File Offset: 0x000EF514
		public List<string> LocalExtraFolders { get; set; } = new List<string>
		{
			"Generators"
		};

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06003923 RID: 14627 RVA: 0x000F131D File Offset: 0x000EF51D
		// (set) Token: 0x06003924 RID: 14628 RVA: 0x000F1325 File Offset: 0x000EF525
		public bool HasChanged { get; set; }

		// Token: 0x06003925 RID: 14629 RVA: 0x000F132E File Offset: 0x000EF52E
		protected override void Start()
		{
			base.Start();
			this.InitializeSaveable();
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x000F133C File Offset: 0x000EF53C
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			Console.Log("Sending " + this.trashItems.Count.ToString() + " trash items to new player", null);
			foreach (TrashItem trashItem in this.trashItems)
			{
				this.CreateTrashItem(connection, trashItem.ID, trashItem.transform.position, trashItem.transform.rotation, Vector3.zero, null, trashItem.GUID.ToString(), false);
			}
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x000F13F8 File Offset: 0x000EF5F8
		public void ReplicateTransformData(TrashItem trash)
		{
			this.SendTransformData(trash.GUID.ToString(), trash.transform.position, trash.transform.rotation, trash.Rigidbody.velocity, Player.Local.LocalConnection);
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x000F144A File Offset: 0x000EF64A
		[ServerRpc(RequireOwnership = false)]
		private void SendTransformData(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
		{
			this.RpcWriter___Server_SendTransformData_2990100769(guid, position, rotation, velocity, sender);
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x000F1468 File Offset: 0x000EF668
		[ObserversRpc]
		private void ReceiveTransformData(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
		{
			this.RpcWriter___Observers_ReceiveTransformData_2990100769(guid, position, rotation, velocity, sender);
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x000F1490 File Offset: 0x000EF690
		public TrashItem CreateTrashItem(string id, Vector3 posiiton, Quaternion rotation, Vector3 initialVelocity = default(Vector3), string guid = "", bool startKinematic = false)
		{
			if (guid == "")
			{
				guid = Guid.NewGuid().ToString();
			}
			this.SendTrashItem(id, posiiton, rotation, initialVelocity, Player.Local.LocalConnection, guid, false);
			return this.CreateAndReturnTrashItem(id, posiiton, rotation, initialVelocity, guid, startKinematic);
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x000F14E8 File Offset: 0x000EF6E8
		[ServerRpc(RequireOwnership = false)]
		private void SendTrashItem(string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			this.RpcWriter___Server_SendTrashItem_478112418(id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x000F1518 File Offset: 0x000EF718
		[ObserversRpc]
		[TargetRpc]
		private void CreateTrashItem(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateTrashItem_2385526393(conn, id, position, rotation, initialVelocity, sender, guid, startKinematic);
			}
			else
			{
				this.RpcWriter___Target_CreateTrashItem_2385526393(conn, id, position, rotation, initialVelocity, sender, guid, startKinematic);
			}
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x000F157C File Offset: 0x000EF77C
		private TrashItem CreateAndReturnTrashItem(string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, string guid, bool startKinematic)
		{
			TrashItem trashPrefab = this.GetTrashPrefab(id);
			if (trashPrefab == null)
			{
				Debug.LogError("Trash item with ID " + id + " not found.");
				return null;
			}
			if (GUIDManager.IsGUIDAlreadyRegistered(new Guid(guid)))
			{
				return null;
			}
			trashPrefab.Draggable.CreateCoM = false;
			trashPrefab.GetComponent<PhysicsDamageable>().ForceMultiplier = this.TrashForceMultiplier;
			TrashItem trashItem = UnityEngine.Object.Instantiate<TrashItem>(trashPrefab, position, rotation, NetworkSingleton<GameManager>.Instance.Temp);
			trashItem.SetGUID(new Guid(guid));
			if (!startKinematic)
			{
				trashItem.SetContinuousCollisionDetection();
			}
			if (initialVelocity != default(Vector3))
			{
				trashItem.SetVelocity(initialVelocity);
			}
			this.trashItems.Add(trashItem);
			this.HasChanged = true;
			return trashItem;
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x000F1638 File Offset: 0x000EF838
		public TrashItem CreateTrashBag(string id, Vector3 posiiton, Quaternion rotation, TrashContentData content, Vector3 initialVelocity = default(Vector3), string guid = "", bool startKinematic = false)
		{
			if (guid == "")
			{
				guid = Guid.NewGuid().ToString();
			}
			this.SendTrashBag(id, posiiton, rotation, content, initialVelocity, Player.Local.LocalConnection, guid, false);
			return this.CreateAndReturnTrashBag(id, posiiton, rotation, content, initialVelocity, guid, startKinematic);
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x000F1694 File Offset: 0x000EF894
		[ServerRpc(RequireOwnership = false)]
		private void SendTrashBag(string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			this.RpcWriter___Server_SendTrashBag_3965031115(id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x000F16C8 File Offset: 0x000EF8C8
		[ObserversRpc]
		[TargetRpc]
		private void CreateTrashBag(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_CreateTrashBag_680856992(conn, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
			}
			else
			{
				this.RpcWriter___Target_CreateTrashBag_680856992(conn, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
			}
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x000F1734 File Offset: 0x000EF934
		private TrashItem CreateAndReturnTrashBag(string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, string guid, bool startKinematic)
		{
			TrashBag trashBag = this.GetTrashPrefab(id) as TrashBag;
			if (trashBag == null)
			{
				Debug.LogError("Trash item with ID " + id + " not found.");
				return null;
			}
			TrashBag trashBag2 = UnityEngine.Object.Instantiate<TrashBag>(trashBag, position, rotation, NetworkSingleton<GameManager>.Instance.Temp);
			trashBag2.SetGUID(new Guid(guid));
			trashBag2.LoadContent(content);
			if (!startKinematic)
			{
				trashBag2.SetContinuousCollisionDetection();
			}
			if (initialVelocity != default(Vector3))
			{
				trashBag2.SetVelocity(initialVelocity);
			}
			this.trashItems.Add(trashBag2);
			this.HasChanged = true;
			return trashBag2;
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x000F17D0 File Offset: 0x000EF9D0
		public void DestroyAllTrash()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			List<TrashItem> list = new List<TrashItem>();
			list.AddRange(this.trashItems);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].DestroyTrash();
			}
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x000F1814 File Offset: 0x000EFA14
		public void DestroyTrash(TrashItem trash)
		{
			this.SendDestroyTrash(trash.GUID.ToString());
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x000F183B File Offset: 0x000EFA3B
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void SendDestroyTrash(string guid)
		{
			this.RpcWriter___Server_SendDestroyTrash_3615296227(guid);
			this.RpcLogic___SendDestroyTrash_3615296227(guid);
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x000F1854 File Offset: 0x000EFA54
		[ObserversRpc(RunLocally = true)]
		private void DestroyTrash(string guid)
		{
			this.RpcWriter___Observers_DestroyTrash_3615296227(guid);
			this.RpcLogic___DestroyTrash_3615296227(guid);
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x000F1878 File Offset: 0x000EFA78
		public TrashItem GetTrashPrefab(string id)
		{
			return this.TrashPrefabs.FirstOrDefault((TrashItem t) => t.ID == id);
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x000F18AC File Offset: 0x000EFAAC
		public TrashItem GetRandomGeneratableTrashPrefab()
		{
			float maxInclusive = this.GenerateableTrashItems.Sum((TrashManager.TrashItemData t) => t.GenerationChance);
			float num = UnityEngine.Random.Range(0f, maxInclusive);
			foreach (TrashManager.TrashItemData trashItemData in this.GenerateableTrashItems)
			{
				if (num < trashItemData.GenerationChance)
				{
					return trashItemData.Item;
				}
				num -= trashItemData.GenerationChance;
			}
			return this.GenerateableTrashItems[this.GenerateableTrashItems.Length - 1].Item;
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x000F193C File Offset: 0x000EFB3C
		public virtual string GetSaveString()
		{
			List<ScheduleOne.Persistence.Datas.TrashItemData> list = new List<ScheduleOne.Persistence.Datas.TrashItemData>();
			int num = 0;
			while (num < this.trashItems.Count && num < 2000)
			{
				list.Add(this.trashItems[num].GetData());
				num++;
			}
			return new TrashData(list.ToArray()).GetJson(true);
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x000F1998 File Offset: 0x000EFB98
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> result = new List<string>();
			string parentFolderPath2 = ((ISaveable)this).WriteFolder(parentFolderPath, "Generators");
			foreach (TrashGenerator trashGenerator in TrashGenerator.AllGenerators)
			{
				if (trashGenerator.ShouldSave() && trashGenerator.HasChanged)
				{
					new SaveRequest(trashGenerator, parentFolderPath2);
				}
			}
			return result;
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x000F1A70 File Offset: 0x000EFC70
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Trash.TrashManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Trash.TrashManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendTransformData_2990100769));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveTransformData_2990100769));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendTrashItem_478112418));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_CreateTrashItem_2385526393));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_CreateTrashItem_2385526393));
			base.RegisterServerRpc(5U, new ServerRpcDelegate(this.RpcReader___Server_SendTrashBag_3965031115));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_CreateTrashBag_680856992));
			base.RegisterTargetRpc(7U, new ClientRpcDelegate(this.RpcReader___Target_CreateTrashBag_680856992));
			base.RegisterServerRpc(8U, new ServerRpcDelegate(this.RpcReader___Server_SendDestroyTrash_3615296227));
			base.RegisterObserversRpc(9U, new ClientRpcDelegate(this.RpcReader___Observers_DestroyTrash_3615296227));
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x000F1B7A File Offset: 0x000EFD7A
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Trash.TrashManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Trash.TrashManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x000F1B93 File Offset: 0x000EFD93
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x000F1BA4 File Offset: 0x000EFDA4
		private void RpcWriter___Server_SendTransformData_2990100769(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteVector3(velocity);
			writer.WriteNetworkConnection(sender);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x000F1C84 File Offset: 0x000EFE84
		private void RpcLogic___SendTransformData_2990100769(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
		{
			this.ReceiveTransformData(guid, position, rotation, velocity, sender);
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x000F1C94 File Offset: 0x000EFE94
		private void RpcReader___Server_SendTransformData_2990100769(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			Vector3 velocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendTransformData_2990100769(guid, position, rotation, velocity, sender);
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x000F1D10 File Offset: 0x000EFF10
		private void RpcWriter___Observers_ReceiveTransformData_2990100769(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
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
			writer.WriteString(guid);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteVector3(velocity);
			writer.WriteNetworkConnection(sender);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x000F1E00 File Offset: 0x000F0000
		private void RpcLogic___ReceiveTransformData_2990100769(string guid, Vector3 position, Quaternion rotation, Vector3 velocity, NetworkConnection sender)
		{
			if (sender.IsLocalClient)
			{
				return;
			}
			TrashItem @object = GUIDManager.GetObject<TrashItem>(new Guid(guid));
			if (@object == null)
			{
				return;
			}
			@object.transform.position = position;
			@object.transform.rotation = rotation;
			@object.Rigidbody.velocity = velocity;
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x000F1E54 File Offset: 0x000F0054
		private void RpcReader___Observers_ReceiveTransformData_2990100769(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			Vector3 velocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveTransformData_2990100769(guid, position, rotation, velocity, sender);
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x000F1ED0 File Offset: 0x000F00D0
		private void RpcWriter___Server_SendTrashItem_478112418(string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x000F1FCC File Offset: 0x000F01CC
		private void RpcLogic___SendTrashItem_478112418(string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (this.trashItems.Count >= 2000)
			{
				this.trashItems[UnityEngine.Random.Range(0, this.trashItems.Count)].DestroyTrash();
			}
			this.CreateTrashItem(null, id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x000F2020 File Offset: 0x000F0220
		private void RpcReader___Server_SendTrashItem_478112418(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendTrashItem_478112418(id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x000F20BC File Offset: 0x000F02BC
		private void RpcWriter___Observers_CreateTrashItem_2385526393(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x000F21C5 File Offset: 0x000F03C5
		private void RpcLogic___CreateTrashItem_2385526393(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (sender.IsLocalClient)
			{
				return;
			}
			this.CreateAndReturnTrashItem(id, position, rotation, initialVelocity, guid, startKinematic);
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x000F21E4 File Offset: 0x000F03E4
		private void RpcReader___Observers_CreateTrashItem_2385526393(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateTrashItem_2385526393(null, id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x000F2284 File Offset: 0x000F0484
		private void RpcWriter___Target_CreateTrashItem_2385526393(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendTargetRpc(4U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x000F238C File Offset: 0x000F058C
		private void RpcReader___Target_CreateTrashItem_2385526393(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateTrashItem_2385526393(base.LocalConnection, id, position, rotation, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x0600394D RID: 14669 RVA: 0x000F2430 File Offset: 0x000F0630
		private void RpcWriter___Server_SendTrashBag_3965031115(string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated(content);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendServerRpc(5U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600394E RID: 14670 RVA: 0x000F2538 File Offset: 0x000F0738
		private void RpcLogic___SendTrashBag_3965031115(string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (this.trashItems.Count >= 2000)
			{
				this.trashItems[UnityEngine.Random.Range(0, this.trashItems.Count)].DestroyTrash();
			}
			this.CreateTrashBag(null, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x0600394F RID: 14671 RVA: 0x000F258C File Offset: 0x000F078C
		private void RpcReader___Server_SendTrashBag_3965031115(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			TrashContentData content = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds(PooledReader0);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendTrashBag_3965031115(id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003950 RID: 14672 RVA: 0x000F263C File Offset: 0x000F083C
		private void RpcWriter___Observers_CreateTrashBag_680856992(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated(content);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003951 RID: 14673 RVA: 0x000F2752 File Offset: 0x000F0952
		private void RpcLogic___CreateTrashBag_680856992(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
		{
			if (sender.IsLocalClient)
			{
				return;
			}
			this.CreateAndReturnTrashBag(id, position, rotation, content, initialVelocity, guid, startKinematic);
		}

		// Token: 0x06003952 RID: 14674 RVA: 0x000F2774 File Offset: 0x000F0974
		private void RpcReader___Observers_CreateTrashBag_680856992(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			TrashContentData content = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds(PooledReader0);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateTrashBag_680856992(null, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003953 RID: 14675 RVA: 0x000F2824 File Offset: 0x000F0A24
		private void RpcWriter___Target_CreateTrashBag_680856992(NetworkConnection conn, string id, Vector3 position, Quaternion rotation, TrashContentData content, Vector3 initialVelocity, NetworkConnection sender, string guid, bool startKinematic = false)
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
			writer.WriteString(id);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.Write___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generated(content);
			writer.WriteVector3(initialVelocity);
			writer.WriteNetworkConnection(sender);
			writer.WriteString(guid);
			writer.WriteBoolean(startKinematic);
			base.SendTargetRpc(7U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x000F293C File Offset: 0x000F0B3C
		private void RpcReader___Target_CreateTrashBag_680856992(PooledReader PooledReader0, Channel channel)
		{
			string id = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			TrashContentData content = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.TrashContentDataFishNet.Serializing.Generateds(PooledReader0);
			Vector3 initialVelocity = PooledReader0.ReadVector3();
			NetworkConnection sender = PooledReader0.ReadNetworkConnection();
			string guid = PooledReader0.ReadString();
			bool startKinematic = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___CreateTrashBag_680856992(base.LocalConnection, id, position, rotation, content, initialVelocity, sender, guid, startKinematic);
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x000F29F0 File Offset: 0x000F0BF0
		private void RpcWriter___Server_SendDestroyTrash_3615296227(string guid)
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
			writer.WriteString(guid);
			base.SendServerRpc(8U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x000F2A97 File Offset: 0x000F0C97
		private void RpcLogic___SendDestroyTrash_3615296227(string guid)
		{
			this.DestroyTrash(guid);
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x000F2AA0 File Offset: 0x000F0CA0
		private void RpcReader___Server_SendDestroyTrash_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string guid = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendDestroyTrash_3615296227(guid);
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x000F2AE0 File Offset: 0x000F0CE0
		private void RpcWriter___Observers_DestroyTrash_3615296227(string guid)
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
			writer.WriteString(guid);
			base.SendObserversRpc(9U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x000F2B98 File Offset: 0x000F0D98
		private void RpcLogic___DestroyTrash_3615296227(string guid)
		{
			TrashItem @object = GUIDManager.GetObject<TrashItem>(new Guid(guid));
			if (@object == null)
			{
				return;
			}
			this.trashItems.Remove(@object);
			GUIDManager.DeregisterObject(@object);
			if (@object.onDestroyed != null)
			{
				@object.onDestroyed(@object);
			}
			@object.Deinitialize();
			UnityEngine.Object.Destroy(@object.gameObject);
			this.HasChanged = true;
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x000F2BFC File Offset: 0x000F0DFC
		private void RpcReader___Observers_DestroyTrash_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string guid = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___DestroyTrash_3615296227(guid);
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x000F2C37 File Offset: 0x000F0E37
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x04002956 RID: 10582
		public const int TRASH_ITEM_LIMIT = 2000;

		// Token: 0x04002957 RID: 10583
		public TrashItem[] TrashPrefabs;

		// Token: 0x04002958 RID: 10584
		public TrashItem TrashBagPrefab;

		// Token: 0x04002959 RID: 10585
		public TrashManager.TrashItemData[] GenerateableTrashItems;

		// Token: 0x0400295A RID: 10586
		private List<TrashItem> trashItems = new List<TrashItem>();

		// Token: 0x0400295B RID: 10587
		public float TrashForceMultiplier = 0.3f;

		// Token: 0x0400295C RID: 10588
		private TrashLoader loader = new TrashLoader();

		// Token: 0x04002960 RID: 10592
		private List<string> writtenItemFiles = new List<string>();

		// Token: 0x04002961 RID: 10593
		private bool dll_Excuted;

		// Token: 0x04002962 RID: 10594
		private bool dll_Excuted;

		// Token: 0x0200081E RID: 2078
		[Serializable]
		public class TrashItemData
		{
			// Token: 0x04002963 RID: 10595
			public TrashItem Item;

			// Token: 0x04002964 RID: 10596
			[Range(0f, 1f)]
			public float GenerationChance = 0.5f;
		}
	}
}
