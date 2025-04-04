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
using ScheduleOne.Casino.UI;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Casino
{
	// Token: 0x02000754 RID: 1876
	public class RTBGameController : CasinoGameController
	{
		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06003308 RID: 13064 RVA: 0x000D5072 File Offset: 0x000D3272
		// (set) Token: 0x06003309 RID: 13065 RVA: 0x000D507A File Offset: 0x000D327A
		public RTBGameController.EStage CurrentStage { get; private set; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x0600330A RID: 13066 RVA: 0x000D5083 File Offset: 0x000D3283
		// (set) Token: 0x0600330B RID: 13067 RVA: 0x000D508B File Offset: 0x000D328B
		public bool IsQuestionActive { get; private set; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x0600330C RID: 13068 RVA: 0x000D5094 File Offset: 0x000D3294
		// (set) Token: 0x0600330D RID: 13069 RVA: 0x000D509C File Offset: 0x000D329C
		public float LocalPlayerBet { get; private set; } = 10f;

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x0600330E RID: 13070 RVA: 0x000D50A5 File Offset: 0x000D32A5
		// (set) Token: 0x0600330F RID: 13071 RVA: 0x000D50AD File Offset: 0x000D32AD
		public float LocalPlayerBetMultiplier { get; private set; } = 1f;

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06003310 RID: 13072 RVA: 0x000D50B6 File Offset: 0x000D32B6
		public float MultipliedLocalPlayerBet
		{
			get
			{
				return this.LocalPlayerBet * this.LocalPlayerBetMultiplier;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06003311 RID: 13073 RVA: 0x000D50C5 File Offset: 0x000D32C5
		// (set) Token: 0x06003312 RID: 13074 RVA: 0x000D50CD File Offset: 0x000D32CD
		public float RemainingAnswerTime { get; private set; } = 6f;

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x000D50D6 File Offset: 0x000D32D6
		public bool IsLocalPlayerInCurrentRound
		{
			get
			{
				return this.playersInCurrentRound.Contains(Player.Local);
			}
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x000D50E8 File Offset: 0x000D32E8
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Casino.RTBGameController_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x000D50FC File Offset: 0x000D32FC
		protected override void Open()
		{
			base.Open();
			Singleton<RTBInterface>.Instance.Open(this);
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x000D510F File Offset: 0x000D330F
		protected override void Close()
		{
			if (this.IsLocalPlayerInCurrentRound)
			{
				this.RemoveLocalPlayerFromGame(true, 0f);
			}
			Singleton<RTBInterface>.Instance.Close();
			base.Close();
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x000D5135 File Offset: 0x000D3335
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
				this.RemoveLocalPlayerFromGame(true, 0f);
			}
			base.Exit(action);
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x000D5174 File Offset: 0x000D3374
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (this.CurrentStage == RTBGameController.EStage.WaitingForPlayers && this.AreAllPlayersReady())
			{
				for (int i = 0; i < this.Players.CurrentPlayerCount; i++)
				{
					this.AddPlayerToCurrentRound(this.Players.GetPlayer(i).NetworkObject);
				}
				this.SetStage(RTBGameController.EStage.RedOrBlack);
			}
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x000D51D4 File Offset: 0x000D33D4
		[ObserversRpc(RunLocally = true)]
		private void SetStage(RTBGameController.EStage stage)
		{
			this.RpcWriter___Observers_SetStage_2502303021(stage);
			this.RpcLogic___SetStage_2502303021(stage);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x000D51F8 File Offset: 0x000D33F8
		private void RunRound(RTBGameController.EStage stage)
		{
			RTBGameController.<>c__DisplayClass50_0 CS$<>8__locals1 = new RTBGameController.<>c__DisplayClass50_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.stage = stage;
			this.SetBetMultiplier(RTBGameController.GetNetBetMultiplier(CS$<>8__locals1.stage - 1));
			base.StartCoroutine(CS$<>8__locals1.<RunRound>g__RunRound|0());
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x000D5239 File Offset: 0x000D3439
		[ObserversRpc(RunLocally = true)]
		private void SetBetMultiplier(float multiplier)
		{
			this.RpcWriter___Observers_SetBetMultiplier_431000436(multiplier);
			this.RpcLogic___SetBetMultiplier_431000436(multiplier);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x000D524F File Offset: 0x000D344F
		[ObserversRpc(RunLocally = true)]
		private void EndGame()
		{
			this.RpcWriter___Observers_EndGame_2166136261();
			this.RpcLogic___EndGame_2166136261();
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x000D5260 File Offset: 0x000D3460
		public void RemoveLocalPlayerFromGame(bool payout, float cameraDelay = 0f)
		{
			RTBGameController.<>c__DisplayClass53_0 CS$<>8__locals1 = new RTBGameController.<>c__DisplayClass53_0();
			CS$<>8__locals1.cameraDelay = cameraDelay;
			CS$<>8__locals1.<>4__this = this;
			this.RequestRemovePlayerFromCurrentRound(Player.Local.NetworkObject);
			this.Players.SetPlayerScore(Player.Local, 0);
			if (payout)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.LocalPlayerBet * this.LocalPlayerBetMultiplier, true, false);
			}
			if (this.onLocalPlayerExitRound != null)
			{
				this.onLocalPlayerExitRound();
			}
			base.StartCoroutine(CS$<>8__locals1.<RemoveLocalPlayerFromGame>g__Wait|0());
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x000D52DE File Offset: 0x000D34DE
		private bool IsCurrentRoundEmpty()
		{
			return this.playersInCurrentRound.Count == 0;
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x000D52F0 File Offset: 0x000D34F0
		private float GetAnswerIndex(RTBGameController.EStage stage, PlayingCard.CardData card)
		{
			if (stage == RTBGameController.EStage.RedOrBlack)
			{
				if (card.Suit == PlayingCard.ECardSuit.Hearts || card.Suit == PlayingCard.ECardSuit.Diamonds)
				{
					return 1f;
				}
				return 2f;
			}
			else if (stage == RTBGameController.EStage.HigherOrLower)
			{
				PlayingCard.CardData card2 = this.drawnCards[this.drawnCards.Count - 2];
				if (this.GetCardNumberValue(card) >= this.GetCardNumberValue(card2))
				{
					return 1f;
				}
				return 2f;
			}
			else
			{
				if (stage != RTBGameController.EStage.InsideOrOutside)
				{
					if (stage == RTBGameController.EStage.Suit)
					{
						switch (card.Suit)
						{
						case PlayingCard.ECardSuit.Spades:
							return 4f;
						case PlayingCard.ECardSuit.Hearts:
							return 1f;
						case PlayingCard.ECardSuit.Diamonds:
							return 3f;
						case PlayingCard.ECardSuit.Clubs:
							return 2f;
						}
					}
					Console.LogError("GetAnswerIndex not implemented for stage " + stage.ToString(), null);
					return 0f;
				}
				PlayingCard.CardData card3 = this.drawnCards[this.drawnCards.Count - 2];
				PlayingCard.CardData card4 = this.drawnCards[this.drawnCards.Count - 3];
				int num = Mathf.Min(this.GetCardNumberValue(card3), this.GetCardNumberValue(card4));
				int num2 = Mathf.Max(this.GetCardNumberValue(card3), this.GetCardNumberValue(card4));
				int cardNumberValue = this.GetCardNumberValue(card);
				if (cardNumberValue >= num && cardNumberValue <= num2)
				{
					return 1f;
				}
				return 2f;
			}
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x000D5438 File Offset: 0x000D3638
		[ObserversRpc(RunLocally = true)]
		private void NotifyAnswer(float answerIndex)
		{
			this.RpcWriter___Observers_NotifyAnswer_431000436(answerIndex);
			this.RpcLogic___NotifyAnswer_431000436(answerIndex);
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x000D545C File Offset: 0x000D365C
		[ObserversRpc(RunLocally = true)]
		private void QuestionDone()
		{
			this.RpcWriter___Observers_QuestionDone_2166136261();
			this.RpcLogic___QuestionDone_2166136261();
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x000D5478 File Offset: 0x000D3678
		private void GetQuestionsAndAnswers(RTBGameController.EStage stage, out string question, out string[] answers)
		{
			question = "";
			answers = new string[0];
			if (stage == RTBGameController.EStage.RedOrBlack)
			{
				question = "What will the next card be?";
				answers = new string[]
				{
					"Red",
					"Black"
				};
			}
			if (stage == RTBGameController.EStage.HigherOrLower)
			{
				question = "Will the next card be higher or lower?";
				answers = new string[]
				{
					"Higher",
					"Lower"
				};
			}
			if (stage == RTBGameController.EStage.InsideOrOutside)
			{
				question = "Will the next card be inside or outside?";
				answers = new string[]
				{
					"Inside",
					"Outside"
				};
			}
			if (stage == RTBGameController.EStage.Suit)
			{
				question = "What will the suit of the next card be?";
				answers = new string[]
				{
					"Hearts",
					"Clubs",
					"Diamonds",
					"Spades"
				};
			}
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x000D5530 File Offset: 0x000D3730
		private void ResetCards()
		{
			for (int i = 0; i < this.Cards.Length; i++)
			{
				this.Cards[i].SetFaceUp(false, true);
				this.Cards[i].GlideTo(this.CardDefaultPositions[i].position, this.CardDefaultPositions[i].rotation, 0.5f, true);
			}
			this.cardsInDeck = new List<PlayingCard.CardData>();
			for (int j = 0; j < 4; j++)
			{
				for (int k = 0; k < 13; k++)
				{
					PlayingCard.CardData item = default(PlayingCard.CardData);
					item.Suit = (PlayingCard.ECardSuit)j;
					item.Value = k + PlayingCard.ECardValue.Ace;
					this.cardsInDeck.Add(item);
				}
			}
			this.drawnCards.Clear();
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x000D55E4 File Offset: 0x000D37E4
		[ObserversRpc(RunLocally = true)]
		private void AddPlayerToCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Observers_AddPlayerToCurrentRound_3323014238(player);
			this.RpcLogic___AddPlayerToCurrentRound_3323014238(player);
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x000D5605 File Offset: 0x000D3805
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void RequestRemovePlayerFromCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Server_RequestRemovePlayerFromCurrentRound_3323014238(player);
			this.RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x000D561C File Offset: 0x000D381C
		[ObserversRpc(RunLocally = true)]
		private void RemovePlayerFromCurrentRound(NetworkObject player)
		{
			this.RpcWriter___Observers_RemovePlayerFromCurrentRound_3323014238(player);
			this.RpcLogic___RemovePlayerFromCurrentRound_3323014238(player);
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x000D5640 File Offset: 0x000D3840
		private PlayingCard.CardData PullCardFromDeck()
		{
			PlayingCard.CardData cardData = this.cardsInDeck[UnityEngine.Random.Range(0, this.cardsInDeck.Count)];
			this.cardsInDeck.Remove(cardData);
			this.drawnCards.Add(cardData);
			return cardData;
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x000D5684 File Offset: 0x000D3884
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

		// Token: 0x06003329 RID: 13097 RVA: 0x000D56A9 File Offset: 0x000D38A9
		public bool AreAllPlayersReady()
		{
			return this.Players.CurrentPlayerCount != 0 && this.GetPlayersReadyCount() == this.Players.CurrentPlayerCount;
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x000D56D0 File Offset: 0x000D38D0
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

		// Token: 0x0600332B RID: 13099 RVA: 0x000D5726 File Offset: 0x000D3926
		public void SetLocalPlayerAnswer(float answer)
		{
			base.LocalPlayerData.SetData<float>("Answer", answer, true);
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x000D573C File Offset: 0x000D393C
		public int GetAnsweredPlayersCount()
		{
			int num = 0;
			for (int i = 0; i < this.Players.CurrentPlayerCount; i++)
			{
				if (!(this.Players.GetPlayer(i) == null) && this.playersInCurrentRound.Contains(this.Players.GetPlayer(i)) && this.Players.GetPlayerData(i).GetData<float>("Answer") > 0.1f)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x000D57B0 File Offset: 0x000D39B0
		public void ToggleLocalPlayerReady()
		{
			bool flag = base.LocalPlayerData.GetData<bool>("Ready");
			flag = !flag;
			base.LocalPlayerData.SetData<bool>("Ready", flag, true);
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x000D57E5 File Offset: 0x000D39E5
		private int GetCardNumberValue(PlayingCard.CardData card)
		{
			if (card.Value == PlayingCard.ECardValue.Ace)
			{
				return 14;
			}
			return (int)card.Value;
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x000D57F9 File Offset: 0x000D39F9
		public static float GetNetBetMultiplier(RTBGameController.EStage stage)
		{
			if (stage == RTBGameController.EStage.RedOrBlack)
			{
				return 2f;
			}
			if (stage == RTBGameController.EStage.HigherOrLower)
			{
				return 3f;
			}
			if (stage == RTBGameController.EStage.InsideOrOutside)
			{
				return 4f;
			}
			if (stage == RTBGameController.EStage.Suit)
			{
				return 20f;
			}
			return 1f;
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x000D5880 File Offset: 0x000D3A80
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Casino.RTBGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Casino.RTBGameControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			base.RegisterObserversRpc(0U, new ClientRpcDelegate(this.RpcReader___Observers_SetStage_2502303021));
			base.RegisterObserversRpc(1U, new ClientRpcDelegate(this.RpcReader___Observers_SetBetMultiplier_431000436));
			base.RegisterObserversRpc(2U, new ClientRpcDelegate(this.RpcReader___Observers_EndGame_2166136261));
			base.RegisterObserversRpc(3U, new ClientRpcDelegate(this.RpcReader___Observers_NotifyAnswer_431000436));
			base.RegisterObserversRpc(4U, new ClientRpcDelegate(this.RpcReader___Observers_QuestionDone_2166136261));
			base.RegisterObserversRpc(5U, new ClientRpcDelegate(this.RpcReader___Observers_AddPlayerToCurrentRound_3323014238));
			base.RegisterServerRpc(6U, new ServerRpcDelegate(this.RpcReader___Server_RequestRemovePlayerFromCurrentRound_3323014238));
			base.RegisterObserversRpc(7U, new ClientRpcDelegate(this.RpcReader___Observers_RemovePlayerFromCurrentRound_3323014238));
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x000D595C File Offset: 0x000D3B5C
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Casino.RTBGameControllerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Casino.RTBGameControllerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x000D5975 File Offset: 0x000D3B75
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x000D5984 File Offset: 0x000D3B84
		private void RpcWriter___Observers_SetStage_2502303021(RTBGameController.EStage stage)
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
			writer.Write___ScheduleOne.Casino.RTBGameController/EStageFishNet.Serializing.Generated(stage);
			base.SendObserversRpc(0U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000D5A3C File Offset: 0x000D3C3C
		private void RpcLogic___SetStage_2502303021(RTBGameController.EStage stage)
		{
			this.CurrentStage = stage;
			if (!this.IsLocalPlayerInCurrentRound && !InstanceFinder.IsServer)
			{
				return;
			}
			if (stage == RTBGameController.EStage.RedOrBlack)
			{
				this.RunRound(RTBGameController.EStage.RedOrBlack);
			}
			if (stage == RTBGameController.EStage.HigherOrLower)
			{
				this.RunRound(RTBGameController.EStage.HigherOrLower);
			}
			if (stage == RTBGameController.EStage.InsideOrOutside)
			{
				this.RunRound(RTBGameController.EStage.InsideOrOutside);
			}
			if (stage == RTBGameController.EStage.Suit)
			{
				this.RunRound(RTBGameController.EStage.Suit);
			}
			if (this.onStageChange != null)
			{
				this.onStageChange(stage);
			}
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x000D5AA0 File Offset: 0x000D3CA0
		private void RpcReader___Observers_SetStage_2502303021(PooledReader PooledReader0, Channel channel)
		{
			RTBGameController.EStage stage = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.Casino.RTBGameController/EStageFishNet.Serializing.Generateds(PooledReader0);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStage_2502303021(stage);
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000D5ADC File Offset: 0x000D3CDC
		private void RpcWriter___Observers_SetBetMultiplier_431000436(float multiplier)
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
			writer.WriteSingle(multiplier, AutoPackType.Unpacked);
			base.SendObserversRpc(1U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x000D5B97 File Offset: 0x000D3D97
		private void RpcLogic___SetBetMultiplier_431000436(float multiplier)
		{
			this.LocalPlayerBetMultiplier = multiplier;
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x000D5BA0 File Offset: 0x000D3DA0
		private void RpcReader___Observers_SetBetMultiplier_431000436(PooledReader PooledReader0, Channel channel)
		{
			float multiplier = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetBetMultiplier_431000436(multiplier);
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x000D5BE0 File Offset: 0x000D3DE0
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
			base.SendObserversRpc(2U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x000D5C89 File Offset: 0x000D3E89
		private void RpcLogic___EndGame_2166136261()
		{
			if (this.IsLocalPlayerInCurrentRound)
			{
				this.RemoveLocalPlayerFromGame(true, 0f);
			}
			this.ResetCards();
			this.SetStage(RTBGameController.EStage.WaitingForPlayers);
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x000D5CAC File Offset: 0x000D3EAC
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

		// Token: 0x0600333D RID: 13117 RVA: 0x000D5CD8 File Offset: 0x000D3ED8
		private void RpcWriter___Observers_NotifyAnswer_431000436(float answerIndex)
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
			writer.WriteSingle(answerIndex, AutoPackType.Unpacked);
			base.SendObserversRpc(3U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x000D5D94 File Offset: 0x000D3F94
		private void RpcLogic___NotifyAnswer_431000436(float answerIndex)
		{
			if (!this.IsLocalPlayerInCurrentRound)
			{
				return;
			}
			if (base.LocalPlayerData.GetData<float>("Answer") == answerIndex)
			{
				Console.Log("Correct answer!", null);
				this.Players.SetPlayerScore(Player.Local, Mathf.RoundToInt(this.MultipliedLocalPlayerBet));
				if (this.onLocalPlayerCorrect != null)
				{
					this.onLocalPlayerCorrect();
					return;
				}
			}
			else
			{
				Console.Log("Incorrect answer!", null);
				this.RemoveLocalPlayerFromGame(false, 2f);
				if (this.onLocalPlayerIncorrect != null)
				{
					this.onLocalPlayerIncorrect();
				}
			}
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x000D5E24 File Offset: 0x000D4024
		private void RpcReader___Observers_NotifyAnswer_431000436(PooledReader PooledReader0, Channel channel)
		{
			float answerIndex = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___NotifyAnswer_431000436(answerIndex);
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x000D5E64 File Offset: 0x000D4064
		private void RpcWriter___Observers_QuestionDone_2166136261()
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
			base.SendObserversRpc(4U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000D5F10 File Offset: 0x000D4110
		private void RpcLogic___QuestionDone_2166136261()
		{
			if (!this.IsLocalPlayerInCurrentRound)
			{
				return;
			}
			if (!this.IsQuestionActive)
			{
				return;
			}
			if (base.LocalPlayerData.GetData<float>("Answer") == 0f)
			{
				this.SetLocalPlayerAnswer(1f);
			}
			this.IsQuestionActive = false;
			if (this.onQuestionDone != null)
			{
				this.onQuestionDone();
			}
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x000D5F6C File Offset: 0x000D416C
		private void RpcReader___Observers_QuestionDone_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___QuestionDone_2166136261();
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x000D5F98 File Offset: 0x000D4198
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
			base.SendObserversRpc(5U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000D6050 File Offset: 0x000D4250
		private void RpcLogic___AddPlayerToCurrentRound_3323014238(NetworkObject player)
		{
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (!this.playersInCurrentRound.Contains(component))
			{
				this.playersInCurrentRound.Add(component);
			}
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x000D6088 File Offset: 0x000D4288
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

		// Token: 0x06003346 RID: 13126 RVA: 0x000D60C4 File Offset: 0x000D42C4
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
			base.SendServerRpc(6U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x000D616B File Offset: 0x000D436B
		private void RpcLogic___RequestRemovePlayerFromCurrentRound_3323014238(NetworkObject player)
		{
			this.RemovePlayerFromCurrentRound(player);
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x000D6174 File Offset: 0x000D4374
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

		// Token: 0x06003349 RID: 13129 RVA: 0x000D61B4 File Offset: 0x000D43B4
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
			base.SendObserversRpc(7U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x000D626C File Offset: 0x000D446C
		private void RpcLogic___RemovePlayerFromCurrentRound_3323014238(NetworkObject player)
		{
			if (player == null)
			{
				return;
			}
			Player component = player.GetComponent<Player>();
			if (component == null)
			{
				return;
			}
			if (this.playersInCurrentRound.Contains(component))
			{
				this.playersInCurrentRound.Remove(component);
			}
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000D62B0 File Offset: 0x000D44B0
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

		// Token: 0x0600334C RID: 13132 RVA: 0x000D62EB File Offset: 0x000D44EB
		protected virtual void dll()
		{
			base.Awake();
			this.ResetCards();
		}

		// Token: 0x04002490 RID: 9360
		public const int BET_MINIMUM = 10;

		// Token: 0x04002491 RID: 9361
		public const int BET_MAXIMUM = 500;

		// Token: 0x04002492 RID: 9362
		public const float ANSWER_MAX_TIME = 6f;

		// Token: 0x04002493 RID: 9363
		[Header("References")]
		public Transform PlayCameraTransform;

		// Token: 0x04002494 RID: 9364
		public Transform FocusedCameraTransform;

		// Token: 0x04002495 RID: 9365
		public PlayingCard[] Cards;

		// Token: 0x04002496 RID: 9366
		public Transform[] CardDefaultPositions;

		// Token: 0x04002497 RID: 9367
		public Transform ActiveCardPosition;

		// Token: 0x04002498 RID: 9368
		public Transform[] DockedCardPositions;

		// Token: 0x0400249A RID: 9370
		public Action<RTBGameController.EStage> onStageChange;

		// Token: 0x0400249B RID: 9371
		public Action<string, string[]> onQuestionReady;

		// Token: 0x0400249C RID: 9372
		public Action onQuestionDone;

		// Token: 0x0400249D RID: 9373
		public Action onLocalPlayerCorrect;

		// Token: 0x0400249E RID: 9374
		public Action onLocalPlayerIncorrect;

		// Token: 0x0400249F RID: 9375
		public Action onLocalPlayerBetChange;

		// Token: 0x040024A0 RID: 9376
		public Action onLocalPlayerExitRound;

		// Token: 0x040024A5 RID: 9381
		private List<Player> playersInCurrentRound = new List<Player>();

		// Token: 0x040024A6 RID: 9382
		private List<PlayingCard.CardData> cardsInDeck = new List<PlayingCard.CardData>();

		// Token: 0x040024A7 RID: 9383
		private List<PlayingCard.CardData> drawnCards = new List<PlayingCard.CardData>();

		// Token: 0x040024A8 RID: 9384
		private bool dll_Excuted;

		// Token: 0x040024A9 RID: 9385
		private bool dll_Excuted;

		// Token: 0x02000755 RID: 1877
		public enum EStage
		{
			// Token: 0x040024AB RID: 9387
			WaitingForPlayers,
			// Token: 0x040024AC RID: 9388
			RedOrBlack,
			// Token: 0x040024AD RID: 9389
			HigherOrLower,
			// Token: 0x040024AE RID: 9390
			InsideOrOutside,
			// Token: 0x040024AF RID: 9391
			Suit
		}
	}
}
