using System;
using System.Collections.Generic;
using ScheduleOne.EntityFramework;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002BC RID: 700
	public class ProceduralTile : MonoBehaviour
	{
		// Token: 0x06000EF8 RID: 3832 RVA: 0x000045B1 File Offset: 0x000027B1
		protected virtual void Awake()
		{
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00042131 File Offset: 0x00040331
		public void AddOccupant(FootprintTile footprint, ProceduralGridItem item)
		{
			if (!this.Occupants.Contains(item))
			{
				this.Occupants.Add(item);
			}
			if (!this.OccupantTiles.Contains(footprint))
			{
				this.OccupantTiles.Add(footprint);
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00042167 File Offset: 0x00040367
		public void RemoveOccupant(FootprintTile footprint, ProceduralGridItem item)
		{
			if (this.Occupants.Contains(item))
			{
				this.Occupants.Remove(item);
			}
			if (this.OccupantTiles.Contains(footprint))
			{
				this.OccupantTiles.Remove(footprint);
			}
		}

		// Token: 0x04000F5C RID: 3932
		[Header("Settings")]
		public ProceduralTile.EProceduralTileType TileType;

		// Token: 0x04000F5D RID: 3933
		[Header("References")]
		public BuildableItem ParentBuildableItem;

		// Token: 0x04000F5E RID: 3934
		public FootprintTile MatchedFootprintTile;

		// Token: 0x04000F5F RID: 3935
		[Header("Occupants")]
		public List<ProceduralGridItem> Occupants = new List<ProceduralGridItem>();

		// Token: 0x04000F60 RID: 3936
		public List<FootprintTile> OccupantTiles = new List<FootprintTile>();

		// Token: 0x020002BD RID: 701
		public enum EProceduralTileType
		{
			// Token: 0x04000F62 RID: 3938
			Rack
		}
	}
}
