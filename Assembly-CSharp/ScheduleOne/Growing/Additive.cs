using System;
using UnityEngine;

namespace ScheduleOne.Growing
{
	// Token: 0x02000865 RID: 2149
	public class Additive : MonoBehaviour
	{
		// Token: 0x04002A39 RID: 10809
		public string AdditiveName = "Name";

		// Token: 0x04002A3A RID: 10810
		public string AssetPath;

		// Token: 0x04002A3B RID: 10811
		[Header("Plant effector settings")]
		public float QualityChange;

		// Token: 0x04002A3C RID: 10812
		public float YieldChange;

		// Token: 0x04002A3D RID: 10813
		public float GrowSpeedMultiplier = 1f;

		// Token: 0x04002A3E RID: 10814
		public float InstantGrowth;
	}
}
