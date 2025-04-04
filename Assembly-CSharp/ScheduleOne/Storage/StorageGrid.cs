using System;
using System.Collections.Generic;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Storage
{
	// Token: 0x02000893 RID: 2195
	public class StorageGrid : MonoBehaviour
	{
		// Token: 0x06003BB2 RID: 15282 RVA: 0x000FB9BC File Offset: 0x000F9BBC
		protected virtual void Awake()
		{
			this.ProcessCoordinateTilePairs();
			this.freeTiles.AddRange(this.storageTiles);
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x000FB9D8 File Offset: 0x000F9BD8
		private void ProcessCoordinateTilePairs()
		{
			foreach (CoordinateStorageTilePair coordinateStorageTilePair in this.coordinateStorageTilePairs)
			{
				this.coordinateToTile.Add(coordinateStorageTilePair.coord, coordinateStorageTilePair.tile);
			}
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x000FBA3C File Offset: 0x000F9C3C
		public void RegisterTile(StorageTile tile)
		{
			this.storageTiles.Add(tile);
			CoordinateStorageTilePair item = default(CoordinateStorageTilePair);
			item.coord = new Coordinate(tile.x, tile.y);
			item.tile = tile;
			this.coordinateStorageTilePairs.Add(item);
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x000FBA8C File Offset: 0x000F9C8C
		public void DeregisterTile(StorageTile tile)
		{
			this.storageTiles.Remove(tile);
			for (int i = 0; i < this.coordinateStorageTilePairs.Count; i++)
			{
				if (this.coordinateStorageTilePairs[i].tile == tile)
				{
					this.coordinateStorageTilePairs.RemoveAt(i);
					i--;
					return;
				}
			}
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x000FBAE8 File Offset: 0x000F9CE8
		public bool IsItemPositionValid(StorageTile primaryTile, FootprintTile primaryFootprintTile, StoredItem item)
		{
			foreach (CoordinateStorageFootprintTilePair coordinateStorageFootprintTilePair in item.CoordinateFootprintTilePairs)
			{
				Coordinate matchedCoordinate = this.GetMatchedCoordinate(coordinateStorageFootprintTilePair.tile);
				if (!this.IsGridPositionValid(matchedCoordinate, coordinateStorageFootprintTilePair.tile))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x000FBB58 File Offset: 0x000F9D58
		public Coordinate GetMatchedCoordinate(FootprintTile tileToMatch)
		{
			Vector3 vector = base.transform.InverseTransformPoint(tileToMatch.transform.position);
			return new Coordinate(Mathf.RoundToInt(vector.x / StorageGrid.gridSize), Mathf.RoundToInt(vector.z / StorageGrid.gridSize));
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x000FBBA3 File Offset: 0x000F9DA3
		public bool IsGridPositionValid(Coordinate gridCoord, FootprintTile tile)
		{
			return this.coordinateToTile.ContainsKey(gridCoord) && !(this.coordinateToTile[gridCoord].occupant != null);
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x000FBBD4 File Offset: 0x000F9DD4
		public StorageTile GetTile(Coordinate coord)
		{
			for (int i = 0; i < this.coordinateStorageTilePairs.Count; i++)
			{
				if (this.coordinateStorageTilePairs[i].coord.Equals(coord))
				{
					return this.coordinateStorageTilePairs[i].tile;
				}
			}
			return null;
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x000FBC24 File Offset: 0x000F9E24
		public int GetUserEndCapacity()
		{
			int actualY = this.GetActualY();
			int num = this.coordinateStorageTilePairs.Count / actualY;
			return (actualY - 1) * (num - 1);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x000FBC50 File Offset: 0x000F9E50
		public int GetActualY()
		{
			int result = 0;
			for (int i = 0; i < this.coordinateStorageTilePairs.Count; i++)
			{
				if (this.coordinateStorageTilePairs[i].coord.x != 0)
				{
					result = i;
					break;
				}
				i++;
			}
			return result;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x000FBC98 File Offset: 0x000F9E98
		public int GetActualX()
		{
			return this.coordinateStorageTilePairs.Count / this.GetActualY();
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x000FBCAC File Offset: 0x000F9EAC
		public int GetTotalFootprintSize()
		{
			return this.coordinateStorageTilePairs.Count;
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x000FBCBC File Offset: 0x000F9EBC
		public bool TryFitItem(int sizeX, int sizeY, List<Coordinate> lockedCoordinates, out Coordinate originCoordinate, out float rotation)
		{
			foreach (CoordinateStorageTilePair coordinateStorageTilePair in this.coordinateStorageTilePairs)
			{
				if (!(coordinateStorageTilePair.tile.occupant != null))
				{
					originCoordinate = coordinateStorageTilePair.coord;
					bool flag = true;
					rotation = 0f;
					for (int i = 0; i < sizeX; i++)
					{
						for (int j = 0; j < sizeY; j++)
						{
							Coordinate coordinate = new Coordinate(coordinateStorageTilePair.tile.x + i, coordinateStorageTilePair.tile.y + j);
							for (int k = 0; k < lockedCoordinates.Count; k++)
							{
								if (coordinate.Equals(lockedCoordinates[k]))
								{
									flag = false;
								}
							}
							StorageTile tile = this.GetTile(coordinate);
							if (tile == null || tile.occupant != null)
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						return true;
					}
					flag = true;
					rotation = 90f;
					for (int l = 0; l < sizeX; l++)
					{
						for (int m = 0; m < sizeY; m++)
						{
							Coordinate coordinate2 = new Coordinate(coordinateStorageTilePair.tile.x + m, coordinateStorageTilePair.tile.y - l);
							for (int n = 0; n < lockedCoordinates.Count; n++)
							{
								if (coordinate2.Equals(lockedCoordinates[n]))
								{
									flag = false;
								}
							}
							StorageTile tile2 = this.GetTile(coordinate2);
							if (tile2 == null || tile2.occupant != null)
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						return true;
					}
				}
			}
			originCoordinate = new Coordinate(0, 0);
			rotation = 0f;
			return false;
		}

		// Token: 0x04002B1A RID: 11034
		public static float gridSize = 0.25f;

		// Token: 0x04002B1B RID: 11035
		public List<StorageTile> storageTiles = new List<StorageTile>();

		// Token: 0x04002B1C RID: 11036
		public List<StorageTile> freeTiles = new List<StorageTile>();

		// Token: 0x04002B1D RID: 11037
		public List<CoordinateStorageTilePair> coordinateStorageTilePairs = new List<CoordinateStorageTilePair>();

		// Token: 0x04002B1E RID: 11038
		protected Dictionary<Coordinate, StorageTile> coordinateToTile = new Dictionary<Coordinate, StorageTile>();
	}
}
