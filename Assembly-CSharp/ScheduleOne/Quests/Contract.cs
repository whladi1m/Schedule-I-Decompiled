using System;
using System.Collections.Generic;
using System.Linq;
using FishNet;
using FishNet.Object;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.UI;
using ScheduleOne.UI.Handover;
using ScheduleOne.UI.Phone;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Quests
{
	// Token: 0x020002D3 RID: 723
	public class Contract : Quest
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00045C62 File Offset: 0x00043E62
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x00045C6A File Offset: 0x00043E6A
		public NetworkObject Customer { get; protected set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x00045C73 File Offset: 0x00043E73
		// (set) Token: 0x06000FA9 RID: 4009 RVA: 0x00045C7B File Offset: 0x00043E7B
		public Dealer Dealer { get; protected set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x00045C84 File Offset: 0x00043E84
		// (set) Token: 0x06000FAB RID: 4011 RVA: 0x00045C8C File Offset: 0x00043E8C
		public float Payment { get; protected set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00045C95 File Offset: 0x00043E95
		// (set) Token: 0x06000FAD RID: 4013 RVA: 0x00045C9D File Offset: 0x00043E9D
		public int PickupScheduleIndex { get; protected set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x00045CA6 File Offset: 0x00043EA6
		// (set) Token: 0x06000FAF RID: 4015 RVA: 0x00045CAE File Offset: 0x00043EAE
		public GameDateTime AcceptTime { get; protected set; }

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00045CB7 File Offset: 0x00043EB7
		protected override void Start()
		{
			this.autoInitialize = false;
			base.Start();
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00045CC8 File Offset: 0x00043EC8
		public virtual void InitializeContract(string title, string description, QuestEntryData[] entries, string guid, NetworkObject customer, float payment, ProductList products, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, int pickupScheduleIndex, GameDateTime acceptTime)
		{
			this.SilentlyInitializeContract(this.title, this.Description, entries, guid, customer, payment, products, deliveryLocationGUID, deliveryWindow, pickupScheduleIndex, acceptTime);
			Debug.Log("Contract initialized");
			Contract.Contracts.Add(this);
			base.InitializeQuest(title, description, entries, guid);
			this.Customer.GetComponent<Customer>().AssignContract(this);
			if (this.DeliveryLocation != null && !this.DeliveryLocation.ScheduledContracts.Contains(this))
			{
				this.DeliveryLocation.ScheduledContracts.Add(this);
			}
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00045D5C File Offset: 0x00043F5C
		public virtual void SilentlyInitializeContract(string title, string description, QuestEntryData[] entries, string guid, NetworkObject customer, float payment, ProductList products, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, int pickupScheduleIndex, GameDateTime acceptTime)
		{
			this.Customer = customer;
			this.Payment = Mathf.Clamp(payment, 0f, float.MaxValue);
			this.ProductList = products;
			if (GUIDManager.IsGUIDValid(deliveryLocationGUID))
			{
				this.DeliveryLocation = GUIDManager.GetObject<DeliveryLocation>(new Guid(deliveryLocationGUID));
			}
			this.DeliveryWindow = deliveryWindow;
			this.PickupScheduleIndex = pickupScheduleIndex;
			this.AcceptTime = acceptTime;
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00045DC3 File Offset: 0x00043FC3
		protected override void MinPass()
		{
			base.MinPass();
			this.UpdateTiming();
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00045DD1 File Offset: 0x00043FD1
		private void OnDestroy()
		{
			Contract.Contracts.Remove(this);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00045DE0 File Offset: 0x00043FE0
		private void UpdateTiming()
		{
			if (base.Expires && this.ExpiryVisibility != EExpiryVisibility.Never)
			{
				int minsUntilExpiry = base.GetMinsUntilExpiry();
				int num = Mathf.FloorToInt((float)minsUntilExpiry / 60f);
				int num2 = minsUntilExpiry - 360;
				int num3 = Mathf.FloorToInt((float)num2 / 60f);
				if (num2 > 0)
				{
					if (num3 > 0)
					{
						base.SetSubtitle("<color=#c0c0c0ff> (Begins in " + num3.ToString() + " hrs)</color>");
						return;
					}
					base.SetSubtitle("<color=#c0c0c0ff> (Begins in " + num2.ToString() + " min)</color>");
					return;
				}
				else if (minsUntilExpiry < 120)
				{
					if (num > 0)
					{
						base.SetSubtitle(string.Concat(new string[]
						{
							"<color=#",
							ColorUtility.ToHtmlStringRGBA(this.criticalTimeBackground.color),
							"> (Expires in ",
							num.ToString(),
							" hrs)</color>"
						}));
						return;
					}
					base.SetSubtitle(string.Concat(new string[]
					{
						"<color=#",
						ColorUtility.ToHtmlStringRGBA(this.criticalTimeBackground.color),
						"> (Expires in ",
						minsUntilExpiry.ToString(),
						" min)</color>"
					}));
					return;
				}
				else
				{
					if (num > 0)
					{
						base.SetSubtitle("<color=green> (Expires in " + num.ToString() + " hrs)</color>");
						return;
					}
					base.SetSubtitle("<color=green> (Expires in " + num.ToString() + " min)</color>");
				}
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00045F48 File Offset: 0x00044148
		public override void End()
		{
			base.End();
			if (this.DeliveryLocation != null)
			{
				this.DeliveryLocation.ScheduledContracts.Remove(this);
			}
			Contract.Contracts.Remove(this);
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00045F7C File Offset: 0x0004417C
		public override void Complete(bool network = true)
		{
			if (InstanceFinder.IsServer && !this.completedContractsIncremented)
			{
				this.completedContractsIncremented = true;
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Completed_Contracts_Count", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Completed_Contracts_Count") + 1f).ToString(), true);
			}
			float lawIntensityChange = (Registry.GetItem(this.ProductList.entries[0].ProductID) as ProductDefinition).LawIntensityChange;
			Mathf.Lerp(0.5f, 2f, (float)this.ProductList.entries[0].Quantity / 25f);
			base.Complete(network);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00046026 File Offset: 0x00044226
		public void SetDealer(Dealer dealer)
		{
			this.Dealer = dealer;
			if (this.journalEntry != null)
			{
				this.journalEntry.gameObject.SetActive(this.ShouldShowJournalEntry());
			}
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00046053 File Offset: 0x00044253
		public virtual void SubmitPayment(float bonusTotal)
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.Payment + bonusTotal, true, false);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00046069 File Offset: 0x00044269
		protected override void SendExpiryReminder()
		{
			Singleton<NotificationsManager>.Instance.SendNotification("<color=#FFB43C>Deal Expiring Soon</color>", this.title, PlayerSingleton<JournalApp>.Instance.AppIcon, 5f, true);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00046090 File Offset: 0x00044290
		protected override void SendExpiredNotification()
		{
			Singleton<NotificationsManager>.Instance.SendNotification("<color=#FF6455>Deal Expired</color>", this.title, PlayerSingleton<JournalApp>.Instance.AppIcon, 5f, true);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x000460B7 File Offset: 0x000442B7
		protected override bool ShouldShowJournalEntry()
		{
			return !(this.Dealer != null) && base.ShouldShowJournalEntry();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x000460CF File Offset: 0x000442CF
		protected override bool CanExpire()
		{
			return !(Singleton<HandoverScreen>.Instance.CurrentContract == this) && !this.Customer.GetComponent<NPC>().dialogueHandler.IsPlaying && base.CanExpire();
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00046104 File Offset: 0x00044304
		public bool DoesProductListMatchSpecified(List<ItemInstance> items, bool enforceQuality)
		{
			using (List<ProductList.Entry>.Enumerator enumerator = this.ProductList.entries.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProductList.Entry entry = enumerator.Current;
					List<ItemInstance> list = (from x in items
					where x.ID == entry.ProductID
					select x).ToList<ItemInstance>();
					List<ProductItemInstance> list2 = new List<ProductItemInstance>();
					for (int i = 0; i < list.Count; i++)
					{
						list2.Add(list[i] as ProductItemInstance);
					}
					List<ProductItemInstance> list3 = new List<ProductItemInstance>();
					for (int j = 0; j < items.Count; j++)
					{
						ProductItemInstance productItemInstance = items[j] as ProductItemInstance;
						if (productItemInstance.Quality >= entry.Quality)
						{
							list3.Add(productItemInstance);
						}
					}
					int num = 0;
					for (int k = 0; k < list2.Count; k++)
					{
						num += list2[k].Quantity * list2[k].Amount;
					}
					int num2 = 0;
					for (int l = 0; l < list3.Count; l++)
					{
						num2 += list3[l].Quantity * list2[l].Amount;
					}
					if (enforceQuality)
					{
						if (num2 < entry.Quantity)
						{
							return false;
						}
					}
					else if (num < entry.Quantity)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000462A4 File Offset: 0x000444A4
		public float GetProductListMatch(List<ItemInstance> items, out int matchedProductCount)
		{
			float num = 0f;
			int totalQuantity = this.ProductList.GetTotalQuantity();
			matchedProductCount = 0;
			List<ItemInstance> list = new List<ItemInstance>();
			for (int i = 0; i < items.Count; i++)
			{
				list.Add(items[i].GetCopy(-1));
			}
			foreach (ProductList.Entry entry in this.ProductList.entries)
			{
				int num2 = entry.Quantity;
				ProductDefinition other = Registry.GetItem(entry.ProductID) as ProductDefinition;
				Dictionary<ProductItemInstance, float> matchRatings = new Dictionary<ProductItemInstance, float>();
				foreach (ItemInstance itemInstance in list)
				{
					if (itemInstance.Quantity != 0)
					{
						ProductItemInstance productItemInstance = itemInstance as ProductItemInstance;
						if (productItemInstance != null)
						{
							matchRatings.Add(productItemInstance, productItemInstance.GetSimilarity(other, entry.Quality));
						}
					}
				}
				List<ProductItemInstance> list2 = matchRatings.Keys.ToList<ProductItemInstance>();
				list2.Sort((ProductItemInstance x, ProductItemInstance y) => matchRatings[y].CompareTo(matchRatings[x]));
				for (int j = 0; j < list2.Count; j++)
				{
					int amount = list2[j].Amount;
					int quantity = list2[j].Quantity;
					int num3 = Mathf.Min(Mathf.CeilToInt((float)num2 / (float)amount), list2[j].Quantity);
					num2 -= num3 * amount;
					num += matchRatings[list2[j]] * (float)num3 * (float)amount;
					if (matchRatings[list2[j]] > 0f)
					{
						matchedProductCount += num3 * amount;
					}
					list2[j].ChangeQuantity(-num3);
				}
			}
			return num / (float)totalQuantity;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000464D8 File Offset: 0x000446D8
		public override string GetSaveString()
		{
			List<QuestEntryData> list = new List<QuestEntryData>();
			for (int i = 0; i < this.Entries.Count; i++)
			{
				list.Add(this.Entries[i].GetSaveData());
			}
			return new ContractData(base.GUID.ToString(), base.QuestState, base.IsTracked, this.title, this.Description, base.Expires, new GameDateTimeData(base.Expiry), list.ToArray(), this.Customer.GetComponent<NPC>().GUID.ToString(), this.Payment, this.ProductList, this.DeliveryLocation.GUID.ToString(), this.DeliveryWindow, this.PickupScheduleIndex, new GameDateTimeData(this.AcceptTime)).GetJson(true);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x000465C0 File Offset: 0x000447C0
		public bool ShouldSave()
		{
			return !(base.gameObject == null) && base.QuestState == EQuestState.Active;
		}

		// Token: 0x04001055 RID: 4181
		public const int DefaultExpiryTime = 2880;

		// Token: 0x04001056 RID: 4182
		public static List<Contract> Contracts = new List<Contract>();

		// Token: 0x0400105A RID: 4186
		[Header("Contract Settings")]
		public ProductList ProductList;

		// Token: 0x0400105B RID: 4187
		public DeliveryLocation DeliveryLocation;

		// Token: 0x0400105C RID: 4188
		public QuestWindowConfig DeliveryWindow;

		// Token: 0x0400105F RID: 4191
		private bool completedContractsIncremented;

		// Token: 0x020002D4 RID: 724
		public class BonusPayment
		{
			// Token: 0x06000FC4 RID: 4036 RVA: 0x000465EF File Offset: 0x000447EF
			public BonusPayment(string title, float amount)
			{
				this.Title = title;
				this.Amount = Mathf.Clamp(amount, 0f, float.MaxValue);
			}

			// Token: 0x04001060 RID: 4192
			public string Title;

			// Token: 0x04001061 RID: 4193
			public float Amount;
		}
	}
}
