using System;
using UnityEngine;

namespace ScheduleOne.Vehicles
{
	// Token: 0x020007B9 RID: 1977
	public class VehicleAxle : MonoBehaviour
	{
		// Token: 0x06003622 RID: 13858 RVA: 0x000E3C75 File Offset: 0x000E1E75
		protected virtual void Awake()
		{
			this.model = base.transform.Find("Model");
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x000E3C90 File Offset: 0x000E1E90
		protected virtual void LateUpdate()
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = this.wheel.axleConnectionPoint.position;
			this.model.transform.position = (position + position2) / 2f;
			base.transform.LookAt(position2);
			this.model.transform.localScale = new Vector3(this.model.transform.localScale.x, 0.5f * Vector3.Distance(position, position2), this.model.transform.localScale.z);
		}

		// Token: 0x040026EF RID: 9967
		[Header("References")]
		[SerializeField]
		protected Wheel wheel;

		// Token: 0x040026F0 RID: 9968
		private Transform model;
	}
}
