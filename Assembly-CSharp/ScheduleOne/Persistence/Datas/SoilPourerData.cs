using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000417 RID: 1047
	public class SoilPourerData : GridItemData
	{
		// Token: 0x06001577 RID: 5495 RVA: 0x0005F7DF File Offset: 0x0005D9DF
		public SoilPourerData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, string soilID) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.SoilID = soilID;
		}

		// Token: 0x0400141D RID: 5149
		public string SoilID;
	}
}
