using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003DA RID: 986
	[Serializable]
	public class CashData : ItemData
	{
		// Token: 0x06001538 RID: 5432 RVA: 0x0005F021 File Offset: 0x0005D221
		public CashData(string iD, int quantity, float cashBalance) : base(iD, quantity)
		{
			this.CashBalance = cashBalance;
		}

		// Token: 0x0400138E RID: 5006
		public float CashBalance;
	}
}
