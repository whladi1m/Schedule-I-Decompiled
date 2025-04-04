using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000412 RID: 1042
	public class MixingStationData : GridItemData
	{
		// Token: 0x06001572 RID: 5490 RVA: 0x0005F70C File Offset: 0x0005D90C
		public MixingStationData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet productContents, ItemSet mixerContents, ItemSet outputContents, MixOperation currentMixOperation, int currentMixTime) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.ProductContents = productContents;
			this.MixerContents = mixerContents;
			this.OutputContents = outputContents;
			this.CurrentMixOperation = currentMixOperation;
			this.CurrentMixTime = currentMixTime;
		}

		// Token: 0x0400140E RID: 5134
		public ItemSet ProductContents;

		// Token: 0x0400140F RID: 5135
		public ItemSet MixerContents;

		// Token: 0x04001410 RID: 5136
		public ItemSet OutputContents;

		// Token: 0x04001411 RID: 5137
		public MixOperation CurrentMixOperation;

		// Token: 0x04001412 RID: 5138
		public int CurrentMixTime;
	}
}
