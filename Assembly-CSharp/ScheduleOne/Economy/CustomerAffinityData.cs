using System;
using System.Collections.Generic;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Economy
{
	// Token: 0x02000653 RID: 1619
	[Serializable]
	public class CustomerAffinityData
	{
		// Token: 0x06002C5B RID: 11355 RVA: 0x000B8A50 File Offset: 0x000B6C50
		public void CopyTo(CustomerAffinityData data)
		{
			using (List<ProductTypeAffinity>.Enumerator enumerator = this.ProductAffinities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProductTypeAffinity affinity = enumerator.Current;
					if (data.ProductAffinities.Exists((ProductTypeAffinity x) => x.DrugType == affinity.DrugType))
					{
						data.ProductAffinities.Find((ProductTypeAffinity x) => x.DrugType == affinity.DrugType).Affinity = affinity.Affinity;
					}
					else
					{
						data.ProductAffinities.Add(new ProductTypeAffinity
						{
							DrugType = affinity.DrugType,
							Affinity = affinity.Affinity
						});
					}
				}
			}
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x000B8B24 File Offset: 0x000B6D24
		public float GetAffinity(EDrugType type)
		{
			ProductTypeAffinity productTypeAffinity = this.ProductAffinities.Find((ProductTypeAffinity x) => x.DrugType == type);
			if (productTypeAffinity == null)
			{
				Debug.LogWarning("No affinity data found for product type " + type.ToString());
				return 0f;
			}
			return productTypeAffinity.Affinity;
		}

		// Token: 0x04001FB4 RID: 8116
		[Header("Product Affinities - How much the customer likes each product type. -1 = hates, 0 = neutral, 1 = loves.")]
		public List<ProductTypeAffinity> ProductAffinities = new List<ProductTypeAffinity>();
	}
}
