using System;
using ScheduleOne.NPCs.Relation;

namespace ScheduleOne.Quests
{
	// Token: 0x020002E7 RID: 743
	public class Quest_Connections : Quest
	{
		// Token: 0x060010A3 RID: 4259 RVA: 0x0004A9AC File Offset: 0x00048BAC
		public override void Begin(bool network = true)
		{
			base.Begin(network);
			foreach (QuestEntry questEntry in this.Entries)
			{
				if (questEntry.GetComponent<NPCUnlockTracker>().Npc.RelationData.Unlocked)
				{
					questEntry.SetState(EQuestState.Completed, true);
				}
				else
				{
					questEntry.SetState(EQuestState.Active, true);
				}
			}
		}
	}
}
