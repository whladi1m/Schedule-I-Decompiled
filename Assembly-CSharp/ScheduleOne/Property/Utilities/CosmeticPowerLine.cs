using System;
using System.Collections.Generic;
using EasyButtons;
using ScheduleOne.Property.Utilities.Power;
using UnityEngine;

namespace ScheduleOne.Property.Utilities
{
	// Token: 0x02000809 RID: 2057
	public class CosmeticPowerLine : MonoBehaviour
	{
		// Token: 0x0600381F RID: 14367 RVA: 0x000ED3DE File Offset: 0x000EB5DE
		[Button]
		public void Draw()
		{
			PowerLine.DrawPowerLine(this.startPoint.position, this.endPoint.position, this.segments, this.LengthFactor);
		}

		// Token: 0x040028DC RID: 10460
		public Transform startPoint;

		// Token: 0x040028DD RID: 10461
		public Transform endPoint;

		// Token: 0x040028DE RID: 10462
		public List<Transform> segments = new List<Transform>();

		// Token: 0x040028DF RID: 10463
		public float LengthFactor = 1.002f;
	}
}
