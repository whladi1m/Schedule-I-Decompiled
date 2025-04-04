using System;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200014D RID: 333
	public static class SpotLightHelper
	{
		// Token: 0x06000647 RID: 1607 RVA: 0x0001C94C File Offset: 0x0001AB4C
		public static float GetIntensity(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.intensity;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001C963 File Offset: 0x0001AB63
		public static float GetSpotAngle(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.spotAngle;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001C97A File Offset: 0x0001AB7A
		public static float GetFallOffEnd(Light light)
		{
			if (!(light != null))
			{
				return 0f;
			}
			return light.range;
		}
	}
}
