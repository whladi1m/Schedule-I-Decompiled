using System;

namespace ScheduleOne.Combat
{
	// Token: 0x02000725 RID: 1829
	public interface IDamageable
	{
		// Token: 0x06003187 RID: 12679
		void SendImpact(Impact impact);

		// Token: 0x06003188 RID: 12680
		void ReceiveImpact(Impact impact);
	}
}
