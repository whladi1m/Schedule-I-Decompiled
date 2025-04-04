using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000418 RID: 1048
	public class ToggleableItemData : GridItemData
	{
		// Token: 0x06001578 RID: 5496 RVA: 0x0005F7F8 File Offset: 0x0005D9F8
		public ToggleableItemData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, bool isOn) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.IsOn = isOn;
		}

		// Token: 0x0400141E RID: 5150
		public bool IsOn;
	}
}
