using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000406 RID: 1030
	public class DealerData : NPCData
	{
		// Token: 0x06001566 RID: 5478 RVA: 0x0005F414 File Offset: 0x0005D614
		public DealerData(string id, bool recruited, string[] assignedCustomerIDs, string[] activeContractGUIDs, float cash, ItemSet overflowItems, bool hasBeenRecommended) : base(id)
		{
			this.Recruited = recruited;
			this.AssignedCustomerIDs = assignedCustomerIDs;
			this.ActiveContractGUIDs = activeContractGUIDs;
			this.Cash = cash;
			this.OverflowItems = overflowItems;
			this.HasBeenRecommended = hasBeenRecommended;
		}

		// Token: 0x040013D7 RID: 5079
		public bool Recruited;

		// Token: 0x040013D8 RID: 5080
		public string[] AssignedCustomerIDs;

		// Token: 0x040013D9 RID: 5081
		public string[] ActiveContractGUIDs;

		// Token: 0x040013DA RID: 5082
		public float Cash;

		// Token: 0x040013DB RID: 5083
		public ItemSet OverflowItems;

		// Token: 0x040013DC RID: 5084
		public bool HasBeenRecommended;
	}
}
