using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Object.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Serializing;
using FishNet.Serializing.Generated;
using FishNet.Transporting;
using ScheduleOne.DevUtilities;
using ScheduleOne.Dialogue;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Map;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.NPCs.Schedules;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using ScheduleOne.Product.Packaging;
using ScheduleOne.Quests;
using ScheduleOne.UI;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x0200065A RID: 1626
	public class Dealer : NPC, IItemSlotOwner
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002C7C RID: 11388 RVA: 0x000B9152 File Offset: 0x000B7352
		// (set) Token: 0x06002C7D RID: 11389 RVA: 0x000B915A File Offset: 0x000B735A
		public bool IsRecruited { get; private set; }

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002C7E RID: 11390 RVA: 0x000B9163 File Offset: 0x000B7363
		// (set) Token: 0x06002C7F RID: 11391 RVA: 0x000B916B File Offset: 0x000B736B
		public List<ItemSlot> ItemSlots { get; set; } = new List<ItemSlot>();

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06002C80 RID: 11392 RVA: 0x000B9174 File Offset: 0x000B7374
		// (set) Token: 0x06002C81 RID: 11393 RVA: 0x000B917C File Offset: 0x000B737C
		public float Cash
		{
			[CompilerGenerated]
			get
			{
				return this.SyncAccessor_<Cash>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.sync___set_value_<Cash>k__BackingField(value, true);
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06002C82 RID: 11394 RVA: 0x000B9186 File Offset: 0x000B7386
		// (set) Token: 0x06002C83 RID: 11395 RVA: 0x000B918E File Offset: 0x000B738E
		public bool HasBeenRecommended { get; private set; }

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06002C84 RID: 11396 RVA: 0x000B9197 File Offset: 0x000B7397
		// (set) Token: 0x06002C85 RID: 11397 RVA: 0x000B919F File Offset: 0x000B739F
		public NPCPoI potentialDealerPoI { get; protected set; }

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002C86 RID: 11398 RVA: 0x000B91A8 File Offset: 0x000B73A8
		// (set) Token: 0x06002C87 RID: 11399 RVA: 0x000B91B0 File Offset: 0x000B73B0
		public NPCPoI dealerPoI { get; protected set; }

		// Token: 0x06002C88 RID: 11400 RVA: 0x000B91BC File Offset: 0x000B73BC
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Economy.Dealer_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002C89 RID: 11401 RVA: 0x000B91DB File Offset: 0x000B73DB
		protected override void OnValidate()
		{
			base.OnValidate();
			this.HomeEvent.Building = this.Home;
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x000B91F4 File Offset: 0x000B73F4
		private new void OnDestroy()
		{
			Dealer.AllDealers.Remove(this);
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x000B9204 File Offset: 0x000B7404
		protected override void Start()
		{
			base.Start();
			if (Application.isEditor)
			{
				foreach (Customer customer in this.InitialCustomers)
				{
					this.SendAddCustomer(customer.NPC.ID);
				}
				foreach (ProductDefinition productDefinition in this.InitialItems)
				{
					base.Inventory.InsertItem(productDefinition.GetDefaultInstance(10), true);
				}
			}
			for (int i = 0; i < base.Inventory.ItemSlots.Count; i++)
			{
				base.Inventory.ItemSlots[i].AddFilter(new ItemFilter_PackagedProduct());
			}
			this.SetUpDialogue();
			this.SetupPoI();
			NPCRelationData relationData = this.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.OnDealerUnlocked));
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x000B9330 File Offset: 0x000B7530
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (connection.IsLocalClient)
			{
				return;
			}
			if (this.IsRecruited)
			{
				this.SetIsRecruited(connection);
			}
			foreach (Customer customer in this.AssignedCustomers)
			{
				this.AddCustomer(connection, customer.NPC.ID);
			}
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000B93B0 File Offset: 0x000B75B0
		private void SetupPoI()
		{
			if (this.dealerPoI == null)
			{
				this.dealerPoI = UnityEngine.Object.Instantiate<NPCPoI>(NetworkSingleton<NPCManager>.Instance.NPCPoIPrefab, base.transform);
				this.dealerPoI.SetMainText(base.fullName + "\n(Dealer)");
				this.dealerPoI.SetNPC(this);
				this.dealerPoI.transform.localPosition = Vector3.zero;
				this.dealerPoI.enabled = this.IsRecruited;
			}
			if (this.potentialDealerPoI == null)
			{
				this.potentialDealerPoI = UnityEngine.Object.Instantiate<NPCPoI>(NetworkSingleton<NPCManager>.Instance.PotentialDealerPoIPrefab, base.transform);
				this.potentialDealerPoI.SetMainText("Potential Dealer\n" + base.fullName);
				this.potentialDealerPoI.SetNPC(this);
				float y = (float)(this.FirstName[0] % '$') * 10f;
				float d = Mathf.Clamp((float)this.FirstName.Length * 1.5f, 1f, 10f);
				Vector3 vector = base.transform.forward;
				vector = Quaternion.Euler(0f, y, 0f) * vector;
				this.potentialDealerPoI.transform.localPosition = vector * d;
			}
			this.UpdatePotentialDealerPoI();
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x000B9500 File Offset: 0x000B7700
		private void SetUpDialogue()
		{
			this.recruitChoice = new DialogueController.DialogueChoice();
			this.recruitChoice.ChoiceText = "Do you want to work for me as a distributor?";
			this.recruitChoice.Enabled = !this.IsRecruited;
			this.recruitChoice.Conversation = this.RecruitDialogue;
			this.recruitChoice.onChoosen.AddListener(new UnityAction(this.RecruitmentRequested));
			this.recruitChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.CanOfferRecruitment);
			this.DialogueController.AddDialogueChoice(this.recruitChoice, 0);
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = "Nevermind";
			dialogueChoice.Enabled = true;
			this.DialogueController.AddDialogueChoice(dialogueChoice, 0);
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x000B95BC File Offset: 0x000B77BC
		protected override void MinPass()
		{
			base.MinPass();
			this.UpdatePotentialDealerPoI();
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				return;
			}
			if (this.currentContract != null)
			{
				this.UpdateCurrentDeal();
			}
			else
			{
				this.CheckAttendStart();
			}
			this.HomeEvent.gameObject.SetActive(true);
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x000B9617 File Offset: 0x000B7817
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void MarkAsRecommended()
		{
			this.RpcWriter___Server_MarkAsRecommended_2166136261();
			this.RpcLogic___MarkAsRecommended_2166136261();
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x000B9625 File Offset: 0x000B7825
		[ObserversRpc(RunLocally = true)]
		private void SetRecommended()
		{
			this.RpcWriter___Observers_SetRecommended_2166136261();
			this.RpcLogic___SetRecommended_2166136261();
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x000B9633 File Offset: 0x000B7833
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void InitialRecruitment()
		{
			this.RpcWriter___Server_InitialRecruitment_2166136261();
			this.RpcLogic___InitialRecruitment_2166136261();
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x000B9644 File Offset: 0x000B7844
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		public virtual void SetIsRecruited(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetIsRecruited_328543758(conn);
				this.RpcLogic___SetIsRecruited_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_SetIsRecruited_328543758(conn);
			}
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000B9679 File Offset: 0x000B7879
		protected virtual void OnDealerUnlocked(NPCRelationData.EUnlockType unlockType, bool b)
		{
			this.UpdatePotentialDealerPoI();
			NetworkSingleton<MoneyManager>.Instance.CashSound.Play();
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000B9690 File Offset: 0x000B7890
		protected virtual void UpdatePotentialDealerPoI()
		{
			this.potentialDealerPoI.enabled = (this.RelationData.IsMutuallyKnown() && !this.RelationData.Unlocked);
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000B96BC File Offset: 0x000B78BC
		private void TradeItems()
		{
			this.dialogueHandler.SkipNextDialogueBehaviourEnd();
			this.itemCountOnTradeStart = base.Inventory.GetTotalItemCount();
			Singleton<StorageMenu>.Instance.Open(base.Inventory, base.fullName + "'s Inventory", "Place <color=#4CB0FF>packaged product</color> here and the dealer will sell it to assigned customers");
			Singleton<StorageMenu>.Instance.onClosed.AddListener(new UnityAction(this.TradeItemsDone));
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000B9728 File Offset: 0x000B7928
		private void TradeItemsDone()
		{
			Singleton<StorageMenu>.Instance.onClosed.RemoveListener(new UnityAction(this.TradeItemsDone));
			this.behaviour.GenericDialogueBehaviour.SendDisable();
			if (base.Inventory.GetTotalItemCount() > this.itemCountOnTradeStart)
			{
				this.dialogueHandler.WorldspaceRend.ShowText("Thanks boss", 2.5f);
				base.PlayVO(EVOLineType.Thanks);
			}
			this.TryMoveOverflowItems();
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000B979A File Offset: 0x000B799A
		private bool CanCollectCash(out string reason)
		{
			reason = string.Empty;
			return this.Cash > 0f;
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000B97B3 File Offset: 0x000B79B3
		private void UpdateCollectCashChoice(float oldCash, float newCash, bool asServer)
		{
			if (this.collectCashChoice == null)
			{
				return;
			}
			this.collectCashChoice.ChoiceText = "I need to collect the earnings <color=#54E717>(" + MoneyManager.FormatAmount(this.Cash, false, false) + ")</color>";
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000B97E5 File Offset: 0x000B79E5
		private void CollectCash()
		{
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(this.Cash, true, true);
			this.SetCash(0f);
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x000B9804 File Offset: 0x000B7A04
		private void UpdateCurrentDeal()
		{
			if (this.currentContract.QuestState != EQuestState.Active)
			{
				this.currentContract.SetDealer(null);
				this.currentContract = null;
				this.DealSignal.gameObject.SetActive(false);
				return;
			}
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000B983C File Offset: 0x000B7A3C
		private bool CanOfferRecruitment(out string reason)
		{
			reason = string.Empty;
			if (this.IsRecruited)
			{
				return false;
			}
			if (!this.HasBeenRecommended)
			{
				reason = "Reach 'friendly' with one of " + this.FirstName + "'s connections";
				return false;
			}
			if (!this.RelationData.IsMutuallyKnown())
			{
				reason = "Unlock one of " + this.FirstName + "'s connections";
				return false;
			}
			return true;
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000B98A4 File Offset: 0x000B7AA4
		private void CheckAttendStart()
		{
			Contract contract = this.ActiveContracts.FirstOrDefault<Contract>();
			if (contract == null)
			{
				return;
			}
			int time = ScheduleOne.GameTime.TimeManager.AddMinutesTo24HourTime(contract.DeliveryWindow.WindowStartTime, 30);
			int num = Mathf.CeilToInt(Vector3.Distance(this.Avatar.CenterPoint, contract.DeliveryLocation.CustomerStandPoint.position) / base.Movement.WalkSpeed * 1.5f);
			num = Mathf.Clamp(num, 15, 360);
			int min = ScheduleOne.GameTime.TimeManager.AddMinutesTo24HourTime(time, -num);
			int minsUntilExpiry = contract.GetMinsUntilExpiry();
			if (NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.IsCurrentTimeWithinRange(min, contract.DeliveryWindow.WindowEndTime) || minsUntilExpiry <= 240)
			{
				Debug.Log("Dealer start attend deal: " + contract.Title);
				this.currentContract = contract;
				this.DealSignal.SetStartTime(NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.CurrentTime);
				this.DealSignal.AssignContract(contract);
				this.DealSignal.gameObject.SetActive(true);
			}
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x000B999C File Offset: 0x000B7B9C
		public virtual bool ShouldAcceptContract(ContractInfo contractInfo, Customer customer)
		{
			foreach (ProductList.Entry entry in contractInfo.Products.entries)
			{
				string productID = entry.ProductID;
				EQuality minQuality = customer.CustomerData.Standards.GetCorrespondingQuality();
				EQuality maxQuality = customer.CustomerData.Standards.GetCorrespondingQuality();
				if (this.SellInsufficientQualityItems)
				{
					minQuality = EQuality.Trash;
				}
				if (this.SellExcessQualityItems)
				{
					maxQuality = EQuality.Heavenly;
				}
				int productCount = this.GetProductCount(productID, minQuality, maxQuality);
				if (entry.Quantity > productCount)
				{
					Console.Log(string.Concat(new string[]
					{
						"Dealer ",
						base.fullName,
						" does not have enough ",
						productID,
						" for ",
						customer.NPC.fullName
					}), null);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x000B9A90 File Offset: 0x000B7C90
		public virtual void ContractedOffered(ContractInfo contractInfo, Customer customer)
		{
			if (!this.ShouldAcceptContract(contractInfo, customer))
			{
				Console.Log("Contract accepted by dealer " + base.fullName, null);
				return;
			}
			EDealWindow dealWindow = this.GetDealWindow();
			Console.Log("Contract accepted by dealer " + base.fullName + " in window " + dealWindow.ToString(), null);
			this.SyncAccessor_acceptedContractGUIDs.Add(customer.ContractAccepted(dealWindow, false));
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000B9B00 File Offset: 0x000B7D00
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendAddCustomer(string npcID)
		{
			this.RpcWriter___Server_SendAddCustomer_3615296227(npcID);
			this.RpcLogic___SendAddCustomer_3615296227(npcID);
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000B9B18 File Offset: 0x000B7D18
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void AddCustomer(NetworkConnection conn, string npcID)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_AddCustomer_2971853958(conn, npcID);
				this.RpcLogic___AddCustomer_2971853958(conn, npcID);
			}
			else
			{
				this.RpcWriter___Target_AddCustomer_2971853958(conn, npcID);
			}
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000B9B59 File Offset: 0x000B7D59
		protected virtual void AddCustomer(Customer customer)
		{
			if (this.AssignedCustomers.Contains(customer))
			{
				return;
			}
			this.AssignedCustomers.Add(customer);
			customer.AssignDealer(this);
			customer.onContractAssigned.AddListener(new UnityAction<Contract>(this.CustomerContractStarted));
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000B9B94 File Offset: 0x000B7D94
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		public void SendRemoveCustomer(string npcID)
		{
			this.RpcWriter___Server_SendRemoveCustomer_3615296227(npcID);
			this.RpcLogic___SendRemoveCustomer_3615296227(npcID);
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000B9BAC File Offset: 0x000B7DAC
		[ObserversRpc(RunLocally = true)]
		private void RemoveCustomer(string npcID)
		{
			this.RpcWriter___Observers_RemoveCustomer_3615296227(npcID);
			this.RpcLogic___RemoveCustomer_3615296227(npcID);
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000B9BCD File Offset: 0x000B7DCD
		public virtual void RemoveCustomer(Customer customer)
		{
			if (!this.AssignedCustomers.Contains(customer))
			{
				return;
			}
			this.AssignedCustomers.Remove(customer);
			customer.AssignDealer(null);
			customer.onContractAssigned.RemoveListener(new UnityAction<Contract>(this.CustomerContractStarted));
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000B9C09 File Offset: 0x000B7E09
		public void ChangeCash(float change)
		{
			this.SetCash(this.Cash + change);
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000B9C19 File Offset: 0x000B7E19
		[ServerRpc(RequireOwnership = false)]
		public void SetCash(float cash)
		{
			this.RpcWriter___Server_SetCash_431000436(cash);
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000B9C28 File Offset: 0x000B7E28
		[ServerRpc(RequireOwnership = false)]
		public virtual void CompletedDeal()
		{
			this.RpcWriter___Server_CompletedDeal_2166136261();
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000B9C3C File Offset: 0x000B7E3C
		[ServerRpc(RequireOwnership = false)]
		public void SubmitPayment(float payment)
		{
			this.RpcWriter___Server_SubmitPayment_431000436(payment);
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x000B9C54 File Offset: 0x000B7E54
		public List<ProductDefinition> GetOrderableProducts()
		{
			List<ProductDefinition> list = new List<ProductDefinition>();
			foreach (ItemSlot itemSlot in this.GetAllSlots())
			{
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is ProductItemInstance)
				{
					ProductItemInstance product = itemSlot.ItemInstance as ProductItemInstance;
					if (list.Find((ProductDefinition x) => x.ID == product.ID) == null)
					{
						list.Add(product.Definition as ProductDefinition);
					}
				}
			}
			return list;
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000B9D04 File Offset: 0x000B7F04
		public int GetProductCount(string productID, EQuality minQuality, EQuality maxQuality)
		{
			int num = 0;
			foreach (ItemSlot itemSlot in this.GetAllSlots())
			{
				if (itemSlot.ItemInstance != null && itemSlot.ItemInstance is ProductItemInstance)
				{
					ProductItemInstance productItemInstance = itemSlot.ItemInstance as ProductItemInstance;
					if (productItemInstance.ID == productID && productItemInstance.Quality >= minQuality && productItemInstance.Quality <= maxQuality)
					{
						num += productItemInstance.Quantity * productItemInstance.Amount;
					}
				}
			}
			return num;
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000B9DA4 File Offset: 0x000B7FA4
		private EDealWindow GetDealWindow()
		{
			EDealWindow window = DealWindowInfo.GetWindow(NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.CurrentTime);
			int num = (int)window;
			int num2 = ScheduleOne.GameTime.TimeManager.GetMinSumFrom24HourTime(DealWindowInfo.GetWindowInfo(window).EndTime) - ScheduleOne.GameTime.TimeManager.GetMinSumFrom24HourTime(NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.CurrentTime);
			List<EDealWindow> list = new List<EDealWindow>();
			if (num2 > 120)
			{
				list.Add(window);
			}
			for (int i = 1; i < 4; i++)
			{
				int item = (num + i) % 4;
				list.Add((EDealWindow)item);
			}
			int num3 = 3;
			for (;;)
			{
				foreach (EDealWindow edealWindow in list)
				{
					if (this.GetContractCountInWindow(edealWindow) <= num3)
					{
						return edealWindow;
					}
				}
				num3++;
			}
			EDealWindow result;
			return result;
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000B9E6C File Offset: 0x000B806C
		private int GetContractCountInWindow(EDealWindow window)
		{
			int num = 0;
			using (List<Contract>.Enumerator enumerator = this.ActiveContracts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (DealWindowInfo.GetWindow(ScheduleOne.GameTime.TimeManager.AddMinutesTo24HourTime(enumerator.Current.DeliveryWindow.WindowStartTime, 1)) == window)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000B9ED8 File Offset: 0x000B80D8
		private void CustomerContractStarted(Contract contract)
		{
			if (!this.SyncAccessor_acceptedContractGUIDs.Contains(contract.GUID.ToString()))
			{
				return;
			}
			this.ActiveContracts.Add(contract);
			contract.SetDealer(this);
			contract.onQuestEnd.AddListener(delegate(EQuestState <p0>)
			{
				this.CustomerContractEnded(contract);
			});
			contract.ShouldSendExpiredNotification = false;
			contract.ShouldSendExpiryReminder = false;
			base.Invoke("SortContracts", 0.05f);
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000B9F84 File Offset: 0x000B8184
		private void CustomerContractEnded(Contract contract)
		{
			if (!this.ActiveContracts.Contains(contract))
			{
				return;
			}
			this.ActiveContracts.Remove(contract);
			contract.SetDealer(null);
			if (InstanceFinder.IsServer && this.GetTotalInventoryItemCount() == 0)
			{
				DialogueChain chain = this.dialogueHandler.Database.GetChain(EDialogueModule.Dealer, "inventory_depleted");
				base.MSGConversation.SendMessageChain(chain.GetMessageChain(), 0f, true, true);
			}
			base.Invoke("SortContracts", 0.05f);
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000BA002 File Offset: 0x000B8202
		private void SortContracts()
		{
			this.ActiveContracts = (from x in this.ActiveContracts
			orderby x.GetMinsUntilExpiry()
			select x).ToList<Contract>();
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void RecruitmentRequested()
		{
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000BA03C File Offset: 0x000B823C
		public bool RemoveContractItems(Contract contract, EQuality targetQuality, out List<ItemInstance> items)
		{
			Dealer.<>c__DisplayClass101_0 CS$<>8__locals1 = new Dealer.<>c__DisplayClass101_0();
			CS$<>8__locals1.targetQuality = targetQuality;
			CS$<>8__locals1.<>4__this = this;
			items = new List<ItemInstance>();
			foreach (ProductList.Entry entry in contract.ProductList.entries)
			{
				int num;
				List<ItemInstance> items2 = this.GetItems(entry.ProductID, entry.Quantity, new Func<ProductItemInstance, bool>(CS$<>8__locals1.<RemoveContractItems>g__DoesQualityMatch|0), out num);
				if (num < entry.Quantity)
				{
					Console.LogWarning("Could not find enough items for contract entry: " + entry.ProductID, null);
				}
				items.AddRange(items2);
			}
			this.TryMoveOverflowItems();
			return true;
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000BA0F8 File Offset: 0x000B82F8
		private List<ItemInstance> GetItems(string ID, int requiredQuantity, Func<ProductItemInstance, bool> qualityCheck, out int returnedQuantity)
		{
			List<ItemInstance> list = new List<ItemInstance>();
			returnedQuantity = 0;
			List<ItemSlot> allSlots = this.GetAllSlots();
			for (int i = 0; i < allSlots.Count; i++)
			{
				if (allSlots[i].ItemInstance == null)
				{
					allSlots.RemoveAt(i);
					i--;
				}
				else
				{
					ProductItemInstance productItemInstance = allSlots[i].ItemInstance as ProductItemInstance;
					if (productItemInstance == null || productItemInstance.ID != ID || productItemInstance.AppliedPackaging == null || !qualityCheck(productItemInstance))
					{
						allSlots.RemoveAt(i);
						i--;
					}
				}
			}
			allSlots.Sort(delegate(ItemSlot x, ItemSlot y)
			{
				if (x.ItemInstance == null)
				{
					return 1;
				}
				if (y.ItemInstance == null)
				{
					return -1;
				}
				return (y.ItemInstance as ProductItemInstance).Amount.CompareTo((x.ItemInstance as ProductItemInstance).Amount);
			});
			foreach (ItemSlot itemSlot in allSlots)
			{
				int amount = (itemSlot.ItemInstance as ProductItemInstance).Amount;
				while (requiredQuantity >= amount && itemSlot.Quantity > 0)
				{
					list.Add(itemSlot.ItemInstance.GetCopy(1));
					itemSlot.ChangeQuantity(-1, false);
					returnedQuantity += amount;
					requiredQuantity -= amount;
				}
			}
			if (requiredQuantity > 0)
			{
				while (requiredQuantity > 0)
				{
					allSlots = this.GetAllSlots();
					for (int j = 0; j < allSlots.Count; j++)
					{
						if (allSlots[j].ItemInstance == null)
						{
							allSlots.RemoveAt(j);
							j--;
						}
						else
						{
							ProductItemInstance productItemInstance2 = allSlots[j].ItemInstance as ProductItemInstance;
							if (productItemInstance2 == null || productItemInstance2.ID != ID || productItemInstance2.AppliedPackaging == null || !qualityCheck(productItemInstance2))
							{
								allSlots.RemoveAt(j);
								j--;
							}
						}
					}
					if (allSlots.Count == 0)
					{
						Console.LogWarning("Dealer " + base.fullName + " has no items to fulfill contract", null);
						return list;
					}
					allSlots.Sort(delegate(ItemSlot x, ItemSlot y)
					{
						if (x.ItemInstance == null)
						{
							return -1;
						}
						if (y.ItemInstance == null)
						{
							return 1;
						}
						return (x.ItemInstance as ProductItemInstance).Amount.CompareTo((y.ItemInstance as ProductItemInstance).Amount);
					});
					ItemSlot itemSlot2 = allSlots[0];
					int amount2 = (itemSlot2.ItemInstance as ProductItemInstance).Amount;
					if (requiredQuantity >= amount2)
					{
						while (requiredQuantity >= amount2)
						{
							if (itemSlot2.Quantity <= 0)
							{
								break;
							}
							Console.Log(string.Concat(new string[]
							{
								"Removing 1x ",
								itemSlot2.ItemInstance.Name,
								"(",
								(itemSlot2.ItemInstance as ProductItemInstance).AppliedPackaging.Name,
								")"
							}), null);
							list.Add(itemSlot2.ItemInstance.GetCopy(1));
							itemSlot2.ChangeQuantity(-1, false);
							returnedQuantity += amount2;
							requiredQuantity -= amount2;
						}
					}
					else
					{
						PackagingDefinition appliedPackaging = (itemSlot2.ItemInstance as ProductItemInstance).AppliedPackaging;
						ProductDefinition productDefinition = (itemSlot2.ItemInstance as ProductItemInstance).Definition as ProductDefinition;
						PackagingDefinition packagingDefinition = null;
						for (int k = 0; k < productDefinition.ValidPackaging.Length; k++)
						{
							if (productDefinition.ValidPackaging[k].ID == appliedPackaging.ID && k > 0)
							{
								packagingDefinition = productDefinition.ValidPackaging[k - 1];
							}
						}
						if (packagingDefinition == null)
						{
							Console.LogWarning("Failed to find next packaging smaller than " + appliedPackaging.ID, null);
							break;
						}
						int quantity = packagingDefinition.Quantity;
						int overrideQuantity = appliedPackaging.Quantity / quantity;
						Console.Log(string.Concat(new string[]
						{
							"Splitting 1x ",
							itemSlot2.ItemInstance.Name,
							"(",
							appliedPackaging.Name,
							") into ",
							overrideQuantity.ToString(),
							"x ",
							packagingDefinition.Name
						}), null);
						ProductItemInstance productItemInstance3 = itemSlot2.ItemInstance.GetCopy(overrideQuantity) as ProductItemInstance;
						productItemInstance3.SetPackaging(packagingDefinition);
						itemSlot2.ChangeQuantity(-1, false);
						this.AddItemToInventory(productItemInstance3);
					}
				}
			}
			return list;
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000BA518 File Offset: 0x000B8718
		public List<ItemSlot> GetAllSlots()
		{
			List<ItemSlot> list = new List<ItemSlot>(base.Inventory.ItemSlots);
			list.AddRange(this.OverflowSlots);
			return list;
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000BA538 File Offset: 0x000B8738
		public void AddItemToInventory(ItemInstance item)
		{
			while (base.Inventory.CanItemFit(item, 1) && item.Quantity > 0)
			{
				base.Inventory.InsertItem(item.GetCopy(1), true);
				item.ChangeQuantity(-1);
			}
			if (item.Quantity > 0 && !ItemSlot.TryInsertItemIntoSet(this.OverflowSlots.ToList<ItemSlot>(), item))
			{
				Console.LogWarning("Dealer " + base.fullName + " has doesn't have enough space for item " + item.ID, null);
			}
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000BA5B8 File Offset: 0x000B87B8
		public void TryMoveOverflowItems()
		{
			foreach (ItemSlot itemSlot in this.OverflowSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					while (base.Inventory.CanItemFit(itemSlot.ItemInstance, 1) && itemSlot.ItemInstance.Quantity > 0)
					{
						base.Inventory.InsertItem(itemSlot.ItemInstance.GetCopy(1), true);
						itemSlot.ItemInstance.ChangeQuantity(-1);
					}
				}
			}
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000BA630 File Offset: 0x000B8830
		public int GetTotalInventoryItemCount()
		{
			List<ItemSlot> allSlots = this.GetAllSlots();
			int num = 0;
			foreach (ItemSlot itemSlot in allSlots)
			{
				if (itemSlot.ItemInstance != null)
				{
					num += itemSlot.ItemInstance.Quantity;
				}
			}
			return num;
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000BA698 File Offset: 0x000B8898
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetStoredInstance(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			this.RpcWriter___Server_SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
			this.RpcLogic___SetStoredInstance_2652194801(conn, itemSlotIndex, instance);
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x000BA6C0 File Offset: 0x000B88C0
		[ObserversRpc(RunLocally = true)]
		[TargetRpc(RunLocally = true)]
		private void SetStoredInstance_Internal(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
			else
			{
				this.RpcWriter___Target_SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
				this.RpcLogic___SetStoredInstance_Internal_2652194801(conn, itemSlotIndex, instance);
			}
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000BA71F File Offset: 0x000B891F
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetItemSlotQuantity(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Server_SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000BA73D File Offset: 0x000B893D
		[ObserversRpc(RunLocally = true)]
		private void SetItemSlotQuantity_Internal(int itemSlotIndex, int quantity)
		{
			this.RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x000BA75B File Offset: 0x000B895B
		[ServerRpc(RunLocally = true, RequireOwnership = false)]
		public void SetSlotLocked(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			this.RpcWriter___Server_SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			this.RpcLogic___SetSlotLocked_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000BA794 File Offset: 0x000B8994
		[TargetRpc(RunLocally = true)]
		[ObserversRpc(RunLocally = true)]
		private void SetSlotLocked_Internal(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
			else
			{
				this.RpcWriter___Target_SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
				this.RpcLogic___SetSlotLocked_Internal_3170825843(conn, itemSlotIndex, locked, lockOwner, lockReason);
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000BA814 File Offset: 0x000B8A14
		public override string GetSaveString()
		{
			string[] array = new string[this.AssignedCustomers.Count];
			for (int i = 0; i < this.AssignedCustomers.Count; i++)
			{
				array[i] = this.AssignedCustomers[i].NPC.ID;
			}
			string[] array2 = new string[this.ActiveContracts.Count];
			for (int j = 0; j < this.ActiveContracts.Count; j++)
			{
				array2[j] = this.ActiveContracts[j].GUID.ToString();
			}
			return new DealerData(this.ID, this.IsRecruited, array, array2, this.Cash, new ItemSet(this.OverflowSlots), this.HasBeenRecommended).GetJson(true);
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000BA8DC File Offset: 0x000B8ADC
		public override void Load(NPCData data, string containerPath)
		{
			base.Load(data, containerPath);
			string json;
			if (((ISaveable)this).TryLoadFile(containerPath, "NPC", out json))
			{
				DealerData dealerData = null;
				try
				{
					dealerData = JsonUtility.FromJson<DealerData>(json);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Failed to deserialize character data: " + ex.Message, null);
					return;
				}
				if (dealerData == null)
				{
					return;
				}
				if (dealerData.Recruited)
				{
					this.SetIsRecruited(null);
				}
				this.SetCash(dealerData.Cash);
				for (int i = 0; i < dealerData.AssignedCustomerIDs.Length; i++)
				{
					NPC npc = NPCManager.GetNPC(dealerData.AssignedCustomerIDs[i]);
					if (npc == null)
					{
						Console.LogWarning("Failed to find customer NPC with ID " + dealerData.AssignedCustomerIDs[i], null);
					}
					else
					{
						Customer component = npc.GetComponent<Customer>();
						if (component == null)
						{
							Console.LogWarning("NPC is not a customer: " + npc.fullName, null);
						}
						else
						{
							this.SendAddCustomer(component.NPC.ID);
						}
					}
				}
				if (dealerData.ActiveContractGUIDs != null)
				{
					for (int j = 0; j < dealerData.ActiveContractGUIDs.Length; j++)
					{
						if (!GUIDManager.IsGUIDValid(dealerData.ActiveContractGUIDs[j]))
						{
							Console.LogWarning("Invalid contract GUID: " + dealerData.ActiveContractGUIDs[j], null);
						}
						else
						{
							Contract @object = GUIDManager.GetObject<Contract>(new Guid(dealerData.ActiveContractGUIDs[j]));
							if (@object != null)
							{
								this.SyncAccessor_acceptedContractGUIDs.Add(@object.GUID.ToString());
								this.CustomerContractStarted(@object);
							}
						}
					}
				}
				if (dealerData.HasBeenRecommended)
				{
					this.MarkAsRecommended();
				}
				for (int k = 0; k < dealerData.OverflowItems.Items.Length; k++)
				{
					ItemInstance instance = ItemDeserializer.LoadItem(dealerData.OverflowItems.Items[k]);
					if (this.OverflowSlots.Length > k)
					{
						this.OverflowSlots[k].SetStoredItem(instance, false);
					}
				}
			}
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x000BAB98 File Offset: 0x000B8D98
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Economy.DealerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Economy.DealerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___acceptedContractGUIDs = new SyncVar<List<string>>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.acceptedContractGUIDs);
			this.syncVar___<Cash>k__BackingField = new SyncVar<float>(this, 1U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.<Cash>k__BackingField);
			this.syncVar___<Cash>k__BackingField.OnChange += this.UpdateCollectCashChoice;
			base.RegisterServerRpc(35U, new ServerRpcDelegate(this.RpcReader___Server_MarkAsRecommended_2166136261));
			base.RegisterObserversRpc(36U, new ClientRpcDelegate(this.RpcReader___Observers_SetRecommended_2166136261));
			base.RegisterServerRpc(37U, new ServerRpcDelegate(this.RpcReader___Server_InitialRecruitment_2166136261));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_SetIsRecruited_328543758));
			base.RegisterTargetRpc(39U, new ClientRpcDelegate(this.RpcReader___Target_SetIsRecruited_328543758));
			base.RegisterServerRpc(40U, new ServerRpcDelegate(this.RpcReader___Server_SendAddCustomer_3615296227));
			base.RegisterObserversRpc(41U, new ClientRpcDelegate(this.RpcReader___Observers_AddCustomer_2971853958));
			base.RegisterTargetRpc(42U, new ClientRpcDelegate(this.RpcReader___Target_AddCustomer_2971853958));
			base.RegisterServerRpc(43U, new ServerRpcDelegate(this.RpcReader___Server_SendRemoveCustomer_3615296227));
			base.RegisterObserversRpc(44U, new ClientRpcDelegate(this.RpcReader___Observers_RemoveCustomer_3615296227));
			base.RegisterServerRpc(45U, new ServerRpcDelegate(this.RpcReader___Server_SetCash_431000436));
			base.RegisterServerRpc(46U, new ServerRpcDelegate(this.RpcReader___Server_CompletedDeal_2166136261));
			base.RegisterServerRpc(47U, new ServerRpcDelegate(this.RpcReader___Server_SubmitPayment_431000436));
			base.RegisterServerRpc(48U, new ServerRpcDelegate(this.RpcReader___Server_SetStoredInstance_2652194801));
			base.RegisterObserversRpc(49U, new ClientRpcDelegate(this.RpcReader___Observers_SetStoredInstance_Internal_2652194801));
			base.RegisterTargetRpc(50U, new ClientRpcDelegate(this.RpcReader___Target_SetStoredInstance_Internal_2652194801));
			base.RegisterServerRpc(51U, new ServerRpcDelegate(this.RpcReader___Server_SetItemSlotQuantity_1692629761));
			base.RegisterObserversRpc(52U, new ClientRpcDelegate(this.RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761));
			base.RegisterServerRpc(53U, new ServerRpcDelegate(this.RpcReader___Server_SetSlotLocked_3170825843));
			base.RegisterTargetRpc(54U, new ClientRpcDelegate(this.RpcReader___Target_SetSlotLocked_Internal_3170825843));
			base.RegisterObserversRpc(55U, new ClientRpcDelegate(this.RpcReader___Observers_SetSlotLocked_Internal_3170825843));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Economy.Dealer));
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000BAE1E File Offset: 0x000B901E
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Economy.DealerAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Economy.DealerAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___acceptedContractGUIDs.SetRegistered();
			this.syncVar___<Cash>k__BackingField.SetRegistered();
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x000BAE4D File Offset: 0x000B904D
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000BAE5C File Offset: 0x000B905C
		private void RpcWriter___Server_MarkAsRecommended_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(35U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x000BAEF6 File Offset: 0x000B90F6
		public void RpcLogic___MarkAsRecommended_2166136261()
		{
			this.SetRecommended();
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x000BAF00 File Offset: 0x000B9100
		private void RpcReader___Server_MarkAsRecommended_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___MarkAsRecommended_2166136261();
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x000BAF30 File Offset: 0x000B9130
		private void RpcWriter___Observers_SetRecommended_2166136261()
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(36U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x000BAFD9 File Offset: 0x000B91D9
		private void RpcLogic___SetRecommended_2166136261()
		{
			if (this.HasBeenRecommended)
			{
				return;
			}
			this.HasBeenRecommended = true;
			base.HasChanged = true;
			if (this.onRecommended != null)
			{
				this.onRecommended.Invoke();
			}
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000BB008 File Offset: 0x000B9208
		private void RpcReader___Observers_SetRecommended_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetRecommended_2166136261();
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000BB034 File Offset: 0x000B9234
		private void RpcWriter___Server_InitialRecruitment_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(37U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000BAB8E File Offset: 0x000B8D8E
		public void RpcLogic___InitialRecruitment_2166136261()
		{
			this.SetIsRecruited(null);
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000BB0D0 File Offset: 0x000B92D0
		private void RpcReader___Server_InitialRecruitment_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___InitialRecruitment_2166136261();
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000BB100 File Offset: 0x000B9300
		private void RpcWriter___Observers_SetIsRecruited_328543758(NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendObserversRpc(38U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000BB1AC File Offset: 0x000B93AC
		public virtual void RpcLogic___SetIsRecruited_328543758(NetworkConnection conn)
		{
			if (this.IsRecruited)
			{
				return;
			}
			this.IsRecruited = true;
			DialogueController.GreetingOverride greetingOverride = new DialogueController.GreetingOverride();
			greetingOverride.Greeting = "Hi boss, what do you need?";
			greetingOverride.PlayVO = true;
			greetingOverride.VOType = EVOLineType.Greeting;
			greetingOverride.ShouldShow = true;
			this.DialogueController.AddGreetingOverride(greetingOverride);
			DialogueController.DialogueChoice dialogueChoice = new DialogueController.DialogueChoice();
			dialogueChoice.ChoiceText = "I need to trade some items";
			dialogueChoice.Enabled = true;
			dialogueChoice.onChoosen.AddListener(new UnityAction(this.TradeItems));
			this.DialogueController.AddDialogueChoice(dialogueChoice, 5);
			this.collectCashChoice = new DialogueController.DialogueChoice();
			this.UpdateCollectCashChoice(0f, 0f, false);
			this.collectCashChoice.Enabled = true;
			this.collectCashChoice.isValidCheck = new DialogueController.DialogueChoice.IsChoiceValid(this.CanCollectCash);
			this.collectCashChoice.onChoosen.AddListener(new UnityAction(this.CollectCash));
			this.collectCashChoice.Conversation = this.CollectCashDialogue;
			this.DialogueController.AddDialogueChoice(this.collectCashChoice, 4);
			this.assignCustomersChoice = new DialogueController.DialogueChoice();
			this.assignCustomersChoice.ChoiceText = "How do I assign customers to you?";
			this.assignCustomersChoice.Enabled = true;
			this.assignCustomersChoice.Conversation = this.AssignCustomersDialogue;
			this.DialogueController.AddDialogueChoice(this.assignCustomersChoice, 3);
			if (this.dealerPoI != null)
			{
				this.dealerPoI.enabled = true;
			}
			if (!this.RelationData.Unlocked)
			{
				this.RelationData.Unlock(NPCRelationData.EUnlockType.DirectApproach, false);
			}
			if (this.recruitChoice != null)
			{
				this.recruitChoice.Enabled = false;
			}
			if (Dealer.onDealerRecruited != null)
			{
				Dealer.onDealerRecruited(this);
			}
			base.HasChanged = true;
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000BB364 File Offset: 0x000B9564
		private void RpcReader___Observers_SetIsRecruited_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetIsRecruited_328543758(null);
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000BB390 File Offset: 0x000B9590
		private void RpcWriter___Target_SetIsRecruited_328543758(NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendTargetRpc(39U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000BB438 File Offset: 0x000B9638
		private void RpcReader___Target_SetIsRecruited_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetIsRecruited_328543758(base.LocalConnection);
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000BB460 File Offset: 0x000B9660
		private void RpcWriter___Server_SendAddCustomer_3615296227(string npcID)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(npcID);
			base.SendServerRpc(40U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000BB507 File Offset: 0x000B9707
		public void RpcLogic___SendAddCustomer_3615296227(string npcID)
		{
			this.AddCustomer(null, npcID);
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000BB514 File Offset: 0x000B9714
		private void RpcReader___Server_SendAddCustomer_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendAddCustomer_3615296227(npcID);
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000BB554 File Offset: 0x000B9754
		private void RpcWriter___Observers_AddCustomer_2971853958(NetworkConnection conn, string npcID)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(npcID);
			base.SendObserversRpc(41U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000BB60C File Offset: 0x000B980C
		private void RpcLogic___AddCustomer_2971853958(NetworkConnection conn, string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogWarning("Failed to find NPC with ID: " + npcID, null);
				return;
			}
			Customer component = npc.GetComponent<Customer>();
			if (component == null)
			{
				Console.LogWarning("NPC " + npcID + " is not a customer", null);
				return;
			}
			this.AddCustomer(component);
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000BB66C File Offset: 0x000B986C
		private void RpcReader___Observers_AddCustomer_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___AddCustomer_2971853958(null, npcID);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000BB6A8 File Offset: 0x000B98A8
		private void RpcWriter___Target_AddCustomer_2971853958(NetworkConnection conn, string npcID)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(npcID);
			base.SendTargetRpc(42U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000BB760 File Offset: 0x000B9960
		private void RpcReader___Target_AddCustomer_2971853958(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___AddCustomer_2971853958(base.LocalConnection, npcID);
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000BB798 File Offset: 0x000B9998
		private void RpcWriter___Server_SendRemoveCustomer_3615296227(string npcID)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(npcID);
			base.SendServerRpc(43U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000BB83F File Offset: 0x000B9A3F
		public void RpcLogic___SendRemoveCustomer_3615296227(string npcID)
		{
			this.RemoveCustomer(npcID);
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000BB848 File Offset: 0x000B9A48
		private void RpcReader___Server_SendRemoveCustomer_3615296227(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SendRemoveCustomer_3615296227(npcID);
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000BB888 File Offset: 0x000B9A88
		private void RpcWriter___Observers_RemoveCustomer_3615296227(string npcID)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteString(npcID);
			base.SendObserversRpc(44U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000BB940 File Offset: 0x000B9B40
		private void RpcLogic___RemoveCustomer_3615296227(string npcID)
		{
			NPC npc = NPCManager.GetNPC(npcID);
			if (npc == null)
			{
				Console.LogWarning("Failed to find NPC with ID: " + npcID, null);
				return;
			}
			Customer component = npc.GetComponent<Customer>();
			if (component == null)
			{
				Console.LogWarning("NPC " + npcID + " is not a customer", null);
				return;
			}
			this.RemoveCustomer(component);
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000BB9A0 File Offset: 0x000B9BA0
		private void RpcReader___Observers_RemoveCustomer_3615296227(PooledReader PooledReader0, Channel channel)
		{
			string npcID = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___RemoveCustomer_3615296227(npcID);
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000BB9DC File Offset: 0x000B9BDC
		private void RpcWriter___Server_SetCash_431000436(float cash)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(cash, AutoPackType.Unpacked);
			base.SendServerRpc(45U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000BBA88 File Offset: 0x000B9C88
		public void RpcLogic___SetCash_431000436(float cash)
		{
			this.Cash = Mathf.Clamp(cash, 0f, float.MaxValue);
			base.HasChanged = true;
			this.UpdateCollectCashChoice(0f, 0f, false);
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000BBAB8 File Offset: 0x000B9CB8
		private void RpcReader___Server_SetCash_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float cash = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetCash_431000436(cash);
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000BBAF0 File Offset: 0x000B9CF0
		private void RpcWriter___Server_CompletedDeal_2166136261()
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			base.SendServerRpc(46U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000BBB8C File Offset: 0x000B9D8C
		public virtual void RpcLogic___CompletedDeal_2166136261()
		{
			this.RelationData.ChangeRelationship(0.05f, true);
			if (this.CompletedDealsVariable != string.Empty)
			{
				NetworkSingleton<VariableDatabase>.Instance.SetVariableValue(this.CompletedDealsVariable, (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>(this.CompletedDealsVariable) + 1f).ToString(), true);
			}
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000BBBEC File Offset: 0x000B9DEC
		private void RpcReader___Server_CompletedDeal_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___CompletedDeal_2166136261();
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x000BBC0C File Offset: 0x000B9E0C
		private void RpcWriter___Server_SubmitPayment_431000436(float payment)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteSingle(payment, AutoPackType.Unpacked);
			base.SendServerRpc(47U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000BBCB8 File Offset: 0x000B9EB8
		public void RpcLogic___SubmitPayment_431000436(float payment)
		{
			if (payment <= 0f)
			{
				return;
			}
			Console.Log("Dealer " + base.fullName + " received payment: " + payment.ToString(), null);
			float cash = this.Cash;
			this.ChangeCash(payment * (1f - this.Cut));
			if (InstanceFinder.IsServer && this.Cash >= 500f && cash < 500f)
			{
				base.MSGConversation.SendMessage(new Message("Hey boss, just letting you know I've got " + MoneyManager.FormatAmount(this.Cash, false, false) + " ready for you to collect.", Message.ESenderType.Other, true, -1), true, true);
			}
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000BBD58 File Offset: 0x000B9F58
		private void RpcReader___Server_SubmitPayment_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float payment = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SubmitPayment_431000436(payment);
		}

		// Token: 0x06002CEB RID: 11499 RVA: 0x000BBD90 File Offset: 0x000B9F90
		private void RpcWriter___Server_SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendServerRpc(48U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CEC RID: 11500 RVA: 0x000BBE56 File Offset: 0x000BA056
		public void RpcLogic___SetStoredInstance_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetStoredInstance_Internal(null, itemSlotIndex, instance);
				return;
			}
			this.SetStoredInstance_Internal(conn, itemSlotIndex, instance);
		}

		// Token: 0x06002CED RID: 11501 RVA: 0x000BBE80 File Offset: 0x000BA080
		private void RpcReader___Server_SetStoredInstance_2652194801(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_2652194801(conn2, itemSlotIndex, instance);
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x000BBEE8 File Offset: 0x000BA0E8
		private void RpcWriter___Observers_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendObserversRpc(49U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000BBFB0 File Offset: 0x000BA1B0
		private void RpcLogic___SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (instance != null)
			{
				this.ItemSlots[itemSlotIndex].SetStoredItem(instance, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].ClearStoredInstance(true);
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x000BBFDC File Offset: 0x000BA1DC
		private void RpcReader___Observers_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(null, itemSlotIndex, instance);
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x000BC030 File Offset: 0x000BA230
		private void RpcWriter___Target_SetStoredInstance_Internal_2652194801(NetworkConnection conn, int itemSlotIndex, ItemInstance instance)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteItemInstance(instance);
			base.SendTargetRpc(50U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000BC0F8 File Offset: 0x000BA2F8
		private void RpcReader___Target_SetStoredInstance_Internal_2652194801(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			ItemInstance instance = PooledReader0.ReadItemInstance();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetStoredInstance_Internal_2652194801(base.LocalConnection, itemSlotIndex, instance);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000BC150 File Offset: 0x000BA350
		private void RpcWriter___Server_SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendServerRpc(51U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x000BC20E File Offset: 0x000BA40E
		public void RpcLogic___SetItemSlotQuantity_1692629761(int itemSlotIndex, int quantity)
		{
			this.SetItemSlotQuantity_Internal(itemSlotIndex, quantity);
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000BC218 File Offset: 0x000BA418
		private void RpcReader___Server_SetItemSlotQuantity_1692629761(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000BC274 File Offset: 0x000BA474
		private void RpcWriter___Observers_SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteInt32(quantity, AutoPackType.Packed);
			base.SendObserversRpc(52U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x000BC341 File Offset: 0x000BA541
		private void RpcLogic___SetItemSlotQuantity_Internal_1692629761(int itemSlotIndex, int quantity)
		{
			this.ItemSlots[itemSlotIndex].SetQuantity(quantity, true);
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000BC358 File Offset: 0x000BA558
		private void RpcReader___Observers_SetItemSlotQuantity_Internal_1692629761(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int quantity = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetItemSlotQuantity_Internal_1692629761(itemSlotIndex, quantity);
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000BC3B0 File Offset: 0x000BA5B0
		private void RpcWriter___Server_SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (!base.IsClientInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because client is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendServerRpc(53U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000BC490 File Offset: 0x000BA690
		public void RpcLogic___SetSlotLocked_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (conn == null || conn.ClientId == -1)
			{
				this.SetSlotLocked_Internal(null, itemSlotIndex, locked, lockOwner, lockReason);
				return;
			}
			this.SetSlotLocked_Internal(conn, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000BC4C0 File Offset: 0x000BA6C0
		private void RpcReader___Server_SetSlotLocked_3170825843(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			NetworkConnection conn2 = PooledReader0.ReadNetworkConnection();
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_3170825843(conn2, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000BC548 File Offset: 0x000BA748
		private void RpcWriter___Target_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendTargetRpc(54U, writer, channel, DataOrderType.Default, conn, false, true);
			writer.Store();
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000BC629 File Offset: 0x000BA829
		private void RpcLogic___SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (locked)
			{
				this.ItemSlots[itemSlotIndex].ApplyLock(lockOwner, lockReason, true);
				return;
			}
			this.ItemSlots[itemSlotIndex].RemoveLock(true);
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x000BC658 File Offset: 0x000BA858
		private void RpcReader___Target_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(base.LocalConnection, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000BC6D4 File Offset: 0x000BA8D4
		private void RpcWriter___Observers_SetSlotLocked_Internal_3170825843(NetworkConnection conn, int itemSlotIndex, bool locked, NetworkObject lockOwner, string lockReason)
		{
			if (!base.IsServerInitialized)
			{
				NetworkManager networkManager = base.NetworkManager;
				if (networkManager == null)
				{
					networkManager = InstanceFinder.NetworkManager;
				}
				if (networkManager != null)
				{
					networkManager.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				else
				{
					Debug.LogWarning("Cannot complete action because server is not active. This may also occur if the object is not yet initialized, has deinitialized, or if it does not contain a NetworkObject component.");
				}
				return;
			}
			Channel channel = Channel.Reliable;
			PooledWriter writer = WriterPool.GetWriter();
			writer.WriteInt32(itemSlotIndex, AutoPackType.Packed);
			writer.WriteBoolean(locked);
			writer.WriteNetworkObject(lockOwner);
			writer.WriteString(lockReason);
			base.SendObserversRpc(55U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000BC7B8 File Offset: 0x000BA9B8
		private void RpcReader___Observers_SetSlotLocked_Internal_3170825843(PooledReader PooledReader0, Channel channel)
		{
			int itemSlotIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			bool locked = PooledReader0.ReadBoolean();
			NetworkObject lockOwner = PooledReader0.ReadNetworkObject();
			string lockReason = PooledReader0.ReadString();
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___SetSlotLocked_Internal_3170825843(null, itemSlotIndex, locked, lockOwner, lockReason);
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x000BC82C File Offset: 0x000BAA2C
		// (set) Token: 0x06002D02 RID: 11522 RVA: 0x000BC834 File Offset: 0x000BAA34
		public float SyncAccessor_<Cash>k__BackingField
		{
			get
			{
				return this.<Cash>k__BackingField;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.<Cash>k__BackingField = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___<Cash>k__BackingField.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x000BC870 File Offset: 0x000BAA70
		public virtual bool Dealer(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_acceptedContractGUIDs(this.syncVar___acceptedContractGUIDs.GetValue(true), true);
					return true;
				}
				List<string> value = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___System.Collections.Generic.List`1<System.String>FishNet.Serializing.Generateds(PooledReader0);
				this.sync___set_value_acceptedContractGUIDs(value, Boolean2);
				return true;
			}
			else
			{
				if (UInt321 != 1U)
				{
					return false;
				}
				if (PooledReader0 == null)
				{
					this.sync___set_value_<Cash>k__BackingField(this.syncVar___<Cash>k__BackingField.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_<Cash>k__BackingField(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002D04 RID: 11524 RVA: 0x000BC90B File Offset: 0x000BAB0B
		// (set) Token: 0x06002D05 RID: 11525 RVA: 0x000BC913 File Offset: 0x000BAB13
		public List<string> SyncAccessor_acceptedContractGUIDs
		{
			get
			{
				return this.acceptedContractGUIDs;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.acceptedContractGUIDs = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___acceptedContractGUIDs.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000BC950 File Offset: 0x000BAB50
		protected virtual void dll()
		{
			base.Awake();
			this.HomeEvent.Building = this.Home;
			this.OverflowSlots = new ItemSlot[10];
			for (int i = 0; i < 10; i++)
			{
				this.OverflowSlots[i] = new ItemSlot();
				this.OverflowSlots[i].SetSlotOwner(this);
			}
			if (this.RelationData.Unlocked)
			{
				this.SetIsRecruited(null);
			}
			else
			{
				NPCRelationData relationData = this.RelationData;
				relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(delegate(NPCRelationData.EUnlockType <p0>, bool <p1>)
				{
					this.SetIsRecruited(null);
				}));
			}
			if (!Dealer.AllDealers.Contains(this))
			{
				Dealer.AllDealers.Add(this);
			}
		}

		// Token: 0x04001FD4 RID: 8148
		public const int MAX_CUSTOMERS = 8;

		// Token: 0x04001FD5 RID: 8149
		public const int DEAL_ARRIVAL_DELAY = 30;

		// Token: 0x04001FD6 RID: 8150
		public const int MIN_TRAVEL_TIME = 15;

		// Token: 0x04001FD7 RID: 8151
		public const int MAX_TRAVEL_TIME = 360;

		// Token: 0x04001FD8 RID: 8152
		public const int OVERFLOW_SLOT_COUNT = 10;

		// Token: 0x04001FD9 RID: 8153
		public const float CASH_REMINDER_THRESHOLD = 500f;

		// Token: 0x04001FDA RID: 8154
		public const float RELATIONSHIP_CHANGE_PER_DEAL = 0.05f;

		// Token: 0x04001FDB RID: 8155
		public static Action<Dealer> onDealerRecruited;

		// Token: 0x04001FDC RID: 8156
		public static Color32 DealerLabelColor = new Color32(120, 200, byte.MaxValue, byte.MaxValue);

		// Token: 0x04001FDD RID: 8157
		public static List<Dealer> AllDealers = new List<Dealer>();

		// Token: 0x04001FE0 RID: 8160
		[Header("Debug")]
		public List<Customer> InitialCustomers = new List<Customer>();

		// Token: 0x04001FE1 RID: 8161
		public List<ProductDefinition> InitialItems = new List<ProductDefinition>();

		// Token: 0x04001FE2 RID: 8162
		[Header("Dealer References")]
		public NPCEnterableBuilding Home;

		// Token: 0x04001FE3 RID: 8163
		public NPCSignal_HandleDeal DealSignal;

		// Token: 0x04001FE4 RID: 8164
		public NPCEvent_StayInBuilding HomeEvent;

		// Token: 0x04001FE5 RID: 8165
		public DialogueController_Supplier DialogueController;

		// Token: 0x04001FE6 RID: 8166
		[Header("Dialogue stuff")]
		public DialogueContainer RecruitDialogue;

		// Token: 0x04001FE7 RID: 8167
		public DialogueContainer CollectCashDialogue;

		// Token: 0x04001FE8 RID: 8168
		public DialogueContainer AssignCustomersDialogue;

		// Token: 0x04001FE9 RID: 8169
		[Header("Dealer Settings")]
		public string HomeName = "Home";

		// Token: 0x04001FEA RID: 8170
		public float SigningFee = 500f;

		// Token: 0x04001FEB RID: 8171
		public float Cut = 0.2f;

		// Token: 0x04001FEC RID: 8172
		public bool SellInsufficientQualityItems;

		// Token: 0x04001FED RID: 8173
		public bool SellExcessQualityItems = true;

		// Token: 0x04001FEE RID: 8174
		[Header("Variables")]
		public string CompletedDealsVariable = string.Empty;

		// Token: 0x04001FF0 RID: 8176
		public List<Customer> AssignedCustomers = new List<Customer>();

		// Token: 0x04001FF1 RID: 8177
		public List<Contract> ActiveContracts = new List<Contract>();

		// Token: 0x04001FF3 RID: 8179
		public UnityEvent onRecommended = new UnityEvent();

		// Token: 0x04001FF4 RID: 8180
		protected ItemSlot[] OverflowSlots;

		// Token: 0x04001FF5 RID: 8181
		private Contract currentContract;

		// Token: 0x04001FF6 RID: 8182
		private DialogueController.DialogueChoice recruitChoice;

		// Token: 0x04001FF7 RID: 8183
		private DialogueController.DialogueChoice collectCashChoice;

		// Token: 0x04001FF8 RID: 8184
		private DialogueController.DialogueChoice assignCustomersChoice;

		// Token: 0x04001FFB RID: 8187
		[SyncVar]
		public List<string> acceptedContractGUIDs = new List<string>();

		// Token: 0x04001FFC RID: 8188
		private int itemCountOnTradeStart;

		// Token: 0x04001FFD RID: 8189
		public SyncVar<float> syncVar___<Cash>k__BackingField;

		// Token: 0x04001FFE RID: 8190
		public SyncVar<List<string>> syncVar___acceptedContractGUIDs;

		// Token: 0x04001FFF RID: 8191
		private bool dll_Excuted;

		// Token: 0x04002000 RID: 8192
		private bool dll_Excuted;
	}
}
