using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScheduleOne.Vehicles.Recording
{
	// Token: 0x020007C8 RID: 1992
	[CreateAssetMenu(fileName = "DrivePath", menuName = "ScriptableObjects/DrivePath", order = 1)]
	[Serializable]
	public class DrivePath : ScriptableObject
	{
		// Token: 0x0400276E RID: 10094
		public int fps = 24;

		// Token: 0x0400276F RID: 10095
		public List<VehicleKeyFrame> keyframes = new List<VehicleKeyFrame>();
	}
}
