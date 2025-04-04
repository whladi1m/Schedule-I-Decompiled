using System;
using System.Collections.Generic;
using ScheduleOne.Police;
using UnityEngine;

namespace ScheduleOne.Law
{
	// Token: 0x020005C5 RID: 1477
	public class SentryLocation : MonoBehaviour
	{
		// Token: 0x04001B60 RID: 7008
		[Header("References")]
		public List<Transform> StandPoints = new List<Transform>();

		// Token: 0x04001B61 RID: 7009
		[Header("Info")]
		public List<PoliceOfficer> AssignedOfficers = new List<PoliceOfficer>();
	}
}
