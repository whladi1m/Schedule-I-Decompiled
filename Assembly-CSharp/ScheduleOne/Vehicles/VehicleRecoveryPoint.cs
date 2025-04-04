using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007C4 RID: 1988
	public class VehicleRecoveryPoint : MonoBehaviour
	{
		// Token: 0x06003672 RID: 13938 RVA: 0x000E51C9 File Offset: 0x000E33C9
		protected virtual void Awake()
		{
			VehicleRecoveryPoint.recoveryPoints.Add(this);
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x000E51D8 File Offset: 0x000E33D8
		public static VehicleRecoveryPoint GetClosestRecoveryPoint(Vector3 pos)
		{
			VehicleRecoveryPoint vehicleRecoveryPoint = null;
			for (int i = 0; i < VehicleRecoveryPoint.recoveryPoints.Count; i++)
			{
				if (vehicleRecoveryPoint == null || Vector3.Distance(VehicleRecoveryPoint.recoveryPoints[i].transform.position, pos) < Vector3.Distance(vehicleRecoveryPoint.transform.position, pos))
				{
					vehicleRecoveryPoint = VehicleRecoveryPoint.recoveryPoints[i];
				}
			}
			return vehicleRecoveryPoint;
		}

		// Token: 0x04002739 RID: 10041
		public static List<VehicleRecoveryPoint> recoveryPoints = new List<VehicleRecoveryPoint>();
	}
}
