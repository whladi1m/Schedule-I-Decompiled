using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008C6 RID: 2246
	public static class DrugTypeMethods
	{
		// Token: 0x06003CDA RID: 15578 RVA: 0x000FF826 File Offset: 0x000FDA26
		public static string GetName(this EDrugType property)
		{
			return PropertyUtility.GetDrugTypeData(property).Name;
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x000FF833 File Offset: 0x000FDA33
		public static Color GetColor(this EDrugType property)
		{
			return PropertyUtility.GetDrugTypeData(property).Color;
		}
	}
}
