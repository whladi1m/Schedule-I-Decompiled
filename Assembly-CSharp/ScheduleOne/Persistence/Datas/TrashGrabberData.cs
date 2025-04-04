using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E4 RID: 996
	[Serializable]
	public class TrashGrabberData : ItemData
	{
		// Token: 0x06001542 RID: 5442 RVA: 0x0005F0B8 File Offset: 0x0005D2B8
		public TrashGrabberData(string iD, int quantity, TrashContentData content) : base(iD, quantity)
		{
			this.Content = content;
		}

		// Token: 0x04001398 RID: 5016
		public TrashContentData Content;
	}
}
