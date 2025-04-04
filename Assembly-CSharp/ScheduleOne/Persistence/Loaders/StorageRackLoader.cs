using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003C5 RID: 965
	public class StorageRackLoader : GridItemLoader
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0005E85D File Offset: 0x0005CA5D
		public override string ItemType
		{
			get
			{
				return typeof(PlaceableStorageData).Name;
			}
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x0005E870 File Offset: 0x0005CA70
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			PlaceableStorageEntity placeableStorageEntity = gridItem as PlaceableStorageEntity;
			if (placeableStorageEntity == null)
			{
				Console.LogWarning("Failed to cast grid item to rack", null);
				return;
			}
			PlaceableStorageData data = base.GetData<PlaceableStorageData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load storage rack data", null);
				return;
			}
			for (int i = 0; i < data.Contents.Items.Length; i++)
			{
				ItemInstance instance = ItemDeserializer.LoadItem(data.Contents.Items[i]);
				if (placeableStorageEntity.StorageEntity.ItemSlots.Count > i)
				{
					placeableStorageEntity.StorageEntity.ItemSlots[i].SetStoredItem(instance, false);
				}
			}
		}
	}
}
