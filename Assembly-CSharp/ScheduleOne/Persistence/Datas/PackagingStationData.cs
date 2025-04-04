using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000413 RID: 1043
	[Serializable]
	public class PackagingStationData : GridItemData
	{
		// Token: 0x06001573 RID: 5491 RVA: 0x0005F745 File Offset: 0x0005D945
		public PackagingStationData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet contents) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Contents = contents;
		}

		// Token: 0x04001413 RID: 5139
		public ItemSet Contents;
	}
}
