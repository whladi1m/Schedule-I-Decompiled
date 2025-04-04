using System;
using FishNet.Object;
using ScheduleOne.EntityFramework;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002B5 RID: 693
	[Serializable]
	public struct CoordinateProceduralTilePair
	{
		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00041AE6 File Offset: 0x0003FCE6
		public ProceduralTile tile
		{
			get
			{
				return this.tileParent.GetComponent<IProceduralTileContainer>().ProceduralTiles[this.tileIndex];
			}
		}

		// Token: 0x04000F45 RID: 3909
		public Coordinate coord;

		// Token: 0x04000F46 RID: 3910
		public NetworkObject tileParent;

		// Token: 0x04000F47 RID: 3911
		public int tileIndex;
	}
}
