using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.Economy;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.Product;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone.Messages
{
	// Token: 0x02000AAD RID: 2733
	public class DealerManagementApp : App<DealerManagementApp>
	{
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x0600499D RID: 18845 RVA: 0x00133EDD File Offset: 0x001320DD
		// (set) Token: 0x0600499E RID: 18846 RVA: 0x00133EE5 File Offset: 0x001320E5
		public Dealer SelectedDealer { get; private set; }

		// Token: 0x0600499F RID: 18847 RVA: 0x00133EF0 File Offset: 0x001320F0
		protected override void Awake()
		{
			base.Awake();
			foreach (Dealer dealer in Dealer.AllDealers)
			{
				if (dealer.IsRecruited)
				{
					this.AddDealer(dealer);
				}
			}
			Dealer.onDealerRecruited = (Action<Dealer>)Delegate.Combine(Dealer.onDealerRecruited, new Action<Dealer>(this.AddDealer));
			this.BackButton.onClick.AddListener(new UnityAction(this.BackPressed));
			this.NextButton.onClick.AddListener(new UnityAction(this.NextPressed));
			this.AssignCustomerButton.onClick.AddListener(new UnityAction(this.AssignCustomer));
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00133FC4 File Offset: 0x001321C4
		protected override void Start()
		{
			base.Start();
			this.CustomerSelector.onCustomerSelected.AddListener(new UnityAction<Customer>(this.AddCustomer));
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00133FE8 File Offset: 0x001321E8
		protected override void OnDestroy()
		{
			Dealer.onDealerRecruited = (Action<Dealer>)Delegate.Remove(Dealer.onDealerRecruited, new Action<Dealer>(this.AddDealer));
			base.OnDestroy();
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00134010 File Offset: 0x00132210
		public override void SetOpen(bool open)
		{
			if (this.SelectedDealer != null)
			{
				this.SetDisplayedDealer(this.SelectedDealer);
			}
			else if (this.dealers.Count > 0)
			{
				this.SetDisplayedDealer(this.dealers[0]);
			}
			else
			{
				this.NoDealersLabel.gameObject.SetActive(true);
				this.Content.gameObject.SetActive(false);
			}
			base.SetOpen(open);
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x00134084 File Offset: 0x00132284
		public void SetDisplayedDealer(Dealer dealer)
		{
			this.SelectedDealer = dealer;
			this.SelectorImage.sprite = dealer.MugshotSprite;
			this.SelectorTitle.text = dealer.fullName;
			this.CashLabel.text = MoneyManager.FormatAmount(dealer.Cash, false, false);
			this.CutLabel.text = Mathf.RoundToInt(dealer.Cut * 100f).ToString() + "%";
			this.HomeLabel.text = dealer.HomeName;
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			List<string> list = new List<string>();
			foreach (ItemSlot itemSlot in dealer.GetAllSlots())
			{
				if (itemSlot.Quantity != 0)
				{
					int num = itemSlot.Quantity;
					if (itemSlot.ItemInstance is ProductItemInstance)
					{
						num *= ((ProductItemInstance)itemSlot.ItemInstance).Amount;
					}
					if (list.Contains(itemSlot.ItemInstance.ID))
					{
						Dictionary<string, int> dictionary2 = dictionary;
						string id = itemSlot.ItemInstance.ID;
						dictionary2[id] += num;
					}
					else
					{
						list.Add(itemSlot.ItemInstance.ID);
						dictionary.Add(itemSlot.ItemInstance.ID, num);
					}
				}
			}
			for (int i = 0; i < this.InventoryEntries.Length; i++)
			{
				if (list.Count > i)
				{
					ItemDefinition item = Registry.GetItem(list[i]);
					this.InventoryEntries[i].Find("Image").GetComponent<Image>().sprite = item.Icon;
					this.InventoryEntries[i].Find("Title").GetComponent<Text>().text = dictionary[list[i]].ToString() + "x " + item.Name;
					this.InventoryEntries[i].gameObject.SetActive(true);
				}
				else
				{
					this.InventoryEntries[i].gameObject.SetActive(false);
				}
			}
			this.CustomerTitleLabel.text = string.Concat(new string[]
			{
				"Assigned Customers (",
				dealer.AssignedCustomers.Count.ToString(),
				"/",
				8.ToString(),
				")"
			});
			for (int j = 0; j < this.CustomerEntries.Length; j++)
			{
				if (dealer.AssignedCustomers.Count > j)
				{
					Customer customer = dealer.AssignedCustomers[j];
					this.CustomerEntries[j].Find("Mugshot").GetComponent<Image>().sprite = customer.NPC.MugshotSprite;
					this.CustomerEntries[j].Find("Name").GetComponent<Text>().text = customer.NPC.fullName;
					Button component = this.CustomerEntries[j].Find("Remove").GetComponent<Button>();
					component.onClick.RemoveAllListeners();
					component.onClick.AddListener(delegate()
					{
						this.RemoveCustomer(customer);
					});
					this.CustomerEntries[j].gameObject.SetActive(true);
				}
				else
				{
					this.CustomerEntries[j].gameObject.SetActive(false);
				}
			}
			this.BackButton.interactable = (this.dealers.IndexOf(dealer) > 0);
			this.NextButton.interactable = (this.dealers.IndexOf(dealer) < this.dealers.Count - 1);
			this.AssignCustomerButton.gameObject.SetActive(dealer.AssignedCustomers.Count < 8);
			this.NoDealersLabel.gameObject.SetActive(false);
			this.Content.gameObject.SetActive(true);
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x001344AC File Offset: 0x001326AC
		private void AddDealer(Dealer dealer)
		{
			if (this.dealers.Contains(dealer))
			{
				return;
			}
			this.dealers.Add(dealer);
			this.dealers = (from d in this.dealers
			orderby d.FirstName
			select d).ToList<Dealer>();
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x00134509 File Offset: 0x00132709
		private void AddCustomer(Customer customer)
		{
			this.SelectedDealer.SendAddCustomer(customer.NPC.ID);
			if (customer.OfferedContractInfo != null)
			{
				Console.Log("Expiring...", null);
				customer.ExpireOffer();
			}
			this.SetDisplayedDealer(this.SelectedDealer);
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x00134546 File Offset: 0x00132746
		private void RemoveCustomer(Customer customer)
		{
			this.SelectedDealer.SendRemoveCustomer(customer.NPC.ID);
			this.SetDisplayedDealer(this.SelectedDealer);
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x0013456C File Offset: 0x0013276C
		private void BackPressed()
		{
			int num = this.dealers.IndexOf(this.SelectedDealer);
			if (num > 0)
			{
				this.SetDisplayedDealer(this.dealers[num - 1]);
			}
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x001345A4 File Offset: 0x001327A4
		private void NextPressed()
		{
			int num = this.dealers.IndexOf(this.SelectedDealer);
			if (num < this.dealers.Count - 1)
			{
				this.SetDisplayedDealer(this.dealers[num + 1]);
			}
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x001345E7 File Offset: 0x001327E7
		public void AssignCustomer()
		{
			this.CustomerSelector.Open();
		}

		// Token: 0x040036F8 RID: 14072
		[Header("References")]
		public Text NoDealersLabel;

		// Token: 0x040036F9 RID: 14073
		public RectTransform Content;

		// Token: 0x040036FA RID: 14074
		public CustomerSelector CustomerSelector;

		// Token: 0x040036FB RID: 14075
		[Header("Selector")]
		public Image SelectorImage;

		// Token: 0x040036FC RID: 14076
		public Text SelectorTitle;

		// Token: 0x040036FD RID: 14077
		public Button BackButton;

		// Token: 0x040036FE RID: 14078
		public Button NextButton;

		// Token: 0x040036FF RID: 14079
		[Header("Basic Info")]
		public Text CashLabel;

		// Token: 0x04003700 RID: 14080
		public Text CutLabel;

		// Token: 0x04003701 RID: 14081
		public Text HomeLabel;

		// Token: 0x04003702 RID: 14082
		[Header("Inventory")]
		public RectTransform[] InventoryEntries;

		// Token: 0x04003703 RID: 14083
		[Header("Customers")]
		public Text CustomerTitleLabel;

		// Token: 0x04003704 RID: 14084
		public RectTransform[] CustomerEntries;

		// Token: 0x04003705 RID: 14085
		public Button AssignCustomerButton;

		// Token: 0x04003706 RID: 14086
		private List<Dealer> dealers = new List<Dealer>();
	}
}
