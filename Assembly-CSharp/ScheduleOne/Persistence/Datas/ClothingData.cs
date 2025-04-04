using System;
using ScheduleOne.Clothing;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003DB RID: 987
	[Serializable]
	public class ClothingData : ItemData
	{
		// Token: 0x06001539 RID: 5433 RVA: 0x0005F032 File Offset: 0x0005D232
		public ClothingData(string iD, int quantity, EClothingColor color) : base(iD, quantity)
		{
			this.Color = color;
		}

		// Token: 0x0400138F RID: 5007
		public EClothingColor Color;
	}
}
