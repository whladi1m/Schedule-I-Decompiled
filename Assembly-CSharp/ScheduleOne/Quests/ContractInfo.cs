using System;
using System.Collections.Generic;
using ScheduleOne.Dialogue;
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.Money;
using ScheduleOne.Product;

namespace ScheduleOne.Quests
{
	// Token: 0x020002D7 RID: 727
	[Serializable]
	public class ContractInfo
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x00046659 File Offset: 0x00044859
		// (set) Token: 0x06000FCA RID: 4042 RVA: 0x00046661 File Offset: 0x00044861
		public DeliveryLocation DeliveryLocation { get; private set; }

		// Token: 0x06000FCB RID: 4043 RVA: 0x0004666C File Offset: 0x0004486C
		public ContractInfo(float payment, ProductList products, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, bool expires, int expiresAfter, int pickupScheduleIndex, bool isCounterOffer)
		{
			this.Payment = payment;
			this.Products = products;
			this.DeliveryLocationGUID = deliveryLocationGUID;
			this.DeliveryWindow = deliveryWindow;
			this.Expires = expires;
			this.ExpiresAfter = expiresAfter;
			this.PickupScheduleIndex = pickupScheduleIndex;
			this.IsCounterOffer = isCounterOffer;
			if (GUIDManager.IsGUIDValid(deliveryLocationGUID))
			{
				this.DeliveryLocation = GUIDManager.GetObject<DeliveryLocation>(new Guid(deliveryLocationGUID));
			}
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0000494F File Offset: 0x00002B4F
		public ContractInfo()
		{
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x000466D8 File Offset: 0x000448D8
		public DialogueChain ProcessMessage(DialogueChain messageChain)
		{
			if (this.DeliveryLocation == null && GUIDManager.IsGUIDValid(this.DeliveryLocationGUID))
			{
				this.DeliveryLocation = GUIDManager.GetObject<DeliveryLocation>(new Guid(this.DeliveryLocationGUID));
			}
			List<string> list = new List<string>();
			string[] lines = messageChain.Lines;
			for (int i = 0; i < lines.Length; i++)
			{
				string text = lines[i].Replace("<PRICE>", "<color=#46CB4F>" + MoneyManager.FormatAmount(this.Payment, false, false) + "</color>");
				text = text.Replace("<PRODUCT>", this.Products.GetCommaSeperatedString());
				text = text.Replace("<QUALITY>", this.Products.GetQualityString());
				text = text.Replace("<LOCATION>", "<b>" + this.DeliveryLocation.GetDescription() + "</b>");
				text = text.Replace("<WINDOW_START>", TimeManager.Get12HourTime((float)this.DeliveryWindow.WindowStartTime, true));
				text = text.Replace("<WINDOW_END>", TimeManager.Get12HourTime((float)this.DeliveryWindow.WindowEndTime, true));
				list.Add(text);
			}
			return new DialogueChain
			{
				Lines = list.ToArray()
			};
		}

		// Token: 0x04001064 RID: 4196
		public float Payment;

		// Token: 0x04001065 RID: 4197
		public ProductList Products;

		// Token: 0x04001066 RID: 4198
		public string DeliveryLocationGUID;

		// Token: 0x04001067 RID: 4199
		public QuestWindowConfig DeliveryWindow;

		// Token: 0x04001068 RID: 4200
		public bool Expires;

		// Token: 0x04001069 RID: 4201
		public int ExpiresAfter;

		// Token: 0x0400106A RID: 4202
		public int PickupScheduleIndex;

		// Token: 0x0400106B RID: 4203
		public bool IsCounterOffer;
	}
}
