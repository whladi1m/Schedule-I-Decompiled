using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005AB RID: 1451
	[Serializable]
	public class PossessingControlledSubstances : Crime
	{
		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x00092B89 File Offset: 0x00090D89
		// (set) Token: 0x06002433 RID: 9267 RVA: 0x00092B91 File Offset: 0x00090D91
		public override string CrimeName { get; protected set; } = "Possession of controlled substances";
	}
}
