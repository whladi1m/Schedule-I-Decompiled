using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E2 RID: 994
	[Serializable]
	public class QualityItemData : ItemData
	{
		// Token: 0x06001540 RID: 5440 RVA: 0x0005F08A File Offset: 0x0005D28A
		public QualityItemData(string iD, int quantity, string quality) : base(iD, quantity)
		{
			this.Quality = quality;
		}

		// Token: 0x04001394 RID: 5012
		public string Quality;
	}
}
