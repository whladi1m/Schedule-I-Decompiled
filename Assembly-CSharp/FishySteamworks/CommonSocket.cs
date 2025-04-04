using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using Steamworks;

namespace FishySteamworks
{
	// Token: 0x02000C10 RID: 3088
	public abstract class CommonSocket
	{
		// Token: 0x0600564D RID: 22093 RVA: 0x0016A187 File Offset: 0x00168387
		internal LocalConnectionState GetLocalConnectionState()
		{
			return this._connectionState;
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x0016A190 File Offset: 0x00168390
		protected virtual void SetLocalConnectionState(LocalConnectionState connectionState, bool server)
		{
			if (connectionState == this._connectionState)
			{
				return;
			}
			this._connectionState = connectionState;
			if (server)
			{
				this.Transport.HandleServerConnectionState(new ServerConnectionStateArgs(connectionState, this.Transport.Index));
				return;
			}
			this.Transport.HandleClientConnectionState(new ClientConnectionStateArgs(connectionState, this.Transport.Index));
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x0016A1EC File Offset: 0x001683EC
		internal virtual void Initialize(Transport t)
		{
			this.Transport = t;
			int num = this.Transport.GetMTU(0);
			num = Math.Max(num, this.Transport.GetMTU(1));
			this.InboundBuffer = new byte[num];
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x0016A22C File Offset: 0x0016842C
		protected byte[] GetIPBytes(string address)
		{
			if (string.IsNullOrEmpty(address))
			{
				return null;
			}
			IPAddress ipaddress;
			if (!IPAddress.TryParse(address, out ipaddress))
			{
				this.Transport.NetworkManager.LogError("Could not parse address " + address + " to IPAddress.");
				return null;
			}
			return ipaddress.GetAddressBytes();
		}

		// Token: 0x06005651 RID: 22097 RVA: 0x0016A278 File Offset: 0x00168478
		protected EResult Send(HSteamNetConnection steamConnection, ArraySegment<byte> segment, byte channelId)
		{
			if (segment.Array.Length - 1 <= segment.Offset + segment.Count)
			{
				byte[] array = segment.Array;
				Array.Resize<byte>(ref array, array.Length + 1);
				array[array.Length - 1] = channelId;
			}
			else
			{
				segment.Array[segment.Offset + segment.Count] = channelId;
			}
			segment = new ArraySegment<byte>(segment.Array, segment.Offset, segment.Count + 1);
			GCHandle gchandle = GCHandle.Alloc(segment.Array, GCHandleType.Pinned);
			IntPtr pData = gchandle.AddrOfPinnedObject() + segment.Offset;
			int nSendFlags = (channelId == 1) ? 0 : 8;
			long num;
			EResult eresult = SteamNetworkingSockets.SendMessageToConnection(steamConnection, pData, (uint)segment.Count, nSendFlags, out num);
			if (eresult != EResult.k_EResultOK)
			{
				this.Transport.NetworkManager.LogWarning(string.Format("Send issue: {0}", eresult));
			}
			gchandle.Free();
			return eresult;
		}

		// Token: 0x06005652 RID: 22098 RVA: 0x0016A364 File Offset: 0x00168564
		internal void ClearQueue(ConcurrentQueue<LocalPacket> queue)
		{
			LocalPacket localPacket;
			while (queue.TryDequeue(out localPacket))
			{
				ByteArrayPool.Store(localPacket.Data);
			}
		}

		// Token: 0x06005653 RID: 22099 RVA: 0x0016A388 File Offset: 0x00168588
		internal void ClearQueue(Queue<LocalPacket> queue)
		{
			while (queue.Count > 0)
			{
				ByteArrayPool.Store(queue.Dequeue().Data);
			}
		}

		// Token: 0x06005654 RID: 22100 RVA: 0x0016A3A8 File Offset: 0x001685A8
		protected void GetMessage(IntPtr ptr, byte[] buffer, out ArraySegment<byte> segment, out byte channel)
		{
			SteamNetworkingMessage_t steamNetworkingMessage_t = Marshal.PtrToStructure<SteamNetworkingMessage_t>(ptr);
			int cbSize = steamNetworkingMessage_t.m_cbSize;
			Marshal.Copy(steamNetworkingMessage_t.m_pData, buffer, 0, cbSize);
			SteamNetworkingMessage_t.Release(ptr);
			channel = buffer[cbSize - 1];
			segment = new ArraySegment<byte>(buffer, 0, cbSize - 1);
		}

		// Token: 0x04004031 RID: 16433
		private LocalConnectionState _connectionState;

		// Token: 0x04004032 RID: 16434
		protected bool PeerToPeer;

		// Token: 0x04004033 RID: 16435
		protected Transport Transport;

		// Token: 0x04004034 RID: 16436
		protected IntPtr[] MessagePointers = new IntPtr[256];

		// Token: 0x04004035 RID: 16437
		protected byte[] InboundBuffer;

		// Token: 0x04004036 RID: 16438
		protected const int MAX_MESSAGES = 256;
	}
}
