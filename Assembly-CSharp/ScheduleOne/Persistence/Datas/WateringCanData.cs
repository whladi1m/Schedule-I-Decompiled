using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E5 RID: 997
	[Serializable]
	public class WateringCanData : ItemData
	{
		// Token: 0x06001543 RID: 5443 RVA: 0x0005F0C9 File Offset: 0x0005D2C9
		public WateringCanData(string iD, int quantity, float currentFillLevel) : base(iD, quantity)
		{
			this.CurrentFillAmount = currentFillLevel;
		}

		// Token: 0x04001399 RID: 5017
		public float CurrentFillAmount;
	}
}
