using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Transporting;
using FishySteamworks.Client;
using Steamworks;

namespace FishySteamworks.Server
{
	// Token: 0x02000C13 RID: 3091
	public class ServerSocket : CommonSocket
	{
		// Token: 0x06005684 RID: 22148 RVA: 0x0016AB89 File Offset: 0x00168D89
		internal RemoteConnectionState GetConnectionState(int connectionId)
		{
			if (this._steamConnections.Second.ContainsKey(connectionId))
			{
				return RemoteConnectionState.Started;
			}
			return RemoteConnectionState.Stopped;
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x0016ABA1 File Offset: 0x00168DA1
		internal void ResetInvalidSocket()
		{
			if (this._socket == HSteamListenSocket.Invalid)
			{
				base.SetLocalConnectionState(LocalConnectionState.Stopped, true);
			}
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x0016ABC0 File Offset: 0x00168DC0
		internal bool StartConnection(string address, ushort port, int maximumClients, bool peerToPeer)
		{
			try
			{
				if (this._onRemoteConnectionStateCallback == null)
				{
					this._onRemoteConnectionStateCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnRemoteConnectionState));
				}
				this.PeerToPeer = peerToPeer;
				byte[] array = (!peerToPeer) ? base.GetIPBytes(address) : null;
				this.PeerToPeer = peerToPeer;
				this.SetMaximumClients(maximumClients);
				this._nextConnectionId = 0;
				this._cachedConnectionIds.Clear();
				this._iteratingConnections = false;
				base.SetLocalConnectionState(LocalConnectionState.Starting, true);
				SteamNetworkingConfigValue_t[] array2 = new SteamNetworkingConfigValue_t[0];
				if (this.PeerToPeer)
				{
					this._socket = SteamNetworkingSockets.CreateListenSocketP2P(0, array2.Length, array2);
				}
				else
				{
					SteamNetworkingIPAddr steamNetworkingIPAddr = default(SteamNetworkingIPAddr);
					steamNetworkingIPAddr.Clear();
					if (array != null)
					{
						steamNetworkingIPAddr.SetIPv6(array, port);
					}
					this._socket = SteamNetworkingSockets.CreateListenSocketIP(ref steamNetworkingIPAddr, 0, array2);
				}
			}
			catch
			{
				base.SetLocalConnectionState(LocalConnectionState.Stopped, true);
				return false;
			}
			if (this._socket == HSteamListenSocket.Invalid)
			{
				base.SetLocalConnectionState(LocalConnectionState.Stopped, true);
				return false;
			}
			base.SetLocalConnectionState(LocalConnectionState.Started, true);
			return true;
		}

