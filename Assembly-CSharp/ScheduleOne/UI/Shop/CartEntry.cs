using System;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A52 RID: 2642
	public class CartEntry : MonoBehaviour
	{
		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x0600475E RID: 18270 RVA: 0x0012BEFB File Offset: 0x0012A0FB
		// (set) Token: 0x0600475F RID: 18271 RVA: 0x0012BF03 File Offset: 0x0012A103
		public int Quantity { get; protected set; }

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06004760 RID: 18272 RVA: 0x0012BF0C File Offset: 0x0012A10C
		// (set) Token: 0x06004761 RID: 18273 RVA: 0x0012BF14 File Offset: 0x0012A114
		public Cart Cart { get; protected set; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06004762 RID: 18274 RVA: 0x0012BF1D File Offset: 0x0012A11D
		// (set) Token: 0x06004763 RID: 18275 RVA: 0x0012BF25 File Offset: 0x0012A125
		public ShopListing Listing { get; protected set; }

		// Token: 0x06004764 RID: 18276 RVA: 0x0012BF30 File Offset: 0x0012A130
		public void Initialize(Cart cart, ShopListing listing, int quantity)
		{
			this.Cart = cart;
			this.Listing = listing;
			this.Quantity = quantity;
			this.IncrementButton.onClick.AddListener(delegate()
			{
				this.ChangeAmount(1);
			});
			this.DecrementButton.onClick.AddListener(delegate()
			{
				this.ChangeAmount(-1);
			});
			this.RemoveButton.onClick.AddListener(delegate()
			{
				this.ChangeAmount(-999);
			});
			this.UpdateTitle();
			this.UpdatePrice();
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x0012BFB2 File Offset: 0x0012A1B2
		public void SetQuantity(int quantity)
		{
			this.Quantity = quantity;
			this.UpdateTitle();
			this.UpdatePrice();
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x0012BFC8 File Offset: 0x0012A1C8
		protected virtual void UpdateTitle()
		{
			this.NameLabel.text = this.Quantity.ToString() + "x " + this.Listing.Item.Name;
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x0012C008 File Offset: 0x0012A208
		private void UpdatePrice()
		{
			this.PriceLabel.text = MoneyManager.FormatAmount((float)this.Quantity * this.Listing.Price, false, false);
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x0012C02F File Offset: 0x0012A22F
		private void ChangeAmount(int change)
		{
			if (change > 0)
			{
				this.Cart.AddItem(this.Listing, change);
				return;
			}
			if (change < 0)
			{
				this.Cart.RemoveItem(this.Listing, -change);
			}
		}

		// Token: 0x04003507 RID: 13575
		[Header("References")]
		public TextMeshProUGUI NameLabel;

		// Token: 0x04003508 RID: 13576
		public TextMeshProUGUI PriceLabel;

		// Token: 0x04003509 RID: 13577
		public Button IncrementButton;

		// Token: 0x0400350A RID: 13578
		public Button DecrementButton;

		// Token: 0x0400350B RID: 13579
		public Button RemoveButton;
	}
}
