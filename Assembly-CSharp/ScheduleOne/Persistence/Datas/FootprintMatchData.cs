using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003CE RID: 974
	[Serializable]
	public class FootprintMatchData
	{
		// Token: 0x0600151E RID: 5406 RVA: 0x0005ED77 File Offset: 0x0005CF77
		public FootprintMatchData(string tileOwnerGUID, int tileIndex, Vector2 footprintCoordinate)
		{
			this.TileOwnerGUID = tileOwnerGUID;
			this.TileIndex = tileIndex;
			this.FootprintCoordinate = footprintCoordinate;
		}

		// Token: 0x04001375 RID: 4981
		public string TileOwnerGUID;

		// Token: 0x04001376 RID: 4982
		public int TileIndex;

		// Token: 0x04001377 RID: 4983
		public Vector2 FootprintCoordinate;
	}
}
