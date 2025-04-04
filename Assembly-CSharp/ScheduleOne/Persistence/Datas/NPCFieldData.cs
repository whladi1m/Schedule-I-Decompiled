using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003F4 RID: 1012
	[Serializable]
	public class NPCFieldData
	{
		// Token: 0x06001553 RID: 5459 RVA: 0x0005F1EB File Offset: 0x0005D3EB
		public NPCFieldData(string npcGuid)
		{
			this.NPCGuid = npcGuid;
		}

		// Token: 0x040013B2 RID: 5042
		public string NPCGuid;
	}
}
