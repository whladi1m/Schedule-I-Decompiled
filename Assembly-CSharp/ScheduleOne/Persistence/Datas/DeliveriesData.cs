using System;
using ScheduleOne.Delivery;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003CD RID: 973
	public class DeliveriesData : SaveData
	{
		// Token: 0x0600151D RID: 5405 RVA: 0x0005ED68 File Offset: 0x0005CF68
		public DeliveriesData(DeliveryInstance[] deliveries)
		{
			this.ActiveDeliveries = deliveries;
		}

		// Token: 0x04001374 RID: 4980
		public DeliveryInstance[] ActiveDeliveries;
	}
}
