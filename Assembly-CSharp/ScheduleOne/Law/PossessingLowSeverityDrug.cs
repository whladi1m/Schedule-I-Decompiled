using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005AC RID: 1452
	[Serializable]
	public class PossessingLowSeverityDrug : Crime
	{
		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x00092BAD File Offset: 0x00090DAD
		// (set) Token: 0x06002436 RID: 9270 RVA: 0x00092BB5 File Offset: 0x00090DB5
		public override string CrimeName { get; protected set; } = "Possession of low-severity drug";
	}
}
