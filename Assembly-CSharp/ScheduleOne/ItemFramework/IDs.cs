using System;
using System.Collections.Generic;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000920 RID: 2336
	public class IDs : ItemFilter
	{
		// Token: 0x06003F73 RID: 16243 RVA: 0x0010B691 File Offset: 0x00109891
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			return this.AcceptedIDs.Contains(instance.ID) && base.DoesItemMatchFilter(instance);
		}

		// Token: 0x04002DC1 RID: 11713
		public List<string> AcceptedIDs = new List<string>();
	}
}
