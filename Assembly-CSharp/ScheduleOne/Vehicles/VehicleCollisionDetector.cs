using System;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007BB RID: 1979
	public class VehicleCollisionDetector : MonoBehaviour
	{
		// Token: 0x06003630 RID: 13872 RVA: 0x000E4492 File Offset: 0x000E2692
		public void OnCollisionEnter(Collision collision)
		{
			if (this.onCollisionEnter != null)
			{
				this.onCollisionEnter.Invoke(collision);
			}
		}

		// Token: 0x04002705 RID: 9989
		public UnityEvent<Collision> onCollisionEnter;
	}
}
