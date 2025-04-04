using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000410 RID: 1040
	[Serializable]
	public class GridItemData : BuildableItemData
	{
		// Token: 0x06001570 RID: 5488 RVA: 0x0005F674 File Offset: 0x0005D874
		public GridItemData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation) : base(guid, item, loadOrder)
		{
			this.GridGUID = grid.GUID.ToString();
			this.OriginCoordinate = originCoordinate;
			this.Rotation = rotation;
		}

		// Token: 0x04001404 RID: 5124
		public string GridGUID;

		// Token: 0x04001405 RID: 5125
		public Vector2 OriginCoordinate;

		// Token: 0x04001406 RID: 5126
		public int Rotation;
	}
}
