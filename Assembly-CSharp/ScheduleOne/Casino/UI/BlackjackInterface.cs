using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Casino.UI
{
	// Token: 0x02000761 RID: 1889
	public class BlackjackInterface : Singleton<BlackjackInterface>
	{
		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060033A0 RID: 13216 RVA: 0x000D798E File Offset: 0x000D5B8E
		// (set) Token: 0x060033A1 RID: 13217 RVA: 0x000D7996 File Offset: 0x000D5B96
		public BlackjackGameController CurrentGame { get; private set; }

		// Token: 0x060033A2 RID: 13218 RVA: 0x000D79A0 File Offset: 0x000D5BA0
		protected override void Awake()
		{
			base.Awake();
			this.BetSlider.onValueChanged.AddListener(new UnityAction<float>(this.BetSliderChanged));
			this.ReadyButton.onClick.AddListener(new UnityAction(this.ReadyButtonClicked));
			this.HitButton.onClick.AddListener(new UnityAction(this.HitClicked));
			this.StandButton.onClick.AddListener(new UnityAction(this.StandClicked));
			this.InputContainerCanvasGroup.alpha = 0f;
			this.InputContainerCanvasGroup.interactable = false;
			this.ScoresContainerCanvasGroup.alpha = 0f;
			this.Canvas.enabled = false;
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x000D7A5C File Offset: 0x000D5C5C
		private void FixedUpdate()
		{
			if (this.CurrentGame == null)
			{
				return;
			}
			bool data = this.CurrentGame.LocalPlayerData.GetData<bool>("Ready");
			this.BetSlider.interactable = (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.WaitingForPlayers && !data);
			if (data)
			{
				this.BetTitleLabel.text = "Waiting for other players...";
			}
			else
			{
				this.BetTitleLabel.text = "Place your bet and press 'ready'";
			}
			if (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.WaitingForPlayers)
			{
				this.BetContainer.gameObject.SetActive(true);
				this.RefreshReadyButton();
			}
			else
			{
				this.BetContainer.gameObject.SetActive(false);
			}
			this.PlayerScoreLabel.text = this.CurrentGame.LocalPlayerScore.ToString();
			if (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.DealerTurn || this.CurrentGame.CurrentStage == BlackjackGameController.EStage.Ending)
			{
				this.DealerScoreLabel.text = this.CurrentGame.DealerScore.ToString();
			}
			else
			{
				this.DealerScoreLabel.text = this.CurrentGame.DealerScore.ToString() + "+?";
			}
			if (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.PlayerTurn && this.CurrentGame.PlayerTurn != null)
			{
				if (this.CurrentGame.PlayerTurn.IsLocalPlayer)
				{
					this.WaitingLabel.text = "Your turn!";
				}
				else
				{
					this.WaitingLabel.text = "Waiting for " + this.CurrentGame.PlayerTurn.PlayerName + "...";
				}
				this.WaitingContainer.gameObject.SetActive(true);
				return;
			}
			if (this.CurrentGame.CurrentStage == BlackjackGameController.EStage.DealerTurn)
			{
				this.WaitingLabel.text = "Dealer's turn...";
				this.WaitingContainer.gameObject.SetActive(true);
				return;
			}
			this.WaitingContainer.gameObject.SetActive(false);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x000D7C50 File Offset: 0x000D5E50
		public void Open(BlackjackGameController game)
		{
			this.CurrentGame = game;
			BlackjackGameController currentGame = this.CurrentGame;
			currentGame.onLocalPlayerBetChange = (Action)Delegate.Combine(currentGame.onLocalPlayerBetChange, new Action(this.RefreshDisplayedBet));
			BlackjackGameController currentGame2 = this.CurrentGame;
			currentGame2.onLocalPlayerExitRound = (Action)Delegate.Combine(currentGame2.onLocalPlayerExitRound, new Action(this.LocalPlayerExitRound));
			BlackjackGameController currentGame3 = this.CurrentGame;
			currentGame3.onInitialCardsDealt = (Action)Delegate.Combine(currentGame3.onInitialCardsDealt, new Action(this.ShowScores));
			BlackjackGameController currentGame4 = this.CurrentGame;
			currentGame4.onLocalPlayerReadyForInput = (Action)Delegate.Combine(currentGame4.onLocalPlayerReadyForInput, new Action(this.LocalPlayerReadyForInput));
			BlackjackGameController currentGame5 = this.CurrentGame;
			currentGame5.onLocalPlayerBust = (Action)Delegate.Combine(currentGame5.onLocalPlayerBust, new Action(this.OnLocalPlayerBust));
			BlackjackGameController currentGame6 = this.CurrentGame;
			currentGame6.onLocalPlayerRoundCompleted = (Action<BlackjackGameController.EPayoutType>)Delegate.Combine(currentGame6.onLocalPlayerRoundCompleted, new Action<BlackjackGameController.EPayoutType>(this.OnLocalPlayerRoundCompleted));
			this.PlayerDisplay.Bind(game.Players);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.Canvas.enabled = true;
			this.BetSlider.SetValueWithoutNotify(0f);
			game.SetLocalPlayerBet(10f);
			this.RefreshDisplayedBet();
			this.RefreshDisplayedBet();
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x000D7DA4 File Offset: 0x000D5FA4
		public void Close()
		{
			if (this.CurrentGame != null)
			{
				BlackjackGameController currentGame = this.CurrentGame;
				currentGame.onLocalPlayerBetChange = (Action)Delegate.Remove(currentGame.onLocalPlayerBetChange, new Action(this.RefreshDisplayedBet));
				BlackjackGameController currentGame2 = this.CurrentGame;
				currentGame2.onLocalPlayerExitRound = (Action)Delegate.Remove(currentGame2.onLocalPlayerExitRound, new Action(this.LocalPlayerExitRound));
				BlackjackGameController currentGame3 = this.CurrentGame;
				currentGame3.onInitialCardsDealt = (Action)Delegate.Remove(currentGame3.onInitialCardsDealt, new Action(this.ShowScores));
				BlackjackGameController currentGame4 = this.CurrentGame;
				currentGame4.onLocalPlayerReadyForInput = (Action)Delegate.Remove(currentGame4.onLocalPlayerReadyForInput, new Action(this.LocalPlayerReadyForInput));
				BlackjackGameController currentGame5 = this.CurrentGame;
				currentGame5.onLocalPlayerBust = (Action)Delegate.Remove(currentGame5.onLocalPlayerBust, new Action(this.OnLocalPlayerBust));
				BlackjackGameController currentGame6 = this.CurrentGame;
				currentGame6.onLocalPlayerRoundCompleted = (Action<BlackjackGameController.EPayoutType>)Delegate.Remove(currentGame6.onLocalPlayerRoundCompleted, new Action<BlackjackGameController.EPayoutType>(this.OnLocalPlayerRoundCompleted));
			}
			this.CurrentGame = null;
			this.PlayerDisplay.Unbind();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			this.Canvas.enabled = false;
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x000D7ED4 File Offset: 0x000D60D4
		private void BetSliderChanged(float newValue)
		{
			this.CurrentGame.SetLocalPlayerBet(this.GetBetFromSliderValue(newValue));
			this.RefreshDisplayedBet();
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x000D7EEE File Offset: 0x000D60EE
		private float GetBetFromSliderValue(float sliderVal)
		{
			return Mathf.Lerp(10f, 1000f, Mathf.Pow(sliderVal, 2f));
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000D7F0C File Offset: 0x000D610C
		private void RefreshDisplayedBet()
		{
			this.BetAmount.text = MoneyManager.FormatAmount(this.CurrentGame.LocalPlayerBet, false, false);
			this.BetSlider.SetValueWithoutNotify(Mathf.Sqrt(Mathf.InverseLerp(10f, 1000f, this.CurrentGame.LocalPlayerBet)));
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000D7F60 File Offset: 0x000D6160
		private void RefreshReadyButton()
		{
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= this.CurrentGame.LocalPlayerBet)
			{
				this.ReadyButton.interactable = true;
				this.BetAmount.color = new Color32(84, 231, 23, byte.MaxValue);
			}
			else
			{
				this.ReadyButton.interactable = false;
				this.BetAmount.color = new Color32(231, 52, 23, byte.MaxValue);
			}
			if (this.CurrentGame.LocalPlayerData.GetData<bool>("Ready"))
			{
				this.ReadyLabel.text = "Cancel";
				return;
			}
			this.ReadyLabel.text = "Ready";
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000D801C File Offset: 0x000D621C
		private void LocalPlayerReadyForInput()
		{
			this.SelectionIndicator.gameObject.SetActive(false);
			this.InputContainerCanvasGroup.interactable = true;
			this.InputContainerAnimation.Play(this.InputContainerFadeIn.name);
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x000D8052 File Offset: 0x000D6252
		private void ShowScores()
		{
			this.ScoresContainerAnimation.Play(this.InputContainerFadeIn.name);
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000D806B File Offset: 0x000D626B
		private void HideScores()
		{
			this.ScoresContainerAnimation.Play(this.InputContainerFadeOut.name);
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000D8084 File Offset: 0x000D6284
		private void HitClicked()
		{
			this.SelectionIndicator.transform.position = this.HitButton.transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
			this.CurrentGame.LocalPlayerData.SetData<float>("Action", 1f, true);
			this.InputContainerCanvasGroup.interactable = false;
			this.InputContainerAnimation.Play(this.InputContainerFadeOut.name);
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x000D8100 File Offset: 0x000D6300
		private void StandClicked()
		{
			this.SelectionIndicator.transform.position = this.StandButton.transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
			this.CurrentGame.LocalPlayerData.SetData<float>("Action", 2f, true);
			this.InputContainerCanvasGroup.interactable = false;
			this.InputContainerAnimation.Play(this.InputContainerFadeOut.name);
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000D817C File Offset: 0x000D637C
		private void LocalPlayerExitRound()
		{
			this.HideScores();
			if (this.InputContainerCanvasGroup.alpha > 0f)
			{
				this.InputContainerCanvasGroup.interactable = false;
				this.InputContainerAnimation.Play(this.InputContainerFadeOut.name);
			}
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000D81B9 File Offset: 0x000D63B9
		private void ReadyButtonClicked()
		{
			this.CurrentGame.ToggleLocalPlayerReady();
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000D81C6 File Offset: 0x000D63C6
		private void OnLocalPlayerBust()
		{
			if (this.onBust != null)
			{
				this.onBust.Invoke();
			}
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x000D81DC File Offset: 0x000D63DC
		private void OnLocalPlayerRoundCompleted(BlackjackGameController.EPayoutType payout)
		{
			float payout2 = this.CurrentGame.GetPayout(this.CurrentGame.LocalPlayerBet, payout);
			this.PayoutLabel.text = MoneyManager.FormatAmount(payout2, false, false);
			switch (payout)
			{
			case BlackjackGameController.EPayoutType.None:
				if (!this.CurrentGame.IsLocalPlayerBust && this.onLose != null)
				{
					this.onLose.Invoke();
					return;
				}
				break;
			case BlackjackGameController.EPayoutType.Blackjack:
				this.PositiveOutcomeLabel.text = "Blackjack!";
				if (this.onBlackjack != null)
				{
					this.onBlackjack.Invoke();
					return;
				}
				break;
			case BlackjackGameController.EPayoutType.Win:
				this.PositiveOutcomeLabel.text = "Win!";
				if (this.onWin != null)
				{
					this.onWin.Invoke();
					return;
				}
				break;
			case BlackjackGameController.EPayoutType.Push:
				if (this.onPush != null)
				{
					this.onPush.Invoke();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x040024F5 RID: 9461
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x040024F6 RID: 9462
		public CasinoGamePlayerDisplay PlayerDisplay;

		// Token: 0x040024F7 RID: 9463
		public RectTransform BetContainer;

		// Token: 0x040024F8 RID: 9464
		public TextMeshProUGUI BetTitleLabel;

		// Token: 0x040024F9 RID: 9465
		public Slider BetSlider;

		// Token: 0x040024FA RID: 9466
		public TextMeshProUGUI BetAmount;

		// Token: 0x040024FB RID: 9467
		public Button ReadyButton;

		// Token: 0x040024FC RID: 9468
		public TextMeshProUGUI ReadyLabel;

		// Token: 0x040024FD RID: 9469
		public RectTransform WaitingContainer;

		// Token: 0x040024FE RID: 9470
		public TextMeshProUGUI WaitingLabel;

		// Token: 0x040024FF RID: 9471
		public TextMeshProUGUI DealerScoreLabel;

		// Token: 0x04002500 RID: 9472
		public TextMeshProUGUI PlayerScoreLabel;

		// Token: 0x04002501 RID: 9473
		public Button HitButton;

		// Token: 0x04002502 RID: 9474
		public Button StandButton;

		// Token: 0x04002503 RID: 9475
		public Animation InputContainerAnimation;

		// Token: 0x04002504 RID: 9476
		public CanvasGroup InputContainerCanvasGroup;

		// Token: 0x04002505 RID: 9477
		public AnimationClip InputContainerFadeIn;

		// Token: 0x04002506 RID: 9478
		public AnimationClip InputContainerFadeOut;

		// Token: 0x04002507 RID: 9479
		public RectTransform SelectionIndicator;

		// Token: 0x04002508 RID: 9480
		public Animation ScoresContainerAnimation;

		// Token: 0x04002509 RID: 9481
		public CanvasGroup ScoresContainerCanvasGroup;

		// Token: 0x0400250A RID: 9482
		public TextMeshProUGUI PositiveOutcomeLabel;

		// Token: 0x0400250B RID: 9483
		public TextMeshProUGUI PayoutLabel;

		// Token: 0x0400250C RID: 9484
		public UnityEvent onBust;

		// Token: 0x0400250D RID: 9485
		public UnityEvent onBlackjack;

		// Token: 0x0400250E RID: 9486
		public UnityEvent onWin;

		// Token: 0x0400250F RID: 9487
		public UnityEvent onLose;

		// Token: 0x04002510 RID: 9488
		public UnityEvent onPush;
	}
}
