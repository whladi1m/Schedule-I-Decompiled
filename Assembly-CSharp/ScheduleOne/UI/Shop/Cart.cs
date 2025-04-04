using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A4F RID: 2639
	public class Cart : MonoBehaviour
	{
		// Token: 0x06004740 RID: 18240 RVA: 0x0012B47D File Offset: 0x0012967D
		protected virtual void Start()
		{
			this.UpdateViewCartText();
			this.BuyButton.onClick.AddListener(new UnityAction(this.Buy));
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0012B4A1 File Offset: 0x001296A1
		protected virtual void Update()
		{
			if (this.Shop.IsOpen)
			{
				this.UpdateEntries();
				this.UpdateLoadVehicleToggle();
				this.UpdateTotal();
				this.UpdateProblem();
			}
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x0012B4C8 File Offset: 0x001296C8
		public void AddItem(ShopListing listing, int quantity)
		{
			if (!this.cartDictionary.ContainsKey(listing))
			{
				this.cartDictionary.Add(listing, 0);
			}
			Console.Log(string.Concat(new string[]
			{
				"Adding ",
				quantity.ToString(),
				" ",
				listing.Item.Name,
				" to cart"
			}), null);
			Dictionary<ShopListing, int> dictionary = this.cartDictionary;
			dictionary[listing] += quantity;
			this.UpdateViewCartText();
			this.UpdateEntries();
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x0012B558 File Offset: 0x00129758
		public void RemoveItem(ShopListing listing, int quantity)
		{
			Dictionary<ShopListing, int> dictionary = this.cartDictionary;
			dictionary[listing] -= quantity;
			if (this.cartDictionary[listing] <= 0)
			{
				this.cartDictionary.Remove(listing);
			}
			this.Shop.RemoveItemSound.Play();
			this.UpdateProblem();
			this.UpdateViewCartText();
			this.UpdateEntries();
			this.UpdateTotal();
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x0012B5C2 File Offset: 0x001297C2
		public void ClearCart()
		{
			this.cartDictionary.Clear();
			this.UpdateViewCartText();
			this.UpdateEntries();
			this.UpdateTotal();
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x0012B5E1 File Offset: 0x001297E1
		public void BopCartIcon()
		{
			if (this.cartIconBop != null)
			{
				base.StopCoroutine(this.cartIconBop);
			}
			this.cartIconBop = base.StartCoroutine(this.<BopCartIcon>g__Routine|20_0());
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x0012B60C File Offset: 0x0012980C
		public bool CanPlayerAffordCart()
		{
			float priceSum = this.GetPriceSum();
			switch (this.Shop.PaymentType)
			{
			case ShopInterface.EPaymentType.Cash:
				return NetworkSingleton<MoneyManager>.Instance.cashBalance >= priceSum;
			case ShopInterface.EPaymentType.Online:
				return NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= priceSum;
			case ShopInterface.EPaymentType.PreferCash:
				return NetworkSingleton<MoneyManager>.Instance.cashBalance >= priceSum || NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= priceSum;
			case ShopInterface.EPaymentType.PreferOnline:
				return NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= priceSum || NetworkSingleton<MoneyManager>.Instance.cashBalance >= priceSum;
			default:
				return false;
			}
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x0012B6A8 File Offset: 0x001298A8
		public void Buy()
		{
			string text;
			if (!this.CanCheckout(out text))
			{
				return;
			}
			this.Shop.HandoverItems();
			switch (this.Shop.PaymentType)
			{
			case ShopInterface.EPaymentType.Cash:
				NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.GetPriceSum(), true, false);
				break;
			case ShopInterface.EPaymentType.Online:
				NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Purchase from " + this.Shop.ShopName, -this.GetPriceSum(), 1f, string.Empty);
				break;
			case ShopInterface.EPaymentType.PreferCash:
				if (NetworkSingleton<MoneyManager>.Instance.cashBalance >= this.GetPriceSum())
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.GetPriceSum(), true, false);
				}
				else
				{
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Purchase from " + this.Shop.ShopName, -this.GetPriceSum(), 1f, string.Empty);
				}
				break;
			case ShopInterface.EPaymentType.PreferOnline:
				if (NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance >= this.GetPriceSum())
				{
					NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Purchase from " + this.Shop.ShopName, -this.GetPriceSum(), 1f, string.Empty);
				}
				else
				{
					NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(-this.GetPriceSum(), true, false);
				}
				break;
			}
			this.ClearCart();
			this.Shop.CheckoutSound.Play();
			this.Shop.SetIsOpen(false);
			if (this.Shop.onOrderCompleted != null)
			{
				this.Shop.onOrderCompleted.Invoke();
			}
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x0012B834 File Offset: 0x00129A34
		private void UpdateEntries()
		{
			List<ShopListing> list = this.cartDictionary.Keys.ToList<ShopListing>();
			for (int i = 0; i < list.Count; i++)
			{
				CartEntry cartEntry = this.GetEntry(list[i]);
				if (cartEntry == null)
				{
					cartEntry = UnityEngine.Object.Instantiate<CartEntry>(this.EntryPrefab, this.CartEntryContainer);
					cartEntry.Initialize(this, list[i], this.cartDictionary[list[i]]);
					this.cartEntries.Add(cartEntry);
				}
				if (cartEntry.Quantity != this.cartDictionary[list[i]])
				{
					cartEntry.SetQuantity(this.cartDictionary[list[i]]);
				}
			}
			for (int j = 0; j < this.cartEntries.Count; j++)
			{
				if (!this.cartDictionary.ContainsKey(this.cartEntries[j].Listing))
				{
					UnityEngine.Object.Destroy(this.cartEntries[j].gameObject);
					this.cartEntries.RemoveAt(j);
					j--;
				}
			}
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x0012B94C File Offset: 0x00129B4C
		private void UpdateTotal()
		{
			this.TotalText.text = string.Concat(new string[]
			{
				"Total: <color=#",
				ColorUtility.ToHtmlStringRGBA(ListingUI.PriceLabelColor_Normal),
				">",
				MoneyManager.FormatAmount(this.GetPriceSum(), false, false),
				"</color>"
			});
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x0012B9AC File Offset: 0x00129BAC
		private void UpdateProblem()
		{
			string text;
			bool flag = this.CanCheckout(out text);
			this.BuyButton.interactable = (flag && this.cartDictionary.Count > 0);
			if (flag)
			{
				this.ProblemText.enabled = false;
			}
			else
			{
				this.ProblemText.text = text;
				this.ProblemText.enabled = true;
			}
			string text2;
			if (this.GetWarning(out text2) && !this.ProblemText.enabled)
			{
				this.WarningText.text = text2;
				this.WarningText.enabled = true;
				return;
			}
			this.WarningText.enabled = false;
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x0012BA48 File Offset: 0x00129C48
		private bool CanCheckout(out string reason)
		{
			if (!this.Shop.WillCartFit())
			{
				if (this.Shop.DeliveryBays.Length != 0)
				{
					reason = "Order too large";
				}
				else
				{
					reason = "Order won't fit in inventory";
				}
				return false;
			}
			if (!this.CanPlayerAffordCart())
			{
				if (this.Shop.PaymentType == ShopInterface.EPaymentType.Cash)
				{
					reason = "Insufficient cash. Visit an ATM to withdraw cash.";
				}
				else if (this.Shop.PaymentType == ShopInterface.EPaymentType.Online)
				{
					reason = "Insufficient online balance. Visit an ATM to deposit cash.";
				}
				else
				{
					reason = "Insufficient funds";
				}
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x0012BAC8 File Offset: 0x00129CC8
		private bool GetWarning(out string warning)
		{
			warning = string.Empty;
			if (this.Shop.GetLoadingBayVehicle() != null && this.LoadVehicleToggle.isOn)
			{
				List<ItemSlot> itemSlots = this.Shop.GetLoadingBayVehicle().Storage.ItemSlots;
				if (!this.Shop.WillCartFit(itemSlots))
				{
					warning = "Vehicle won't fit everything. Some items will be placed on the pallets.";
					return true;
				}
			}
			else
			{
				List<ItemSlot> availableSlots = PlayerSingleton<PlayerInventory>.Instance.hotbarSlots.Cast<ItemSlot>().ToList<ItemSlot>();
				if (!this.Shop.WillCartFit(availableSlots))
				{
					warning = "Inventory won't fit everything. Some items will be placed on the pallets.";
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x0012BB58 File Offset: 0x00129D58
		private void UpdateViewCartText()
		{
			int itemSum = this.GetItemSum();
			if (itemSum > 0)
			{
				this.ViewCartText.text = string.Concat(new string[]
				{
					"View Cart (",
					itemSum.ToString(),
					" item",
					(itemSum > 1) ? "s" : "",
					")"
				});
				return;
			}
			this.ViewCartText.text = "View Cart";
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x0012BBCC File Offset: 0x00129DCC
		private void UpdateLoadVehicleToggle()
		{
			this.LoadVehicleToggle.gameObject.SetActive(this.Shop.GetLoadingBayVehicle() != null);
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x0012BBF0 File Offset: 0x00129DF0
		private int GetItemSum()
		{
			int num = 0;
			List<ShopListing> list = this.cartDictionary.Keys.ToList<ShopListing>();
			for (int i = 0; i < list.Count; i++)
			{
				num += this.cartDictionary[list[i]];
			}
			return num;
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x0012BC38 File Offset: 0x00129E38
		private float GetPriceSum()
		{
			float num = 0f;
			List<ShopListing> list = this.cartDictionary.Keys.ToList<ShopListing>();
			for (int i = 0; i < list.Count; i++)
			{
				num += (float)this.cartDictionary[list[i]] * list[i].Price;
			}
			return num;
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x0012BC94 File Offset: 0x00129E94
		private CartEntry GetEntry(ShopListing listing)
		{
			return this.cartEntries.Find((CartEntry x) => x.Listing == listing);
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x0012BCC5 File Offset: 0x00129EC5
		private bool IsMouseOverMenuArea()
		{
			return RectTransformUtility.RectangleContainsScreenPoint(this.CartArea.rectTransform, Input.mousePosition);
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x0012BCE4 File Offset: 0x00129EE4
		public int GetTotalSlotRequirement()
		{
			ShopListing[] array = this.cartDictionary.Keys.ToArray<ShopListing>();
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = this.cartDictionary[array[i]];
				num += Mathf.CeilToInt((float)num2 / (float)array[i].Item.StackLimit);
			}
			return num;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x0012BD59 File Offset: 0x00129F59
		[CompilerGenerated]
		private IEnumerator <BopCartIcon>g__Routine|20_0()
		{
			Vector3 startScale = Vector3.one;
			Vector3 endScale = Vector3.one * 1.25f;
			float lerpTime = 0.09f;
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.CartIcon.transform.localScale = Vector3.Lerp(startScale, endScale, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			for (float i = 0f; i < lerpTime; i += Time.deltaTime)
			{
				this.CartIcon.transform.localScale = Vector3.Lerp(endScale, startScale, i / lerpTime);
				yield return new WaitForEndOfFrame();
			}
			this.CartIcon.transform.localScale = startScale;
			this.cartIconBop = null;
			yield break;
		}

		// Token: 0x040034F0 RID: 13552
		[Header("References")]
		public ShopInterface Shop;

		// Token: 0x040034F1 RID: 13553
		public Image CartIcon;

		// Token: 0x040034F2 RID: 13554
		public TextMeshProUGUI ViewCartText;

		// Token: 0x040034F3 RID: 13555
		public RectTransform CartEntryContainer;

		// Token: 0x040034F4 RID: 13556
		public TextMeshProUGUI ProblemText;

		// Token: 0x040034F5 RID: 13557
		public TextMeshProUGUI WarningText;

		// Token: 0x040034F6 RID: 13558
		public Button BuyButton;

		// Token: 0x040034F7 RID: 13559
		public RectTransform CartContainer;

		// Token: 0x040034F8 RID: 13560
		public Image CartArea;

		// Token: 0x040034F9 RID: 13561
		public TextMeshProUGUI TotalText;

		// Token: 0x040034FA RID: 13562
		public Toggle LoadVehicleToggle;

		// Token: 0x040034FB RID: 13563
		[Header("Prefabs")]
		public CartEntry EntryPrefab;

		// Token: 0x040034FC RID: 13564
		public Dictionary<ShopListing, int> cartDictionary = new Dictionary<ShopListing, int>();

		// Token: 0x040034FD RID: 13565
		private Coroutine cartIconBop;

		// Token: 0x040034FE RID: 13566
		private List<CartEntry> cartEntries = new List<CartEntry>();
	}
}
