using System;
using ScheduleOne.Money;
using ScheduleOne.UI.Shop;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Delivery
{
	// Token: 0x02000AAC RID: 2732
	public class ListingEntry : MonoBehaviour
	{
		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x0600498F RID: 18831 RVA: 0x00133CC2 File Offset: 0x00131EC2
		// (set) Token: 0x06004990 RID: 18832 RVA: 0x00133CCA File Offset: 0x00131ECA
		public ShopListing MatchingListing { get; private set; }

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06004991 RID: 18833 RVA: 0x00133CD3 File Offset: 0x00131ED3
		// (set) Token: 0x06004992 RID: 18834 RVA: 0x00133CDB File Offset: 0x00131EDB
		public int SelectedQuantity { get; private set; }

		// Token: 0x06004993 RID: 18835 RVA: 0x00133CE4 File Offset: 0x00131EE4
		public void Initialize(ShopListing match)
		{
			this.MatchingListing = match;
			this.Icon.sprite = this.MatchingListing.Item.Icon;
			this.ItemNameLabel.text = this.MatchingListing.Item.Name;
			this.ItemPriceLabel.text = MoneyManager.FormatAmount(this.MatchingListing.Price, false, false);
			this.QuantityInput.onSubmit.AddListener(new UnityAction<string>(this.OnQuantityInputSubmitted));
			this.QuantityInput.onEndEdit.AddListener(delegate(string value)
			{
				this.ValidateInput();
			});
			this.IncrementButton.onClick.AddListener(delegate()
			{
				this.ChangeQuantity(1);
			});
			this.DecrementButton.onClick.AddListener(delegate()
			{
				this.ChangeQuantity(-1);
			});
			this.QuantityInput.SetTextWithoutNotify(this.SelectedQuantity.ToString());
			this.RefreshLocked();
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00133DDA File Offset: 0x00131FDA
		public void RefreshLocked()
		{
			if (this.MatchingListing.Item.IsPurchasable)
			{
				this.LockedContainer.gameObject.SetActive(false);
				return;
			}
			this.LockedContainer.gameObject.SetActive(true);
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00133E14 File Offset: 0x00132014
		public void SetQuantity(int quant, bool notify = true)
		{
			if (!this.MatchingListing.Item.IsPurchasable)
			{
				quant = 0;
			}
			this.SelectedQuantity = Mathf.Clamp(quant, 0, 999);
			this.QuantityInput.SetTextWithoutNotify(this.SelectedQuantity.ToString());
			if (notify && this.onQuantityChanged != null)
			{
				this.onQuantityChanged.Invoke();
			}
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00133E77 File Offset: 0x00132077
		private void ChangeQuantity(int change)
		{
			this.SetQuantity(this.SelectedQuantity + change, true);
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00133E88 File Offset: 0x00132088
		private void OnQuantityInputSubmitted(string value)
		{
			int quant;
			if (int.TryParse(value, out quant))
			{
				this.SetQuantity(quant, true);
				return;
			}
			this.SetQuantity(0, true);
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00133EB0 File Offset: 0x001320B0
		private void ValidateInput()
		{
			this.OnQuantityInputSubmitted(this.QuantityInput.text);
		}

		// Token: 0x040036EF RID: 14063
		[Header("References")]
		public Image Icon;

		// Token: 0x040036F0 RID: 14064
		public Text ItemNameLabel;

		// Token: 0x040036F1 RID: 14065
		public Text ItemPriceLabel;

		// Token: 0x040036F2 RID: 14066
		public InputField QuantityInput;

		// Token: 0x040036F3 RID: 14067
		public Button IncrementButton;

		// Token: 0x040036F4 RID: 14068
		public Button DecrementButton;

		// Token: 0x040036F5 RID: 14069
		public RectTransform LockedContainer;

		// Token: 0x040036F6 RID: 14070
		public UnityEvent onQuantityChanged;
	}
}
