using System;
using System.Collections.Generic;

namespace ScheduleOne.Management.Settings
{
	// Token: 0x02000587 RID: 1415
	[Serializable]
	public class ItemSelectionSetting
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x0600235A RID: 9050 RVA: 0x000903CA File Offset: 0x0008E5CA
		// (set) Token: 0x0600235B RID: 9051 RVA: 0x000903D2 File Offset: 0x0008E5D2
		public List<string> SelectedItems { get; protected set; } = new List<string>();
	}
}