		// Token: 0x06005687 RID: 22151 RVA: 0x0016ACC4 File Offset: 0x00168EC4
		internal bool StopConnection()
		{
			if (this._socket != HSteamListenSocket.Invalid)
			{
				SteamNetworkingSockets.CloseListenSocket(this._socket);
				if (this._onRemoteConnectionStateCallback != null)
				{
					this._onRemoteConnectionStateCallback.Dispose();
					this._onRemoteConnectionStateCallback = null;
				}
				this._socket = HSteamListenSocket.Invalid;
			}
			this._pendingConnectionChanges.Clear();
			if (base.GetLocalConnectionState() == LocalConnectionState.Stopped)
			{
				return false;
			}
			base.SetLocalConnectionState(LocalConnectionState.Stopping, true);
			base.SetLocalConnectionState(LocalConnectionState.Stopped, true);
			return true;
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x0016AD3C File Offset: 0x00168F3C
		internal bool StopConnection(int connectionId)
		{
			if (connectionId == 32767)
			{
				if (this._clientHost != null)
				{
					this._clientHost.StopConnection();
					return true;
				}
				return false;
			}
			else
			{
				HSteamNetConnection socket;
				if (this._steamConnections.Second.TryGetValue(connectionId, out socket))
				{
					return this.StopConnection(connectionId, socket);
				}
				this.Transport.NetworkManager.LogError(string.Format("Steam connection not found for connectionId {0}.", connectionId));
				return false;
			}
		}

		// Token: 0x06005689 RID: 22153 RVA: 0x0016ADA8 File Offset: 0x00168FA8
		private bool StopConnection(int connectionId, HSteamNetConnection socket)
		{
			SteamNetworkingSockets.CloseConnection(socket, 0, string.Empty, false);
			if (!this._iteratingConnections)
			{
				this.RemoveConnection(connectionId);
			}
			else
			{
				this._pendingConnectionChanges.Add(new ServerSocket.ConnectionChange(connectionId));
			}
			return true;
		}

		// Token: 0x0600568A RID: 22154 RVA: 0x0016ADDC File Offset: 0x00168FDC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void OnRemoteConnectionState(SteamNetConnectionStatusChangedCallback_t args)
		{
			ulong steamID = args.m_info.m_identityRemote.GetSteamID64();
			if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting)
			{
				if (this._steamConnections.Count >= this.GetMaximumClients())
				{
					this.Transport.NetworkManager.Log(string.Format("Incoming connection {0} was rejected because would exceed the maximum connection count.", steamID));
					SteamNetworkingSockets.CloseConnection(args.m_hConn, 0, "Max Connection Count", false);
					return;
				}
				EResult eresult = SteamNetworkingSockets.AcceptConnection(args.m_hConn);
				if (eresult == EResult.k_EResultOK)
				{
					this.Transport.NetworkManager.Log(string.Format("Accepting connection {0}", steamID));
					return;
				}
				this.Transport.NetworkManager.Log(string.Format("Connection {0} could not be accepted: {1}", steamID, eresult.ToString()));
				return;
			}
			else
			{
				if (args.m_info.m_eState != ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected)
				{
					if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer || args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally)
					{
						int connectionId;
						if (this._steamConnections.TryGetValue(args.m_hConn, out connectionId))
						{
							this.StopConnection(connectionId, args.m_hConn);
							return;
						}
					}
					else
					{
						this.Transport.NetworkManager.Log(string.Format("Connection {0} state changed: {1}", steamID, args.m_info.m_eState.ToString()));
					}
					return;
				}
				int num;
				if (this._cachedConnectionIds.Count <= 0)
				{
					int nextConnectionId = this._nextConnectionId;
					this._nextConnectionId = nextConnectionId + 1;
					num = nextConnectionId;
				}
				else
				{
					num = this._cachedConnectionIds.Dequeue();
				}
				int num2 = num;
				if (!this._iteratingConnections)
				{
					this.AddConnection(num2, args.m_hConn, args.m_info.m_identityRemote.GetSteamID());
					return;
				}
				this._pendingConnectionChanges.Add(new ServerSocket.ConnectionChange(num2, args.m_hConn, args.m_info.m_identityRemote.GetSteamID()));
				return;
			}
		}

