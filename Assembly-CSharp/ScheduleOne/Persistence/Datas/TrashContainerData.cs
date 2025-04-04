using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000419 RID: 1049
	public class TrashContainerData : GridItemData
	{
		// Token: 0x06001579 RID: 5497 RVA: 0x0005F811 File Offset: 0x0005DA11
		public TrashContainerData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, TrashContentData contentData) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.ContentData = contentData;
		}

		// Token: 0x0400141F RID: 5151
		public TrashContentData ContentData;
	}
}
