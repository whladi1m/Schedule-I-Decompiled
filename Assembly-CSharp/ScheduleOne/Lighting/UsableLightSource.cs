using System;
using UnityEngine;

namespace ScheduleOne.Lighting
{
	// Token: 0x0200059F RID: 1439
	public class UsableLightSource : MonoBehaviour
	{
		// Token: 0x04001AC2 RID: 6850
		[Range(0.5f, 2f)]
		public float GrowSpeedMultiplier = 1f;

		// Token: 0x04001AC3 RID: 6851
		public bool isEmitting = true;
	}
}
