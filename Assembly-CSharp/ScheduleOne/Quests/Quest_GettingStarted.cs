using System;
using ScheduleOne.Economy;
using ScheduleOne.NPCs.CharacterClasses;

namespace ScheduleOne.Quests
{
	// Token: 0x020002ED RID: 749
	public class Quest_GettingStarted : Quest
	{
		// Token: 0x060010B7 RID: 4279 RVA: 0x0004ADC3 File Offset: 0x00048FC3
		protected override void MinPass()
		{
			base.MinPass();
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0004ADCB File Offset: 0x00048FCB
		public override void SetQuestState(EQuestState state, bool network = true)
		{
			base.SetQuestState(state, network);
		}

		// Token: 0x040010E8 RID: 4328
		public float CashAmount = 375f;

		// Token: 0x040010E9 RID: 4329
		public DeadDrop CashDrop;

		// Token: 0x040010EA RID: 4330
		public UncleNelson Nelson;
	}
}
