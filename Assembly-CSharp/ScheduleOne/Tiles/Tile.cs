using System;
using System.Collections.Generic;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.EntityFramework;
using ScheduleOne.Lighting;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002BE RID: 702
	[Serializable]
	public class Tile : MonoBehaviour
	{
		// Token: 0x06000EFC RID: 3836 RVA: 0x000421BD File Offset: 0x000403BD
		public void InitializePropertyTile(int _x, int _y, float _available_Offset, Grid _ownerGrid)
		{
			this.x = _x;
			this.y = _y;
			this.AvailableOffset = _available_Offset;
			this.OwnerGrid = _ownerGrid;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x000421DC File Offset: 0x000403DC
		public void AddOccupant(GridItem occ, FootprintTile tile)
		{
			this.BuildableOccupants.Remove(occ);
			this.BuildableOccupants.Add(occ);
			this.OccupantTiles.Remove(tile);
			this.OccupantTiles.Add(tile);
			if (this.onTileChanged != null)
			{
				this.onTileChanged(this);
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00042230 File Offset: 0x00040430
		public void AddOccupant(Constructable_GridBased occ, FootprintTile tile)
		{
			this.ConstructableOccupants.Remove(occ);
			this.ConstructableOccupants.Add(occ);
			this.OccupantTiles.Remove(tile);
			this.OccupantTiles.Add(tile);
			if (this.onTileChanged != null)
			{
				this.onTileChanged(this);
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00042283 File Offset: 0x00040483
		public void RemoveOccupant(GridItem occ, FootprintTile tile)
		{
			this.BuildableOccupants.Remove(occ);
			this.OccupantTiles.Remove(tile);
			if (this.onTileChanged != null)
			{
				this.onTileChanged(this);
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x000422B3 File Offset: 0x000404B3
		public void RemoveOccupant(Constructable_GridBased occ, FootprintTile tile)
		{
			this.ConstructableOccupants.Remove(occ);
			this.OccupantTiles.Remove(tile);
			if (this.onTileChanged != null)
			{
				this.onTileChanged(this);
			}
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x000422E3 File Offset: 0x000404E3
		public virtual bool CanBeBuiltOn()
		{
			return !(this.OwnerGrid.GetComponentInParent<Property>() != null) || this.OwnerGrid.GetComponentInParent<Property>().IsOwned;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00042310 File Offset: 0x00040510
		public List<Tile> GetSurroundingTiles()
		{
			List<Tile> list = new List<Tile>();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					Tile tile = this.OwnerGrid.GetTile(new Coordinate(this.x + i - 1, this.y + j - 1));
					if (tile != null && tile != this && !list.Contains(tile))
					{
						list.Add(tile);
					}
				}
			}
			return list;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00014002 File Offset: 0x00012202
		public virtual bool IsIndoorTile()
		{
			return false;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00042383 File Offset: 0x00040583
		public void SetVisible(bool vis)
		{
			base.transform.Find("Model").gameObject.SetActive(vis);
		}

		// Token: 0x04000F63 RID: 3939
		public static float TileSize = 0.5f;

		// Token: 0x04000F64 RID: 3940
		public int x;

		// Token: 0x04000F65 RID: 3941
		public int y;

		// Token: 0x04000F66 RID: 3942
		[Header("Settings")]
		public float AvailableOffset = 1000f;

		// Token: 0x04000F67 RID: 3943
		[Header("References")]
		public Grid OwnerGrid;

		// Token: 0x04000F68 RID: 3944
		public LightExposureNode LightExposureNode;

		// Token: 0x04000F69 RID: 3945
		[Header("Occupants")]
		public List<GridItem> BuildableOccupants = new List<GridItem>();

		// Token: 0x04000F6A RID: 3946
		public List<Constructable_GridBased> ConstructableOccupants = new List<Constructable_GridBased>();

		// Token: 0x04000F6B RID: 3947
		public List<FootprintTile> OccupantTiles = new List<FootprintTile>();

		// Token: 0x04000F6C RID: 3948
		public Tile.TileChange onTileChanged;

		// Token: 0x020002BF RID: 703
		// (Invoke) Token: 0x06000F08 RID: 3848
		public delegate void TileChange(Tile thisTile);
	}
}
