using System;
using System.Collections.Generic;
using System.IO;
using EasyButtons;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Variables
{
	// Token: 0x02000292 RID: 658
	public class VariableDatabase : NetworkSingleton<VariableDatabase>, IBaseSaveable, ISaveable
	{
		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x0003C805 File Offset: 0x0003AA05
		public string SaveFolderName
		{
			get
			{
				return "Variables";
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0003C805 File Offset: 0x0003AA05
		public string SaveFileName
		{
			get
			{
				return "Variables";
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x0003C80C File Offset: 0x0003AA0C
		public Loader Loader
		{
			get
			{
				return this.loader;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool ShouldSaveUnderFolder
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0003C814 File Offset: 0x0003AA14
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x0003C81C File Offset: 0x0003AA1C
		public List<string> LocalExtraFiles { get; set; } = new List<string>();

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x0003C825 File Offset: 0x0003AA25
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x0003C82D File Offset: 0x0003AA2D
		public List<string> LocalExtraFolders { get; set; } = new List<string>();

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x0003C836 File Offset: 0x0003AA36
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x0003C83E File Offset: 0x0003AA3E
		public bool HasChanged { get; set; }

		// Token: 0x06000D9D RID: 3485 RVA: 0x0003C848 File Offset: 0x0003AA48
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Variables.VariableDatabase_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0003C867 File Offset: 0x0003AA67
		public virtual void InitializeSaveable()
		{
			Singleton<SaveManager>.Instance.RegisterSaveable(this);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0003C874 File Offset: 0x0003AA74
		private void CreateVariables()
		{
			for (int i = 0; i < this.Creators.Length; i++)
			{
				if (this.Creators[i].Mode == EVariableMode.Player)
				{
					this.playerVariables.Add(this.Creators[i].Name.ToLower());
				}
				else
				{
					this.CreateVariable(this.Creators[i].Name, this.Creators[i].Type, this.Creators[i].InitialValue, this.Creators[i].Persistent, EVariableMode.Global, null, EVariableReplicationMode.Networked);
				}
			}
			this.SetVariableValue("IsDemo", false.ToString(), true);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0003C918 File Offset: 0x0003AB18
		public void CreatePlayerVariables(Player owner)
		{
			for (int i = 0; i < this.Creators.Length; i++)
			{
				if (this.Creators[i].Mode == EVariableMode.Player)
				{
					this.CreateVariable(this.Creators[i].Name, this.Creators[i].Type, this.Creators[i].InitialValue, this.Creators[i].Persistent, EVariableMode.Player, owner, EVariableReplicationMode.Local);
				}
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0003C988 File Offset: 0x0003AB88
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsHost)
			{
				return;
			}
			for (int i = 0; i < this.VariableList.Count; i++)
			{
				if (this.VariableList[i].ReplicationMode != EVariableReplicationMode.Local)
				{
					this.VariableList[i].ReplicateValue(connection);
				}
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0003C9E0 File Offset: 0x0003ABE0
		public void CreateVariable(string name, VariableDatabase.EVariableType type, string initialValue, bool persistent, EVariableMode mode, Player owner, EVariableReplicationMode replicationMode = EVariableReplicationMode.Networked)
		{
			if (type == VariableDatabase.EVariableType.Bool)
			{
				new BoolVariable(name, replicationMode, persistent, mode, owner, initialValue == "true");
				return;
			}
			if (type != VariableDatabase.EVariableType.Number)
			{
				return;
			}
			float num;
			float value = float.TryParse(initialValue, out num) ? num : 0f;
			new NumberVariable(name, replicationMode, persistent, mode, owner, value);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0003CA3C File Offset: 0x0003AC3C
		public void AddVariable(BaseVariable variable)
		{
			if (this.VariableDict.ContainsKey(variable.Name))
			{
				Console.LogError("Variable with name " + variable.Name + " already exists in the database.", null);
				return;
			}
			this.VariableList.Add(variable);
			this.VariableDict.Add(variable.Name.ToLower(), variable);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0003CA9B File Offset: 0x0003AC9B
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendValue(NetworkConnection conn, string variableName, string value)
		{
			this.RpcWriter___Server_SendValue_3895153758(conn, variableName, value);
			this.RpcLogic___SendValue_3895153758(conn, variableName, value);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0003CAC1 File Offset: 0x0003ACC1
		[ObserversRpc]
		[TargetRpc]
		public void ReceiveValue(NetworkConnection conn, string variableName, string value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceiveValue_3895153758(conn, variableName, value);
			}
			else
			{
				this.RpcWriter___Target_ReceiveValue_3895153758(conn, variableName, value);
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0003CAF4 File Offset: 0x0003ACF4
		public void SetVariableValue(string variableName, string value, bool network = true)
		{
			variableName = variableName.ToLower();
			if (this.playerVariables.Contains(variableName))
			{
				Player.Local.SetVariableValue(variableName, value, network);
				return;
			}
			if (this.VariableDict.ContainsKey(variableName))
			{
				this.VariableDict[variableName].SetValue(value, network);
				return;
			}
			Console.LogWarning("Failed to find variable with name: " + variableName, null);
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0003CB58 File Offset: 0x0003AD58
		public BaseVariable GetVariable(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.playerVariables.Contains(variableName))
			{
				return Player.Local.GetVariable(variableName);
			}
			if (this.VariableDict.ContainsKey(variableName))
			{
				return this.VariableDict[variableName];
			}
			Console.LogWarning("Failed to find variable with name: " + variableName, null);
			return null;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0003CBB4 File Offset: 0x0003ADB4
		public T GetValue<T>(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.playerVariables.Contains(variableName))
			{
				return Player.Local.GetValue<T>(variableName);
			}
			if (this.VariableDict.ContainsKey(variableName))
			{
				return (T)((object)this.VariableDict[variableName].GetValue());
			}
			Console.LogError("Variable with name " + variableName + " does not exist in the database.", null);
			return default(T);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0003CC28 File Offset: 0x0003AE28
		[Button]
		public void PrintAllVariables()
		{
			for (int i = 0; i < this.VariableList.Count; i++)
			{
				this.PrintVariableValue(this.VariableList[i].Name);
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0003CC64 File Offset: 0x0003AE64
		public void PrintVariableValue(string variableName)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				string str = "Value of ";
				string str2 = variableName;
				string str3 = ": ";
				object value = this.VariableDict[variableName].GetValue();
				Console.Log(str + str2 + str3 + ((value != null) ? value.ToString() : null), null);
				return;
			}
			Console.LogError("Variable with name " + variableName + " does not exist in the database.", null);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0003CCD4 File Offset: 0x0003AED4
		public void NotifyItemAcquired(string id, int quantity)
		{
			if (this.VariableDict.ContainsKey(id + "_acquired"))
			{
				float value = this.GetValue<float>(id + "_acquired");
				this.SetVariableValue(id + "_acquired", (value + (float)quantity).ToString(), true);
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0003CD29 File Offset: 0x0003AF29
		public virtual string GetSaveString()
		{
			return string.Empty;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0003CD30 File Offset: 0x0003AF30
		public virtual List<string> WriteData(string parentFolderPath)
		{
			List<string> list = new List<string>();
			string containerFolder = ((ISaveable)this).GetContainerFolder(parentFolderPath);
			for (int i = 0; i < this.VariableList.Count; i++)
			{
				if (this.VariableList[i] != null && this.VariableList[i].Persistent && this.VariableList[i].VariableMode != EVariableMode.Player)
				{
					string json = new VariableData(this.VariableList[i].Name, this.VariableList[i].GetValue().ToString()).GetJson(true);
					string text = SaveManager.MakeFileSafe(this.VariableList[i].Name) + ".json";
					list.Add(text);
					string text2 = Path.Combine(containerFolder, text);
					try
					{
						File.WriteAllText(text2, json);
					}
					catch (Exception ex)
					{
						Console.LogWarning("Failed to write variable file: " + text2 + " - " + ex.Message, null);
					}
				}
			}
			return list;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0003CE4C File Offset: 0x0003B04C
		public void Load(VariableData data)
		{
			if (this.playerVariables.Contains(data.Name.ToLower()))
			{
				Console.Log("Player variable: " + data.Name + " loaded from database. Redirecting to player.", null);
				Player.Local.SetVariableValue(data.Name, data.Value, false);
				return;
			}
			BaseVariable variable = this.GetVariable(data.Name);
			if (variable == null)
			{
				Console.LogWarning("Failed to find variable with name: " + data.Name, null);
				return;
			}
			variable.SetValue(data.Value, true);
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0003CF30 File Offset: 0x0003B130
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Variables.VariableDatabaseAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Variables.VariableDatabaseAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendValue_3895153758));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveValue_3895153758));
			base.RegisterTargetRpc(2U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveValue_3895153758));
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0003CF99 File Offset: 0x0003B199
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Variables.VariableDatabaseAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Variables.VariableDatabaseAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0003CFB2 File Offset: 0x0003B1B2
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0003CFC0 File Offset: 0x0003B1C0
		private void RpcWriter___Server_SendValue_3895153758(NetworkConnection conn, string variableName, string value)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0003D081 File Offset: 0x0003B281
		public void RpcLogic___SendValue_3895153758(NetworkConnection conn, string variableName, string value)
		{
			this.ReceiveValue(conn, variableName, value);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0003D08C File Offset: 0x0003B28C
		private void RpcReader___Server_SendValue_3895153758(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendValue_3895153758(conn2, variableName, value);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0003D0EC File Offset: 0x0003B2EC
		private void RpcWriter___Observers_ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0003D1AF File Offset: 0x0003B3AF
		public void RpcLogic___ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
		{
			variableName = variableName.ToLower();
			if (this.VariableDict.ContainsKey(variableName))
			{
				this.VariableDict[variableName].SetValue(value, false);
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0003D1DC File Offset: 0x0003B3DC
		private void RpcReader___Observers_ReceiveValue_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveValue_3895153758(null, variableName, value);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0003D220 File Offset: 0x0003B420
		private void RpcWriter___Target_ReceiveValue_3895153758(NetworkConnection conn, string variableName, string value)
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
			writer.WriteString(variableName);
			writer.WriteString(value);
			base.SendTargetRpc(2U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0003D2E4 File Offset: 0x0003B4E4
		private void RpcReader___Target_ReceiveValue_3895153758(PooledReader PooledReader0, Channel channel)
		{
			string variableName = PooledReader0.ReadString();
			string value = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveValue_3895153758(base.LocalConnection, variableName, value);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0003D32C File Offset: 0x0003B52C
		protected virtual void dll()
		{
			base.Awake();
			List<VariableCreator> list = new List<VariableCreator>(this.Creators);
			for (int i = 0; i < this.ItemsToTrackAcquire.Length; i++)
			{
				list.Add(new VariableCreator
				{
					InitialValue = "0",
					Mode = EVariableMode.Global,
					Type = VariableDatabase.EVariableType.Number,
					Persistent = true,
					Name = this.ItemsToTrackAcquire[i].ID + "_acquired"
				});
			}
			this.Creators = list.ToArray();
			this.CreateVariables();
			this.InitializeSaveable();
		}

		// Token: 0x04000E36 RID: 3638
		public List<BaseVariable> VariableList = new List<BaseVariable>();

		// Token: 0x04000E37 RID: 3639
		public Dictionary<string, BaseVariable> VariableDict = new Dictionary<string, BaseVariable>();

		// Token: 0x04000E38 RID: 3640
		private List<string> playerVariables = new List<string>();

		// Token: 0x04000E39 RID: 3641
		public VariableCreator[] Creators;

		// Token: 0x04000E3A RID: 3642
		public StorableItemDefinition[] ItemsToTrackAcquire;

		// Token: 0x04000E3B RID: 3643
		private VariablesLoader loader = new VariablesLoader();

		// Token: 0x04000E3F RID: 3647
		private bool dll_Excuted;

		// Token: 0x04000E40 RID: 3648
		private bool dll_Excuted;

		// Token: 0x02000293 RID: 659
		public enum EVariableType
		{
			// Token: 0x04000E42 RID: 3650
			Bool,
			// Token: 0x04000E43 RID: 3651
			Number
		}
	}
}
