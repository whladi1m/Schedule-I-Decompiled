using System;
using ScheduleOne.Product;
using ScheduleOne.Quests;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000423 RID: 1059
	[Serializable]
	public class ContractData : QuestData
	{
		// Token: 0x06001584 RID: 5508 RVA: 0x0005F978 File Offset: 0x0005DB78
		public ContractData(string guid, EQuestState state, bool isTracked, string title, string desc, bool isTimed, GameDateTimeData expiry, QuestEntryData[] entries, string customerGUID, float payment, ProductList productList, string deliveryLocationGUID, QuestWindowConfig deliveryWindow, int pickupScheduleIndex, GameDateTimeData acceptTime) : base(guid, state, isTracked, title, desc, isTimed, expiry, entries)
		{
			this.CustomerGUID = customerGUID;
			this.Payment = payment;
			this.ProductList = productList;
			this.DeliveryLocationGUID = deliveryLocationGUID;
			this.DeliveryWindow = deliveryWindow;
			this.PickupScheduleIndex = pickupScheduleIndex;
			this.AcceptTime = acceptTime;
		}

		// Token: 0x0400143D RID: 5181
		public string CustomerGUID;

		// Token: 0x0400143E RID: 5182
		public float Payment;

		// Token: 0x0400143F RID: 5183
		public ProductList ProductList;

		// Token: 0x04001440 RID: 5184
		public string DeliveryLocationGUID;

		// Token: 0x04001441 RID: 5185
		public QuestWindowConfig DeliveryWindow;

		// Token: 0x04001442 RID: 5186
		public int PickupScheduleIndex;

		// Token: 0x04001443 RID: 5187
		public GameDateTimeData AcceptTime;
	}
}
