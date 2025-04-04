using System;
using System.Collections;
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
using ScheduleOne.Levelling;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.NPCs;
using ScheduleOne.NPCs.Relation;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.PlayerScripts;
using ScheduleOne.Quests;
using ScheduleOne.Storage;
using ScheduleOne.UI.Phone;
using ScheduleOne.UI.Phone.Delivery;
using ScheduleOne.UI.Phone.Messages;
using ScheduleOne.UI.Shop;
using ScheduleOne.Variables;
using ScheduleOne.VoiceOver;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Economy
{
	// Token: 0x02000668 RID: 1640
	public class Supplier : NPC
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000BD045 File Offset: 0x000BB245
		// (set) Token: 0x06002D2F RID: 11567 RVA: 0x000BD04D File Offset: 0x000BB24D
		public Supplier.ESupplierStatus Status { get; private set; }

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002D30 RID: 11568 RVA: 0x000BD056 File Offset: 0x000BB256
		// (set) Token: 0x06002D31 RID: 11569 RVA: 0x000BD05E File Offset: 0x000BB25E
		public bool DeliveriesEnabled { get; private set; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06002D32 RID: 11570 RVA: 0x000BD067 File Offset: 0x000BB267
		public float Debt
		{
			get
			{
				return this.SyncAccessor_debt;
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x000BD06F File Offset: 0x000BB26F
		// (set) Token: 0x06002D34 RID: 11572 RVA: 0x000BD077 File Offset: 0x000BB277
		public int minsUntilDeaddropReady { get; private set; } = -1;

		// Token: 0x06002D35 RID: 11573 RVA: 0x000BD080 File Offset: 0x000BB280
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			this.Awake_UserLogic_ScheduleOne.Economy.Supplier_Assembly-CSharp.dll();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000BD094 File Offset: 0x000BB294
		protected override void Start()
		{
			base.Start();
			NPCRelationData relationData = this.RelationData;
			relationData.onUnlocked = (Action<NPCRelationData.EUnlockType, bool>)Delegate.Combine(relationData.onUnlocked, new Action<NPCRelationData.EUnlockType, bool>(this.SupplierUnlocked));
			NPCRelationData relationData2 = this.RelationData;
			relationData2.onRelationshipChange = (Action<float>)Delegate.Combine(relationData2.onRelationshipChange, new Action<float>(this.RelationshipChange));
			string orderCompleteDialogue = this.dialogueHandler.Database.GetLine(EDialogueModule.Generic, "meeting_order_complete");
			this.Shop.onOrderCompleted.AddListener(delegate()
			{
				this.dialogueHandler.ShowWorldspaceDialogue(orderCompleteDialogue, 3f);
			});
			this.dialogueController = this.dialogueHandler.GetComponent<DialogueController>();
			this.meetingGreeting = new DialogueController.GreetingOverride();
			this.meetingGreeting.Greeting = this.dialogueHandler.Database.GetLine(EDialogueModule.Generic, "supplier_meeting_greeting");
			this.meetingGreeting.PlayVO = true;
			this.meetingGreeting.VOType = EVOLineType.Question;
			this.dialogueController.AddGreetingOverride(this.meetingGreeting);
			this.meetingChoice = new DialogueController.DialogueChoice();
			this.meetingChoice.ChoiceText = "Yes";
			this.meetingChoice.onChoosen.AddListener(delegate()
			{
				this.Shop.SetIsOpen(true);
			});
			this.meetingChoice.Enabled = false;
			this.dialogueController.AddDialogueChoice(this.meetingChoice, 0);
			TimeManager instance = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance.onTimeSkip = (Action<int>)Delegate.Combine(instance.onTimeSkip, new Action<int>(this.OnTimeSkip));
			foreach (PhoneShopInterface.Listing listing in this.OnlineShopItems)
			{
				if ((listing.Item as StorableItemDefinition).RequiresLevelToPurchase)
				{
					NetworkSingleton<LevelManager>.Instance.AddUnlockable(new Unlockable((listing.Item as StorableItemDefinition).RequiredRank, listing.Item.Name, listing.Item.Icon));
				}
			}
			TimeManager instance2 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance2.onHourPass = (Action)Delegate.Remove(instance2.onHourPass, new Action(this.HourPass));
			TimeManager instance3 = NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance;
			instance3.onHourPass = (Action)Delegate.Combine(instance3.onHourPass, new Action(this.HourPass));
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000BD2CB File Offset: 0x000BB4CB
		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);
			if (this.Status == Supplier.ESupplierStatus.Meeting)
			{
				this.MeetAtLocation(connection, SupplierLocation.AllLocations.IndexOf(this.currentLocation), 360);
			}
			if (this.DeliveriesEnabled)
			{
				this.EnableDeliveries(connection);
			}
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000BD308 File Offset: 0x000BB508
		[ServerRpc(RequireOwnership = false)]
		public void SendUnlocked()
		{
			this.RpcWriter___Server_SendUnlocked_2166136261();
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000BD310 File Offset: 0x000BB510
		[ObserversRpc]
		private void SetUnlocked()
		{
			this.RpcWriter___Observers_SetUnlocked_2166136261();
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000BD318 File Offset: 0x000BB518
		protected override void MinPass()
		{
			base.MinPass();
			this.minsSinceDeaddropOrder++;
			if (this.Status == Supplier.ESupplierStatus.Meeting)
			{
				this.minsSinceMeetingStart++;
				this.minsSinceLastMeetingEnd = 0;
				if (this.minsSinceMeetingStart > 360)
				{
					this.EndMeeting();
				}
			}
			else
			{
				this.minsSinceLastMeetingEnd++;
			}
			if (InstanceFinder.IsServer)
			{
				if (this.SyncAccessor_deadDropPreparing)
				{
					this.minsUntilDeaddropReady--;
					if (this.minsUntilDeaddropReady <= 0)
					{
						this.CompleteDeaddrop();
					}
				}
				if (this.SyncAccessor_debt > 0f && !this.Stash.Storage.IsOpened && this.Stash.CashAmount > 1f && this.minsSinceDeaddropOrder > 3)
				{
					this.TryRecoverDebt();
				}
			}
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000BD3E8 File Offset: 0x000BB5E8
		protected void HourPass()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (!this.repaymentReminderSent && this.SyncAccessor_debt > this.GetDeadDropLimit() * 0.5f && !this.SyncAccessor_deadDropPreparing)
			{
				float num = 0.020833334f;
				if (UnityEngine.Random.Range(0f, 1f) < num)
				{
					this.SendDebtReminder();
				}
			}
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000BD43F File Offset: 0x000BB63F
		private void OnTimeSkip(int minsSlept)
		{
			if (this.SyncAccessor_deadDropPreparing)
			{
				this.minsUntilDeaddropReady -= minsSlept;
			}
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000BD458 File Offset: 0x000BB658
		[ObserversRpc(RunLocally = true)]
		public void MeetAtLocation(NetworkConnection conn, int locationIndex, int expireIn)
		{
			this.RpcWriter___Observers_MeetAtLocation_3470796954(conn, locationIndex, expireIn);
			this.RpcLogic___MeetAtLocation_3470796954(conn, locationIndex, expireIn);
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000BD48C File Offset: 0x000BB68C
		public void EndMeeting()
		{
			Console.Log("Meeting ended", null);
			this.Status = Supplier.ESupplierStatus.Idle;
			this.minsSinceMeetingStart = -1;
			this.meetingGreeting.ShouldShow = false;
			this.meetingChoice.Enabled = false;
			this.currentLocation.SetActiveSupplier(null);
			this.SetVisible(false);
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000BD4DD File Offset: 0x000BB6DD
		protected virtual void SupplierUnlocked(NPCRelationData.EUnlockType type, bool notify)
		{
			if (notify)
			{
				this.SetUnlockMessage();
			}
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x000BD4E8 File Offset: 0x000BB6E8
		protected virtual void RelationshipChange(float change)
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			if (Singleton<LoadManager>.Instance.IsLoading)
			{
				if (this.RelationData.RelationDelta >= 5f && !this.DeliveriesEnabled)
				{
					this.EnableDeliveries(null);
				}
				return;
			}
			float num = this.RelationData.RelationDelta - change;
			float relationDelta = this.RelationData.RelationDelta;
			if (num < 4f && relationDelta >= 4f)
			{
				Console.Log("Supplier relationship high enough for meetings", null);
				DialogueChain chain = this.dialogueHandler.Database.GetChain(EDialogueModule.Generic, "supplier_meetings_unlocked");
				if (chain == null)
				{
					return;
				}
				base.MSGConversation.SendMessageChain(chain.GetMessageChain(), 3f, true, true);
			}
			if (relationDelta >= 5f && !this.DeliveriesEnabled)
			{
				Console.Log("Supplier relationship high enough for deliveries", null);
				this.EnableDeliveries(null);
				DialogueChain chain2 = this.dialogueHandler.Database.GetChain(EDialogueModule.Generic, "supplier_deliveries_unlocked");
				if (chain2 != null)
				{
					base.MSGConversation.SendMessageChain(chain2.GetMessageChain(), 3f, true, true);
				}
			}
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x000BD5E5 File Offset: 0x000BB7E5
		[ObserversRpc(RunLocally = true)]
		[TargetRpc]
		private void EnableDeliveries(NetworkConnection conn)
		{
			if (conn == null)
			{
				this.RpcWriter___Observers_EnableDeliveries_328543758(conn);
				this.RpcLogic___EnableDeliveries_328543758(conn);
			}
			else
			{
				this.RpcWriter___Target_EnableDeliveries_328543758(conn);
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000BD610 File Offset: 0x000BB810
		public void SetUnlockMessage()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			DialogueChain chain = this.dialogueHandler.Database.GetChain(EDialogueModule.Generic, "supplier_unlocked");
			if (chain == null)
			{
				return;
			}
			base.MSGConversation.SendMessageChain(chain.GetMessageChain(), 0f, true, true);
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000BD658 File Offset: 0x000BB858
		protected override void CreateMessageConversation()
		{
			base.CreateMessageConversation();
			SendableMessage sendableMessage = base.MSGConversation.CreateSendableMessage("I need to order a dead drop");
			sendableMessage.IsValidCheck = new SendableMessage.ValidityCheck(this.IsDeadDropValid);
			sendableMessage.disableDefaultSendBehaviour = true;
			sendableMessage.onSelected = (Action)Delegate.Combine(sendableMessage.onSelected, new Action(this.DeaddropRequested));
			SendableMessage sendableMessage2 = base.MSGConversation.CreateSendableMessage("We need to meet up");
			sendableMessage2.IsValidCheck = new SendableMessage.ValidityCheck(this.IsMeetupValid);
			sendableMessage2.onSent = (Action)Delegate.Combine(sendableMessage2.onSent, new Action(this.MeetupRequested));
			SendableMessage sendableMessage3 = base.MSGConversation.CreateSendableMessage("I want to pay off my debt");
			sendableMessage3.onSent = (Action)Delegate.Combine(sendableMessage3.onSent, new Action(this.PayDebtRequested));
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000BD72C File Offset: 0x000BB92C
		protected virtual void DeaddropRequested()
		{
			float orderLimit = Mathf.Max(this.GetDeadDropLimit() - this.SyncAccessor_debt, 0f);
			PlayerSingleton<MessagesApp>.Instance.PhoneShopInterface.Open("Request Dead Drop", "Select items to order from " + this.FirstName, base.MSGConversation, this.OnlineShopItems.ToList<PhoneShopInterface.Listing>(), orderLimit, this.SyncAccessor_debt, new Action<List<PhoneShopInterface.CartEntry>, float>(this.DeaddropConfirmed));
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000BD79C File Offset: 0x000BB99C
		protected virtual void DeaddropConfirmed(List<PhoneShopInterface.CartEntry> cart, float totalPrice)
		{
			if (this.SyncAccessor_deadDropPreparing)
			{
				Console.LogWarning("Already preparing a dead drop", null);
				return;
			}
			int num = cart.Sum((PhoneShopInterface.CartEntry x) => x.Quantity);
			StringIntPair[] array = new StringIntPair[cart.Count];
			for (int i = 0; i < cart.Count; i++)
			{
				array[i] = new StringIntPair(cart[i].Listing.Item.ID, cart[i].Quantity);
			}
			string text = "I need a dead drop:\n";
			for (int j = 0; j < cart.Count; j++)
			{
				if (cart[j].Quantity > 0)
				{
					text = text + cart[j].Quantity.ToString() + "x " + cart[j].Listing.Item.Name;
					if (j < cart.Count - 1)
					{
						text += "\n";
					}
				}
			}
			base.MSGConversation.SendMessage(new Message(text, Message.ESenderType.Player, false, -1), true, true);
			int num2 = Mathf.Clamp(num * 30, 30, 360);
			string text2 = this.dialogueHandler.Database.GetLine(EDialogueModule.Supplier, "deaddrop_requested");
			if (num2 < 60)
			{
				text2 = text2.Replace("<TIME>", num2.ToString() + ((num2 == 1) ? " min" : " mins"));
			}
			else
			{
				float num3 = (float)Mathf.FloorToInt((float)num2 / 60f);
				float num4 = (float)num2 - num3 * 60f;
				string text3 = num3.ToString() + ((num3 == 1f) ? " hour" : " hours");
				if (num4 > 0f)
				{
					text3 = text3 + " " + num4.ToString() + " min";
				}
				text2 = text2.Replace("<TIME>", text3);
			}
			base.MSGConversation.SendMessageChain(new MessageChain
			{
				Messages = new List<string>
				{
					text2
				},
				id = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
			}, 0.5f, false, true);
			NetworkSingleton<VariableDatabase>.Instance.SetVariableValue("Deaddrops_Ordered", (NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Deaddrops_Ordered") + 1f).ToString(), true);
			this.SetDeaddrop(array, num2);
			this.minsSinceDeaddropOrder = 0;
			this.ChangeDebt(totalPrice);
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000BDA0F File Offset: 0x000BBC0F
		[ServerRpc(RequireOwnership = false)]
		private void SetDeaddrop(StringIntPair[] items, int minsUntilReady)
		{
			this.RpcWriter___Server_SetDeaddrop_3971994486(items, minsUntilReady);
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000BDA1F File Offset: 0x000BBC1F
		[ServerRpc(RequireOwnership = false, RunLocally = true)]
		private void ChangeDebt(float amount)
		{
			this.RpcWriter___Server_ChangeDebt_431000436(amount);
			this.RpcLogic___ChangeDebt_431000436(amount);
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000BDA38 File Offset: 0x000BBC38
		private void TryRecoverDebt()
		{
			float num = Mathf.Min(this.SyncAccessor_debt, this.Stash.CashAmount);
			if (num > 0f)
			{
				Debug.Log("Recovering debt: " + num.ToString());
				float num2 = this.SyncAccessor_debt;
				this.Stash.RemoveCash(num);
				this.ChangeDebt(-num);
				this.RelationData.ChangeRelationship(num / this.MaxOrderLimit * 0.5f, true);
				float num3 = num2 - num;
				string text = "I've received " + MoneyManager.FormatAmount(num, false, false) + " cash from you.";
				if (num3 <= 0f)
				{
					text += " Your debt is now paid off.";
				}
				else
				{
					text = text + " Your debt is now " + MoneyManager.FormatAmount(num3, false, false);
				}
				this.repaymentReminderSent = false;
				base.MSGConversation.SendMessageChain(new MessageChain
				{
					Messages = new List<string>
					{
						text
					},
					id = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
				}, 0f, true, true);
			}
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x000BDB3C File Offset: 0x000BBD3C
		private void CompleteDeaddrop()
		{
			Console.Log("Dead drop ready", null);
			DeadDrop randomEmptyDrop = DeadDrop.GetRandomEmptyDrop(Player.Local.transform.position);
			if (randomEmptyDrop == null)
			{
				Console.LogError("No empty dead drop locations", null);
				return;
			}
			foreach (StringIntPair stringIntPair in this.deaddropItems)
			{
				ItemDefinition item = Registry.GetItem(stringIntPair.String);
				if (item == null)
				{
					Console.LogError("Item not found: " + stringIntPair.String, null);
				}
				else
				{
					int num;
					for (int j = stringIntPair.Int; j > 0; j -= num)
					{
						num = Mathf.Min(j, item.StackLimit);
						ItemInstance defaultInstance = item.GetDefaultInstance(num);
						randomEmptyDrop.Storage.InsertItem(defaultInstance, true);
					}
				}
			}
			string text = this.dialogueHandler.Database.GetLine(EDialogueModule.Supplier, "deaddrop_ready");
			text = text.Replace("<LOCATION>", randomEmptyDrop.DeadDropDescription);
			base.MSGConversation.SendMessageChain(new MessageChain
			{
				Messages = new List<string>
				{
					text
				},
				id = UnityEngine.Random.Range(int.MinValue, int.MaxValue)
			}, 0f, true, true);
			this.sync___set_value_deadDropPreparing(false, true);
			this.minsUntilDeaddropReady = -1;
			this.deaddropItems = null;
			if (this.onDeaddropReady != null)
			{
				this.onDeaddropReady.Invoke();
			}
			string guidString = GUIDManager.GenerateUniqueGUID().ToString();
			NetworkSingleton<QuestManager>.Instance.CreateDeaddropCollectionQuest(null, randomEmptyDrop.GUID.ToString(), guidString);
			this.SetDeaddrop(null, -1);
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x000BDCE0 File Offset: 0x000BBEE0
		private void SendDebtReminder()
		{
			this.repaymentReminderSent = true;
			DialogueChain chain = this.dialogueHandler.Database.GetChain(EDialogueModule.Supplier, "supplier_request_repayment");
			chain.Lines[0] = chain.Lines[0].Replace("<DEBT>", "<color=#46CB4F>" + MoneyManager.FormatAmount(this.SyncAccessor_debt, false, false) + "</color>");
			base.MSGConversation.SendMessageChain(chain.GetMessageChain(), 0f, true, true);
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x000BDD5C File Offset: 0x000BBF5C
		protected virtual void MeetupRequested()
		{
			if (InstanceFinder.IsServer)
			{
				int locationIndex;
				SupplierLocation appropriateLocation = this.GetAppropriateLocation(out locationIndex);
				string text = this.dialogueHandler.Database.GetLine(EDialogueModule.Generic, "supplier_meet_confirm");
				text = text.Replace("<LOCATION>", appropriateLocation.LocationDescription);
				MessageChain messageChain = new MessageChain();
				messageChain.Messages.Add(text);
				messageChain.id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
				base.MSGConversation.SendMessageChain(messageChain, 0.5f, true, true);
				this.MeetAtLocation(null, locationIndex, 360);
			}
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000BDDEC File Offset: 0x000BBFEC
		protected virtual void PayDebtRequested()
		{
			if (!InstanceFinder.IsServer)
			{
				return;
			}
			MessageChain messageChain = new MessageChain();
			messageChain.Messages.Add("You can pay off your debt by placing cash in my stash. It's " + this.Stash.locationDescription + ".");
			messageChain.id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			base.MSGConversation.SendMessageChain(messageChain, 0.5f, true, true);
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000BDE54 File Offset: 0x000BC054
		protected SupplierLocation GetAppropriateLocation(out int locationIndex)
		{
			locationIndex = -1;
			List<SupplierLocation> list = new List<SupplierLocation>();
			list.AddRange(SupplierLocation.AllLocations);
			foreach (SupplierLocation supplierLocation in SupplierLocation.AllLocations)
			{
				if (supplierLocation.IsOccupied)
				{
					list.Remove(supplierLocation);
				}
			}
			foreach (SupplierLocation supplierLocation2 in SupplierLocation.AllLocations)
			{
				foreach (Player player in Player.PlayerList)
				{
					if (Vector3.Distance(supplierLocation2.transform.position, player.Avatar.CenterPoint) < 30f)
					{
						list.Remove(supplierLocation2);
					}
				}
			}
			if (list.Count == 0)
			{
				Console.LogError("No available locations for supplier", null);
				return null;
			}
			SupplierLocation supplierLocation3 = list[UnityEngine.Random.Range(0, list.Count)];
			locationIndex = SupplierLocation.AllLocations.IndexOf(supplierLocation3);
			return supplierLocation3;
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000BDFA0 File Offset: 0x000BC1A0
		private bool IsDeadDropValid(SendableMessage message, out string invalidReason)
		{
			invalidReason = string.Empty;
			if (this.SyncAccessor_deadDropPreparing)
			{
				invalidReason = "Already waiting for a dead drop";
				return false;
			}
			return true;
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000BDFBC File Offset: 0x000BC1BC
		private bool IsMeetupValid(SendableMessage message, out string invalidReason)
		{
			if (this.RelationData.RelationDelta < 4f)
			{
				invalidReason = "Insufficient trust";
				return false;
			}
			if (this.Status != Supplier.ESupplierStatus.Idle)
			{
				invalidReason = "Busy";
				return false;
			}
			if (this.minsSinceLastMeetingEnd < 720)
			{
				invalidReason = "Too soon since last meeting";
				return false;
			}
			invalidReason = "";
			return true;
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000BE013 File Offset: 0x000BC213
		public virtual float GetDeadDropLimit()
		{
			return Mathf.Lerp(this.MinOrderLimit, this.MaxOrderLimit, this.RelationData.RelationDelta / 5f);
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000BE037 File Offset: 0x000BC237
		public override string GetSaveString()
		{
			return new SupplierData(this.ID, this.minsSinceMeetingStart, this.minsSinceLastMeetingEnd, this.SyncAccessor_debt, this.minsUntilDeaddropReady, this.deaddropItems, this.repaymentReminderSent).GetJson(true);
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000BE070 File Offset: 0x000BC270
		public override void Load(NPCData data, string containerPath)
		{
			base.Load(data, containerPath);
			string json;
			if (((ISaveable)this).TryLoadFile(containerPath, "NPC", out json))
			{
				SupplierData supplierData = null;
				try
				{
					supplierData = JsonUtility.FromJson<SupplierData>(json);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Failed to deserialize character data: " + ex.Message, null);
					return;
				}
				this.minsSinceMeetingStart = supplierData.timeSinceMeetingStart;
				this.minsSinceLastMeetingEnd = supplierData.timeSinceLastMeetingEnd;
				this.sync___set_value_debt(supplierData.debt, true);
				this.minsUntilDeaddropReady = supplierData.minsUntilDeadDropReady;
				if (this.minsUntilDeaddropReady > 0)
				{
					this.sync___set_value_deadDropPreparing(true, true);
				}
				if (supplierData.deaddropItems != null)
				{
					this.deaddropItems = supplierData.deaddropItems.ToArray<StringIntPair>();
				}
				this.repaymentReminderSent = supplierData.debtReminderSent;
			}
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000BE1AC File Offset: 0x000BC3AC
		[CompilerGenerated]
		private IEnumerator <EnableDeliveries>g__Wait|54_0()
		{
			yield return new WaitUntil(() => PlayerSingleton<DeliveryApp>.InstanceExists);
			PlayerSingleton<DeliveryApp>.Instance.GetShop(this.Shop).SetIsAvailable();
			yield break;
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000BE1BC File Offset: 0x000BC3BC
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.Economy.SupplierAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.Economy.SupplierAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
			this.syncVar___deadDropPreparing = new SyncVar<bool>(this, 2U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.deadDropPreparing);
			this.syncVar___debt = new SyncVar<float>(this, 1U, WritePermission.ServerOnly, ReadPermission.Observers, -1f, Channel.Reliable, this.debt);
			base.RegisterServerRpc(35U, new ServerRpcDelegate(this.RpcReader___Server_SendUnlocked_2166136261));
			base.RegisterObserversRpc(36U, new ClientRpcDelegate(this.RpcReader___Observers_SetUnlocked_2166136261));
			base.RegisterObserversRpc(37U, new ClientRpcDelegate(this.RpcReader___Observers_MeetAtLocation_3470796954));
			base.RegisterObserversRpc(38U, new ClientRpcDelegate(this.RpcReader___Observers_EnableDeliveries_328543758));
			base.RegisterTargetRpc(39U, new ClientRpcDelegate(this.RpcReader___Target_EnableDeliveries_328543758));
			base.RegisterServerRpc(40U, new ServerRpcDelegate(this.RpcReader___Server_SetDeaddrop_3971994486));
			base.RegisterServerRpc(41U, new ServerRpcDelegate(this.RpcReader___Server_ChangeDebt_431000436));
			base.RegisterSyncVarRead(new SyncVarReadDelegate(this.ReadSyncVar___ScheduleOne.Economy.Supplier));
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000BE2E9 File Offset: 0x000BC4E9
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.Economy.SupplierAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.Economy.SupplierAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
			this.syncVar___deadDropPreparing.SetRegistered();
			this.syncVar___debt.SetRegistered();
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000BE318 File Offset: 0x000BC518
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000BE328 File Offset: 0x000BC528
		private void RpcWriter___Server_SendUnlocked_2166136261()
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

		// Token: 0x06002D5A RID: 11610 RVA: 0x000BE3C2 File Offset: 0x000BC5C2
		public void RpcLogic___SendUnlocked_2166136261()
		{
			this.SetUnlocked();
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000BE3CC File Offset: 0x000BC5CC
		private void RpcReader___Server_SendUnlocked_2166136261(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SendUnlocked_2166136261();
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000BE3EC File Offset: 0x000BC5EC
		private void RpcWriter___Observers_SetUnlocked_2166136261()
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

		// Token: 0x06002D5D RID: 11613 RVA: 0x000BE495 File Offset: 0x000BC695
		private void RpcLogic___SetUnlocked_2166136261()
		{
			this.RelationData.Unlock(NPCRelationData.EUnlockType.Recommendation, true);
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000BE4A4 File Offset: 0x000BC6A4
		private void RpcReader___Observers_SetUnlocked_2166136261(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___SetUnlocked_2166136261();
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000BE4C4 File Offset: 0x000BC6C4
		private void RpcWriter___Observers_MeetAtLocation_3470796954(NetworkConnection conn, int locationIndex, int expireIn)
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
			writer.WriteNetworkConnection(conn);
			writer.WriteInt32(locationIndex, AutoPackType.Packed);
			writer.WriteInt32(expireIn, AutoPackType.Packed);
			base.SendObserversRpc(37U, writer, channel, DataOrderType.Default, false, false, false);
			writer.Store();
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000BE5A0 File Offset: 0x000BC7A0
		public void RpcLogic___MeetAtLocation_3470796954(NetworkConnection conn, int locationIndex, int expireIn)
		{
			SupplierLocation supplierLocation = SupplierLocation.AllLocations[locationIndex];
			Console.Log(string.Concat(new string[]
			{
				base.fullName,
				" meeting at ",
				supplierLocation.name,
				" in ",
				expireIn.ToString(),
				" minutes"
			}), null);
			this.Status = Supplier.ESupplierStatus.Meeting;
			this.currentLocation = supplierLocation;
			this.minsSinceMeetingStart = 0;
			supplierLocation.SetActiveSupplier(this);
			ShopInterface shop = this.Shop;
			StorageEntity[] deliveryBays = supplierLocation.DeliveryBays;
			shop.DeliveryBays = deliveryBays;
			this.meetingGreeting.ShouldShow = true;
			this.meetingChoice.Enabled = true;
			this.movement.Warp(supplierLocation.SupplierStandPoint.position);
			this.movement.FaceDirection(supplierLocation.SupplierStandPoint.forward, 0.5f);
			this.SetVisible(true);
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x000BE680 File Offset: 0x000BC880
		private void RpcReader___Observers_MeetAtLocation_3470796954(PooledReader PooledReader0, Channel channel)
		{
			NetworkConnection conn = PooledReader0.ReadNetworkConnection();
			int locationIndex = PooledReader0.ReadInt32(AutoPackType.Packed);
			int expireIn = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___MeetAtLocation_3470796954(conn, locationIndex, expireIn);
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x000BE6E8 File Offset: 0x000BC8E8
		private void RpcWriter___Observers_EnableDeliveries_328543758(NetworkConnection conn)
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

		// Token: 0x06002D63 RID: 11619 RVA: 0x000BE791 File Offset: 0x000BC991
		private void RpcLogic___EnableDeliveries_328543758(NetworkConnection conn)
		{
			this.DeliveriesEnabled = true;
			Singleton<CoroutineService>.Instance.StartCoroutine(this.<EnableDeliveries>g__Wait|54_0());
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x000BE7AC File Offset: 0x000BC9AC
		private void RpcReader___Observers_EnableDeliveries_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			if (base.IsHost)
			{
				return;
			}
			this.RpcLogic___EnableDeliveries_328543758(null);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000BE7D8 File Offset: 0x000BC9D8
		private void RpcWriter___Target_EnableDeliveries_328543758(NetworkConnection conn)
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

		// Token: 0x06002D66 RID: 11622 RVA: 0x000BE880 File Offset: 0x000BCA80
		private void RpcReader___Target_EnableDeliveries_328543758(PooledReader PooledReader0, Channel channel)
		{
			if (!base.IsClientInitialized)
			{
				return;
			}
			this.RpcLogic___EnableDeliveries_328543758(base.LocalConnection);
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000BE8A8 File Offset: 0x000BCAA8
		private void RpcWriter___Server_SetDeaddrop_3971994486(StringIntPair[] items, int minsUntilReady)
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
			writer.Write___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generated(items);
			writer.WriteInt32(minsUntilReady, AutoPackType.Packed);
			base.SendServerRpc(40U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x000BE961 File Offset: 0x000BCB61
		private void RpcLogic___SetDeaddrop_3971994486(StringIntPair[] items, int minsUntilReady)
		{
			if (items != null)
			{
				this.minsSinceDeaddropOrder = 0;
				this.sync___set_value_deadDropPreparing(true, true);
			}
			else
			{
				this.sync___set_value_deadDropPreparing(false, true);
			}
			this.minsUntilDeaddropReady = minsUntilReady;
			this.deaddropItems = items;
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x000BE990 File Offset: 0x000BCB90
		private void RpcReader___Server_SetDeaddrop_3971994486(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			StringIntPair[] items = FishNet.Serializing.Generated.GeneratedReaders___Internal.Read___ScheduleOne.DevUtilities.StringIntPair[]FishNet.Serializing.Generateds(PooledReader0);
			int minsUntilReady = PooledReader0.ReadInt32(AutoPackType.Packed);
			if (!base.IsServerInitialized)
			{
				return;
			}
			this.RpcLogic___SetDeaddrop_3971994486(items, minsUntilReady);
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000BE9D8 File Offset: 0x000BCBD8
		private void RpcWriter___Server_ChangeDebt_431000436(float amount)
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
			writer.WriteSingle(amount, AutoPackType.Unpacked);
			base.SendServerRpc(41U, writer, channel, DataOrderType.Default);
			writer.Store();
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000BEA84 File Offset: 0x000BCC84
		private void RpcLogic___ChangeDebt_431000436(float amount)
		{
			this.sync___set_value_debt(Mathf.Clamp(this.SyncAccessor_debt + amount, 0f, this.GetDeadDropLimit()), true);
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000BEAA8 File Offset: 0x000BCCA8
		private void RpcReader___Server_ChangeDebt_431000436(PooledReader PooledReader0, Channel channel, NetworkConnection conn)
		{
			float amount = PooledReader0.ReadSingle(AutoPackType.Unpacked);
			if (!base.IsServerInitialized)
			{
				return;
			}
			if (conn.IsLocalClient)
			{
				return;
			}
			this.RpcLogic___ChangeDebt_431000436(amount);
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002D6D RID: 11629 RVA: 0x000BEAEB File Offset: 0x000BCCEB
		// (set) Token: 0x06002D6E RID: 11630 RVA: 0x000BEAF3 File Offset: 0x000BCCF3
		public float SyncAccessor_debt
		{
			get
			{
				return this.debt;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.debt = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___debt.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000BEB30 File Offset: 0x000BCD30
		public virtual bool Supplier(PooledReader PooledReader0, uint UInt321, bool Boolean2)
		{
			if (UInt321 == 2U)
			{
				if (PooledReader0 == null)
				{
					this.sync___set_value_deadDropPreparing(this.syncVar___deadDropPreparing.GetValue(true), true);
					return true;
				}
				bool value = PooledReader0.ReadBoolean();
				this.sync___set_value_deadDropPreparing(value, Boolean2);
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
					this.sync___set_value_debt(this.syncVar___debt.GetValue(true), true);
					return true;
				}
				float value2 = PooledReader0.ReadSingle(AutoPackType.Unpacked);
				this.sync___set_value_debt(value2, Boolean2);
				return true;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002D70 RID: 11632 RVA: 0x000BEBCB File Offset: 0x000BCDCB
		// (set) Token: 0x06002D71 RID: 11633 RVA: 0x000BEBD3 File Offset: 0x000BCDD3
		public bool SyncAccessor_deadDropPreparing
		{
			get
			{
				return this.deadDropPreparing;
			}
			set
			{
				if (value || !base.IsServerInitialized)
				{
					this.deadDropPreparing = value;
				}
				if (Application.isPlaying)
				{
					this.syncVar___deadDropPreparing.SetValue(value, value);
				}
			}
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000BEC0F File Offset: 0x000BCE0F
		protected virtual void dll()
		{
			base.Awake();
		}

		// Token: 0x04002039 RID: 8249
		public const float MEETUP_RELATIONSHIP_REQUIREMENT = 4f;

		// Token: 0x0400203A RID: 8250
		public const int MEETUP_DURATION_MINS = 360;

		// Token: 0x0400203B RID: 8251
		public const int MEETING_COOLDOWN_MINS = 720;

		// Token: 0x0400203C RID: 8252
		public const int DEADDROP_WAIT_PER_ITEM = 30;

		// Token: 0x0400203D RID: 8253
		public const int DEADDROP_MAX_WAIT = 360;

		// Token: 0x0400203E RID: 8254
		public const int DEADDROP_ITEM_LIMIT = 10;

		// Token: 0x0400203F RID: 8255
		public const float DELIVERY_RELATIONSHIP_REQUIREMENT = 5f;

		// Token: 0x04002040 RID: 8256
		public static Color32 SupplierLabelColor = new Color32(byte.MaxValue, 150, 145, byte.MaxValue);

		// Token: 0x04002043 RID: 8259
		[Header("Supplier Settings")]
		public float MinOrderLimit = 100f;

		// Token: 0x04002044 RID: 8260
		public float MaxOrderLimit = 500f;

		// Token: 0x04002045 RID: 8261
		public PhoneShopInterface.Listing[] OnlineShopItems;

		// Token: 0x04002046 RID: 8262
		[TextArea(3, 10)]
		public string SupplierRecommendMessage = "My friend <NAME> can hook you up with <PRODUCT>. I've passed your number on to them.";

		// Token: 0x04002047 RID: 8263
		[TextArea(3, 10)]
		public string SupplierUnlockHint = "You can now order <PRODUCT> from <NAME>. <PRODUCT> can be used to <PURPOSE>.";

		// Token: 0x04002048 RID: 8264
		[Header("References")]
		public ShopInterface Shop;

		// Token: 0x04002049 RID: 8265
		public SupplierStash Stash;

		// Token: 0x0400204A RID: 8266
		public UnityEvent onDeaddropReady;

		// Token: 0x0400204B RID: 8267
		private int minsSinceMeetingStart = -1;

		// Token: 0x0400204C RID: 8268
		private int minsSinceLastMeetingEnd = 720;

		// Token: 0x0400204D RID: 8269
		private SupplierLocation currentLocation;

		// Token: 0x0400204E RID: 8270
		private DialogueController dialogueController;

		// Token: 0x0400204F RID: 8271
		private DialogueController.GreetingOverride meetingGreeting;

		// Token: 0x04002050 RID: 8272
		private DialogueController.DialogueChoice meetingChoice;

		// Token: 0x04002051 RID: 8273
		[SyncVar]
		public float debt;

		// Token: 0x04002052 RID: 8274
		[SyncVar]
		public bool deadDropPreparing;

		// Token: 0x04002054 RID: 8276
		private StringIntPair[] deaddropItems;

		// Token: 0x04002055 RID: 8277
		private int minsSinceDeaddropOrder;

		// Token: 0x04002056 RID: 8278
		private bool repaymentReminderSent;

		// Token: 0x04002057 RID: 8279
		public SyncVar<float> syncVar___debt;

		// Token: 0x04002058 RID: 8280
		public SyncVar<bool> syncVar___deadDropPreparing;

		// Token: 0x04002059 RID: 8281
		private bool dll_Excuted;

		// Token: 0x0400205A RID: 8282
		private bool dll_Excuted;

		// Token: 0x02000669 RID: 1641
		public enum ESupplierStatus
		{
			// Token: 0x0400205C RID: 8284
			Idle,
			// Token: 0x0400205D RID: 8285
			PreppingDeadDrop,
			// Token: 0x0400205E RID: 8286
			Meeting
		}
	}
}
