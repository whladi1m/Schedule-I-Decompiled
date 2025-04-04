using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x0200042E RID: 1070
	public class TrashGeneratorData : SaveData
	{
		// Token: 0x06001595 RID: 5525 RVA: 0x0005FBD5 File Offset: 0x0005DDD5
		public TrashGeneratorData(string guid, string[] generatedItems)
		{
			this.GUID = guid;
			this.GeneratedItems = generatedItems;
		}

		// Token: 0x04001460 RID: 5216
		public string GUID;

		// Token: 0x04001461 RID: 5217
		public string[] GeneratedItems;
	}
}
