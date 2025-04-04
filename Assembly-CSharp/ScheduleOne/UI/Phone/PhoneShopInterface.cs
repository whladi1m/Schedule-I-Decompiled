using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A85 RID: 2693
	public class PhoneShopInterface : MonoBehaviour
	{
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06004889 RID: 18569 RVA: 0x0012FA4B File Offset: 0x0012DC4B
		// (set) Token: 0x0600488A RID: 18570 RVA: 0x0012FA53 File Offset: 0x0012DC53
		public bool IsOpen { get; private set; }

		// Token: 0x0600488B RID: 18571 RVA: 0x0012FA5C File Offset: 0x0012DC5C
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			this.ConfirmButton.onClick.AddListener(new UnityAction(this.ConfirmOrderPressed));
			this.ItemLimitContainer.gameObject.SetActive(true);
			this.Close();
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x0012FAB0 File Offset: 0x0012DCB0
		public void Open(string title, string subtitle, MSGConversation _conversation, List<PhoneShopInterface.Listing> listings, float _orderLimit, float debt, Action<List<PhoneShopInterface.CartEntry>, float> _orderConfirmedCallback)
		{
			this.IsOpen = true;
			this.TitleLabel.text = title;
			this.SubtitleLabel.text = subtitle;
			this.OrderLimitLabel.text = MoneyManager.FormatAmount(_orderLimit, false, false);
			this.DebtLabel.text = MoneyManager.FormatAmount(debt, false, false);
			this.orderLimit = _orderLimit;
			this.conversation = _conversation;
			MSGConversation msgconversation = this.conversation;
			msgconversation.onMessageRendered = (Action)Delegate.Combine(msgconversation.onMessageRendered, new Action(this.Close));
			this.orderConfirmedCallback = _orderConfirmedCallback;
			this._items.Clear();
			this._items.AddRange(listings);
			using (List<PhoneShopInterface.Listing>.Enumerator enumerator = listings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PhoneShopInterface.Listing entry = enumerator.Current;
					RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.EntryPrefab, this.EntryContainer);
					rectTransform.Find("Icon").GetComponent<Image>().sprite = entry.Item.Icon;
					rectTransform.Find("Name").GetComponent<Text>().text = entry.Item.Name;
					rectTransform.Find("Price").GetComponent<Text>().text = MoneyManager.FormatAmount(entry.Price, false, false);
					rectTransform.Find("Quantity").GetComponent<Text>().text = "0";
					StorableItemDefinition storableItemDefinition = entry.Item as StorableItemDefinition;
					if (!storableItemDefinition.RequiresLevelToPurchase || NetworkSingleton<LevelManager>.Instance.GetFullRank() >= storableItemDefinition.RequiredRank)
					{
						rectTransform.Find("Quantity/Remove").GetComponent<Button>().onClick.AddListener(delegate()
						{
							this.ChangeListingQuantity(entry, -1);
						});
						rectTransform.Find("Quantity/Add").GetComponent<Button>().onClick.AddListener(delegate()
						{
							this.ChangeListingQuantity(entry, 1);
						});
						rectTransform.Find("Locked").gameObject.SetActive(false);
					}
					else
					{
						rectTransform.Find("Locked/Title").GetComponent<Text>().text = "Unlocks at " + storableItemDefinition.RequiredRank.ToString();
						rectTransform.Find("Locked").gameObject.SetActive(true);
					}
					this._entries.Add(rectTransform);
				}
			}
			this.CartChanged();
			this.Container.gameObject.SetActive(true);
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x0012FD54 File Offset: 0x0012DF54
		public void Close()
		{
			this.IsOpen = false;
			this._items.Clear();
			this._cart.Clear();
			if (this.conversation != null)
			{
				MSGConversation msgconversation = this.conversation;
				msgconversation.onMessageRendered = (Action)Delegate.Remove(msgconversation.onMessageRendered, new Action(this.Close));
			}
			foreach (RectTransform rectTransform in this._entries)
			{
				UnityEngine.Object.Destroy(rectTransform.gameObject);
			}
			this._entries.Clear();
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x0012FE14 File Offset: 0x0012E014
		public void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (!this.IsOpen)
			{
				return;
			}
			action.used = true;
			this.Close();
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x0012FE38 File Offset: 0x0012E038
		private void ChangeListingQuantity(PhoneShopInterface.Listing listing, int change)
		{
			PhoneShopInterface.CartEntry cartEntry = this._cart.Find((PhoneShopInterface.CartEntry e) => e.Listing.Item.ID == listing.Item.ID);
			if (cartEntry == null)
			{
				cartEntry = new PhoneShopInterface.CartEntry(listing, 0);
				this._cart.Add(cartEntry);
			}
			cartEntry.Quantity = Mathf.Clamp(cartEntry.Quantity + change, 0, 99);
			this._entries[this._items.IndexOf(listing)].Find("Quantity").GetComponent<Text>().text = cartEntry.Quantity.ToString();
			this.CartChanged();
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x0012FEDD File Offset: 0x0012E0DD
		private void CartChanged()
		{
			this.UpdateOrderTotal();
			this.ConfirmButton.interactable = this.CanConfirmOrder();
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x0012FEF8 File Offset: 0x0012E0F8
		private void ConfirmOrderPressed()
		{
			int num;
			this.orderConfirmedCallback(this._cart, this.GetOrderTotal(out num));
			this.Close();
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x0012FF24 File Offset: 0x0012E124
		private bool CanConfirmOrder()
		{
			int num;
			float orderTotal = this.GetOrderTotal(out num);
			return orderTotal > 0f && orderTotal <= this.orderLimit && num <= 10;
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x0012FF58 File Offset: 0x0012E158
		private void UpdateOrderTotal()
		{
			int num;
			float orderTotal = this.GetOrderTotal(out num);
			this.OrderTotalLabel.text = MoneyManager.FormatAmount(orderTotal, false, false);
			this.OrderTotalLabel.color = ((orderTotal <= this.orderLimit) ? this.ValidAmountColor : this.InvalidAmountColor);
			this.ItemLimitLabel.text = num.ToString() + "/" + 10.ToString();
			this.ItemLimitLabel.color = ((num <= 10) ? Color.black : this.InvalidAmountColor);
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x0012FFE8 File Offset: 0x0012E1E8
		private float GetOrderTotal(out int itemCount)
		{
			float num = 0f;
			itemCount = 0;
			foreach (PhoneShopInterface.CartEntry cartEntry in this._cart)
			{
				num += cartEntry.Listing.Price * (float)cartEntry.Quantity;
				itemCount += cartEntry.Quantity;
			}
			return num;
		}

		// Token: 0x040035E5 RID: 13797
		public RectTransform EntryPrefab;

		// Token: 0x040035E6 RID: 13798
		public Color ValidAmountColor;

		// Token: 0x040035E7 RID: 13799
		public Color InvalidAmountColor;

		// Token: 0x040035E8 RID: 13800
		[Header("References")]
		public GameObject Container;

		// Token: 0x040035E9 RID: 13801
		public Text TitleLabel;

		// Token: 0x040035EA RID: 13802
		public Text SubtitleLabel;

		// Token: 0x040035EB RID: 13803
		public RectTransform EntryContainer;

		// Token: 0x040035EC RID: 13804
		public Text OrderTotalLabel;

		// Token: 0x040035ED RID: 13805
		public Text OrderLimitLabel;

		// Token: 0x040035EE RID: 13806
		public Text DebtLabel;

		// Token: 0x040035EF RID: 13807
		public Button ConfirmButton;

		// Token: 0x040035F0 RID: 13808
		public GameObject ItemLimitContainer;

		// Token: 0x040035F1 RID: 13809
		public Text ItemLimitLabel;

		// Token: 0x040035F2 RID: 13810
		private List<RectTransform> _entries = new List<RectTransform>();

		// Token: 0x040035F3 RID: 13811
		private List<PhoneShopInterface.Listing> _items = new List<PhoneShopInterface.Listing>();

		// Token: 0x040035F4 RID: 13812
		private List<PhoneShopInterface.CartEntry> _cart = new List<PhoneShopInterface.CartEntry>();

		// Token: 0x040035F5 RID: 13813
		private float orderLimit;

		// Token: 0x040035F6 RID: 13814
		private Action<List<PhoneShopInterface.CartEntry>, float> orderConfirmedCallback;

		// Token: 0x040035F7 RID: 13815
		private MSGConversation conversation;

		// Token: 0x02000A86 RID: 2694
		[Serializable]
		public class Listing
		{
			// Token: 0x06004896 RID: 18582 RVA: 0x00130089 File Offset: 0x0012E289
			public Listing(ItemDefinition item, float price)
			{
				this.Item = item;
				this.Price = price;
			}

			// Token: 0x040035F8 RID: 13816
			public ItemDefinition Item;

			// Token: 0x040035F9 RID: 13817
			public float Price;
		}

		// Token: 0x02000A87 RID: 2695
		[Serializable]
		public class CartEntry
		{
			// Token: 0x06004897 RID: 18583 RVA: 0x0013009F File Offset: 0x0012E29F
			public CartEntry(PhoneShopInterface.Listing listing, int quantity)
			{
				this.Listing = listing;
				this.Quantity = quantity;
			}

			// Token: 0x040035FA RID: 13818
			public PhoneShopInterface.Listing Listing;

			// Token: 0x040035FB RID: 13819
			public int Quantity;
		}
	}
}
