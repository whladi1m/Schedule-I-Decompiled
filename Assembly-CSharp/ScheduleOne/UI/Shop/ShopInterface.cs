using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Audio;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.ItemFramework;
using ScheduleOne.Levelling;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Storage;
using ScheduleOne.Variables;
using ScheduleOne.Vehicles;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Shop
{
	// Token: 0x02000A5C RID: 2652
	public class ShopInterface : MonoBehaviour
	{
		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060047A2 RID: 18338 RVA: 0x0012C9DB File Offset: 0x0012ABDB
		// (set) Token: 0x060047A3 RID: 18339 RVA: 0x0012C9E3 File Offset: 0x0012ABE3
		public bool IsOpen { get; protected set; }

		// Token: 0x060047A4 RID: 18340 RVA: 0x0012C9EC File Offset: 0x0012ABEC
		protected virtual void Awake()
		{
			foreach (ShopListing listing in this.Listings)
			{
				this.CreateListingUI(listing);
			}
			this.ListingScrollRect.verticalNormalizedPosition = 1f;
			this.Listings = (from x in this.Listings
			orderby x.Item.Name
			select x).ToList<ShopListing>();
			this.categoryButtons = base.GetComponentsInChildren<CategoryButton>().ToList<CategoryButton>();
			this.StoreNameLabel.text = this.ShopName;
			this.ListingContainer.anchoredPosition = Vector2.zero;
			this.AmountSelector.onSubmitted.AddListener(new UnityAction<int>(this.QuantitySelected));
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x0012CAD4 File Offset: 0x0012ACD4
		protected virtual void Start()
		{
			this.RefreshShownItems();
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 7);
			this.RestockAllListings();
			foreach (ShopListing shopListing in this.Listings)
			{
				if (shopListing.Item.RequiresLevelToPurchase)
				{
					NetworkSingleton<LevelManager>.Instance.AddUnlockable(new Unlockable(shopListing.Item.RequiredRank, shopListing.Item.Name, shopListing.Item.Icon));
				}
			}
			this.IsOpen = false;
			this.Canvas.enabled = false;
			this.Container.gameObject.SetActive(false);
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x0012CBA0 File Offset: 0x0012ADA0
		private void OnValidate()
		{
			this.StoreNameLabel.text = this.ShopName;
			for (int i = 0; i < this.Listings.Count; i++)
			{
				if (!(this.Listings[i].Item == null))
				{
					string text = "(";
					for (int j = 0; j < this.Listings[i].Item.ShopCategories.Count; j++)
					{
						text = text + this.Listings[i].Item.ShopCategories[j].Category.ToString() + ", ";
					}
					text += ")";
					this.Listings[i].name = string.Concat(new string[]
					{
						this.Listings[i].Item.Name,
						" ($",
						this.Listings[i].Price.ToString(),
						") ",
						text
					});
					if (this.Listings[i].Item.RequiresLevelToPurchase)
					{
						ShopListing shopListing = this.Listings[i];
						string name = shopListing.name;
						string str = " [Rank ";
						FullRank requiredRank = this.Listings[i].Item.RequiredRank;
						shopListing.name = name + str + requiredRank.ToString() + "]";
					}
				}
			}
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x0012CD2E File Offset: 0x0012AF2E
		protected virtual void Update()
		{
			if (this.IsOpen && Input.GetMouseButtonUp(0))
			{
				if (this.dropdownMouseUp)
				{
					this.AmountSelector.Close();
					this.selectedListing = null;
					return;
				}
				this.dropdownMouseUp = true;
			}
		}

		// Token: 0x060047A8 RID: 18344 RVA: 0x0012CD64 File Offset: 0x0012AF64
		public virtual void SetIsOpen(bool isOpen)
		{
			this.IsOpen = isOpen;
			PlayerSingleton<PlayerCamera>.Instance.RemoveActiveUIElement(this.ShopName);
			if (isOpen)
			{
				PlayerSingleton<PlayerCamera>.Instance.AddActiveUIElement(this.ShopName);
				PlayerSingleton<PlayerCamera>.Instance.FreeMouse();
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(false);
				PlayerSingleton<PlayerMovement>.Instance.canMove = false;
				PlayerSingleton<PlayerInventory>.Instance.SetInventoryEnabled(true);
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(false);
				this.SelectCategory(EShopCategory.All);
				this.RefreshShownItems();
				this.ListingScrollRect.verticalNormalizedPosition = 1f;
				this.ListingScrollRect.content.anchoredPosition = Vector2.zero;
				this.RefreshUnlockStatus();
				Singleton<InputPromptsCanvas>.Instance.LoadModule("exitonly");
				if (this.ShowCurrencyHint)
				{
					this.ShowCurrencyHint = false;
					Singleton<HintDisplay>.Instance.ShowHint("Your <h1>online balance</h> is displayed in the top right corner.", 10f);
					base.Invoke("Hint", 10.5f);
				}
			}
			else
			{
				PlayerSingleton<PlayerCamera>.Instance.LockMouse();
				PlayerSingleton<PlayerCamera>.Instance.SetCanLook(true);
				PlayerSingleton<PlayerMovement>.Instance.canMove = true;
				PlayerSingleton<PlayerInventory>.Instance.SetEquippingEnabled(true);
				Singleton<InputPromptsCanvas>.Instance.UnloadModule();
				this.DetailPanel.Close();
				this.AmountSelector.Close();
			}
			this.Canvas.enabled = isOpen;
			this.Container.gameObject.SetActive(isOpen);
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x0012CEB8 File Offset: 0x0012B0B8
		private void Hint()
		{
			Singleton<HintDisplay>.Instance.ShowHint("Most legal shops will only accept <h1>card payments</h>, while most illegal shops only take cash. Visit an <h1>ATM</h> to deposit and withdraw cash.", 20f);
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x0012CECE File Offset: 0x0012B0CE
		protected virtual void Exit(ExitAction action)
		{
			if (action.used)
			{
				return;
			}
			if (this.IsOpen)
			{
				action.used = true;
				this.SetIsOpen(false);
			}
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x0012CEF0 File Offset: 0x0012B0F0
		private void CreateListingUI(ShopListing listing)
		{
			ListingUI component = UnityEngine.Object.Instantiate<GameObject>(this.ListingUIPrefab.gameObject, this.ListingContainer).GetComponent<ListingUI>();
			component.Initialize(listing);
			ListingUI ui = component;
			ListingUI listingUI = component;
			listingUI.onClicked = (Action)Delegate.Combine(listingUI.onClicked, new Action(delegate()
			{
				this.ListingClicked(ui);
			}));
			ListingUI listingUI2 = component;
			listingUI2.onDropdownClicked = (Action)Delegate.Combine(listingUI2.onDropdownClicked, new Action(delegate()
			{
				this.DropdownClicked(ui);
			}));
			ListingUI listingUI3 = component;
			listingUI3.hoverStart = (Action)Delegate.Combine(listingUI3.hoverStart, new Action(delegate()
			{
				this.EntryHovered(ui);
			}));
			ListingUI listingUI4 = component;
			listingUI4.hoverEnd = (Action)Delegate.Combine(listingUI4.hoverEnd, new Action(this.EntryUnhovered));
			this.listingUI.Add(component);
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x0012CFC8 File Offset: 0x0012B1C8
		public void SelectCategory(EShopCategory category)
		{
			CategoryButton categoryButton = this.categoryButtons.Find((CategoryButton x) => x.Category == category);
			if (categoryButton == null)
			{
				Console.LogWarning("Category button not found: " + category.ToString(), null);
				return;
			}
			categoryButton.Select();
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x0012D02C File Offset: 0x0012B22C
		public virtual void ListingClicked(ListingUI listingUI)
		{
			if (!listingUI.Listing.Item.IsPurchasable)
			{
				return;
			}
			int quantity = 1;
			if (this.AmountSelector.IsOpen)
			{
				quantity = this.AmountSelector.SelectedAmount;
			}
			this.Cart.AddItem(listingUI.Listing, quantity);
			this.AddItemSound.Play();
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x0012D084 File Offset: 0x0012B284
		private void ShowCartAnimation(ListingUI listing)
		{
			ShopInterface.<>c__DisplayClass42_0 CS$<>8__locals1 = new ShopInterface.<>c__DisplayClass42_0();
			CS$<>8__locals1.listing = listing;
			CS$<>8__locals1.<>4__this = this;
			base.StartCoroutine(CS$<>8__locals1.<ShowCartAnimation>g__Routine|0());
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x0012D0B2 File Offset: 0x0012B2B2
		public void CategorySelected(EShopCategory category)
		{
			if (category == this.categoryFilter)
			{
				return;
			}
			this.DeselectCurrentCategory();
			this.categoryFilter = category;
			this.RefreshShownItems();
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x0012D0D1 File Offset: 0x0012B2D1
		private void DeselectCurrentCategory()
		{
			this.categoryButtons.Find((CategoryButton x) => x.Category == this.categoryFilter).Deselect();
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x0012D0F0 File Offset: 0x0012B2F0
		private void RefreshShownItems()
		{
			for (int i = 0; i < this.listingUI.Count; i++)
			{
				if (this.searchTerm != string.Empty)
				{
					this.listingUI[i].gameObject.SetActive(this.listingUI[i].Listing.DoesListingMatchSearchTerm(this.searchTerm));
				}
				else
				{
					this.listingUI[i].gameObject.SetActive(this.listingUI[i].Listing.DoesListingMatchCategoryFilter(this.categoryFilter) && this.listingUI[i].Listing.ShouldShow());
				}
			}
			for (int j = 0; j < this.listingUI.Count; j++)
			{
				this.listingUI[j].transform.SetSiblingIndex(j);
			}
			List<ListingUI> list = this.listingUI.FindAll((ListingUI x) => !x.Listing.Item.IsPurchasable);
			list.Sort((ListingUI x, ListingUI y) => x.Listing.Item.RequiredRank.CompareTo(y.Listing.Item.RequiredRank));
			for (int k = 0; k < list.Count; k++)
			{
				list[k].transform.SetAsLastSibling();
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x0012D24C File Offset: 0x0012B44C
		private void RefreshUnlockStatus()
		{
			for (int i = 0; i < this.listingUI.Count; i++)
			{
				this.listingUI[i].UpdateLockStatus();
			}
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0012D280 File Offset: 0x0012B480
		private void RestockAllListings()
		{
			foreach (ShopListing shopListing in this.Listings)
			{
				shopListing.Restock();
			}
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x000022C9 File Offset: 0x000004C9
		public bool CanCartFitItem(ShopListing listing)
		{
			return true;
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x0012D2D0 File Offset: 0x0012B4D0
		public bool WillCartFit()
		{
			List<ItemSlot> availableSlots = this.GetAvailableSlots();
			return this.WillCartFit(availableSlots);
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x0012D2EC File Offset: 0x0012B4EC
		public bool WillCartFit(List<ItemSlot> availableSlots)
		{
			List<ShopListing> list = this.Cart.cartDictionary.Keys.ToList<ShopListing>();
			List<ItemSlot> list2 = new List<ItemSlot>();
			for (int i = 0; i < list.Count; i++)
			{
				int num = this.Cart.cartDictionary[list[i]];
				ItemInstance defaultInstance = list[i].Item.GetDefaultInstance(1);
				int num2 = 0;
				while (num2 < availableSlots.Count && num > 0)
				{
					if (!list2.Contains(availableSlots[num2]))
					{
						int capacityForItem = availableSlots[num2].GetCapacityForItem(defaultInstance);
						if (capacityForItem > 0)
						{
							list2.Add(availableSlots[num2]);
							num -= Mathf.Min(num, capacityForItem);
						}
					}
					num2++;
				}
				if (num > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0012D3B8 File Offset: 0x0012B5B8
		public virtual bool HandoverItems()
		{
			List<ItemSlot> availableSlots = this.GetAvailableSlots();
			List<ShopListing> list = this.Cart.cartDictionary.Keys.ToList<ShopListing>();
			bool result = true;
			for (int i = 0; i < list.Count; i++)
			{
				NetworkSingleton<VariableDatabase>.Instance.NotifyItemAcquired(list[i].Item.ID, this.Cart.cartDictionary[list[i]]);
				int num = this.Cart.cartDictionary[list[i]];
				ItemInstance defaultInstance = list[i].Item.GetDefaultInstance(1);
				int num2 = 0;
				while (num2 < availableSlots.Count && num > 0)
				{
					int capacityForItem = availableSlots[num2].GetCapacityForItem(defaultInstance);
					if (capacityForItem != 0)
					{
						int num3 = Mathf.Min(capacityForItem, num);
						availableSlots[num2].AddItem(defaultInstance.GetCopy(num3), false);
						num -= num3;
					}
					num2++;
				}
				if (num > 0)
				{
					Debug.LogWarning("Failed to handover all items in cart: " + defaultInstance.Name);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0012D4D0 File Offset: 0x0012B6D0
		public List<ItemSlot> GetAvailableSlots()
		{
			List<ItemSlot> list = new List<ItemSlot>();
			LandVehicle loadingBayVehicle = this.GetLoadingBayVehicle();
			if (loadingBayVehicle != null && this.Cart.LoadVehicleToggle.isOn)
			{
				list.AddRange(loadingBayVehicle.Storage.ItemSlots);
			}
			else
			{
				list.AddRange(PlayerSingleton<PlayerInventory>.Instance.hotbarSlots);
			}
			for (int i = 0; i < this.DeliveryBays.Length; i++)
			{
				list.AddRange(this.DeliveryBays[i].ItemSlots);
			}
			return list;
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x0012D550 File Offset: 0x0012B750
		public LandVehicle GetLoadingBayVehicle()
		{
			if (this.LoadingBayDetector != null && this.LoadingBayDetector.closestVehicle != null && this.LoadingBayDetector.closestVehicle.IsPlayerOwned)
			{
				return this.LoadingBayDetector.closestVehicle;
			}
			return null;
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x0012D5A0 File Offset: 0x0012B7A0
		public void PlaceItemInDeliveryBay(ItemInstance item)
		{
			int num = item.Quantity;
			foreach (StorageEntity storageEntity in this.DeliveryBays)
			{
				int num2 = storageEntity.HowManyCanFit(item);
				if (num2 > 0)
				{
					ItemInstance copy = item.GetCopy(Mathf.Min(num, num2));
					storageEntity.InsertItem(copy, true);
					num -= copy.Quantity;
				}
				if (num <= 0)
				{
					break;
				}
			}
			if (num > 0)
			{
				Console.LogWarning("Could not fit all items in delivery bay!", null);
			}
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x0012D614 File Offset: 0x0012B814
		public void QuantitySelected(int amount)
		{
			if (this.selectedListing == null)
			{
				return;
			}
			if (!this.selectedListing.Listing.Item.IsPurchasable)
			{
				return;
			}
			this.Cart.AddItem(this.selectedListing.Listing, amount);
			this.AddItemSound.Play();
			this.AmountSelector.Close();
			this.selectedListing = null;
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x0012D67C File Offset: 0x0012B87C
		public void OpenAmountSelector(ListingUI listing)
		{
			this.selectedListing = listing;
			this.AmountSelector.transform.position = listing.TopDropdownAnchor.position;
			this.dropdownMouseUp = false;
			this.AmountSelector.Open();
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0012D6B2 File Offset: 0x0012B8B2
		private void DropdownClicked(ListingUI listing)
		{
			if (this.selectedListing == listing)
			{
				this.AmountSelector.Close();
				this.selectedListing = null;
				return;
			}
			this.OpenAmountSelector(listing);
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x0012D6DC File Offset: 0x0012B8DC
		private void EntryHovered(ListingUI listing)
		{
			this.DetailPanel.Open(listing);
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x0012D6EA File Offset: 0x0012B8EA
		private void EntryUnhovered()
		{
			this.DetailPanel.Close();
		}

		// Token: 0x04003546 RID: 13638
		public const int MAX_ITEM_QUANTITY = 999;

		// Token: 0x04003548 RID: 13640
		[Header("Settings")]
		public string ShopName = "Shop";

		// Token: 0x04003549 RID: 13641
		public ShopInterface.EPaymentType PaymentType;

		// Token: 0x0400354A RID: 13642
		public bool ShowCurrencyHint;

		// Token: 0x0400354B RID: 13643
		[Header("Listings")]
		public List<ShopListing> Listings = new List<ShopListing>();

		// Token: 0x0400354C RID: 13644
		[Header("References")]
		public Canvas Canvas;

		// Token: 0x0400354D RID: 13645
		public RectTransform Container;

		// Token: 0x0400354E RID: 13646
		public RectTransform ListingContainer;

		// Token: 0x0400354F RID: 13647
		public TextMeshProUGUI StoreNameLabel;

		// Token: 0x04003550 RID: 13648
		public Cart Cart;

		// Token: 0x04003551 RID: 13649
		public StorageEntity[] DeliveryBays;

		// Token: 0x04003552 RID: 13650
		public VehicleDetector LoadingBayDetector;

		// Token: 0x04003553 RID: 13651
		public ShopInterfaceDetailPanel DetailPanel;

		// Token: 0x04003554 RID: 13652
		public ScrollRect ListingScrollRect;

		// Token: 0x04003555 RID: 13653
		public ShopAmountSelector AmountSelector;

		// Token: 0x04003556 RID: 13654
		public DeliveryVehicle DeliveryVehicle;

		// Token: 0x04003557 RID: 13655
		[Header("Audio")]
		public AudioSourceController AddItemSound;

		// Token: 0x04003558 RID: 13656
		public AudioSourceController RemoveItemSound;

		// Token: 0x04003559 RID: 13657
		public AudioSourceController CheckoutSound;

		// Token: 0x0400355A RID: 13658
		[Header("Prefabs")]
		public ListingUI ListingUIPrefab;

		// Token: 0x0400355B RID: 13659
		public UnityEvent onOrderCompleted;

		// Token: 0x0400355C RID: 13660
		[SerializeField]
		private List<CategoryButton> categoryButtons = new List<CategoryButton>();

		// Token: 0x0400355D RID: 13661
		private EShopCategory categoryFilter;

		// Token: 0x0400355E RID: 13662
		private string searchTerm = string.Empty;

		// Token: 0x0400355F RID: 13663
		private List<ListingUI> listingUI = new List<ListingUI>();

		// Token: 0x04003560 RID: 13664
		private ListingUI selectedListing;

		// Token: 0x04003561 RID: 13665
		private bool dropdownMouseUp;

		// Token: 0x02000A5D RID: 2653
		public enum EPaymentType
		{
			// Token: 0x04003563 RID: 13667
			Cash,
			// Token: 0x04003564 RID: 13668
			Online,
			// Token: 0x04003565 RID: 13669
			PreferCash,
			// Token: 0x04003566 RID: 13670
			PreferOnline
		}
	}
}
