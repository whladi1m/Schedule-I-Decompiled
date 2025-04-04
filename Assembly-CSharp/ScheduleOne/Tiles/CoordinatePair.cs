using System;

namespace ScheduleOne.Tiles
{
	// Token: 0x020002B7 RID: 695
	public class CoordinatePair
	{
		// Token: 0x06000EDE RID: 3806 RVA: 0x00041B03 File Offset: 0x0003FD03
		public CoordinatePair(Coordinate _c1, Coordinate _c2)
		{
			this.coord1 = _c1;
			this.coord2 = _c2;
		}

		// Token: 0x04000F4A RID: 3914
		public Coordinate coord1;

		// Token: 0x04000F4B RID: 3915
		public Coordinate coord2;
	}
}
