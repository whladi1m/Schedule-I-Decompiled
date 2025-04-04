using System;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x02000665 RID: 1637
	public class CustomerSatisfaction
	{
		// Token: 0x06002D2A RID: 11562 RVA: 0x000BCFBC File Offset: 0x000BB1BC
		public static float GetRelationshipChange(float satisfaction)
		{
			return Mathf.Lerp(-0.5f, 0.5f, satisfaction);
		}
	}
}
