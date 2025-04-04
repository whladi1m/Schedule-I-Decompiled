using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200040E RID: 1038
	public class ChemistryStationData : GridItemData
	{
		// Token: 0x0600156E RID: 5486 RVA: 0x0005F5F4 File Offset: 0x0005D7F4
		public ChemistryStationData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, ItemSet inputContents, ItemSet outputContents, string currentRecipeID, EQuality productQuality, Color startLiquidColor, float liquidLevel, int currentTime) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.InputContents = inputContents;
			this.OutputContents = outputContents;
			this.CurrentRecipeID = currentRecipeID;
			this.ProductQuality = productQuality;
			this.StartLiquidColor = startLiquidColor;
			this.LiquidLevel = liquidLevel;
			this.CurrentTime = currentTime;
		}

		// Token: 0x040013FA RID: 5114
		public ItemSet InputContents;

		// Token: 0x040013FB RID: 5115
		public ItemSet OutputContents;

		// Token: 0x040013FC RID: 5116
		public string CurrentRecipeID;

		// Token: 0x040013FD RID: 5117
		public EQuality ProductQuality;

		// Token: 0x040013FE RID: 5118
		public Color StartLiquidColor;

		// Token: 0x040013FF RID: 5119
		public float LiquidLevel;

		// Token: 0x04001400 RID: 5120
		public int CurrentTime;
	}
}
