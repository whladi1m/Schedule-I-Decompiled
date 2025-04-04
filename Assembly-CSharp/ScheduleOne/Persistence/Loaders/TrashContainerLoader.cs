using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.ObjectScripts;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003C7 RID: 967
	public class TrashContainerLoader : GridItemLoader
	{
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x0005E998 File Offset: 0x0005CB98
		public override string ItemType
		{
			get
			{
				return typeof(TrashContainerData).Name;
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0005E9AC File Offset: 0x0005CBAC
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			TrashContainerData data = base.GetData<TrashContainerData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load toggleableitem data", null);
				return;
			}
			TrashContainerItem trashContainerItem = gridItem as TrashContainerItem;
			if (trashContainerItem != null)
			{
				trashContainerItem.Container.Content.LoadFromData(data.ContentData);
			}
		}
	}
}
