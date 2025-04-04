using System;
using FishNet.Managing;
using FishNet.Transporting;
using FishySteamworks.Client;
using FishySteamworks.Server;
using Steamworks;
using UnityEngine;

namespace FishySteamworks
{
	// Token: 0x02000C12 RID: 3090
	public class FishySteamworks : Transport
	{
		// Token: 0x06005657 RID: 22103 RVA: 0x0016A45C File Offset: 0x0016865C
		~FishySteamworks()
		{
			this.Shutdown();
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x0016A488 File Offset: 0x00168688
		public override void Initialize(NetworkManager networkManager, int transportIndex)
		{
			base.Initialize(networkManager, transportIndex);
			this._client = new ClientSocket();
			this._clientHost = new ClientHostSocket();
			this._server = new ServerSocket();
			this.CreateChannelData();
			this._client.Initialize(this);
			this._clientHost.Initialize(this);
			this._server.Initialize(this);
		}

		// Token: 0x06005659 RID: 22105 RVA: 0x0016A4E8 File Offset: 0x001686E8
		private void OnDestroy()
		{
			this.Shutdown();
		}

		// Token: 0x0600565A RID: 22106 RVA: 0x0016A4F0 File Offset: 0x001686F0
		private void Update()
		{
			this._clientHost.CheckSetStarted();
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x0016A4FD File Offset: 0x001686FD
		private void CreateChannelData()
		{
			this._mtus = new int[]
			{
				1048576,
				1200
			};
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x0016A51C File Offset: 0x0016871C
		private bool InitializeRelayNetworkAccess()
		{
			bool result;
			try
			{
				SteamNetworkingUtils.InitRelayNetworkAccess();
				if (this.IsNetworkAccessAvailable())
				{
					this.LocalUserSteamID = SteamUser.GetSteamID().m_SteamID;
				}
				this._shutdownCalled = false;
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x0016A568 File Offset: 0x00168768
		public bool IsNetworkAccessAvailable()
		{
			bool result;
			try
			{
				InteropHelp.TestIfAvailableClient();
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600565E RID: 22110 RVA: 0x0016A594 File Offset: 0x00168794
		public override string GetConnectionAddress(int connectionId)
		{
			return this._server.GetConnectionAddress(connectionId);
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600565F RID: 22111 RVA: 0x0016A5A4 File Offset: 0x001687A4
		// (remove) Token: 0x06005660 RID: 22112 RVA: 0x0016A5DC File Offset: 0x001687DC
		public override event Action<ClientConnectionStateArgs> OnClientConnectionState;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06005661 RID: 22113 RVA: 0x0016A614 File Offset: 0x00168814
		// (remove) Token: 0x06005662 RID: 22114 RVA: 0x0016A64C File Offset: 0x0016884C
		public override event Action<ServerConnectionStateArgs> OnServerConnectionState;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06005663 RID: 22115 RVA: 0x0016A684 File Offset: 0x00168884
		// (remove) Token: 0x06005664 RID: 22116 RVA: 0x0016A6BC File Offset: 0x001688BC
		public override event Action<RemoteConnectionStateArgs> OnRemoteConnectionState;

		// Token: 0x06005665 RID: 22117 RVA: 0x0016A6F1 File Offset: 0x001688F1
		public override LocalConnectionState GetConnectionState(bool server)
		{
			if (server)
			{
				return this._server.GetLocalConnectionState();
			}
			return this._client.GetLocalConnectionState();
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x0016A70D File Offset: 0x0016890D
		public override RemoteConnectionState GetConnectionState(int connectionId)
		{
			return this._server.GetConnectionState(connectionId);
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x0016A71B File Offset: 0x0016891B
		public override void HandleClientConnectionState(ClientConnectionStateArgs connectionStateArgs)
		{
			Action<ClientConnectionStateArgs> onClientConnectionState = this.OnClientConnectionState;
			if (onClientConnectionState == null)
			{
				return;
			}
			onClientConnectionState(connectionStateArgs);
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x0016A72E File Offset: 0x0016892E
		public override void HandleServerConnectionState(ServerConnectionStateArgs connectionStateArgs)
		{
			Action<ServerConnectionStateArgs> onServerConnectionState = this.OnServerConnectionState;
			if (onServerConnectionState == null)
			{
				return;
			}
			onServerConnectionState(connectionStateArgs);
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x0016A741 File Offset: 0x00168941
		public override void HandleRemoteConnectionState(RemoteConnectionStateArgs connectionStateArgs)
		{
			Action<RemoteConnectionStateArgs> onRemoteConnectionState = this.OnRemoteConnectionState;
			if (onRemoteConnectionState == null)
			{
				return;
			}
			onRemoteConnectionState(connectionStateArgs);
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x0016A754 File Offset: 0x00168954
		public override void IterateIncoming(bool server)
		{
			if (server)
			{
				this._server.IterateIncoming();
				return;
			}
			this._client.IterateIncoming();
			this._clientHost.IterateIncoming();
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x0016A77B File Offset: 0x0016897B
		public override void IterateOutgoing(bool server)
		{
			if (server)
			{
				this._server.IterateOutgoing();
				return;
			}
			this._client.IterateOutgoing();
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600566C RID: 22124 RVA: 0x0016A798 File Offset: 0x00168998
		// (remove) Token: 0x0600566D RID: 22125 RVA: 0x0016A7D0 File Offset: 0x001689D0
		public override event Action<ClientReceivedDataArgs> OnClientReceivedData;

		// Token: 0x0600566E RID: 22126 RVA: 0x0016A805 File Offset: 0x00168A05
		public override void HandleClientReceivedDataArgs(ClientReceivedDataArgs receivedDataArgs)
		{
			Action<ClientReceivedDataArgs> onClientReceivedData = this.OnClientReceivedData;
			if (onClientReceivedData == null)
			{
				return;
			}
			onClientReceivedData(receivedDataArgs);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600566F RID: 22127 RVA: 0x0016A818 File Offset: 0x00168A18
		// (remove) Token: 0x06005670 RID: 22128 RVA: 0x0016A850 File Offset: 0x00168A50
		public override event Action<ServerReceivedDataArgs> OnServerReceivedData;

		// Token: 0x06005671 RID: 22129 RVA: 0x0016A885 File Offset: 0x00168A85
		public override void HandleServerReceivedDataArgs(ServerReceivedDataArgs receivedDataArgs)
		{
			Action<ServerReceivedDataArgs> onServerReceivedData = this.OnServerReceivedData;
			if (onServerReceivedData == null)
			{
				return;
			}
			onServerReceivedData(receivedDataArgs);
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x0016A898 File Offset: 0x00168A98
		public override void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			this._client.SendToServer(channelId, segment);
			this._clientHost.SendToServer(channelId, segment);
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x0016A8B4 File Offset: 0x00168AB4
		public override void SendToClient(byte channelId, ArraySegment<byte> segment, int connectionId)
		{
			this._server.SendToClient(channelId, segment, connectionId);
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x0016A8C4 File Offset: 0x00168AC4
		public override int GetMaximumClients()
		{
			return this._server.GetMaximumClients();
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x0016A8D1 File Offset: 0x00168AD1
		public override void SetMaximumClients(int value)
		{
			this._server.SetMaximumClients(value);
		}

		// Token: 0x06005676 RID: 22134 RVA: 0x0016A8DF File Offset: 0x00168ADF
		public override void SetClientAddress(string address)
		{
			this._clientAddress = address;
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x0016A8E8 File Offset: 0x00168AE8
		public override void SetServerBindAddress(string address, IPAddressType addressType)
		{
			this._serverBindAddress = address;
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x0016A8F1 File Offset: 0x00168AF1
		public override void SetPort(ushort port)
		{
			this._port = port;
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x0016A8FA File Offset: 0x00168AFA
		public override bool StartConnection(bool server)
		{
			if (server)
			{
				return this.StartServer();
			}
			return this.StartClient(this._clientAddress);
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x0016A912 File Offset: 0x00168B12
		public override bool StopConnection(bool server)
		{
			if (server)
			{
				return this.StopServer();
			}
			return this.StopClient();
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x0016A924 File Offset: 0x00168B24
		public override bool StopConnection(int connectionId, bool immediately)
		{
			return this.StopClient(connectionId, immediately);
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x0016A92E File Offset: 0x00168B2E
		public override void Shutdown()
		{
			if (this._shutdownCalled)
			{
				return;
			}
			this._shutdownCalled = true;
			this.StopConnection(false);
			this.StopConnection(true);
		}

		// Token: 0x0600567D RID: 22141 RVA: 0x0016A950 File Offset: 0x00168B50
		private bool StartServer()
		{
			if (!this.InitializeRelayNetworkAccess())
			{
				base.NetworkManager.LogError("RelayNetworkAccess could not be initialized.");
				return false;
			}
			if (!this.IsNetworkAccessAvailable())
			{
				base.NetworkManager.LogError("Server network access is not available.");
				return false;
			}
			this._server.ResetInvalidSocket();
			if (this._server.GetLocalConnectionState() != LocalConnectionState.Stopped)
			{
				base.NetworkManager.LogError("Server is already running.");
				return false;
			}
			bool flag = this._client.GetLocalConnectionState() > LocalConnectionState.Stopped;
			if (flag)
			{
				this._client.StopConnection();
			}
			bool flag2 = this._server.StartConnection(this._serverBindAddress, this._port, (int)this._maximumClients, this._peerToPeer);
			if (flag2 && flag)
			{
				this.StartConnection(false);
			}
			return flag2;
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x0016AA09 File Offset: 0x00168C09
		private bool StopServer()
		{
			return this._server != null && this._server.StopConnection();
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x0016AA20 File Offset: 0x00168C20
		private bool StartClient(string address)
		{
			if (this._server.GetLocalConnectionState() == LocalConnectionState.Stopped)
			{
				if (this._client.GetLocalConnectionState() != LocalConnectionState.Stopped)
				{
					base.NetworkManager.LogError("Client is already running.");
					return false;
				}
				if (this._clientHost.GetLocalConnectionState() != LocalConnectionState.Stopped)
				{
					this._clientHost.StopConnection();
				}
				if (!this.InitializeRelayNetworkAccess())
				{
					base.NetworkManager.LogError("RelayNetworkAccess could not be initialized.");
					return false;
				}
				if (!this.IsNetworkAccessAvailable())
				{
					base.NetworkManager.LogError("Client network access is not available.");
					return false;
				}
				this._client.StartConnection(address, this._port, this._peerToPeer);
			}
			else
			{
				this._clientHost.StartConnection(this._server);
			}
			return true;
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x0016AAD8 File Offset: 0x00168CD8
		private bool StopClient()
		{
			bool flag = false;
			if (this._client != null)
			{
				flag |= this._client.StopConnection();
			}
			if (this._clientHost != null)
			{
				flag |= this._clientHost.StopConnection();
			}
			return flag;
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x0016AB14 File Offset: 0x00168D14
		private bool StopClient(int connectionId, bool immediately)
		{
			return this._server.StopConnection(connectionId);
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x0016AB22 File Offset: 0x00168D22
		public override int GetMTU(byte channel)
		{
			if ((int)channel >= this._mtus.Length)
			{
				Debug.LogError(string.Format("Channel {0} is out of bounds.", channel));
				return 0;
			}
			return this._mtus[(int)channel];
		}

		// Token: 0x0400403A RID: 16442
		[NonSerialized]
		public ulong LocalUserSteamID;

		// Token: 0x0400403B RID: 16443
		[Tooltip("Address server should bind to.")]
		[SerializeField]
		private string _serverBindAddress = string.Empty;

		// Token: 0x0400403C RID: 16444
		[Tooltip("Port to use.")]
		[SerializeField]
		private ushort _port = 7770;

		// Token: 0x0400403D RID: 16445
		[Tooltip("Maximum number of players which may be connected at once.")]
		[Range(1f, 65535f)]
		[SerializeField]
		private ushort _maximumClients = 9001;

		// Token: 0x0400403E RID: 16446
		[Tooltip("True if using peer to peer socket.")]
		[SerializeField]
		private bool _peerToPeer;

		// Token: 0x0400403F RID: 16447
		[Tooltip("Address client should connect to.")]
		[SerializeField]
		private string _clientAddress = string.Empty;

		// Token: 0x04004040 RID: 16448
		private int[] _mtus;

		// Token: 0x04004041 RID: 16449
		private ClientSocket _client;

		// Token: 0x04004042 RID: 16450
		private ClientHostSocket _clientHost;

		// Token: 0x04004043 RID: 16451
		private ServerSocket _server;

		// Token: 0x04004044 RID: 16452
		private bool _shutdownCalled = true;

		// Token: 0x04004045 RID: 16453
		internal const int CLIENT_HOST_ID = 32767;
	}
}
