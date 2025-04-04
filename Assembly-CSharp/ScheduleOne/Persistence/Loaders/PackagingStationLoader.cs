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
	// Token: 0x020003BF RID: 959
	public class PackagingStationLoader : GridItemLoader
	{
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060014F7 RID: 5367 RVA: 0x0005E2BE File Offset: 0x0005C4BE
		public override string ItemType
		{
			get
			{
				return typeof(PackagingStationData).Name;
			}
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0005E2D0 File Offset: 0x0005C4D0
		public override void Load(string mainPath)
		{
			PackagingStationLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new PackagingStationLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as PackagingStation);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to pot", null);
				return;
			}
			PackagingStationData data = base.GetData<PackagingStationData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load packaging station data data", null);
				return;
			}
			for (int i = 0; i < data.Contents.Items.Length; i++)
			{
				ItemInstance instance = ItemDeserializer.LoadItem(data.Contents.Items[i]);
				if (CS$<>8__locals1.station.ItemSlots.Count > i)
				{
					CS$<>8__locals1.station.ItemSlots[i].SetStoredItem(instance, false);
				}
			}
			CS$<>8__locals1.station.UpdatePackagingVisuals();
			CS$<>8__locals1.station.UpdateProductVisuals();
			string json;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out json))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<PackagingStationConfigurationData>(json);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}
	}
}
