using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ScheduleOne.UI.Phone
{
	// Token: 0x02000A80 RID: 2688
	public class CustomerSelector : MonoBehaviour
	{
		// Token: 0x06004860 RID: 18528 RVA: 0x0012F084 File Offset: 0x0012D284
		public void Awake()
		{
			for (int i = 0; i < Customer.UnlockedCustomers.Count; i++)
			{
				this.CreateEntry(Customer.UnlockedCustomers[i]);
			}
			Customer.onCustomerUnlocked = (Action<Customer>)Delegate.Combine(Customer.onCustomerUnlocked, new Action<Customer>(this.CreateEntry));
			this.Close();
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x0012F0DD File Offset: 0x0012D2DD
		public void Start()
		{
			GameInput.RegisterExitListener(new GameInput.ExitDelegate(this.Exit), 7);
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0012F0F1 File Offset: 0x0012D2F1
		private void OnDestroy()
		{
			Customer.onCustomerUnlocked = (Action<Customer>)Delegate.Remove(Customer.onCustomerUnlocked, new Action<Customer>(this.CreateEntry));
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0012F113 File Offset: 0x0012D313
		private void Exit(ExitAction action)
		{
			if (action == null)
			{
				return;
			}
			if (action.used)
			{
				return;
			}
			if (this != null && base.gameObject != null && base.gameObject.activeInHierarchy)
			{
				action.used = true;
				this.Close();
			}
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x0012F154 File Offset: 0x0012D354
		public void Open()
		{
			for (int i = 0; i < this.customerEntries.Count; i++)
			{
				if (this.entryToCustomer[this.customerEntries[i]].AssignedDealer != null)
				{
					this.customerEntries[i].gameObject.SetActive(false);
				}
				else
				{
					this.customerEntries[i].gameObject.SetActive(true);
				}
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x000BEE78 File Offset: 0x000BD078
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x0012F1D8 File Offset: 0x0012D3D8
		private void CreateEntry(Customer customer)
		{
			RectTransform component = UnityEngine.Object.Instantiate<GameObject>(this.ButtonPrefab, this.EntriesContainer).GetComponent<RectTransform>();
			component.Find("Mugshot").GetComponent<Image>().sprite = customer.NPC.MugshotSprite;
			component.Find("Name").GetComponent<Text>().text = customer.NPC.fullName;
			component.GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.CustomerSelected(customer);
			});
			this.customerEntries.Add(component);
			this.entryToCustomer.Add(component, customer);
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x0012F294 File Offset: 0x0012D494
		private void CustomerSelected(Customer customer)
		{
			if (customer.AssignedDealer == null && this.onCustomerSelected != null)
			{
				this.onCustomerSelected.Invoke(customer);
			}
			this.Close();
		}

		// Token: 0x040035B8 RID: 13752
		public GameObject ButtonPrefab;

		// Token: 0x040035B9 RID: 13753
		[Header("References")]
		public RectTransform EntriesContainer;

		// Token: 0x040035BA RID: 13754
		public UnityEvent<Customer> onCustomerSelected;

		// Token: 0x040035BB RID: 13755
		private List<RectTransform> customerEntries = new List<RectTransform>();

		// Token: 0x040035BC RID: 13756
		private Dictionary<RectTransform, Customer> entryToCustomer = new Dictionary<RectTransform, Customer>();
	}
}
