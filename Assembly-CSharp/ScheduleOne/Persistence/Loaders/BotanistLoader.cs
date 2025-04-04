using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Employees;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x0200039E RID: 926
	public class BotanistLoader : EmployeeLoader
	{
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0005C810 File Offset: 0x0005AA10
		public override string NPCType
		{
			get
			{
				return typeof(BotanistData).Name;
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0005C82C File Offset: 0x0005AA2C
		public override void Load(string mainPath)
		{
			BotanistLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new BotanistLoader.<>c__DisplayClass3_0();
			Employee employee = base.LoadAndCreateEmployee(mainPath);
			if (employee == null)
			{
				return;
			}
			CS$<>8__locals1.botanist = (employee as Botanist);
			if (CS$<>8__locals1.botanist == null)
			{
				Console.LogWarning("Failed to cast employee to botanist", null);
				return;
			}
			string json;
			if (base.TryLoadFile(mainPath, "Configuration", out json))
			{
				BotanistLoader.<>c__DisplayClass3_1 CS$<>8__locals2 = new BotanistLoader.<>c__DisplayClass3_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.configData = JsonUtility.FromJson<BotanistConfigurationData>(json);
				if (CS$<>8__locals2.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals2.<Load>g__LoadConfiguration|0));
				}
			}
			string json2;
			if (base.TryLoadFile(mainPath, "NPC", out json2))
			{
				BotanistLoader.<>c__DisplayClass3_2 CS$<>8__locals3 = new BotanistLoader.<>c__DisplayClass3_2();
				CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals3.data = null;
				try
				{
					CS$<>8__locals3.data = JsonUtility.FromJson<BotanistData>(json2);
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
					Console.LogWarning("Failed to load botanist data", null);
					return;
				}
				Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals3.<Load>g__LoadConfiguration|1));
			}
		}
	}
}
