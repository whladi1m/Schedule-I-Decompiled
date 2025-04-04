using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007B5 RID: 1973
	[RequireComponent(typeof(BoxCollider))]
	public class SpeedZone : MonoBehaviour
	{
		// Token: 0x0600360E RID: 13838 RVA: 0x000E39C4 File Offset: 0x000E1BC4
		public virtual void Awake()
		{
			SpeedZone.speedZones.Add(this);
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000E39D4 File Offset: 0x000E1BD4
		public static List<SpeedZone> GetSpeedZones(Vector3 point)
		{
			List<SpeedZone> list = new List<SpeedZone>();
			for (int i = 0; i < SpeedZone.speedZones.Count; i++)
			{
				if (SpeedZone.speedZones[i].col.bounds.Contains(point))
				{
					list.Add(SpeedZone.speedZones[i]);
				}
			}
			return list;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmos()
		{
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000045B1 File Offset: 0x000027B1
		private void OnDrawGizmosSelected()
		{
		}

		// Token: 0x040026E0 RID: 9952
		public static List<SpeedZone> speedZones = new List<SpeedZone>();

		// Token: 0x040026E1 RID: 9953
		public BoxCollider col;

		// Token: 0x040026E2 RID: 9954
		public float speed = 20f;
	}
}
