using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042F RID: 1071
	[Serializable]
	public class TrashItemData : SaveData
	{
		// Token: 0x06001596 RID: 5526 RVA: 0x0005FBEB File Offset: 0x0005DDEB
		public TrashItemData(string trashID, string guid, Vector3 position, Quaternion rotation)
		{
			this.TrashID = trashID;
			this.GUID = guid;
			this.Position = position;
			this.Rotation = rotation;
		}

		// Token: 0x04001462 RID: 5218
		public string TrashID;

		// Token: 0x04001463 RID: 5219
		public string GUID;

		// Token: 0x04001464 RID: 5220
		public Vector3 Position;

		// Token: 0x04001465 RID: 5221
		public Quaternion Rotation;

		// Token: 0x04001466 RID: 5222
		public TrashContentData Contents;
	}
}
