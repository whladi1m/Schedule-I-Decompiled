using System;

namespace ScheduleOne.Persistence.Datas
{
	// Token: 0x02000402 RID: 1026
	public class NPCHealthData : SaveData
	{
		// Token: 0x06001562 RID: 5474 RVA: 0x0005F370 File Offset: 0x0005D570
		public NPCHealthData(float health, bool isDead, int daysPassedSinceDeath)
		{
			this.Health = health;
			this.IsDead = isDead;
			this.DaysPassedSinceDeath = daysPassedSinceDeath;
		}

		// Token: 0x040013D1 RID: 5073
		public float Health;

		// Token: 0x040013D2 RID: 5074
		public bool IsDead;

		// Token: 0x040013D3 RID: 5075
		public int DaysPassedSinceDeath;
	}
}
