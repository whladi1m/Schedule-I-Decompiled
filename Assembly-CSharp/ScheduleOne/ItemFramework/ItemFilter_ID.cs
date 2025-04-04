using System;
using System.Collections.Generic;

namespace ScheduleOne.ItemFramework
{
	// Token: 0x02000925 RID: 2341
	public class ItemFilter_ID : ItemFilter
	{
		// Token: 0x06003F80 RID: 16256 RVA: 0x0010B7E6 File Offset: 0x001099E6
		public ItemFilter_ID(List<string> acceptedIDs)
		{
			this.AcceptedIDs = acceptedIDs;
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0010B800 File Offset: 0x00109A00
		public override bool DoesItemMatchFilter(ItemInstance instance)
		{
			return instance != null && this.AcceptedIDs.Contains(instance.ID) && base.DoesItemMatchFilter(instance);
		}

		// Token: 0x04002DC4 RID: 11716
		public List<string> AcceptedIDs = new List<string>();
	}
}
