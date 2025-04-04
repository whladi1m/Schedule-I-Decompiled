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
using FluffyUnderware.DevTools.Extensions;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Casino
{
	// Token: 0x0200074A RID: 1866
	public class CasinoGamePlayers : NetworkBehaviour
	{
		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000D348D File Offset: 0x000D168D
		public int CurrentPlayerCount
		{
			get
			{
				return this.Players.Count((Player p) => p != null);
			}
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x000D34B9 File Offset: 0x000D16B9
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.CasinoGamePlayers_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x000D34D0 File Offset: 0x000D16D0
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.CurrentPlayerCount > 0)
			{
				this.SetPlayerList(connection, this.GetPlayerObjects());
				foreach (Player player in this.Players)
				{
					if (!(player == null) && this.playerScores[player] != 0)
					{
						this.SetPlayerScore(connection, player.NetworkObject, this.playerScores[player]);
					}
				}
			}
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x000D3543 File Offset: 0x000D1743
		public void AddPlayer(Player player)
		{
			this.RequestAddPlayer(player.NetworkObject);
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x000D3551 File Offset: 0x000D1751
		public void RemovePlayer(Player player)
		{
			this.RequestRemovePlayer(player.NetworkObject);
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x000D355F File Offset: 0x000D175F
		public void SetPlayerScore(Player player, int score)
		{
			this.RequestSetScore(player.NetworkObject, score);
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000D356E File Offset: 0x000D176E
		public int GetPlayerScore(Player player)
		{
			if (player == null)
			{
				return 0;
			}
			if (this.playerScores.ContainsKey(player))
			{
				return this.playerScores[player];
			}
			return 0;
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000D3597 File Offset: 0x000D1797
		public Player GetPlayer(int index)
		{
			if (index < this.Players.Length)
			{
				return this.Players[index];
			}
			return null;
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x000D35AE File Offset: 0x000D17AE
		public int GetPlayerIndex(Player player)
		{
			return this.Players.IndexOf(player);
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x000D35BC File Offset: 0x000D17BC
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void RequestAddPlayer(NetworkObject playerObject)
		{
			this.RpcWriter___Server_RequestAddPlayer_3323014238(playerObject);
			this.RpcLogic___RequestAddPlayer_3323014238(playerObject);
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x000D35E0 File Offset: 0x000D17E0
		private void AddPlayerToArray(Player player)
		{
			for (int i = 0; i < this.PlayerLimit; i++)
			{
				if (this.Players[i] == null)
				{
					this.Players[i] = player;
					return;
				}
			}
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x000D3618 File Offset: 0x000D1818
		[ServerRpc(RequireOwnership = false)]
		private void RequestRemovePlayer(NetworkObject playerObject)
		{
			this.RpcWriter___Server_RequestRemovePlayer_3323014238(playerObject);
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x000D3630 File Offset: 0x000D1830
		private void RemovePlayerFromArray(Player player)
		{
			for (int i = 0; i < this.PlayerLimit; i++)
			{
				if (this.Players[i] == player)
				{
					this.Players[i] = null;
					return;
				}
			}
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x000D3668 File Offset: 0x000D1868
		[ServerRpc(RequireOwnership = false)]
		private void RequestSetScore(NetworkObject playerObject, int score)
		{
			this.RpcWriter___Server_RequestSetScore_4172557123(playerObject, score);
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x000D3678 File Offset: 0x000D1878
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetPlayerScore(NetworkConnection conn, NetworkObject playerObject, int score)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetPlayerScore_1865307316(conn, playerObject, score);
				this.RpcLogic___SetPlayerScore_1865307316(conn, playerObject, score);
			}
			else
			{
				this.RpcWriter___Target_SetPlayerScore_1865307316(conn, playerObject, score);
			}
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x000D36C8 File Offset: 0x000D18C8
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void SetPlayerList(NetworkConnection conn, NetworkObject[] playerObjects)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetPlayerList_204172449(conn, playerObjects);
				this.RpcLogic___SetPlayerList_204172449(conn, playerObjects);
			}
			else
			{
				this.RpcWriter___Target_SetPlayerList_204172449(conn, playerObjects);
			}
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x000D3709 File Offset: 0x000D1909
		public CasinoGamePlayerData GetPlayerData()
		{
			return this.GetPlayerData(Player.Local);
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x000D3716 File Offset: 0x000D1916
		public CasinoGamePlayerData GetPlayerData(Player player)
		{
			if (!this.playerDatas.ContainsKey(player))
			{
				this.playerDatas.Add(player, new CasinoGamePlayerData(this, player));
			}
			return this.playerDatas[player];
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x000D3745 File Offset: 0x000D1945
		public CasinoGamePlayerData GetPlayerData(int index)
		{
			if (index < this.Players.Length && this.Players[index] != null)
			{
				return this.GetPlayerData(this.Players[index]);
			}
			return null;
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x000D3772 File Offset: 0x000D1972
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPlayerBool(NetworkObject playerObject, string key, bool value)
		{
			this.RpcWriter___Server_SendPlayerBool_77262511(playerObject, key, value);
			this.RpcLogic___SendPlayerBool_77262511(playerObject, key, value);
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x000D3798 File Offset: 0x000D1998
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceivePlayerBool(NetworkConnection conn, NetworkObject playerObject, string key, bool value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceivePlayerBool_1748594478(conn, playerObject, key, value);
				this.RpcLogic___ReceivePlayerBool_1748594478(conn, playerObject, key, value);
			}
			else
			{
				this.RpcWriter___Target_ReceivePlayerBool_1748594478(conn, playerObject, key, value);
			}
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x000D37F1 File Offset: 0x000D19F1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPlayerFloat(NetworkObject playerObject, string key, float value)
		{
			this.RpcWriter___Server_SendPlayerFloat_2931762093(playerObject, key, value);
			this.RpcLogic___SendPlayerFloat_2931762093(playerObject, key, value);
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x000D3818 File Offset: 0x000D1A18
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void ReceivePlayerFloat(NetworkConnection conn, NetworkObject playerObject, string key, float value)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_ReceivePlayerFloat_2317689966(conn, playerObject, key, value);
				this.RpcLogic___ReceivePlayerFloat_2317689966(conn, playerObject, key, value);
			}
			else
			{
				this.RpcWriter___Target_ReceivePlayerFloat_2317689966(conn, playerObject, key, value);
			}
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x000D3874 File Offset: 0x000D1A74
		private NetworkObject[] GetPlayerObjects()
		{
			NetworkObject[] array = new NetworkObject[this.PlayerLimit];
			for (int i = 0; i < this.PlayerLimit; i++)
			{
				if (this.Players[i] != null)
				{
					array[i] = this.Players[i].NetworkObject;
				}
			}
			return array;
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x000D38E4 File Offset: 0x000D1AE4
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.CasinoGamePlayersAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.CasinoGamePlayersAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_RequestAddPlayer_3323014238));
			base.RegisterServerRpc(1U, new ServerRpcDelegate(this.RpcReader___Server_RequestRemovePlayer_3323014238));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_RequestSetScore_4172557123));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetPlayerScore_1865307316));
			base.RegisterTargetRpc(4U, new ClientRpcDelegate(this.RpcReader___Target_SetPlayerScore_1865307316));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetPlayerList_204172449));
			base.RegisterTargetRpc(6U, new ClientRpcDelegate(this.RpcReader___Target_SetPlayerList_204172449));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_SendPlayerBool_77262511));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerBool_1748594478));
			base.RegisterTargetRpc(9U, new ClientRpcDelegate(this.RpcReader___Target_ReceivePlayerBool_1748594478));
			base.RegisterServerRpc(10U, new ServerRpcDelegate(this.RpcReader___Server_SendPlayerFloat_2931762093));
			base.RegisterObserversRpc(11U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerFloat_2317689966));
			base.RegisterTargetRpc(12U, new ClientRpcDelegate(this.RpcReader___Target_ReceivePlayerFloat_2317689966));
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x000D3A2D File Offset: 0x000D1C2D
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.CasinoGamePlayersAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.CasinoGamePlayersAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x000D3A40 File Offset: 0x000D1C40
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x000D3A50 File Offset: 0x000D1C50
		private void RpcWriter___Server_RequestAddPlayer_3323014238(NetworkObject playerObject)
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

		// Token: 0x060032C5 RID: 12997 RVA: 0x000D3AF8 File Offset: 0x000D1CF8
		private void RpcLogic___RequestAddPlayer_3323014238(NetworkObject playerObject)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component != null && !this.Players.Contains(component))
			{
				this.AddPlayerToArray(component);
				if (!this.playerScores.ContainsKey(component))
				{
					this.playerScores.Add(component, 0);
				}
				if (!this.playerDatas.ContainsKey(component))
				{
					this.playerDatas.Add(component, new CasinoGamePlayerData(this, component));
				}
			}
			this.SetPlayerList(null, this.GetPlayerObjects());
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x000D3B74 File Offset: 0x000D1D74
		private void RpcReader___Server_RequestAddPlayer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
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
			this.RpcLogic___RequestAddPlayer_3323014238(playerObject);
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x000D3BB4 File Offset: 0x000D1DB4
		private void RpcWriter___Server_RequestRemovePlayer_3323014238(NetworkObject playerObject)
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
			base.SendServerRpc(1U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x000D3C5C File Offset: 0x000D1E5C
		private void RpcLogic___RequestRemovePlayer_3323014238(NetworkObject playerObject)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component != null && this.Players.Contains(component))
			{
				this.RemovePlayerFromArray(component);
			}
			this.SetPlayerList(null, this.GetPlayerObjects());
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x000D3C9C File Offset: 0x000D1E9C
		private void RpcReader___Server_RequestRemovePlayer_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___RequestRemovePlayer_3323014238(playerObject);
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x000D3CD0 File Offset: 0x000D1ED0
		private void RpcWriter___Server_RequestSetScore_4172557123(NetworkObject playerObject, int score)
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
			writer.WriteInt32(score, AutoPackType.Packed);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x000D3D89 File Offset: 0x000D1F89
		private void RpcLogic___RequestSetScore_4172557123(NetworkObject playerObject, int score)
		{
			this.SetPlayerScore(null, playerObject, score);
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x000D3D94 File Offset: 0x000D1F94
		private void RpcReader___Server_RequestSetScore_4172557123(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			int score = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___RequestSetScore_4172557123(playerObject, score);
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x000D3DDC File Offset: 0x000D1FDC
		private void RpcWriter___Observers_SetPlayerScore_1865307316(NetworkConnection conn, NetworkObject playerObject, int score)
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
			writer.WriteNetworkObject(playerObject);
			writer.WriteInt32(score, AutoPackType.Packed);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x000D3EA4 File Offset: 0x000D20A4
		private void RpcLogic___SetPlayerScore_1865307316(NetworkConnection conn, NetworkObject playerObject, int score)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (!this.playerScores.ContainsKey(component))
			{
				this.playerScores.Add(component, score);
			}
			else
			{
				this.playerScores[component] = score;
			}
			if (this.onPlayerScoresChanged != null)
			{
				this.onPlayerScoresChanged.Invoke();
			}
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x000D3F00 File Offset: 0x000D2100
		private void RpcReader___Observers_SetPlayerScore_1865307316(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			int score = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPlayerScore_1865307316(null, playerObject, score);
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x000D3F54 File Offset: 0x000D2154
		private void RpcWriter___Target_SetPlayerScore_1865307316(NetworkConnection conn, NetworkObject playerObject, int score)
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
			writer.WriteNetworkObject(playerObject);
			writer.WriteInt32(score, AutoPackType.Packed);
			base.SendTargetRpc(4U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x000D401C File Offset: 0x000D221C
		private void RpcReader___Target_SetPlayerScore_1865307316(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			int score = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetPlayerScore_1865307316(base.LocalConnection, playerObject, score);
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x000D406C File Offset: 0x000D226C
		private void RpcWriter___Observers_SetPlayerList_204172449(NetworkConnection conn, NetworkObject[] playerObjects)
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
			writer.Write___FishNet.Object.NetworkObject[]FishNet.Serializing.Generated(playerObjects);
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000D4124 File Offset: 0x000D2324
		private void RpcLogic___SetPlayerList_204172449(NetworkConnection conn, NetworkObject[] playerObjects)
		{
			this.Players = new Player[this.PlayerLimit];
			for (int i = 0; i < this.PlayerLimit; i++)
			{
				this.Players[i] = null;
			}
			for (int j = 0; j < playerObjects.Length; j++)
			{
				if (!(playerObjects[j] == null))
				{
					Player component = playerObjects[j].GetComponent<Player>();
					if (component != null)
					{
						this.Players[j] = component;
						if (!this.playerScores.ContainsKey(component))
						{
							this.playerScores.Add(component, 0);
						}
						if (!this.playerDatas.ContainsKey(component))
						{
							this.playerDatas.Add(component, new CasinoGamePlayerData(this, component));
						}
					}
				}
			}
			if (this.onPlayerListChanged != null)
			{
				this.onPlayerListChanged.Invoke();
			}
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000D41E0 File Offset: 0x000D23E0
		private void RpcReader___Observers_SetPlayerList_204172449(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject[] playerObjects = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___FishNet.Object.NetworkObject[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetPlayerList_204172449(null, playerObjects);
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x000D421C File Offset: 0x000D241C
		private void RpcWriter___Target_SetPlayerList_204172449(NetworkConnection conn, NetworkObject[] playerObjects)
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
			writer.Write___FishNet.Object.NetworkObject[]FishNet.Serializing.Generated(playerObjects);
			base.SendTargetRpc(6U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000D42D4 File Offset: 0x000D24D4
		private void RpcReader___Target_SetPlayerList_204172449(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject[] playerObjects = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___FishNet.Object.NetworkObject[]FishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetPlayerList_204172449(base.LocalConnection, playerObjects);
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000D430C File Offset: 0x000D250C
		private void RpcWriter___Server_SendPlayerBool_77262511(NetworkObject playerObject, string key, bool value)
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
			writer.WriteString(key);
			writer.WriteBoolean(value);
			base.SendServerRpc(7U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000D43CD File Offset: 0x000D25CD
		public void RpcLogic___SendPlayerBool_77262511(NetworkObject playerObject, string key, bool value)
		{
			this.ReceivePlayerBool(null, playerObject, key, value);
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x000D43DC File Offset: 0x000D25DC
		private void RpcReader___Server_SendPlayerBool_77262511(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlayerBool_77262511(playerObject, key, value);
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x000D443C File Offset: 0x000D263C
		private void RpcWriter___Observers_ReceivePlayerBool_1748594478(NetworkConnection conn, NetworkObject playerObject, string key, bool value)
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
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteBoolean(value);
			base.SendObserversRpc(8U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x000D450C File Offset: 0x000D270C
		private void RpcLogic___ReceivePlayerBool_1748594478(NetworkConnection conn, NetworkObject playerObject, string key, bool value)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (!this.playerDatas.ContainsKey(component))
			{
				this.playerDatas.Add(component, new CasinoGamePlayerData(this, component));
			}
			this.playerDatas[component].SetData<bool>(key, value, false);
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x000D4560 File Offset: 0x000D2760
		private void RpcReader___Observers_ReceivePlayerBool_1748594478(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerBool_1748594478(null, playerObject, key, value);
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x000D45C0 File Offset: 0x000D27C0
		private void RpcWriter___Target_ReceivePlayerBool_1748594478(NetworkConnection conn, NetworkObject playerObject, string key, bool value)
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
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteBoolean(value);
			base.SendTargetRpc(9U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x000D4690 File Offset: 0x000D2890
		private void RpcReader___Target_ReceivePlayerBool_1748594478(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			bool value = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerBool_1748594478(base.LocalConnection, playerObject, key, value);
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x000D46EC File Offset: 0x000D28EC
		private void RpcWriter___Server_SendPlayerFloat_2931762093(NetworkObject playerObject, string key, float value)
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
			writer.WriteString(key);
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendServerRpc(10U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x000D47B2 File Offset: 0x000D29B2
		public void RpcLogic___SendPlayerFloat_2931762093(NetworkObject playerObject, string key, float value)
		{
			this.ReceivePlayerFloat(null, playerObject, key, value);
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000D47C0 File Offset: 0x000D29C0
		private void RpcReader___Server_SendPlayerFloat_2931762093(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlayerFloat_2931762093(playerObject, key, value);
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x000D4828 File Offset: 0x000D2A28
		private void RpcWriter___Observers_ReceivePlayerFloat_2317689966(NetworkConnection conn, NetworkObject playerObject, string key, float value)
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
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendObserversRpc(11U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x000D4900 File Offset: 0x000D2B00
		private void RpcLogic___ReceivePlayerFloat_2317689966(NetworkConnection conn, NetworkObject playerObject, string key, float value)
		{
			Player component = playerObject.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (!this.playerDatas.ContainsKey(component))
			{
				this.playerDatas.Add(component, new CasinoGamePlayerData(this, component));
			}
			this.playerDatas[component].SetData<float>(key, value, false);
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x000D4954 File Offset: 0x000D2B54
		private void RpcReader___Observers_ReceivePlayerFloat_2317689966(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerFloat_2317689966(null, playerObject, key, value);
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x000D49B8 File Offset: 0x000D2BB8
		private void RpcWriter___Target_ReceivePlayerFloat_2317689966(NetworkConnection conn, NetworkObject playerObject, string key, float value)
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
			writer.WriteNetworkObject(playerObject);
			writer.WriteString(key);
			writer.WriteSingle(value, AutoPackType.Unpacked);
			base.SendTargetRpc(12U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x000D4A8C File Offset: 0x000D2C8C
		private void RpcReader___Target_ReceivePlayerFloat_2317689966(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject playerObject = PooledReader0.ReadNetworkObject();
			string key = PooledReader0.ReadString();
			float value = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerFloat_2317689966(base.LocalConnection, playerObject, key, value);
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x000D4AEA File Offset: 0x000D2CEA
		private void dll()
		{
			this.Players = new Player[this.PlayerLimit];
		}

		// Token: 0x04002452 RID: 9298
		public int PlayerLimit = 4;

		// Token: 0x04002453 RID: 9299
		private Player[] Players;

		// Token: 0x04002454 RID: 9300
		public UnityEvent onPlayerListChanged;

		// Token: 0x04002455 RID: 9301
		public UnityEvent onPlayerScoresChanged;

		// Token: 0x04002456 RID: 9302
		private Dictionary<Player, int> playerScores = new Dictionary<Player, int>();

		// Token: 0x04002457 RID: 9303
		private Dictionary<Player, CasinoGamePlayerData> playerDatas = new Dictionary<Player, CasinoGamePlayerData>();

		// Token: 0x04002458 RID: 9304
		private bool dll_Excuted;

		// Token: 0x04002459 RID: 9305
		private bool dll_Excuted;
	}
}
