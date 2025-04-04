using System;

namespace ScheduleOne.Law
{
	// Token: 0x020005B8 RID: 1464
	[Serializable]
	public class Vandalism : Crime
	{
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06002459 RID: 9305 RVA: 0x00092D5D File Offset: 0x00090F5D
		// (set) Token: 0x0600245A RID: 9306 RVA: 0x00092D65 File Offset: 0x00090F65
		public override string CrimeName { get; protected set; } = "Vandalism";
	}
}
