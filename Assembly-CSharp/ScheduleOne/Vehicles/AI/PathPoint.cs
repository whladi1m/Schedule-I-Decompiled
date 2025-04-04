using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007E2 RID: 2018
	public class PathPoint : MonoBehaviour
	{
		// Token: 0x040027E1 RID: 10209
		public List<PathPoint> connections = new List<PathPoint>();

		// Token: 0x040027E2 RID: 10210
		public bool unique;
	}
}
