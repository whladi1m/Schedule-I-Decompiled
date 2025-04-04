using System;
using ScheduleOne.Persistence;
using ScheduleOne.Persistence.Datas;

namespace ScheduleOne.Trash
{
	// Token: 0x02000811 RID: 2065
	public class TrashBag : TrashItem
	{
		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06003892 RID: 14482 RVA: 0x000EF63C File Offset: 0x000ED83C
		// (set) Token: 0x06003893 RID: 14483 RVA: 0x000EF644 File Offset: 0x000ED844
		public TrashContent Content { get; private set; } = new TrashContent();

		// Token: 0x06003894 RID: 14484 RVA: 0x000EF64D File Offset: 0x000ED84D
		public void LoadContent(TrashContentData data)
		{
			this.Content.LoadFromData(data);
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x000EF65C File Offset: 0x000ED85C
		public override TrashItemData GetData()
		{
			return new TrashBagData(this.ID, base.GUID.ToString(), base.transform.position, base.transform.rotation, this.Content.GetData());
		}
	}
}
