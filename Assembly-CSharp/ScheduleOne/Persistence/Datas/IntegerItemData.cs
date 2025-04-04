using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003DD RID: 989
	[Serializable]
	public class IntegerItemData : ItemData
	{
		// Token: 0x0600153B RID: 5435 RVA: 0x0005F050 File Offset: 0x0005D250
		public IntegerItemData(string iD, int quantity, int value) : base(iD, quantity)
		{
			this.Value = value;
		}

		// Token: 0x04001390 RID: 5008
		public int Value;
	}
}
