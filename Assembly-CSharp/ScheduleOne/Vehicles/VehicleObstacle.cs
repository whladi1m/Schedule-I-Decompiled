using System;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007C2 RID: 1986
	public class VehicleObstacle : MonoBehaviour
	{
		// Token: 0x04002733 RID: 10035
		public Collider col;

		// Token: 0x04002734 RID: 10036
		[Header("Settings")]
		public bool twoSided = true;

		// Token: 0x04002735 RID: 10037
		public VehicleObstacle.EObstacleType type;

		// Token: 0x020007C3 RID: 1987
		public enum EObstacleType
		{
			// Token: 0x04002737 RID: 10039
			Generic,
			// Token: 0x04002738 RID: 10040
			TrafficLight
		}
	}
}
