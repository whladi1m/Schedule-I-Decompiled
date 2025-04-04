using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B2 RID: 1458
	[Serializable]
	public class FailureToComply : Crime
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x00092C85 File Offset: 0x00090E85
		// (set) Token: 0x06002448 RID: 9288 RVA: 0x00092C8D File Offset: 0x00090E8D
		public override string CrimeName { get; protected set; } = "Failure to comply with police instruction";
	}
}
