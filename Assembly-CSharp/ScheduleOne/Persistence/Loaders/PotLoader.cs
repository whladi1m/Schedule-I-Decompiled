using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003C1 RID: 961
	public class PotLoader : GridItemLoader
	{
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x0005E451 File Offset: 0x0005C651
		public override string ItemType
		{
			get
			{
				return typeof(PotData).Name;
			}
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0005E464 File Offset: 0x0005C664
		public override void Load(string mainPath)
		{
			PotLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new PotLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.pot = (gridItem as Pot);
			if (CS$<>8__locals1.pot == null)
			{
				Console.LogWarning("Failed to cast grid item to pot", null);
				return;
			}
			string json;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out json))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<PotConfigurationData>(json);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
			PotData data = base.GetData<PotData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load pot data", null);
				return;
			}
			if (!string.IsNullOrEmpty(data.SoilID))
			{
				CS$<>8__locals1.pot.SetSoilID(data.SoilID);
				CS$<>8__locals1.pot.AddSoil(data.SoilLevel);
				CS$<>8__locals1.pot.SetSoilUses(data.RemainingSoilUses);
			}
			CS$<>8__locals1.pot.ChangeWaterAmount(data.WaterLevel);
			for (int i = 0; i < data.AppliedAdditives.Length; i++)
			{
				CS$<>8__locals1.pot.ApplyAdditive(null, data.AppliedAdditives[i], false);
			}
			CS$<>8__locals1.pot.LoadPlant(data.PlantData);
		}
	}
}
