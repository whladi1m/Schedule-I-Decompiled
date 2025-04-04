using System;
using ScheduleOne.DevUtilities;
using ScheduleOne.Variables;
using UnityEngine;

namespace ScheduleOne.Quests
{
	// Token: 0x020002F0 RID: 752
	public class Quest_OnTheGrind : Quest
	{
		// Token: 0x060010BE RID: 4286 RVA: 0x0004AF14 File Offset: 0x00049114
		protected override void MinPass()
		{
			base.MinPass();
			int num = Mathf.RoundToInt(NetworkSingleton<VariableDatabase>.Instance.GetValue<float>("Completed_Contracts_Count"));
			if (this.CompleteDealsEntry.State == EQuestState.Active)
			{
				this.CompleteDealsEntry.SetEntryTitle("Complete 3 deals (" + num.ToString() + "/3)");
			}
		}

		// Token: 0x040010EF RID: 4335
		public QuestEntry CompleteDealsEntry;
	}
}
