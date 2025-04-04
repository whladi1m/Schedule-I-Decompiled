using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003A2 RID: 930
	public class ChemistLoader : EmployeeLoader
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0005CA33 File Offset: 0x0005AC33
		public override string NPCType
		{
			get
			{
				return typeof(ChemistData).Name;
			}
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0005CA44 File Offset: 0x0005AC44
		public override void Load(string mainPath)
		{
			ChemistLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new ChemistLoader.<>c__DisplayClass3_0();
			Employee employee = base.LoadAndCreateEmployee(mainPath);
			if (employee == null)
			{
				return;
			}
			CS$<>8__locals1.chemist = (employee as Chemist);
			if (CS$<>8__locals1.chemist == null)
			{
				Console.LogWarning("Failed to cast employee to chemist", null);
				return;
			}
			string json;
			if (base.TryLoadFile(mainPath, "Configuration", out json))
			{
				ChemistLoader.<>c__DisplayClass3_1 CS$<>8__locals2 = new ChemistLoader.<>c__DisplayClass3_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.configData = JsonUtility.FromJson<ChemistConfigurationData>(json);
				if (CS$<>8__locals2.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals2.<Load>g__LoadConfiguration|0));
				}
			}
			string json2;
			if (base.TryLoadFile(mainPath, "NPC", out json2))
			{
				ChemistLoader.<>c__DisplayClass3_2 CS$<>8__locals3 = new ChemistLoader.<>c__DisplayClass3_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals3.data = null;
				try
				{
					CS$<>8__locals3.data = JsonUtility.FromJson<ChemistData>(json2);
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
					Console.LogWarning("Failed to load chemist data", null);
					return;
				}
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals3.<Load>g__LoadConfiguration|1));
			}
		}
	}
}
