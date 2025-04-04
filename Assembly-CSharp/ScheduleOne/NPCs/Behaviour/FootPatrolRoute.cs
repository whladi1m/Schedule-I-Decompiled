using System;
using FluffyUnderware.DevTools.Extensions;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000511 RID: 1297
	public class FootPatrolRoute : MonoBehaviour
	{
		// Token: 0x06001F2F RID: 7983 RVA: 0x0007FA94 File Offset: 0x0007DC94
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
			Gizmos.color = this.PathColor;
			for (int j = 0; j < this.Waypoints.Length - 1; j++)
			{
				if (!(this.Waypoints[j] == null))
				{
					Gizmos.DrawLine(this.Waypoints[j].position + Vector3.up * 0.5f, this.Waypoints[j + 1].position + Vector3.up * 0.5f);
				}
			}
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x0007FBA5 File Offset: 0x0007DDA5
		private void OnValidate()
		{
			this.UpdateWaypoints();
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0007FBAD File Offset: 0x0007DDAD
		private void UpdateWaypoints()
		{
			this.Waypoints = base.transform.GetComponentsInChildren<Transform>();
			this.Waypoints = this.Waypoints.Remove(base.transform);
		}

		// Token: 0x04001861 RID: 6241
		[Header("Settings")]
		public string RouteName = "Foot patrol route";

		// Token: 0x04001862 RID: 6242
		public Color PathColor = Color.red;

		// Token: 0x04001863 RID: 6243
		public Transform[] Waypoints;

		// Token: 0x04001864 RID: 6244
		public int StartWaypointIndex;
	}
}
