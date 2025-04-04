using System;
using System.Collections.Generic;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.GameTime;
using ScheduleOne.NPCs;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007E4 RID: 2020
	public class Sensor : MonoBehaviour
	{
		// Token: 0x060036EE RID: 14062 RVA: 0x000E777E File Offset: 0x000E597E
		protected virtual void Start()
		{
			this.vehicle = base.GetComponentInParent<LandVehicle>();
			base.InvokeRepeating("Check", 0f, 0.33f);
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x000E77A4 File Offset: 0x000E59A4
		public void Check()
		{
			if (NetworkSingleton<TimeManager>.Instance.SleepInProgress)
			{
				return;
			}
			if (this.vehicle.Agent.KinematicMode)
			{
				return;
			}
			if (this.vehicle.isStatic)
			{
				return;
			}
			if (this.vehicle != null)
			{
				this.calculatedDetectionRange = Mathf.Lerp(this.minDetectionRange, this.maxDetectionRange, Mathf.Clamp01(this.vehicle.speed_Kmh / this.vehicle.TopSpeed));
			}
			else
			{
				this.calculatedDetectionRange = this.maxDetectionRange;
			}
			Vector3 origin = base.transform.position - base.transform.forward * this.checkRadius;
			this.hits = Physics.SphereCastAll(origin, this.checkRadius, base.transform.forward, this.calculatedDetectionRange, this.checkMask, QueryTriggerInteraction.Collide).ToList<RaycastHit>();
			this.hits = (from x in this.hits
			orderby Vector3.Distance(base.transform.position, x.point)
			select x).ToList<RaycastHit>();
			bool flag = false;
			for (int i = 0; i < this.hits.Count; i++)
			{
				if (!(this.vehicle != null) || !this.hits[i].collider.transform.IsChildOf(this.vehicle.transform))
				{
					VehicleObstacle componentInParent = this.hits[i].collider.transform.GetComponentInParent<VehicleObstacle>();
					LandVehicle componentInParent2 = this.hits[i].collider.transform.GetComponentInParent<LandVehicle>();
					NPC componentInParent3 = this.hits[i].collider.transform.GetComponentInParent<NPC>();
					Player componentInParent4 = this.hits[i].collider.transform.GetComponentInParent<Player>();
					if (componentInParent != null)
					{
						if (!componentInParent.twoSided && Vector3.Angle(-componentInParent.transform.forward, base.transform.forward) > 90f)
						{
							goto IL_230;
						}
					}
					else if (!(componentInParent2 != null) && !(componentInParent3 != null))
					{
						componentInParent4 != null;
					}
					flag = true;
					this.hit = this.hits[i];
					break;
				}
				IL_230:;
			}
			if (flag)
			{
				this.obstruction = this.hit.collider;
				this.obstructionDistance = Vector3.Distance(base.transform.position, this.hit.point);
				return;
			}
			this.obstruction = null;
			this.obstructionDistance = float.MaxValue;
		}

		// Token: 0x040027E3 RID: 10211
		public Collider obstruction;

		// Token: 0x040027E4 RID: 10212
		public float obstructionDistance;

		// Token: 0x040027E5 RID: 10213
		public const float checkRate = 0.33f;

		// Token: 0x040027E6 RID: 10214
		[Header("Settings")]
		[SerializeField]
		protected float minDetectionRange = 3f;

		// Token: 0x040027E7 RID: 10215
		[SerializeField]
		protected float maxDetectionRange = 12f;

		// Token: 0x040027E8 RID: 10216
		public float checkRadius = 1f;

		// Token: 0x040027E9 RID: 10217
		public LayerMask checkMask;

		// Token: 0x040027EA RID: 10218
		private LandVehicle vehicle;

		// Token: 0x040027EB RID: 10219
		[HideInInspector]
		public float calculatedDetectionRange;

		// Token: 0x040027EC RID: 10220
		private RaycastHit hit;

		// Token: 0x040027ED RID: 10221
		private List<RaycastHit> hits = new List<RaycastHit>();
	}
}
