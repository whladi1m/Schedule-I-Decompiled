using System;
using Pathfinding;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006C1 RID: 1729
	public class AstarUtility : MonoBehaviour
	{
		// Token: 0x06002F2D RID: 12077 RVA: 0x000C4C90 File Offset: 0x000C2E90
		public static Vector3 GetClosestPointOnGraph(Vector3 point, string GraphName)
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName(GraphName);
			return AstarPath.active.GetNearest(point, nnconstraint).position;
		}
	}
}
