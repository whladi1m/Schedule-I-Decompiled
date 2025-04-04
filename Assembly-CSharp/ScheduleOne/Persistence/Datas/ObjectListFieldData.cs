using System;
using System.Collections.Generic;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F7 RID: 1015
	[Serializable]
	public class ObjectListFieldData
	{
		// Token: 0x06001556 RID: 5462 RVA: 0x0005F218 File Offset: 0x0005D418
		public ObjectListFieldData(List<string> objectGUIDs)
		{
			this.ObjectGUIDs = objectGUIDs;
		}

		// Token: 0x040013B5 RID: 5045
		public List<string> ObjectGUIDs;
	}
}
