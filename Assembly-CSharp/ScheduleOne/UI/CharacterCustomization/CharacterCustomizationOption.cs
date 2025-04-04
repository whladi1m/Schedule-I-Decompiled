using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Levelling;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.CharacterCustomization
{
	// Token: 0x02000B2C RID: 2860
	public class CharacterCustomizationOption : MonoBehaviour
	{
		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06004C1F RID: 19487 RVA: 0x00140DAF File Offset: 0x0013EFAF
		// (set) Token: 0x06004C20 RID: 19488 RVA: 0x00140DB7 File Offset: 0x0013EFB7
		public bool purchased { get; private set; }

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06004C21 RID: 19489 RVA: 0x00140DC0 File Offset: 0x0013EFC0
		private bool purchaseable
		{
			get
			{
				return !this.RequireLevel || this.RequiredLevel <= NetworkSingleton<LevelManager>.Instance.GetFullRank();
			}
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x00140DE4 File Offset: 0x0013EFE4
		private void Awake()
		{
			this.NameLabel.text = this.Name;
			if (this.Price > 0f)
			{
				this.PriceLabel.text = MoneyManager.FormatAmount(this.Price, false, false);
			}
			else
			{
				this.PriceLabel.text = "Free";
			}
			this.UpdatePriceColor();
			this.LevelLabel.text = this.RequiredLevel.ToString();
			this.MainButton.onClick.AddListener(new UnityAction(this.Selected));
			this.BuyButton.onClick.AddListener(new UnityAction(this.Purchased));
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x00140E93 File Offset: 0x0013F093
		private void OnValidate()
		{
			base.gameObject.name = this.Name;
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x00140EA6 File Offset: 0x0013F0A6
		private void FixedUpdate()
		{
			this.BuyButton.interactable = (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.Price);
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x00140EC8 File Offset: 0x0013F0C8
		private void Start()
		{
			this.UpdateUI();
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x00140ED0 File Offset: 0x0013F0D0
		private void Selected()
		{
			this.SetSelected(true);
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x00140EDC File Offset: 0x0013F0DC
		private void Purchased()
		{
			if (!this.purchaseable)
			{
				return;
			}
			if (this.onPurchase != null)
			{
				this.onPurchase.Invoke();
			}
			if (this.Price > 0f)
			{
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Character customization", -this.Price, 1f, string.Empty);
			}
			this.SetPurchased(true);
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x00140F3C File Offset: 0x0013F13C
		private void UpdatePriceColor()
		{
			if (this.Price <= 0f)
			{
				Color color;
				this.PriceLabel.color = (ColorUtility.TryParseHtmlString("#4CBFFF", out color) ? color : Color.white);
				return;
			}
			if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= this.Price)
			{
				Color color2;
				this.PriceLabel.color = (ColorUtility.TryParseHtmlString("#4CBFFF", out color2) ? color2 : Color.white);
				return;
			}
			this.PriceLabel.color = new Color32(200, 75, 70, byte.MaxValue);
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x00140FD0 File Offset: 0x0013F1D0
		public void SetSelected(bool _selected)
		{
			this.selected = _selected;
			this.SelectionIndicator.gameObject.SetActive(this.selected);
			this.NameLabel.rectTransform.offsetMin = new Vector2(this.selected ? 30f : 10f, this.NameLabel.rectTransform.offsetMin.y);
			this.UpdateUI();
			if (this.selected)
			{
				if (this.onSelect != null)
				{
					this.onSelect.Invoke();
					return;
				}
			}
			else if (this.onDeselect != null)
			{
				this.onDeselect.Invoke();
			}
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x00141070 File Offset: 0x0013F270
		public void SetPurchased(bool _purchased)
		{
			this.purchased = _purchased;
			this.BuyButton.gameObject.SetActive(!this.purchased);
			this.PriceLabel.gameObject.SetActive(!this.purchased);
			if (_purchased)
			{
				this.SetSelected(true);
			}
			this.UpdateUI();
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x001410C8 File Offset: 0x0013F2C8
		private void UpdateUI()
		{
			this.LockDisplay.gameObject.SetActive(!this.purchaseable);
			this.PriceLabel.gameObject.SetActive(this.purchaseable && !this.purchased);
			this.BuyButton.gameObject.SetActive(this.purchaseable && !this.purchased);
			this.UpdatePriceColor();
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x0014113C File Offset: 0x0013F33C
		public void ParentCategoryClosed()
		{
			if (this.selected && !this.purchased)
			{
				this.SetSelected(false);
				return;
			}
			if (this.purchased && !this.selected)
			{
				this.SetSelected(true);
			}
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x0014116D File Offset: 0x0013F36D
		public void SiblingOptionSelected(CharacterCustomizationOption option)
		{
			if (option != this && this.selected)
			{
				this.SetSelected(false);
			}
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x00141187 File Offset: 0x0013F387
		public void SiblingOptionPurchased(CharacterCustomizationOption option)
		{
			if (option != this && this.purchased)
			{
				this.SetPurchased(false);
			}
		}

		// Token: 0x04003982 RID: 14722
		public string Name = "Option";

		// Token: 0x04003983 RID: 14723
		public string Label = "AssetPath or Label";

		// Token: 0x04003984 RID: 14724
		public float Price;

		// Token: 0x04003985 RID: 14725
		public bool RequireLevel;

		// Token: 0x04003986 RID: 14726
		public FullRank RequiredLevel = new FullRank(ERank.Street_Rat, 1);

		// Token: 0x04003987 RID: 14727
		[Header("References")]
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003988 RID: 14728
		public TextMeshProUGUI PriceLabel;

		// Token: 0x04003989 RID: 14729
		public TextMeshProUGUI LevelLabel;

		// Token: 0x0400398A RID: 14730
		public RectTransform LockDisplay;

		// Token: 0x0400398B RID: 14731
		public Button MainButton;

		// Token: 0x0400398C RID: 14732
		public Button BuyButton;

		// Token: 0x0400398D RID: 14733
		public RectTransform SelectionIndicator;

		// Token: 0x0400398E RID: 14734
		[Header("Events")]
		public UnityEvent onSelect;

		// Token: 0x0400398F RID: 14735
		public UnityEvent onDeselect;

		// Token: 0x04003990 RID: 14736
		public UnityEvent onPurchase;

		// Token: 0x04003992 RID: 14738
		private bool selected;
	}
}
