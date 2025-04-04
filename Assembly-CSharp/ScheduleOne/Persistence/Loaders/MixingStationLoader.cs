using System;
using System.IO;
using ScheduleOne.DevUtilities;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;
using UnityEngine;
using UnityEngine.Events;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003BD RID: 957
	public class MixingStationLoader : GridItemLoader
	{
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x0005E0C9 File Offset: 0x0005C2C9
		public override string ItemType
		{
			get
			{
				return typeof(MixingStationData).Name;
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0005E0DC File Offset: 0x0005C2DC
		public override void Load(string mainPath)
		{
			MixingStationLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new MixingStationLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as MixingStation);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to mixing station", null);
				return;
			}
			MixingStationData data = base.GetData<MixingStationData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load mixing station data", null);
				return;
			}
			ItemInstance instance = ItemDeserializer.LoadItem(data.ProductContents.Items[0]);
			CS$<>8__locals1.station.ProductSlot.SetStoredItem(instance, false);
			ItemInstance instance2 = ItemDeserializer.LoadItem(data.MixerContents.Items[0]);
			CS$<>8__locals1.station.MixerSlot.SetStoredItem(instance2, false);
			ItemInstance instance3 = ItemDeserializer.LoadItem(data.OutputContents.Items[0]);
			CS$<>8__locals1.station.OutputSlot.SetStoredItem(instance3, false);
			if (data.CurrentMixOperation != null)
			{
				CS$<>8__locals1.station.SetMixOperation(null, data.CurrentMixOperation, data.CurrentMixTime);
				if (data.CurrentMixTime >= CS$<>8__locals1.station.GetMixTimeForCurrentOperation())
				{
					CS$<>8__locals1.station.MixingDone_Networked();
				}
			}
			string json;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out json))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<MixingStationConfigurationData>(json);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}
	}
}
