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
	// Token: 0x020003B4 RID: 948
	public class CauldronLoader : GridItemLoader
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0005D7F0 File Offset: 0x0005B9F0
		public override string ItemType
		{
			get
			{
				return typeof(CauldronData).Name;
			}
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x0005D804 File Offset: 0x0005BA04
		public override void Load(string mainPath)
		{
			CauldronLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new CauldronLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.station = (gridItem as Cauldron);
			if (CS$<>8__locals1.station == null)
			{
				Console.LogWarning("Failed to cast grid item to Cauldron", null);
				return;
			}
			CauldronData data = base.GetData<CauldronData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load cauldron data", null);
				return;
			}
			for (int i = 0; i < data.Ingredients.Items.Length; i++)
			{
				ItemInstance instance = ItemDeserializer.LoadItem(data.Ingredients.Items[i]);
				if (CS$<>8__locals1.station.IngredientSlots.Length > i)
				{
					CS$<>8__locals1.station.IngredientSlots[i].SetStoredItem(instance, false);
				}
			}
			ItemInstance instance2 = ItemDeserializer.LoadItem(data.Liquid.Items[0]);
			CS$<>8__locals1.station.LiquidSlot.SetStoredItem(instance2, false);
			ItemInstance instance3 = ItemDeserializer.LoadItem(data.Output.Items[0]);
			CS$<>8__locals1.station.OutputSlot.SetStoredItem(instance3, false);
			if (data.RemainingCookTime > 0)
			{
				CS$<>8__locals1.station.StartCookOperation(null, data.RemainingCookTime, data.InputQuality);
			}
			string json;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out json))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<CauldronConfigurationData>(json);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}
	}
}
