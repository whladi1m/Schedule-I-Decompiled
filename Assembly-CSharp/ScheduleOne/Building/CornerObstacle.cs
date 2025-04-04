using System;
using System.Collections.Generic;
using ScheduleOne.Tiles;
using UnityEngine;

namespace ScheduleOne.Building
{
	// Token: 0x0200077E RID: 1918
	public class CornerObstacle : MonoBehaviour
	{
		// Token: 0x06003445 RID: 13381 RVA: 0x000DBE0C File Offset: 0x000DA00C
		public List<Tile> GetNeighbourTiles(Tile pairedTile)
		{
			List<Tile> list = new List<Tile>();
			List<Tile> surroundingTiles = pairedTile.GetSurroundingTiles();
			surroundingTiles.Add(pairedTile);
			for (int i = 0; i < surroundingTiles.Count; i++)
			{
				if (Vector3.Distance(surroundingTiles[i].transform.position, base.transform.position) < 0.5f)
				{
					list.Add(surroundingTiles[i]);
				}
			}
			return list;
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x000DBE74 File Offset: 0x000DA074
		private bool ApproxEquals(float a, float b, float precision)
		{
			return Mathf.Abs(a - b) <= precision;
		}

		// Token: 0x0400257D RID: 9597
		public bool obstacleEnabled;

		// Token: 0x0400257E RID: 9598
		public FootprintTile parentFootprint;

		// Token: 0x0400257F RID: 9599
		public Vector2 coordinates = Vector2.zero;
	}
}
