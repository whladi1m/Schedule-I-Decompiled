using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005AE RID: 1454
	[Serializable]
	public class PossessingHighSeverityDrug : Crime
	{
		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x00092BF5 File Offset: 0x00090DF5
		// (set) Token: 0x0600243C RID: 9276 RVA: 0x00092BFD File Offset: 0x00090DFD
		public override string CrimeName { get; protected set; } = "Possession of high-severity drug";
	}
}
