using System;
using Pathfinding;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007E1 RID: 2017
	public class PathGroup
	{
		// Token: 0x040027DD RID: 10205
		public Vector3 entryPoint;

		// Token: 0x040027DE RID: 10206
		public Path startToEntryPath;

		// Token: 0x040027DF RID: 10207
		public Path entryToExitPath;

		// Token: 0x040027E0 RID: 10208
		public Path exitToDestinationPath;
	}
}
