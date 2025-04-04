using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200040F RID: 1039
	public class DryingRackData : GridItemData
	{
		// Token: 0x0600156F RID: 5487 RVA: 0x0005F648 File Offset: 0x0005D848
		public DryingRackData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet input, ItemSet output, DryingOperation[] dryingOperations) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.Input = input;
			this.Output = output;
			this.DryingOperations = dryingOperations;
		}

		// Token: 0x04001401 RID: 5121
		public ItemSet Input;

		// Token: 0x04001402 RID: 5122
		public ItemSet Output;

		// Token: 0x04001403 RID: 5123
		public DryingOperation[] DryingOperations;
	}
}
