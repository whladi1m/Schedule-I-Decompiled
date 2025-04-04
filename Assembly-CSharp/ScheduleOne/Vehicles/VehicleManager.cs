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
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007C0 RID: 1984
	public class VehicleManager : NetworkSingleton<VehicleManager>, IBaseSaveable, ISaveable
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x0600364F RID: 13903 RVA: 0x000E4C14 File Offset: 0x000E2E14
		public string SaveFolderName
		{
			get
			{
				return "OwnedVehicles";
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06003650 RID: 13904 RVA: 0x000E4C14 File Offset: 0x000E2E14
		public string SaveFileName
		{
			get
			{
				return "OwnedVehicles";
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06003651 RID: 13905 RVA: 0x000E4C1B File Offset: 0x000E2E1B
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06003652 RID: 13906 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06003653 RID: 13907 RVA: 0x000E4C23 File Offset: 0x000E2E23
		// (set) Token: 0x06003654 RID: 13908 RVA: 0x000E4C2B File Offset: 0x000E2E2B
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06003655 RID: 13909 RVA: 0x000E4C34 File Offset: 0x000E2E34
		// (set) Token: 0x06003656 RID: 13910 RVA: 0x000E4C3C File Offset: 0x000E2E3C
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06003657 RID: 13911 RVA: 0x000E4C45 File Offset: 0x000E2E45
		// (set) Token: 0x06003658 RID: 13912 RVA: 0x000E4C4D File Offset: 0x000E2E4D
		public bool HasChanged { get; set; }

		// Token: 0x06003659 RID: 13913 RVA: 0x000E4C56 File Offset: 0x000E2E56
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Vehicles.VehicleManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000E4C6A File Offset: 0x000E2E6A
		[ServerRpc(RequireOwnership = false)]
		public void SpawnVehicle(string vehicleCode, Vector3 position, Quaternion rotation, bool playerOwned)
		{
			this.RpcWriter___Server_SpawnVehicle_3323115898(vehicleCode, position, rotation, playerOwned);
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000E4C84 File Offset: 0x000E2E84
		public LandVehicle SpawnAndReturnVehicle(string vehicleCode, Vector3 position, Quaternion rotation, bool playerOwned)
		{
			LandVehicle vehiclePrefab = this.GetVehiclePrefab(vehicleCode);
			if (vehiclePrefab == null)
			{
				Console.LogError("SpawnVehicle: '" + vehicleCode + "' is not a valid vehicle code!", null);
				return null;
			}
			LandVehicle component = UnityEngine.Object.Instantiate<GameObject>(vehiclePrefab.gameObject).GetComponent<LandVehicle>();
			component.transform.position = position;
			component.transform.rotation = rotation;
			base.NetworkObject.Spawn(component.gameObject, null, default(Scene));
			component.SetIsPlayerOwned(null, playerOwned);
			if (playerOwned)
			{
				this.PlayerOwnedVehicles.Add(component);
			}
			return component;
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000E4D18 File Offset: 0x000E2F18
		public LandVehicle GetVehiclePrefab(string vehicleCode)
		{
			return this.VehiclePrefabs.Find((LandVehicle x) => x.VehicleCode.ToLower() == vehicleCode.ToLower());
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x000E4D49 File Offset: 0x000E2F49
		public LandVehicle SpawnAndLoadVehicle(VehicleData data, string path, bool playerOwned)
		{
			LandVehicle landVehicle = this.SpawnAndReturnVehicle(data.VehicleCode, data.Position, data.Rotation, playerOwned);
			landVehicle.Load(data, path);
			return landVehicle;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000E4D6C File Offset: 0x000E2F6C
		public void LoadVehicle(VehicleData data, string path)
		{
			LandVehicle @object = GUIDManager.GetObject<LandVehicle>(new Guid(data.GUID));
			if (@object == null)
			{
				Console.LogError("LoadVehicle: Vehicle not found with GUID " + data.GUID, null);
				return;
			}
			@object.Load(data, path);
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000E4DB4 File Offset: 0x000E2FB4
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < this.PlayerOwnedVehicles.Count; i++)
			{
				new SaveRequest(this.PlayerOwnedVehicles[i], containerFolder);
				list.Add(this.PlayerOwnedVehicles[i].SaveFolderName);
			}
			return list;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000E4E10 File Offset: 0x000E3010
		public void SpawnLoanSharkVehicle(Vector3 position, Quaternion rot)
		{
			LandVehicle landVehicle = NetworkSingleton<VehicleManager>.Instance.SpawnAndReturnVehicle("shitbox", position, rot, true);
			this.EnableLoanSharkVisuals(landVehicle.NetworkObject);
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000E4E3C File Offset: 0x000E303C
		[ObserversRpc(RunLocally = true)]
		private void EnableLoanSharkVisuals(NetworkObject veh)
		{
			this.RpcWriter___Observers_EnableLoanSharkVisuals_3323014238(veh);
			this.RpcLogic___EnableLoanSharkVisuals_3323014238(veh);
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x000E4EAC File Offset: 0x000E30AC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Vehicles.VehicleManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SpawnVehicle_3323115898));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_EnableLoanSharkVisuals_3323014238));
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000E4EFE File Offset: 0x000E30FE
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Vehicles.VehicleManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000E4F17 File Offset: 0x000E3117
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000E4F28 File Offset: 0x000E3128
		private void RpcWriter___Server_SpawnVehicle_3323115898(string vehicleCode, Vector3 position, Quaternion rotation, bool playerOwned)
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
			writer.WriteString(vehicleCode);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteBoolean(playerOwned);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x000E4FFB File Offset: 0x000E31FB
		public void RpcLogic___SpawnVehicle_3323115898(string vehicleCode, Vector3 position, Quaternion rotation, bool playerOwned)
		{
			this.SpawnAndReturnVehicle(vehicleCode, position, rotation, playerOwned);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x000E500C File Offset: 0x000E320C
		private void RpcReader___Server_SpawnVehicle_3323115898(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string vehicleCode = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			bool playerOwned = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SpawnVehicle_3323115898(vehicleCode, position, rotation, playerOwned);
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x000E5078 File Offset: 0x000E3278
		private void RpcWriter___Observers_EnableLoanSharkVisuals_3323014238(NetworkObject veh)
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
			writer.WriteNetworkObject(veh);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x000E512E File Offset: 0x000E332E
		private void RpcLogic___EnableLoanSharkVisuals_3323014238(NetworkObject veh)
		{
			if (veh == null)
			{
				Console.LogWarning("Vehicle not found", null);
				return;
			}
			veh.GetComponent<LoanSharkCarVisuals>().Configure(true, true);
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x000E5154 File Offset: 0x000E3354
		private void RpcReader___Observers_EnableLoanSharkVisuals_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject veh = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnableLoanSharkVisuals_3323014238(veh);
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x000E518F File Offset: 0x000E338F
		protected virtual void dll()
		{
			base.Awake();
			this.InitializeSaveable();
		}

		// Token: 0x04002729 RID: 10025
		public List<LandVehicle> AllVehicles = new List<LandVehicle>();

		// Token: 0x0400272A RID: 10026
		[Header("Vehicles")]
		public List<LandVehicle> VehiclePrefabs = new List<LandVehicle>();

		// Token: 0x0400272B RID: 10027
		public List<LandVehicle> PlayerOwnedVehicles = new List<LandVehicle>();

		// Token: 0x0400272C RID: 10028
		private VehiclesLoader loader = new VehiclesLoader();

		// Token: 0x04002730 RID: 10032
		private bool dll_Excuted;

		// Token: 0x04002731 RID: 10033
		private bool dll_Excuted;
	}
}
