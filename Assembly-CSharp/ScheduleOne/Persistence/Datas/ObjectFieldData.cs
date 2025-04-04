using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F6 RID: 1014
	[Serializable]
	public class ObjectFieldData
	{
		// Token: 0x06001555 RID: 5461 RVA: 0x0005F209 File Offset: 0x0005D409
		public ObjectFieldData(string objectGUID)
		{
			this.ObjectGUID = objectGUID;
		}

		// Token: 0x040013B4 RID: 5044
		public string ObjectGUID;
	}
}
