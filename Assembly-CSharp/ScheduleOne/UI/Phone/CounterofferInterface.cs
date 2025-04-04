using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A83 RID: 2691
	public class CounterofferInterface : MonoBehaviour
	{
		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06004873 RID: 18547 RVA: 0x0012F48F File Offset: 0x0012D68F
		// (set) Token: 0x06004874 RID: 18548 RVA: 0x0012F497 File Offset: 0x0012D697
		public bool IsOpen { get; private set; }

		// Token: 0x06004875 RID: 18549 RVA: 0x000045B1 File Offset: 0x000027B1
		private void Awake()
		{
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x0012F4A0 File Offset: 0x0012D6A0
		private void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 4);
			this.Close();
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x0012F4BC File Offset: 0x0012D6BC
		private void Update()
		{
			if (this.EntryContainer.gameObject.activeSelf && GameInput.GetButtonUp(GameInput.ButtonCode.PrimaryClick) && this.mouseUp)
			{
				this.EntryContainer.gameObject.SetActive(false);
				this.DisplayProduct(this.product);
			}
			if (!GameInput.GetButton(GameInput.ButtonCode.PrimaryClick))
			{
				this.mouseUp = true;
			}
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x0012F518 File Offset: 0x0012D718
		public void Open(ProductDefinition product, int quantity, float price, MSGConversation _conversation, Action<ProductDefinition, int, float> _orderConfirmedCallback)
		{
			this.IsOpen = true;
			this.product = product;
			this.quantity = Mathf.Clamp(quantity, 1, this.MaxQuantity);
			this.price = price;
			foreach (ProductDefinition key in ProductManager.DiscoveredProducts)
			{
				if (!this.productEntries.ContainsKey(key))
				{
					this.CreateProductEntry(key);
				}
			}
			this.conversation = _conversation;
			MSGConversation msgconversation = this.conversation;
			msgconversation.onMessageRendered = (Action)Delegate.Combine(msgconversation.onMessageRendered, new Action(this.Close));
			this.orderConfirmedCallback = _orderConfirmedCallback;
			this.EntryContainer.gameObject.SetActive(false);
			this.Container.gameObject.SetActive(true);
			this.SetProduct(product);
			this.PriceInput.text = price.ToString();
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x0012F614 File Offset: 0x0012D814
		private void CreateProductEntry(ProductDefinition product)
		{
			if (this.productEntries.ContainsKey(product))
			{
				return;
			}
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ProductEntryPrefab, this.EntryContainer).GetComponent<RectTransform>();
			component.Find("Icon").GetComponent<Image>().sprite = product.Icon;
			component.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.EntryContainer.gameObject.SetActive(false);
				this.SetProduct(product);
			});
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				this.DisplayProduct(product);
			});
			component.gameObject.AddComponent<EventTrigger>().triggers.Add(entry);
			this.productEntries.Add(product, component);
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x0012F6E8 File Offset: 0x0012D8E8
		public void Close()
		{
			this.IsOpen = false;
			if (this.conversation != null)
			{
				MSGConversation msgconversation = this.conversation;
				msgconversation.onMessageRendered = (Action)Delegate.Remove(msgconversation.onMessageRendered, new Action(this.Close));
			}
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x0012F73C File Offset: 0x0012D93C
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

		// Token: 0x0600487C RID: 18556 RVA: 0x0012F760 File Offset: 0x0012D960
		public void Send()
		{
			float num;
			if (float.TryParse(this.PriceInput.text, out num))
			{
				this.price = num;
			}
			this.price = Mathf.Clamp(this.price, 1f, 9999f);
			this.PriceInput.SetTextWithoutNotify(this.price.ToString());
			if (this.orderConfirmedCallback != null)
			{
				this.orderConfirmedCallback(this.product, this.quantity, this.price);
			}
			this.Close();
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x0012F7E4 File Offset: 0x0012D9E4
		private void UpdateFairPrice()
		{
			float amount = this.product.MarketValue * (float)this.quantity;
			this.FairPriceLabel.text = "Fair price: " + MoneyManager.FormatAmount(amount, false, false);
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x0012F822 File Offset: 0x0012DA22
		private void SetProduct(ProductDefinition newProduct)
		{
			this.product = newProduct;
			this.DisplayProduct(newProduct);
			this.UpdateFairPrice();
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x0012F838 File Offset: 0x0012DA38
		private void DisplayProduct(ProductDefinition tempProduct)
		{
			this.ProductIcon.sprite = tempProduct.Icon;
			this.UpdatePriceQuantityLabel(tempProduct.Name);
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x0012F857 File Offset: 0x0012DA57
		public void ChangeQuantity(int change)
		{
			this.quantity = Mathf.Clamp(this.quantity + change, 1, this.MaxQuantity);
			this.UpdatePriceQuantityLabel(this.product.Name);
			this.UpdateFairPrice();
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x0012F88C File Offset: 0x0012DA8C
		private void UpdatePriceQuantityLabel(string productName)
		{
			this.ProductLabel.text = this.quantity.ToString() + "x " + productName;
			float value = -(this.ProductLabel.preferredWidth / 2f) + 20f;
			this.ProductLabelRect.anchoredPosition = new Vector2(Mathf.Clamp(value, -120f, float.MaxValue), this.ProductLabelRect.anchoredPosition.y);
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x0012F903 File Offset: 0x0012DB03
		public void ChangePrice(float change)
		{
			this.price = Mathf.Clamp(this.price + change, 1f, 9999f);
			this.PriceInput.SetTextWithoutNotify(this.price.ToString());
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0012F938 File Offset: 0x0012DB38
		public void PriceSubmitted(string value)
		{
			float num;
			if (float.TryParse(value, out num))
			{
				this.price = num;
			}
			else
			{
				this.price = 0f;
			}
			this.price = Mathf.Clamp(this.price, 1f, 9999f);
			this.PriceInput.SetTextWithoutNotify(this.price.ToString());
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x0012F994 File Offset: 0x0012DB94
		public void ProductClicked()
		{
			this.mouseUp = false;
			if (!this.EntryContainer.gameObject.activeSelf)
			{
				this.EntryContainer.gameObject.SetActive(true);
				return;
			}
			this.EntryContainer.gameObject.SetActive(false);
			this.DisplayProduct(this.product);
		}

		// Token: 0x040035CA RID: 13770
		public const int COUNTEROFFER_SUCCESS_XP = 5;

		// Token: 0x040035CC RID: 13772
		public const int MinQuantity = 1;

		// Token: 0x040035CD RID: 13773
		public int MaxQuantity = 50;

		// Token: 0x040035CE RID: 13774
		public const float MinPrice = 1f;

		// Token: 0x040035CF RID: 13775
		public const float MaxPrice = 9999f;

		// Token: 0x040035D0 RID: 13776
		public float IconAlignment = 0.2f;

		// Token: 0x040035D1 RID: 13777
		public GameObject ProductEntryPrefab;

		// Token: 0x040035D2 RID: 13778
		[Header("References")]
		public GameObject Container;

		// Token: 0x040035D3 RID: 13779
		public Text TitleLabel;

		// Token: 0x040035D4 RID: 13780
		public Button ConfirmButton;

		// Token: 0x040035D5 RID: 13781
		public Image ProductIcon;

		// Token: 0x040035D6 RID: 13782
		public Text ProductLabel;

		// Token: 0x040035D7 RID: 13783
		public RectTransform ProductLabelRect;

		// Token: 0x040035D8 RID: 13784
		public InputField PriceInput;

		// Token: 0x040035D9 RID: 13785
		public Text FairPriceLabel;

		// Token: 0x040035DA RID: 13786
		public RectTransform EntryContainer;

		// Token: 0x040035DB RID: 13787
		private Action<ProductDefinition, int, float> orderConfirmedCallback;

		// Token: 0x040035DC RID: 13788
		private ProductDefinition product;

		// Token: 0x040035DD RID: 13789
		private int quantity;

		// Token: 0x040035DE RID: 13790
		private float price;

		// Token: 0x040035DF RID: 13791
		private Dictionary<ProductDefinition, RectTransform> productEntries = new Dictionary<ProductDefinition, RectTransform>();

		// Token: 0x040035E0 RID: 13792
		private bool mouseUp;

		// Token: 0x040035E1 RID: 13793
		private MSGConversation conversation;
	}
}
