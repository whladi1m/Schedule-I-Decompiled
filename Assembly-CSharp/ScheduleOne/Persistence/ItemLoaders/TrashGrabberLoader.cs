using System;
using ScheduleOne.ItemFramework;
using ScheduleOne.ObjectScripts.WateringCan;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Persistence.ItemLoaders
{
	// Token: 0x0200043C RID: 1084
	public class TrashGrabberLoader : ItemLoader
	{
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060015B5 RID: 5557 RVA: 0x000602D5 File Offset: 0x0005E4D5
		public override string ItemType
		{
			get
			{
				return typeof(TrashGrabberData).Name;
			}
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x000602E8 File Offset: 0x0005E4E8
		public override ItemInstance LoadItem(string itemString)
		{
			TrashGrabberData trashGrabberData = base.LoadData<TrashGrabberData>(itemString);
			if (trashGrabberData == null)
			{
				Console.LogWarning("Failed loading item data from " + itemString, null);
				return null;
			}
			if (trashGrabberData.ID == string.Empty)
			{
				return null;
			}
			ItemDefinition item = Registry.GetItem(trashGrabberData.ID);
			if (item == null)
			{
				Console.LogWarning("Failed to find item definition for " + trashGrabberData.ID, null);
				return null;
			}
			TrashGrabberInstance trashGrabberInstance = new TrashGrabberInstance(item, trashGrabberData.Quantity);
			trashGrabberInstance.LoadContentData(trashGrabberData.Content);
			return trashGrabberInstance;
		}
	}
}
