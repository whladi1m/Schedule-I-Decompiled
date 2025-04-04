using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200040D RID: 1037
	public class CauldronData : GridItemData
	{
		// Token: 0x0600156D RID: 5485 RVA: 0x0005F5B8 File Offset: 0x0005D7B8
		public CauldronData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet ingredients, ItemSet liquid, ItemSet output, int remainingCookTime, EQuality inputQuality) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Ingredients = ingredients;
			this.Liquid = liquid;
			this.Output = output;
			this.RemainingCookTime = remainingCookTime;
			this.InputQuality = inputQuality;
		}

		// Token: 0x040013F5 RID: 5109
		public ItemSet Ingredients;

		// Token: 0x040013F6 RID: 5110
		public ItemSet Liquid;

		// Token: 0x040013F7 RID: 5111
		public ItemSet Output;

		// Token: 0x040013F8 RID: 5112
		public int RemainingCookTime;

		// Token: 0x040013F9 RID: 5113
		public EQuality InputQuality;
	}
}
