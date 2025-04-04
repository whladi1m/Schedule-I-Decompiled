using System;
using System.Collections.Generic;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using FishySteamworks.Server;

namespace FishySteamworks.Client
{
	// Token: 0x02000C15 RID: 3093
	public class ClientHostSocket : CommonSocket
	{
		// Token: 0x0600569B RID: 22171 RVA: 0x0016B57A File Offset: 0x0016977A
		internal void CheckSetStarted()
		{
			if (this._server != null && base.GetLocalConnectionState() == LocalConnectionState.Starting && this._server.GetLocalConnectionState() == LocalConnectionState.Started)
			{
				this.SetLocalConnectionState(LocalConnectionState.Started, false);
			}
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x0016B5A3 File Offset: 0x001697A3
		internal bool StartConnection(ServerSocket serverSocket)
		{
			this._server = serverSocket;
			this._server.SetClientHostSocket(this);
			if (this._server.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return false;
			}
			this.SetLocalConnectionState(LocalConnectionState.Starting, false);
			return true;
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x0016B5D1 File Offset: 0x001697D1
		protected override void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			base.SetLocalConnectionState(connectionState, server);
			if (connectionState == LocalConnectionState.Started)
			{
				this._server.OnClientHostState(true);
				return;
			}
			this._server.OnClientHostState(false);
		}

		// Token: 0x0600569E RID: 22174 RVA: 0x0016B5F8 File Offset: 0x001697F8
		internal bool StopConnection()
		{
			if (base.GetLocalConnectionState() == LocalConnectionState.Stopped || base.GetLocalConnectionState() == LocalConnectionState.Stopping)
			{
				return false;
			}
			base.ClearQueue(this._incoming);
			this.SetLocalConnectionState(LocalConnectionState.Stopping, false);
			this.SetLocalConnectionState(LocalConnectionState.Stopped, false);
			this._server.SetClientHostSocket(null);
			return true;
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x0016B638 File Offset: 0x00169838
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			while (this._incoming.Count > 0)
			{
				LocalPacket localPacket = this._incoming.Dequeue();
				ArraySegment<byte> data = new ArraySegment<byte>(localPacket.Data, 0, localPacket.Length);
				this.Transport.HandleClientReceivedDataArgs(new ClientReceivedDataArgs(data, (Channel)localPacket.Channel, this.Transport.Index));
				ByteArrayPool.Store(localPacket.Data);
			}
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x0016B6AA File Offset: 0x001698AA
		internal void ReceivedFromLocalServer(LocalPacket packet)
		{
			this._incoming.Enqueue(packet);
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x0016B6B8 File Offset: 0x001698B8
		internal void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			if (this._server.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			LocalPacket packet = new LocalPacket(segment, channelId);
			this._server.ReceivedFromClientHost(packet);
		}

		// Token: 0x0400405A RID: 16474
		private ServerSocket _server;

		// Token: 0x0400405B RID: 16475
		private Queue<LocalPacket> _incoming = new Queue<LocalPacket>();
	}
}
