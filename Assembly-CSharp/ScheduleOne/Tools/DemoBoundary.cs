using System;
using ScheduleOne.PlayerScripts;
using UnityEngine;

namespace ScheduleOne.Tools
{
	// Token: 0x02000837 RID: 2103
	public class DemoBoundary : MonoBehaviour
	{
		// Token: 0x060039BE RID: 14782 RVA: 0x000F405B File Offset: 0x000F225B
		private void OnValidate()
		{
			if (this.Collider == null)
			{
				this.Collider = base.GetComponent<Collider>();
			}
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x000F4077 File Offset: 0x000F2277
		private void Start()
		{
			base.InvokeRepeating("UpdateBoundary", 0f, 0.25f);
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x000F4090 File Offset: 0x000F2290
		private void UpdateBoundary()
		{
			if (Player.Local == null)
			{
				return;
			}
			Vector3 vector = this.Collider.transform.InverseTransformPoint(Player.Local.transform.position);
			this.Collider.enabled = (vector.x > 0f);
		}

		// Token: 0x040029B9 RID: 10681
		public Collider Collider;
	}
}
