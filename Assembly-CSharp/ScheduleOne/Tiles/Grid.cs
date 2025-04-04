using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.ConstructableScripts;
using ScheduleOne.EntityFramework;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002B9 RID: 697
	public class Grid : MonoBehaviour, IGUIDRegisterable
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00041CBB File Offset: 0x0003FEBB
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x00041CC3 File Offset: 0x0003FEC3
		public Guid GUID { get; protected set; }

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00041CCC File Offset: 0x0003FECC
		public void SetGUID(Guid guid)
		{
			this.GUID = guid;
			GUIDManager.RegisterObject(this);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00041CDC File Offset: 0x0003FEDC
		protected virtual void Awake()
		{
			if (this.IsStatic)
			{
				if (!GUIDManager.IsGUIDValid(this.StaticGUID))
				{
					Console.LogError("Static GUID is not valid.", null);
				}
				((IGUIDRegisterable)this).SetGUID(this.StaticGUID);
			}
			if (base.GetComponentInParent<Property>() != null && !this.IsStatic)
			{
				Debug.LogWarning("Grid is a child of a Property, but is not marked as static!");
			}
			this.SetInvisible();
			this.ProcessCoordinateDataPairs();
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00041D44 File Offset: 0x0003FF44
		public virtual void DestroyGrid()
		{
			GridItem[] componentsInChildren = this.Container.GetComponentsInChildren<GridItem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].DestroyItem(true);
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00041D74 File Offset: 0x0003FF74
		private void ProcessCoordinateDataPairs()
		{
			foreach (CoordinateTilePair coordinateTilePair in this.CoordinateTilePairs)
			{
				this._coordinateToTile.Add(coordinateTilePair.coord, coordinateTilePair.tile);
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00041DD8 File Offset: 0x0003FFD8
		public void RegisterTile(Tile tile)
		{
			this.Tiles.Add(tile);
			CoordinateTilePair item = default(CoordinateTilePair);
			item.coord = new Coordinate(tile.x, tile.y);
			item.tile = tile;
			this.CoordinateTilePairs.Add(item);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00041E28 File Offset: 0x00040028
		public void DeregisterTile(Tile tile)
		{
			Console.Log("Deregistering tile: " + tile.x.ToString() + ", " + tile.y.ToString(), null);
			this.Tiles.Remove(tile);
			for (int i = 0; i < this.CoordinateTilePairs.Count; i++)
			{
				if (this.CoordinateTilePairs[i].tile == tile)
				{
					this.CoordinateTilePairs.RemoveAt(i);
					i--;
					return;
				}
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00041EB0 File Offset: 0x000400B0
		public Coordinate GetMatchedCoordinate(FootprintTile tileToMatch)
		{
			Vector3 vector = base.transform.InverseTransformPoint(tileToMatch.transform.position);
			return new Coordinate(Mathf.RoundToInt(vector.x / Grid.GridSideLength), Mathf.RoundToInt(vector.z / Grid.GridSideLength));
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00041EFC File Offset: 0x000400FC
		public bool IsTileValidAtCoordinate(Coordinate gridCoord, FootprintTile tile, GridItem tileOwner = null)
		{
			if (!this._coordinateToTile.ContainsKey(gridCoord))
			{
				return false;
			}
			Tile tile2 = this._coordinateToTile[gridCoord];
			return tile2.ConstructableOccupants.Count <= 0 && (tile2.BuildableOccupants.Count <= 0 || (!(tileOwner == null) && tileOwner.CanShareTileWith(tile2.BuildableOccupants))) && (tile2.AvailableOffset == 0f || tile.RequiredOffset == 0f || tile2.AvailableOffset >= tile.RequiredOffset) && tile2.CanBeBuiltOn();
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00041F90 File Offset: 0x00040190
		public bool IsTileValidAtCoordinate(Coordinate gridCoord, FootprintTile tile, Constructable_GridBased ignoreConstructable)
		{
			if (!this._coordinateToTile.ContainsKey(gridCoord))
			{
				return false;
			}
			Tile tile2 = this._coordinateToTile[gridCoord];
			if (tile2.BuildableOccupants.Count > 0)
			{
				return false;
			}
			for (int i = 0; i < tile2.ConstructableOccupants.Count; i++)
			{
				if (tile2.ConstructableOccupants[i] != ignoreConstructable)
				{
					return false;
				}
			}
			return (tile2.AvailableOffset == 0f || tile.RequiredOffset == 0f || tile2.AvailableOffset >= tile.RequiredOffset) && tile2.CanBeBuiltOn();
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00042028 File Offset: 0x00040228
		public Tile GetTile(Coordinate coord)
		{
			return this.CoordinateTilePairs.Find((CoordinateTilePair x) => x.coord.Equals(coord)).tile;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00042060 File Offset: 0x00040260
		[Button]
		public void SetVisible()
		{
			for (int i = 0; i < this.CoordinateTilePairs.Count; i++)
			{
				this.CoordinateTilePairs[i].tile.SetVisible(true);
			}
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0004209C File Offset: 0x0004029C
		[Button]
		public void SetInvisible()
		{
			for (int i = 0; i < this.CoordinateTilePairs.Count; i++)
			{
				this.CoordinateTilePairs[i].tile.SetVisible(false);
			}
		}

		// Token: 0x04000F53 RID: 3923
		public static float GridSideLength = 0.5f;

		// Token: 0x04000F54 RID: 3924
		public List<Tile> Tiles = new List<Tile>();

		// Token: 0x04000F55 RID: 3925
		public List<CoordinateTilePair> CoordinateTilePairs = new List<CoordinateTilePair>();

		// Token: 0x04000F56 RID: 3926
		public Transform Container;

		// Token: 0x04000F57 RID: 3927
		public bool IsStatic;

		// Token: 0x04000F58 RID: 3928
		public string StaticGUID = string.Empty;

		// Token: 0x04000F5A RID: 3930
		protected Dictionary<Coordinate, Tile> _coordinateToTile = new Dictionary<Coordinate, Tile>();
	}
}
