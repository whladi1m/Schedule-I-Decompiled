using System;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000529 RID: 1321
	public class VehiclePatrolRoute : MonoBehaviour
	{
		// Token: 0x06002051 RID: 8273 RVA: 0x00084C24 File Offset: 0x00082E24
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(base.transform.position + Vector3.up * 0.5f, 0.5f);
			Gizmos.color = Color.yellow;
			for (int i = 0; i < this.Waypoints.Length; i++)
			{
				if (!(this.Waypoints[i] == null))
				{
					Gizmos.DrawWireSphere(this.Waypoints[i].position + Vector3.up * 0.5f, 0.5f);
				}
			}
			Gizmos.color = Color.red;
			for (int j = 0; j < this.Waypoints.Length - 1; j++)
			{
				if (!(this.Waypoints[j] == null))
				{
					Gizmos.DrawLine(this.Waypoints[j].position + Vector3.up * 0.5f, this.Waypoints[j + 1].position + Vector3.up * 0.5f);
				}
			}
		}

		// Token: 0x040018FF RID: 6399
		[Header("Settings")]
		public string RouteName = "Vehicle patrol route";

		// Token: 0x04001900 RID: 6400
		public Transform[] Waypoints;

		// Token: 0x04001901 RID: 6401
		public int StartWaypointIndex;
	}
}
