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
	// Token: 0x020003B1 RID: 945
	public class BrickPressLoader : GridItemLoader
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0005D583 File Offset: 0x0005B783
		public override string ItemType
		{
			get
			{
				return typeof(BrickPressData).Name;
			}
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0005D59C File Offset: 0x0005B79C
		public override void Load(string mainPath)
		{
			BrickPressLoader.<>c__DisplayClass3_0 CS$<>8__locals1 = new BrickPressLoader.<>c__DisplayClass3_0();
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			CS$<>8__locals1.brickPress = (gridItem as BrickPress);
			if (CS$<>8__locals1.brickPress == null)
			{
				Console.LogWarning("Failed to cast grid item to brick press", null);
				return;
			}
			BrickPressData data = base.GetData<BrickPressData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load brick press data", null);
				return;
			}
			for (int i = 0; i < data.Contents.Items.Length; i++)
			{
				ItemInstance instance = ItemDeserializer.LoadItem(data.Contents.Items[i]);
				if (CS$<>8__locals1.brickPress.ItemSlots.Count > i)
				{
					CS$<>8__locals1.brickPress.ItemSlots[i].SetStoredItem(instance, false);
				}
			}
			string json;
			if (File.Exists(Path.Combine(mainPath, "Configuration.json")) && base.TryLoadFile(mainPath, "Configuration", out json))
			{
				CS$<>8__locals1.configData = JsonUtility.FromJson<BrickPressConfigurationData>(json);
				if (CS$<>8__locals1.configData != null)
				{
					Singleton<LoadManager>.Instance.onLoadComplete.AddListener(new UnityAction(CS$<>8__locals1.<Load>g__LoadConfiguration|0));
				}
			}
		}
	}
}
