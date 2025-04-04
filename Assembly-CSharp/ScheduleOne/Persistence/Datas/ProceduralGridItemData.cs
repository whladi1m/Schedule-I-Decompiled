using System;
using ScheduleOne.ItemFramework;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000416 RID: 1046
	[Serializable]
	public class ProceduralGridItemData : BuildableItemData
	{
		// Token: 0x06001576 RID: 5494 RVA: 0x0005F7C4 File Offset: 0x0005D9C4
		public ProceduralGridItemData(Guid guid, ItemInstance item, int loadOrder, int rotation, FootprintMatchData[] footprintMatches) : base(guid, item, loadOrder)
		{
			this.Rotation = rotation;
			this.FootprintMatches = footprintMatches;
		}

		// Token: 0x0400141B RID: 5147
		public int Rotation;

		// Token: 0x0400141C RID: 5148
		public FootprintMatchData[] FootprintMatches;
	}
}
