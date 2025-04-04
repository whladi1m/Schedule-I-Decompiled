using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A6 RID: 934
	public class CleanerLoader : EmployeeLoader
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0005CC35 File Offset: 0x0005AE35
		public override string NPCType
		{
			get
			{
				return typeof(CleanerData).Name;
			}
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0005CC48 File Offset: 0x0005AE48
		public override void Load(string mainPath)
		{
			CleanerLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new CleanerLoader.<>c__DisplayClass3_0();
			Employee employee = base.LoadAndCreateEmployee(mainPath);
			if (employee == null)
			{
				return;
			}
			CS$<>8__locals1.cleaner = (employee as Cleaner);
			if (CS$<>8__locals1.cleaner == null)
			{
				Console.LogWarning("Failed to cast employee to Cleaner", null);
				return;
			}
			string json;
			if (base.TryLoadFile(mainPath, "Configuration", out json))
			{
				CleanerLoader.<>c__DisplayClass3_1 CS$<>8__locals2 = new CleanerLoader.<>c__DisplayClass3_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.configData = JsonUtility.FromJson<CleanerConfigurationData>(json);
				if (CS$<>8__locals2.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals2.<Load>g__LoadConfiguration|0));
				}
			}
			string json2;
			if (base.TryLoadFile(mainPath, "NPC", out json2))
			{
				CleanerLoader.<>c__DisplayClass3_2 CS$<>8__locals3 = new CleanerLoader.<>c__DisplayClass3_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals3.data = null;
				try
				{
					CS$<>8__locals3.data = JsonUtility.FromJson<CleanerData>(json2);
				}
				catch (Exception ex)
				{
					Type type = base.GetType();
					string str = (type != null) ? type.ToString() : null;
					string str2 = " error reading data: ";
					Exception ex2 = ex;
					Console.LogError(str + str2 + ((ex2 != null) ? ex2.ToString() : null), null);
				}
				if (CS$<>8__locals3.data == null)
				{
					Console.LogWarning("Failed to load cleaner data", null);
					return;
				}
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals3.<Load>g__LoadConfiguration|1));
			}
		}
	}
}
