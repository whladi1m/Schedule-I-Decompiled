using System;
using System.Collections.Generic;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Product;
using ScheduleOne.Quests;
using ScheduleOne.UI.Phone.Messages;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x0200065F RID: 1631
	[Serializable]
	public class DealGenerationEvent
	{
		// Token: 0x06002D12 RID: 11538 RVA: 0x000BCB28 File Offset: 0x000BAD28
		public ContractInfo GenerateContractInfo(Customer customer)
		{
			return new ContractInfo(this.Payment, this.ProductList, this.DeliveryLocation.GUID.ToString(), this.DeliveryWindow, this.Expires, this.ExpiresAfter, this.PickupScheduleGroup, false);
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000BCB78 File Offset: 0x000BAD78
		public bool ShouldGenerate(Customer customer)
		{
			if (customer.NPC.RelationData.RelationDelta < this.RelationshipRequirement)
			{
				return false;
			}
			return this.ApplicableDays.Exists((DealGenerationEvent.DayContainer x) => x.Day == NetworkSingleton<TimeManager>.Instance.CurrentDay) && NetworkSingleton<TimeManager>.Instance.IsCurrentTimeWithinRange(this.GenerationTime, TimeManager.AddMinutesTo24HourTime(this.GenerationTime, this.GenerationWindowDuration));
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000BCBF3 File Offset: 0x000BADF3
		public MessageChain GetRandomRequestMessage()
		{
			return this.ProcessMessage(this.RequestMessageChains[UnityEngine.Random.Range(0, this.RequestMessageChains.Length - 1)]);
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000BCC14 File Offset: 0x000BAE14
		public MessageChain ProcessMessage(MessageChain messageChain)
		{
			MessageChain messageChain2 = new MessageChain();
			foreach (string text in messageChain.Messages)
			{
				string text2 = text.Replace("<PRICE>", "<color=#46CB4F>" + MoneyManager.FormatAmount(this.Payment, false, false) + "</color>");
				text2 = text2.Replace("<PRODUCT>", this.GetProductStringList());
				text2 = text2.Replace("<QUALITY>", this.GetQualityString());
				text2 = text2.Replace("<LOCATION>", this.DeliveryLocation.GetDescription());
				text2 = text2.Replace("<WINDOW_START>", TimeManager.Get12HourTime((float)this.DeliveryWindow.WindowStartTime, true));
				text2 = text2.Replace("<WINDOW_END>", TimeManager.Get12HourTime((float)this.DeliveryWindow.WindowEndTime, true));
				messageChain2.Messages.Add(text2);
			}
			return messageChain2;
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000BCD18 File Offset: 0x000BAF18
		public MessageChain GetRejectionMessage()
		{
			return this.ProcessMessage(this.ContractRejectedResponses[UnityEngine.Random.Range(0, this.ContractRejectedResponses.Length - 1)]);
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000BCD37 File Offset: 0x000BAF37
		public MessageChain GetAcceptanceMessage()
		{
			return this.ProcessMessage(this.ContractAcceptedResponses[UnityEngine.Random.Range(0, this.ContractAcceptedResponses.Length - 1)]);
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000BCD56 File Offset: 0x000BAF56
		public string GetProductStringList()
		{
			return this.ProductList.GetCommaSeperatedString();
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000BCD63 File Offset: 0x000BAF63
		public string GetQualityString()
		{
			return this.ProductList.GetQualityString();
		}

		// Token: 0x0400200A RID: 8202
		[Header("Settings")]
		public bool Enabled = true;

		// Token: 0x0400200B RID: 8203
		public bool CanBeAccepted = true;

		// Token: 0x0400200C RID: 8204
		public bool CanBeRejected = true;

		// Token: 0x0400200D RID: 8205
		[Header("Timing Settings")]
		public List<DealGenerationEvent.DayContainer> ApplicableDays = new List<DealGenerationEvent.DayContainer>();

		// Token: 0x0400200E RID: 8206
		public int GenerationTime;

		// Token: 0x0400200F RID: 8207
		public int GenerationWindowDuration = 60;

		// Token: 0x04002010 RID: 8208
		[Header("Products and payment")]
		public ProductList ProductList;

		// Token: 0x04002011 RID: 8209
		public float Payment = 100f;

		// Token: 0x04002012 RID: 8210
		[Range(0f, 5f)]
		public float RelationshipRequirement = 1f;

		// Token: 0x04002013 RID: 8211
		[Header("Messages")]
		[SerializeField]
		private MessageChain[] RequestMessageChains;

		// Token: 0x04002014 RID: 8212
		public MessageChain[] ContractAcceptedResponses;

		// Token: 0x04002015 RID: 8213
		public MessageChain[] ContractRejectedResponses;

		// Token: 0x04002016 RID: 8214
		[Header("Location settings")]
		public DeliveryLocation DeliveryLocation;

		// Token: 0x04002017 RID: 8215
		public int PickupScheduleGroup;

		// Token: 0x04002018 RID: 8216
		[Header("Window/expiry settings")]
		public QuestWindowConfig DeliveryWindow;

		// Token: 0x04002019 RID: 8217
		public bool Expires = true;

		// Token: 0x0400201A RID: 8218
		[Tooltip("How many days after being accepted does this contract expire? Exact expiry is adjusted to match window end")]
		[Range(1f, 7f)]
		public int ExpiresAfter = 2;

		// Token: 0x02000660 RID: 1632
		[Serializable]
		public class DayContainer
		{
			// Token: 0x0400201B RID: 8219
			public EDay Day;
		}
	}
}
