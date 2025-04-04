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
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x02000746 RID: 1862
	public class CardController : NetworkBehaviour
	{
		// Token: 0x0600326F RID: 12911 RVA: 0x000D24CC File Offset: 0x000D06CC
		public virtual void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.CardController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x000D24EB File Offset: 0x000D06EB
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCardValue(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
		{
			this.RpcWriter___Server_SendCardValue_3709737967(cardId, suit, value);
			this.RpcLogic___SendCardValue_3709737967(cardId, suit, value);
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000D2514 File Offset: 0x000D0714
		[ObserversRpc(RunLocally = true)]
		private void SetCardValue(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
		{
			this.RpcWriter___Observers_SetCardValue_3709737967(cardId, suit, value);
			this.RpcLogic___SetCardValue_3709737967(cardId, suit, value);
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x000D2545 File Offset: 0x000D0745
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCardFaceUp(string cardId, bool faceUp)
		{
			this.RpcWriter___Server_SendCardFaceUp_310431262(cardId, faceUp);
			this.RpcLogic___SendCardFaceUp_310431262(cardId, faceUp);
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x000D2564 File Offset: 0x000D0764
		[ObserversRpc(RunLocally = true)]
		private void SetCardFaceUp(string cardId, bool faceUp)
		{
			this.RpcWriter___Observers_SetCardFaceUp_310431262(cardId, faceUp);
			this.RpcLogic___SetCardFaceUp_310431262(cardId, faceUp);
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x000D258D File Offset: 0x000D078D
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendCardGlide(string cardId, Vector3 position, Quaternion rotation, float glideTime)
		{
			this.RpcWriter___Server_SendCardGlide_2833372058(cardId, position, rotation, glideTime);
			this.RpcLogic___SendCardGlide_2833372058(cardId, position, rotation, glideTime);
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000D25BC File Offset: 0x000D07BC
		[ObserversRpc(RunLocally = true)]
		private void SetCardGlide(string cardId, Vector3 position, Quaternion rotation, float glideTime)
		{
			this.RpcWriter___Observers_SetCardGlide_2833372058(cardId, position, rotation, glideTime);
			this.RpcLogic___SetCardGlide_2833372058(cardId, position, rotation, glideTime);
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000D25F5 File Offset: 0x000D07F5
		private PlayingCard GetCard(string cardId)
		{
			return this.cardDictionary[cardId];
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000D2624 File Offset: 0x000D0824
		public virtual void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.CardControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.CardControllerAssembly-CSharp.dll_Excuted = true;
			base.RegisterServerRpc(0U, new ServerRpcDelegate(this.RpcReader___Server_SendCardValue_3709737967));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetCardValue_3709737967));
			base.RegisterServerRpc(2U, new ServerRpcDelegate(this.RpcReader___Server_SendCardFaceUp_310431262));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_SetCardFaceUp_310431262));
			base.RegisterServerRpc(4U, new ServerRpcDelegate(this.RpcReader___Server_SendCardGlide_2833372058));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_SetCardGlide_2833372058));
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x000D26CC File Offset: 0x000D08CC
		public virtual void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.CardControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.CardControllerAssembly-CSharp.dll_Excuted = true;
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x000D26DF File Offset: 0x000D08DF
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x000D26F0 File Offset: 0x000D08F0
		private void RpcWriter___Server_SendCardValue_3709737967(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
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
			writer.WriteString(cardId);
			writer.Write___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generated(suit);
			writer.Write___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generated(value);
			base.SendServerRpc(0U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000D27B1 File Offset: 0x000D09B1
		public void RpcLogic___SendCardValue_3709737967(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
		{
			this.SetCardValue(cardId, suit, value);
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x000D27BC File Offset: 0x000D09BC
		private void RpcReader___Server_SendCardValue_3709737967(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string cardId = PooledReader0.ReadString();
			PlayingCard.ECardSuit suit = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generateds(PooledReader0);
			PlayingCard.ECardValue value = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCardValue_3709737967(cardId, suit, value);
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x000D281C File Offset: 0x000D0A1C
		private void RpcWriter___Observers_SetCardValue_3709737967(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
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
			writer.WriteString(cardId);
			writer.Write___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generated(suit);
			writer.Write___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generated(value);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x000D28EC File Offset: 0x000D0AEC
		private void RpcLogic___SetCardValue_3709737967(string cardId, PlayingCard.ECardSuit suit, PlayingCard.ECardValue value)
		{
			PlayingCard card = this.GetCard(cardId);
			if (card != null)
			{
				card.SetCard(suit, value, false);
			}
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x000D2914 File Offset: 0x000D0B14
		private void RpcReader___Observers_SetCardValue_3709737967(PooledReader PooledReader0, Channel channel)
		{
			string cardId = PooledReader0.ReadString();
			PlayingCard.ECardSuit suit = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardSuitFishNet.Serializing.Generateds(PooledReader0);
			PlayingCard.ECardValue value = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Casino.PlayingCard/ECardValueFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCardValue_3709737967(cardId, suit, value);
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x000D2974 File Offset: 0x000D0B74
		private void RpcWriter___Server_SendCardFaceUp_310431262(string cardId, bool faceUp)
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
			writer.WriteString(cardId);
			writer.WriteBoolean(faceUp);
			base.SendServerRpc(2U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x000D2A28 File Offset: 0x000D0C28
		public void RpcLogic___SendCardFaceUp_310431262(string cardId, bool faceUp)
		{
			this.SetCardFaceUp(cardId, faceUp);
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x000D2A34 File Offset: 0x000D0C34
		private void RpcReader___Server_SendCardFaceUp_310431262(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string cardId = PooledReader0.ReadString();
			bool faceUp = PooledReader0.ReadBoolean();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCardFaceUp_310431262(cardId, faceUp);
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x000D2A84 File Offset: 0x000D0C84
		private void RpcWriter___Observers_SetCardFaceUp_310431262(string cardId, bool faceUp)
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
			writer.WriteString(cardId);
			writer.WriteBoolean(faceUp);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000D2B48 File Offset: 0x000D0D48
		private void RpcLogic___SetCardFaceUp_310431262(string cardId, bool faceUp)
		{
			PlayingCard card = this.GetCard(cardId);
			if (card != null)
			{
				card.SetFaceUp(faceUp, false);
			}
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x000D2B70 File Offset: 0x000D0D70
		private void RpcReader___Observers_SetCardFaceUp_310431262(PooledReader PooledReader0, Channel channel)
		{
			string cardId = PooledReader0.ReadString();
			bool faceUp = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCardFaceUp_310431262(cardId, faceUp);
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x000D2BBC File Offset: 0x000D0DBC
		private void RpcWriter___Server_SendCardGlide_2833372058(string cardId, Vector3 position, Quaternion rotation, float glideTime)
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
			writer.WriteString(cardId);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteSingle(glideTime, AutoPackType.Unpacked);
			base.SendServerRpc(4U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x000D2C94 File Offset: 0x000D0E94
		public void RpcLogic___SendCardGlide_2833372058(string cardId, Vector3 position, Quaternion rotation, float glideTime)
		{
			this.SetCardGlide(cardId, position, rotation, glideTime);
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x000D2CA4 File Offset: 0x000D0EA4
		private void RpcReader___Server_SendCardGlide_2833372058(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string cardId = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			float glideTime = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendCardGlide_2833372058(cardId, position, rotation, glideTime);
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x000D2D20 File Offset: 0x000D0F20
		private void RpcWriter___Observers_SetCardGlide_2833372058(string cardId, Vector3 position, Quaternion rotation, float glideTime)
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
			writer.WriteString(cardId);
			writer.WriteVector3(position);
			writer.WriteQuaternion(rotation, AutoPackType.Packed);
			writer.WriteSingle(glideTime, AutoPackType.Unpacked);
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x000D2E08 File Offset: 0x000D1008
		private void RpcLogic___SetCardGlide_2833372058(string cardId, Vector3 position, Quaternion rotation, float glideTime)
		{
			PlayingCard card = this.GetCard(cardId);
			if (card != null)
			{
				card.GlideTo(position, rotation, glideTime, false);
			}
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x000D2E34 File Offset: 0x000D1034
		private void RpcReader___Observers_SetCardGlide_2833372058(PooledReader PooledReader0, Channel channel)
		{
			string cardId = PooledReader0.ReadString();
			Vector3 position = PooledReader0.ReadVector3();
			Quaternion rotation = PooledReader0.ReadQuaternion(AutoPackType.Packed);
			float glideTime = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetCardGlide_2833372058(cardId, position, rotation, glideTime);
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x000D2EAC File Offset: 0x000D10AC
		private void dll()
		{
			this.cards = new List<PlayingCard>(base.GetComponentsInChildren<PlayingCard>());
			foreach (PlayingCard playingCard in this.cards)
			{
				playingCard.SetCardController(this);
				if (this.cardDictionary.ContainsKey(playingCard.CardID))
				{
					Debug.LogError("Card ID " + playingCard.CardID + " already exists in the dictionary.");
				}
				else
				{
					this.cardDictionary.Add(playingCard.CardID, playingCard);
				}
			}
		}

		// Token: 0x0400243D RID: 9277
		private List<PlayingCard> cards = new List<PlayingCard>();

		// Token: 0x0400243E RID: 9278
		private Dictionary<string, PlayingCard> cardDictionary = new Dictionary<string, PlayingCard>();

		// Token: 0x0400243F RID: 9279
		private bool dll_Excuted;

		// Token: 0x04002440 RID: 9280
		private bool dll_Excuted;
	}
}
