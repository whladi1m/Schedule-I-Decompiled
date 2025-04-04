using System;
using System.Collections.Generic;
using ScheduleOne.Building;
using ScheduleOne.EntityFramework;
using UnityEngine;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002B8 RID: 696
	public class FootprintTile : MonoBehaviour
	{
		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x00041B19 File Offset: 0x0003FD19
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x00041B21 File Offset: 0x0003FD21
		public Tile MatchedStandardTile { get; protected set; }

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00041B2A File Offset: 0x0003FD2A
		protected virtual void Awake()
		{
			this.tileAppearance.SetVisible(false);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00041B38 File Offset: 0x0003FD38
		public virtual void Initialize(Tile matchedTile)
		{
			this.MatchedStandardTile = matchedTile;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00041B44 File Offset: 0x0003FD44
		public bool AreCornerObstaclesBlocked(Tile proposedTile)
		{
			if (proposedTile == null)
			{
				return true;
			}
			for (int i = 0; i < this.Corners.Count; i++)
			{
				if (this.Corners[i].obstacleEnabled)
				{
					List<Tile> neighbourTiles = this.Corners[i].GetNeighbourTiles(proposedTile);
					if (neighbourTiles.Count >= 4)
					{
						Dictionary<GridItem, int> dictionary = new Dictionary<GridItem, int>();
						for (int j = 0; j < neighbourTiles.Count; j++)
						{
							for (int k = 0; k < neighbourTiles[j].BuildableOccupants.Count; k++)
							{
								if (!dictionary.ContainsKey(neighbourTiles[j].BuildableOccupants[k]))
								{
									dictionary.Add(neighbourTiles[j].BuildableOccupants[k], 1);
								}
								else
								{
									Dictionary<GridItem, int> dictionary2 = dictionary;
									GridItem key = neighbourTiles[j].BuildableOccupants[k];
									int num = dictionary2[key];
									dictionary2[key] = num + 1;
								}
							}
						}
						foreach (GridItem key2 in dictionary.Keys)
						{
							if (dictionary[key2] == neighbourTiles.Count)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04000F4C RID: 3916
		public TileAppearance tileAppearance;

		// Token: 0x04000F4D RID: 3917
		public TileDetector tileDetector;

		// Token: 0x04000F4E RID: 3918
		public int X;

		// Token: 0x04000F4F RID: 3919
		public int Y;

		// Token: 0x04000F50 RID: 3920
		public float RequiredOffset;

		// Token: 0x04000F51 RID: 3921
		public List<CornerObstacle> Corners = new List<CornerObstacle>();
	}
}
