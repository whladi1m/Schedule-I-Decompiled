using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles.AI
{
	// Token: 0x020007D4 RID: 2004
	[RequireComponent(typeof(BoxCollider))]
	public class FunnelZone : MonoBehaviour
	{
		// Token: 0x060036AF RID: 13999 RVA: 0x000E6214 File Offset: 0x000E4414
		protected virtual void Awake()
		{
			FunnelZone.funnelZones.Add(this);
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x000E6224 File Offset: 0x000E4424
		public static FunnelZone GetFunnelZone(Vector3 point)
		{
			for (int i = 0; i < FunnelZone.funnelZones.Count; i++)
			{
				if (FunnelZone.funnelZones[i].col.bounds.Contains(point))
				{
					return FunnelZone.funnelZones[i];
				}
			}
			return null;
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x000E6274 File Offset: 0x000E4474
		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(0.5f, 0.5f, 1f, 0.5f);
			Gizmos.DrawCube(base.transform.TransformPoint(this.col.center), new Vector3(this.col.size.x, this.col.size.y, this.col.size.z));
			Gizmos.DrawLine(base.transform.position, this.entryPoint.position);
		}

		// Token: 0x040027A7 RID: 10151
		public static List<FunnelZone> funnelZones = new List<FunnelZone>();

		// Token: 0x040027A8 RID: 10152
		public BoxCollider col;

		// Token: 0x040027A9 RID: 10153
		public Transform entryPoint;
	}
}
