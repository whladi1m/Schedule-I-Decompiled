using System;
using ScheduleOne.NPCs.Relation;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x020003E3 RID: 995
	[Serializable]
	public class RelationshipData : SaveData
	{
		// Token: 0x06001541 RID: 5441 RVA: 0x0005F09B File Offset: 0x0005D29B
		public RelationshipData(float relationDelta, bool unlocked, NPCRelationData.EUnlockType unlockType)
		{
			this.RelationDelta = relationDelta;
			this.Unlocked = unlocked;
			this.UnlockType = unlockType;
		}

		// Token: 0x04001395 RID: 5013
		public float RelationDelta;

		// Token: 0x04001396 RID: 5014
		public bool Unlocked;

		// Token: 0x04001397 RID: 5015
		public NPCRelationData.EUnlockType UnlockType;
	}
}
