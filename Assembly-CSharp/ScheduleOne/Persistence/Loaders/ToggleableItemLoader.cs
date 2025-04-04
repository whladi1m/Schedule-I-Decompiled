using System;
using ScheduleOne.EntityFramework;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.Loaders
{
	// Token: 0x020003C6 RID: 966
	public class ToggleableItemLoader : GridItemLoader
	{
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x0005E925 File Offset: 0x0005CB25
		public override string ItemType
		{
			get
			{
				return typeof(ToggleableItemData).Name;
			}
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0005E938 File Offset: 0x0005CB38
		public override void Load(string mainPath)
		{
			GridItem gridItem = base.LoadAndCreate(mainPath);
			if (gridItem == null)
			{
				Console.LogWarning("Failed to load grid item", null);
				return;
			}
			ToggleableItemData data = base.GetData<ToggleableItemData>(mainPath);
			if (data == null)
			{
				Console.LogWarning("Failed to load toggleableitem data", null);
				return;
			}
			ToggleableItem toggleableItem = gridItem as ToggleableItem;
			if (toggleableItem != null && data.IsOn)
			{
				toggleableItem.TurnOn(true);
			}
		}
	}
}
