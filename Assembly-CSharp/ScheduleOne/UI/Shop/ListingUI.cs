using System;
using ScheduleOne.Money;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A58 RID: 2648
	public class ListingUI : MonoBehaviour
	{
		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x0600477F RID: 18303 RVA: 0x0012C447 File Offset: 0x0012A647
		// (set) Token: 0x06004780 RID: 18304 RVA: 0x0012C44F File Offset: 0x0012A64F
		public ShopListing Listing { get; protected set; }

		// Token: 0x06004781 RID: 18305 RVA: 0x0012C458 File Offset: 0x0012A658
		public virtual void Initialize(ShopListing listing)
		{
			this.Listing = listing;
			this.Icon.sprite = listing.Item.Icon;
			this.NameLabel.text = listing.Item.Name;
			this.UpdatePrice();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate(BaseEventData <p0>)
			{
				this.HoverStart();
			});
			this.Trigger.triggers.Add(entry);
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.PointerExit;
			entry2.callback.AddListener(delegate(BaseEventData <p0>)
			{
				this.HoverEnd();
			});
			this.Trigger.triggers.Add(entry2);
			listing.onQuantityChanged = (Action)Delegate.Combine(listing.onQuantityChanged, new Action(this.QuantityChanged));
			this.BuyButton.onClick.AddListener(new UnityAction(this.Clicked));
			this.DropdownButton.onClick.AddListener(new UnityAction(this.DropdownClicked));
			this.UpdateLockStatus();
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x0012C568 File Offset: 0x0012A768
		public virtual RectTransform GetIconCopy(RectTransform parent)
		{
			return UnityEngine.Object.Instantiate<GameObject>(this.Icon.gameObject, parent).GetComponent<RectTransform>();
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0012C580 File Offset: 0x0012A780
		private void Clicked()
		{
			if (this.onClicked != null)
			{
				this.onClicked();
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0012C595 File Offset: 0x0012A795
		private void DropdownClicked()
		{
			if (this.onDropdownClicked != null)
			{
				this.onDropdownClicked();
			}
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0012C5AA File Offset: 0x0012A7AA
		private void HoverStart()
		{
			if (this.hoverStart != null)
			{
				this.hoverStart();
			}
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0012C5BF File Offset: 0x0012A7BF
		private void HoverEnd()
		{
			if (this.hoverEnd != null)
			{
				this.hoverEnd();
			}
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0012C5D4 File Offset: 0x0012A7D4
		private void QuantityChanged()
		{
			this.UpdatePrice();
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0012C5DC File Offset: 0x0012A7DC
		private void UpdatePrice()
		{
			if (this.Listing.IsInStock)
			{
				this.PriceLabel.text = MoneyManager.FormatAmount(this.Listing.Price, false, false);
				this.PriceLabel.color = ListingUI.PriceLabelColor_Normal;
				return;
			}
			this.PriceLabel.text = "Out of stock";
			this.PriceLabel.color = ListingUI.PriceLabelColor_NoStock;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x0012C64E File Offset: 0x0012A84E
		public void UpdateLockStatus()
		{
			this.LockedContainer.gameObject.SetActive(!this.Listing.Item.IsPurchasable);
		}

		// Token: 0x04003529 RID: 13609
		public static Color32 PriceLabelColor_Normal = new Color32(90, 185, 90, byte.MaxValue);

		// Token: 0x0400352A RID: 13610
		public static Color32 PriceLabelColor_NoStock = new Color32(165, 70, 60, byte.MaxValue);

		// Token: 0x0400352C RID: 13612
		[Header("References")]
		public Image Icon;

		// Token: 0x0400352D RID: 13613
		public TextMeshProUGUI NameLabel;

		// Token: 0x0400352E RID: 13614
		public TextMeshProUGUI PriceLabel;

		// Token: 0x0400352F RID: 13615
		public GameObject LockedContainer;

		// Token: 0x04003530 RID: 13616
		public Button BuyButton;

		// Token: 0x04003531 RID: 13617
		public Button DropdownButton;

		// Token: 0x04003532 RID: 13618
		public EventTrigger Trigger;

		// Token: 0x04003533 RID: 13619
		public RectTransform DetailPanelAnchor;

		// Token: 0x04003534 RID: 13620
		public RectTransform DropdownAnchor;

		// Token: 0x04003535 RID: 13621
		public RectTransform TopDropdownAnchor;

		// Token: 0x04003536 RID: 13622
		public Action hoverStart;

		// Token: 0x04003537 RID: 13623
		public Action hoverEnd;

		// Token: 0x04003538 RID: 13624
		public Action onClicked;

		// Token: 0x04003539 RID: 13625
		public Action onDropdownClicked;
	}
}
