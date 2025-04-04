using System;
using UnityEngine;

namespace ScheduleOne.Product
{
	// Token: 0x020008C9 RID: 2249
	public static class PropertyMethods
	{
		// Token: 0x06003CDD RID: 15581 RVA: 0x000FF840 File Offset: 0x000FDA40
		public static string GetName(this EProperty property)
		{
			return PropertyUtility.GetPropertyData(property).Name;
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x000FF84D File Offset: 0x000FDA4D
		public static string GetDescription(this EProperty property)
		{
			return PropertyUtility.GetPropertyData(property).Description;
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x000FF85A File Offset: 0x000FDA5A
		public static Color GetColor(this EProperty property)
		{
			return PropertyUtility.GetPropertyData(property).Color;
		}
	}
}
