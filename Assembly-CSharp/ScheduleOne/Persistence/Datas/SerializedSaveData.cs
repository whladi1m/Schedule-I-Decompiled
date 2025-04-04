using System;
using UnityEngine;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000428 RID: 1064
	[Serializable]
	public class SerializedSaveData
	{
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x0005FAEE File Offset: 0x0005DCEE
		public string Version
		{
			get
			{
				return Application.version;
			}
		}

		// Token: 0x04001452 RID: 5202
		[NonSerialized]
		public static string _DataType;

		// Token: 0x04001453 RID: 5203
		public string DataType = SerializedSaveData._DataType;

		// Token: 0x04001454 RID: 5204
		[NonSerialized]
		public static int _DataVersion;

		// Token: 0x04001455 RID: 5205
		public int DataVersion = SerializedSaveData._DataVersion;
	}
}
