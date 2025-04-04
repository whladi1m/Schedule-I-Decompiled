using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005AF RID: 1455
	[Serializable]
	public class Evading : Crime
	{
		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x00092C19 File Offset: 0x00090E19
		// (set) Token: 0x0600243F RID: 9279 RVA: 0x00092C21 File Offset: 0x00090E21
		public override string CrimeName { get; protected set; } = "Evading arrest";
	}
}
