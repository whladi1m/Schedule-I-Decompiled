using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles.Recording
{
	// Token: 0x020007C9 RID: 1993
	[Serializable]
	public class VehicleKeyFrame
	{
		// Token: 0x04002770 RID: 10096
		public Vector3 position;

		// Token: 0x04002771 RID: 10097
		public Quaternion rotation;

		// Token: 0x04002772 RID: 10098
		public bool brakesApplied;

		// Token: 0x04002773 RID: 10099
		public bool reversing;

		// Token: 0x04002774 RID: 10100
		public bool headlightsOn;

		// Token: 0x04002775 RID: 10101
		public List<VehicleKeyFrame.WheelTransform> wheels = new List<VehicleKeyFrame.WheelTransform>();

		// Token: 0x020007CA RID: 1994
		[Serializable]
		public class WheelTransform
		{
			// Token: 0x04002776 RID: 10102
			public float yPos;

			// Token: 0x04002777 RID: 10103
			public Quaternion rotation;
		}
	}
}
