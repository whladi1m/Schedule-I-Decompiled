using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200040B RID: 1035
	[Serializable]
	public class BrickPressData : GridItemData
	{
		// Token: 0x0600156B RID: 5483 RVA: 0x0005F56B File Offset: 0x0005D76B
		public BrickPressData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet contents) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Contents = contents;
		}

		// Token: 0x040013F1 RID: 5105
		public ItemSet Contents;
	}
}
