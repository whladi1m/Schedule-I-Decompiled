using System;
using ScheduleOne.PlayerScripts;

namespace ScheduleOne.Police
{
	// Token: 0x02000330 RID: 816
	public class Investigation
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x0004DB5E File Offset: 0x0004BD5E
		// (set) Token: 0x060011DF RID: 4575 RVA: 0x0004DB66 File Offset: 0x0004BD66
		public float CurrentProgress { get; protected set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x0004DB6F File Offset: 0x0004BD6F
		// (set) Token: 0x060011E1 RID: 4577 RVA: 0x0004DB77 File Offset: 0x0004BD77
		public Player Target { get; protected set; }

		// Token: 0x060011E2 RID: 4578 RVA: 0x0004DB80 File Offset: 0x0004BD80
		public Investigation(Player target)
		{
			this.Target = target;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0004DB8F File Offset: 0x0004BD8F
		public void ChangeProgress(float progress)
		{
			this.CurrentProgress += progress;
		}
	}
}
