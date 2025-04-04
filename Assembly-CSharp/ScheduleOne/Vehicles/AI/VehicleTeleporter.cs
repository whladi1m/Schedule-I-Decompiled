using System;
using Pathfinding;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007F1 RID: 2033
	[RequireComponent(typeof(LandVehicle))]
	public class VehicleTeleporter : MonoBehaviour
	{
		// Token: 0x06003737 RID: 14135 RVA: 0x000EA5D4 File Offset: 0x000E87D4
		public void MoveToGraph(bool resetRotation = true)
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("General Vehicle Graph");
			NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, nnconstraint);
			base.transform.position = nearest.position + base.transform.up * base.GetComponent<LandVehicle>().boundingBoxDimensions.y / 2f;
			if (resetRotation)
			{
				base.transform.rotation = Quaternion.identity;
			}
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x000EA664 File Offset: 0x000E8864
		public void MoveToRoadNetwork(bool resetRotation = true)
		{
			NNConstraint nnconstraint = new NNConstraint();
			nnconstraint.graphMask = GraphMask.FromGraphName("Road Nodes");
			NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, nnconstraint);
			base.transform.position = nearest.position + base.transform.up * base.GetComponent<LandVehicle>().boundingBoxDimensions.y / 2f;
			if (resetRotation)
			{
				base.transform.rotation = Quaternion.identity;
			}
		}
	}
}
