using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039D RID: 925
	public class WeedProductLoader : Loader
	{
		// Token: 0x060014A6 RID: 5286 RVA: 0x0005C790 File Offset: 0x0005A990
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, false))
			{
				WeedProductData weedProductData = null;
				try
				{
					weedProductData = JsonUtility.FromJson<WeedProductData>(json);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading product data: " + ex.Message);
				}
				if (weedProductData == null)
				{
					return;
				}
				NetworkSingleton<ProductManager>.Instance.CreateWeed_Server(weedProductData.Name, weedProductData.ID, weedProductData.DrugType, weedProductData.Properties.ToList<string>(), weedProductData.AppearanceSettings);
			}
		}
	}
}
