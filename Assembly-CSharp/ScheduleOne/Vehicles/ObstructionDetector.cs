using System;
using System.Collections.Generic;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007B2 RID: 1970
	[RequireComponent(typeof(Rigidbody))]
	public class ObstructionDetector : MonoBehaviour
	{
		// Token: 0x06003605 RID: 13829 RVA: 0x000E34DB File Offset: 0x000E16DB
		protected virtual void Awake()
		{
			this.vehicle = base.gameObject.GetComponentInParent<LandVehicle>();
			this.range = base.transform.Find("Collider").transform.localScale.z;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000E3514 File Offset: 0x000E1714
		protected virtual void FixedUpdate()
		{
			this.closestObstructionDistance = float.MaxValue;
			for (int i = 0; i < this.vehicles.Count; i++)
			{
				if (Vector3.Distance(base.transform.position, this.vehicles[i].transform.position) < this.closestObstructionDistance)
				{
					this.closestObstructionDistance = Vector3.Distance(base.transform.position, this.vehicles[i].transform.position);
				}
			}
			for (int j = 0; j < this.npcs.Count; j++)
			{
				if (Vector3.Distance(base.transform.position, this.npcs[j].transform.position) < this.closestObstructionDistance)
				{
					this.closestObstructionDistance = Vector3.Distance(base.transform.position, this.npcs[j].transform.position);
				}
			}
			for (int k = 0; k < this.players.Count; k++)
			{
				if (Vector3.Distance(base.transform.position, this.players[k].transform.position) < this.closestObstructionDistance)
				{
					this.closestObstructionDistance = Vector3.Distance(base.transform.position, this.players[k].transform.position);
				}
			}
			for (int l = 0; l < this.vehicleObstacles.Count; l++)
			{
				if (Vector3.Distance(base.transform.position, this.vehicleObstacles[l].transform.position) < this.closestObstructionDistance)
				{
					this.closestObstructionDistance = Vector3.Distance(base.transform.position, this.vehicleObstacles[l].transform.position);
				}
			}
			this.vehicles.Clear();
			this.npcs.Clear();
			this.players.Clear();
			this.vehicleObstacles.Clear();
			float num = this.closestObstructionDistance;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x000E3728 File Offset: 0x000E1928
		private void OnTriggerStay(Collider other)
		{
			LandVehicle componentInParent = other.GetComponentInParent<LandVehicle>();
			NPC componentInParent2 = other.GetComponentInParent<NPC>();
			PlayerMovement componentInParent3 = other.GetComponentInParent<PlayerMovement>();
			VehicleObstacle componentInParent4 = other.GetComponentInParent<VehicleObstacle>();
			if (componentInParent != null && componentInParent != this.vehicle && !this.vehicles.Contains(componentInParent))
			{
				this.vehicles.Add(componentInParent);
			}
			if (componentInParent2 != null && !this.npcs.Contains(componentInParent2))
			{
				this.npcs.Add(componentInParent2);
			}
			if (componentInParent3 != null && !this.players.Contains(componentInParent3))
			{
				this.players.Add(componentInParent3);
			}
			if (componentInParent4 != null && (componentInParent4.twoSided || Vector3.Angle(-componentInParent4.transform.forward, base.transform.forward) < 90f) && !this.vehicleObstacles.Contains(componentInParent4))
			{
				this.vehicleObstacles.Add(componentInParent4);
			}
		}

		// Token: 0x040026D0 RID: 9936
		private LandVehicle vehicle;

		// Token: 0x040026D1 RID: 9937
		public List<LandVehicle> vehicles = new List<LandVehicle>();

		// Token: 0x040026D2 RID: 9938
		public List<NPC> npcs = new List<NPC>();

		// Token: 0x040026D3 RID: 9939
		public List<PlayerMovement> players = new List<PlayerMovement>();

		// Token: 0x040026D4 RID: 9940
		public List<VehicleObstacle> vehicleObstacles = new List<VehicleObstacle>();

		// Token: 0x040026D5 RID: 9941
		public float closestObstructionDistance;

		// Token: 0x040026D6 RID: 9942
		public float range;
	}
}
