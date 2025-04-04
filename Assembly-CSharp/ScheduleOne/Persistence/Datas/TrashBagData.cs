using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042C RID: 1068
	public class TrashBagData : TrashItemData
	{
		// Token: 0x06001593 RID: 5523 RVA: 0x0005FBB1 File Offset: 0x0005DDB1
		public TrashBagData(string trashID, string guid, Vector3 position, Quaternion rotation, TrashContentData contents) : base(trashID, guid, position, rotation)
		{
			this.Contents = contents;
		}
	}
}
