using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Serializing;
using FishNet.Transporting;
using ScheduleOne.Casino.UI;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x0200073C RID: 1852
	public class BlackjackGameController : CasinoGameController
	{
		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x000D00D6 File Offset: 0x000CE2D6
		// (set) Token: 0x0600320D RID: 12813 RVA: 0x000D00DE File Offset: 0x000CE2DE
		public BlackjackGameController.EStage CurrentStage { get; private set; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x000D00E7 File Offset: 0x000CE2E7
		// (set) Token: 0x0600320F RID: 12815 RVA: 0x000D00EF File Offset: 0x000CE2EF
		public Player PlayerTurn { get; private set; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06003210 RID: 12816 RVA: 0x000D00F8 File Offset: 0x000CE2F8
		// (set) Token: 0x06003211 RID: 12817 RVA: 0x000D0100 File Offset: 0x000CE300
		public float LocalPlayerBet { get; private set; } = 10f;

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x000D0109 File Offset: 0x000CE309
		// (set) Token: 0x06003213 RID: 12819 RVA: 0x000D0111 File Offset: 0x000CE311
		public int DealerScore { get; private set; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06003214 RID: 12820 RVA: 0x000D011A File Offset: 0x000CE31A
		// (set) Token: 0x06003215 RID: 12821 RVA: 0x000D0122 File Offset: 0x000CE322
		public int LocalPlayerScore { get; private set; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06003216 RID: 12822 RVA: 0x000D012B File Offset: 0x000CE32B
		// (set) Token: 0x06003217 RID: 12823 RVA: 0x000D0133 File Offset: 0x000CE333
		public bool IsLocalPlayerBlackjack { get; private set; }

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x000D013C File Offset: 0x000CE33C
		// (set) Token: 0x06003219 RID: 12825 RVA: 0x000D0144 File Offset: 0x000CE344
		public bool IsLocalPlayerBust { get; private set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600321A RID: 12826 RVA: 0x000D014D File Offset: 0x000CE34D
		public bool IsLocalPlayerInCurrentRound
		{
			get
			{
				return this.playersInCurrentRound.Contains(Player.Local);
			}
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x000D015F File Offset: 0x000CE35F
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.BlackjackGameController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x000D0174 File Offset: 0x000CE374
		protected override void Open()
		{
			base.Open();
			Singleton<BlackjackInterface>.Instance.Open(this);
			this.localFocusCameraTransform = this.FocusedCameraTransforms[this.Players.GetPlayerIndex(Player.Local)];
			this.localFinalCameraTransform = this.FinalCameraTransforms[this.Players.GetPlayerIndex(Player.Local)];
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000D01CC File Offset: 0x000CE3CC
		protected override void Close()
		{
			if (this.IsLocalPlayerInCurrentRound)
			{
				this.RemoveLocalPlayerFromGame(BlackjackGameController.EPayoutType.None, 0f);
			}
			Singleton<BlackjackInterface>.Instance.Close();
			base.Close();
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x000D01F2 File Offset: 0x000CE3F2
		protected override void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!base.IsOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape && this.IsLocalPlayerInCurrentRound)
			{
				action.used = true;
				this.RemoveLocalPlayerFromGame(BlackjackGameController.EPayoutType.None, 0f);
			}
			base.Exit(action);
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x000D0234 File Offset: 0x000CE434
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.CurrentStage == BlackjackGameController.EStage.WaitingForPlayers && this.AreAllPlayersReady())
			{
				for (int i = 0; i < this.Players.CurrentPlayerCount; i++)
				{
					this.AddPlayerToCurrentRound(this.Players.GetPlayer(i).NetworkObject);
				}
				this.StartGame();
			}
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x000D0294 File Offset: 0x000CE494
		private List<Player> GetClockwisePlayers()
		{
			List<Player> list = new List<Player>();
			Player player = this.Players.GetPlayer(3);
			Player player2 = this.Players.GetPlayer(1);
			Player player3 = this.Players.GetPlayer(0);
			Player player4 = this.Players.GetPlayer(2);
			if (player != null)
			{
				list.Add(player);
			}
			if (player2 != null)
			{
				list.Add(player2);
			}
			if (player3 != null)
			{
				list.Add(player3);
			}
			if (player4 != null)
			{
				list.Add(player4);
			}
			return list;
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x000D0320 File Offset: 0x000CE520
		[ObserversRpc(RunLocally = true)]
		private void StartGame()
		{
			this.RpcWriter___Observers_StartGame_2166136261();
			this.RpcLogic___StartGame_2166136261();
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x000D033C File Offset: 0x000CE53C
		[ObserversRpc(RunLocally = true)]
		private void NotifyPlayerScore(NetworkObject player, int score, bool blackjack)
		{
			this.RpcWriter___Observers_NotifyPlayerScore_2864061566(player, score, blackjack);
			this.RpcLogic___NotifyPlayerScore_2864061566(player, score, blackjack);
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x000D036D File Offset: 0x000CE56D
		private Transform[] GetPlayerCardPositions(int playerIndex)
		{
			switch (playerIndex)
			{
			case 0:
				return this.Player1CardPositions;
			case 1:
				return this.Player2CardPositions;
			case 2:
				return this.Player3CardPositions;
			case 3:
				return this.Player4CardPositions;
			default:
				return null;
			}
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x000D03A4 File Offset: 0x000CE5A4
		[ObserversRpc(RunLocally = true)]
		private void SetRoundEnded(bool ended)
		{
			this.RpcWriter___Observers_SetRoundEnded_1140765316(ended);
			this.RpcLogic___SetRoundEnded_1140765316(ended);
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x000D03BA File Offset: 0x000CE5BA
		private void AddCardToPlayerHand(int playerIndex, PlayingCard card)
		{
			this.AddCardToPlayerHand(playerIndex, card.CardID);
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x000D03CC File Offset: 0x000CE5CC
		[ObserversRpc(RunLocally = true)]
		private void AddCardToPlayerHand(int playerindex, string cardID)
		{
			this.RpcWriter___Observers_AddCardToPlayerHand_2801973956(playerindex, cardID);
			this.RpcLogic___AddCardToPlayerHand_2801973956(playerindex, cardID);
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x000D03F8 File Offset: 0x000CE5F8
		[ObserversRpc(RunLocally = true)]
		private void AddCardToDealerHand(string cardID)
		{
			this.RpcWriter___Observers_AddCardToDealerHand_3615296227(cardID);
			this.RpcLogic___AddCardToDealerHand_3615296227(cardID);
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x000D0419 File Offset: 0x000CE619
		private List<PlayingCard> GetPlayerCards(int playerIndex)
		{
			switch (playerIndex)
			{
			case 0:
				return this.player1Hand;
			case 1:
				return this.player2Hand;
			case 2:
				return this.player3Hand;
			case 3:
				return this.player4Hand;
			default:
				return null;
			}
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x000D0450 File Offset: 0x000CE650
		private int GetHandScore(List<PlayingCard> cards, bool countFaceDown = true)
		{
			int num = 0;
			foreach (PlayingCard playingCard in cards)
			{
				if (countFaceDown || playingCard.IsFaceUp)
				{
					num += this.GetCardValue(playingCard, true);
				}
			}
			if (num > 21)
			{
				using (List<PlayingCard>.Enumerator enumerator = cards.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Value == PlayingCard.ECardValue.Ace)
						{
							num -= 10;
						}
						if (num <= 21)
						{
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x000D0500 File Offset: 0x000CE700
		private int GetCardValue(PlayingCard card, bool aceAsEleven = true)
		{
			if (card.Value == PlayingCard.ECardValue.Ace)
			{
				if (!aceAsEleven)
				{
					return 1;
				}
				return 11;
			}
			else
			{
				if (card.Value == PlayingCard.ECardValue.Jack || card.Value == PlayingCard.ECardValue.Queen || card.Value == PlayingCard.ECardValue.King)
				{
					return 10;
				}
				return (int)card.Value;
			}
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000D053C File Offset: 0x000CE73C
		private PlayingCard DrawCard()
		{
			PlayingCard playingCard = this.playStack[0];
			this.playStack.RemoveAt(0);
			PlayingCard.CardData cardData = this.cardValuesInDeck[UnityEngine.Random.Range(0, this.cardValuesInDeck.Count)];
			this.cardValuesInDeck.Remove(cardData);
			this.drawnCardsValues.Add(cardData);
			playingCard.SetCard(cardData.Suit, cardData.Value, true);
			return playingCard;
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000D05AC File Offset: 0x000CE7AC
		private void ResetCards()
		{
			if (InstanceFinder.IsServer)
			{
				for (int i = 0; i < this.Cards.Length; i++)
				{
					this.Cards[i].SetFaceUp(false, true);
					this.Cards[i].GlideTo(this.DefaultCardPositions[i].position, this.DefaultCardPositions[i].rotation, 0.5f, true);
				}
			}
			this.cardValuesInDeck = new List<PlayingCard.CardData>();
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 13; k++)
				{
					PlayingCard.CardData item = default(PlayingCard.CardData);
					item.Suit = (PlayingCard.ECardSuit)j;
					item.Value = k + PlayingCard.ECardValue.Ace;
					this.cardValuesInDeck.Add(item);
				}
			}
			this.playStack = new List<PlayingCard>();
			this.playStack.AddRange(this.Cards);
			this.player1Hand.Clear();
			this.player2Hand.Clear();
			this.player3Hand.Clear();
			this.player4Hand.Clear();
			this.dealerHand.Clear();
			this.drawnCardsValues.Clear();
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x000D06B7 File Offset: 0x000CE8B7
		[ObserversRpc(RunLocally = true)]
		private void EndGame()
		{
			this.RpcWriter___Observers_EndGame_2166136261();
			this.RpcLogic___EndGame_2166136261();
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x000D06C8 File Offset: 0x000CE8C8
		public void RemoveLocalPlayerFromGame(BlackjackGameController.EPayoutType payout, float cameraDelay = 0f)
		{
			BlackjackGameController.<>c__DisplayClass83_0 CS$<>8__locals1 = new BlackjackGameController.<>c__DisplayClass83_0();
			CS$<>8__locals1.cameraDelay = cameraDelay;
			CS$<>8__locals1.<>4__this = this;
			this.RequestRemovePlayerFromCurrentRound(Player.Local.NetworkObject);
			this.Players.SetPlayerScore(Player.Local, 0);
			float payout2 = this.GetPayout(this.LocalPlayerBet, payout);
			if (payout2 > 0f)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(payout2, true, false);
			}
			if (this.onLocalPlayerRoundCompleted != null)
			{
				this.onLocalPlayerRoundCompleted(payout);
			}
			if (this.onLocalPlayerExitRound != null)
			{
				this.onLocalPlayerExitRound();
			}
			base.StartCoroutine(CS$<>8__locals1.<RemoveLocalPlayerFromGame>g__Wait|0());
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x000D0761 File Offset: 0x000CE961
		public float GetPayout(float bet, BlackjackGameController.EPayoutType payout)
		{
			switch (payout)
			{
			case BlackjackGameController.EPayoutType.Blackjack:
				return bet * 2.5f;
			case BlackjackGameController.EPayoutType.Win:
				return bet * 2f;
			case BlackjackGameController.EPayoutType.Push:
				return bet;
			default:
				return 0f;
			}
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x000D0790 File Offset: 0x000CE990
		private bool IsCurrentRoundEmpty()
		{
			return this.playersInCurrentRound.Count == 0;
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x000D07A0 File Offset: 0x000CE9A0
		[ObserversRpc(RunLocally = true)]
		private void AddPlayerToCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Observers_AddPlayerToCurrentRound_3323014238(player);
			this.RpcLogic___AddPlayerToCurrentRound_3323014238(player);
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x000D07C1 File Offset: 0x000CE9C1
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void RequestRemovePlayerFromCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Server_RequestRemovePlayerFromCurrentRound_3323014238(player);
			this.RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x000D07D8 File Offset: 0x000CE9D8
		[ObserversRpc(RunLocally = true)]
		private void RemovePlayerFromCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Observers_RemovePlayerFromCurrentRound_3323014238(player);
			this.RpcLogic___RemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x000D07F9 File Offset: 0x000CE9F9
		public void SetLocalPlayerBet(float bet)
		{
			if (!base.IsOpen)
			{
				return;
			}
			this.LocalPlayerBet = bet;
			if (this.onLocalPlayerBetChange != null)
			{
				this.onLocalPlayerBetChange();
			}
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x000D081E File Offset: 0x000CEA1E
		public bool AreAllPlayersReady()
		{
			return this.Players.CurrentPlayerCount != 0 && this.GetPlayersReadyCount() == this.Players.CurrentPlayerCount;
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x000D0844 File Offset: 0x000CEA44
		public int GetPlayersReadyCount()
		{
			int num = 0;
			for (int i = 0; i < this.Players.CurrentPlayerCount; i++)
			{
				if (!(this.Players.GetPlayer(i) == null) && this.Players.GetPlayerData(i).GetData<bool>("Ready"))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x000D089C File Offset: 0x000CEA9C
		public void ToggleLocalPlayerReady()
		{
			bool flag = base.LocalPlayerData.GetData<bool>("Ready");
			flag = !flag;
			base.LocalPlayerData.SetData<bool>("Ready", flag, true);
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x000D0958 File Offset: 0x000CEB58
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.BlackjackGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.BlackjackGameControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_StartGame_2166136261));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_NotifyPlayerScore_2864061566));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_SetRoundEnded_1140765316));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_AddCardToPlayerHand_2801973956));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_AddCardToDealerHand_3615296227));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_EndGame_2166136261));
			base.RegisterObserversRpc(6U, new ClientRpcDelegate(this.RpcReader___Observers_AddPlayerToCurrentRound_3323014238));
			base.RegisterServerRpc(7U, new ServerRpcDelegate(this.RpcReader___Server_RequestRemovePlayerFromCurrentRound_3323014238));
			base.RegisterObserversRpc(8U, new ClientRpcDelegate(this.RpcReader___Observers_RemovePlayerFromCurrentRound_3323014238));
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x000D0A4B File Offset: 0x000CEC4B
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.BlackjackGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.BlackjackGameControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x000D0A64 File Offset: 0x000CEC64
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000D0A74 File Offset: 0x000CEC74
		private void RpcWriter___Observers_StartGame_2166136261()
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
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000D0B20 File Offset: 0x000CED20
		private void RpcLogic___StartGame_2166136261()
		{
			BlackjackGameController.<>c__DisplayClass70_0 CS$<>8__locals1 = new BlackjackGameController.<>c__DisplayClass70_0();
			CS$<>8__locals1.<>4__this = this;
			this.ResetCards();
			this.CurrentStage = BlackjackGameController.EStage.Dealing;
			this.PlayerTurn = null;
			this.IsLocalPlayerBlackjack = false;
			this.IsLocalPlayerBust = false;
			if (InstanceFinder.IsServer)
			{
				this.SetRoundEnded(false);
			}
			if (this.IsLocalPlayerInCurrentRound)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.LocalPlayerBet, true, false);
				this.Players.SetPlayerScore(Player.Local, Mathf.RoundToInt(this.LocalPlayerBet));
				base.LocalPlayerData.SetData<bool>("Ready", false, true);
			}
			CS$<>8__locals1.clockwisePlayers = this.GetClockwisePlayers();
			if (this.gameRoutine != null)
			{
				Console.LogWarning("Game routine already running, stopping...", null);
				base.StopCoroutine(this.gameRoutine);
			}
			this.gameRoutine = Singleton<CoroutineService>.Instance.StartCoroutine(CS$<>8__locals1.<StartGame>g__GameRoutine|0());
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x000D0BF4 File Offset: 0x000CEDF4
		private void RpcReader___Observers_StartGame_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___StartGame_2166136261();
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x000D0C20 File Offset: 0x000CEE20
		private void RpcWriter___Observers_NotifyPlayerScore_2864061566(NetworkObject player, int score, bool blackjack)
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
			writer.WriteNetworkObject(player);
			writer.WriteInt32(score, AutoPackType.Packed);
			writer.WriteBoolean(blackjack);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x000D0CF8 File Offset: 0x000CEEF8
		private void RpcLogic___NotifyPlayerScore_2864061566(NetworkObject player, int score, bool blackjack)
		{
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (component.IsLocalPlayer)
			{
				this.LocalPlayerScore = score;
				this.IsLocalPlayerBlackjack = blackjack;
				if (score > 21)
				{
					this.IsLocalPlayerBust = true;
				}
			}
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x000D0D38 File Offset: 0x000CEF38
		private void RpcReader___Observers_NotifyPlayerScore_2864061566(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			int score = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool blackjack = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___NotifyPlayerScore_2864061566(player, score, blackjack);
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x000D0D9C File Offset: 0x000CEF9C
		private void RpcWriter___Observers_SetRoundEnded_1140765316(bool ended)
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
			writer.WriteBoolean(ended);
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000D0E52 File Offset: 0x000CF052
		private void RpcLogic___SetRoundEnded_1140765316(bool ended)
		{
			this.roundEnded = ended;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x000D0E5C File Offset: 0x000CF05C
		private void RpcReader___Observers_SetRoundEnded_1140765316(PooledReader PooledReader0, Channel channel)
		{
			bool ended = PooledReader0.ReadBoolean();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetRoundEnded_1140765316(ended);
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x000D0E98 File Offset: 0x000CF098
		private void RpcWriter___Observers_AddCardToPlayerHand_2801973956(int playerindex, string cardID)
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
			writer.WriteInt32(playerindex, AutoPackType.Packed);
			writer.WriteString(cardID);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x000D0F60 File Offset: 0x000CF160
		private void RpcLogic___AddCardToPlayerHand_2801973956(int playerindex, string cardID)
		{
			PlayingCard playingCard = this.Cards.FirstOrDefault((PlayingCard x) => x.CardID == cardID);
			if (playingCard == null)
			{
				return;
			}
			switch (playerindex)
			{
			case 0:
				if (!this.player1Hand.Contains(playingCard))
				{
					this.player1Hand.Add(playingCard);
					return;
				}
				break;
			case 1:
				if (!this.player2Hand.Contains(playingCard))
				{
					this.player2Hand.Add(playingCard);
					return;
				}
				break;
			case 2:
				if (!this.player3Hand.Contains(playingCard))
				{
					this.player3Hand.Add(playingCard);
					return;
				}
				break;
			case 3:
				if (!this.player4Hand.Contains(playingCard))
				{
					this.player4Hand.Add(playingCard);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x000D1020 File Offset: 0x000CF220
		private void RpcReader___Observers_AddCardToPlayerHand_2801973956(PooledReader PooledReader0, Channel channel)
		{
			int playerindex = PooledReader0.ReadInt32(AutoPackType.Packed);
			string cardID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddCardToPlayerHand_2801973956(playerindex, cardID);
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000D1074 File Offset: 0x000CF274
		private void RpcWriter___Observers_AddCardToDealerHand_3615296227(string cardID)
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
			writer.WriteString(cardID);
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x000D112C File Offset: 0x000CF32C
		private void RpcLogic___AddCardToDealerHand_3615296227(string cardID)
		{
			PlayingCard playingCard = this.Cards.FirstOrDefault((PlayingCard x) => x.CardID == cardID);
			if (playingCard == null)
			{
				return;
			}
			if (!this.dealerHand.Contains(playingCard))
			{
				this.dealerHand.Add(playingCard);
			}
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x000D1184 File Offset: 0x000CF384
		private void RpcReader___Observers_AddCardToDealerHand_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string cardID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddCardToDealerHand_3615296227(cardID);
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x000D11C0 File Offset: 0x000CF3C0
		private void RpcWriter___Observers_EndGame_2166136261()
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
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x000D1269 File Offset: 0x000CF469
		private void RpcLogic___EndGame_2166136261()
		{
			this.PlayerTurn = null;
			this.CurrentStage = BlackjackGameController.EStage.WaitingForPlayers;
			this.ResetCards();
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000D1280 File Offset: 0x000CF480
		private void RpcReader___Observers_EndGame_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EndGame_2166136261();
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x000D12AC File Offset: 0x000CF4AC
		private void RpcWriter___Observers_AddPlayerToCurrentRound_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendObserversRpc(6U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x000D1364 File Offset: 0x000CF564
		private void RpcLogic___AddPlayerToCurrentRound_3323014238(NetworkObject player)
		{
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			Console.Log("Adding player to current round: " + component.PlayerName, null);
			if (!this.playersInCurrentRound.Contains(component))
			{
				this.playersInCurrentRound.Add(component);
			}
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x000D13B4 File Offset: 0x000CF5B4
		private void RpcReader___Observers_AddPlayerToCurrentRound_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddPlayerToCurrentRound_3323014238(player);
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000D13F0 File Offset: 0x000CF5F0
		private void RpcWriter___Server_RequestRemovePlayerFromCurrentRound_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendServerRpc(7U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000D1497 File Offset: 0x000CF697
		private void RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(NetworkObject player)
		{
			this.RemovePlayerFromCurrentRound(player);
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x000D14A0 File Offset: 0x000CF6A0
		private void RpcReader___Server_RequestRemovePlayerFromCurrentRound_3323014238(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x000D14E0 File Offset: 0x000CF6E0
		private void RpcWriter___Observers_RemovePlayerFromCurrentRound_3323014238(NetworkObject player)
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
			writer.WriteNetworkObject(player);
			base.SendObserversRpc(8U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x000D1598 File Offset: 0x000CF798
		private void RpcLogic___RemovePlayerFromCurrentRound_3323014238(NetworkObject player)
		{
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			Console.Log("Removing player from current round: " + component.PlayerName, null);
			if (this.playersInCurrentRound.Contains(component))
			{
				this.playersInCurrentRound.Remove(component);
			}
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x000D15E8 File Offset: 0x000CF7E8
		private void RpcReader___Observers_RemovePlayerFromCurrentRound_3323014238(PooledReader PooledReader0, Channel channel)
		{
			NetworkObject player = PooledReader0.ReadNetworkObject();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x000D1623 File Offset: 0x000CF823
		protected virtual void dll()
		{
			base.Awake();
			this.ResetCards();
		}

		// Token: 0x040023F4 RID: 9204
		public const int BET_MINIMUM = 10;

		// Token: 0x040023F5 RID: 9205
		public const int BET_MAXIMUM = 1000;

		// Token: 0x040023F6 RID: 9206
		public const float PAYOUT_RATIO = 1f;

		// Token: 0x040023F7 RID: 9207
		public const float BLACKJACK_PAYOUT_RATIO = 1.5f;

		// Token: 0x040023FF RID: 9215
		[Header("References")]
		public PlayingCard[] Cards;

		// Token: 0x04002400 RID: 9216
		public Transform[] DefaultCardPositions;

		// Token: 0x04002401 RID: 9217
		public Transform[] FocusedCameraTransforms;

		// Token: 0x04002402 RID: 9218
		public Transform[] FinalCameraTransforms;

		// Token: 0x04002403 RID: 9219
		public Transform[] Player1CardPositions;

		// Token: 0x04002404 RID: 9220
		public Transform[] Player2CardPositions;

		// Token: 0x04002405 RID: 9221
		public Transform[] Player3CardPositions;

		// Token: 0x04002406 RID: 9222
		public Transform[] Player4CardPositions;

		// Token: 0x04002407 RID: 9223
		public Transform[] DealerCardPositions;

		// Token: 0x04002408 RID: 9224
		private List<Player> playersInCurrentRound = new List<Player>();

		// Token: 0x04002409 RID: 9225
		private List<PlayingCard> playStack = new List<PlayingCard>();

		// Token: 0x0400240A RID: 9226
		private List<PlayingCard> player1Hand = new List<PlayingCard>();

		// Token: 0x0400240B RID: 9227
		private List<PlayingCard> player2Hand = new List<PlayingCard>();

		// Token: 0x0400240C RID: 9228
		private List<PlayingCard> player3Hand = new List<PlayingCard>();

		// Token: 0x0400240D RID: 9229
		private List<PlayingCard> player4Hand = new List<PlayingCard>();

		// Token: 0x0400240E RID: 9230
		private List<PlayingCard> dealerHand = new List<PlayingCard>();

		// Token: 0x0400240F RID: 9231
		private List<PlayingCard.CardData> cardValuesInDeck = new List<PlayingCard.CardData>();

		// Token: 0x04002410 RID: 9232
		private List<PlayingCard.CardData> drawnCardsValues = new List<PlayingCard.CardData>();

		// Token: 0x04002411 RID: 9233
		protected Transform localFocusCameraTransform;

		// Token: 0x04002412 RID: 9234
		protected Transform localFinalCameraTransform;

		// Token: 0x04002413 RID: 9235
		public Action onLocalPlayerBetChange;

		// Token: 0x04002414 RID: 9236
		public Action onLocalPlayerExitRound;

		// Token: 0x04002415 RID: 9237
		public Action onInitialCardsDealt;

		// Token: 0x04002416 RID: 9238
		public Action onLocalPlayerReadyForInput;

		// Token: 0x04002417 RID: 9239
		public Action onLocalPlayerBust;

		// Token: 0x04002418 RID: 9240
		public Action<BlackjackGameController.EPayoutType> onLocalPlayerRoundCompleted;

		// Token: 0x04002419 RID: 9241
		private bool roundEnded;

		// Token: 0x0400241A RID: 9242
		private Coroutine gameRoutine;

		// Token: 0x0400241B RID: 9243
		private bool dll_Excuted;

		// Token: 0x0400241C RID: 9244
		private bool dll_Excuted;

		// Token: 0x0200073D RID: 1853
		public enum EStage
		{
			// Token: 0x0400241E RID: 9246
			WaitingForPlayers,
			// Token: 0x0400241F RID: 9247
			Dealing,
			// Token: 0x04002420 RID: 9248
			PlayerTurn,
			// Token: 0x04002421 RID: 9249
			DealerTurn,
			// Token: 0x04002422 RID: 9250
			Ending
		}

		// Token: 0x0200073E RID: 1854
		public enum EPayoutType
		{
			// Token: 0x04002424 RID: 9252
			None,
			// Token: 0x04002425 RID: 9253
			Blackjack,
			// Token: 0x04002426 RID: 9254
			Win,
			// Token: 0x04002427 RID: 9255
			Push
		}
	}
}
