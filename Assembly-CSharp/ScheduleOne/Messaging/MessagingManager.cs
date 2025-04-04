using System;
using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace ScheduleOne.Messaging
{
	// Token: 0x0200053A RID: 1338
	public class MessagingManager : NetworkSingleton<MessagingManager>
	{
		// Token: 0x060020B0 RID: 8368 RVA: 0x000863D3 File Offset: 0x000845D3
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Messaging.MessagingManager_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000863E8 File Offset: 0x000845E8
		public override void OnSpawnServer(NetworkConnection connection)
		{
			MessagingManager.<>c__DisplayClass2_0 CS$<>8__locals1 = new MessagingManager.<>c__DisplayClass2_0();
			CS$<>8__locals1.connection = connection;
			CS$<>8__locals1.<>4__this = this;
			base.OnSpawnServer(CS$<>8__locals1.connection);
			if (CS$<>8__locals1.connection.IsLocalClient)
			{
				return;
			}
			base.StartCoroutine(CS$<>8__locals1.<OnSpawnServer>g__SendMessages|0());
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00086430 File Offset: 0x00084630
		public MSGConversation GetConversation(NPC npc)
		{
			if (!this.ConversationMap.ContainsKey(npc))
			{
				Console.LogError("No conversation found for " + npc.fullName, null);
				return null;
			}
			return this.ConversationMap[npc];
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00086464 File Offset: 0x00084664
		public void Register(NPC npc, MSGConversation convs)
		{
			if (this.ConversationMap.ContainsKey(npc))
			{
				Console.LogError("Conversation already registered for " + npc.fullName, null);
				return;
			}
			this.ConversationMap.Add(npc, convs);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00086498 File Offset: 0x00084698
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMessage(Message m, bool notify, string npcID)
		{
			this.RpcWriter___Server_SendMessage_2134336246(m, notify, npcID);
			this.RpcLogic___SendMessage_2134336246(m, notify, npcID);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x000864C0 File Offset: 0x000846C0
		[ObserversRpc(RunLocally = true)]
		private void ReceiveMessage(Message m, bool notify, string npcID)
		{
			this.RpcWriter___Observers_ReceiveMessage_2134336246(m, notify, npcID);
			this.RpcLogic___ReceiveMessage_2134336246(m, notify, npcID);
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000864F1 File Offset: 0x000846F1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendMessageChain(MessageChain m, string npcID, float initialDelay, bool notify)
		{
			this.RpcWriter___Server_SendMessageChain_3949292778(m, npcID, initialDelay, notify);
			this.RpcLogic___SendMessageChain_3949292778(m, npcID, initialDelay, notify);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00086520 File Offset: 0x00084720
		[ObserversRpc(RunLocally = true)]
		private void ReceiveMessageChain(MessageChain m, string npcID, float initialDelay, bool notify)
		{
			this.RpcWriter___Observers_ReceiveMessageChain_3949292778(m, npcID, initialDelay, notify);
			this.RpcLogic___ReceiveMessageChain_3949292778(m, npcID, initialDelay, notify);
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00086559 File Offset: 0x00084759
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendResponse(int responseIndex, string npcID)
		{
			this.RpcWriter___Server_SendResponse_2801973956(responseIndex, npcID);
			this.RpcLogic___SendResponse_2801973956(responseIndex, npcID);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x00086578 File Offset: 0x00084778
		[ObserversRpc(RunLocally = true)]
		private void ReceiveResponse(int responseIndex, string npcID)
		{
			this.RpcWriter___Observers_ReceiveResponse_2801973956(responseIndex, npcID);
			this.RpcLogic___ReceiveResponse_2801973956(responseIndex, npcID);
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000865A1 File Offset: 0x000847A1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendPlayerMessage(int sendableIndex, int sentIndex, string npcID)
		{
			this.RpcWriter___Server_SendPlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
			this.RpcLogic___SendPlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000865C8 File Offset: 0x000847C8
		[ObserversRpc(RunLocally = true)]
		private void ReceivePlayerMessage(int sendableIndex, int sentIndex, string npcID)
		{
			this.RpcWriter___Observers_ReceivePlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
			this.RpcLogic___ReceivePlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000865FC File Offset: 0x000847FC
		[TargetRpc]
		private void ReceiveMSGConversationData(NetworkConnection conn, string npcID, MSGConversationData data)
		{
			this.RpcWriter___Target_ReceiveMSGConversationData_2662241369(conn, npcID, data);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x0008661B File Offset: 0x0008481B
		[ServerRpc(RequireOwnership = false)]
		public void ClearResponses(string npcID)
		{
			this.RpcWriter___Server_ClearResponses_3615296227(npcID);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00086628 File Offset: 0x00084828
		[ObserversRpc]
		private void ReceiveClearResponses(string npcID)
		{
			this.RpcWriter___Observers_ReceiveClearResponses_3615296227(npcID);
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x0008663F File Offset: 0x0008483F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void ShowResponses(string npcID, List<Response> responses, float delay)
		{
			this.RpcWriter___Server_ShowResponses_995803534(npcID, responses, delay);
			this.RpcLogic___ShowResponses_995803534(npcID, responses, delay);
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00086668 File Offset: 0x00084868
		[ObserversRpc(RunLocally = true)]
		private void ReceiveShowResponses(string npcID, List<Response> responses, float delay)
		{
			this.RpcWriter___Observers_ReceiveShowResponses_995803534(npcID, responses, delay);
			this.RpcLogic___ReceiveShowResponses_995803534(npcID, responses, delay);
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000866AC File Offset: 0x000848AC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Messaging.MessagingManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Messaging.MessagingManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendMessage_2134336246));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveMessage_2134336246));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendMessageChain_3949292778));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveMessageChain_3949292778));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendResponse_2801973956));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveResponse_2801973956));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_SendPlayerMessage_1952281135));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_ReceivePlayerMessage_1952281135));
			base.RegisterTargetRpc(8U, new ClientRpcDelegate(this.RpcReader___Target_ReceiveMSGConversationData_2662241369));
			base.RegisterServerRpc(9U, new ServerRpcDelegate(this.RpcReader___Server_ClearResponses_3615296227));
			base.RegisterObserversRpc(10U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveClearResponses_3615296227));
			base.RegisterServerRpc(11U, new ServerRpcDelegate(this.RpcReader___Server_ShowResponses_995803534));
			base.RegisterObserversRpc(12U, new ClientRpcDelegate(this.RpcReader___Observers_ReceiveShowResponses_995803534));
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000867FB File Offset: 0x000849FB
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Messaging.MessagingManagerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Messaging.MessagingManagerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x00086814 File Offset: 0x00084A14
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00086824 File Offset: 0x00084A24
		private void RpcWriter___Server_SendMessage_2134336246(Message m, bool notify, string npcID)
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
			writer.Write___ScheduleOne.Messaging.MessageFishNet.Serializing.Generated(m);
			writer.WriteBoolean(notify);
			writer.WriteString(npcID);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000868E5 File Offset: 0x00084AE5
		public void RpcLogic___SendMessage_2134336246(Message m, bool notify, string npcID)
		{
			this.ReceiveMessage(m, notify, npcID);
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000868F0 File Offset: 0x00084AF0
		private void RpcReader___Server_SendMessage_2134336246(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			Message m = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Messaging.MessageFishNet.Serializing.Generateds(PooledReader0);
			bool notify = PooledReader0.ReadBoolean();
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMessage_2134336246(m, notify, npcID);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00086950 File Offset: 0x00084B50
		private void RpcWriter___Observers_ReceiveMessage_2134336246(Message m, bool notify, string npcID)
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
			writer.Write___ScheduleOne.Messaging.MessageFishNet.Serializing.Generated(m);
			writer.WriteBoolean(notify);
			writer.WriteString(npcID);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x00086A20 File Offset: 0x00084C20
		private void RpcLogic___ReceiveMessage_2134336246(Message m, bool notify, string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].SendMessage(m, notify, false);
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x00086A64 File Offset: 0x00084C64
		private void RpcReader___Observers_ReceiveMessage_2134336246(PooledReader PooledReader0, Channel channel)
		{
			Message m = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Messaging.MessageFishNet.Serializing.Generateds(PooledReader0);
			bool notify = PooledReader0.ReadBoolean();
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveMessage_2134336246(m, notify, npcID);
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x00086AC4 File Offset: 0x00084CC4
		private void RpcWriter___Server_SendMessageChain_3949292778(MessageChain m, string npcID, float initialDelay, bool notify)
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
			writer.Write___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generated(m);
			writer.WriteString(npcID);
			writer.WriteSingle(initialDelay, AutoPackType.Unpacked);
			writer.WriteBoolean(notify);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x00086B97 File Offset: 0x00084D97
		public void RpcLogic___SendMessageChain_3949292778(MessageChain m, string npcID, float initialDelay, bool notify)
		{
			this.ReceiveMessageChain(m, npcID, initialDelay, notify);
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x00086BA4 File Offset: 0x00084DA4
		private void RpcReader___Server_SendMessageChain_3949292778(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			MessageChain m = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generateds(PooledReader0);
			string npcID = PooledReader0.ReadString();
			float initialDelay = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			bool notify = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendMessageChain_3949292778(m, npcID, initialDelay, notify);
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x00086C1C File Offset: 0x00084E1C
		private void RpcWriter___Observers_ReceiveMessageChain_3949292778(MessageChain m, string npcID, float initialDelay, bool notify)
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
			writer.Write___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generated(m);
			writer.WriteString(npcID);
			writer.WriteSingle(initialDelay, AutoPackType.Unpacked);
			writer.WriteBoolean(notify);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00086D00 File Offset: 0x00084F00
		private void RpcLogic___ReceiveMessageChain_3949292778(MessageChain m, string npcID, float initialDelay, bool notify)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].SendMessageChain(m, initialDelay, notify, false);
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00086D48 File Offset: 0x00084F48
		private void RpcReader___Observers_ReceiveMessageChain_3949292778(PooledReader PooledReader0, Channel channel)
		{
			MessageChain m = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.UI.Phone.Messages.MessageChainFishNet.Serializing.Generateds(PooledReader0);
			string npcID = PooledReader0.ReadString();
			float initialDelay = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			bool notify = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveMessageChain_3949292778(m, npcID, initialDelay, notify);
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00086DBC File Offset: 0x00084FBC
		private void RpcWriter___Server_SendResponse_2801973956(int responseIndex, string npcID)
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
			writer.WriteInt32(responseIndex, AutoPackType.Packed);
			writer.WriteString(npcID);
			base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x00086E75 File Offset: 0x00085075
		public void RpcLogic___SendResponse_2801973956(int responseIndex, string npcID)
		{
			this.ReceiveResponse(responseIndex, npcID);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00086E80 File Offset: 0x00085080
		private void RpcReader___Server_SendResponse_2801973956(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int responseIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendResponse_2801973956(responseIndex, npcID);
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00086ED4 File Offset: 0x000850D4
		private void RpcWriter___Observers_ReceiveResponse_2801973956(int responseIndex, string npcID)
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
			writer.WriteInt32(responseIndex, AutoPackType.Packed);
			writer.WriteString(npcID);
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x00086F9C File Offset: 0x0008519C
		private void RpcLogic___ReceiveResponse_2801973956(int responseIndex, string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			MSGConversation msgconversation = this.ConversationMap[npc];
			if (msgconversation.currentResponses.Count <= responseIndex)
			{
				Console.LogWarning("Response index out of range for " + npc.fullName, null);
				return;
			}
			msgconversation.ResponseChosen(msgconversation.currentResponses[responseIndex], false);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00087010 File Offset: 0x00085210
		private void RpcReader___Observers_ReceiveResponse_2801973956(PooledReader PooledReader0, Channel channel)
		{
			int responseIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveResponse_2801973956(responseIndex, npcID);
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x00087064 File Offset: 0x00085264
		private void RpcWriter___Server_SendPlayerMessage_1952281135(int sendableIndex, int sentIndex, string npcID)
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
			writer.WriteInt32(sendableIndex, AutoPackType.Packed);
			writer.WriteInt32(sentIndex, AutoPackType.Packed);
			writer.WriteString(npcID);
			base.SendServerRpc(6U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x0008712F File Offset: 0x0008532F
		public void RpcLogic___SendPlayerMessage_1952281135(int sendableIndex, int sentIndex, string npcID)
		{
			this.ReceivePlayerMessage(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x0008713C File Offset: 0x0008533C
		private void RpcReader___Server_SendPlayerMessage_1952281135(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int sendableIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int sentIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendPlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000871A8 File Offset: 0x000853A8
		private void RpcWriter___Observers_ReceivePlayerMessage_1952281135(int sendableIndex, int sentIndex, string npcID)
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
			writer.WriteInt32(sendableIndex, AutoPackType.Packed);
			writer.WriteInt32(sentIndex, AutoPackType.Packed);
			writer.WriteString(npcID);
			base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x00087284 File Offset: 0x00085484
		private void RpcLogic___ReceivePlayerMessage_1952281135(int sendableIndex, int sentIndex, string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].SendPlayerMessage(sendableIndex, sentIndex, false);
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000872C8 File Offset: 0x000854C8
		private void RpcReader___Observers_ReceivePlayerMessage_1952281135(PooledReader PooledReader0, Channel channel)
		{
			int sendableIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int sentIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceivePlayerMessage_1952281135(sendableIndex, sentIndex, npcID);
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00087330 File Offset: 0x00085530
		private void RpcWriter___Target_ReceiveMSGConversationData_2662241369(NetworkConnection conn, string npcID, MSGConversationData data)
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
			writer.WriteString(npcID);
			writer.Write___ScheduleOne.Persistence.Datas.MSGConversationDataFishNet.Serializing.Generated(data);
			base.SendTargetRpc(8U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000873F4 File Offset: 0x000855F4
		private void RpcLogic___ReceiveMSGConversationData_2662241369(NetworkConnection conn, string npcID, MSGConversationData data)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].Load(data);
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00087438 File Offset: 0x00085638
		private void RpcReader___Target_ReceiveMSGConversationData_2662241369(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			MSGConversationData data = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Persistence.Datas.MSGConversationDataFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveMSGConversationData_2662241369(base.LocalConnection, npcID, data);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00087480 File Offset: 0x00085680
		private void RpcWriter___Server_ClearResponses_3615296227(string npcID)
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
			writer.WriteString(npcID);
			base.SendServerRpc(9U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00087527 File Offset: 0x00085727
		public void RpcLogic___ClearResponses_3615296227(string npcID)
		{
			this.ReceiveClearResponses(npcID);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00087530 File Offset: 0x00085730
		private void RpcReader___Server_ClearResponses_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___ClearResponses_3615296227(npcID);
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00087564 File Offset: 0x00085764
		private void RpcWriter___Observers_ReceiveClearResponses_3615296227(string npcID)
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
			writer.WriteString(npcID);
			base.SendObserversRpc(10U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x0008761C File Offset: 0x0008581C
		private void RpcLogic___ReceiveClearResponses_3615296227(string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].ClearResponses(false);
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00087660 File Offset: 0x00085860
		private void RpcReader___Observers_ReceiveClearResponses_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___ReceiveClearResponses_3615296227(npcID);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00087694 File Offset: 0x00085894
		private void RpcWriter___Server_ShowResponses_995803534(string npcID, List<Response> responses, float delay)
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
			writer.WriteString(npcID);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generated(responses);
			writer.WriteSingle(delay, AutoPackType.Unpacked);
			base.SendServerRpc(11U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x0008775A File Offset: 0x0008595A
		public void RpcLogic___ShowResponses_995803534(string npcID, List<Response> responses, float delay)
		{
			this.ReceiveShowResponses(npcID, responses, delay);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00087768 File Offset: 0x00085968
		private void RpcReader___Server_ShowResponses_995803534(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string npcID = PooledReader0.ReadString();
			List<Response> responses = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generateds(PooledReader0);
			float delay = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ShowResponses_995803534(npcID, responses, delay);
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000877D0 File Offset: 0x000859D0
		private void RpcWriter___Observers_ReceiveShowResponses_995803534(string npcID, List<Response> responses, float delay)
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
			writer.WriteString(npcID);
			writer.Write___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generated(responses);
			writer.WriteSingle(delay, AutoPackType.Unpacked);
			base.SendObserversRpc(12U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000878A8 File Offset: 0x00085AA8
		private void RpcLogic___ReceiveShowResponses_995803534(string npcID, List<Response> responses, float delay)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogError("NPC not found with ID " + npcID, null);
				return;
			}
			this.ConversationMap[npc].ShowResponses(responses, delay, false);
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000878EC File Offset: 0x00085AEC
		private void RpcReader___Observers_ReceiveShowResponses_995803534(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			List<Response> responses = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<ScheduleOne.Messaging.Response>FishNet.Serializing.Generateds(PooledReader0);
			float delay = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___ReceiveShowResponses_995803534(npcID, responses, delay);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x0008794E File Offset: 0x00085B4E
		protected virtual void dll()
		{
			base.Awake();
		}

		// Token: 0x04001948 RID: 6472
		protected Dictionary<NPC, MSGConversation> ConversationMap = new Dictionary<NPC, MSGConversation>();

		// Token: 0x04001949 RID: 6473
		private bool dll_Excuted;

		// Token: 0x0400194A RID: 6474
		private bool dll_Excuted;
	}
}
