using System;
using System.Collections;
using System.Collections.Generic;
using ScheduleOne.Audio;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.ATM
{
	// Token: 0x02000B3E RID: 2878
	public class ATMInterface : MonoBehaviour
	{
		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06004C81 RID: 19585 RVA: 0x0014200A File Offset: 0x0014020A
		// (set) Token: 0x06004C82 RID: 19586 RVA: 0x00142012 File Offset: 0x00140212
		public bool isOpen { get; protected set; }

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06004C83 RID: 19587 RVA: 0x0014201B File Offset: 0x0014021B
		private float relevantBalance
		{
			get
			{
				if (!this.depositing)
				{
					return NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance;
				}
				return NetworkSingleton<MoneyManager>.Instance.cashBalance;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06004C84 RID: 19588 RVA: 0x0014203A File Offset: 0x0014023A
		private static float remainingAllowedDeposit
		{
			get
			{
				return 10000f - ATM.WeeklyDepositSum;
			}
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x00142048 File Offset: 0x00140248
		private void Awake()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
			Player.onLocalPlayerSpawned = (Action)Delegate.Combine(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x00142095 File Offset: 0x00140295
		private void OnDestroy()
		{
			Player.onLocalPlayerSpawned = (Action)Delegate.Remove(Player.onLocalPlayerSpawned, new Action(this.PlayerSpawned));
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x001420B8 File Offset: 0x001402B8
		protected virtual void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 2);
			this.activeScreen = this.menuScreen;
			this.canvas.enabled = false;
			for (int i = 0; i < this.amountButtons.Count; i++)
			{
				int fuckYou = i;
				this.amountButtons[i].onClick.AddListener(delegate()
				{
					this.AmountSelected(fuckYou);
				});
				if (i == this.amountButtons.Count - 1)
				{
					this.amountButtons[i].transform.Find("Text").GetComponent<Text>().text = "ALL ()";
				}
				else
				{
					this.amountButtons[i].transform.Find("Text").GetComponent<Text>().text = MoneyManager.FormatAmount((float)ATMInterface.amounts[i], false, false);
				}
			}
			this.depositLimitContainer.gameObject.SetActive(true);
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x001421C3 File Offset: 0x001403C3
		private void PlayerSpawned()
		{
			this.canvas.worldCamera = PlayerSingleton<PlayerCamera>.Instance.Camera;
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x001421DC File Offset: 0x001403DC
		protected virtual void Update()
		{
			if (this.isOpen)
			{
				this.onlineBalanceText.text = MoneyManager.FormatAmount(NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance, false, false);
				this.cleanCashText.text = MoneyManager.FormatAmount(NetworkSingleton<MoneyManager>.Instance.cashBalance, false, false);
				this.depositLimitText.text = MoneyManager.FormatAmount(ATM.WeeklyDepositSum, false, false) + " / " + MoneyManager.FormatAmount(10000f, false, false);
				if (ATM.WeeklyDepositSum >= 10000f)
				{
					this.depositLimitText.color = new Color32(byte.MaxValue, 75, 75, byte.MaxValue);
				}
				else
				{
					this.depositLimitText.color = Color.white;
				}
				if (this.activeScreen == this.amountSelectorScreen)
				{
					if (this.depositing)
					{
						this.amountButtons[this.amountButtons.Count - 1].transform.Find("Text").GetComponent<Text>().text = "MAX (" + MoneyManager.FormatAmount(Mathf.Min(NetworkSingleton<MoneyManager>.Instance.cashBalance, ATMInterface.remainingAllowedDeposit), false, false) + ")";
					}
					this.UpdateAvailableAmounts();
					this.confirmAmountButton.interactable = (this.relevantBalance > 0f);
					if (this.depositing)
					{
						if (this.selectedAmountIndex == ATMInterface.amounts.Length)
						{
							this.confirmButtonText.text = "DEPOSIT ALL";
						}
						else
						{
							this.confirmButtonText.text = "DEPOSIT " + MoneyManager.FormatAmount(this.selectedAmount, false, false);
						}
					}
					else
					{
						this.confirmButtonText.text = "WITHDRAW " + MoneyManager.FormatAmount(this.selectedAmount, false, false);
					}
					if (this.relevantBalance < ATMInterface.GetAmountFromIndex(this.selectedAmountIndex, this.depositing))
					{
						this.DefaultAmountSelection();
					}
				}
				if (this.activeScreen == this.menuScreen)
				{
					this.menu_DepositButton.interactable = (ATM.WeeklyDepositSum < 10000f);
				}
				if (this.activeScreen == this.processingScreen)
				{
					this.processingScreenIndicator.localEulerAngles = new Vector3(0f, 0f, this.processingScreenIndicator.localEulerAngles.z - Time.deltaTime * 360f);
				}
			}
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x00142434 File Offset: 0x00140634
		protected virtual void LateUpdate()
		{
			if (this.isOpen && this.activeScreen == this.amountSelectorScreen)
			{
				if (this.selectedAmountIndex == -1)
				{
					this.selectedButtonIndicator.gameObject.SetActive(false);
					return;
				}
				this.selectedButtonIndicator.anchoredPosition = this.amountButtons[this.selectedAmountIndex].GetComponent<RectTransform>().anchoredPosition;
				this.selectedButtonIndicator.gameObject.SetActive(true);
			}
		}

		// Token: 0x06004C8B RID: 19595 RVA: 0x001424B0 File Offset: 0x001406B0
		public virtual void SetIsOpen(bool o)
		{
			if (o == this.isOpen)
			{
				return;
			}
			this.isOpen = o;
			this.canvas.enabled = this.isOpen;
			EventSystem.current.SetSelectedGameObject(null);
			if (this.isOpen)
			{
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				Singleton<HUD>.Instance.SetCrosshairVisible(false);
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(base.name);
				this.SetActiveScreen(this.menuScreen);
				return;
			}
			this.atm.Exit();
			PlayerSingleton<PlayerCamera>.Instance.LockMouse();
			Singleton<HUD>.Instance.SetCrosshairVisible(true);
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(base.name);
		}

		// Token: 0x06004C8C RID: 19596 RVA: 0x00142554 File Offset: 0x00140754
		public virtual void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (action.exitType == ExitType.Escape)
			{
				action.used = true;
				if (this.activeScreen == this.menuScreen || this.activeScreen == this.successScreen)
				{
					this.SetIsOpen(false);
					return;
				}
				if (this.activeScreen == this.amountSelectorScreen)
				{
					this.SetActiveScreen(this.menuScreen);
				}
			}
		}

		// Token: 0x06004C8D RID: 19597 RVA: 0x001425D0 File Offset: 0x001407D0
		public void SetActiveScreen(RectTransform screen)
		{
			this.menuScreen.gameObject.SetActive(false);
			this.amountSelectorScreen.gameObject.SetActive(false);
			this.processingScreen.gameObject.SetActive(false);
			this.successScreen.gameObject.SetActive(false);
			this.activeScreen = screen;
			this.activeScreen.gameObject.SetActive(true);
			if (this.activeScreen == this.menuScreen)
			{
				this.menu_TitleText.text = "Hello, " + Player.Local.PlayerName;
				this.menu_DepositButton.Select();
				return;
			}
			if (this.activeScreen == this.amountSelectorScreen)
			{
				this.UpdateAvailableAmounts();
				this.DefaultAmountSelection();
				return;
			}
			if (this.activeScreen == this.successScreen)
			{
				this.doneButton.Select();
			}
		}

		// Token: 0x06004C8E RID: 19598 RVA: 0x001426B8 File Offset: 0x001408B8
		private void DefaultAmountSelection()
		{
			if (this.amountButtons[0].interactable)
			{
				this.amountButtons[0].Select();
				this.AmountSelected(0);
				return;
			}
			if (this.amountButtons[this.amountButtons.Count - 1].interactable && this.relevantBalance > 0f)
			{
				this.amountButtons[this.amountButtons.Count - 1].Select();
				this.AmountSelected(this.amountButtons.Count - 1);
				return;
			}
			this.AmountSelected(-1);
			for (int i = 0; i < this.amountButtons.Count; i++)
			{
			}
		}

		// Token: 0x06004C8F RID: 19599 RVA: 0x0014276B File Offset: 0x0014096B
		public void DepositButtonPressed()
		{
			this.amountSelectorTitle.text = "Select amount to deposit";
			this.depositing = true;
			this.SetActiveScreen(this.amountSelectorScreen);
		}

		// Token: 0x06004C90 RID: 19600 RVA: 0x00142790 File Offset: 0x00140990
		public void WithdrawButtonPressed()
		{
			this.amountSelectorTitle.text = "Select amount to withdraw";
			this.depositing = false;
			this.amountButtons[this.amountButtons.Count - 1].transform.Find("Text").GetComponent<Text>().text = MoneyManager.FormatAmount((float)ATMInterface.amounts[ATMInterface.amounts.Length - 1], false, false);
			this.SetActiveScreen(this.amountSelectorScreen);
		}

		// Token: 0x06004C91 RID: 19601 RVA: 0x00142808 File Offset: 0x00140A08
		public void CancelAmountSelection()
		{
			this.SetActiveScreen(this.menuScreen);
		}

		// Token: 0x06004C92 RID: 19602 RVA: 0x00142816 File Offset: 0x00140A16
		public void AmountSelected(int amountIndex)
		{
			this.selectedAmountIndex = amountIndex;
			this.SetSelectedAmount(ATMInterface.GetAmountFromIndex(amountIndex, this.depositing));
		}

		// Token: 0x06004C93 RID: 19603 RVA: 0x00142834 File Offset: 0x00140A34
		private void SetSelectedAmount(float amount)
		{
			float max;
			if (this.depositing)
			{
				max = Mathf.Min(NetworkSingleton<MoneyManager>.Instance.cashBalance, ATMInterface.remainingAllowedDeposit);
			}
			else
			{
				max = NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance;
			}
			this.selectedAmount = Mathf.Clamp(amount, 0f, max);
			this.amountLabelText.text = MoneyManager.FormatAmount(this.selectedAmount, false, false);
		}

		// Token: 0x06004C94 RID: 19604 RVA: 0x0014289C File Offset: 0x00140A9C
		public static float GetAmountFromIndex(int index, bool depositing)
		{
			if (index == -1 || index >= ATMInterface.amounts.Length)
			{
				return 0f;
			}
			if (depositing && index == ATMInterface.amounts.Length - 1)
			{
				return Mathf.Min(NetworkSingleton<MoneyManager>.Instance.cashBalance, ATMInterface.remainingAllowedDeposit);
			}
			return (float)ATMInterface.amounts[index];
		}

		// Token: 0x06004C95 RID: 19605 RVA: 0x001428EC File Offset: 0x00140AEC
		private void UpdateAvailableAmounts()
		{
			for (int i = 0; i < ATMInterface.amounts.Length; i++)
			{
				if (this.depositing && i == ATMInterface.amounts.Length - 1)
				{
					this.amountButtons[this.amountButtons.Count - 1].interactable = (this.relevantBalance > 0f && ATMInterface.remainingAllowedDeposit > 0f);
					return;
				}
				if (this.depositing)
				{
					this.amountButtons[i].interactable = (this.relevantBalance >= (float)ATMInterface.amounts[i] && ATM.WeeklyDepositSum + (float)ATMInterface.amounts[i] <= 10000f);
				}
				else
				{
					this.amountButtons[i].interactable = (this.relevantBalance >= (float)ATMInterface.amounts[i]);
				}
			}
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x001429C9 File Offset: 0x00140BC9
		public void AmountConfirmed()
		{
			base.StartCoroutine(this.ProcessTransaction(this.selectedAmount, this.depositing));
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x001429E4 File Offset: 0x00140BE4
		public void ChangeAmount(float amount)
		{
			this.selectedAmountIndex = -1;
			this.SetSelectedAmount(this.selectedAmount + amount);
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x001429FB File Offset: 0x00140BFB
		protected IEnumerator ProcessTransaction(float amount, bool depositing)
		{
			this.SetActiveScreen(this.processingScreen);
			yield return new WaitForSeconds(1f);
			this.CompleteSound.Play();
			if (depositing)
			{
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= amount)
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-amount, true, false);
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Cash Deposit", amount, 1f, string.Empty);
					ATM.WeeklyDepositSum += amount;
					this.successScreenSubtitle.text = "You have deposited " + MoneyManager.FormatAmount(amount, false, false);
					this.SetActiveScreen(this.successScreen);
				}
				else
				{
					this.SetActiveScreen(this.menuScreen);
				}
			}
			else if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= amount)
			{
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(amount, true, false);
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Cash Withdrawal", -amount, 1f, string.Empty);
				this.successScreenSubtitle.text = "You have withdrawn " + MoneyManager.FormatAmount(amount, false, false);
				this.SetActiveScreen(this.successScreen);
			}
			else
			{
				this.SetActiveScreen(this.menuScreen);
			}
			yield break;
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x00142A18 File Offset: 0x00140C18
		public void DoneButtonPressed()
		{
			this.SetIsOpen(false);
		}

		// Token: 0x06004C9A RID: 19610 RVA: 0x00142808 File Offset: 0x00140A08
		public void ReturnToMenuButtonPressed()
		{
			this.SetActiveScreen(this.menuScreen);
		}

		// Token: 0x040039D4 RID: 14804
		[Header("References")]
		[SerializeField]
		protected Canvas canvas;

		// Token: 0x040039D5 RID: 14805
		[SerializeField]
		protected ATM atm;

		// Token: 0x040039D6 RID: 14806
		[SerializeField]
		protected AudioSourceController CompleteSound;

		// Token: 0x040039D7 RID: 14807
		[Header("Menu")]
		[SerializeField]
		protected RectTransform menuScreen;

		// Token: 0x040039D8 RID: 14808
		[SerializeField]
		protected Text menu_TitleText;

		// Token: 0x040039D9 RID: 14809
		[SerializeField]
		protected Button menu_DepositButton;

		// Token: 0x040039DA RID: 14810
		[SerializeField]
		protected Button menu_WithdrawButton;

		// Token: 0x040039DB RID: 14811
		[Header("Top bar")]
		[SerializeField]
		protected Text depositLimitText;

		// Token: 0x040039DC RID: 14812
		[SerializeField]
		protected Text onlineBalanceText;

		// Token: 0x040039DD RID: 14813
		[SerializeField]
		protected Text cleanCashText;

		// Token: 0x040039DE RID: 14814
		[SerializeField]
		protected RectTransform depositLimitContainer;

		// Token: 0x040039DF RID: 14815
		[Header("Amount screen")]
		[SerializeField]
		protected RectTransform amountSelectorScreen;

		// Token: 0x040039E0 RID: 14816
		[SerializeField]
		protected Text amountSelectorTitle;

		// Token: 0x040039E1 RID: 14817
		[SerializeField]
		protected List<Button> amountButtons = new List<Button>();

		// Token: 0x040039E2 RID: 14818
		[SerializeField]
		protected Text amountLabelText;

		// Token: 0x040039E3 RID: 14819
		[SerializeField]
		protected RectTransform amountBackground;

		// Token: 0x040039E4 RID: 14820
		[SerializeField]
		protected RectTransform selectedButtonIndicator;

		// Token: 0x040039E5 RID: 14821
		[SerializeField]
		protected Button confirmAmountButton;

		// Token: 0x040039E6 RID: 14822
		[SerializeField]
		protected Text confirmButtonText;

		// Token: 0x040039E7 RID: 14823
		[Header("Processing screen")]
		[SerializeField]
		protected RectTransform processingScreen;

		// Token: 0x040039E8 RID: 14824
		[SerializeField]
		protected RectTransform processingScreenIndicator;

		// Token: 0x040039E9 RID: 14825
		[Header("Success screen")]
		[SerializeField]
		protected RectTransform successScreen;

		// Token: 0x040039EA RID: 14826
		[SerializeField]
		protected Text successScreenSubtitle;

		// Token: 0x040039EB RID: 14827
		[SerializeField]
		protected Button doneButton;

		// Token: 0x040039ED RID: 14829
		private RectTransform activeScreen;

		// Token: 0x040039EE RID: 14830
		public static int[] amounts = new int[]
		{
			20,
			50,
			100,
			500,
			1000,
			5000
		};

		// Token: 0x040039EF RID: 14831
		private bool depositing = true;

		// Token: 0x040039F0 RID: 14832
		private int selectedAmountIndex;

		// Token: 0x040039F1 RID: 14833
		private float selectedAmount;
	}
}
