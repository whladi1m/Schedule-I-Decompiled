using System;
using System.Linq;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Product;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200038A RID: 906
	public class MethProductLoader : Loader
	{
		// Token: 0x0600147B RID: 5243 RVA: 0x0005B5D8 File Offset: 0x000597D8
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, out json, false))
			{
				MethProductData methProductData = null;
				try
				{
					methProductData = JsonUtility.FromJson<MethProductData>(json);
				}
				catch (Exception ex)
				{
					Debug.LogError("Error loading product data: " + ex.Message);
				}
				if (methProductData == null)
				{
					return;
				}
				NetworkSingleton<ProductManager>.Instance.CreateMeth_Server(methProductData.Name, methProductData.ID, methProductData.DrugType, methProductData.Properties.ToList<string>(), methProductData.AppearanceSettings);
			}
		}
	}
}
