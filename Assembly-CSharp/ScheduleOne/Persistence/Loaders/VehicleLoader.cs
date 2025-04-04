using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Vehicles;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039B RID: 923
	public class VehicleLoader : Loader
	{
		// Token: 0x060014A2 RID: 5282 RVA: 0x0005C6CC File Offset: 0x0005A8CC
		public override void Load(string mainPath)
		{
			string json;
			if (base.TryLoadFile(mainPath, "Vehicle", out json))
			{
				VehicleData vehicleData = null;
				try
				{
					vehicleData = JsonUtility.FromJson<VehicleData>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (vehicleData != null)
				{
					NetworkSingleton<VehicleManager>.Instance.SpawnAndLoadVehicle(vehicleData, mainPath, true);
				}
			}
		}
	}
}
