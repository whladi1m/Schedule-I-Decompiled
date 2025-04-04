using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Property;
using UnityEngine;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x02000382 RID: 898
	public class BusinessLoader : PropertyLoader
	{
		// Token: 0x06001467 RID: 5223 RVA: 0x0005B134 File Offset: 0x00059334
		public override void Load(string mainPath)
		{
			base.Load(mainPath);
			string json;
			if (base.TryLoadFile(mainPath, "Business", out json))
			{
				BusinessData businessData = null;
				try
				{
					businessData = JsonUtility.FromJson<BusinessData>(json);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (businessData != null)
				{
					Singleton<BusinessManager>.Instance.LoadBusiness(businessData, mainPath);
				}
			}
		}
	}
}
