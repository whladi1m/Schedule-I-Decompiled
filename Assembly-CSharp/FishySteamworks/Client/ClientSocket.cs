using System;
using System.Diagnostics;
using System.Threading;
using FishNet.Transporting;
using Steamworks;
using UnityEngine;

namespace FishySteamworks.Client
{
	// Token: 0x02000C16 RID: 3094
	public class ClientSocket : CommonSocket
	{
		// Token: 0x060056A3 RID: 22179 RVA: 0x0016B708 File Offset: 0x00169908
		private void CheckTimeout()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			do
			{
				if ((float)(stopwatch.ElapsedMilliseconds / 1000L) > this._connectTimeout)
				{
					this.StopConnection();
				}
				Thread.Sleep(50);
			}
			while (base.GetLocalConnectionState() == LocalConnectionState.Starting);
			stopwatch.Stop();
			this._timeoutThread.Abort();
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x0016B760 File Offset: 0x00169960
		internal bool StartConnection(string address, ushort port, bool peerToPeer)
		{
			try
			{
				if (this._onLocalConnectionStateCallback == null)
				{
					this._onLocalConnectionStateCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnLocalConnectionState));
				}
				this.PeerToPeer = peerToPeer;
				byte[] array = (!peerToPeer) ? base.GetIPBytes(address) : null;
				if (!peerToPeer && array == null)
				{
					base.SetLocalConnectionState(LocalConnectionState.Stopped, false);
					return false;
				}
				base.SetLocalConnectionState(LocalConnectionState.Starting, false);
				this._connectTimeout = Time.unscaledTime + 8000f;
				this._timeoutThread = new Thread(new ThreadStart(this.CheckTimeout));
				this._timeoutThread.Start();
				this._hostSteamID = new CSteamID(ulong.Parse(address));
				SteamNetworkingIdentity steamNetworkingIdentity = default(SteamNetworkingIdentity);
				steamNetworkingIdentity.SetSteamID(this._hostSteamID);
				SteamNetworkingConfigValue_t[] array2 = new SteamNetworkingConfigValue_t[0];
				if (this.PeerToPeer)
				{
					this._socket = SteamNetworkingSockets.ConnectP2P(ref steamNetworkingIdentity, 0, array2.Length, array2);
				}
				else
				{
					SteamNetworkingIPAddr steamNetworkingIPAddr = default(SteamNetworkingIPAddr);
					steamNetworkingIPAddr.Clear();
					steamNetworkingIPAddr.SetIPv6(array, port);
					this._socket = SteamNetworkingSockets.ConnectByIPAddress(ref steamNetworkingIPAddr, 0, array2);
				}
			}
			catch
			{
				base.SetLocalConnectionState(LocalConnectionState.Stopped, false);
				return false;
			}
			return true;
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x0016B884 File Offset: 0x00169A84
		private void OnLocalConnectionState(SteamNetConnectionStatusChangedCallback_t args)
		{
			if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected)
			{
				base.SetLocalConnectionState(LocalConnectionState.Started, false);
				return;
			}
			if (args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer || args.m_info.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally)
			{
				this.Transport.NetworkManager.Log("Connection was closed by peer, " + args.m_info.m_szEndDebug);
				this.StopConnection();
				return;
			}
			this.Transport.NetworkManager.Log("Connection state changed: " + args.m_info.m_eState.ToString() + " - " + args.m_info.m_szEndDebug);
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x0016B934 File Offset: 0x00169B34
		internal bool StopConnection()
		{
			if (this._timeoutThread != null && this._timeoutThread.IsAlive)
			{
				this._timeoutThread.Abort();
			}
			if (this._socket != HSteamNetConnection.Invalid)
			{
				if (this._onLocalConnectionStateCallback != null)
				{
					this._onLocalConnectionStateCallback.Dispose();
					this._onLocalConnectionStateCallback = null;
				}
				SteamNetworkingSockets.CloseConnection(this._socket, 0, string.Empty, false);
				this._socket = HSteamNetConnection.Invalid;
			}
			if (base.GetLocalConnectionState() == LocalConnectionState.Stopped || base.GetLocalConnectionState() == LocalConnectionState.Stopping)
			{
				return false;
			}
			base.SetLocalConnectionState(LocalConnectionState.Stopping, false);
			base.SetLocalConnectionState(LocalConnectionState.Stopped, false);
			return true;
		}

		// Token: 0x060056A7 RID: 22183 RVA: 0x0016B9D0 File Offset: 0x00169BD0
		internal void IterateIncoming()
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			int num = SteamNetworkingSockets.ReceiveMessagesOnConnection(this._socket, this.MessagePointers, 256);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					ArraySegment<byte> data;
					byte channel;
					base.GetMessage(this.MessagePointers[i], this.InboundBuffer, out data, out channel);
					this.Transport.HandleClientReceivedDataArgs(new ClientReceivedDataArgs(data, (Channel)channel, this.Transport.Index));
				}
			}
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x0016BA44 File Offset: 0x00169C44
		internal void SendToServer(byte channelId, ArraySegment<byte> segment)
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			EResult eresult = base.Send(this._socket, segment, channelId);
			if (eresult == EResult.k_EResultNoConnection || eresult == EResult.k_EResultInvalidParam)
			{
				this.Transport.NetworkManager.Log("Connection to server was lost.");
				this.StopConnection();
				return;
			}
			if (eresult != EResult.k_EResultOK)
			{
				this.Transport.NetworkManager.LogError("Could not send: " + eresult.ToString());
			}
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x0016BABA File Offset: 0x00169CBA
		internal void IterateOutgoing()
		{
			if (base.GetLocalConnectionState() != LocalConnectionState.Started)
			{
				return;
			}
			SteamNetworkingSockets.FlushMessagesOnConnection(this._socket);
		}

		// Token: 0x0400405C RID: 16476
		private Callback<SteamNetConnectionStatusChangedCallback_t> _onLocalConnectionStateCallback;

		// Token: 0x0400405D RID: 16477
		private CSteamID _hostSteamID = CSteamID.Nil;

		// Token: 0x0400405E RID: 16478
		private HSteamNetConnection _socket;

		// Token: 0x0400405F RID: 16479
		private Thread _timeoutThread;

		// Token: 0x04004060 RID: 16480
		private float _connectTimeout = -1f;

		// Token: 0x04004061 RID: 16481
		private const float CONNECT_TIMEOUT_DURATION = 8000f;
	}
}
