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
	// Token: 0x020003B8 RID: 952
	public class DryingRackLoader : GridItemLoader
	{
		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0005DBE2 File Offset: 0x0005BDE2
		public override string ItemType
		{
			get
			{
				return typeof(DryingRackData).Name;
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0005DBF4 File Offset: 0x0005BDF4
		public override void Load(string mainPath)
		{
			DryingRackLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new DryingRackLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as DryingRack);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to DryingRack", null);
				return;
			}
			DryingRackData data = base.GetData<DryingRackData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load DryingRack data", null);
				return;
			}
			ItemInstance instance = ItemDeserializer.LoadItem(data.Input.Items[0]);
			CS$<>8__locals1.station.InputSlot.SetStoredItem(instance, false);
			ItemInstance instance2 = ItemDeserializer.LoadItem(data.Output.Items[0]);
			CS$<>8__locals1.station.OutputSlot.SetStoredItem(instance2, false);
			for (int i = 0; i < data.DryingOperations.Length; i++)
			{
				if (data.DryingOperations[i] != null && data.DryingOperations[i].Quantity > 0 && !string.IsNullOrEmpty(data.DryingOperations[i].ItemID))
				{
					CS$<>8__locals1.station.DryingOperations.Add(data.DryingOperations[i]);
				}
			}
			CS$<>8__locals1.station.RefreshHangingVisuals();
			string json;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out json))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<DryingRackConfigurationData>(json);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}
	}
}
