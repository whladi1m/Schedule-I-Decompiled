using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B4 RID: 1460
	[Serializable]
	public class ViolatingCurfew : Crime
	{
		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x00092CCD File Offset: 0x00090ECD
		// (set) Token: 0x0600244E RID: 9294 RVA: 0x00092CD5 File Offset: 0x00090ED5
		public override string CrimeName { get; protected set; } = "Violating curfew";
	}
}
