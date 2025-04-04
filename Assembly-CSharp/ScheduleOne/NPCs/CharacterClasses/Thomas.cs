using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.ItemFramework;
using ScheduleOne.Messaging;
using ScheduleOne.Money;
using ScheduleOne.NPCs.Schedules;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Datas.Characters;
using ScheduleOne.Product;
using ScheduleOne.Quests;
using ScheduleOne.UI.Handover;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.NPCs.CharacterClasses
{
	// Token: 0x020004CE RID: 1230
	public class Thomas : NPC
	{
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x000722FB File Offset: 0x000704FB
		// (set) Token: 0x06001B6D RID: 7021 RVA: 0x00072303 File Offset: 0x00070503
		public bool MeetingReminderSent { get; protected set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001B6E RID: 7022 RVA: 0x0007230C File Offset: 0x0007050C
		// (set) Token: 0x06001B6F RID: 7023 RVA: 0x00072314 File Offset: 0x00070514
		public bool HandoverReminderSent { get; protected set; }

		// Token: 0x06001B70 RID: 7024 RVA: 0x0007231D File Offset: 0x0007051D
		protected override void Start()
		{
			base.Start();
			this.dialogueHandler.onDialogueChoiceChosen.AddListener(new UnityAction<string>(this.DialogueChoiceCallback));
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x00072341 File Offset: 0x00070541
		public void SetFirstMeetingEventActive(bool active)
		{
			this.FirstMeetingEvent.gameObject.SetActive(active);
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x00072354 File Offset: 0x00070554
		public void SetHandoverEventActive(bool active)
		{
			this.HandoverEvent.gameObject.SetActive(active);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x00072368 File Offset: 0x00070568
		public void SendMeetingReminder()
		{
			if (this.MeetingReminderSent)
			{
				Console.LogWarning("Reminder message already sent", null);
				return;
			}
			this.MeetingReminderSent = true;
			base.HasChanged = true;
			Message message = new Message();
			message.text = "Either you haven't read our note or are choosing to ignore it - for your sake I'll assume the former. We have business to discuss at Hyland Manor ASAP. - TB";
			message.sender = Message.ESenderType.Other;
			message.endOfGroup = true;
			base.MSGConversation.SetIsKnown(false);
			base.MSGConversation.SendMessage(message, true, true);
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x000723D0 File Offset: 0x000705D0
		public void SendHandoverReminder()
		{
			if (this.HandoverReminderSent)
			{
				Console.LogWarning("Reminder message already sent", null);
				return;
			}
			Debug.Log("Sending reminder");
			this.HandoverReminderSent = true;
			base.HasChanged = true;
			Message message = new Message();
			message.text = "You haven't yet made this week's delivery. There are 24 hours left. Don't make this difficult. - TB";
			message.sender = Message.ESenderType.Other;
			message.endOfGroup = true;
			base.MSGConversation.SendMessage(message, true, true);
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x00072436 File Offset: 0x00070636
		public void InitialMeetingComplete()
		{
			base.MSGConversation.SetIsKnown(true);
			this.SetFirstMeetingEventActive(false);
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0007244C File Offset: 0x0007064C
		private void DialogueChoiceCallback(string choiceLabel)
		{
			if (choiceLabel == "BEGIN_HANDOVER")
			{
				ProductList productList = new ProductList();
				productList.entries.Add(new ProductList.Entry
				{
					ProductID = "ogkush",
					Quantity = 15,
					Quality = EQuality.Trash
				});
				Contract contract = new GameObject("CartelContract").AddComponent<Contract>();
				contract.transform.SetParent(base.transform);
				contract.SilentlyInitializeContract("Cartel Contract", "Deliver the goods to the cartel", new QuestEntryData[0], string.Empty, base.NetworkObject, 100f, productList, string.Empty, new QuestWindowConfig(), 0, NetworkSingleton<ScheduleOne.GameTime.TimeManager>.Instance.GetDateTime());
				Singleton<HandoverScreen>.Instance.Open(contract, base.GetComponent<Customer>(), HandoverScreen.EMode.Contract, new Action<HandoverScreen.EHandoverOutcome, List<ItemInstance>, float>(this.ProcessItemHandover), null);
			}
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x00072518 File Offset: 0x00070718
		private void ProcessItemHandover(HandoverScreen.EHandoverOutcome outcome, List<ItemInstance> items, float price)
		{
			if (outcome == HandoverScreen.EHandoverOutcome.Cancelled)
			{
				return;
			}
			Singleton<HandoverScreen>.Instance.ClearCustomerSlots(false);
			this.SetHandoverEventActive(false);
			this.HandoverReminderSent = false;
			base.HasChanged = true;
			NetworkSingleton<MoneyManager>.Instance.ChangeCashBalance(100f, true, false);
			if (this.onCartelContractReceived != null)
			{
				this.onCartelContractReceived.Invoke();
			}
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0007256D File Offset: 0x0007076D
		public override string GetSaveString()
		{
			return new ThomasData(this.ID, this.MeetingReminderSent, this.HandoverReminderSent).GetJson(true);
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0007258C File Offset: 0x0007078C
		public override void Load(NPCData data, string containerPath)
		{
			base.Load(data, containerPath);
			string json;
			if (((ISaveable)this).TryLoadFile(containerPath, "NPC", out json))
			{
				ThomasData thomasData = null;
				try
				{
					thomasData = JsonUtility.FromJson<ThomasData>(json);
				}
				catch (Exception ex)
				{
					Console.LogWarning("Failed to deserialize character data: " + ex.Message, null);
					return;
				}
				this.MeetingReminderSent = thomasData.MeetingReminderSent;
				this.HandoverReminderSent = thomasData.HandoverReminderSent;
			}
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x00072600 File Offset: 0x00070800
		public override void NetworkInitialize___Early()
		{
			if (this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ThomasAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize___EarlyScheduleOne.NPCs.CharacterClasses.ThomasAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize___Early();
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x00072619 File Offset: 0x00070819
		public override void NetworkInitialize__Late()
		{
			if (this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ThomasAssembly-CSharp.dll_Excuted)
			{
				return;
			}
			this.NetworkInitialize__LateScheduleOne.NPCs.CharacterClasses.ThomasAssembly-CSharp.dll_Excuted = true;
			base.NetworkInitialize__Late();
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00072632 File Offset: 0x00070832
		public override void NetworkInitializeIfDisabled()
		{
			this.NetworkInitialize___Early();
			this.NetworkInitialize__Late();
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00072640 File Offset: 0x00070840
		public override void Awake()
		{
			this.NetworkInitialize___Early();
			base.Awake();
			this.NetworkInitialize__Late();
		}

		// Token: 0x040016F8 RID: 5880
		public const int CARTEL_CONTRACT_QUANTITY = 15;

		// Token: 0x040016F9 RID: 5881
		public const float CARTEL_CONTRACT_PAYMENT = 100f;

		// Token: 0x040016FA RID: 5882
		public NPCEvent_LocationDialogue FirstMeetingEvent;

		// Token: 0x040016FB RID: 5883
		public NPCEvent_LocationDialogue HandoverEvent;

		// Token: 0x040016FC RID: 5884
		public UnityEvent onCartelContractReceived;

		// Token: 0x040016FF RID: 5887
		private bool dll_Excuted;

		// Token: 0x04001700 RID: 5888
		private bool dll_Excuted;
	}
}
