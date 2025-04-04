using System;
using System.Collections.Generic;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.NPCs.Behaviour
{
	// Token: 0x02000512 RID: 1298
	public class PatrolGroup
	{
		// Token: 0x06001F33 RID: 7987 RVA: 0x0007FBF5 File Offset: 0x0007DDF5
		public PatrolGroup(FootPatrolRoute route)
		{
			this.Route = route;
			this.CurrentWaypoint = route.StartWaypointIndex;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0007FC1C File Offset: 0x0007DE1C
		public Vector3 GetDestination(NPC member)
		{
			if (!this.Members.Contains(member))
			{
				Console.LogWarning(member.name + " is not a member of this patrol group!", null);
				return member.transform.position;
			}
			return this.Route.Waypoints[this.CurrentWaypoint].TransformPoint(this.GetMemberOffset(member));
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0007FC78 File Offset: 0x0007DE78
		public void DisbandGroup()
		{
			foreach (NPC npc in new List<NPC>(this.Members))
			{
				(npc as PoliceOfficer).FootPatrolBehaviour.Disable_Networked(null);
				(npc as PoliceOfficer).FootPatrolBehaviour.End_Networked(null);
			}
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0007FCEC File Offset: 0x0007DEEC
		public void AdvanceGroup()
		{
			this.CurrentWaypoint++;
			if (this.CurrentWaypoint == this.Route.Waypoints.Length)
			{
				this.CurrentWaypoint = 0;
			}
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x0007FD18 File Offset: 0x0007DF18
		private Vector3 GetMemberOffset(NPC member)
		{
			if (!this.Members.Contains(member))
			{
				Console.LogWarning(member.name + " is not a member of this patrol group!", null);
				return Vector3.zero;
			}
			int num = this.Members.IndexOf(member);
			Vector3 zero = Vector3.zero;
			zero.z -= (float)num * 1f;
			zero.x += ((num % 2 == 0) ? 0.6f : -0.6f);
			return zero;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0007FD94 File Offset: 0x0007DF94
		public bool IsGroupReadyToAdvance()
		{
			for (int i = 0; i < this.Members.Count; i++)
			{
				if (!(this.Members[i] as PoliceOfficer).FootPatrolBehaviour.IsReadyToAdvance())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0007FDD8 File Offset: 0x0007DFD8
		public bool IsPaused()
		{
			for (int i = 0; i < this.Members.Count; i++)
			{
				if (this.Members[i].behaviour.activeBehaviour == null || this.Members[i].behaviour.activeBehaviour.GetType() != typeof(FootPatrolBehaviour))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001865 RID: 6245
		public List<NPC> Members = new List<NPC>();

		// Token: 0x04001866 RID: 6246
		public FootPatrolRoute Route;

		// Token: 0x04001867 RID: 6247
		public int CurrentWaypoint;
	}
}
