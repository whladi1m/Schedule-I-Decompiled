using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.Casino.UI
{
	// Token: 0x02000763 RID: 1891
	public class RTBInterface : Singleton<RTBInterface>
	{
		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x060033B9 RID: 13241 RVA: 0x000D84BF File Offset: 0x000D66BF
		// (set) Token: 0x060033BA RID: 13242 RVA: 0x000D84C7 File Offset: 0x000D66C7
		public RTBGameController CurrentGame { get; private set; }

		// Token: 0x060033BB RID: 13243 RVA: 0x000D84D0 File Offset: 0x000D66D0
		protected override void Awake()
		{
			base.Awake();
			this.BetSlider.onValueChanged.AddListener(new UnityAction<float>(this.BetSliderChanged));
			this.ReadyButton.onClick.AddListener(new UnityAction(this.ReadyButtonClicked));
			for (int i = 0; i < this.AnswerButtons.Length; i++)
			{
				int index = i;
				this.AnswerButtons[i].onClick.AddListener(delegate()
				{
					this.AnswerButtonClicked(index);
				});
			}
			this.ForfeitButton.onClick.AddListener(new UnityAction(this.ForfeitClicked));
			this.QuestionCanvasGroup.alpha = 0f;
			this.QuestionCanvasGroup.interactable = false;
			this.Canvas.enabled = false;
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000D85A4 File Offset: 0x000D67A4
		private void FixedUpdate()
		{
			if (this.CurrentGame == null)
			{
				return;
			}
			this.StatusLabel.text = this.GetStatusText();
			bool data = this.CurrentGame.LocalPlayerData.GetData<bool>("Ready");
			this.BetSlider.interactable = (this.CurrentGame.CurrentStage == RTBGameController.EStage.WaitingForPlayers && !data);
			if (data)
			{
				this.BetTitleLabel.text = "Waiting for other players...";
			}
			else
			{
				this.BetTitleLabel.text = "Place your bet and press 'ready'";
			}
			if (this.CurrentGame.CurrentStage == RTBGameController.EStage.WaitingForPlayers)
			{
				this.BetContainer.gameObject.SetActive(true);
				this.RefreshReadyButton();
				return;
			}
			this.BetContainer.gameObject.SetActive(false);
		}

		// Token: 0x060033BD RID: 13245 RVA: 0x000D8664 File Offset: 0x000D6864
		private string GetStatusText()
		{
			switch (this.CurrentGame.CurrentStage)
			{
			case RTBGameController.EStage.WaitingForPlayers:
				return string.Concat(new string[]
				{
					"Waiting for players... (",
					this.CurrentGame.GetPlayersReadyCount().ToString(),
					"/",
					this.CurrentGame.Players.CurrentPlayerCount.ToString(),
					")"
				});
			case RTBGameController.EStage.RedOrBlack:
				return "Round 1\nPredict if the next card will be red or black.\nYou can also forfeit and cash out.";
			case RTBGameController.EStage.HigherOrLower:
				return "Round 2\nPredict if the next card will be higher or lower than the previous card.\nYou can also forfeit and cash out.";
			case RTBGameController.EStage.InsideOrOutside:
				return "Round 3\nPredict if the next card will be inside or outside the previous two cards (Ace counts as 11).\nYou can also forfeit and cash out.";
			case RTBGameController.EStage.Suit:
				return "Round 4\nPredict the suit of the next card.\nYou can also forfeit and cash out.";
			default:
				return "Unknown";
			}
		}

		// Token: 0x060033BE RID: 13246 RVA: 0x000D870C File Offset: 0x000D690C
		public void Open(RTBGameController game)
		{
			this.CurrentGame = game;
			RTBGameController currentGame = this.CurrentGame;
			currentGame.onQuestionReady = (Action<string, string[]>)Delegate.Combine(currentGame.onQuestionReady, new Action<string, string[]>(this.QuestionReady));
			RTBGameController currentGame2 = this.CurrentGame;
			currentGame2.onQuestionDone = (Action)Delegate.Combine(currentGame2.onQuestionDone, new Action(this.QuestionDone));
			RTBGameController currentGame3 = this.CurrentGame;
			currentGame3.onLocalPlayerCorrect = (Action)Delegate.Combine(currentGame3.onLocalPlayerCorrect, new Action(this.Correct));
			RTBGameController currentGame4 = this.CurrentGame;
			currentGame4.onLocalPlayerIncorrect = (Action)Delegate.Combine(currentGame4.onLocalPlayerIncorrect, new Action(this.Incorrect));
			RTBGameController currentGame5 = this.CurrentGame;
			currentGame5.onLocalPlayerBetChange = (Action)Delegate.Combine(currentGame5.onLocalPlayerBetChange, new Action(this.RefreshDisplayedBet));
			RTBGameController currentGame6 = this.CurrentGame;
			currentGame6.onLocalPlayerExitRound = (Action)Delegate.Combine(currentGame6.onLocalPlayerExitRound, new Action(this.LocalPlayerExitRound));
			this.PlayerDisplay.Bind(game.Players);
			Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
			this.Canvas.enabled = true;
			this.BetSlider.SetValueWithoutNotify(0f);
			game.SetLocalPlayerBet(10f);
			this.RefreshDisplayedBet();
			this.RefreshDisplayedBet();
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x000D8860 File Offset: 0x000D6A60
		public void Close()
		{
			if (this.CurrentGame != null)
			{
				RTBGameController currentGame = this.CurrentGame;
				currentGame.onQuestionReady = (Action<string, string[]>)Delegate.Remove(currentGame.onQuestionReady, new Action<string, string[]>(this.QuestionReady));
				RTBGameController currentGame2 = this.CurrentGame;
				currentGame2.onQuestionDone = (Action)Delegate.Remove(currentGame2.onQuestionDone, new Action(this.QuestionDone));
				RTBGameController currentGame3 = this.CurrentGame;
				currentGame3.onLocalPlayerCorrect = (Action)Delegate.Remove(currentGame3.onLocalPlayerCorrect, new Action(this.Correct));
				RTBGameController currentGame4 = this.CurrentGame;
				currentGame4.onLocalPlayerIncorrect = (Action)Delegate.Remove(currentGame4.onLocalPlayerIncorrect, new Action(this.Incorrect));
				RTBGameController currentGame5 = this.CurrentGame;
				currentGame5.onLocalPlayerBetChange = (Action)Delegate.Remove(currentGame5.onLocalPlayerBetChange, new Action(this.RefreshDisplayedBet));
				RTBGameController currentGame6 = this.CurrentGame;
				currentGame6.onLocalPlayerExitRound = (Action)Delegate.Remove(currentGame6.onLocalPlayerExitRound, new Action(this.LocalPlayerExitRound));
			}
			this.CurrentGame = null;
			this.PlayerDisplay.Unbind();
			Singleton<InputPromptsCanvas>.Instance.UnloadModule();
			this.Canvas.enabled = false;
		}

		// Token: 0x060033C0 RID: 13248 RVA: 0x000D8990 File Offset: 0x000D6B90
		private void BetSliderChanged(float newValue)
		{
			this.CurrentGame.SetLocalPlayerBet(this.GetBetFromSliderValue(newValue));
			this.RefreshDisplayedBet();
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x000D89AA File Offset: 0x000D6BAA
		private float GetBetFromSliderValue(float sliderVal)
		{
			return Mathf.Lerp(10f, 500f, Mathf.Pow(sliderVal, 2f));
		}

		// Token: 0x060033C2 RID: 13250 RVA: 0x000D89C8 File Offset: 0x000D6BC8
		private void RefreshDisplayedBet()
		{
			this.BetAmount.text = MoneyManager.FormatAmount(this.CurrentGame.LocalPlayerBet, false, false);
			this.BetSlider.SetValueWithoutNotify(Mathf.Sqrt(Mathf.InverseLerp(10f, 500f, this.CurrentGame.LocalPlayerBet)));
		}

		// Token: 0x060033C3 RID: 13251 RVA: 0x000D8A1C File Offset: 0x000D6C1C
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

		// Token: 0x060033C4 RID: 13252 RVA: 0x000D8AD8 File Offset: 0x000D6CD8
		private void QuestionReady(string question, string[] answers)
		{
			this.QuestionLabel.text = question;
			this.SelectionIndicator.gameObject.SetActive(false);
			this.ForfeitLabel.text = "Forfeit and collect " + MoneyManager.FormatAmount(this.CurrentGame.MultipliedLocalPlayerBet, false, true);
			this.QuestionCanvasGroup.interactable = true;
			for (int i = 0; i < this.AnswerButtons.Length; i++)
			{
				if (answers.Length > i)
				{
					this.AnswerLabels[i].text = answers[i];
					this.AnswerButtons[i].gameObject.SetActive(true);
				}
				else
				{
					this.AnswerButtons[i].gameObject.SetActive(false);
				}
			}
			this.QuestionContainerAnimation.Play(this.QuestionContainerFadeIn.name);
			this.TimerSlider.value = 1f;
			base.StartCoroutine(this.<QuestionReady>g__Routine|38_0());
		}

		// Token: 0x060033C5 RID: 13253 RVA: 0x000D8BBC File Offset: 0x000D6DBC
		private void AnswerButtonClicked(int index)
		{
			this.SelectionIndicator.transform.position = this.AnswerButtons[index].transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
			this.CurrentGame.SetLocalPlayerAnswer((float)index + 1f);
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000D8C10 File Offset: 0x000D6E10
		private void ForfeitClicked()
		{
			this.SelectionIndicator.transform.position = this.ForfeitButton.transform.position;
			this.SelectionIndicator.gameObject.SetActive(true);
			this.CurrentGame.RemoveLocalPlayerFromGame(true, 0f);
			this.QuestionDone();
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x000D8C65 File Offset: 0x000D6E65
		private void QuestionDone()
		{
			this.QuestionCanvasGroup.interactable = false;
			this.QuestionContainerAnimation.Play(this.QuestionContainerFadeOut.name);
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x000D8C8A File Offset: 0x000D6E8A
		private void LocalPlayerExitRound()
		{
			this.QuestionCanvasGroup.interactable = false;
			if (this.QuestionCanvasGroup.alpha > 0f)
			{
				this.QuestionContainerAnimation.Stop();
				this.QuestionCanvasGroup.alpha = 0f;
			}
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x000D8CC8 File Offset: 0x000D6EC8
		private void Correct()
		{
			this.WinningsMultiplierLabel.text = Mathf.RoundToInt(this.CurrentGame.LocalPlayerBetMultiplier).ToString() + "x";
			if (this.CurrentGame.CurrentStage == RTBGameController.EStage.Suit)
			{
				if (this.onFinalCorrect != null)
				{
					this.onFinalCorrect.Invoke();
					return;
				}
			}
			else if (this.onCorrect != null)
			{
				this.onCorrect.Invoke();
			}
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x000D8D37 File Offset: 0x000D6F37
		private void Incorrect()
		{
			if (this.onIncorrect != null)
			{
				this.onIncorrect.Invoke();
			}
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000D8D4C File Offset: 0x000D6F4C
		private void ReadyButtonClicked()
		{
			this.CurrentGame.ToggleLocalPlayerReady();
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x000D8D61 File Offset: 0x000D6F61
		[CompilerGenerated]
		private IEnumerator <QuestionReady>g__Routine|38_0()
		{
			while (this.CurrentGame != null && this.CurrentGame.IsQuestionActive)
			{
				this.TimerSlider.value = this.CurrentGame.RemainingAnswerTime / 6f;
				yield return new WaitForEndOfFrame();
			}
			yield break;
		}

		// Token: 0x04002515 RID: 9493
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x04002516 RID: 9494
		public CasinoGamePlayerDisplay PlayerDisplay;

		// Token: 0x04002517 RID: 9495
		public TextMeshProUGUI StatusLabel;

		// Token: 0x04002518 RID: 9496
		public RectTransform BetContainer;

		// Token: 0x04002519 RID: 9497
		public TextMeshProUGUI BetTitleLabel;

		// Token: 0x0400251A RID: 9498
		public Slider BetSlider;

		// Token: 0x0400251B RID: 9499
		public TextMeshProUGUI BetAmount;

		// Token: 0x0400251C RID: 9500
		public Button ReadyButton;

		// Token: 0x0400251D RID: 9501
		public TextMeshProUGUI ReadyLabel;

		// Token: 0x0400251E RID: 9502
		public TextMeshProUGUI WinningsMultiplierLabel;

		// Token: 0x0400251F RID: 9503
		[Header("Question and answers")]
		public RectTransform QuestionContainer;

		// Token: 0x04002520 RID: 9504
		public TextMeshProUGUI QuestionLabel;

		// Token: 0x04002521 RID: 9505
		public Slider TimerSlider;

		// Token: 0x04002522 RID: 9506
		public Button[] AnswerButtons;

		// Token: 0x04002523 RID: 9507
		public TextMeshProUGUI[] AnswerLabels;

		// Token: 0x04002524 RID: 9508
		public Button ForfeitButton;

		// Token: 0x04002525 RID: 9509
		public TextMeshProUGUI ForfeitLabel;

		// Token: 0x04002526 RID: 9510
		public Animation QuestionContainerAnimation;

		// Token: 0x04002527 RID: 9511
		public AnimationClip QuestionContainerFadeIn;

		// Token: 0x04002528 RID: 9512
		public AnimationClip QuestionContainerFadeOut;

		// Token: 0x04002529 RID: 9513
		public CanvasGroup QuestionCanvasGroup;

		// Token: 0x0400252A RID: 9514
		public RectTransform SelectionIndicator;

		// Token: 0x0400252B RID: 9515
		public UnityEvent onCorrect;

		// Token: 0x0400252C RID: 9516
		public UnityEvent onFinalCorrect;

		// Token: 0x0400252D RID: 9517
		public UnityEvent onIncorrect;
	}
}
