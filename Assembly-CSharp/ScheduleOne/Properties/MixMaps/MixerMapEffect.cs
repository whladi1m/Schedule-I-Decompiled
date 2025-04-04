using System;
using UnityEngine;

namespace ScheduleOne.Properties.MixMaps
{
	// Token: 0x0200032D RID: 813
	[Serializable]
	public class MixerMapEffect
	{
		// Token: 0x060011D6 RID: 4566 RVA: 0x0004DA02 File Offset: 0x0004BC02
		public bool IsPointInEffect(Vector2 point)
		{
			return Vector2.Distance(point, this.Position) < this.Radius;
		}

		// Token: 0x04001160 RID: 4448
		public Vector2 Position;

		// Token: 0x04001161 RID: 4449
		public float Radius;

		// Token: 0x04001162 RID: 4450
		public Property Property;
	}
}
