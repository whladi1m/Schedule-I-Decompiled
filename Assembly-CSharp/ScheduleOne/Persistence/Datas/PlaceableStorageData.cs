using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000414 RID: 1044
	[Serializable]
	public class PlaceableStorageData : GridItemData
	{
		// Token: 0x06001574 RID: 5492 RVA: 0x0005F75E File Offset: 0x0005D95E
		public PlaceableStorageData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet contents) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Contents = contents;
		}

		// Token: 0x04001414 RID: 5140
		public ItemSet Contents;
	}
}
