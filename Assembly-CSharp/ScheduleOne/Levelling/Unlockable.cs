using System;
using UnityEngine;

namespace ScheduleOne.Levelling
{
	// Token: 0x020005A5 RID: 1445
	public class Unlockable
	{
		// Token: 0x0600241D RID: 9245 RVA: 0x0009276C File Offset: 0x0009096C
		public Unlockable(FullRank rank, string title, Sprite icon)
		{
			this.Rank = rank;
			this.Title = title;
			this.Icon = icon;
		}

		// Token: 0x04001AE9 RID: 6889
		public FullRank Rank;

		// Token: 0x04001AEA RID: 6890
		public string Title;

		// Token: 0x04001AEB RID: 6891
		public Sprite Icon;
	}
}