		// Token: 0x0600568B RID: 22155 RVA: 0x0016AFB8 File Offset: 0x001691B8
		private void AddConnection(int connectionId, HSteamNetConnection steamConnection, CSteamID steamId)
		{
			this._steamConnections.Add(steamConnection, connectionId);
			this._steamIds.Add(steamId, connectionId);
			this.Transport.NetworkManager.Log(string.Format("Client with SteamID {0} connected. Assigning connection id {1}", steamId.m_SteamID, connectionId));
			this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Started, connectionId, this.Transport.Index));
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x0016B028 File Offset: 0x00169228
		private void RemoveConnection(int connectionId)
		{
			this._steamConnections.Remove(connectionId);
			this._steamIds.Remove(connectionId);
			this.Transport.NetworkManager.Log(string.Format("Client with ConnectionID {0} disconnected.", connectionId));
			this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Stopped, connectionId, this.Transport.Index));
			this._cachedConnectionIds.Enqueue(connectionId);
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x0016B098 File Offset: 0x00169298
		internal void IterateOutgoing()
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			this._iteratingConnections = true;
			foreach (HSteamNetConnection hConn in this._steamConnections.FirstTypes)
			{
				SteamNetworkingSockets.FlushMessagesOnConnection(hConn);
			}
			this._iteratingConnections = false;
			this.ProcessPendingConnectionChanges();
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x0016B108 File Offset: 0x00169308
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() == LocalConnectionState.Stopped || base.GetLocalConnectionState() == LocalConnectionState.Stopping)
			{
				return;
			}
			this._iteratingConnections = true;
			while (this._clientHostIncoming.Count > 0)
			{
				LocalPacket localPacket = this._clientHostIncoming.Dequeue();
				ArraySegment<byte> data = new ArraySegment<byte>(localPacket.Data, 0, localPacket.Length);
				this.Transport.HandleServerReceivedDataArgs(new ServerReceivedDataArgs(data, (Channel)localPacket.Channel, 32767, this.Transport.Index));
			}
			foreach (KeyValuePair<HSteamNetConnection, int> keyValuePair in this._steamConnections.First)
			{
				HSteamNetConnection key = keyValuePair.Key;
				int value = keyValuePair.Value;
				int num = SteamNetworkingSockets.ReceiveMessagesOnConnection(key, this.MessagePointers, 256);
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						ArraySegment<byte> data2;
						byte channel;
						base.GetMessage(this.MessagePointers[i], this.InboundBuffer, out data2, out channel);
						this.Transport.HandleServerReceivedDataArgs(new ServerReceivedDataArgs(data2, (Channel)channel, value, this.Transport.Index));
					}
				}
			}
			this._iteratingConnections = false;
			this.ProcessPendingConnectionChanges();
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x0016B24C File Offset: 0x0016944C
		private void ProcessPendingConnectionChanges()
		{
			foreach (ServerSocket.ConnectionChange connectionChange in this._pendingConnectionChanges)
			{
				if (connectionChange.IsConnect)
				{
					this.AddConnection(connectionChange.ConnectionId, connectionChange.SteamConnection, connectionChange.SteamId);
				}
				else
				{
					this.RemoveConnection(connectionChange.ConnectionId);
				}
			}
			this._pendingConnectionChanges.Clear();
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x0016B2D4 File Offset: 0x001694D4
		internal void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			if (connectionId == 32767)
			{
				if (this._clientHost != null)
				{
					LocalPacket packet = new LocalPacket(segment, channelId);
					this._clientHost.ReceivedFromLocalServer(packet);
				}
				return;
			}
			HSteamNetConnection hsteamNetConnection;
			if (this._steamConnections.TryGetValue(connectionId, out hsteamNetConnection))
			{
				EResult eresult = base.Send(hsteamNetConnection, segment, channelId);
				if (eresult == EResult.k_EResultNoConnection || eresult == EResult.k_EResultInvalidParam)
				{
					this.Transport.NetworkManager.Log(string.Format("Connection to {0} was lost.", connectionId));
					this.StopConnection(connectionId, hsteamNetConnection);
					return;
				}
				if (eresult != EResult.k_EResultOK)
				{
					this.Transport.NetworkManager.LogError("Could not send: " + eresult.ToString());
					return;
				}
			}
			else
			{
				this.Transport.NetworkManager.LogError(string.Format("ConnectionId {0} does not exist, data will not be sent.", connectionId));
			}
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x0016B3AC File Offset: 0x001695AC
		internal string GetConnectionAddress(int connectionId)
		{
			CSteamID csteamID;
			if (this._steamIds.TryGetValue(connectionId, out csteamID))
			{
				return csteamID.ToString();
			}
			this.Transport.NetworkManager.LogError(string.Format("ConnectionId {0} is invalid; address cannot be returned.", connectionId));
			return string.Empty;
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x0016B3FC File Offset: 0x001695FC
		internal void SetMaximumClients(int value)
		{
			this._maximumClients = Math.Min(value, 32766);
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x0016B40F File Offset: 0x0016960F
		internal int GetMaximumClients()
		{
			return this._maximumClients;
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x0016B417 File Offset: 0x00169617
		internal void SetClientHostSocket(ClientHostSocket socket)
		{
			this._clientHost = socket;
		}

		// Token: 0x06005695 RID: 22165 RVA: 0x0016B420 File Offset: 0x00169620
		internal void OnClientHostState(bool started)
		{
			FishySteamworks fishySteamworks = (FishySteamworks)this.Transport;
			CSteamID key = new CSteamID(fishySteamworks.LocalUserSteamID);
			if (!started && this._clientHostStarted)
			{
				base.ClearQueue(this._clientHostIncoming);
				this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Stopped, 32767, this.Transport.Index));
				this._steamIds.Remove(key);
			}
			else if (started)
			{
				this._steamIds[key] = 32767;
				this.Transport.HandleRemoteConnectionState(new RemoteConnectionStateArgs(RemoteConnectionState.Started, 32767, this.Transport.Index));
			}
			this._clientHostStarted = started;
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x0016B4C8 File Offset: 0x001696C8
		internal void ReceivedFromClientHost(LocalPacket packet)
		{
			if (!this._clientHostStarted)
			{
				return;
			}
			this._clientHostIncoming.Enqueue(packet);
		}

		// Token: 0x0400404B RID: 16459
		private BidirectionalDictionary<HSteamNetConnection, int> _steamConnections = new BidirectionalDictionary<HSteamNetConnection, int>();

		// Token: 0x0400404C RID: 16460
		private BidirectionalDictionary<CSteamID, int> _steamIds = new BidirectionalDictionary<CSteamID, int>();

		// Token: 0x0400404D RID: 16461
		private int _maximumClients;

		// Token: 0x0400404E RID: 16462
		private int _nextConnectionId;

		// Token: 0x0400404F RID: 16463
		private HSteamListenSocket _socket = new HSteamListenSocket(0U);

		// Token: 0x04004050 RID: 16464
		private Queue<LocalPacket> _clientHostIncoming = new Queue<LocalPacket>();

		// Token: 0x04004051 RID: 16465
		private bool _clientHostStarted;

		// Token: 0x04004052 RID: 16466
		private Callback<SteamNetConnectionStatusChangedCallback_t> _onRemoteConnectionStateCallback;

		// Token: 0x04004053 RID: 16467
		private Queue<int> _cachedConnectionIds = new Queue<int>();

		// Token: 0x04004054 RID: 16468
		private ClientHostSocket _clientHost;

		// Token: 0x04004055 RID: 16469
		private bool _iteratingConnections;

		// Token: 0x04004056 RID: 16470
		private List<ServerSocket.ConnectionChange> _pendingConnectionChanges = new List<ServerSocket.ConnectionChange>();

		// Token: 0x02000C14 RID: 3092
		public struct ConnectionChange
		{
			// Token: 0x17000C21 RID: 3105
			// (get) Token: 0x06005698 RID: 22168 RVA: 0x0016B536 File Offset: 0x00169736
			public bool IsConnect
			{
				get
				{
					return this.SteamId.IsValid();
				}
			}

			// Token: 0x06005699 RID: 22169 RVA: 0x0016B543 File Offset: 0x00169743
			public ConnectionChange(int id)
			{
				this.ConnectionId = id;
				this.SteamId = CSteamID.Nil;
				this.SteamConnection = default(HSteamNetConnection);
			}

			// Token: 0x0600569A RID: 22170 RVA: 0x0016B563 File Offset: 0x00169763
			public ConnectionChange(int id, HSteamNetConnection steamConnection, CSteamID steamId)
			{
				this.ConnectionId = id;
				this.SteamConnection = steamConnection;
				this.SteamId = steamId;
			}

			// Token: 0x04004057 RID: 16471
			public int ConnectionId;

			// Token: 0x04004058 RID: 16472
			public HSteamNetConnection SteamConnection;

			// Token: 0x04004059 RID: 16473
			public CSteamID SteamId;
		}
	}
}
