using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Properties.MixMaps
{
	// Token: 0x0200032C RID: 812
	[Serializable]
	public class MixerMap : ScriptableObject
	{
		// Token: 0x060011D3 RID: 4563 RVA: 0x0004D960 File Offset: 0x0004BB60
		public MixerMapEffect GetEffectAtPoint(Vector2 point)
		{
			if (point.magnitude > this.MapRadius)
			{
				return null;
			}
			for (int i = 0; i < this.Effects.Count; i++)
			{
				if (this.Effects[i].IsPointInEffect(point))
				{
					return this.Effects[i];
				}
			}
			return null;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0004D9B8 File Offset: 0x0004BBB8
		public MixerMapEffect GetEffect(Property property)
		{
			for (int i = 0; i < this.Effects.Count; i++)
			{
				if (this.Effects[i].Property == property)
				{
					return this.Effects[i];
				}
			}
			return null;
		}

		// Token: 0x0400115E RID: 4446
		public float MapRadius;

		// Token: 0x0400115F RID: 4447
		public List<MixerMapEffect> Effects;
	}
}
