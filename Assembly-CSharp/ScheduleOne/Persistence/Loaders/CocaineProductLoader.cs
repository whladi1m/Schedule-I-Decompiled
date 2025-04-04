using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000383 RID: 899
	public class CocaineProductLoader : Loader
	{
		// Token: 0x06001469 RID: 5225 RVA: 0x0005B1B4 File Offset: 0x000593B4
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, false))
			{
				CocaineProductData cocaineProductData = null;
				try
				{
					cocaineProductData = JsonUtility.FromJson<CocaineProductData>(json);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading product data: " + ex.Message);
				}
				if (cocaineProductData == null)
				{
					return;
				}
				NetworkSingleton<ProductManager>.Instance.CreateCocaine_Server(cocaineProductData.Name, cocaineProductData.ID, cocaineProductData.DrugType, cocaineProductData.Properties.ToList<string>(), cocaineProductData.AppearanceSettings);
			}
		}
	}
}
