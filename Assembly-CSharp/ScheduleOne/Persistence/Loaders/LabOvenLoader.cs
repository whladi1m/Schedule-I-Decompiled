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
	// Token: 0x020003BB RID: 955
	public class LabOvenLoader : GridItemLoader
	{
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0005DEDC File Offset: 0x0005C0DC
		public override string ItemType
		{
			get
			{
				return typeof(LabOvenData).Name;
			}
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0005DEF0 File Offset: 0x0005C0F0
		public override void Load(string mainPath)
		{
			LabOvenLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new LabOvenLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as LabOven);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to lab oven", null);
				return;
			}
			LabOvenData data = base.GetData<LabOvenData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load lab oven data", null);
				return;
			}
			for (int i = 0; i < data.InputContents.Items.Length; i++)
			{
				ItemInstance instance = ItemDeserializer.LoadItem(data.InputContents.Items[i]);
				if (CS$<>8__locals1.station.ItemSlots.Count > i)
				{
					CS$<>8__locals1.station.ItemSlots[i].SetStoredItem(instance, false);
				}
			}
			ItemInstance instance2 = ItemDeserializer.LoadItem(data.OutputContents.Items[0]);
			CS$<>8__locals1.station.OutputSlot.SetStoredItem(instance2, false);
			if (data.CurrentIngredientID != string.Empty)
			{
				OvenCookOperation operation = new OvenCookOperation(data.CurrentIngredientID, data.CurrentIngredientQuality, data.CurrentIngredientQuantity, data.CurrentProductID, data.CurrentCookProgress);
				CS$<>8__locals1.station.SetCookOperation(null, operation, false);
			}
			string json;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out json))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<LabOvenConfigurationData>(json);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}
	}
}
