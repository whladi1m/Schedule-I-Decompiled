using System;

namespace ScheduleOne.DevUtilities
{
	// Token: 0x020006E7 RID: 1767
	[Serializable]
	public class StringIntPair
	{
		// Token: 0x0600300B RID: 12299 RVA: 0x000C825C File Offset: 0x000C645C
		public StringIntPair(string str, int i)
		{
			this.String = str;
			this.Int = i;
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x0000494F File Offset: 0x00002B4F
		public StringIntPair()
		{
		}

		// Token: 0x0400224B RID: 8779
		public string String;

		// Token: 0x0400224C RID: 8780
		public int Int;
	}
}
