using System;
using System.Collections.Generic;
using ScheduleOne.Delivery;
using ScheduleOne.DevUtilities;
using ScheduleOne.Money;
using ScheduleOne.Property;
using ScheduleOne.UI.Shop;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Delivery
{
	// Token: 0x02000AAA RID: 2730
	public class DeliveryShop : MonoBehaviour
	{
		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x0600496B RID: 18795 RVA: 0x00132FFF File Offset: 0x001311FF
		// (set) Token: 0x0600496C RID: 18796 RVA: 0x00133007 File Offset: 0x00131207
		public ShopInterface MatchingShop { get; private set; }

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x0600496D RID: 18797 RVA: 0x00133010 File Offset: 0x00131210
		// (set) Token: 0x0600496E RID: 18798 RVA: 0x00133018 File Offset: 0x00131218
		public bool IsExpanded { get; private set; }

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x00133021 File Offset: 0x00131221
		// (set) Token: 0x06004970 RID: 18800 RVA: 0x00133029 File Offset: 0x00131229
		public bool IsAvailable { get; private set; }

		// Token: 0x06004971 RID: 18801 RVA: 0x00133034 File Offset: 0x00131234
		private void Start()
		{
			this.MatchingShop = NetworkSingleton<DeliveryManager>.Instance.AllShops.Find((ShopInterface x) => x.ShopName == this.MatchingShopInterfaceName);
			if (this.MatchingShop == null)
			{
				Debug.LogError("Could not find shop interface with name " + this.MatchingShopInterfaceName);
				return;
			}
			foreach (ShopListing shopListing in this.MatchingShop.Listings)
			{
				if (shopListing.CanBeDelivered)
				{
					ListingEntry listingEntry = UnityEngine.Object.Instantiate<ListingEntry>(this.ListingEntryPrefab, this.ListingContainer);
					listingEntry.Initialize(shopListing);
					listingEntry.onQuantityChanged.AddListener(new UnityAction(this.RefreshCart));
					this.listingEntries.Add(listingEntry);
				}
			}
			this.DeliveryFeeLabel.text = MoneyManager.FormatAmount(this.DeliveryFee, false, false);
			int num = Mathf.CeilToInt((float)this.listingEntries.Count / 2f);
			this.ContentsContainer.sizeDelta = new Vector2(this.ContentsContainer.sizeDelta.x, 230f + (float)num * 60f);
			this.HeaderButton.onClick.AddListener(delegate()
			{
				this.SetIsExpanded(!this.IsExpanded);
			});
			this.OrderButton.onClick.AddListener(new UnityAction(this.OrderPressed));
			this.DestinationDropdown.onValueChanged.AddListener(new UnityAction<int>(this.DestinationDropdownSelected));
			this.LoadingDockDropdown.onValueChanged.AddListener(new UnityAction<int>(this.LoadingDockDropdownSelected));
			this.SetIsExpanded(false);
			if (this.AvailableByDefault)
			{
				this.SetIsAvailable();
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			this.MatchingShop.DeliveryVehicle.Deactivate();
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x00133210 File Offset: 0x00131410
		private void FixedUpdate()
		{
			if (this.IsExpanded && PlayerSingleton<DeliveryApp>.Instance.isOpen)
			{
				this.RefreshOrderButton();
			}
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x0013322C File Offset: 0x0013142C
		public void SetIsExpanded(bool expanded)
		{
			this.IsExpanded = expanded;
			this.ContentsContainer.gameObject.SetActive(this.IsExpanded);
			this.HeaderImage.sprite = (this.IsExpanded ? this.HeaderImage_Expanded : this.HeaderImage_Hidden);
			this.HeaderArrow.localRotation = (this.IsExpanded ? Quaternion.Euler(0f, 0f, 270f) : Quaternion.Euler(0f, 0f, 180f));
			PlayerSingleton<DeliveryApp>.Instance.RefreshContent(true);
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x001332BF File Offset: 0x001314BF
		public void SetIsAvailable()
		{
			this.IsAvailable = true;
			base.gameObject.SetActive(true);
			PlayerSingleton<DeliveryApp>.Instance.RefreshContent(true);
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x001332E0 File Offset: 0x001314E0
		public void OrderPressed()
		{
			string str;
			if (!this.CanOrder(out str))
			{
				Debug.LogWarning("Cannot order: " + str);
				return;
			}
			float orderTotal = this.GetOrderTotal();
			List<StringIntPair> list = new List<StringIntPair>();
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				if (listingEntry.SelectedQuantity > 0)
				{
					list.Add(new StringIntPair(listingEntry.MatchingListing.Item.ID, listingEntry.SelectedQuantity));
				}
			}
			int orderItemCount = this.GetOrderItemCount();
			int timeUntilArrival = Mathf.RoundToInt(Mathf.Lerp(60f, 360f, Mathf.Clamp01((float)orderItemCount / 160f)));
			DeliveryInstance delivery = new DeliveryInstance(GUIDManager.GenerateUniqueGUID().ToString(), this.MatchingShopInterfaceName, this.destinationProperty.PropertyCode, this.loadingDockIndex - 1, list.ToArray(), EDeliveryStatus.InTransit, timeUntilArrival);
			NetworkSingleton<DeliveryManager>.Instance.SendDelivery(delivery);
			NetworkSingleton<MoneyManager>.Instance.CreateOnlineTransaction("Delivery from " + this.MatchingShop.ShopName, -orderTotal, 1f, string.Empty);
			PlayerSingleton<DeliveryApp>.Instance.PlayOrderSubmittedAnim();
			this.ResetCart();
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x00133434 File Offset: 0x00131634
		public void RefreshShop()
		{
			this.RefreshCart();
			this.RefreshOrderButton();
			this.RefreshDestinationUI();
			this.RefreshLoadingDockUI();
			this.RefreshEntryOrder();
			this.RefreshEntriesLocked();
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x0013345C File Offset: 0x0013165C
		public void ResetCart()
		{
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				listingEntry.SetQuantity(0, false);
			}
			this.RefreshCart();
			this.RefreshOrderButton();
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x001334BC File Offset: 0x001316BC
		private void RefreshCart()
		{
			this.ItemTotalLabel.text = MoneyManager.FormatAmount(this.GetCartCost(), false, false);
			this.OrderTotalLabel.text = MoneyManager.FormatAmount(this.GetOrderTotal(), false, false);
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x001334F0 File Offset: 0x001316F0
		private void RefreshOrderButton()
		{
			string text;
			if (this.CanOrder(out text))
			{
				this.OrderButton.interactable = true;
				this.OrderButtonNote.enabled = false;
				return;
			}
			this.OrderButton.interactable = false;
			this.OrderButtonNote.text = text;
			this.OrderButtonNote.enabled = true;
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00133544 File Offset: 0x00131744
		public bool CanOrder(out string reason)
		{
			reason = string.Empty;
			if (this.HasActiveDelivery())
			{
				reason = "Delivery already in progress";
				return false;
			}
			float cartCost = this.GetCartCost();
			if (this.GetOrderTotal() > NetworkSingleton<MoneyManager>.Instance.SyncAccessor_onlineBalance)
			{
				reason = "Insufficient online balance";
				return false;
			}
			if (this.destinationProperty == null)
			{
				reason = "Select a destination";
				return false;
			}
			if (this.destinationProperty.LoadingDockCount == 0)
			{
				reason = "Selected destination has no loading docks";
				return false;
			}
			if (this.loadingDockIndex == 0)
			{
				reason = "Select a loading dock";
				return false;
			}
			if (!this.WillCartFitInVehicle())
			{
				reason = "Order is too large for delivery vehicle";
				return false;
			}
			return cartCost > 0f;
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x001335E2 File Offset: 0x001317E2
		public bool HasActiveDelivery()
		{
			return !(this.destinationProperty == null) && NetworkSingleton<DeliveryManager>.Instance.GetActiveShopDelivery(this) != null;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00133604 File Offset: 0x00131804
		public bool WillCartFitInVehicle()
		{
			int num = 0;
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				if (listingEntry.SelectedQuantity != 0)
				{
					int i = listingEntry.SelectedQuantity;
					int stackLimit = listingEntry.MatchingListing.Item.StackLimit;
					while (i > 0)
					{
						if (i > stackLimit)
						{
							i -= stackLimit;
						}
						else
						{
							i = 0;
						}
						num++;
					}
				}
			}
			return num <= 16;
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00133694 File Offset: 0x00131894
		public void RefreshDestinationUI()
		{
			Property y = this.destinationProperty;
			this.destinationProperty = null;
			this.DestinationDropdown.ClearOptions();
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
			list.Add(new Dropdown.OptionData("-"));
			List<Property> potentialDestinations = this.GetPotentialDestinations();
			int num = 0;
			for (int i = 0; i < potentialDestinations.Count; i++)
			{
				list.Add(new Dropdown.OptionData(potentialDestinations[i].PropertyName));
				if (potentialDestinations[i] == y)
				{
					num = i + 1;
				}
			}
			this.DestinationDropdown.AddOptions(list);
			this.DestinationDropdown.SetValueWithoutNotify(num);
			this.DestinationDropdownSelected(num);
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x0013373C File Offset: 0x0013193C
		private void DestinationDropdownSelected(int index)
		{
			if (index > 0 && index <= this.GetPotentialDestinations().Count)
			{
				this.destinationProperty = this.GetPotentialDestinations()[index - 1];
				if (this.loadingDockIndex == 0 && this.destinationProperty.LoadingDockCount > 0)
				{
					this.loadingDockIndex = 1;
				}
			}
			else
			{
				this.destinationProperty = null;
			}
			this.RefreshLoadingDockUI();
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x0013379B File Offset: 0x0013199B
		private List<Property> GetPotentialDestinations()
		{
			return new List<Property>(Property.OwnedProperties);
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x001337A8 File Offset: 0x001319A8
		public void RefreshLoadingDockUI()
		{
			int value = this.loadingDockIndex;
			this.loadingDockIndex = 0;
			this.LoadingDockDropdown.ClearOptions();
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
			list.Add(new Dropdown.OptionData("-"));
			if (this.destinationProperty != null)
			{
				for (int i = 0; i < this.destinationProperty.LoadingDockCount; i++)
				{
					list.Add(new Dropdown.OptionData((i + 1).ToString()));
				}
			}
			this.LoadingDockDropdown.AddOptions(list);
			int num = Mathf.Clamp(value, 0, list.Count - 1);
			this.LoadingDockDropdown.SetValueWithoutNotify(num);
			this.LoadingDockDropdownSelected(num);
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x0013384E File Offset: 0x00131A4E
		private void LoadingDockDropdownSelected(int index)
		{
			this.loadingDockIndex = index;
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x00133858 File Offset: 0x00131A58
		private float GetCartCost()
		{
			float num = 0f;
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				num += (float)listingEntry.SelectedQuantity * listingEntry.MatchingListing.Price;
			}
			return num;
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x001338C4 File Offset: 0x00131AC4
		private float GetOrderTotal()
		{
			return this.GetCartCost() + this.DeliveryFee;
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x001338D4 File Offset: 0x00131AD4
		private int GetOrderItemCount()
		{
			int num = 0;
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				num += listingEntry.SelectedQuantity;
			}
			return num;
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x0013392C File Offset: 0x00131B2C
		private void RefreshEntryOrder()
		{
			List<ListingEntry> list = new List<ListingEntry>();
			List<ListingEntry> list2 = new List<ListingEntry>();
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				if (!listingEntry.MatchingListing.Item.IsPurchasable)
				{
					list2.Add(listingEntry);
				}
				else
				{
					list.Add(listingEntry);
				}
			}
			list.AddRange(list2);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetSiblingIndex(i);
			}
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x001339D8 File Offset: 0x00131BD8
		private void RefreshEntriesLocked()
		{
			foreach (ListingEntry listingEntry in this.listingEntries)
			{
				listingEntry.RefreshLocked();
			}
		}

		// Token: 0x040036C5 RID: 14021
		public const int DELIVERY_VEHICLE_SLOT_CAPACITY = 16;

		// Token: 0x040036C6 RID: 14022
		public const int DELIVERY_TIME_MIN = 60;

		// Token: 0x040036C7 RID: 14023
		public const int DELIVERY_TIME_MAX = 360;

		// Token: 0x040036C8 RID: 14024
		public const int DELIVERY_TIME_ITEM_COUNT_DIVISOR = 160;

		// Token: 0x040036CC RID: 14028
		[Header("References")]
		public Image HeaderImage;

		// Token: 0x040036CD RID: 14029
		public Button HeaderButton;

		// Token: 0x040036CE RID: 14030
		public RectTransform ContentsContainer;

		// Token: 0x040036CF RID: 14031
		public RectTransform ListingContainer;

		// Token: 0x040036D0 RID: 14032
		public Text DeliveryFeeLabel;

		// Token: 0x040036D1 RID: 14033
		public Text ItemTotalLabel;

		// Token: 0x040036D2 RID: 14034
		public Text OrderTotalLabel;

		// Token: 0x040036D3 RID: 14035
		public Button OrderButton;

		// Token: 0x040036D4 RID: 14036
		public Text OrderButtonNote;

		// Token: 0x040036D5 RID: 14037
		public Dropdown DestinationDropdown;

		// Token: 0x040036D6 RID: 14038
		public Dropdown LoadingDockDropdown;

		// Token: 0x040036D7 RID: 14039
		[Header("Settings")]
		public string MatchingShopInterfaceName = "ShopInterface";

		// Token: 0x040036D8 RID: 14040
		public float DeliveryFee = 200f;

		// Token: 0x040036D9 RID: 14041
		public bool AvailableByDefault;

		// Token: 0x040036DA RID: 14042
		public ListingEntry ListingEntryPrefab;

		// Token: 0x040036DB RID: 14043
		public Sprite HeaderImage_Hidden;

		// Token: 0x040036DC RID: 14044
		public Sprite HeaderImage_Expanded;

		// Token: 0x040036DD RID: 14045
		public RectTransform HeaderArrow;

		// Token: 0x040036DE RID: 14046
		private List<ListingEntry> listingEntries = new List<ListingEntry>();

		// Token: 0x040036DF RID: 14047
		private Property destinationProperty;

		// Token: 0x040036E0 RID: 14048
		private int loadingDockIndex;
	}
}
