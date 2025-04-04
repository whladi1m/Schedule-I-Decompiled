using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000415 RID: 1045
	public class PotData : GridItemData
	{
		// Token: 0x06001575 RID: 5493 RVA: 0x0005F778 File Offset: 0x0005D978
		public PotData(Guid guid, ItemInstance item, int loadOrder, Grid grid, Vector2 originCoordinate, int rotation, string soilID, float soilLevel, int remainingSoilUses, float waterLevel, string[] appliedAdditives, PlantData plantData) : base(guid, item, loadOrder, grid, originCoordinate, rotation)
		{
			this.SoilID = soilID;
			this.SoilLevel = soilLevel;
			this.RemainingSoilUses = remainingSoilUses;
			this.WaterLevel = waterLevel;
			this.AppliedAdditives = appliedAdditives;
			this.PlantData = plantData;
		}

		// Token: 0x04001415 RID: 5141
		public string SoilID;

		// Token: 0x04001416 RID: 5142
		public float SoilLevel;

		// Token: 0x04001417 RID: 5143
		public int RemainingSoilUses;

		// Token: 0x04001418 RID: 5144
		public float WaterLevel;

		// Token: 0x04001419 RID: 5145
		public string[] AppliedAdditives;

		// Token: 0x0400141A RID: 5146
		public PlantData PlantData;
	}
}
