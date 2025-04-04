using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005AD RID: 1453
	[Serializable]
	public class PossessingModerateSeverityDrug : Crime
	{
		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x00092BD1 File Offset: 0x00090DD1
		// (set) Token: 0x06002439 RID: 9273 RVA: 0x00092BD9 File Offset: 0x00090DD9
		public override string CrimeName { get; protected set; } = "Possession of moderate-severity drug";
	}
}
