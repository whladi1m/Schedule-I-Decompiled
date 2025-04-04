using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B9 RID: 1465
	[Serializable]
	public class Theft : Crime
	{
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x00092D81 File Offset: 0x00090F81
		// (set) Token: 0x0600245D RID: 9309 RVA: 0x00092D89 File Offset: 0x00090F89
		public override string CrimeName { get; protected set; } = "Theft";
	}
}
