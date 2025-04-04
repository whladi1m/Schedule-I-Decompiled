using System;
using UnityEngine;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006D3 RID: 1747
	public class MapHeightSampler
	{
		// Token: 0x06002F8C RID: 12172 RVA: 0x000C6354 File Offset: 0x000C4554
		public static bool Sample(float x, out float y, float z)
		{
			y = 0f;
			Vector3 vector = new Vector3(x, MapHeightSampler.SampleHeight, z);
			Debug.DrawRay(vector, Vector3.down * MapHeightSampler.SampleDistance, Color.red, 100f);
			RaycastHit raycastHit;
			if (Physics.Raycast(vector, Vector3.down, out raycastHit, MapHeightSampler.SampleDistance, 1 << LayerMask.NameToLayer("Default"), QueryTriggerInteraction.Ignore))
			{
				y = raycastHit.point.y;
			}
			return false;
		}

		// Token: 0x040021F0 RID: 8688
		private static float SampleHeight = 100f;

		// Token: 0x040021F1 RID: 8689
		private static float SampleDistance = 200f;

		// Token: 0x040021F2 RID: 8690
		public static Vector3 ResetPosition = new Vector3(-166.5f, 3f, -60f);
	}
}
