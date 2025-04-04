using System;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x02000652 RID: 1618
	[Serializable]
	public class ProductTypeAffinity
	{
		// Token: 0x04001FB2 RID: 8114
		public EDrugType DrugType;

		// Token: 0x04001FB3 RID: 8115
		[Range(-1f, 1f)]
		public float Affinity;
	}
}
